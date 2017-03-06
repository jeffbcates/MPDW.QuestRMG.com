using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Dynamic;
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
using Quest.MasterPricing.Services.Data.Database;


namespace Quest.MasterPricing.Services.Data.Filters
{
    public class DbResultsMgr : DbMgr
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
        public DbResultsMgr()
            : base()
        {
            initialize();
        }
        public DbResultsMgr(UserSession userSession)
            : base()
        {
            _userSession = userSession;
            initialize();
        }
        #endregion


        #region Properties
        /*==================================================================================================================================
         * Properties
         *=================================================================================================================================*/
        public UserSession UserSession
        {
            get
            {
                return (this._userSession);
            }
        }
        #endregion


        #region Public Methods
        /*==================================================================================================================================
         * Public Methods
         *=================================================================================================================================*/
        public questStatus ExecuteFilter(RunFilterRequest runFilterRequest, out ResultsSet resultsSet) // 
        {
            // Initialize
            questStatus status = null;
            resultsSet = null;


            // Read the filter
            FilterId filterId = new FilterId(runFilterRequest.FilterId.Id);
            Filter filter = null;
            DbFilterMgr dbFilterMgr = new DbFilterMgr(this.UserSession);
            status = dbFilterMgr.GetFilter(filterId, out filter);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }

            // If no SQL, return.
            if (string.IsNullOrEmpty(filter.SQL))
            {
                return (new questStatus(Severity.Error, "Filter has no SQL"));
            }


            // Read the tableset
            TablesetId tablesetId = new TablesetId(filter.TablesetId);
            Tableset tableset = null;
            DbTablesetsMgr dbTablesetsMgr = new DbTablesetsMgr(this.UserSession);
            status = dbTablesetsMgr.Read(tablesetId, out tableset);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }

            // Read the database
            DatabaseId databaseId = new DatabaseId(tableset.DatabaseId);
            Quest.Functional.MasterPricing.Database database = null;
            DbDatabasesMgr dbDatabasesMgr = new DbDatabasesMgr(this.UserSession);
            status = dbDatabasesMgr.Read(databaseId, out database);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }

            // Execute SQL.
            try {
                status = executeSQL(runFilterRequest, database, filter, out resultsSet);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }
            }
            catch (System.Exception ex)
            {
                return (new questStatus(Severity.Error, String.Format("EXCEPTION: executing filter SQL: {0}", ex.Message)));
            }

            // Append lookup or type list Id's to result columns with lookups.
            // NOTE: Lookups and typeList are mutually exclusive.
            FilterItem filterItem = null;
            try {
                for (int idx=0; idx < filter.FilterItemList.Count; idx += 1)
                {
                    filterItem = filter.FilterItemList[idx];

                    string columnIdentifier = null;
                    FilterColumn filterColumn = null;
                    status = GetResultsColumnIdentifier(filter, filterItem, out columnIdentifier, out filterColumn);
                    if (!questStatusDef.IsSuccess(status))
                    {
                        return (status);
                    }
                    if (columnIdentifier == null)
                    {
                        return (new questStatus(Severity.Error, String.Format("ERROR: columnIdentifier is NULL for filterItem {0}  FilterId: {1}",
                                filterItem.Id, filterItem.FilterId)));
                    }
                    if (filterColumn == null)
                    {
                        return (new questStatus(Severity.Error, String.Format("ERROR: filterColumn is NULL for filterItem {0}  FilterId: {1}",
                                filterItem.Id, filterItem.FilterId)));
                    }
                    if (! string.IsNullOrEmpty(filterItem.Label))
                    {
                        resultsSet.ResultColumns[columnIdentifier].Name = filterColumn.TablesetColumn.Name;
                        resultsSet.ResultColumns[columnIdentifier].Label = filterItem.Label;
                    }
                    if (filterItem.LookupId.HasValue)
                    {
                        resultsSet.ResultColumns[columnIdentifier].LookupId = filterItem.LookupId;
                    }
                    if (filterItem.TypeListId.HasValue)
                    {
                        resultsSet.ResultColumns[columnIdentifier].TypeListId = filterItem.TypeListId;
                    }
                    resultsSet.ResultColumns[columnIdentifier].bIsHidden = filterItem.bHidden;
                }
            }
            catch (System.Exception ex)
            {
                return (new questStatus(Severity.Error, String.Format("EXCEPTION: building filter results set for FilterItem {0}: {1}", 
                        filterItem.Id, ex.Message)));
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus GetResultsColumnIdentifier(Filter filter, FilterItem filterItem, out string columnIdentifier, out FilterColumn filterColumn)
        {
            // Initialize
            columnIdentifier = null;
            filterColumn = null;
            int numEntities = filter.FilterTableList.Count + filter.FilterViewList.Count;


            FilterColumn _filterColumn = filterItem.FilterColumn;
            if (_filterColumn == null)
            {
                return (new questStatus(Severity.Error, String.Format("ERROR: filter item {0} filter column not found in filter (building lookups)", filterItem.FilterEntityId)));
            }
            filterColumn = _filterColumn;

            // If column labeled, that's it.
            if (!string.IsNullOrEmpty(filterItem.Label))
            {
                columnIdentifier = filterItem.Label;
                return (new questStatus(Severity.Success));
            }

            // Determine column identifier.
            // Single-entity filters, it's the column name alone.
            // More than one entity, it's the entity name  + "_" +  column name.
            if (numEntities == 1)
            {
                columnIdentifier = _filterColumn.TablesetColumn.Column.Name;
            }
            else {
                if (_filterColumn.FilterEntityTypeId == FilterEntityType.Table)
                {
                    FilterTable filterTable = filter.FilterTableList.Find(delegate (FilterTable t) { return (t.Id == _filterColumn.FilterEntityId); });
                    columnIdentifier = filterTable.TablesetTable.Table.Name + "_" + _filterColumn.TablesetColumn.Column.Name;
                }
                else if (_filterColumn.FilterEntityTypeId == FilterEntityType.View)
                {
                    FilterView filterView = filter.FilterViewList.Find(delegate (FilterView v) { return (v.Id == _filterColumn.FilterEntityId); });
                    columnIdentifier = filterView.TablesetView.View.Name + "_" + _filterColumn.TablesetColumn.Column.Name;
                }
                else
                {
                    return (new questStatus(Severity.Error, String.Format("ERROR: invalid filter column {0} entity type id: {1} (determining column identifier)",
                            _filterColumn.FilterEntityId, _filterColumn.FilterEntityTypeId)));
                }
            }
            return (new questStatus(Severity.Success));
        }
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

        #region SQL Execution
        //
        //  SQL Execution
        //
        private questStatus executeSQL(RunFilterRequest runFilterRequest, Quest.Functional.MasterPricing.Database database, Filter filter, out ResultsSet resultSet)
        {
            // Initialize
            questStatus status = null;
            resultSet = null;
            FilterId filterId = new FilterId(filter.Id);


            // Execute SQL
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(database.ConnectionString))
                {
                    sqlConnection.Open();
                    string sql = filter.SQL;

                    // Apply run requests
                    // TODO: TEMPORARY
                    if (runFilterRequest != null)
                    {
                        if (! string.IsNullOrEmpty(runFilterRequest.RowLimit))
                        {
                            var _sql = sql.Replace("SELECT DISTINCT", "SELECT DISTINCT TOP " + runFilterRequest.RowLimit);
                            sql = _sql;
                        }
                    }

                    using (SqlCommand cmd = sqlConnection.CreateCommand())
                    {
                        cmd.CommandText = sql;
                        cmd.CommandType = CommandType.Text;
                        using (SqlDataReader rdr = cmd.ExecuteReader())
                        {
                            resultSet = new ResultsSet(filterId);
                            Dictionary<string, Column> dynamicType = null;
                            status = BuildType(rdr, out dynamicType);
                            if (!questStatusDef.IsSuccess(status))
                            {
                                return (status);
                            }
                            resultSet.ResultColumns = dynamicType;
                            while (rdr.Read())
                            {
                                dynamic resultRow = null;
                                status = GetRow(rdr, out resultRow);
                                if (!questStatusDef.IsSuccess(status))
                                {
                                    return (status);
                                }
                                resultSet.Data.Add(resultRow);
                            }
                        }
                    }
                }
            }
            catch (System.Exception ex)
            {
                return (new questStatus(Severity.Fatal, String.Format("EXCEPTION: executing filter {0} SQL: {1}",
                    filter.Name, ex.Message)));
            }
            return (new questStatus(Severity.Success));
        }
        private questStatus executeSQLWithPaging(RunFilterRequest runFilterRequest, Quest.Functional.MasterPricing.Database database, Filter filter, out ResultsSet resultSet)
        {
            // Initialize
            questStatus status = null;
            resultSet = null;
            FilterId filterId = new FilterId(filter.Id);


            // Execute SQL
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(database.ConnectionString))
                {
                    sqlConnection.Open();
                    string sql = filter.SQL;


                    // Wrap SQL in paging context
                    if (runFilterRequest != null)
                    {
                        if (!string.IsNullOrEmpty(runFilterRequest.RowLimit))
                        {
                            var _sql = sql.Replace("SELECT DISTINCT", "SELECT DISTINCT TOP " + runFilterRequest.RowLimit);
                            sql = _sql;
                        }

                        ////// WORKING SQL EXAMPLE
                        ////SELECT  *
                        ////FROM    (SELECT    ROW_NUMBER() OVER(ORDER BY[T1].[UOM] ASC) AS RowNum, [T1].[Id] AS 'Id', [T1].[Type] AS 'Type', [T1].[UOM] AS 'UOM', [T1].[Size] AS 'Size', [T1].[StartDate] AS 'StartDate', [T1].[EndDate] AS 'EndDate', [T1].[CreateDate] AS 'CreateDate', [T1].[CreateUser] AS 'CreateUser', [T1].[UpdateDate] AS 'UpdateDate', [T1].[UpdateUser] AS 'UpdateUser', [T1].[Material] AS 'Material', [T1].[DisplayName] AS 'DisplayName', [T1].[MaterialUOM] AS 'MaterialUOM', [T1].[NetSuiteEquipId] AS 'NetSuiteEquipId'
                        
                        ////          FROM[dbo].[QuestEquipment] T1
                        ////          ----WHERE     OrderDate >= '1980-01-01'
                        ////        ) AS RowConstrainedResult
                        ////WHERE RowNum >= 1
                        ////    AND RowNum <= 20
                        ////ORDER BY RowNum


                    }

                    using (SqlCommand cmd = sqlConnection.CreateCommand())
                    {
                        cmd.CommandText = sql;
                        cmd.CommandType = CommandType.Text;
                        using (SqlDataReader rdr = cmd.ExecuteReader())
                        {
                            resultSet = new ResultsSet(filterId);
                            Dictionary<string, Column> dynamicType = null;
                            status = BuildType(rdr, out dynamicType);
                            if (!questStatusDef.IsSuccess(status))
                            {
                                return (status);
                            }
                            resultSet.ResultColumns = dynamicType;
                            while (rdr.Read())
                            {
                                dynamic resultRow = null;
                                status = GetRow(rdr, out resultRow);
                                if (!questStatusDef.IsSuccess(status))
                                {
                                    return (status);
                                }
                                resultSet.Data.Add(resultRow);
                            }
                        }
                    }
                }
            }
            catch (System.Exception ex)
            {
                return (new questStatus(Severity.Fatal, String.Format("EXCEPTION: executing filter {0} SQL: {1}",
                    filter.Name, ex.Message)));
            }
            return (new questStatus(Severity.Success));
        }
        #endregion

        #endregion
    }
}