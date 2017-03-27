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
using Quest.Functional.FMS;
using Quest.Functional.MasterPricing;
using Quest.MPDW.Services.Data;
using Quest.Services.Dbio.MasterPricing;
using Quest.MasterPricing.Services.Data.Filters;
using Quest.MasterPricing.Services.Data.Database;


namespace Quest.MasterPricing.Services.Data.Bulk
{
    public class DbBulkUpdateMgr : DbSQLMgr
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
        public DbBulkUpdateMgr()
            : base()
        {
            initialize();
        }
        public DbBulkUpdateMgr(UserSession userSession)
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
        public questStatus GenerateBulkUpdateSQL(BulkUpdateRequest bulkUpdateRequest)
        {
            // Initialize
            questStatus status = null;
            StringBuilder sbBulkUPDATE = new StringBuilder();

            // Validate
            if (bulkUpdateRequest.Filter.FilterTableList.Count != 1)
            {
                return (new questStatus(Severity.Error, String.Format("ERROR: currently, only single-table filters supported with BUlk Update")));
            }

            // Get info on FROM clause on filter SQL
            string FROMClause = null;
            List<FilterEntity> FROMEntityList = null;
            List<JoinEntity> joinEntityList = null;
            DbFilterSQLMgr dbFilterSQLMgr = new DbFilterSQLMgr();
            status = dbFilterSQLMgr.GetFROMEntities(bulkUpdateRequest.Filter, out FROMClause, out FROMEntityList, out joinEntityList);
            if (! questStatusDef.IsSuccess(status))
            {
                return (status);
            }

            sbBulkUPDATE.AppendLine(String.Format("UPDATE [{0}].[{1}] SET ",
                    bulkUpdateRequest.Filter.FilterTableList[0].TablesetTable.Table.Schema, bulkUpdateRequest.Filter.FilterTableList[0].TablesetTable.Table.Name));
            List<string> setClauseList = new List<string>();
            for (int idx = 0; idx < bulkUpdateRequest.Columns.Count; idx += 1)
            {
                BulkUpdateColumnValue bulkUpdateColumnValue = bulkUpdateRequest.Columns[idx];
                string setClause = null;

                status = getFilterColumn(bulkUpdateColumnValue, bulkUpdateRequest.Filter, FROMEntityList.Count);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }

                if (bulkUpdateColumnValue.bNull)
                {
                    setClause += String.Format(" [{0}] = NULL ", bulkUpdateColumnValue.Name);
                    setClauseList.Add(setClause);
                }
                else if (!string.IsNullOrEmpty(bulkUpdateColumnValue.Value))
                {
                    int _value = 0;
                    if (int.TryParse(bulkUpdateColumnValue.Value, out _value))
                    {
                        if (_value == -1 && bulkUpdateColumnValue.LookupId >= BaseId.VALID_ID)
                        {
                            continue;
                        }
                    }
                    setClause += String.Format(" [{0}] = {1} ", bulkUpdateColumnValue.Name, getValueIdentifier(bulkUpdateColumnValue));
                    setClauseList.Add(setClause);
                }
            }
            for (int idx=0; idx < setClauseList.Count; idx += 1)
            {
                string setClause = setClauseList[idx];
                if (idx > 0)
                {
                    sbBulkUPDATE.Append("    ,");
                }
                sbBulkUPDATE.AppendLine(setClause);
            }

            // KLUGIE: Get WHERE clause from Filter
            string whereClause = null;
            if (bulkUpdateRequest.Filter.SQL.IndexOf("WHERE") > -1)
            {
                // NOTE: NEED SUBCLAUSES PARSED OUT; THIS IS A QUICK KLUGIE FOR SINGLE TABLE ONLY.  BUT, MANY THINGS CAN COME AFTER A WHERE CLAUSE.
                //       WE'LL FIND OUT WHAT WHEN THE SQL HERE BOMBS OUT.
                whereClause = bulkUpdateRequest.Filter.SQL.Substring(bulkUpdateRequest.Filter.SQL.IndexOf("WHERE"));

                // Shamelessly strip off table identifer.  SINGLE TABLE ---HAS--- TO BE [T1]. TABLE ALIAS.  
                // I SHOULD BE ASHAMED OF THIS, BUT I'M NOT.  GET 'ER DONE.
                whereClause = whereClause.Replace("[T1].", "");
            }
            if (whereClause != null)
            {
                sbBulkUPDATE.AppendLine(whereClause);
            }

            // String it and return it.  Pray for the best.
            bulkUpdateRequest.SQL = sbBulkUPDATE.ToString();


            return (new questStatus(Severity.Success));
        }
        public questStatus PerformBulkUpdate(BulkUpdateRequest bulkUpdateRequest, out int numRows)
        {
            // Initialize
            questStatus status = null;
            numRows = -1;

            try
            {
                // Get database connection string
                TablesetId tablesetId = new TablesetId(bulkUpdateRequest.Filter.TablesetId);
                Tableset tableset = null;
                DbTablesetsMgr dbTablesetsMgr = new DbTablesetsMgr(this.UserSession);
                status = dbTablesetsMgr.Read(tablesetId, out tableset);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }
                DatabaseId databaseId = new DatabaseId(tableset.DatabaseId);
                Quest.Functional.MasterPricing.Database database = null;
                DbDatabasesMgr dbDatabasesMgr = new DbDatabasesMgr(this.UserSession);
                status = dbDatabasesMgr.Read(databaseId, out database);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }

                using (SqlConnection conn = new SqlConnection(database.ConnectionString))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(null, conn))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = bulkUpdateRequest.SQL;
                        try
                        {
                            int retval = cmd.ExecuteNonQuery();
                            if (retval < 0)
                            {
                                return (new questStatus(Severity.Error, String.Format("ERROR: Bulk update failed: {0}", numRows)));
                            }
                            if (retval == 0)
                            {
                                return (new questStatus(Severity.Warning, String.Format("WARNING: {0} rows bulk updated", numRows)));
                            }
                            numRows += retval;
                        }
                        catch (System.Exception ex)
                        {
                            return (new questStatus(Severity.Error, String.Format("EXCEPTION: Bulk update failed: {0}",
                                    ex.Message)));
                        }
                    }
                }
            }
            catch (System.Exception ex)
            {
                return (new questStatus(Severity.Fatal, String.Format("EXCEPTION: {0}.{1}: {2}",
                        this.GetType().Name, MethodBase.GetCurrentMethod().Name,
                        ex.InnerException != null ? ex.InnerException.Message : ex.Message)));
            }
            return (new questStatus(Severity.Success));
        }


        #region Filter Procedure Utility Routines
        //
        // Filter Procedure Utility Routines
        //
        public questStatus GetFilterProcedure(BulkUpdateRequest bulkUpdateRequest, string Action, out FilterProcedure filterProcedure)
        {
            // Initialize
            questStatus status = null;
            filterProcedure = null;


            // Get the filter procedures.
            FilterId filterId = new FilterId(bulkUpdateRequest.FilterId);
            List<FilterProcedure> filterProcedureList = null;
            DbFilterProceduresMgr dbFilterProceduresMgr = new DbFilterProceduresMgr(this.UserSession);
            status = dbFilterProceduresMgr.Read(filterId, out filterProcedureList);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }

            // Determine if given action exists.
            FilterProcedure UPDATEsproc = filterProcedureList.Find(delegate (FilterProcedure fp) { return (fp.Action == Action); });
            if (UPDATEsproc == null)
            {
                return (new questStatus(Severity.Warning, String.Format("No {0} filter procedure", Action)));
            }

            // Get parameters
            FilterProcedureId filterProcedureId = new FilterProcedureId(UPDATEsproc.Id);
            List<FilterProcedureParameter> filterProcedureParameterList = null;
            DbFilterProcedureParametersMgr dbFilterProcedureParametersMgr = new DbFilterProcedureParametersMgr(this.UserSession);
            status = dbFilterProcedureParametersMgr.Read(filterProcedureId, out filterProcedureParameterList);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }

            // Return filter procedure with parameters.
            UPDATEsproc.ParameterList = filterProcedureParameterList;
            filterProcedure = UPDATEsproc;

            return (new questStatus(Severity.Success));
        }
        public questStatus PerformBulkUpdateFilterProcedure(BulkUpdateRequest bulkUpdateRequest, FilterProcedure filterProcedure, ResultsSet resultsSet)
        {
            // Initialize
            questStatus status = null;


            try
            {
                // Get database connection string
                TablesetId tablesetId = new TablesetId(bulkUpdateRequest.Filter.TablesetId);
                Tableset tableset = null;
                DbTablesetsMgr dbTablesetsMgr = new DbTablesetsMgr(this.UserSession);
                status = dbTablesetsMgr.Read(tablesetId, out tableset);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }
                DatabaseId databaseId = new DatabaseId(tableset.DatabaseId);
                Quest.Functional.MasterPricing.Database database = null;
                DbDatabasesMgr dbDatabasesMgr = new DbDatabasesMgr(this.UserSession);
                status = dbDatabasesMgr.Read(databaseId, out database);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }

                // Execute sproc
                bool bTransaction = true;  // Update all rows are none of them.
                using (SqlConnection conn = new SqlConnection(database.ConnectionString))
                {
                    conn.Open();
                    SqlTransaction trans = null;
                    if (bTransaction)
                    {
                        trans = conn.BeginTransaction();
                    }
                    foreach (dynamic _dynRow in resultsSet.Data)
                    {
                        using (SqlCommand cmd = new SqlCommand(null, conn, trans))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.CommandText = filterProcedure.Name;
                            foreach (FilterProcedureParameter filterParam in filterProcedure.ParameterList)
                            {
                                if (filterParam.Direction != "Input")
                                {
                                    SqlParameter sqlParam= new SqlParameter();
                                    if (filterParam.Direction == "ReturnValue")
                                    {
                                        sqlParam.Direction = ParameterDirection.ReturnValue;
                                    }
                                    else if (filterParam.Direction == "Output")
                                    {
                                        sqlParam.Direction = ParameterDirection.Output;
                                    }
                                    else
                                    {
                                        continue; // Input/ouput TODO
                                    }
                                    sqlParam.ParameterName = filterParam.ParameterName;


                                    // TEMPORARY
                                    continue;
                                }

                                // Get the column name from the parameter name
                                FilterItem bulkUpdateFilterItem = bulkUpdateRequest.Filter.FilterItemList.Find(delegate (FilterItem fi)
                                {
                                    return (String.Equals(fi.ParameterName, filterParam.ParameterName, StringComparison.CurrentCultureIgnoreCase));
                                });
                                if (bulkUpdateFilterItem == null)
                                {
                                    trans.Rollback();
                                    return (new questStatus(Severity.Error, String.Format("ERROR: filter item not found for sproc parameter {0}",
                                            filterParam.ParameterName)));
                                }

                                // Get the bulk update value.
                                // NOTE: THIS COULD BE A TROUBLE SPOT.  ORIGINAL REQUIREMENT WAS SINGLE-ENTITY FILTERS ONLY HAD PROCEDURES.  THUS, THOSE FILTER ITEMS
                                //       WOULD NEVER HAVE NAMES QUALIFIED BY THE ENTITY THEY'RE IN.  BUT, FILTERS WITH ENTITIES THAT HAVE NO COLUMNS IN THE FILTER ITEMS 
                                //       TECHNICALLY QUALIFY AS 'SINGLE-ENTITY FILTER'.  THUS, IF THE NAME ALONE DOESN'T MATCH.  GO FOR THE ENTITY_NAME AS A MATCH.
                                BulkUpdateColumnValue bulkUpdateColumnValue = bulkUpdateRequest.Columns.Find(delegate (BulkUpdateColumnValue cv)
                                {
                                    return (cv.Name == bulkUpdateFilterItem.FilterColumn.Name);
                                });
                                if (bulkUpdateColumnValue == null)
                                {
                                    bulkUpdateColumnValue = bulkUpdateRequest.Columns.Find(delegate (BulkUpdateColumnValue cv)
                                    {
                                        string[] parts = cv.Name.Split('_');
                                        if (parts.Length == 2)
                                        {
                                            return (parts[0] == bulkUpdateFilterItem.FilterColumn.ParentEntityType.Name && parts[1] == bulkUpdateFilterItem.FilterColumn.Name);
                                        }
                                        return (false);
                                    });

                                }
                                if (bulkUpdateColumnValue == null)
                                {
                                    return (new questStatus(Severity.Error, String.Format("ERROR: bulk update column value {0} not found in bulk update columns",
                                            bulkUpdateFilterItem.FilterColumn.Name)));
                                }

                                // Determine bulk update value to use.
                                string updateValue = null;
                                if (bulkUpdateColumnValue.bNull)
                                {
                                    updateValue = null;
                                }
                                else if (!string.IsNullOrEmpty(bulkUpdateColumnValue.Value))
                                {
                                    updateValue = bulkUpdateColumnValue.Value;
                                }
                                else if (!filterParam.bRequired)
                                {
                                    updateValue = null;
                                }
                                else  // Value is required, use results value since a value not specified in bulk updates.
                                {
                                    // Indexing not working, but should be ...
                                    ////updateValue = _dynRow[bulkUpdateColumnValue.Name];
                                    bool bFound = false;
                                    foreach (KeyValuePair<string, object> kvp in _dynRow)
                                    {
                                        if (kvp.Key == bulkUpdateColumnValue.Name)
                                        {
                                            updateValue = kvp.Value != null ? kvp.Value.ToString() : null;  // Not sure if we go w/ Null here. But, oh well ...
                                            bFound = true;
                                            break;
                                        }
                                    }
                                    if (!bFound)
                                    {
                                        return (new questStatus(Severity.Error, String.Format("ERROR: filter results column {0} not found to use in bulk update operation",
                                                bulkUpdateColumnValue.Name)));
                                    }
                                }

                                // Bind the parameter
                                // TODO:REFACTOR
                                SqlDbType sqlDbType = (SqlDbType)Enum.Parse(typeof(SqlDbType), filterParam.SqlDbType, true);
                                SqlParameter sqlParameter = new SqlParameter(filterParam.ParameterName, sqlDbType);

                                if (sqlDbType == SqlDbType.Bit)
                                {
                                    bool bValue = updateValue != "0";
                                    sqlParameter.Value = bValue;
                                }
                                else if (sqlDbType == SqlDbType.Int)
                                {
                                    int intValue = Convert.ToInt32(updateValue);
                                    sqlParameter.Value = intValue;
                                }
                                else if (sqlDbType == SqlDbType.NVarChar)
                                {
                                    if (updateValue == null)
                                    {
                                        sqlParameter.Value = DBNull.Value;
                                    }
                                    else
                                    {
                                        sqlParameter.Value = updateValue.ToString();
                                    }
                                }
                                else if (sqlDbType == SqlDbType.VarChar)
                                {
                                    if (updateValue == null)
                                    {
                                        sqlParameter.Value = DBNull.Value;
                                    }
                                    else
                                    {
                                        sqlParameter.Value = updateValue.ToString();
                                    }
                                }
                                else if (sqlDbType == SqlDbType.DateTime)
                                {
                                    if (updateValue == null)
                                    {
                                        sqlParameter.Value = DBNull.Value;
                                    }
                                    else
                                    {
                                        sqlParameter.Value = Convert.ToDateTime(updateValue);
                                    }
                                }
                                else if (sqlDbType == SqlDbType.DateTime2)
                                {
                                    if (updateValue == null)
                                    {
                                        sqlParameter.Value = DBNull.Value;
                                    }
                                    else
                                    {
                                        sqlParameter.Value = Convert.ToDateTime(updateValue);
                                    }
                                }
                                else if (sqlDbType == SqlDbType.Date)
                                {
                                    if (updateValue == null)
                                    {
                                        sqlParameter.Value = DBNull.Value;
                                    }
                                    else
                                    {
                                        sqlParameter.Value = Convert.ToDateTime(updateValue);
                                    }
                                }
                                else if (sqlDbType == SqlDbType.Decimal)
                                {
                                    if (updateValue == null)
                                    {
                                        sqlParameter.Value = DBNull.Value;
                                    }
                                    else
                                    {
                                        sqlParameter.Value = Convert.ToDecimal(updateValue);
                                    }
                                }
                                else
                                {
                                    if (updateValue == null)
                                    {
                                        sqlParameter.Value = DBNull.Value;
                                    }
                                    else
                                    {
                                        sqlParameter.Value = updateValue;
                                    }
                                }
                                cmd.Parameters.Add(sqlParameter);
                            }
                            // Execute the command
                            try
                            {
                                int numRows = cmd.ExecuteNonQuery();
                                if (numRows < 1)
                                {
                                    return (new questStatus(Severity.Error, String.Format("ERROR: Bulk update stored procedure failed: Rows: {0}", numRows)));
                                }
                            }
                            catch (SqlException ex)
                            {
                                return (new questStatus(Severity.Error, String.Format("SQL EXCEPTION: Bulk update stored procedure {0}: {1}",
                                        filterProcedure.Name, ex.Message)));
                            }
                            catch (System.Exception ex)
                            {
                                return (new questStatus(Severity.Error, String.Format("EXCEPTION: Bulk update stored procedure {0}: {1}",
                                        filterProcedure.Name, ex.Message)));
                            }
                        }
                    }
                    if (bTransaction)
                    {
                        trans.Commit();
                    }
                }
            }
            catch (System.Exception ex)
            {
                return (new questStatus(Severity.Fatal, String.Format("EXCEPTION: Bulk Update Operation: {0}.{1}: {2}",
                        this.GetType().Name, MethodBase.GetCurrentMethod().Name,
                        ex.InnerException != null ? ex.InnerException.Message : ex.Message)));
            }
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
        private questStatus getFilterColumn(BulkUpdateColumnValue bulkUpdateColumnValue, Filter filter, int numFROMEntities)
        {
            // Initialize
            string[] pp = null;
            string entityName = null;
            string columnName = null;
            FilterColumn filterColumn = null;


            if (filter.FilterTableList.Count == 1)
            {
                entityName = filter.FilterTableList[0].TablesetTable.Table.Name;
                columnName = bulkUpdateColumnValue.Name;
            }
            else
            {
                pp = bulkUpdateColumnValue.Name.Split('_');
                entityName = pp[0];
                columnName = pp[1];
                filterColumn = null;
            }




            // Get FilterColumn of given bulk update column.
            FilterTable filterTable = filter.FilterTableList.Find(delegate (FilterTable t) { return t.TablesetTable.Table.Name == entityName; });
            if (filterTable != null)
            {
                filterColumn = filter.FilterColumnList.Find(delegate (FilterColumn c) {
                    return c.TablesetColumn.Column.Name == columnName && c.TablesetColumn.Column.EntityTypeId == EntityType.Table && c.TablesetColumn.Column.EntityId == filterTable.TablesetTable.Table.Id;
                });
                if (filterColumn == null)
                {
                    return (new questStatus(Severity.Error, String.Format("ERROR: filter column not found for bulk update column {0}",
                            bulkUpdateColumnValue.Name)));
                }
            }
            else
            {
                FilterView filterView = filter.FilterViewList.Find(delegate (FilterView v) { return v.TablesetView.View.Name == entityName; });
                if (filterView == null)
                {
                    return (new questStatus(Severity.Error, String.Format("ERROR: filter view not found for bulk update column {0}",
                            bulkUpdateColumnValue.Name)));
                }
                filterColumn = filter.FilterColumnList.Find(delegate (FilterColumn c) {
                    return c.TablesetColumn.Column.Name == columnName && c.TablesetColumn.Column.EntityTypeId == EntityType.View && c.TablesetColumn.Column.EntityId == filterView.TablesetView.View.Id;
                });
                if (filterColumn == null)
                {
                    return (new questStatus(Severity.Error, String.Format("ERROR: filter column not found for bulk update column {0} (view search)",
                            bulkUpdateColumnValue.Name)));
                }
            }
            bulkUpdateColumnValue.FilterColumn = filterColumn;

            return (new questStatus(Severity.Success));
        }
        private string getValueIdentifier(BulkUpdateColumnValue bulkUpdateColumnValue)
        {
            string valueIdentifier = null;
            bool bUseQuotes = false;


            // Figure out if quotes should be used.
            if (bulkUpdateColumnValue.FilterColumn.TablesetColumn.Column.DataTypeName.Contains("char"))
            {
                bUseQuotes = true;
            }
            else if (bulkUpdateColumnValue.FilterColumn.TablesetColumn.Column.DataTypeName.Contains("date"))
            {
                bUseQuotes = true;
            }
            else if (bulkUpdateColumnValue.FilterColumn.TablesetColumn.Column.DataTypeName.Contains("time"))
            {
                bUseQuotes = true;
            }

            // Build value assignment.
            if (bUseQuotes)
            {
                valueIdentifier = String.Format("'{0}'", bulkUpdateColumnValue.Value);
            }
            else
            {
                valueIdentifier = String.Format("{0}", bulkUpdateColumnValue.Value);
            }
            return (valueIdentifier);
        }
        #endregion
    }
}