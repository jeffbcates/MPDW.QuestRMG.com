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
using Quest.Functional.MasterPricing;
using Quest.MPDW.Services.Data;
using Quest.MPDW.Services.Business;
using Quest.MasterPricing.Services.Data.Database;
using Quest.MasterPricing.Services.Business.Database;
using Quest.MasterPricing.Services.Business.Filters;


namespace Quest.MasterPricing.Services.Business.Tablesets
{
    public class TablesetMgr : Mgr
    {
        #region Declarations
        /*==================================================================================================================================
         * Declarations
         *=================================================================================================================================*/
        private DbTablesetMgr _dbTablesetMgr = null;
        private UserSession _userSession = null;

        #endregion


        #region Constructors
        /*==================================================================================================================================
         * Constructors
         *=================================================================================================================================*/
        public TablesetMgr()
            : base()
        {
            initialize();
        }
        public TablesetMgr(UserSession userSession)
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

        #region Tablesets
        //----------------------------------------------------------------------------------------------------------------------------------
        // Tablesets
        //----------------------------------------------------------------------------------------------------------------------------------
        public questStatus List(QueryOptions queryOptions, out List<Tableset> tableSetList, out QueryResponse queryResponse)
        {
            // Initialize
            questStatus status = null;
            tableSetList = null;


            // List tablesets
            status = _dbTablesetMgr.List(queryOptions, out tableSetList, out queryResponse);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus ReadTablesetConfiguration(TablesetId tablesetId, out TablesetConfiguration tablesetConfiguration)
        {
            // Initialize 
            questStatus status = null;
            tablesetConfiguration = null;


            // Read tableset configuration
            status = _dbTablesetMgr.ReadTablesetConfiguration(tablesetId, out tablesetConfiguration);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }

            // Read database entities
            DatabaseId databaseId = new DatabaseId(tablesetConfiguration.Database.Id);
            DatabaseEntities databaseEntities = null;
            DatabaseMgr databaseMgr = new DatabaseMgr(this.UserSession);
            status = databaseMgr.ReadDatabaseEntities(databaseId, out databaseEntities);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }


            // Sort out what's assigned and not assigned to the tableset.
            List<Table> nonAssignedTableList = new List<Table>();
            List<View> nonAssignedViewList = new List<View>();
            foreach (Table table in databaseEntities.TableList)
            {
                TablesetTable tablesetTable = tablesetConfiguration.TablesetTables.Find(delegate (TablesetTable ts) { return ts.Schema == table.Schema && ts.Name == table.Name; });
                if (tablesetTable == null)
                {
                    nonAssignedTableList.Add(table);
                }
            }
            tablesetConfiguration.DBTableList = nonAssignedTableList;

            // Load database views and columns into configuration NOT assigned to tableset.
            foreach (View view in databaseEntities.ViewList)
            {
                TablesetView tablesetView = tablesetConfiguration.TablesetViews.Find(delegate (TablesetView tv) { return tv.Schema == view.Schema && tv.Name == view.Name; });
                if (tablesetView == null)
                {
                    nonAssignedViewList.Add(view);
                }
            }
            tablesetConfiguration.DBViewList = nonAssignedViewList;


            return (new questStatus(Severity.Success));
        }
        public questStatus SaveTablesetConfiguration(TablesetConfiguration tablesetConfiguration, out TablesetId tablesetId)
        {
            // Initialize 
            questStatus status = null;
            tablesetId = null;
            DbMgrTransaction trans = null;


            try
            {
                // BEGIN TRANSACTION
                status = BeginTransaction("SaveTablesetConfiguration" + Guid.NewGuid().ToString(), out trans);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }


                /*
                 * Update tableset info.
                 */
                // Read the tableset
                TablesetsMgr tablesetsMgr = new TablesetsMgr(this.UserSession);
                TablesetId _tablesetId = new TablesetId(tablesetConfiguration.Tableset.Id);
                Tableset _tableset = null;
                status = tablesetsMgr.Read(trans, _tablesetId, out _tableset);
                if (!questStatusDef.IsSuccess(status))
                {
                    RollbackTransaction(trans);
                    return (status);
                }


                /*
                 * Remove all tableset entities.
                 */
                status = ClearTablesetEntities(trans, _tablesetId);
                if (!questStatusDef.IsSuccess(status))
                {
                    RollbackTransaction(trans);
                    return (status);
                }


                /*
                 * Get database entites.
                 */
                DatabaseId databaseId = new DatabaseId(tablesetConfiguration.Database.Id);
                DatabaseMgr databaseMgr = new DatabaseMgr(this.UserSession);
                DatabaseEntities databaseEntities = null;
                status = databaseMgr.ReadDatabaseEntities(databaseId, out databaseEntities);
                if (!questStatusDef.IsSuccess(status))
                {
                    RollbackTransaction(trans);
                    return (status);
                }


                #region Save tableset info.
                /*
                 * Save tableset info.
                 */
                DbTablesetColumnsMgr dbTablesetColumnsMgr = new DbTablesetColumnsMgr(this.UserSession);

                // Save table info.
                DbTablesetTablesMgr dbTablesetTablesMgr = new DbTablesetTablesMgr(this.UserSession);
                List<TablesetTable> tablesetTableList = new List<TablesetTable>();
                foreach (TablesetTable tablesetTable in tablesetConfiguration.TablesetTables)
                {
                    Table _table = databaseEntities.TableList.Find(delegate (Table t) { return t.Schema == tablesetTable.Schema && t.Name == tablesetTable.Name; });
                    if (_table == null)
                    {
                        RollbackTransaction(trans);
                        return (new questStatus(Severity.Error, String.Format("ERROR: tableset table [{0}].[{1}] not found in database metainfo.  Try refreshing database schema info",
                                tablesetTable.Schema, tablesetTable.Name)));
                    }
                    tablesetTable.TablesetId = _tableset.Id;
                    tablesetTable.Table = _table;
                    tablesetTableList.Add(tablesetTable);


                    // Create tableset table.
                    TablesetTableId tablesetTableId = null;
                    status = dbTablesetTablesMgr.Create(trans, tablesetTable, out tablesetTableId);
                    if (!questStatusDef.IsSuccess(status))
                    {
                        RollbackTransaction(trans);
                        return (status);
                    }

                    foreach (Column column in _table.ColumnList)
                    {
                        Column _column = _table.ColumnList.Find(delegate (Column c) { return c.Name == column.Name; });
                        if (_column == null)
                        {
                            RollbackTransaction(trans);
                            return (new questStatus(Severity.Error, String.Format("ERROR: column [{0}] not found in table [{1}].[{2}] in database metainfo.  Try refreshing database schema info",
                                    column.Name, _table.Schema, _table.Name)));
                        }

                        TablesetColumn tablesetColumn = new TablesetColumn();
                        tablesetColumn.EntityTypeId = EntityType.Table;
                        tablesetColumn.TableSetEntityId = tablesetTableId.Id;
                        tablesetColumn.Name = column.Name;

                        TablesetColumnId tablesetColumnId = null;
                        status = dbTablesetColumnsMgr.Create(trans, tablesetColumn, out tablesetColumnId);
                        if (!questStatusDef.IsSuccess(status))
                        {
                            RollbackTransaction(trans);
                            return (status);
                        }
                    }
                }

                // Save view info.
                DbTablesetViewsMgr dbTablesetViewsMgr = new DbTablesetViewsMgr(this.UserSession);
                List<TablesetView> tablesetViewList = new List<TablesetView>();
                foreach (TablesetView tablesetView in tablesetConfiguration.TablesetViews)
                {
                    View _view = databaseEntities.ViewList.Find(delegate (View v) { return v.Schema == tablesetView.Schema && v.Name == tablesetView.Name; });
                    if (_view == null)
                    {
                        RollbackTransaction(trans);
                        return (new questStatus(Severity.Error, String.Format("ERROR: tableset view [{0}].[{1}] not found in database metainfo.  Try refreshing database schema info",
                                tablesetView.Schema, tablesetView.Name)));
                    }
                    tablesetView.TablesetId = _tableset.Id;
                    tablesetView.View = _view;
                    tablesetViewList.Add(tablesetView);

                    // Create tableset view.
                    TablesetViewId tablesetViewId = null;
                    status = dbTablesetViewsMgr.Create(trans, tablesetView, out tablesetViewId);
                    if (!questStatusDef.IsSuccess(status))
                    {
                        RollbackTransaction(trans);
                        return (status);
                    }

                    foreach (Column column in _view.ColumnList)
                    {
                        Column _column = _view.ColumnList.Find(delegate (Column c) { return c.Name == column.Name; });
                        if (_column == null)
                        {
                            RollbackTransaction(trans);
                            return (new questStatus(Severity.Error, String.Format("ERROR: column [{0}] not found in view [{1}].[{2}] in database metainfo.  Try refreshing database schema info",
                                    column.Name, _view.Schema, _view.Name)));
                        }

                        TablesetColumn tablesetColumn = new TablesetColumn();
                        tablesetColumn.EntityTypeId = EntityType.View;
                        tablesetColumn.TableSetEntityId = tablesetViewId.Id;
                        tablesetColumn.Name = column.Name;

                        TablesetColumnId tablesetColumnId = null;
                        status = dbTablesetColumnsMgr.Create(trans, tablesetColumn, out tablesetColumnId);
                        if (!questStatusDef.IsSuccess(status))
                        {
                            RollbackTransaction(trans);
                            return (status);
                        }
                    }
                }
                #endregion


                // Update tableset.
                _tableset.DatabaseId = tablesetConfiguration.Database.Id;
                status = tablesetsMgr.Update(trans, _tableset);
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

                // Return the tableset id
                tablesetId = new TablesetId(tablesetConfiguration.Tableset.Id);
            }
            catch (System.Exception ex)
            {
                if (trans != null)
                {
                    RollbackTransaction(trans);
                }
                return (new questStatus(Severity.Fatal, String.Format("EXCEPTION: {0}.{1}: {2}",
                        this.GetType().Name, MethodBase.GetCurrentMethod().Name,
                        ex.InnerException != null ? ex.InnerException.Message : ex.Message)));
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus ReadTablesetDataManagement(TablesetId tablesetId, out TablesetDataManagement tablesetDataManagement)
        {
            // Initialize 
            questStatus status = null;
            tablesetDataManagement = null;



            // Read tableset data management
            status = _dbTablesetMgr.ReadTablesetDataManagement(tablesetId, out tablesetDataManagement);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus ClearTablesetEntities(DbMgrTransaction trans, TablesetId tablesetId)
        {
            // Initialize 
            questStatus status = null;


            // Read tableset.
            Tableset tableset = null;
            DbTablesetsMgr dbTablesetsMgr = new DbTablesetsMgr(this.UserSession);
            status = dbTablesetsMgr.Read(tablesetId, out tableset);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }

            DbTablesetColumnsMgr dbTablesetColumnsMgr = new DbTablesetColumnsMgr(this.UserSession);


            // Read all tableset tables
            List<TablesetTable> tablesetTableList = null;
            DbTablesetTablesMgr dbTablesetTablesMgr = new DbTablesetTablesMgr(this.UserSession);
            status = dbTablesetTablesMgr.Read(tablesetId, out tablesetTableList);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }

            // Delete all tablesetColumns to these tables. Then delete all tables in the tableset.
            EntityType entityType = new EntityType();
            entityType.Id = EntityType.Table;
            foreach (TablesetTable tablesetTable in tablesetTableList)
            {
                TableSetEntityId tableSetEntityId = new TableSetEntityId(tablesetTable.Id);
                status = dbTablesetColumnsMgr.Delete(trans, entityType, tableSetEntityId);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }
            }
            dbTablesetTablesMgr.Delete(trans, tablesetId);


            // Read all tableset views
            List<TablesetView> tablesetViewList = null;
            DbTablesetViewsMgr dbTablesetViewsMgr = new DbTablesetViewsMgr(this.UserSession);
            status = dbTablesetViewsMgr.Read(tablesetId, out tablesetViewList);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }

            // Delete all tablesetColumns to these views. Then delete all views in the tableset.
            entityType.Id = EntityType.View;
            foreach (TablesetView tablesetView in tablesetViewList)
            {
                TableSetEntityId tableSetEntityId = new TableSetEntityId(tablesetView.Id);
                status = dbTablesetColumnsMgr.Delete(trans, entityType, tableSetEntityId);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }
            }
            dbTablesetViewsMgr.Delete(trans, tablesetId);


            return (new questStatus(Severity.Success));
        }
        
        public questStatus Delete(TablesetId tablesetId)
        {
            // Initialize
            questStatus status = null;
            DbMgrTransaction trans = null;


            // BEGIN TRANSACTION
            status = BeginTransaction("DeleteTableset_" + Guid.NewGuid().ToString(), out trans);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }


            try
            {
                // Get all filters
                List<Filter> filterList = null;
                FilterMgr filterMgr = new FilterMgr(this.UserSession);
                status = filterMgr.Read(trans, tablesetId, out filterList);
                if (!questStatusDef.IsSuccess(status))
                {
                    RollbackTransaction(trans);
                    return (status);
                }
              
                // Delete tableset filters
                foreach (Filter filter in filterList)
                {
                    FilterId filterId = new FilterId(filter.Id);
                    status = filterMgr.Delete(trans, filterId);
                    if (!questStatusDef.IsSuccess(status))
                    {
                        RollbackTransaction(trans);
                        return (status);
                    }
                }

                // Delete Tableset
                DbTablesetsMgr dbTablesetsMgr = new DbTablesetsMgr(this.UserSession);
                status = dbTablesetsMgr.Delete(tablesetId);
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
            }
            catch (System.Exception ex)
            {
                if (trans != null)
                {
                    RollbackTransaction(trans);
                }
                return (new questStatus(Severity.Fatal, String.Format("EXCEPTION: {0}.{1}: {2}",
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
                _dbTablesetMgr = new DbTablesetMgr();
            }
            catch (System.Exception ex)
            {
                status = new questStatus(Severity.Fatal, String.Format("EXCEPTION: {0}.{1}: {2}",
                        this.GetType().ToString(), MethodInfo.GetCurrentMethod().Name, ex.Message));
                throw new System.Exception(status.Message, ex);
            }
            return (new questStatus(Severity.Success));
        }
        #endregion
    }
}

