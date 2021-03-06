﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Validation;
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
using Quest.Functional.Logging;
using Quest.Services.Data.Logging;


namespace Quest.MasterPricing.Services.Data.Database
{
    public class DbTablesetsMgr : DbLogsMgr
    {
        #region Declarations
        /*==================================================================================================================================
         * Declarations
         *=================================================================================================================================*/
        DbTablesetLogsMgr _dbTablesetLogsMgr = null;

        #endregion


        #region Constructors
        /*==================================================================================================================================
         * Constructors
         *=================================================================================================================================*/
        public DbTablesetsMgr(UserSession userSession)
            : base(userSession)
        {
            initialize();
        }
        #endregion


        #region Properties
        /*==================================================================================================================================
         * Properties
         *=================================================================================================================================*/
        public bool bLogging
        {
            get
            {
                return (this.LogSettings.bLogDatabases);
            }
        }
        #endregion


        #region Public Methods
        /*==================================================================================================================================
         * Public Methods
         *=================================================================================================================================*/
        public questStatus Create(Quest.Functional.MasterPricing.Tableset tableset, out TablesetId tablesetId)
        {
            // Initialize
            questStatus status = null;
            tablesetId = null;


            // Data rules.


            // Create the tableset
            using (MasterPricingEntities dbContext = new MasterPricingEntities())
            {
                status = create(dbContext, tableset, out tablesetId);
                if (bLogging)
                {
                    TablesetLog tablesetLog = new TablesetLog();
                    tablesetLog.Database = "Database.Id=" + tableset.DatabaseId.ToString();
                    tablesetLog.Name = tableset.Name == null ? "(null)" : tableset.Name;
                    tablesetLog.Event = "CREATE";
                    tablesetLog.Data = status.ToString();
                    TablesetLogId tablesetLogId = null;
                    _dbTablesetLogsMgr.Create(tablesetLog, out tablesetLogId);
                }
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Create(DbMgrTransaction trans, Quest.Functional.MasterPricing.Tableset tableset, out TablesetId tablesetId)
        {
            // Initialize
            questStatus status = null;
            tablesetId = null;


            // Data rules.


            // Create the tableset in this transaction.
            status = create((MasterPricingEntities)trans.DbContext, tableset, out tablesetId);
            if (bLogging)
            {
                TablesetLog tablesetLog = new TablesetLog();
                tablesetLog.Database = "Database.Id=" + tableset.DatabaseId.ToString();
                tablesetLog.Name = tableset.Name == null ? "(null)" : tableset.Name;
                tablesetLog.Event = "CREATE";
                tablesetLog.Data = status.ToString();
                TablesetLogId tablesetLogId = null;
                _dbTablesetLogsMgr.Create(tablesetLog, out tablesetLogId);
            }
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Read(TablesetId tablesetId, out Quest.Functional.MasterPricing.Tableset tableset)
        {
            // Initialize
            questStatus status = null;
            tableset = null;


            // Perform read
            using (MasterPricingEntities dbContext = new MasterPricingEntities())
            {
                Quest.Services.Dbio.MasterPricing.Tablesets _tableset = null;
                status = read(dbContext, tablesetId, out _tableset);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }
                tableset = new Quest.Functional.MasterPricing.Tableset();
                BufferMgr.TransferBuffer(_tableset, tableset);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Read(string name, out Quest.Functional.MasterPricing.Tableset tableset)
        {
            // Initialize
            questStatus status = null;
            tableset = null;


            // Perform read
            using (MasterPricingEntities dbContext = new MasterPricingEntities())
            {
                Quest.Services.Dbio.MasterPricing.Tablesets _tableset = null;
                status = read(dbContext, name, out _tableset);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }
                tableset = new Quest.Functional.MasterPricing.Tableset();
                BufferMgr.TransferBuffer(_tableset, tableset);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Read(DbMgrTransaction trans, TablesetId tablesetId, out Quest.Functional.MasterPricing.Tableset tableset)
        {
            // Initialize
            questStatus status = null;
            tableset = null;


            // Perform read.
            using (MasterPricingEntities dbContext = new MasterPricingEntities())
            {
                Quest.Services.Dbio.MasterPricing.Tablesets _tableset = null;
                status = read((MasterPricingEntities)trans.DbContext, tablesetId, out _tableset);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }
                tableset = new Quest.Functional.MasterPricing.Tableset();
                BufferMgr.TransferBuffer(_tableset, tableset);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Read(DbMgrTransaction trans, DatabaseId databaseId, out List<Quest.Functional.MasterPricing.Tableset> tablesetList)
        {
            // Initialize
            questStatus status = null;
            tablesetList = null;


            // Perform read
            List<Quest.Services.Dbio.MasterPricing.Tablesets> _tablesetList = null;
            status = read((MasterPricingEntities)trans.DbContext, databaseId, out _tablesetList);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            tablesetList = new List<Quest.Functional.MasterPricing.Tableset>();
            foreach (Quest.Services.Dbio.MasterPricing.Tablesets _tableSet in _tablesetList)
            {
                Quest.Functional.MasterPricing.Tableset tableset = new Tableset();
                BufferMgr.TransferBuffer(_tableSet, tableset);
                tablesetList.Add(tableset);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Read(DatabaseId databaseId, out List<Quest.Functional.MasterPricing.Tableset> tablesetList)
        {
            // Initialize
            questStatus status = null;
            tablesetList = null;


            // Perform read
            using (MasterPricingEntities dbContext = new MasterPricingEntities())
            {
                List<Quest.Services.Dbio.MasterPricing.Tablesets> _tablesetList = null;
                status = read(dbContext, databaseId, out _tablesetList);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }
                tablesetList = new List<Tableset>();
                foreach (Quest.Services.Dbio.MasterPricing.Tablesets _tableSet in _tablesetList)
                {
                    Quest.Functional.MasterPricing.Tableset tableSet = new Quest.Functional.MasterPricing.Tableset();
                    BufferMgr.TransferBuffer(_tableSet, tableSet);
                    tablesetList.Add(tableSet);
                }
            }
            return (new questStatus(Severity.Success));
        }

        public questStatus Update(Quest.Functional.MasterPricing.Tableset tableset)
        {
            // Initialize
            questStatus status = null;


            // If the database was changed, delete the tablset configuration and any filters based on the tableset
            questStatus status2 = RemoveTablesetInfoIFDbChanged(tableset);
            if (!questStatusDef.IsSuccess(status2))
            {
                return (status2);
            }

            // Perform update.
            using (MasterPricingEntities dbContext = new MasterPricingEntities())
            {
                status = update(dbContext, tableset);
                if (bLogging)
                {
                    TablesetLog tablesetLog = new TablesetLog();
                    tablesetLog.Database = "Database.Id=" + tableset.DatabaseId.ToString();
                    tablesetLog.Name = tableset.Name == null ? "(null)" : tableset.Name;
                    tablesetLog.Event = "UPDATE";
                    tablesetLog.Data = status.ToString();
                    TablesetLogId tablesetLogId = null;
                    _dbTablesetLogsMgr.Create(tablesetLog, out tablesetLogId);
                }
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }
            }
            return (status2);
        }
        public questStatus Update(DbMgrTransaction trans, Quest.Functional.MasterPricing.Tableset tableset)
        {
            // Initialize
            questStatus status = null;
            bool bCreateTransaction = trans == null;


            // Remove all tableset info and filters based on the tableset if the database changed.
            bool bFiltersRemoved = false;
            status = RemoveTablesetInfoIFDbChanged(trans, tableset);
            if (!questStatusDef.IsSuccess(status))
            {
                if (questStatusDef.IsWarning(status))
                {
                    bFiltersRemoved = true;
                }
                else
                {
                    return (status);
                }
            }

            // Perform update in this transaction.
            status = update((MasterPricingEntities)trans.DbContext, tableset);
            if (bLogging)
            {
                TablesetLog tablesetLog = new TablesetLog();
                tablesetLog.Database = "Database.Id=" + tableset.DatabaseId.ToString();
                tablesetLog.Name = tableset.Name == null ? "(null)" : tableset.Name;
                tablesetLog.Event = "UPDATE";
                tablesetLog.Data = status.ToString();
                TablesetLogId tablesetLogId = null;
                _dbTablesetLogsMgr.Create(tablesetLog, out tablesetLogId);
            }
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            if (bFiltersRemoved)
            {
                return (new questStatus(Severity.Warning, "Tableset database was changed.  All tableset filters were deleted."));
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Delete(TablesetId tablesetId)
        {
            // Initialize
            questStatus status = null;


            // Perform delete.
            using (MasterPricingEntities dbContext = new MasterPricingEntities())
            {
                status = delete(dbContext, tablesetId);
                if (bLogging)
                {
                    TablesetLog tablesetLog = new TablesetLog();
                    tablesetLog.Database = "";
                    tablesetLog.Name = "Tableset.Id=" + tablesetId.Id;
                    tablesetLog.Event = "DELETE";
                    tablesetLog.Data = status.ToString();
                    TablesetLogId tablesetLogId = null;
                    _dbTablesetLogsMgr.Create(tablesetLog, out tablesetLogId);
                }
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Delete(DbMgrTransaction trans, TablesetId tablesetId)
        {
            // Initialize
            questStatus status = null;


            // Perform delete in this transaction.
            status = delete((MasterPricingEntities)trans.DbContext, tablesetId);
            if (bLogging)
            {
                TablesetLog tablesetLog = new TablesetLog();
                tablesetLog.Database = "";
                tablesetLog.Name = "Tableset.Id=" + tablesetId.Id;
                tablesetLog.Event = "DELETE";
                tablesetLog.Data = status.ToString();
                TablesetLogId tablesetLogId = null;
                _dbTablesetLogsMgr.Create(tablesetLog, out tablesetLogId);
            }
            if (!questStatusDef.IsSuccess(status))
            {
                RollbackTransaction(trans);
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus List(QueryOptions queryOptions, out List<Quest.Functional.MasterPricing.Tableset> tablesetList, out QueryResponse queryResponse)
        {
            // Initialize
            questStatus status = null;
            tablesetList = null;
            queryResponse = null;


            using (MasterPricingEntities dbContext = new MasterPricingEntities())
            {
                using (DbContextTransaction tx = dbContext.Database.BeginTransaction(IsolationLevel.ReadUncommitted))
                {
                    try
                    {
                        PropertyInfo[] dbProperties = typeof(Quest.Services.Dbio.MasterPricing.Tablesets).GetProperties().ToArray();
                        int totalRecords = dbContext.vwTablesetsList4.Where(BuildWhereClause(queryOptions, dbProperties)).Count();
                        List<Quest.Services.Dbio.MasterPricing.vwTablesetsList4> _tablesetsList = dbContext.vwTablesetsList4.Where(BuildWhereClause(queryOptions, dbProperties))
                                .OrderBy(BuildSortString(queryOptions.SortColumns))
                                .Skip(queryOptions.Paging.PageSize * (queryOptions.Paging.PageNumber - 1))
                                .Take(queryOptions.Paging.PageSize).ToList();
                        if (_tablesetsList == null)
                        {
                            return (new questStatus(Severity.Warning));
                        }
                        tablesetList = new List<Quest.Functional.MasterPricing.Tableset>();
                        foreach (Quest.Services.Dbio.MasterPricing.vwTablesetsList4 _tableset in _tablesetsList)
                        {
                            Quest.Functional.MasterPricing.Tableset tableset = new Quest.Functional.MasterPricing.Tableset();
                            BufferMgr.TransferBuffer(_tableset, tableset);
                            tablesetList.Add(tableset);
                        }
                        status = BuildQueryResponse(totalRecords, queryOptions, out queryResponse);
                        if (!questStatusDef.IsSuccess(status))
                        {
                            return (status);
                        }
                    }
                    catch (System.Exception ex)
                    {
                        status = new questStatus(Severity.Fatal, String.Format("EXCEPTION: {0}.{1}: {2}",
                                this.GetType().Name, MethodBase.GetCurrentMethod().Name,
                                ex.InnerException != null ? ex.InnerException.Message : ex.Message));
                        LogException(ex, status);
                        return (status);
                    }
                }
            }
            return (new questStatus(Severity.Success));
        }

        public questStatus RemoveTablesetInfoIFDbChanged(Tableset tableset)
        {
            // Initialize 
            questStatus status = null;


            // Start a new transaction
            string transactionName = null;
            status = GetUniqueTransactionName("ClearTablesetConfiguration" + tableset.Id.ToString(), out transactionName);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            DbMgrTransaction trans = null;
            status = BeginTransaction(transactionName, out trans);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }

            // Remove all tableset configuration and filters based on the tableset.
            questStatus status2 = RemoveTablesetInfoIFDbChanged(trans, tableset);
            if (!questStatusDef.IsSuccess(status2))
            {
                // IF warning, it means the filters were removed due to the database changing on the tableset.
                if (! questStatusDef.IsWarning(status2))
                {
                    RollbackTransaction(trans);
                    return (status);
                }
            }


            // Commit transaction
            status = CommitTransaction(trans);
            if (!questStatusDef.IsSuccess(status))
            {
                RollbackTransaction(trans);
                return (status);
            }
            return (status2);
        }
        public questStatus RemoveTablesetInfoIFDbChanged(DbMgrTransaction trans, Tableset tableset)
        {
            // Initialize 
            questStatus status = null;


            // If the database was changed, delete the tablset configuration and any filters based on the tableset
            TablesetId tablesetId = new TablesetId(tableset.Id);
            Quest.Functional.MasterPricing.Tableset _tableset = null;
            status = Read(tablesetId, out _tableset);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            if (tableset.DatabaseId != _tableset.DatabaseId)
            {
                // Remove all tableset filters
                status = RemoveTablesetFilters(trans, tablesetId);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }

                // Remove all tableset configuration
                status = ClearTablesetEntities(trans, tablesetId);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }
                return (new questStatus(Severity.Warning, "Tableset database was changed.  All tableset filters were deleted."));
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

        public questStatus RemoveTablesetFilters(DbMgrTransaction trans, TablesetId tablesetId)
        {
            // Initialize
            questStatus status = null;


            DbFilterMgr dbFilterMgr = new DbFilterMgr(this.UserSession);
            status = dbFilterMgr.Delete(trans, tablesetId);
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
                initLogger();
            }
            catch (System.Exception ex)
            {
                status = new questStatus(Severity.Fatal, String.Format("EXCEPTION: {0}.{1}: {2}",
                        this.GetType().ToString(), MethodInfo.GetCurrentMethod().Name, ex.Message));
                LogException(ex, status);
                throw new System.Exception(status.Message, ex);
            }
            return (new questStatus(Severity.Success));
        }


        #region Logging
        /*----------------------------------------------------------------------------------------------------------------------------------
         * Logging
         *---------------------------------------------------------------------------------------------------------------------------------*/
        private questStatus initLogger()
        {
            // Initialize
            questStatus status = null;


            if (this.LogSettings.bLogTablesets)
            {
                try
                {
                    _dbTablesetLogsMgr = new DbTablesetLogsMgr(this.UserSession);
                }
                catch (System.Exception ex)
                {
                    status = new questStatus(Severity.Fatal, String.Format("EXCEPTION: {0}.{1}: {2}",
                            this.GetType().ToString(), MethodInfo.GetCurrentMethod().Name, ex.Message));
                    LogException(ex, status);
                    return (status);
                }
            }
            return (new questStatus(Severity.Success));
        }
        #endregion


        #region Tablesets
        /*----------------------------------------------------------------------------------------------------------------------------------
         * Tablesets
         *---------------------------------------------------------------------------------------------------------------------------------*/
        private questStatus create(MasterPricingEntities dbContext, Quest.Functional.MasterPricing.Tableset tableset, out TablesetId tablesetId)
        {
            // Initialize
            questStatus status = null;
            tablesetId = null;


            // Perform create
            try
            {
                Quest.Services.Dbio.MasterPricing.Tablesets _tableset = new Quest.Services.Dbio.MasterPricing.Tablesets();
                BufferMgr.TransferBuffer(tableset, _tableset);
                dbContext.Tablesets.Add(_tableset);
                dbContext.SaveChanges();
                if (_tableset.Id == 0)
                {
                    return (new questStatus(Severity.Error, "Quest.Functional.MasterPricing.Tableset not created"));
                }
                tablesetId = new TablesetId(_tableset.Id);
            }
            catch (DbEntityValidationException ex)
            {
                var errorMessages = ex.EntityValidationErrors.SelectMany(x => x.ValidationErrors).Select(x => x.ErrorMessage);
                String fullErrorMessage = string.Join("; ", errorMessages);
                String exceptionMessage = string.Concat(ex.Message, fullErrorMessage);

                status = new questStatus(Severity.Fatal, String.Format("EXCEPTION: {0}.{1}: {2}",
                                        this.GetType().Name, MethodBase.GetCurrentMethod().Name,
                                        exceptionMessage));
                LogException(ex, status);
                return (status);
            }
            catch (System.Exception ex)
            {
                status = new questStatus(Severity.Fatal, String.Format("EXCEPTION: {0}.{1}: {2}",
                        this.GetType().Name, MethodBase.GetCurrentMethod().Name,
                        ex.InnerException != null ? ex.InnerException.Message : ex.Message));
                LogException(ex, status);
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        private questStatus read(MasterPricingEntities dbContext, TablesetId tablesetId, out Quest.Services.Dbio.MasterPricing.Tablesets tableset)
        {
            // Initialize
            questStatus status = null;
            tableset = null;


            try
            {
                tableset = dbContext.Tablesets.Where(r => r.Id == tablesetId.Id).SingleOrDefault();
                if (tableset == null)
                {
                    return (new questStatus(Severity.Error, String.Format("ERROR: {0}.{1}: {2}",
                            this.GetType().Name, MethodBase.GetCurrentMethod().Name,
                            String.Format("Id {0} not found", tablesetId.Id))));
                }
            }
            catch (System.Exception ex)
            {
                status = new questStatus(Severity.Fatal, String.Format("EXCEPTION: {0}.{1}: {2}",
                        this.GetType().Name, MethodBase.GetCurrentMethod().Name,
                        ex.InnerException != null ? ex.InnerException.Message : ex.Message));
                LogException(ex, status);
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        private questStatus read(MasterPricingEntities dbContext, string name, out Quest.Services.Dbio.MasterPricing.Tablesets tableset)
        {
            // Initialize
            questStatus status = null;
            tableset = null;


            try
            {
                tableset = dbContext.Tablesets.Where(r => r.Name == name).SingleOrDefault();
                if (tableset == null)
                {
                    return (new questStatus(Severity.Error, String.Format("ERROR: {0}.{1}: {2}",
                            this.GetType().Name, MethodBase.GetCurrentMethod().Name,
                            String.Format("Name {0} not found", name))));
                }
            }
            catch (System.Exception ex)
            {
                status = new questStatus(Severity.Fatal, String.Format("EXCEPTION: {0}.{1}: {2}",
                        this.GetType().Name, MethodBase.GetCurrentMethod().Name,
                        ex.InnerException != null ? ex.InnerException.Message : ex.Message));
                LogException(ex, status);
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        private questStatus read(MasterPricingEntities dbContext, DatabaseId databaseId, out List<Quest.Services.Dbio.MasterPricing.Tablesets> tablesetList)
        {
            // Initialize
            questStatus status = null;
            tablesetList = null;


            try
            {
                tablesetList = dbContext.Tablesets.Where(r => r.DatabaseId == databaseId.Id).ToList();
                if (tablesetList == null)
                {
                    return (new questStatus(Severity.Error, String.Format("ERROR: {0}.{1}: {2}",
                            this.GetType().Name, MethodBase.GetCurrentMethod().Name,
                            String.Format("DatabaseId {0} not found", databaseId.Id))));
                }
            }
            catch (System.Exception ex)
            {
                status = new questStatus(Severity.Fatal, String.Format("EXCEPTION: {0}.{1}: {2}",
                        this.GetType().Name, MethodBase.GetCurrentMethod().Name,
                        ex.InnerException != null ? ex.InnerException.Message : ex.Message));
                LogException(ex, status);
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        private questStatus update(MasterPricingEntities dbContext, Quest.Functional.MasterPricing.Tableset tableset)
        {
            // Initialize 
            questStatus status = null;


            try
            {
                // Read the record.
                TablesetId tablesetId = new TablesetId(tableset.Id);
                Quest.Services.Dbio.MasterPricing.Tablesets _tableset = null;
                status = read(dbContext, tablesetId, out _tableset);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }

                // Update the record.
                BufferMgr.TransferBuffer(tableset, _tableset);
                dbContext.SaveChanges();
            }
            catch (DbEntityValidationException ex)
            {
                var errorMessages = ex.EntityValidationErrors.SelectMany(x => x.ValidationErrors).Select(x => x.ErrorMessage);
                String fullErrorMessage = string.Join("; ", errorMessages);
                String exceptionMessage = string.Concat(ex.Message, fullErrorMessage);
                status = new questStatus(Severity.Fatal, String.Format("EXCEPTION: {0}.{1}: {2}",
                                        this.GetType().Name, MethodBase.GetCurrentMethod().Name,
                                        exceptionMessage));
                LogException(ex, status);
                return (status);
            }
            catch (System.Exception ex)
            {
                status = new questStatus(Severity.Fatal, String.Format("EXCEPTION: {0}.{1}: {2}",
                        this.GetType().Name, MethodBase.GetCurrentMethod().Name,
                        ex.InnerException != null ? ex.InnerException.Message : ex.Message));
                LogException(ex, status);
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        private questStatus delete(MasterPricingEntities dbContext, TablesetId tablesetId)
        {
            // Initialize 
            questStatus status = null;


            try
            {
                // Read the record.
                Quest.Services.Dbio.MasterPricing.Tablesets _tableset = null;
                status = read(dbContext, tablesetId, out _tableset);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }

                // Delete the record.
                dbContext.Tablesets.Remove(_tableset);
                dbContext.SaveChanges();
            }
            catch (DbEntityValidationException ex)
            {
                var errorMessages = ex.EntityValidationErrors.SelectMany(x => x.ValidationErrors).Select(x => x.ErrorMessage);
                String fullErrorMessage = string.Join("; ", errorMessages);
                String exceptionMessage = string.Concat(ex.Message, fullErrorMessage);
                status = new questStatus(Severity.Fatal, String.Format("EXCEPTION: {0}.{1}: {2}",
                                        this.GetType().Name, MethodBase.GetCurrentMethod().Name,
                                        exceptionMessage));
                LogException(ex, status);
                return (status);
            }
            catch (System.Exception ex)
            {
                status = new questStatus(Severity.Fatal, String.Format("EXCEPTION: {0}.{1}: {2}",
                        this.GetType().Name, MethodBase.GetCurrentMethod().Name,
                        ex.InnerException != null ? ex.InnerException.Message : ex.Message));
                LogException(ex, status);
                return (status);
            }
            status = new questStatus(Severity.Success, "Tableset successfully deleted.  TablesetId.Id=" + tablesetId.Id);
            if (bLogging)
            {
                TablesetLog tablesetLog = new TablesetLog();
                tablesetLog.Name = "TablesetId.Id=" + tablesetId.Id;
                tablesetLog.Event = "DELETE";
                tablesetLog.Data = status.ToString();
                TablesetLogId tablesetLogId = null;
                _dbTablesetLogsMgr.Create(tablesetLog, out tablesetLogId);
            }
            return (status);
        }
        #endregion

        #endregion
    }
}
