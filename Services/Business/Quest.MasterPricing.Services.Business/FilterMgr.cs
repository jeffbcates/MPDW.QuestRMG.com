using System;
using System.IO;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Reflection;
using System.Threading.Tasks;
using Quest.Util.Status;
using Quest.Util.Buffer;
using Quest.Util.Data;
using Quest.Functional.ASM;
using Quest.Functional.FMS;
using Quest.Functional.MasterPricing;
using Quest.MPDW.Services.Data;
using Quest.MPDW.Services.Business;
using Quest.MasterPricing.Services.Business.Database;
using Quest.MasterPricing.Services.Data.Filters;
using Quest.MasterPricing.Services.Data.Database;


namespace Quest.MasterPricing.Services.Business.Filters
{
    public class FilterMgr : Mgr
    {
        #region Declarations
        /*==================================================================================================================================
         * Declarations
         *=================================================================================================================================*/
        private DbFilterMgr _dbFilterMgr = null;
        private UserSession _userSession = null;

        #endregion


        #region Constructors
        /*==================================================================================================================================
         * Constructors
         *=================================================================================================================================*/
        public FilterMgr()
            : base()
        {
            initialize();
        }
        public FilterMgr(UserSession userSession)
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
        public questStatus Save(FilterId filterId, Filter filter)
        {
            // Initialize 
            questStatus status = null;
            Mgr mgr = new Mgr();
            DbMgrTransaction trans = null;
            ColumnsMgr columnMgr = new ColumnsMgr(this.UserSession);


            try
            {
                // Validate filter
                status = Verify(filterId, filter);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }
         

                // BEGIN TRANSACTION
                status = mgr.BeginTransaction("SaveFilter" + Guid.NewGuid().ToString(), out trans);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }


                // Remove filter entities, items and values.  Also, tables and columns.
                status = _dbFilterMgr.Clear(trans, filterId);
                if (!questStatusDef.IsSuccess(status))
                {
                    mgr.RollbackTransaction(trans);
                    return (status);
                }

                // Save filter tables
                FilterTablesMgr filterTablesMgr = new FilterTablesMgr(this.UserSession);
                foreach (FilterTable filterTable in filter.FilterTableList)
                {
                    filterTable.FilterId = filter.Id;
                    FilterTableId filterTableId = null;
                    status = filterTablesMgr.Create(trans, filterTable, out filterTableId);
                    if (!questStatusDef.IsSuccess(status))
                    {
                        mgr.RollbackTransaction(trans);
                        return (status);
                    }
                    filterTable.Id = filterTableId.Id;
                }

                // Save filter views
                FilterViewsMgr filterViewsMgr = new FilterViewsMgr(this.UserSession);
                foreach (FilterView _filterView in filter.FilterViewList)
                {
                    _filterView.FilterId = filter.Id;
                    FilterViewId filterViewId = null;
                    status = filterViewsMgr.Create(trans, _filterView, out filterViewId);
                    if (!questStatusDef.IsSuccess(status))
                    {
                        mgr.RollbackTransaction(trans);
                        return (status);
                    }
                    _filterView.Id = filterViewId.Id;
                }

                // Save filter columns
                DbFilterMgr dbFilterMgr = new DbFilterMgr(this.UserSession);
                FilterColumnsMgr filterColumnsMgr = new FilterColumnsMgr(this.UserSession);
                foreach (FilterColumn filterColumn in filter.FilterColumnList)
                {
                    if (filterColumn.FilterEntityId < BaseId.VALID_ID)
                    {
                        // Get the FilterTable or FilterView Id.
                        // This is a klugie.  Gotta rework DataManager panel first, though.  No time to do that.
                        if (filterColumn.FilterEntityTypeId == FilterEntityType.Table)
                        {
                            TablesetTable tablesetTable = null;
                            status = dbFilterMgr.GetTablesetTable(filterColumn, out tablesetTable);
                            if (!questStatusDef.IsSuccess(status))
                            {
                                mgr.RollbackTransaction(trans);
                                return (status);
                            }
                            FilterTable filterTable = filter.FilterTableList.Find(delegate (FilterTable t) { return (t.Schema == tablesetTable.Schema && t.Name == tablesetTable.Name); });
                            if (filterTable == null)
                            {
                                return (new questStatus(Severity.Error, String.Format("ERROR: FilterTable not found for TableSetTable Schema:{0} Name:{1}",
                                        tablesetTable.Schema, tablesetTable.Name)));
                            }
                            filterColumn.FilterEntityId = filterTable.Id;
                        }
                        else if (filterColumn.FilterEntityTypeId == FilterEntityType.View)
                        {
                            TablesetView tablesetView = null;
                            status = dbFilterMgr.GetTablesetView(filterColumn, out tablesetView);
                            if (!questStatusDef.IsSuccess(status))
                            {
                                mgr.RollbackTransaction(trans);
                                return (status);
                            }
                            FilterView filterView = filter.FilterViewList.Find(delegate (FilterView v) { return (v.Schema == tablesetView.Schema && v.Name == tablesetView.Name); });
                            if (filterView == null)
                            {
                                return (new questStatus(Severity.Error, String.Format("ERROR: FilterView not found for TableSetTable Schema:{0} Name:{1}",
                                        tablesetView.Schema, tablesetView.Name)));
                            }
                            filterColumn.FilterEntityId = filterView.Id;
                        }
                        else
                        {
                            return (new questStatus(Severity.Error, String.Format("ERROR: FilterColumn FilterEntityTypeId {0} not supported.  FilterColumn {1}",
                                    filterColumn.FilterEntityTypeId, filterColumn.Id)));
                        }
                    }

                    // Save
                    FilterColumnId filterColumnId = null;
                    status = filterColumnsMgr.Create(trans, filterColumn, out filterColumnId);
                    if (!questStatusDef.IsSuccess(status))
                    {
                        mgr.RollbackTransaction(trans);
                        return (status);
                    }
                    filterColumn.Id = filterColumnId.Id;
                }


                // Save filter items.
                FilterItemsMgr filterItemsMgr = new FilterItemsMgr(this.UserSession);
                FilterItemJoinsMgr filterItemJoinsMgr = new FilterItemJoinsMgr(this.UserSession);
                FilterOperationsMgr filterOperationsMgr = new FilterOperationsMgr(this.UserSession);
                FilterValuesMgr filterValuesMgr = new FilterValuesMgr(this.UserSession);
                foreach (FilterItem filterItem in filter.FilterItemList)
                {
                    FilterColumn filterColumn = null;

                    // Get filterItem FilterEntityId
                    if (filterItem.FilterEntityId < BaseId.VALID_ID)
                    {
                        status = dbFilterMgr.GetFilterColumn(filter, filterItem, out filterColumn);
                        if (!questStatusDef.IsSuccess(status))
                        {
                            mgr.RollbackTransaction(trans);
                            return (status);
                        }
                        filterItem.FilterEntityId = filterColumn.Id;
                        filterItem.FilterColumn = filterColumn;        // helps with bookkeeping further down.
                    }

                    // Save FilterItem
                    FilterItemId filterItemId = null;
                    status = filterItemsMgr.Create(trans, filterItem, out filterItemId);
                    if (!questStatusDef.IsSuccess(status))
                    {
                        mgr.RollbackTransaction(trans);
                        return (status);
                    }
                    filterItem.Id = filterItemId.Id;


                    //  Save filter item joins
                    foreach (FilterItemJoin filterItemJoin in filterItem.JoinList)
                    {
                        filterItemJoin.FilterItemId = filterItemId.Id;


                        if (string.IsNullOrEmpty(filterItemJoin.SourceSchema) || string.IsNullOrEmpty(filterItemJoin.SourceEntityName) || string.IsNullOrEmpty(filterItemJoin.SourceColumnName))
                        {

                            // Get join target type (Table or View)
                            TablesetColumnId tablesetColumnId = new TablesetColumnId(filterItemJoin.ColumnId);
                            TablesetTable tablesetTable = null;
                            TablesetView tablesetView = null;
                            status = getJoinTarget(tablesetColumnId, out tablesetTable, out tablesetView);
                            if (!questStatusDef.IsSuccess(status))
                            {
                                mgr.RollbackTransaction(trans);
                                return (status);
                            }
                            filterItemJoin.TargetEntityTypeId = tablesetTable == null ? FilterEntityType.View : FilterEntityType.Table;

                            // Get target identifier parts.
                            string targetSchema = null;
                            string targetEntityName = null;
                            string targetColumnName = null;
                            SQLIdentifier.ParseThreePartIdentifier(filterItemJoin.Identifier, out targetSchema, out targetEntityName, out targetColumnName);
                            filterItemJoin.TargetSchema = targetSchema;
                            filterItemJoin.TargetEntityName = targetEntityName;
                            filterItemJoin.TargetColumnName = targetColumnName;


                            // Get source identifier parts.
                            string sourceSchema = null;
                            string sourceEntityName = null;
                            string sourceColumnName = null;
                            if (filterColumn.FilterEntityTypeId == FilterEntityType.Table)
                            {
                                FilterTable filterTable = filter.FilterTableList.Find(delegate (FilterTable t) { return (filterColumn.FilterEntityId == t.Id); });
                                if (filterTable == null)
                                {
                                    mgr.RollbackTransaction(trans);
                                    return (new questStatus(Severity.Error, String.Format("ERROR: building Join on {0}, FilterTable {1} not found",
                                            filterItemJoin.Identifier, filterColumn.FilterEntityId)));
                                }
                                sourceSchema = filterTable.Schema;
                                sourceEntityName = filterTable.Name;
                            }
                            else if (filterColumn.FilterEntityTypeId == FilterEntityType.View)
                            {
                                FilterView filterView = filter.FilterViewList.Find(delegate (FilterView v) { return (filterColumn.FilterEntityId == v.Id); });
                                if (filterView == null)
                                {
                                    mgr.RollbackTransaction(trans);
                                    return (new questStatus(Severity.Error, String.Format("ERROR: building Join on {0}, FilterView {1} not found",
                                            filterItemJoin.Identifier, filterColumn.FilterEntityId)));
                                }
                                sourceSchema = filterView.Schema;
                                sourceEntityName = filterView.Name;
                            }
                            sourceColumnName = filterColumn.Name;
                            filterItemJoin.SourceSchema = sourceSchema;
                            filterItemJoin.SourceEntityName = sourceEntityName;
                            filterItemJoin.SourceColumnName = sourceColumnName;
                        }

                        // Create FilterItemJoin
                        FilterItemJoinId filterItemJoinId = null;
                        status = filterItemJoinsMgr.Create(trans, filterItemJoin, out filterItemJoinId);
                        if (!questStatusDef.IsSuccess(status))
                        {
                            mgr.RollbackTransaction(trans);
                            return (status);
                        }
                    }

                    // Save filter operations
                    foreach (FilterOperation filterOperation in filterItem.OperationList)
                    {
                        filterOperation.FilterItemId = filterItem.Id;

                        FilterOperationId filterOperationId = null;
                        status = filterOperationsMgr.Create(trans, filterOperation, out filterOperationId);
                        if (!questStatusDef.IsSuccess(status))
                        {
                            mgr.RollbackTransaction(trans);
                            return (status);
                        }

                        // Save filter values
                        foreach (FilterValue filterValue in filterOperation.ValueList)
                        {
                            filterValue.FilterOperationId = filterOperationId.Id;
                            FilterValueId filterValueId = null;
                            status = filterValuesMgr.Create(trans, filterValue, out filterValueId);
                            if (!questStatusDef.IsSuccess(status))
                            {
                                mgr.RollbackTransaction(trans);
                                return (status);
                            }
                        }
                    }
                }

                // Save filter procedures
                //     Get the filter's database.
                Quest.Functional.MasterPricing.Database database = null;
                status = GetFilterDatabase(filterId, out database);
                if (!questStatusDef.IsSuccess(status))
                {
                    mgr.RollbackTransaction(trans);
                    return (status);
                }
                // NOTE: business rules reads in procedure parameters and writes them.
                FilterProceduresMgr filterProceduresMgr = new FilterProceduresMgr(this.UserSession);
                foreach (FilterProcedure filterProcedure in filter.FilterProcedureList)
                {
                    if(string.IsNullOrEmpty(filterProcedure.Name))
                    {
                        continue;
                    }
                    filterProcedure.FilterId = filter.Id;
                    FilterProcedureId filterProcedureId = null;
                    status = filterProceduresMgr.Create(trans, database, filterProcedure, out filterProcedureId);
                    if (!questStatusDef.IsSuccess(status))
                    {
                        mgr.RollbackTransaction(trans);
                        return (status);
                    }
                }


                // COMMIT TRANSACTION
                status = mgr.CommitTransaction(trans);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }

                // Generate filter SQL
                Filter filterWithSQL = null;
                status = dbFilterMgr.GenerateFilterSQL(filterId, out filterWithSQL);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }

                // Update filter
                FiltersMgr filtersMgr = new FiltersMgr(this.UserSession);
                status = filtersMgr.Update(filterWithSQL);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }
            }
            catch (System.Exception ex)
            {
                if (trans != null)
                {
                    mgr.RollbackTransaction(trans);
                }
                return (new questStatus(Severity.Fatal, String.Format("EXCEPTION: {0}.{1}: {2}",
                        this.GetType().Name, MethodBase.GetCurrentMethod().Name,
                        ex.InnerException != null ? ex.InnerException.Message : ex.Message)));
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Verify(FilterId filterId, Filter filter)
        {
            // Initialize
            questStatus status = null;


            // Verify filter Id
            Filter _filter = null;
            FiltersMgr filtersMgr = new FiltersMgr(this.UserSession);
            status = filtersMgr.Read(filterId, out _filter);
            if (!questStatusDef.IsSuccess(status))
            {
                return (new questStatus(Severity.Error, String.Format("Error reading FilterId {0} not found: {0}", 
                        filterId.Id, status.Message)));
            }

            // Verify tableset Id
            TablesetId tablesetId = new TablesetId(filter.TablesetId);
            Tableset _tableset = null;
            TablesetsMgr tablesetsMgr = new TablesetsMgr(this.UserSession);
            status = tablesetsMgr.Read(tablesetId, out _tableset);
            if (!questStatusDef.IsSuccess(status))
            {
                return (new questStatus(Severity.Error, String.Format("Error reading TablesetId not found: {0}", 
                        tablesetId, status.Message)));
            }


            // Make shift validation for now.
            status = Verify(filter);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Verify(Filter filter)
        {
            // Make shift validation for now.
            for (int fiidx = 0; fiidx < filter.FilterItemList.Count; fiidx += 1)
            {
                FilterItem filterItem = filter.FilterItemList[fiidx];
                for (int foidx = 0; foidx < filterItem.OperationList.Count; foidx += 1)
                {
                    FilterOperation filterOperation = filterItem.OperationList[foidx];
                    if (filterOperation.FilterOperatorId >= BaseId.VALID_ID)
                    {
                        if (filterOperation.ValueList.Count == 0)
                        {
                            return (new questStatus(Severity.Error, String.Format("ERROR: FilterOperation #{0} has no values",
                                    (fiidx + 1))));
                        }
                    }
                }
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Run(RunFilterRequest runFilterRequest, Filter filter, out ResultsSet resultsSet)
        {
            // Initialize
            questStatus status = null;
            resultsSet = null;


            // Run the filter
            DbResultsMgr dbResultsMgr = new DbResultsMgr(this.UserSession);
            status = dbResultsMgr.Run(runFilterRequest, filter, out resultsSet);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus GetFilter(FilterId filterId, out Filter filter)
        {
            // Initialize 
            questStatus status = null;
            filter = null;


            status = _dbFilterMgr.GetFilter(filterId, out filter);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Clear(FilterId filterId)
        {
            return (_dbFilterMgr.Clear(filterId));
        }
        public questStatus GenerateFilterSQL(FilterId filterId, out Filter filter)
        {
            // TODO: business rules about what can/cannot be queried.


            // Generate SQL.
            return (_dbFilterMgr.GenerateFilterSQL(filterId, out filter));
        }
        public questStatus GenerateFilterSQL(Filter filter, out Filter filterWithSQL)
        {
            // TODO: business rules about what can/cannot be queried.


            // Generate SQL.
            return (_dbFilterMgr.GenerateFilterSQL(filter, out filterWithSQL));
        }

        public questStatus SaveSQL(Filter filter)
        {
            // TODO: business rules about what can/cannot be saved.


            // Generate SQL.
            return (_dbFilterMgr.SaveSQL(filter));
        }
        public questStatus ExecuteFilter(RunFilterRequest runFilterRequest, out ResultsSet resultsSet)
        {
            // TODO: business rules about what can/cannot be saved.

            // Initialize
            resultsSet = null;
            DbResultsMgr dbResultsMgr = new DbResultsMgr(this.UserSession);


            // Generate SQL.

            return (dbResultsMgr.ExecuteFilter(runFilterRequest, out resultsSet));
        }
        public questStatus GetFilterProcedures(FilterId filterId, out List<FilterProcedure> filterProcedureList)
        {
            // Initialize
            questStatus status = null;
            filterProcedureList = null;


            DbFilterProceduresMgr dbFilterProceduresMgr = new DbFilterProceduresMgr(this.UserSession);
            status = dbFilterProceduresMgr.Read(filterId, out filterProcedureList);
            if (!questStatusDef.IsSuccessOrWarning(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus GetDatabaseStoredProcedures(TablesetId tablesetId, out List<StoredProcedure> storedProcedureList)
        {
            // Initialize
            questStatus status = null;
            storedProcedureList = null;


            // Get tableset
            Tableset tableset = null;
            TablesetsMgr tablesetsMgr = new TablesetsMgr(this.UserSession);
            status = tablesetsMgr.Read(tablesetId, out tableset);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }


            // Get database
            Quest.Functional.MasterPricing.Database database = null;
            DatabaseId databaseId = new DatabaseId(tableset.DatabaseId);
            DatabasesMgr databasesMgr = new DatabasesMgr(this.UserSession);
            status = databasesMgr.Read(databaseId, out database);
            if (!questStatusDef.IsSuccessOrWarning(status))
            {
                return (status);
            }


            // Get database stored procedures
            DbFilterProceduresMgr dbFilterProceduresMgr = new DbFilterProceduresMgr(this.UserSession);
            status = GetDatabaseStoredProcedures(database, out storedProcedureList);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus GetFilterDatabase(Quest.Functional.MasterPricing.FilterId filterId, out Quest.Functional.MasterPricing.Database database)
        {
            // Initialize
            questStatus status = null;
            database = null;


            // Get filter
            Filter filter = null;
            FiltersMgr filtersMgr = new FiltersMgr(this.UserSession);
            status = filtersMgr.Read(filterId, out filter);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }


            // Get tableset
            Tableset tableset = null;
            TablesetId tablesetId = new TablesetId(filter.TablesetId);
            TablesetsMgr tablesetsMgr = new TablesetsMgr(this.UserSession);
            status = tablesetsMgr.Read(tablesetId, out tableset);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }


            // Get database
            DatabaseId databaseId = new DatabaseId(tableset.DatabaseId);
            DatabasesMgr databasesMgr = new DatabasesMgr(this.UserSession);
            status = databasesMgr.Read(databaseId, out database);
            if (!questStatusDef.IsSuccessOrWarning(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        
        public questStatus Read(DbMgrTransaction trans, TablesetId tablesetId, out List<Filter> filterList)
        {
            // Initialize
            questStatus status = null;
            filterList = null;

            // Get all filters for this tableset.
            DbFilterMgr dbFilterMgr = new DbFilterMgr(this.UserSession);
            status = dbFilterMgr.Read(trans, tablesetId, out filterList);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Delete(DbMgrTransaction trans, FilterId filterId)
        {
            // Initialize
            questStatus status = null;

            // TODO: DELETE ALL STUFF WITH A FILTER.
            DbFilterMgr dbFilterMgr = new DbFilterMgr(this.UserSession);
            status = dbFilterMgr.Delete(trans, filterId);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }

        public questStatus Copy(FilterId filterId, out FilterId newFilterId)
        {
            // Initialize
            questStatus status = null;
            newFilterId = null;


            DbFilterMgr dbFilterMgr = new DbFilterMgr(this.UserSession);
            status = dbFilterMgr.Copy(filterId, out newFilterId);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
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
                _dbFilterMgr = new DbFilterMgr();
            }
            catch (System.Exception ex)
            {
                status = new questStatus(Severity.Fatal, String.Format("EXCEPTION: {0}.{1}: {2}",
                        this.GetType().ToString(), MethodInfo.GetCurrentMethod().Name, ex.Message));
                throw new System.Exception(status.Message, ex);
            }
            return (new questStatus(Severity.Success));
        }
        private questStatus getJoinTarget(TablesetColumnId tablesetColumnId, out TablesetTable tablesetTable, out TablesetView tablesetView)
        {
            // Initialize
            questStatus status = null;
            tablesetTable = null;
            tablesetView = null;


            // Get TablesetColumn
            DbTablesetColumnsMgr dbTablesetColumnsMgr = new DbTablesetColumnsMgr(this.UserSession);
            TablesetColumn tablesetColumn = null;
            status = dbTablesetColumnsMgr.Read(tablesetColumnId, out tablesetColumn);
            if (!questStatusDef.IsSuccessOrWarning(status))
            {
                return (status);
            }

            if (tablesetColumn.EntityTypeId == EntityType.Table)
            {
                TablesetTableId tablesetTableId = new TablesetTableId(tablesetColumn.TableSetEntityId);
                DbTablesetTablesMgr dbTablesetTablesMgr = new DbTablesetTablesMgr(this.UserSession);
                status = dbTablesetTablesMgr.Read(tablesetTableId, out tablesetTable);
                if (!questStatusDef.IsSuccessOrWarning(status))
                {
                    return (status);
                }
            }
            else if (tablesetColumn.EntityTypeId == EntityType.View)
            {
                TablesetViewId tablesetViewId = new TablesetViewId(tablesetColumn.TableSetEntityId);
                DbTablesetViewsMgr dbTablesetViewsMgr = new DbTablesetViewsMgr(this.UserSession);
                status = dbTablesetViewsMgr.Read(tablesetViewId, out tablesetView);
                if (!questStatusDef.IsSuccessOrWarning(status))
                {
                    return (status);
                }
            }
            return (new questStatus(Severity.Success));
        }
        #endregion
    }
}

