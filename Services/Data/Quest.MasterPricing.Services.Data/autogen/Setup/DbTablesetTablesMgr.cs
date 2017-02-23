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
using Quest.Services.Dbio.MasterPricing;


namespace Quest.MasterPricing.Services.Data.Database
{
    public class DbTablesetTablesMgr : DbMgrSessionBased
    {
        #region Declarations
        /*==================================================================================================================================
         * Declarations
         *=================================================================================================================================*/
        #endregion


        #region Constructors
        /*==================================================================================================================================
         * Constructors
         *=================================================================================================================================*/
        public DbTablesetTablesMgr(UserSession userSession)
            : base(userSession)
        {
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
        public questStatus Create(Quest.Functional.MasterPricing.TablesetTable tablesetTable, out TablesetTableId tablesetTableId)
        {
            // Initialize
            questStatus status = null;
            tablesetTableId = null;


            // Data rules.


            // Create the tablesetTable
            using (MasterPricingEntities dbContext = new MasterPricingEntities())
            {
                status = create(dbContext, tablesetTable, out tablesetTableId);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Create(DbMgrTransaction trans, Quest.Functional.MasterPricing.TablesetTable tablesetTable, out TablesetTableId tablesetTableId)
        {
            // Initialize
            questStatus status = null;
            tablesetTableId = null;


            // Data rules.


            // Create the tablesetTable in this transaction.
            status = create((MasterPricingEntities)trans.DbContext, tablesetTable, out tablesetTableId);
            if (! questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Read(TablesetId tablesetId, out List<Quest.Functional.MasterPricing.TablesetTable> tablesetTableList)
        {
            // Initialize
            questStatus status = null;
            tablesetTableList = null;


            // Perform read
            using (MasterPricingEntities dbContext = new MasterPricingEntities())
            {
                List<Quest.Services.Dbio.MasterPricing.TablesetTables> _tablesetTableList = null;
                status = read(dbContext, tablesetId, out _tablesetTableList);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }
                tablesetTableList = new List<TablesetTable>();
                foreach (Quest.Services.Dbio.MasterPricing.TablesetTables _tablesetTable in _tablesetTableList)
                {
                    Quest.Functional.MasterPricing.TablesetTable tablesetTable = new Quest.Functional.MasterPricing.TablesetTable();
                    BufferMgr.TransferBuffer(_tablesetTable, tablesetTable);
                    tablesetTableList.Add(tablesetTable);
                }
            }
            return (new questStatus(Severity.Success));
        }





        public questStatus Read(TablesetTableId tablesetTableId, out Quest.Functional.MasterPricing.TablesetTable tablesetTable)
        {
            // Initialize
            questStatus status = null;
            tablesetTable = null;


            // Perform read
            using (MasterPricingEntities dbContext = new MasterPricingEntities())
            {
                Quest.Services.Dbio.MasterPricing.TablesetTables _tablesetTable = null;
                status = read(dbContext, tablesetTableId, out _tablesetTable);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }
                tablesetTable = new Quest.Functional.MasterPricing.TablesetTable();
                BufferMgr.TransferBuffer(_tablesetTable, tablesetTable);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Read(DbMgrTransaction trans, TablesetTableId tablesetTableId, out Quest.Functional.MasterPricing.TablesetTable tablesetTable)
        {
            // Initialize
            questStatus status = null;
            tablesetTable = null;


            // Perform read.
            using (MasterPricingEntities dbContext = new MasterPricingEntities())
            {
                Quest.Services.Dbio.MasterPricing.TablesetTables _tablesetTable = null;
                status = read((MasterPricingEntities)trans.DbContext, tablesetTableId, out _tablesetTable);
                if (! questStatusDef.IsSuccess(status))
                {
                    return (status);
                }
                tablesetTable = new Quest.Functional.MasterPricing.TablesetTable();
                BufferMgr.TransferBuffer(_tablesetTable, tablesetTable);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Read(FilterTableTablesetTableId filterTableTablesetTableId, out Quest.Functional.MasterPricing.TablesetTable tablesetTable)
        {
            // Initialize
            questStatus status = null;
            tablesetTable = null;


            // Perform read
            using (MasterPricingEntities dbContext = new MasterPricingEntities())
            {
                Quest.Services.Dbio.MasterPricing.TablesetTables _tablesetTable = null;
                status = read(dbContext, filterTableTablesetTableId, out _tablesetTable);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }
                tablesetTable = new Quest.Functional.MasterPricing.TablesetTable();
                BufferMgr.TransferBuffer(_tablesetTable, tablesetTable);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Read(DbMgrTransaction trans, FilterTableTablesetTableId filterTableTablesetTableId, out Quest.Functional.MasterPricing.TablesetTable tablesetTable)
        {
            // Initialize
            questStatus status = null;
            tablesetTable = null;


            // Perform read.
            using (MasterPricingEntities dbContext = new MasterPricingEntities())
            {
                Quest.Services.Dbio.MasterPricing.TablesetTables _tablesetTable = null;
                status = read((MasterPricingEntities)trans.DbContext, filterTableTablesetTableId, out _tablesetTable);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }
                tablesetTable = new Quest.Functional.MasterPricing.TablesetTable();
                BufferMgr.TransferBuffer(_tablesetTable, tablesetTable);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Update(Quest.Functional.MasterPricing.TablesetTable tablesetTable)
        {
            // Initialize
            questStatus status = null;


            // Perform update.
            using (MasterPricingEntities dbContext = new MasterPricingEntities())
            {
                status = update(dbContext, tablesetTable);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Update(DbMgrTransaction trans, Quest.Functional.MasterPricing.TablesetTable tablesetTable)
        {
            // Initialize
            questStatus status = null;
            bool bCreateTransaction = trans == null;


            // Perform update in this transaction.
            status = update((MasterPricingEntities)trans.DbContext, tablesetTable);
            if (!questStatusDef.IsSuccess(status))
            {
                RollbackTransaction(trans);
                return (status);
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
            if (! questStatusDef.IsSuccess(status))
            {
                RollbackTransaction(trans);
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus List(QueryOptions queryOptions, out List<Quest.Functional.MasterPricing.TablesetTable> tablesetTableList, out QueryResponse queryResponse)
        {
            // Initialize
            questStatus status = null;
            tablesetTableList = null;
            queryResponse = null;


            using (MasterPricingEntities dbContext = new MasterPricingEntities())
            {
                using (DbContextTransaction tx = dbContext.Database.BeginTransaction(IsolationLevel.ReadUncommitted))
                {
                    try
                    {
                        PropertyInfo[] dbProperties = typeof(Quest.Services.Dbio.MasterPricing.TablesetTables).GetProperties().ToArray();
                        int totalRecords = dbContext.vwTablesetTablesList2.Where(BuildWhereClause(queryOptions, dbProperties)).Count();
                        List<Quest.Services.Dbio.MasterPricing.vwTablesetTablesList2> _tablesetTablesList = dbContext.vwTablesetTablesList2.Where(BuildWhereClause(queryOptions, dbProperties))
                                .OrderBy(BuildSortString(queryOptions.SortColumns))
                                .Skip(queryOptions.Paging.PageSize * (queryOptions.Paging.PageNumber - 1))
                                .Take(queryOptions.Paging.PageSize).ToList();
                        if (_tablesetTablesList == null)
                        {
                            return (new questStatus(Severity.Warning));
                        }
                        tablesetTableList = new List<Quest.Functional.MasterPricing.TablesetTable>();
                        foreach (Quest.Services.Dbio.MasterPricing.vwTablesetTablesList2 _tablesetTable in _tablesetTablesList)
                        {
                            Quest.Functional.MasterPricing.TablesetTable tablesetTable = new Quest.Functional.MasterPricing.TablesetTable();
                            BufferMgr.TransferBuffer(_tablesetTable, tablesetTable);
                            tablesetTableList.Add(tablesetTable);
                        }
                        status = BuildQueryResponse(totalRecords, queryOptions, out queryResponse);
                        if (!questStatusDef.IsSuccess(status))
                        {
                            return (status);
                        }
                    }
                    catch (System.Exception ex)
                    {
                        return (new questStatus(Severity.Fatal, String.Format("EXCEPTION: {0}.{1}: {2}",
                                this.GetType().Name, MethodBase.GetCurrentMethod().Name,
                                ex.InnerException != null ? ex.InnerException.Message : ex.Message)));
                    }
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
                throw new System.Exception(status.Message, ex);
            }
            return (new questStatus(Severity.Success));
        }


        #region TablesetTables
        /*----------------------------------------------------------------------------------------------------------------------------------
         * TablesetTables
         *---------------------------------------------------------------------------------------------------------------------------------*/
        private questStatus create(MasterPricingEntities dbContext, Quest.Functional.MasterPricing.TablesetTable tablesetTable, out TablesetTableId tablesetTableId)
        {
            // Initialize
            tablesetTableId = null;


            // Perform create
            try
            {
                Quest.Services.Dbio.MasterPricing.TablesetTables _tablesetTable = new Quest.Services.Dbio.MasterPricing.TablesetTables();
                BufferMgr.TransferBuffer(tablesetTable, _tablesetTable);
                dbContext.TablesetTables.Add(_tablesetTable);
                dbContext.SaveChanges();
                if (_tablesetTable.Id == 0)
                {
                    return (new questStatus(Severity.Error, "Quest.Functional.MasterPricing.TablesetTable not created"));
                }
                tablesetTableId = new TablesetTableId(_tablesetTable.Id);
            }
            catch (System.Exception ex)
            {
                return (new questStatus(Severity.Fatal, String.Format("EXCEPTION: {0}.{1}: {2}",
                        this.GetType().Name, MethodBase.GetCurrentMethod().Name,
                        ex.InnerException != null ? ex.InnerException.Message : ex.Message)));
            }
            return (new questStatus(Severity.Success));
        }
        private questStatus read(MasterPricingEntities dbContext, TablesetId tablesetId, out List<Quest.Services.Dbio.MasterPricing.TablesetTables> tablesetTableList)
        {
            // Initialize
            tablesetTableList = null;


            try
            {
                tablesetTableList = dbContext.TablesetTables.Where(r => r.TablesetId == tablesetId.Id).ToList();
                if (tablesetTableList == null)
                {
                    return (new questStatus(Severity.Error, String.Format("ERROR: {0}.{1}: {2}",
                            this.GetType().Name, MethodBase.GetCurrentMethod().Name,
                            String.Format("TablesetTables for TablesetId {0} not found", tablesetId.Id))));
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
        private questStatus read(MasterPricingEntities dbContext, TablesetTableId tablesetTableId, out Quest.Services.Dbio.MasterPricing.TablesetTables tablesetTable)
        {
            // Initialize
            tablesetTable = null;


            try
            {
                tablesetTable = dbContext.TablesetTables.Where(r => r.Id == tablesetTableId.Id).SingleOrDefault();
                if (tablesetTable == null)
                {
                    return (new questStatus(Severity.Error, String.Format("ERROR: {0}.{1}: {2}",
                            this.GetType().Name, MethodBase.GetCurrentMethod().Name,
                            String.Format("TablesetId.Id {0} not found", tablesetTableId.Id))));
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
        private questStatus read(MasterPricingEntities dbContext, FilterTableTablesetTableId filterTableTablesetTableId, out Quest.Services.Dbio.MasterPricing.TablesetTables tablesetTable)
        {
            // Initialize
            tablesetTable = null;


            try
            {
                tablesetTable = dbContext.TablesetTables.Where(r => r.TablesetId == filterTableTablesetTableId.TablesetId.Id && r.Schema == filterTableTablesetTableId.Schema && r.Name == filterTableTablesetTableId.Name).SingleOrDefault();
                if (tablesetTable == null)
                {
                    return (new questStatus(Severity.Error, String.Format("ERROR: {0}.{1}: {2}",
                            this.GetType().Name, MethodBase.GetCurrentMethod().Name,
                            String.Format("FilterTableTablesetTableId.Id {0} Schema {1} Name {2} not found", filterTableTablesetTableId.TablesetId.Id, filterTableTablesetTableId.Schema, filterTableTablesetTableId.Name))));
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
        private questStatus update(MasterPricingEntities dbContext, Quest.Functional.MasterPricing.TablesetTable tablesetTable)
        {
            // Initialize 
            questStatus status = null;


            try
            {
                // Read the record.
                TablesetTableId tablesetTableId = new TablesetTableId(tablesetTable.Id);
                Quest.Services.Dbio.MasterPricing.TablesetTables _tablesetTable = null;
                status = read(dbContext, tablesetTableId, out _tablesetTable);
                if (! questStatusDef.IsSuccess(status))
                {
                    return (status);
                }

                // Update the record.
                BufferMgr.TransferBuffer(tablesetTable, _tablesetTable);
                dbContext.SaveChanges();
            }
            catch (System.Exception ex)
            {
                return (new questStatus(Severity.Fatal, String.Format("EXCEPTION: {0}.{1}: {2}",
                        this.GetType().Name, MethodBase.GetCurrentMethod().Name,
                        ex.InnerException != null ? ex.InnerException.Message : ex.Message)));
            }
            return (new questStatus(Severity.Success));
        }
        private questStatus delete(MasterPricingEntities dbContext, TablesetId tablesetId)
        {
            try
            {
                dbContext.TablesetTables.RemoveRange(dbContext.TablesetTables.Where(r => r.Id == tablesetId.Id));
                dbContext.SaveChanges();
            }
            catch (System.Exception ex)
            {
                return (new questStatus(Severity.Fatal, String.Format("EXCEPTION: {0}.{1}: {2}",
                        this.GetType().Name, MethodBase.GetCurrentMethod().Name,
                        ex.InnerException != null ? ex.InnerException.Message : ex.Message)));
            }
            return (new questStatus(Severity.Success));
        }
        #endregion

        #endregion
    }
}
