using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Linq;
using System.Linq.Dynamic;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Quest.Util.Status;
using Quest.Util.Buffer;
using Quest.Util.Data;
using Quest.Functional.ASM;
using Quest.Functional.MasterPricing;
using Quest.MPDW.Services.Data;
using Quest.MasterPricing.Services.SQL;
using Quest.MasterPricing.Services.Data.Database;


namespace Quest.MasterPricing.Services.Data.Filters
{
    public class DbFilterSQLMgr : DbSQLMgr
    {
        #region Declarations
        /*==================================================================================================================================
         * Declarations
         *=================================================================================================================================*/
        private UserSession _userSession = null;

        #endregion


        #region Constructors
        /*==================================================================================================================================
         * Constructors
         *=================================================================================================================================*/
        public DbFilterSQLMgr()
            : base()
        {
            initialize();
        }
        public DbFilterSQLMgr(UserSession userSession)
            : base(userSession)
        {
            _userSession = userSession;
            initialize();
        }
        #endregion


        #region Properties
        /*==================================================================================================================================
         * Properties
         *=================================================================================================================================*/
        #endregion


        #region Public Methods
        /*==================================================================================================================================
         * Public Methods
         *=================================================================================================================================*/
        public questStatus GenerateFilterSQL(Filter filter, out Filter filterWithSQL)
        {
            // Initialize
            questStatus status = null;
            filterWithSQL = null;


            // There has to be at least one item.
            if (filter.FilterItemList.Count == 0)
            {
                filterWithSQL = filter;
                filterWithSQL.SQL = null;
                return (new questStatus(Severity.Success));
            }

            // Build FROM entities
            List<FilterEntity> FROMEntityList = null;
            try
            {
                status = buildFROMEntities(filter, out FROMEntityList);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }
            }
            catch (System.Exception ex)
            {
                return (new questStatus(Severity.Fatal, String.Format("EXCEPTION: generating filter SQL: Build FROM entities: {0}",
                    ex.Message)));
            }


            // Build JOIN entity list
            List<JoinEntity> joinEntityList = null;
            try
            {
                status = getJoinEntities(filter, out joinEntityList);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }
            }
            catch (System.Exception ex)
            {
                return (new questStatus(Severity.Fatal, String.Format("EXCEPTION: generating filter SQL: Build JOIN entity list: {0}",
                    ex.Message)));
            }


            // Remove FROM entities being joined to except self-joins.
            List<FilterEntity> FROMEntityList2 = new List<FilterEntity>();
            try
            {
                foreach (FilterEntity filterEntity in FROMEntityList)
                {
                    JoinEntity joinEntity = null;
                    if (filterEntity.Type.Id == FilterEntityType.Table)
                    {
                        joinEntity = joinEntityList.Find(delegate (JoinEntity e) { return e.FilterTable != null && e.FilterTable.Schema == filterEntity.FilterTable.Schema && e.FilterTable.Name == filterEntity.FilterTable.Name; });
                    }
                    else if (filterEntity.Type.Id == FilterEntityType.View)
                    {
                        joinEntity = joinEntityList.Find(delegate (JoinEntity e) { return e.FilterView != null && e.FilterView.Schema == filterEntity.FilterView.Schema && e.FilterView.Name == filterEntity.FilterView.Name; });
                    }
                    else
                    {
                        return (new questStatus(Severity.Error, String.Format("ERROR: unknown filter entity type {0}", filterEntity.Type.Id)));
                    }
                    if (joinEntity == null)
                    {
                        FROMEntityList2.Add(filterEntity);
                    }
                    else if (isSelfJoin(filter, joinEntity))
                    {
                        FROMEntityList2.Add(filterEntity);
                    }
                }
                FROMEntityList = FROMEntityList2;
            }
            catch (System.Exception ex)
            {
                return (new questStatus(Severity.Fatal, String.Format("EXCEPTION: generating filter SQL: Remove FROM entities being joined to except self-joins: {0}",
                    ex.Message)));
            }


            // Set aliases
            // TODO: OPTIMIZE.  DONE ABOVE, BUT COULD BE MESSED UP AFTER PREVIOUS BLOCK.
            try
            {
                status = setAliases(FROMEntityList, out FROMEntityList2);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }
                FROMEntityList = FROMEntityList2;

                List<JoinEntity> joinEntityList2 = null;
                status = setAliases(joinEntityList, out joinEntityList2);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }
                joinEntityList = joinEntityList2;
            }
            catch (System.Exception ex)
            {
                return (new questStatus(Severity.Fatal, String.Format("EXCEPTION: generating filter SQL: Set aliases: {0}",
                    ex.Message)));
            }



            // Build FROM Clause
            StringBuilder sbFROM = new StringBuilder("    FROM ");
            try
            {
                for (int fedx = 0; fedx < FROMEntityList.Count; fedx += 1)
                {
                    FilterEntity filterEntity = FROMEntityList2[fedx];

                    if (fedx > 0)
                    {
                        sbFROM.Append("        ,");
                    }

                    if (filterEntity.Type.Id == FilterEntityType.Table)
                    {
                        sbFROM.Append(BracketIdentifier(filterEntity.FilterTable.TablesetTable.Table.Schema));
                        sbFROM.Append(".");
                        sbFROM.Append(BracketIdentifier(filterEntity.FilterTable.TablesetTable.Table.Name));
                    }
                    else if (filterEntity.Type.Id == FilterEntityType.View)
                    {
                        sbFROM.Append(BracketIdentifier(filterEntity.FilterView.TablesetView.View.Schema));
                        sbFROM.Append(".");
                        sbFROM.Append(BracketIdentifier(filterEntity.FilterView.TablesetView.View.Name));
                    }
                    sbFROM.Append(" " + filterEntity.Alias);
                    sbFROM.AppendLine();
                }
            }
            catch (System.Exception ex)
            {
                return (new questStatus(Severity.Fatal, String.Format("EXCEPTION: generating filter SQL: Build FROM Clause: {0}",
                    ex.Message)));
            }


            // Append JOINs onto FROM clause
            List<string> joinClauseList = null;
            status = buildJOINclauses(filter, FROMEntityList, joinEntityList, out joinClauseList);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            foreach (string joinClause in joinClauseList)
            {
                sbFROM.AppendLine(joinClause);
            }



            // Build result columns
            FilterItem filterItem = null;
            StringBuilder sbResultColumns = new StringBuilder("");
            try
            {
                string lastFilterEntityAlias = null;
                for (int idx = 0; idx < filter.FilterItemList.Count; idx += 1)
                {
                    filterItem = filter.FilterItemList[idx];
                    FilterEntity filterEntity = null;
                    status = getFilterItemFROMEntity(filterItem, FROMEntityList, out filterEntity);
                    if (!questStatusDef.IsSuccessOrWarning(status))
                    {
                        return (status);
                    }


                    JoinEntity joinEntity = null;
                    if (filterEntity == null)
                    {
                        status = getFilterItemJOINEntity(filter, filterItem, joinEntityList, out joinEntity);
                        if (!questStatusDef.IsSuccess(status))
                        {
                            return (status);
                        }
                        if (joinEntity == null)
                        {
                            return (new questStatus(Severity.Error, String.Format("ERROR: filterItem {0} not found in FROM nor JOIN entities building result columns",
                                filterItem.Id)));
                        }
                        filterEntity = new FilterEntity();
                        filterEntity.Alias = joinEntity.Alias;
                        filterEntity.Type = joinEntity.Type;
                        filterEntity.FilterTable = joinEntity.FilterTable;
                        filterEntity.FilterView = joinEntity.FilterView;
                    }

                    // Some formatting logic ...
                    if (filterEntity.Alias != lastFilterEntityAlias)
                    {
                        if (!string.IsNullOrEmpty(lastFilterEntityAlias))
                        {
                            sbResultColumns.AppendLine();
                            sbResultColumns.Append("                ");
                        }
                        lastFilterEntityAlias = filterEntity.Alias;
                    }

                    filterItem.Identifier = BracketIdentifier(filterEntity.Alias) + "." + BracketIdentifier(filterItem.FilterColumn.Name);

                    // Label, use if provided.  Otherwise, column name only when only 1 entity in filter.
                    if (string.IsNullOrEmpty(filterItem.Label))
                    {
                        if ((filter.FilterTableList.Count + filter.FilterViewList.Count) == 1)
                        {
                            filterItem.Label = filterItem.FilterColumn.Name;
                        }
                        else
                        {
                            if (filterEntity.Type.Id == FilterEntityType.Table)
                            {
                                filterItem.Label = string.IsNullOrEmpty(filterItem.Label) ? filterEntity.FilterTable.TablesetTable.Table.Name + "_" + filterItem.FilterColumn.Name : filterItem.Label;
                            }
                            else if (filterEntity.Type.Id == FilterEntityType.View)
                            {
                                filterItem.Label = string.IsNullOrEmpty(filterItem.Label) ? filterEntity.FilterView.TablesetView.View.Name + "_" + filterItem.FilterColumn.Name : filterItem.Label;
                            }
                        }
                    }

                    // Add column to SQL with label, optional comma if another column TODO.
                    sbResultColumns.Append(filterItem.Identifier);
                    sbResultColumns.Append(" AS '" + filterItem.Label + "'");
                    if (idx + 1 < filter.FilterItemList.Count)
                    {
                        sbResultColumns.Append(", ");
                    }
                }
            }
            catch (System.Exception ex)
            {
                return (new questStatus(Severity.Fatal, String.Format("EXCEPTION: Building result columns SQL on FilterItem {0}: {1}",
                    filterItem.Id, ex.Message)));
            }


            // Build WHERE clause
            StringBuilder sbWHEREclause = null;
            List<string> whereClauseList = new List<string>();
            try
            {
                for (int idx = 0; idx < filter.FilterItemList.Count; idx += 1)
                {
                    // Initialize
                    List<string> itemClauseList = new List<string>();
                    filterItem = filter.FilterItemList[idx];


                    // Build sub-WHERE clause(es)
                    int numItemClauses = 0;
                    for (int odx = 0; odx < filterItem.OperationList.Count; odx += 1)
                    {
                        FilterOperation filterOperation = filterItem.OperationList[odx];


                        // If no operator, skip it.
                        if (filterOperation.FilterOperatorId < BaseId.VALID_ID)
                        {
                            continue;
                        }

                        // Generate clause(es).
                        ClauseGenerationResult clauseGenerationResult = null;
                        status = GenerateOperatorClause(filterItem, filterOperation, out clauseGenerationResult);
                        if (!questStatusDef.IsSuccess(status))
                        {
                            return (status);
                        }

                        // Wrap multiple subclauses in parens.
                        // Accumulate subclause IF we have at least one.
                        StringBuilder sbItemClause = new StringBuilder();
                        if (clauseGenerationResult.NumSubclauses > 1)
                        {
                            sbItemClause.AppendLine(" ( ");
                        }
                        if (clauseGenerationResult.NumSubclauses > 0)
                        {
                            numItemClauses += 1;
                            sbItemClause.AppendLine(clauseGenerationResult.Clause);
                        }
                        if (clauseGenerationResult.NumSubclauses > 1)
                        {
                            sbItemClause.AppendLine(" ) ");
                        }


                        // Accumulate clauses
                        itemClauseList.Add(sbItemClause.ToString());
                    }
                    // Build sub-WHERE clause for this item.
                    //       Wrap in parens if more than one subclause.
                    //       OR subclauses.
                    StringBuilder sbWHERESubClause = new StringBuilder();
                    if (itemClauseList.Count > 1)
                    {
                        sbWHERESubClause.Append(" ( ");
                    }
                    for (int sdx = 0; sdx < itemClauseList.Count; sdx += 1)
                    {
                        sbWHERESubClause.Append(itemClauseList[sdx]);
                        if (sdx + 1 < itemClauseList.Count)
                        {
                            sbWHERESubClause.Append(" OR ");
                        }
                    }
                    if (itemClauseList.Count > 1)
                    {
                        sbWHERESubClause.Append(" ) ");
                    }
                    if (itemClauseList.Count > 0)
                    {
                        whereClauseList.Add(sbWHERESubClause.ToString());
                    }
                }

                // Build WHERE clause for all item clauses.
                //       Wrap in parens if more than one subclause.
                //       OR subclauses.
                sbWHEREclause = new StringBuilder();
                if (whereClauseList.Count > 1)
                {
                    sbWHEREclause.AppendLine(" ( ");
                }
                for (int wdx = 0; wdx < whereClauseList.Count; wdx += 1)
                {
                    sbWHEREclause.Append("    " + whereClauseList[wdx]);
                    if (wdx + 1 < whereClauseList.Count)
                    {
                        sbWHEREclause.AppendLine("    AND ");
                    }
                }
                if (whereClauseList.Count > 1)
                {
                    sbWHEREclause.AppendLine(" ) ");
                }
            }
            catch (System.Exception ex)
            {
                return (new questStatus(Severity.Fatal, String.Format("EXCEPTION: generating filter SQL WHERE clause: {0}",
                    ex.Message)));
            }


            // Build SQL
            try
            {
                StringBuilder sbSQL = new StringBuilder("SELECT DISTINCT "); // TODO: DISTINCT explicit and UI-configurable
                sbSQL.AppendLine(sbResultColumns.ToString());
                sbSQL.Append(sbFROM.ToString());
                if (whereClauseList.Count > 0)
                {
                    sbSQL.AppendLine("WHERE ");
                    sbSQL.Append(sbWHEREclause.ToString());
                }

                // Temporary
                filterWithSQL = new Filter();
                BufferMgr.TransferBuffer(filter, filterWithSQL);
                filterWithSQL.SQL = sbSQL.ToString();
            }
            catch (System.Exception ex)
            {
                return (new questStatus(Severity.Fatal, String.Format("EXCEPTION: generating filter SQL agreggating subclauses: {0}",
                    ex.Message)));
            }
            return (new questStatus(Severity.Success));
        }


        #region Operation Clauses
        //
        // Operation Clauses
        //
        public questStatus GenerateOperatorClause(FilterItem filterItem, FilterOperation filterOperation, out ClauseGenerationResult clauseGenerationResult)
        {
            // Initialize
            questStatus status = null;
            clauseGenerationResult = null;


            switch (filterOperation.FilterOperatorId)
            {
                case FilterOperator.NoOperation:
                    break;

                case FilterOperator.IsEqualTo:
                    status = GenerateIsEqualTo(filterItem, filterOperation, out clauseGenerationResult);
                    if (!questStatusDef.IsSuccess(status))
                    {
                        return (new questStatus(Severity.Error, String.Format("ERROR: generation IsEqualTo clause.  FilterItem.Id={0}  FilterOperation.Id={1}",
                            filterItem.Id, filterOperation.Id)));
                    }
                    break;
                case FilterOperator.Contains:
                    status = GenerateContains(filterItem, filterOperation, out clauseGenerationResult);
                    if (!questStatusDef.IsSuccess(status))
                    {
                        return (new questStatus(Severity.Error, String.Format("ERROR: generation Contains clause.  FilterItem.Id={0}  FilterOperation.Id={1}",
                            filterItem.Id, filterOperation.Id)));
                    }
                    break;
                case FilterOperator.StartsWith:
                    status = GenerateStartsWith(filterItem, filterOperation, out clauseGenerationResult);
                    if (!questStatusDef.IsSuccess(status))
                    {
                        return (new questStatus(Severity.Error, String.Format("ERROR: generation StartsWith clause.  FilterItem.Id={0}  FilterOperation.Id={1}",
                            filterItem.Id, filterOperation.Id)));
                    }
                    break;
                case FilterOperator.EndsWith:
                    status = GenerateEndsWith(filterItem, filterOperation, out clauseGenerationResult);
                    if (!questStatusDef.IsSuccess(status))
                    {
                        return (new questStatus(Severity.Error, String.Format("ERROR: generation EndsWith clause.  FilterItem.Id={0}  FilterOperation.Id={1}",
                            filterItem.Id, filterOperation.Id)));
                    }
                    break;
                case FilterOperator.IsBlank:
                    status = GenerateIsBlank(filterItem, filterOperation, out clauseGenerationResult);
                    if (!questStatusDef.IsSuccess(status))
                    {
                        return (new questStatus(Severity.Error, String.Format("ERROR: generation IsBlank clause.  FilterItem.Id={0}  FilterOperation.Id={1}",
                            filterItem.Id, filterOperation.Id)));
                    }
                    break;
                case FilterOperator.IsNull:
                    status = GenerateIsNull(filterItem, filterOperation, out clauseGenerationResult);
                    if (!questStatusDef.IsSuccess(status))
                    {
                        return (new questStatus(Severity.Error, String.Format("ERROR: generation IsNull clause.  FilterItem.Id={0}  FilterOperation.Id={1}",
                            filterItem.Id, filterOperation.Id)));
                    }
                    break;
                case FilterOperator.IsNOTEqualTo:
                    status = GenerateIsNOTEqualTo(filterItem, filterOperation, out clauseGenerationResult);
                    if (!questStatusDef.IsSuccess(status))
                    {
                        return (new questStatus(Severity.Error, String.Format("ERROR: generation IsNOTEqualTo clause.  FilterItem.Id={0}  FilterOperation.Id={1}",
                            filterItem.Id, filterOperation.Id)));
                    }
                    break;
                case FilterOperator.DoesNOTContain:
                    status = GenerateDoesNOTContain(filterItem, filterOperation, out clauseGenerationResult);
                    if (!questStatusDef.IsSuccess(status))
                    {
                        return (new questStatus(Severity.Error, String.Format("ERROR: generation DoesNOTContain clause.  FilterItem.Id={0}  FilterOperation.Id={1}",
                            filterItem.Id, filterOperation.Id)));
                    }
                    break;
                case FilterOperator.DoesNOTStartWith:
                    status = GenerateDoesNOTStartWith(filterItem, filterOperation, out clauseGenerationResult);
                    if (!questStatusDef.IsSuccess(status))
                    {
                        return (new questStatus(Severity.Error, String.Format("ERROR: generation DoesNOTStartWith clause.  FilterItem.Id={0}  FilterOperation.Id={1}",
                            filterItem.Id, filterOperation.Id)));
                    }
                    break;
                case FilterOperator.DoesNOTEndWith:
                    status = GenerateDoesNOTEndWith(filterItem, filterOperation, out clauseGenerationResult);
                    if (!questStatusDef.IsSuccess(status))
                    {
                        return (new questStatus(Severity.Error, String.Format("ERROR: generation DoesNOTEndWith clause.  FilterItem.Id={0}  FilterOperation.Id={1}",
                            filterItem.Id, filterOperation.Id)));
                    }
                    break;
                case FilterOperator.IsNOTBlank:
                    status = GenerateIsNOTBlank(filterItem, filterOperation, out clauseGenerationResult);
                    if (!questStatusDef.IsSuccess(status))
                    {
                        return (new questStatus(Severity.Error, String.Format("ERROR: generation IsNOTBlank clause.  FilterItem.Id={0}  FilterOperation.Id={1}",
                            filterItem.Id, filterOperation.Id)));
                    }
                    break;
                case FilterOperator.IsNOTNull:
                    status = GenerateIsNOTNull(filterItem, filterOperation, out clauseGenerationResult);
                    if (!questStatusDef.IsSuccess(status))
                    {
                        return (new questStatus(Severity.Error, String.Format("ERROR: generation IsNOTNull clause.  FilterItem.Id={0}  FilterOperation.Id={1}",
                            filterItem.Id, filterOperation.Id)));
                    }
                    break;
                case FilterOperator.LessThanOrEqualTo:
                    status = GenerateLessThanOrEqualTo(filterItem, filterOperation, out clauseGenerationResult);
                    if (!questStatusDef.IsSuccess(status))
                    {
                        return (new questStatus(Severity.Error, String.Format("ERROR: generation LessThanOrEqualTo clause.  FilterItem.Id={0}  FilterOperation.Id={1}",
                            filterItem.Id, filterOperation.Id)));
                    }
                    break;
                case FilterOperator.LessThan:
                    status = GenerateLessThan(filterItem, filterOperation, out clauseGenerationResult);
                    if (!questStatusDef.IsSuccess(status))
                    {
                        return (new questStatus(Severity.Error, String.Format("ERROR: generation LessThan clause.  FilterItem.Id={0}  FilterOperation.Id={1}",
                            filterItem.Id, filterOperation.Id)));
                    }
                    break;
                case FilterOperator.GreaterThan:
                    status = GenerateGreaterThan(filterItem, filterOperation, out clauseGenerationResult);
                    if (!questStatusDef.IsSuccess(status))
                    {
                        return (new questStatus(Severity.Error, String.Format("ERROR: generation GreaterThan clause.  FilterItem.Id={0}  FilterOperation.Id={1}",
                            filterItem.Id, filterOperation.Id)));
                    }
                    break;
                case FilterOperator.GreaterThanOrEqualTo:
                    status = GenerateGreaterThanOrEqualTo(filterItem, filterOperation, out clauseGenerationResult);
                    if (!questStatusDef.IsSuccess(status))
                    {
                        return (new questStatus(Severity.Error, String.Format("ERROR: generation GreaterThanOrEqualTo clause.  FilterItem.Id={0}  FilterOperation.Id={1}",
                            filterItem.Id, filterOperation.Id)));
                    }
                    break;
                case FilterOperator.MatchesAdvanced:
                    status = GenerateMatchesAdvanced(filterItem, filterOperation, out clauseGenerationResult);
                    if (!questStatusDef.IsSuccess(status))
                    {
                        return (new questStatus(Severity.Error, String.Format("ERROR: generation MatchesAdvanced clause.  FilterItem.Id={0}  FilterOperation.Id={1}",
                            filterItem.Id, filterOperation.Id)));
                    }
                    break;
                default:
                    return (new questStatus(Severity.Error, String.Format("ERROR: Invalid filter operation Id {0}.  Filter Item {1}",
                            filterOperation.FilterOperatorId, filterItem.Id)));
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus GenerateIsEqualTo(FilterItem filterItem, FilterOperation filterOperation, out ClauseGenerationResult clauseGenerationResult)
        {
            // Initialize
            clauseGenerationResult = null;


            // Generate clause
            StringBuilder sbClause = new StringBuilder();
            sbClause.Append(filterItem.Identifier);
            sbClause.Append(" IN (");
            sbClause.Append(quoteValueList(filterOperation.ValueList));
            sbClause.Append(")");


            // Return clause
            clauseGenerationResult = new ClauseGenerationResult();
            clauseGenerationResult.NumSubclauses = 1;
            clauseGenerationResult.Clause = sbClause.ToString();


            return (new questStatus(Severity.Success));
        }
        public questStatus GenerateContains(FilterItem filterItem, FilterOperation filterOperation, out ClauseGenerationResult clauseGenerationResult)
        {
            return (generateContains(false, filterItem, filterOperation, out clauseGenerationResult));
        }
        public questStatus GenerateStartsWith(FilterItem filterItem, FilterOperation filterOperation, out ClauseGenerationResult clauseGenerationResult)
        {
            return (generateStartsWith(false, filterItem, filterOperation, out clauseGenerationResult));
        }
        public questStatus GenerateEndsWith(FilterItem filterItem, FilterOperation filterOperation, out ClauseGenerationResult clauseGenerationResult)
        {
            return (generateEndsWith(false, filterItem, filterOperation, out clauseGenerationResult));
        }
        public questStatus GenerateIsBlank(FilterItem filterItem, FilterOperation filterOperation, out ClauseGenerationResult clauseGenerationResult)
        {
            return (generateIsBlank(false, filterItem, filterOperation, out clauseGenerationResult));
        }
        public questStatus GenerateIsNull(FilterItem filterItem, FilterOperation filterOperation, out ClauseGenerationResult clauseGenerationResult)
        {
            return (generateIsNull(false, filterItem, filterOperation, out clauseGenerationResult));
        }
        public questStatus GenerateIsNOTEqualTo(FilterItem filterItem, FilterOperation filterOperation, out ClauseGenerationResult clauseGenerationResult)
        {
            // Initialize
            clauseGenerationResult = new ClauseGenerationResult();
            StringBuilder sbClause = new StringBuilder();

            // Generate clause
            sbClause.Append(filterItem.Identifier);
            sbClause.Append(" NOT IN (");
            sbClause.Append(quoteValueList(filterOperation.ValueList));
            sbClause.Append(")");


            // Return clause
            clauseGenerationResult.NumSubclauses = 1;
            clauseGenerationResult.Clause = sbClause.ToString();

            return (new questStatus(Severity.Success));
        }
        public questStatus GenerateDoesNOTContain(FilterItem filterItem, FilterOperation filterOperation, out ClauseGenerationResult clauseGenerationResult)
        {
            return (generateContains(true, filterItem, filterOperation, out clauseGenerationResult));
        }
        public questStatus GenerateDoesNOTStartWith(FilterItem filterItem, FilterOperation filterOperation, out ClauseGenerationResult clauseGenerationResult)
        {
            return (generateStartsWith(true, filterItem, filterOperation, out clauseGenerationResult));
        }
        public questStatus GenerateDoesNOTEndWith(FilterItem filterItem, FilterOperation filterOperation, out ClauseGenerationResult clauseGenerationResult)
        {
            return (generateEndsWith(true, filterItem, filterOperation, out clauseGenerationResult));
        }
        public questStatus GenerateIsNOTBlank(FilterItem filterItem, FilterOperation filterOperation, out ClauseGenerationResult clauseGenerationResult)
        {
            return (generateIsBlank(true, filterItem, filterOperation, out clauseGenerationResult));
        }
        public questStatus GenerateIsNOTNull(FilterItem filterItem, FilterOperation filterOperation, out ClauseGenerationResult clauseGenerationResult)
        {
            return (generateIsNull(true, filterItem, filterOperation, out clauseGenerationResult));
        }
        public questStatus GenerateLessThanOrEqualTo(FilterItem filterItem, FilterOperation filterOperation, out ClauseGenerationResult clauseGenerationResult)
        {
            return (generateOpClause(filterItem, "<=", filterOperation, out clauseGenerationResult));
        }
        public questStatus GenerateLessThan(FilterItem filterItem, FilterOperation filterOperation, out ClauseGenerationResult clauseGenerationResult)
        {
            return (generateOpClause(filterItem, "<", filterOperation, out clauseGenerationResult));
        }
        public questStatus GenerateGreaterThan(FilterItem filterItem, FilterOperation filterOperation, out ClauseGenerationResult clauseGenerationResult)
        {
            return (generateOpClause(filterItem, ">", filterOperation, out clauseGenerationResult));
        }
        public questStatus GenerateGreaterThanOrEqualTo(FilterItem filterItem, FilterOperation filterOperation, out ClauseGenerationResult clauseGenerationResult)
        {
            return (generateOpClause(filterItem, ">==", filterOperation, out clauseGenerationResult));
        }
        public questStatus GenerateMatchesAdvanced(FilterItem filterItem, FilterOperation filterOperation, out ClauseGenerationResult clauseGenerationResult)
        {
            // Initialize
            clauseGenerationResult = new ClauseGenerationResult();
            StringBuilder sbClause = new StringBuilder();

            // Return clause
            clauseGenerationResult.Clause = sbClause.ToString();

            return (new questStatus(Severity.Success));
        }
        #endregion


        #region Major Clauses
        //
        // Major Clauses
        //
        public questStatus BuildFROMClause(Filter filter, out string FROMClause)
        {
            // Initialize
            questStatus status = null;
            FROMClause = null;


            // There has to be at least one item.
            if (filter.FilterItemList.Count == 0)
            {
                return (new questStatus(Severity.Success));
            }

            // Build FROM entities
            List<FilterEntity> FROMEntityList = null;
            status = buildFROMEntities(filter, out FROMEntityList);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }

            // Build JOIN entity list
            List<JoinEntity> joinEntityList = null;
            status = getJoinEntities(filter, out joinEntityList);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }

            // Remove FROM entities being joined to except self-joins.
            List<FilterEntity> FROMEntityList2 = new List<FilterEntity>();
            foreach (FilterEntity filterEntity in FROMEntityList)
            {
                JoinEntity joinEntity = null;
                if (filterEntity.Type.Id == FilterEntityType.Table)
                {
                    joinEntity = joinEntityList.Find(delegate (JoinEntity e) { return e.FilterTable != null && e.FilterTable.TablesetTable.Table.Id == filterEntity.FilterTable.TablesetTable.Table.Id; });
                }
                else if (filterEntity.Type.Id == FilterEntityType.View)
                {
                    joinEntity = joinEntityList.Find(delegate (JoinEntity e) { return e.FilterView != null && e.FilterView.TablesetView.View.Id == filterEntity.FilterView.TablesetView.View.Id; });
                }
                else
                {
                    return (new questStatus(Severity.Error, String.Format("ERROR: unknown filter entity type {0}", filterEntity.Type.Id)));
                }
                if (joinEntity == null)
                {
                    FROMEntityList2.Add(filterEntity);
                }
                else if (isSelfJoin(filter, joinEntity))
                {
                    FROMEntityList2.Add(filterEntity);
                }
            }
            FROMEntityList = FROMEntityList2;

            // Set aliases
            // TODO: OPTIMIZE.  DONE ABOVE, BUT COULD BE MESSED UP AFTER PREVIOUS BLOCK.
            status = setAliases(FROMEntityList, out FROMEntityList2);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            FROMEntityList = FROMEntityList2;

            List<JoinEntity> joinEntityList2 = null;
            status = setAliases(joinEntityList, out joinEntityList2);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            joinEntityList = joinEntityList2;


            // Build FROM Clause
            StringBuilder sbFROM = new StringBuilder("    FROM ");
            for (int fedx = 0; fedx < FROMEntityList.Count; fedx += 1)
            {
                FilterEntity filterEntity = FROMEntityList2[fedx];

                if (fedx > 0)
                {
                    sbFROM.Append(",");
                }

                if (filterEntity.Type.Id == FilterEntityType.Table)
                {
                    sbFROM.Append(BracketIdentifier(filterEntity.FilterTable.TablesetTable.Table.Schema));
                    sbFROM.Append(".");
                    sbFROM.Append(BracketIdentifier(filterEntity.FilterTable.TablesetTable.Table.Name));
                }
                else if (filterEntity.Type.Id == FilterEntityType.View)
                {
                    sbFROM.Append(BracketIdentifier(filterEntity.FilterView.TablesetView.View.Schema));
                    sbFROM.Append(".");
                    sbFROM.Append(BracketIdentifier(filterEntity.FilterView.TablesetView.View.Name));
                }
                sbFROM.Append(" " + filterEntity.Alias);
                sbFROM.AppendLine();
            }

            // Return FROM clause
            FROMClause = sbFROM.ToString();


            return (new questStatus(Severity.Success));
        }
        #endregion


        #endregion


        #region Private Methods
        /*==================================================================================================================================
         * Private Methods
         *=================================================================================================================================*/
        private questStatus initialize()
        {
            // Initialize
            questStatus status = null;
            try
            {
            }
            catch (System.Exception ex)
            {
                status = new questStatus(Severity.Fatal, String.Format("EXCEPTION: {0}.{1}: {2}",
                        this.GetType().ToString(), MethodInfo.GetCurrentMethod().Name, ex.Message));
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        private string quoteValueList(List<FilterValue> filterValueList, bool preLike = false, bool postLike = false)
        {
            StringBuilder sbValueList = new StringBuilder();
            for (int idx = 0; idx < filterValueList.Count; idx += 1)
            {
                FilterValue filterValue = filterValueList[idx];
                sbValueList.Append("'" + (preLike ? "%" : "") + filterValue.Value + (postLike ? "%" : "") + "'");
                if (idx + 1 < filterValueList.Count)
                {
                    sbValueList.Append(", ");
                }
            }
            return (sbValueList.ToString());
        }
        private int numSubsequentOperations(int idxCurrentOperation, List<FilterOperation> filterOperationList)
        {
            // Return number of operations past current operation.
            int num = 0;
            for (int idx = idxCurrentOperation; idx < filterOperationList.Count; idx += 1)
            {
                if (filterOperationList[idx].FilterOperatorId >= BaseId.VALID_ID)
                {
                    num += 1;
                }
            }
            return (num);
        }

        private questStatus generateContains(bool bNOT, FilterItem filterItem, FilterOperation filterOperation, out ClauseGenerationResult clauseGenerationResult)
        {
            // Initialize
            clauseGenerationResult = new ClauseGenerationResult();


            // Generate clause
            StringBuilder sbClause = new StringBuilder();
            for (int idx = 0; idx < filterOperation.ValueList.Count; idx += 1)
            {
                clauseGenerationResult.NumSubclauses += 1;

                FilterValue filterValue = filterOperation.ValueList[idx];
                sbClause.Append(filterItem.Identifier);
                sbClause.Append(" " + (bNOT ? "NOT" : "") + " LIKE (");
                sbClause.Append("'%" + filterValue.Value + "%'");
                sbClause.Append(")");

                if (idx + 1 < filterOperation.ValueList.Count)
                {
                    sbClause.Append(" AND ");
                }
            }
            if (filterOperation.ValueList.Count > 1)
            {
                sbClause.Insert(0, " (");
                sbClause.Append(") ");
            }

            // Return clause
            clauseGenerationResult.Clause = sbClause.ToString();


            return (new questStatus(Severity.Success));
        }
        private questStatus generateStartsWith(bool bNOT, FilterItem filterItem, FilterOperation filterOperation, out ClauseGenerationResult clauseGenerationResult)
        {
            // Initialize
            clauseGenerationResult = new ClauseGenerationResult();
            StringBuilder sbClause = new StringBuilder();

            // Generate clause
            for (int idx = 0; idx < filterOperation.ValueList.Count; idx += 1)
            {
                clauseGenerationResult.NumSubclauses += 1;

                FilterValue filterValue = filterOperation.ValueList[idx];
                sbClause.Append(filterItem.Identifier);
                sbClause.Append(" " + (bNOT ? "NOT" : "") + " LIKE (");
                sbClause.Append("'" + filterValue.Value + "%'");
                sbClause.Append(")");

                if (idx + 1 < filterOperation.ValueList.Count)
                {
                    sbClause.Append(" AND ");
                }
            }
            if (filterOperation.ValueList.Count > 1)
            {
                sbClause.Insert(0, " (");
                sbClause.Append(") ");
            }

            // Return clause
            clauseGenerationResult.Clause = sbClause.ToString();


            return (new questStatus(Severity.Success));
        }
        private questStatus generateEndsWith(bool bNOT, FilterItem filterItem, FilterOperation filterOperation, out ClauseGenerationResult clauseGenerationResult)
        {
            // Initialize
            clauseGenerationResult = new ClauseGenerationResult();
            StringBuilder sbClause = new StringBuilder();

            // Generate clause
            for (int idx = 0; idx < filterOperation.ValueList.Count; idx += 1)
            {
                clauseGenerationResult.NumSubclauses += 1;

                FilterValue filterValue = filterOperation.ValueList[idx];
                sbClause.Append(filterItem.Identifier);
                sbClause.Append(" " + (bNOT ? "NOT" : "") + " LIKE (");
                sbClause.Append("'%" + filterValue.Value + "'");
                sbClause.Append(")");

                if (idx + 1 < filterOperation.ValueList.Count)
                {
                    sbClause.Append(" AND ");
                }
            }
            if (filterOperation.ValueList.Count > 1)
            {
                sbClause.Insert(0, " (");
                sbClause.Append(") ");
            }

            // Return clause
            clauseGenerationResult.Clause = sbClause.ToString();


            return (new questStatus(Severity.Success));
        }
        private questStatus generateIsBlank(bool bNOT, FilterItem filterItem, FilterOperation filterOperation, out ClauseGenerationResult clauseGenerationResult)
        {
            // Initialize
            clauseGenerationResult = new ClauseGenerationResult();
            StringBuilder sbClause = new StringBuilder();


            // Generate clause
            sbClause.Append(filterItem.Identifier);
            sbClause.Append(" " + (bNOT ? "NOT" : "") + " LIKE (");
            sbClause.Append("'% %'");
            sbClause.Append(" AND ");
            sbClause.Append(" " + filterItem.Identifier + " IS NULL ");
            sbClause.Append(") ");


            // Return clause
            clauseGenerationResult.NumSubclauses = 1;
            clauseGenerationResult.Clause = sbClause.ToString();


            return (new questStatus(Severity.Success));
        }
        private questStatus generateIsNull(bool bNOT, FilterItem filterItem, FilterOperation filterOperation, out ClauseGenerationResult clauseGenerationResult)
        {
            // Initialize
            clauseGenerationResult = new ClauseGenerationResult();
            StringBuilder sbClause = new StringBuilder();


            // Generate clause
            sbClause.Append(filterItem.Identifier);
            sbClause.Append(" IS " + (bNOT ? "NOT" : "") + " NULL");


            // Return clause
            clauseGenerationResult.NumSubclauses = 1;
            clauseGenerationResult.Clause = sbClause.ToString();

            return (new questStatus(Severity.Success));
        }
        private questStatus generateOpClause(FilterItem filterItem, string operatorToken, FilterOperation filterOperation, out ClauseGenerationResult clauseGenerationResult)
        {
            // Initialize
            clauseGenerationResult = new ClauseGenerationResult();

            // Generate clause
            StringBuilder sbClause = new StringBuilder();
            for (int idx = 0; idx < filterOperation.ValueList.Count; idx += 1)
            {
                FilterValue filterValue = filterOperation.ValueList[idx];
                sbClause.Append(filterItem.Identifier);
                sbClause.Append(" " + operatorToken + " ");

                int _value;
                if (int.TryParse(filterValue.Value, out _value))
                {
                    sbClause.Append(filterValue.Value);
                }
                else
                {
                    sbClause.Append("'" + filterValue.Value + "'");
                }

                if (idx + 1 < filterOperation.ValueList.Count)
                {
                    sbClause.Append(" AND ");
                }
            }

            // Return clause result
            clauseGenerationResult.NumSubclauses = filterOperation.ValueList.Count;
            clauseGenerationResult.Clause = sbClause.ToString();

            return (new questStatus(Severity.Success));
        }

        private questStatus buildFROMEntities(Filter filter, out List<FilterEntity> normalizedFROMEntityList)
        {
            // Initialize
            normalizedFROMEntityList = new List<FilterEntity>();
            int numTables = 0;
            int numViews = 0;

            // Build normalized list of all the entities from which filter items are in.
            for (int idx = 0; idx < filter.FilterItemList.Count; idx += 1)
            { 
                FilterItem filterItem = filter.FilterItemList[idx];
                if (filterItem.FilterColumn.FilterEntityTypeId == FilterEntityType.Table)
                {
                    FilterTable filterTable = filter.FilterTableList.Find(delegate (FilterTable t) { return (t.Id == filterItem.FilterColumn.FilterEntityId); });
                    if (filterTable == null)
                    {
                        return (new questStatus(Severity.Error, String.Format("ERROR: building FROM entities, filterItem #{0} FilterColumn.FilterEntityId {1} not in FilterTableList",
                                idx+1, filterItem.FilterColumn.FilterEntityId)));
                    }
                    FilterEntity filterEntity = normalizedFROMEntityList.Find(delegate (FilterEntity fe) {
                            return (fe.Type.Id == FilterEntityType.Table && fe.FilterTable.Schema == filterTable.Schema && fe.FilterTable.Name == filterTable.Name); });
                    if (filterEntity == null)
                    {
                        numTables += 1;
                        filterEntity = new FilterEntity();
                        filterEntity.Type.Id = FilterEntityType.Table;
                        filterEntity.FilterTable = filterTable;
                        filterEntity.FilterView = null;
                        filterEntity.Alias = "T" + (numTables).ToString();
                        normalizedFROMEntityList.Add(filterEntity);
                    }
                }
                else if (filterItem.FilterColumn.FilterEntityTypeId == FilterEntityType.View)
                {
                    FilterView filterView = filter.FilterViewList.Find(delegate (FilterView v) { return (v.Id == filterItem.FilterColumn.FilterEntityId); });
                    if (filterView == null)
                    {
                        return (new questStatus(Severity.Error, String.Format("ERROR: building FROM entities, filterItem #{0} FilterColumn.FilterEntityId {1} not in FilterViewList",
                                idx + 1, filterItem.FilterColumn.FilterEntityId)));
                    }
                    FilterEntity filterEntity = normalizedFROMEntityList.Find(delegate (FilterEntity fe) {
                        return (fe.Type.Id == FilterEntityType.View && fe.FilterView.Schema == filterView.Schema && fe.FilterView.Name == filterView.Name);
                    });
                    if (filterEntity == null)
                    {
                        numViews += 1;
                        filterEntity = new FilterEntity();
                        filterEntity.Type.Id = FilterEntityType.View;
                        filterEntity.FilterTable = null;
                        filterEntity.FilterView = filterView;
                        filterEntity.Alias = "V" + (numViews).ToString();
                        normalizedFROMEntityList.Add(filterEntity);
                    }
                }
            }
            return (new questStatus(Severity.Success));
        }
        private questStatus getJoinEntities(Filter filter, out List<JoinEntity> joinEntityList)
        {
            // Initialize
            questStatus status = null;
            joinEntityList = new List<JoinEntity>();


            for (int idx = 0; idx < filter.FilterItemList.Count; idx += 1)
            {
                FilterItem filterItem = filter.FilterItemList[idx];
                for (int jdx = 0; jdx < filterItem.JoinList.Count; jdx += 1)
                {
                    FilterItemJoin filterItemJoin = filterItem.JoinList[jdx];

                    TablesetColumn sourceColumn = null;
                    status = getFilterItemJoinTablesetColumn(filter, filterItemJoin, out sourceColumn);
                    if (!questStatusDef.IsSuccess(status))
                    {
                        return (status);
                    }

                    if (filterItemJoin.TargetEntityTypeId == FilterEntityType.Table)
                    {
                        FilterTable JOINTable = filter.FilterTableList.Find(delegate (FilterTable t) { return filterItemJoin.TargetSchema == t.Schema && filterItemJoin.TargetEntityName == t.Name; });
                        if (JOINTable == null)
                        {
                            return (new questStatus(Severity.Error, String.Format("ERROR: FilterItem {0} FilterItemJoin {1} FilterTable [{2}].[{3}] not found determining JOIN entities",
                                    filterItem.Id, filterItemJoin.Id, filterItemJoin.TargetSchema, filterItemJoin.TargetEntityName)));
                        }
                        JoinEntity joinEntity = new JoinEntity();
                        joinEntity.Type.Id = FilterEntityType.Table;
                        joinEntity.FilterItem = filterItem;
                        joinEntity.FilterTable = JOINTable;
                        joinEntity.FilterView = null;
                        joinEntity.Alias = "JT" + (joinEntityList.Count + 1).ToString();
                        joinEntity.TablesetColumn = sourceColumn;
                        joinEntity.FilterItemJoin.FilterItemId = filterItem.Id;
                        joinEntity.FilterItemJoin.ColumnId = sourceColumn.Id;
                        joinEntity.FilterItemJoin.JoinType = filterItemJoin.JoinType;
                        joinEntityList.Add(joinEntity);
                    }
                    else if (filterItemJoin.TargetEntityTypeId == FilterEntityType.View)
                    {
                        FilterView JOINView = filter.FilterViewList.Find(delegate (FilterView v) { return filterItemJoin.TargetSchema == v.Schema && filterItemJoin.TargetEntityName == v.Name; });
                        if (JOINView == null)
                        {
                            return (new questStatus(Severity.Error, String.Format("ERROR: FilterItem {0} FilterItemJoin {1} FilterView [{2}].[{3}] not found determining JOIN entities",
                                    filterItem.Id, filterItemJoin.Id, filterItemJoin.TargetSchema, filterItemJoin.TargetEntityName)));
                        }
                        JoinEntity joinEntity = new JoinEntity();
                        joinEntity.Type.Id = FilterEntityType.View;
                        joinEntity.FilterItem = filterItem;
                        joinEntity.FilterTable = null;
                        joinEntity.FilterView = JOINView;
                        joinEntity.Alias = "JV" + (joinEntityList.Count + 1).ToString();
                        joinEntity.TablesetColumn = sourceColumn;
                        joinEntity.FilterItemJoin.FilterItemId = filterItem.Id;
                        joinEntity.FilterItemJoin.ColumnId = sourceColumn.Id;
                        joinEntity.FilterItemJoin.JoinType = filterItemJoin.JoinType;
                        joinEntityList.Add(joinEntity);
                    }
                    else
                    {
                        return (new questStatus(Severity.Error, String.Format("ERROR: Invalid entity type {0} on source TablesetColumn {0}",
                            sourceColumn.EntityTypeId, sourceColumn.Id)));
                    }
                }
            }
            return (new questStatus(Severity.Success));
        }
        private questStatus setAliases(List<FilterEntity> filterEntityList, out List<FilterEntity> aliasedFilterEntityList)
        {
            // Initialize
            aliasedFilterEntityList = new List<FilterEntity>();

            int numTables = 0;
            int numViews = 0;
            foreach (FilterEntity filterEntity in filterEntityList)
            {
                if (filterEntity.Type.Id == FilterEntityType.Table)
                {
                    numTables += 1;
                    filterEntity.Alias = "T" + numTables.ToString();
                }
                else if (filterEntity.Type.Id == FilterEntityType.View)
                {
                    numViews += 1;
                    filterEntity.Alias = "V" + numViews.ToString();
                }
                aliasedFilterEntityList.Add(filterEntity);
            }
            return (new questStatus(Severity.Success));
        }
        private questStatus setAliases(List<JoinEntity> joinEntityList, out List<JoinEntity> aliasedJoinEntityList)
        {
            // Initialize
            aliasedJoinEntityList = new List<JoinEntity>();

            int numJoinTables = 0;
            int numJoinViews = 0;
            foreach (JoinEntity joinEntity in joinEntityList)
            {
                if (joinEntity.Type.Id == FilterEntityType.Table)
                {
                    numJoinTables += 1;
                    joinEntity.Alias = "JT" + numJoinTables.ToString();
                }
                else if (joinEntity.Type.Id == FilterEntityType.View)
                {
                    numJoinViews += 1;
                    joinEntity.Alias = "JV" + numJoinViews.ToString();
                }
                aliasedJoinEntityList.Add(joinEntity);
            }
            return (new questStatus(Severity.Success));
        }

        private bool isSelfJoin(Filter filter, JoinEntity joinEntity)
        {
            // TEMPORARY
            // TODO: SUPPORT MULTIPLE JOINS
            return ((joinEntity.FilterItem.JoinList[0].SourceSchema == joinEntity.FilterItem.JoinList[0].TargetSchema)
                &&
                (joinEntity.FilterItem.JoinList[0].SourceEntityName == joinEntity.FilterItem.JoinList[0].TargetEntityName));
        }

        private questStatus getFilterItemFROMEntity(FilterItem filterItem, List<FilterEntity> FROMEntityList, out FilterEntity FROMEntity)
        {
            // Initialize
            FROMEntity = null;


            if (filterItem.FilterColumn.FilterEntityTypeId == FilterEntityType.Table)
            {
                foreach (FilterEntity filterEntity in FROMEntityList)
                {
                    if (filterEntity.FilterTable == null)
                    {
                        continue;
                    }
                    foreach (FilterColumn filterColumn in filterEntity.FilterTable.FilterColumnList)
                    {
                        if (filterItem.FilterColumn.Id == filterColumn.Id)
                        {
                            FROMEntity = filterEntity;
                            return (new questStatus(Severity.Success));
                        }
                    }
                }
            }
            else if (filterItem.FilterColumn.FilterEntityTypeId == FilterEntityType.View)
            {
                foreach (FilterEntity filterEntity in FROMEntityList)
                {
                    if (filterEntity.FilterView == null)
                    {
                        continue;
                    }
                    foreach (FilterColumn filterColumn in filterEntity.FilterView.FilterColumnList)
                    {
                        if (filterItem.FilterColumn.Id == filterColumn.Id)
                        {
                            FROMEntity = filterEntity;
                            return (new questStatus(Severity.Success));
                        }
                    }
                }
            }
            else
            {
                return (new questStatus(Severity.Error, String.Format("ERROR: FilterItem {0} FilterColumn.FilterEntityTypeId {1} unsupported (getFilterItemFROMEntity)",
                        filterItem.Id, filterItem.FilterColumn.FilterEntityTypeId)));
            }
            return (new questStatus(Severity.Success));
        }
        private questStatus getFilterItemJOINEntity(Filter filter, FilterItem filterItem, List<JoinEntity> JOINEntityList, out JoinEntity JOINEntity)
        {
            // Initialize
            JOINEntity = null;

            FilterColumn filterColumn = filterItem.FilterColumn;


            if (filterColumn.FilterEntityTypeId == FilterEntityType.Table)
            {
                foreach (JoinEntity joinEntity in JOINEntityList)
                {
                    if (joinEntity.FilterTable != null && filterColumn.FilterEntityId == joinEntity.FilterTable.Id)
                    {
                        JOINEntity = joinEntity;
                        return (new questStatus(Severity.Success));
                    }
                }
            }
            else if (filterColumn.FilterEntityTypeId == FilterEntityType.View)
            {
                foreach (JoinEntity joinEntity in JOINEntityList)
                {
                    if (joinEntity.FilterView != null && filterColumn.FilterEntityId == joinEntity.FilterView.Id)
                    {
                        JOINEntity = joinEntity;
                        return (new questStatus(Severity.Success));
                    }
                }
            }
            else
            {
                return (new questStatus(Severity.Error, String.Format("ERROR: FilterColumn {0} has invalid FilterEntityTypeId: {1} - getFilterItemJOINEntity",
                        filterColumn.Id, filterColumn.FilterEntityTypeId)));
            }
            return (new questStatus(Severity.Success));
        }




        ////////private questStatus getFilterItemTablesetColumn(Filter filter, FilterItem filterItem, out TablesetColumn tablesetColumn)
        ////////{
        ////////    // Initialize
        ////////    tablesetColumn = null;


        ////////    if (filterItem.FilterEntityTypeId != FilterEntityType.Column)
        ////////    {
        ////////        return (new questStatus(Severity.Error, String.Format("EXCEPTION: FilterItem {0} unsupported Entity type to determine TablesetColumn: {1}",
        ////////                filterItem.Id, filterItem.FilterEntityTypeId)));
        ////////    }

        ////////    foreach (FilterTable filterTable in filter.FilterTableList)
        ////////    {
        ////////        foreach (FilterColumn filterColumn in filterTable.FilterColumnList)
        ////////        {
        ////////            if (filterItem.FilterEntityId == filterColumn.TablesetColumnId)
        ////////            {
        ////////                tablesetColumn = filterColumn.TablesetColumn;
        ////////                return (new questStatus(Severity.Success));
        ////////            }
        ////////        }
        ////////    }
        ////////    foreach (FilterView filterView in filter.FilterViewList)
        ////////    {
        ////////        foreach (FilterColumn filterColumn in filterView.FilterColumnList)
        ////////        {
        ////////            if (filterItem.FilterEntityId == filterColumn.TablesetColumnId)
        ////////            {
        ////////                tablesetColumn = filterColumn.TablesetColumn;
        ////////                return (new questStatus(Severity.Success));
        ////////            }
        ////////        }
        ////////    }
        ////////    return (new questStatus(Severity.Error, String.Format("FilterItem {0} TablesetColumn not found", filterItem.Id)));
        ////////}
        private questStatus getFilterItemJoinTablesetColumn(Filter filter, FilterItemJoin filterItemJoin, out TablesetColumn tablesetColumn)
        {
            // Initialize
            tablesetColumn = null;


            foreach (FilterTable filterTable in filter.FilterTableList)
            {
                foreach (TablesetColumn _tablesetColumn in filterTable.TablesetTable.TablesetColumnList)
                {
                    if (filterItemJoin.ColumnId == _tablesetColumn.Id)
                    {
                        tablesetColumn = _tablesetColumn;
                        return (new questStatus(Severity.Success));
                    }
                }
            }
            foreach (FilterView filterView in filter.FilterViewList)
            {
                foreach (TablesetColumn _tablesetColumn in filterView.TablesetView.TablesetColumnList)
                {
                    if (filterItemJoin.ColumnId == _tablesetColumn.Id)
                    {
                        tablesetColumn = _tablesetColumn;
                        return (new questStatus(Severity.Success));
                    }
                }
            }
            return (new questStatus(Severity.Error, String.Format("FilterItemJoin {0} TablesetColumn not found", filterItemJoin.Id)));
        }

        private questStatus buildJOINclauses(Filter filter, List<FilterEntity> FROMEntityList, List<JoinEntity> joinEntityList, out List<string> joinClauseList)
        {
            // Initialize
            questStatus status = null;
            joinClauseList = new List<string>();


            // Build a JOIN clause for every join in the filter
            foreach (FilterItem filterItem in filter.FilterItemList)
            {
                foreach (FilterItemJoin filterItemJoin in filterItem.JoinList)
                {
                    // Get source alias
                    string sourceAlias = null;
                    status = findAlias(FROMEntityList, joinEntityList, filterItemJoin.SourceSchema, filterItemJoin.SourceEntityName, out sourceAlias);
                    if (!questStatusDef.IsSuccess(status))
                    {
                        return (status);
                    }

                    // Get target alias
                    string targetAlias = null;
                    status = findAlias(FROMEntityList, joinEntityList, filterItemJoin.TargetSchema, filterItemJoin.TargetEntityName, out targetAlias);
                    if (!questStatusDef.IsSuccess(status))
                    {
                        return (status);
                    }

                    // Build JOIN clause
                    string joinClause = String.Format("    " + filterItemJoin.JoinType + " " + BracketIdentifier(filterItemJoin.TargetSchema) + "." + BracketIdentifier(filterItemJoin.TargetEntityName) + " " + targetAlias
                            + " ON " + targetAlias + "." + filterItemJoin.TargetColumnName + " = " + sourceAlias + "." + filterItemJoin.SourceColumnName);
                    joinClauseList.Add(joinClause);
                }
            }
            return (new questStatus(Severity.Success));
        }

        private questStatus findAlias(List<FilterEntity> FROMEntityList, List<JoinEntity> joinEntityList, string schema, string name, out string alias)
        {
            // Initialize
            alias = null;

            foreach (FilterEntity filterEntity in FROMEntityList)
            {
                if (filterEntity.FilterTable != null)
                {
                    if (name == filterEntity.FilterTable.Name && schema == filterEntity.FilterTable.Schema)
                    {
                        alias = filterEntity.Alias;
                        return (new questStatus(Severity.Success));
                    }
                }
                else if (filterEntity.FilterView != null)
                {
                    if (name == filterEntity.FilterView.Name && schema == filterEntity.FilterView.Schema)
                    {
                        alias = filterEntity.Alias;
                        return (new questStatus(Severity.Success));
                    }
                }
            }
            foreach (JoinEntity joinEntity in joinEntityList)
            {
                if (joinEntity.FilterTable != null)
                {
                    if (name == joinEntity.FilterTable.Name && schema == joinEntity.FilterTable.Schema)
                    {
                        alias = joinEntity.Alias;
                        return (new questStatus(Severity.Success));
                    }
                }
                else if (joinEntity.FilterView != null)
                {
                    if (name == joinEntity.FilterView.Name && schema == joinEntity.FilterView.Schema)
                    {
                        alias = joinEntity.Alias;
                        return (new questStatus(Severity.Success));
                    }
                }
            }
            return (new questStatus(Severity.Error, String.Format("ERROR: [{0}].[{1}] alias not found in FROM entities or JOIN entities",
                    name, schema)));
        }
        #endregion
    }
}