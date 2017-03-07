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
using Quest.MasterPricing.Services.Data.Database;


namespace Quest.MasterPricing.Services.Data.Filters
{
    public class DbFilterMgr : DbMgr
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
        public DbFilterMgr()
            : base()
        {
            initialize();
        }
        public DbFilterMgr(UserSession userSession)
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
        public questStatus GetFilter(FilterId filterId, out Filter filter)
        {
            // Initialize
            questStatus status = null;
            filter = null;
            DbMgrTransaction trans = null;


            // BEGIN TRANSACTION
            status = BeginTransaction("GetFilter_" + filterId.ToString(), out trans);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }

            // Call transaction-based method
            status = GetFilter(trans, filterId, out filter);
            if (!questStatusDef.IsSuccess(status))
            {
                RollbackTransaction(trans);
                return (status);
            }

            // COMMIT TRANSACTION
            status = CommitTransaction(trans);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus GetFilter(DbMgrTransaction trans, FilterId filterId, out Filter filter)
        {
            // Initialize
            questStatus status = null;
            filter = null;

            #region Basic Filter Info 
            //
            // Basic Filter Info 
            //
            // Read filter
            DbFiltersMgr dbFiltersMgr = new DbFiltersMgr(this.UserSession);
            status = dbFiltersMgr.Read(trans, filterId, out filter);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }

            // Read tableset
            TablesetId tablesetId = new TablesetId(filter.TablesetId);
            Tableset tableset = null;
            DbTablesetsMgr dbTablesetsMgr = new DbTablesetsMgr(this.UserSession);
            status = dbTablesetsMgr.Read(tablesetId, out tableset);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }

            // Read database
            DatabaseId databaseId = new DatabaseId(tableset.DatabaseId);
            Quest.Functional.MasterPricing.Database database = null;
            DbDatabasesMgr dbDatabasesMgr = new DbDatabasesMgr(this.UserSession);
            status = dbDatabasesMgr.Read(databaseId, out database);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            #endregion


            // Get table info
            List<FilterTable> filterTableList = null;
            status = GetFilterTables(databaseId, filterId, out filterTableList);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            filter.FilterTableList = filterTableList;


            // Get view info
            List<FilterView> filterViewList = null;
            status = GetFilterViews(databaseId, filterId, out filterViewList);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            filter.FilterViewList = filterViewList;


            // TODO: COLUMNS-ONLY VS. TABLES WHICH HAVE ALL COLUMNS IN THE FILTER.
            List<FilterColumn> filterColumnList = null;
            status = GetFilterColumns(databaseId, filterId, out filterColumnList);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            filter.FilterColumnList = filterColumnList;



            // Load all filter items.
            List<FilterItem> filterItemList = null;
            status = GetFilterItems(databaseId, filterId, filter, out filterItemList);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            filter.FilterItemList = filterItemList;


            // Read all filter procedures
            List<FilterProcedure> filterProcedureList = null;
            DbFilterProceduresMgr dbFilterProceduresMgr = new DbFilterProceduresMgr(this.UserSession);
            status = dbFilterProceduresMgr.Read(filterId, out filterProcedureList);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            DbFilterProcedureParametersMgr dbFilterProcedureParametersMgr = new DbFilterProcedureParametersMgr(this.UserSession);
            foreach (FilterProcedure filterProcedure in filterProcedureList)
            {
                FilterProcedureId filterProcedureId = new FilterProcedureId(filterProcedure.Id);
                List<FilterProcedureParameter> filterProcedureParameterList = null;
                status = dbFilterProcedureParametersMgr.Read(filterProcedureId, out filterProcedureParameterList);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }
                filterProcedure.ParameterList.AddRange(filterProcedureParameterList);
            }
            filter.FilterProcedureList = filterProcedureList;


            return (new questStatus(Severity.Success));
        }
        public questStatus GetFilterItems(DatabaseId databaseId, FilterId filterId, Filter filter, out List<FilterItem> filterItemList)
        {
            // Initialize
            questStatus status = null;
            filterItemList = null;


            // Load all filter items.
            DbFilterItemsMgr dbFilterItemsMgr = new DbFilterItemsMgr(this.UserSession);
            status = dbFilterItemsMgr.Read(filterId, out filterItemList);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }

            // Get all filter items
            List<FilterItem> _filterItemList = new List<FilterItem>();
            foreach (FilterItem filterItem in filterItemList)
            {
                FilterItemId filterItemId = new FilterItemId(filterItem.Id);
                FilterItem _filterItem = null;
                status = GetFilterItem(databaseId, filter, filterItemId, out _filterItem);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }
                _filterItemList.Add(_filterItem);
            }
            filterItemList = _filterItemList;


            return (new questStatus(Severity.Success));
        }
        public questStatus GetFilterItem(DatabaseId databaseId, Filter filter, FilterItemId filterItemId, out FilterItem filterItem)
        {
            // Initialize
            questStatus status = null;
            filterItem = null;


            // Read filter item
            DbFilterItemsMgr dbFilterItemsMgr = new DbFilterItemsMgr(this.UserSession);
            status = dbFilterItemsMgr.Read(filterItemId, out filterItem);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }

            // Klugie: temporary, but still hideous.
            // Just back up and get stuff we need.  (All this due to refactoring, more to do).
            TablesetId tablesetId = null;
            status = klugieGetFilterItemInfo(filterItem, out databaseId, out tablesetId);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }


            // Get FilterColumn
            FilterColumn filterColumn = null;
            if (filterItem.FilterEntityTypeId == FilterEntityType.Column) {
                FilterColumnId filterColumnId = new FilterColumnId(filterItem.FilterEntityId);
                DbFilterColumnsMgr dbFilterColumnsMgr = new DbFilterColumnsMgr(this.UserSession);
                status = dbFilterColumnsMgr.Read(filterColumnId, out filterColumn);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }
                filterItem.FilterColumn = filterColumn;
            }

            // Load column metadata.
            status = LoadFilterColumnMetadata(databaseId, tablesetId, filterColumn);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }


            // Joins
            List<FilterItemJoin> filterItemJoinList = null;
            DbFilterItemJoinsMgr dbFilterItemJoinsMgr = new DbFilterItemJoinsMgr(this.UserSession);
            status = dbFilterItemJoinsMgr.Read(filterItemId, out filterItemJoinList);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            foreach (FilterItemJoin filterItemJoin in filterItemJoinList)
            {
                status = getJoinTargetColumnId(filter, filterItemJoin);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }
            }
            filterItem.JoinList = filterItemJoinList;


            // Operations
            List<FilterOperation> filterOperationList = null;
            DbFilterOperationsMgr dbFilterOperationsMgr = new DbFilterOperationsMgr(this.UserSession);
            status = dbFilterOperationsMgr.Read(filterItemId, out filterOperationList);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }

            foreach (FilterOperation filterOperation in filterOperationList)
            {
                // Values
                FilterOperationId filterOperationId = new FilterOperationId(filterOperation.Id);
                List<FilterValue> filterValueList = null;
                DbFilterValuesMgr dbFilterValuesMgr = new DbFilterValuesMgr(this.UserSession);
                status = dbFilterValuesMgr.Read(filterOperationId, out filterValueList);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }
                filterOperation.ValueList = filterValueList;
            }
            filterItem.OperationList.AddRange(filterOperationList);


            // Lookup
            DbLookupsMgr dbLookupsMgr = new DbLookupsMgr(this.UserSession);
            if (filterItem.LookupId.HasValue)
            {
                LookupId lookupId = new LookupId(filterItem.LookupId.Value);
                Lookup lookup = null;
                status = dbLookupsMgr.Read(lookupId, out lookup);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }
                filterItem.Lookup = lookup;
            }


            // TypeList
            DbTypeListsMgr dbTypeListsMgr = new DbTypeListsMgr(this.UserSession);
            if (filterItem.TypeListId.HasValue)
            {
                TypeListId typeListId = new TypeListId(filterItem.TypeListId.Value);
                TypeList typeList = null;
                status = dbTypeListsMgr.Read(typeListId, out typeList);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }
                filterItem.TypeList = typeList;
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus GetFilterColumns(DatabaseId databaseId, FilterId filterId, out List<FilterColumn> filterColumnList)
        {
            // Initialize
            questStatus status = null;
            filterColumnList = null;


            // Get filter for tableset Id
            Filter filter = null;
            DbFiltersMgr dbFiltersMgr = new DbFiltersMgr(this.UserSession);
            status = dbFiltersMgr.Read(filterId, out filter);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            TablesetId tablesetId = new TablesetId(filter.TablesetId);


            // Get FilterColumns
            DbFilterColumnsMgr dbFilterColumnsMgr = new DbFilterColumnsMgr(this.UserSession);
            status = dbFilterColumnsMgr.Read(filterId, out filterColumnList);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }

            // Get tableset column for each filter Column
            foreach (FilterColumn filterColumn in filterColumnList)
            {
                status = LoadFilterColumnMetadata(databaseId, tablesetId, filterColumn);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }
            }
            filter.FilterColumnList = filterColumnList;


            return (new questStatus(Severity.Success));
        }


        #region Table-related
        //
        // Table-related
        //
        public questStatus GetFilterTables(DatabaseId databaseId, FilterId filterId, out List<FilterTable> filterTableList)
        {
            // Initialize
            questStatus status = null;


            // Get all filter tables for this filter
            DbFilterTablesMgr dbFilterTablesMgr = new DbFilterTablesMgr(this.UserSession);
            status = dbFilterTablesMgr.Read(filterId, out filterTableList);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }

            // Get filter for tableset id
            DbFiltersMgr dbFiltersMgr = new DbFiltersMgr(this.UserSession);
            Filter filter = null;
            status = dbFiltersMgr.Read(filterId, out filter);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            TablesetId tablesetId = new TablesetId(filter.TablesetId);


            // Get tableset tables for these filter tables.
            DbTablesetMgr dbTablesetMgr = new DbTablesetMgr(this.UserSession);
            foreach (FilterTable filterTable in filterTableList)
            {
                FilterTableTablesetTableId filterTableTablesetTableId = new FilterTableTablesetTableId(tablesetId, filterTable.Schema, filterTable.Name);
                TablesetTable tablesetTable = null;
                status = dbTablesetMgr.GetTablesetTable(databaseId, filterTableTablesetTableId, out tablesetTable);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }
                filterTable.TablesetTable = tablesetTable;

                List<FilterColumn> filterColumnList = null;
                status = GetFilterColumns(databaseId, filterId, filterTable, out filterColumnList); 
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }
                filterTable.FilterColumnList = filterColumnList;
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus GetFilterColumns(DatabaseId databaseId, FilterId filterId, FilterTable filterTable, out List<FilterColumn> filterColumnList)
        {
            // Initialize
            questStatus status = null;
            filterColumnList = null;


            // Get filter for tableset Id
            Filter filter = null;
            DbFiltersMgr dbFiltersMgr = new DbFiltersMgr(this.UserSession);
            status = dbFiltersMgr.Read(filterId, out filter);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            TablesetId tablesetId = new TablesetId(filter.TablesetId);


            // Get FilterColumns
            FilterTableId filterTableId = new FilterTableId(filterTable.Id);
            DbFilterColumnsMgr dbFilterColumnsMgr = new DbFilterColumnsMgr(this.UserSession);
            status = dbFilterColumnsMgr.Read(filterId, filterTableId, out filterColumnList);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }

            foreach (FilterColumn filterColumn in filterColumnList)
            {
                status = LoadFilterColumnMetadata(databaseId, tablesetId, filterColumn);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }
            }
            return (new questStatus(Severity.Success));
        }
        #endregion


        #region View-related
        //
        // View-related
        //
        public questStatus GetFilterViews(DatabaseId databaseId, FilterId filterId, out List<FilterView> filterViewList)
        {
            // Initialize
            questStatus status = null;


            // Get all filter views for this filter
            DbFilterViewsMgr dbFilterViewsMgr = new DbFilterViewsMgr(this.UserSession);
            status = dbFilterViewsMgr.Read(filterId, out filterViewList);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }

            // Get filter for tableset id
            DbFiltersMgr dbFiltersMgr = new DbFiltersMgr(this.UserSession);
            Filter filter = null;
            status = dbFiltersMgr.Read(filterId, out filter);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            TablesetId tablesetId = new TablesetId(filter.TablesetId);


            // Get tableset views for these filter views.
            DbTablesetMgr dbTablesetMgr = new DbTablesetMgr(this.UserSession);
            foreach (FilterView filterView in filterViewList)
            {
                FilterViewTablesetViewId filterViewTablesetViewId = new FilterViewTablesetViewId(tablesetId, filterView.Schema, filterView.Name);
                TablesetView tablesetView = null;
                status = dbTablesetMgr.GetTablesetView(databaseId, filterViewTablesetViewId, out tablesetView);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }
                filterView.TablesetView = tablesetView;

                List<FilterColumn> filterColumnList = null;
                status = GetFilterColumns(databaseId, filterId, filterView, out filterColumnList);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }
                filterView.FilterColumnList = filterColumnList;
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus GetFilterColumns(DatabaseId databaseId, FilterId filterId, FilterView filterView, out List<FilterColumn> filterColumnList)
        {
            // Initialize
            questStatus status = null;
            filterColumnList = null;


            // Get filter for tableset Id
            Filter filter = null;
            DbFiltersMgr dbFiltersMgr = new DbFiltersMgr(this.UserSession);
            status = dbFiltersMgr.Read(filterId, out filter);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            TablesetId tablesetId = new TablesetId(filter.TablesetId);


            // Get FilterColumns
            FilterViewId filterViewId = new FilterViewId(filterView.Id);
            DbFilterColumnsMgr dbFilterColumnsMgr = new DbFilterColumnsMgr(this.UserSession);
            status = dbFilterColumnsMgr.Read(filterId, filterViewId, out filterColumnList);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }

            foreach (FilterColumn filterColumn in filterColumnList)
            {
                status = LoadFilterColumnMetadata(databaseId, tablesetId, filterColumn);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }
            }
            return (new questStatus(Severity.Success));
        }
        #endregion


        public questStatus LoadFilterColumnMetadata(DatabaseId databaseId, TablesetId tablesetId, FilterColumn filterColumn)
        {
            // Initialize
            questStatus status = null;


            // We find FilterColumn metainfo by name: column name + schema and name of the table/view in which it resides.
            string columnName = filterColumn.Name;

            // Get the FilterTable or FilterColumn according to the FilterEntityType
            // Return an error if not one of the two; other stuff not supported yet.
            DbFilterTablesMgr dbFilterTablesMgr = new DbFilterTablesMgr(this.UserSession);
            DbTablesetTablesMgr dbTablesetTablesMgr = new DbTablesetTablesMgr(this.UserSession);
            DbFilterViewsMgr dbFilterViewsMgr = new DbFilterViewsMgr(this.UserSession);
            DbTablesetViewsMgr dbTablesetViewsMgr = new DbTablesetViewsMgr(this.UserSession);
            EntityTypeId entityTypeId = null;
            EntityId entityId = null;
            if (filterColumn.FilterEntityTypeId == FilterEntityType.Table)
            {
                // Get FilterTable
                FilterTableId filterTableId = new FilterTableId(filterColumn.FilterEntityId);
                FilterTable filterTable = null;
                status = dbFilterTablesMgr.Read(filterTableId, out filterTable);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }
                filterColumn.ParentEntityType.type = "table";
                filterColumn.ParentEntityType.Schema = filterTable.Schema;
                filterColumn.ParentEntityType.Name = filterTable.Name;

                // Get the tableset table
                FilterTableTablesetTableId filterTableTablesetTableId = new FilterTableTablesetTableId(tablesetId, filterTable.Schema, filterTable.Name);
                TablesetTable tablesetTable = null;
                status = dbTablesetTablesMgr.Read(filterTableTablesetTableId, out tablesetTable);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }

                // Setup tableset column read id
                entityTypeId = new EntityTypeId(EntityType.Table);
                entityId = new EntityId(tablesetTable.Id);
                columnName = filterColumn.Name;
            }
            else if (filterColumn.FilterEntityTypeId == FilterEntityType.View)
            {
                // Get FilterView
                FilterViewId filterViewId = new FilterViewId(filterColumn.FilterEntityId);
                FilterView filterView = null;
                status = dbFilterViewsMgr.Read(filterViewId, out filterView);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }
                filterColumn.ParentEntityType.type = "view";
                filterColumn.ParentEntityType.Schema = filterView.Schema;
                filterColumn.ParentEntityType.Name = filterView.Name;

                // Get the tableset view
                FilterViewTablesetViewId filterViewTablesetViewId = new FilterViewTablesetViewId(tablesetId, filterView.Schema, filterView.Name);
                TablesetView tablesetView = null;
                status = dbTablesetViewsMgr.Read(filterViewTablesetViewId, out tablesetView);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }

                // Setup tableset column read id
                entityTypeId = new EntityTypeId(EntityType.View);
                entityId = new EntityId(tablesetView.Id);
            }
            else
            {
                return (new questStatus(Severity.Error, String.Format("ERROR: unsupported FilterEntityType {0} for FilterColumn {1}",
                    filterColumn.FilterEntityTypeId, filterColumn.Id)));
            }

            // Get tableset column
            DbTablesetColumnsMgr dbTablesetColumnsMgr = new DbTablesetColumnsMgr(this.UserSession);
            FilterColumnTablesetColumnId filterColumnTablesetColumnId = new FilterColumnTablesetColumnId(entityTypeId, entityId, columnName);
            TablesetColumn tablesetColumn = null;
            status = dbTablesetColumnsMgr.Read(filterColumnTablesetColumnId, out tablesetColumn);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            filterColumn.TablesetColumn = tablesetColumn;


            // Get column metainfo
            Column column = null;
            status = GetTablesteColumnMetainfo(databaseId, tablesetColumn, out column);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            filterColumn.TablesetColumn.Column = column;


            return (new questStatus(Severity.Success));
        }
        public questStatus GetTablesteColumnMetainfo(DatabaseId databaseId, TablesetColumn tablesetColumn, out Column column)
        {
            // Initialize
            questStatus status = null;
            column = null;
            EntityId entityId = null;
            EntityTypeId entityTypeId = null;


            // Get TablesetTable or TablesetView based on EntityTypeId
            if (tablesetColumn.EntityTypeId == EntityType.Table)
            {
                TablesetTableId tablesetTableId = new TablesetTableId(tablesetColumn.TableSetEntityId);
                TablesetTable tablesetTable = null;
                DbTablesetTablesMgr dbTablesetTablesMgr = new DbTablesetTablesMgr(this.UserSession);
                status = dbTablesetTablesMgr.Read(tablesetTableId, out tablesetTable);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }

                // Get table
                Table table = null;
                DbTablesMgr dbTablesMgr = new DbTablesMgr(this.UserSession);
                status = dbTablesMgr.Read(databaseId, tablesetTable.Schema, tablesetTable.Name, out table);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }
                entityId = new EntityId(table.Id);
                entityTypeId = new EntityTypeId(EntityType.Table);
            }
            else if (tablesetColumn.EntityTypeId == EntityType.View)
            {
                TablesetViewId tablesetViewId = new TablesetViewId(tablesetColumn.TableSetEntityId);
                TablesetView tablesetView = null;
                DbTablesetViewsMgr dbTablesetViewsMgr = new DbTablesetViewsMgr(this.UserSession);
                status = dbTablesetViewsMgr.Read(tablesetViewId, out tablesetView);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }

                // Get view
                View view = null;
                DbViewsMgr dbViewsMgr = new DbViewsMgr(this.UserSession);
                status = dbViewsMgr.Read(databaseId, tablesetView.Schema, tablesetView.Name, out view);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }
                entityId = new EntityId(view.Id);
                entityTypeId = new EntityTypeId(EntityType.View);
            }
            else
            {
                return (new questStatus(Severity.Error, String.Format("ERROR: Invalid EntityType {0} for TablesetColumn.Id {1}",
                        tablesetColumn.EntityTypeId, tablesetColumn.Id)));
            }

            // Get column
            string columnName = tablesetColumn.Name;
            DbColumnsMgr dbColumnsMgr = new DbColumnsMgr(this.UserSession);
            status = dbColumnsMgr.Read(entityTypeId, entityId, columnName, out column);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }




        #region Utility Routines
        //
        // Utility Routines
        //
        public questStatus GetTablesetTable(FilterColumn filterColumn, out TablesetTable tablesetTable)
        {
            // Initialize
            questStatus status = null;
            tablesetTable = null;

            TablesetTableId tablesetTableId = new TablesetTableId(filterColumn.TablesetEntityId);
            DbTablesetTablesMgr dbTablesetTablesMgr = new DbTablesetTablesMgr(this.UserSession);
            status = dbTablesetTablesMgr.Read(tablesetTableId, out tablesetTable);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus GetFilterTable(FilterId filterId, TablesetTable tablesetTable, out FilterTable filterTable)
        {
            // Initialize
            questStatus status = null;
            filterTable = null;

            FilterTableNameIdentifier filterTableNameIdentifier = new FilterTableNameIdentifier(filterId, tablesetTable.Schema, tablesetTable.Name);
            DbFilterTablesMgr dbFilterTablesMgr = new DbFilterTablesMgr(this.UserSession);
            status = dbFilterTablesMgr.Read(filterTableNameIdentifier, out filterTable);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus GetTablesetView(FilterColumn filterColumn, out TablesetView tablesetView)
        {
            // Initialize
            questStatus status = null;
            tablesetView = null;

            TablesetViewId tablesetViewId = new TablesetViewId(filterColumn.TablesetEntityId);
            DbTablesetViewsMgr dbTablesetViewsMgr = new DbTablesetViewsMgr(this.UserSession);
            status = dbTablesetViewsMgr.Read(tablesetViewId, out tablesetView);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus GetFilterView(FilterId filterId, TablesetView tablesetView, out FilterView filterView)
        {
            // Initialize
            questStatus status = null;
            filterView = null;

            FilterViewNameIdentifier filterViewNameIdentifier = new FilterViewNameIdentifier(filterId, tablesetView.Schema, tablesetView.Name);
            DbFilterViewsMgr dbFilterViewsMgr = new DbFilterViewsMgr(this.UserSession);
            status = dbFilterViewsMgr.Read(filterViewNameIdentifier, out filterView);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus GetFilterColumn(Filter filter, FilterItem filterItem, out FilterColumn filterColumn)
        {
            // Initialize
            questStatus status = null;
            filterColumn = null;


            // Get TablesetColumn.
            TablesetColumnId tablesetColumnId = new TablesetColumnId(filterItem.TablesetColumnId);
            TablesetColumn tablesetColumn = null;
            DbTablesetColumnsMgr dbTablesetColumnsMgr = new DbTablesetColumnsMgr(this.UserSession);
            status = dbTablesetColumnsMgr.Read(tablesetColumnId, out tablesetColumn);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }

            FilterColumn _filterColumn = null;
            if (tablesetColumn.EntityTypeId == EntityType.Table)
            {
                _filterColumn = filter.FilterColumnList.Find(delegate (FilterColumn c) { return (c.FilterEntityTypeId == FilterEntityType.Table && c.TablesetEntityId == tablesetColumn.TableSetEntityId && c.Name == tablesetColumn.Name); });
            }
            else if (tablesetColumn.EntityTypeId == EntityType.View)
            {
                _filterColumn = filter.FilterColumnList.Find(delegate (FilterColumn c) { return (c.FilterEntityTypeId == FilterEntityType.View && c.TablesetEntityId == tablesetColumn.TableSetEntityId && c.Name == tablesetColumn.Name); });
            }
            else
            {
                return (new questStatus(Severity.Error, String.Format("ERROR: GetFilterColumn: TablesetColumn {0} EntityTypeId {1} not supported",
                        tablesetColumn.Id, tablesetColumn.EntityTypeId)));
            }
            if (_filterColumn == null)
            {
                return (new questStatus(Severity.Error, String.Format("ERROR: GetFilterColumn: FilterColumn not found for TablesetColumn Type:{0} Name:{1}",
                        tablesetColumn.EntityTypeId, tablesetColumn.Name)));
            }
            filterColumn = _filterColumn;

            return (new questStatus(Severity.Success));
        }
        #endregion



        #region SQL-related
        //
        // SQL-related
        //
        public questStatus GenerateFilterSQL(FilterId filterId, out Filter filter)
        {
            // Initialize
            questStatus status = null;
            filter = null;

            // Read filter
            status = GetFilter(filterId, out filter);
            if (! questStatusDef.IsSuccess(status))
            {
                return (status);
            }

            // Generate filter SQL
            Filter filterWithSQL = null;
            status = GenerateFilterSQL(filter, out filterWithSQL);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            filter = filterWithSQL;


            return (new questStatus(Severity.Success));
        }
        public questStatus GenerateFilterSQL(Filter filter, out Filter filterWithSQL)
        {
            // Initialize
            questStatus status = null;
            filterWithSQL = null;

            DbFilterSQLMgr dbFilterSQLMgr = new DbFilterSQLMgr(this.UserSession);
            status = dbFilterSQLMgr.GenerateFilterSQL(filter, out filterWithSQL);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus SaveSQL(Filter filter)
        {
            // Initialize
            questStatus status = null;

            FilterId filterId = new FilterId(filter.Id);
            Filter saveFilter = null;
            DbFiltersMgr dbFiltersMgr = new DbFiltersMgr(this.UserSession);


            // Read just the filter
            status = dbFiltersMgr.Read(filterId, out saveFilter);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }

            // Update the filter SQL.
            saveFilter.SQL = filter.SQL;
            status = dbFiltersMgr.Update(saveFilter);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        #endregion


        #region Delete Filter
        //
        // Delete Filter
        //
        public questStatus Clear(FilterId filterId)
        {
            // Initialize
            questStatus status = null;
            DbMgrTransaction trans = null;


            // BEGIN TRANSACTION
            status = BeginTransaction("ClearFilter_" + filterId.ToString(), out trans);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }

            // Call transaction-based method
            status = Clear(trans, filterId);
            if (!questStatusDef.IsSuccess(status))
            {
                RollbackTransaction(trans);
                return (status);
            }

            // COMMIT TRANSACTION
            status = CommitTransaction(trans);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Clear(DbMgrTransaction trans, FilterId filterId)
        {
            // Initialize
            questStatus status = null;



            // Get filter
            Filter filter = null;
            status = GetFilter(trans, filterId, out filter);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }

            // Delete all items, joins, operations and values.
            DbFilterItemJoinsMgr dbFilterItemJoinsMgr = new DbFilterItemJoinsMgr(this.UserSession);
            DbFilterOperationsMgr dbFilterOperationsMgr = new DbFilterOperationsMgr(this.UserSession);
            DbFilterValuesMgr dbFilterValuesMgr = new DbFilterValuesMgr(this.UserSession);
            foreach (FilterItem filterItem in filter.FilterItemList)
            {
                FilterItemId filterItemId = new FilterItemId(filterItem.Id);

                // Joins
                status = dbFilterItemJoinsMgr.Delete(trans, filterItemId);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }

                // Values
                foreach (FilterOperation filterOperation in filterItem.OperationList)
                {
                    // Delete all filter values for this operation.
                    FilterOperationId filterOperationId = new FilterOperationId(filterOperation.Id);
                    status = dbFilterValuesMgr.Delete(trans, filterOperationId);
                    if (!questStatusDef.IsSuccess(status))
                    {
                        return (status);
                    }
                }

                // Operations
                // TODO: FIND OUT WHY THIS DOESN'T DELETE FilterOperations.
                status = dbFilterOperationsMgr.Delete(trans, filterItemId);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }
            }

            // Delete all filter items
            DbFilterItemsMgr dbFilterItemsMgr = new DbFilterItemsMgr(this.UserSession);
            status = dbFilterItemsMgr.Delete(filterId);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }



            // Delete all filter columns
            DbFilterColumnsMgr dbFilterColumnsMgr = new DbFilterColumnsMgr(this.UserSession);
            status = dbFilterColumnsMgr.Delete(trans, filterId);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }

            // Delete all filter tables
            DbFilterTablesMgr dbFiltersTableMgr = new DbFilterTablesMgr(this.UserSession);
            status = dbFiltersTableMgr.Delete(trans, filterId);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }

            // Delete all filter views
            DbFilterViewsMgr dbFilterViewsMgr = new DbFilterViewsMgr(this.UserSession);
            status = dbFilterViewsMgr.Delete(trans, filterId);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }

            // Delete filter procedures and parameters
            DbFilterProceduresMgr dbFilterProceduresMgr = new DbFilterProceduresMgr(this.UserSession);
            DbFilterProcedureParametersMgr dbFilterProcedureParametersMgr = new DbFilterProcedureParametersMgr(this.UserSession);
            foreach (FilterProcedure filterProcedure in filter.FilterProcedureList)
            {
                FilterProcedureId filterProcedureId = new FilterProcedureId(filterProcedure.Id);
                status = dbFilterProcedureParametersMgr.Delete(filterProcedureId);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }
                status = dbFilterProceduresMgr.Delete(trans, filterProcedureId);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Delete(FilterId filterId)
        {
            // Initialize
            questStatus status = null;
            DbMgrTransaction trans = null;


            // BEGIN TRANSACTION
            status = BeginTransaction("DeleteFilter_" + filterId.ToString(), out trans);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }


            // Delete the filter.
            status = Delete(trans, filterId);
            if (!questStatusDef.IsSuccess(status))
            {
                RollbackTransaction(trans);
                return (status);
            }


            // COMMIT TRANSACTION
            status = CommitTransaction(trans);
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


            // Clear the filter.
            status = Clear(trans, filterId);
            if (!questStatusDef.IsSuccess(status))
            {
                RollbackTransaction(trans);
                return (status);
            }

            // Delete the filter
            DbFiltersMgr dbFiltersMgr = new DbFiltersMgr(this.UserSession);
            status = dbFiltersMgr.Delete(trans, filterId);
            if (!questStatusDef.IsSuccess(status))
            {
                RollbackTransaction(trans);
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        #endregion

        public questStatus Read(DbMgrTransaction trans, TablesetId tablesetId, out List<Filter> filterList)
        {
            // Initialize
            questStatus status = null;
            filterList = null;


            // Get all filters for this tableset.
            DbFiltersMgr dbFiltersMgr = new DbFiltersMgr(this.UserSession);
            status = dbFiltersMgr.Read(trans, tablesetId, out filterList);
            if (! questStatusDef.IsSuccess(status))
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
            DbMgrTransaction trans = null;



            // Get the filter.
            Filter filter = null;
            DbFiltersMgr dbFiltersMgr = new DbFiltersMgr(this.UserSession);
            status = GetFilter(filterId, out filter);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }



            // BEGIN TRANSACTION
            status = BeginTransaction("CopyFilter_" + filterId.ToString(), out trans);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }



            // Change the name and save it for a new filterId.
            filter.Name = filter.Name + " (Copy)";
            status = dbFiltersMgr.Create(trans, filter, out newFilterId);
            if (!questStatusDef.IsSuccess(status))
            {
                RollbackTransaction(trans);
                return (status);
            }

            // Copy tables.
            DbFilterTablesMgr dbFilterTablesMgr = new DbFilterTablesMgr(this.UserSession);
            DbFilterColumnsMgr dbFilterColumnsMgr = new DbFilterColumnsMgr(this.UserSession);
            foreach (FilterTable filterTable in filter.FilterTableList)
            {
                filterTable.FilterId = newFilterId.Id;
                FilterTableId filterTableId = null;
                status = dbFilterTablesMgr.Create(trans, filterTable, out filterTableId);
                if (!questStatusDef.IsSuccess(status))
                {
                    RollbackTransaction(trans);
                    return (status);
                }
                foreach (FilterColumn filterColumn in filterTable.FilterColumnList)
                {
                    filterColumn.FilterId = newFilterId.Id;
                    filterColumn.FilterEntityId = filterTableId.Id;
                    FilterColumnId filterColumnId = null;
                    status = dbFilterColumnsMgr.Create(trans, filterColumn, out filterColumnId);
                    if (!questStatusDef.IsSuccess(status))
                    {
                        RollbackTransaction(trans);
                        return (status);
                    }
                }
            }

            // Copy views.
            DbFilterViewsMgr dbFilterViewsMgr = new DbFilterViewsMgr(this.UserSession);
            foreach (FilterView filterView in filter.FilterViewList)
            {
                filterView.FilterId = newFilterId.Id;
                FilterViewId filterViewId = null;
                status = dbFilterViewsMgr.Create(trans, filterView, out filterViewId);
                if (!questStatusDef.IsSuccess(status))
                {
                    RollbackTransaction(trans);
                    return (status);
                }
                foreach (FilterColumn filterColumn in filterView.FilterColumnList)
                {
                    filterColumn.FilterId = newFilterId.Id;
                    filterColumn.FilterEntityId = filterViewId.Id;
                    FilterColumnId filterColumnId = null;
                    status = dbFilterColumnsMgr.Create(trans, filterColumn, out filterColumnId);
                    if (!questStatusDef.IsSuccess(status))
                    {
                        RollbackTransaction(trans);
                        return (status);
                    }
                }
            }

            // Copy the columns
            Dictionary<int, int> oldToNewFilterEntityIds = new Dictionary<int, int>();
            foreach (FilterColumn filterColumn in filter.FilterColumnList)
            {
                filterColumn.FilterId = newFilterId.Id;
                FilterColumnId filterColumnId = null;
                int oldId = filterColumn.Id;
                status = dbFilterColumnsMgr.Create(filterColumn, out filterColumnId);
                if (!questStatusDef.IsSuccess(status))
                {
                    RollbackTransaction(trans);
                    return (status);
                }
                oldToNewFilterEntityIds.Add(oldId, filterColumnId.Id);
                filterColumn.Id = filterColumnId.Id;
            }

            // Copy items
            DbFilterItemsMgr dbFilterItemsMgr = new DbFilterItemsMgr(this.UserSession);
            DbFilterItemJoinsMgr dbFilterItemJoinsMgr = new DbFilterItemJoinsMgr(this.UserSession);
            DbFilterOperationsMgr dbFilterOperationsMgr = new DbFilterOperationsMgr(this.UserSession);
            DbFilterValuesMgr dbFilterValuesMgr = new DbFilterValuesMgr(this.UserSession);
            foreach (FilterItem filterItem in filter.FilterItemList)
            {
                filterItem.FilterId = newFilterId.Id;
                int newFilterEntityId = -1;
                if (! oldToNewFilterEntityIds.TryGetValue(filterItem.FilterEntityId, out newFilterEntityId))
                {
                    status = new questStatus(Severity.Error, String.Format("FilterItem {0}  filterEntityId {1}  not found in old-to-new mappings",
                        filterItem.Id, filterItem.FilterEntityId));
                    RollbackTransaction(trans);
                    return (status);
                }
                filterItem.FilterEntityId = newFilterEntityId;

                FilterItemId filterItemId = null;
                status = dbFilterItemsMgr.Create(trans, filterItem, out filterItemId);
                if (!questStatusDef.IsSuccess(status))
                {
                    RollbackTransaction(trans);
                    return (status);
                }

                // Save filter item joins
                foreach (FilterItemJoin filterItemJoin in filterItem.JoinList)
                {
                    filterItemJoin.FilterItemId = filterItemId.Id;
                    FilterItemJoinId filterItemJoinId = null;
                    status = dbFilterItemJoinsMgr.Create(trans, filterItemJoin, out filterItemJoinId);
                    if (!questStatusDef.IsSuccess(status))
                    {
                        RollbackTransaction(trans);
                        return (status);
                    }
                }

                // Save filter operations
                foreach (FilterOperation filterOperation in filterItem.OperationList)
                {
                    filterOperation.FilterItemId = filterItemId.Id;
                    FilterOperationId filterOperationId = null;
                    status = dbFilterOperationsMgr.Create(trans, filterOperation, out filterOperationId);
                    if (!questStatusDef.IsSuccess(status))
                    {
                        RollbackTransaction(trans);
                        return (status);
                    }

                    // Save filter values
                    foreach (FilterValue filterValue in filterOperation.ValueList)
                    {
                        filterValue.FilterOperationId = filterOperationId.Id;
                        FilterValueId filterValueId = null;
                        status = dbFilterValuesMgr.Create(trans, filterValue, out filterValueId);
                        if (!questStatusDef.IsSuccess(status))
                        {
                            RollbackTransaction(trans);
                            return (status);
                        }
                    }
                }
            }

            // Save filter procedures
            DbFilterProceduresMgr dbFilterProceduresMgr = new DbFilterProceduresMgr(this.UserSession);
            DbFilterProcedureParametersMgr dbFilterProcedureParametersMgr = new DbFilterProcedureParametersMgr(this.UserSession);
            foreach (FilterProcedure filterProcedure in filter.FilterProcedureList)
            {
                filterProcedure.FilterId = newFilterId.Id;
                FilterProcedureId filterProcedureId = null;
                status = dbFilterProceduresMgr.Create(trans, filterProcedure, out filterProcedureId);
                if (!questStatusDef.IsSuccess(status))
                {
                    RollbackTransaction(trans);
                    return (status);
                }

                foreach (FilterProcedureParameter filterProcedureParameter in filterProcedure.ParameterList)
                {
                    filterProcedureParameter.FilterProcedureId = filterProcedureId.Id;
                    FilterProcedureParameterId filterProcedureParameterId = null;
                    status = dbFilterProcedureParametersMgr.Create(trans, filterProcedureParameter, out filterProcedureParameterId);
                    if (!questStatusDef.IsSuccess(status))
                    {
                        RollbackTransaction(trans);
                        return (status);
                    }
                }
            }


            // COMMIT TRANSACTION
            status = CommitTransaction(trans);
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
            }
            catch (System.Exception ex)
            {
                status = new questStatus(Severity.Fatal, String.Format("EXCEPTION: {0}.{1}: {2}",
                        this.GetType().ToString(), MethodInfo.GetCurrentMethod().Name, ex.Message));
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        private string bracketIdentifier(string identifier)
        {
            if (identifier.StartsWith("[") && identifier.EndsWith("]"))
            {
                return (identifier);
            }
            string _identifier = "[" + identifier + "]";
            return (_identifier);
        }
        private string quoteValueList(List<FilterValue> filterValueList, bool preLike = false, bool postLike = false)
        {
            StringBuilder sbValueList = new StringBuilder();
            foreach (FilterValue filterValue in filterValueList)
            {
                sbValueList.Append("'" + (preLike ? "%" : "") + filterValue.Value + (postLike ? "%" : "") + "'");
            }
            return (sbValueList.ToString());
        }
        private questStatus getJoinTargetColumnId(Filter filter, FilterItemJoin filterItemJoin)
        {
            if (filterItemJoin.TargetEntityTypeId == FilterEntityType.Table)
            {
                // Get the TablesetColumnId for the given join.
                FilterTable filterTable = filter.FilterTableList.Find(delegate (FilterTable t)
                {
                    return (filterItemJoin.TargetSchema == t.Schema && filterItemJoin.TargetEntityName == t.Name);
                });
                if (filterTable == null)
                {
                    return (new questStatus(String.Format("ERROR: seeking FilterItemJoin {0} TablesetColumnId not found (FilterTable) [{1}].[{2}]",
                        filterItemJoin.Id, filterItemJoin.TargetSchema, filterItemJoin.TargetEntityName)));
                }

                // Now get the TablesetColumnId for the given column name
                FilterColumn filterColumn = filterTable.FilterColumnList.Find(delegate (FilterColumn fc)
                {
                    return (filterItemJoin.TargetColumnName == fc.TablesetColumn.Name);
                });
                if (filterColumn == null)
                {
                    return (new questStatus(String.Format("ERROR: seeking FilterItemJoin {0} FilterColumn not found (FilterTable) [{1}].[{2}]",
                        filterItemJoin.Id, filterItemJoin.TargetSchema, filterItemJoin.TargetEntityName)));
                }
                filterItemJoin.ColumnId = filterColumn.TablesetColumn.Id;
            }
            else if (filterItemJoin.TargetEntityTypeId == FilterEntityType.View)
            {
                // Get the TablesetColumnId for the given join.
                FilterView filterView = filter.FilterViewList.Find(delegate (FilterView v)
                {
                    return (filterItemJoin.TargetSchema == v.Schema && filterItemJoin.TargetEntityName == v.Name);
                });
                if (filterView == null)
                {
                    return (new questStatus(String.Format("ERROR: seeking FilterItemJoin {0} TablesetColumnId not found (FilterView) [{1}].[{2}]",
                        filterItemJoin.Id, filterItemJoin.TargetSchema, filterItemJoin.TargetEntityName)));
                }

                // Now get the TablesetColumnId for the given column name
                FilterColumn filterColumn = filterView.FilterColumnList.Find(delegate (FilterColumn fc)
                {
                    return (filterItemJoin.TargetColumnName == fc.TablesetColumn.Name);
                });
                if (filterColumn == null)
                {
                    return (new questStatus(String.Format("ERROR: seeking FilterItemJoin {0} FilterColumn not found (FilterView) [{1}].[{2}]",
                        filterItemJoin.Id, filterItemJoin.TargetSchema, filterItemJoin.TargetEntityName)));
                }
                filterItemJoin.ColumnId = filterColumn.TablesetColumn.Id;
            }
            return (new questStatus(Severity.Success));
        }


        #region Hide this, embarrassing
        //
        // Hide this, embarrassing
        //
        // Indemenity clause: database and tableset changes independent of filters hath wreaked havoc in places.
        private questStatus klugieGetFilterItemInfo(FilterItemJoin filterItemJoin, out DatabaseId databaseId, out TablesetId tablesetId)
        {
            // Initialize 
            questStatus status = null;
            databaseId = null;
            tablesetId = null;

            FilterItemId filterItemId = new FilterItemId(filterItemJoin.FilterItemId);
            FilterItem filterItem = null;
            DbFilterItemsMgr dbFilterItemsMgr = new DbFilterItemsMgr(this.UserSession);

            status = dbFilterItemsMgr.Read(filterItemId, out filterItem);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (klugieGetFilterItemInfo(filterItem, out databaseId, out tablesetId));
        }
        private questStatus klugieGetFilterItemInfo(FilterItem filterItem, out DatabaseId databaseId, out TablesetId tablesetId)
        {
            // Initialize 
            questStatus status = null;
            databaseId = null;
            tablesetId = null;


            // Klugie: temporary
            // Just back up and get stuff we need.  (All this due to refactoring, more to do).

            // Get the filter
            FilterId filterId = new FilterId(filterItem.FilterId);
            Filter filter = null;
            DbFiltersMgr dbFiltersMgr = new DbFiltersMgr(this.UserSession);
            status = dbFiltersMgr.Read(filterId, out filter);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }

            // Get the tableset
            DbTablesetsMgr dbTablesetsMgr = new DbTablesetsMgr(this.UserSession);
            tablesetId = new TablesetId(filter.TablesetId);
            Tableset tableset = null;
            status = dbTablesetsMgr.Read(tablesetId, out tableset);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }

            // Return info.
            databaseId = new DatabaseId(tableset.DatabaseId);
            tablesetId = new TablesetId(tableset.Id);
            
            return (new questStatus(Severity.Success));
        }
        #endregion

        #endregion
    }
}
