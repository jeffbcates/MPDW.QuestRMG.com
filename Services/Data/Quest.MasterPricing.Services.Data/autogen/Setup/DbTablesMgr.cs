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
    public class DbTablesMgr : DbMgrSessionBased
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
        public DbTablesMgr(UserSession userSession)
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
        public questStatus Create(Quest.Functional.MasterPricing.Table table, out TableId tableId)
        {
            // Initialize
            questStatus status = null;
            tableId = null;


            // Data rules.


            // Create the table
            using (MasterPricingEntities dbContext = new MasterPricingEntities())
            {
                status = create(dbContext, table, out tableId);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Create(DbMgrTransaction trans, Quest.Functional.MasterPricing.Table table, out TableId tableId)
        {
            // Initialize
            questStatus status = null;
            tableId = null;


            // Data rules.


            // Create the table in this transaction.
            status = create((MasterPricingEntities)trans.DbContext, table, out tableId);
            if (! questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Create(DbMgrTransaction trans, List<Quest.Functional.MasterPricing.Table> tableList, out List<Quest.Functional.MasterPricing.Table> tableIdList)
        {
            // Initialize
            questStatus status = null;
            tableIdList = null;


            // Data rules.


            // Create the tables in this transaction.
            status = create((MasterPricingEntities)trans.DbContext, tableList, out tableIdList);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Read(TableId tableId, out Quest.Functional.MasterPricing.Table table)
        {
            // Initialize
            questStatus status = null;
            table = null;


            // Perform read
            using (MasterPricingEntities dbContext = new MasterPricingEntities())
            {
                Quest.Services.Dbio.MasterPricing.Tables _table = null;
                status = read(dbContext, tableId, out _table);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }
                table = new Quest.Functional.MasterPricing.Table();
                BufferMgr.TransferBuffer(_table, table);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Read(DatabaseId databaseId, string schema, string name, out Quest.Functional.MasterPricing.Table table)
        {
            // Initialize
            questStatus status = null;
            table = null;


            // Perform read
            using (MasterPricingEntities dbContext = new MasterPricingEntities())
            {
                Quest.Services.Dbio.MasterPricing.Tables _table = null;
                status = read(dbContext, databaseId, schema, name, out _table);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }
                table = new Quest.Functional.MasterPricing.Table();
                BufferMgr.TransferBuffer(_table, table);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Read(DatabaseId databaseId, out List<Quest.Functional.MasterPricing.Table> tableList)
        {
            // Initialize
            questStatus status = null;
            tableList = null;


            // Perform read
            using (MasterPricingEntities dbContext = new MasterPricingEntities())
            {
                List<Quest.Services.Dbio.MasterPricing.Tables> _tableList = null;
                status = read(dbContext, databaseId, out _tableList);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }
                tableList = new List<Table>();
                foreach (Quest.Services.Dbio.MasterPricing.Tables _table in _tableList)
                {
                    Quest.Functional.MasterPricing.Table table = new Table();
                    BufferMgr.TransferBuffer(_table, table);
                    tableList.Add(table);
                }
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Read(DbMgrTransaction trans, TableId tableId, out Quest.Functional.MasterPricing.Table table)
        {
            // Initialize
            questStatus status = null;
            table = null;


            // Perform read.
            using (MasterPricingEntities dbContext = new MasterPricingEntities())
            {
                Quest.Services.Dbio.MasterPricing.Tables _table = null;
                status = read((MasterPricingEntities)trans.DbContext, tableId, out _table);
                if (! questStatusDef.IsSuccess(status))
                {
                    return (status);
                }
                table = new Quest.Functional.MasterPricing.Table();
                BufferMgr.TransferBuffer(_table, table);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Update(Quest.Functional.MasterPricing.Table table)
        {
            // Initialize
            questStatus status = null;


            // Perform update.
            using (MasterPricingEntities dbContext = new MasterPricingEntities())
            {
                status = update(dbContext, table);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Update(DbMgrTransaction trans, Quest.Functional.MasterPricing.Table table)
        {
            // Initialize
            questStatus status = null;
            bool bCreateTransaction = trans == null;


            // Perform update in this transaction.
            status = update((MasterPricingEntities)trans.DbContext, table);
            if (!questStatusDef.IsSuccess(status))
            {
                RollbackTransaction(trans);
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Delete(TableId tableId)
        {
            // Initialize
            questStatus status = null;


            // Perform delete.
            using (MasterPricingEntities dbContext = new MasterPricingEntities())
            {
                status = delete(dbContext, tableId);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Delete(DbMgrTransaction trans, TableId tableId)
        {
            // Initialize
            questStatus status = null;


            // Perform delete in this transaction.
            status = delete((MasterPricingEntities)trans.DbContext, tableId);
            if (! questStatusDef.IsSuccess(status))
            {
                RollbackTransaction(trans);
                return (status);
            }
            return (new questStatus(Severity.Success));
        }

        public questStatus Delete(DatabaseId databaseId)
        {
            // Initialize
            questStatus status = null;


            // Perform delete.
            using (MasterPricingEntities dbContext = new MasterPricingEntities())
            {
                status = delete(dbContext, databaseId);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Delete(DbMgrTransaction trans, DatabaseId databaseId)
        {
            // Initialize
            questStatus status = null;


            // Perform delete in this transaction.
            status = delete((MasterPricingEntities)trans.DbContext, databaseId);
            if (!questStatusDef.IsSuccess(status))
            {
                RollbackTransaction(trans);
                return (status);
            }
            return (new questStatus(Severity.Success));
        }



        public questStatus List(QueryOptions queryOptions, out List<Quest.Functional.MasterPricing.Table> tableList, out QueryResponse queryResponse)
        {
            // Initialize
            questStatus status = null;
            tableList = null;
            queryResponse = null;


            using (MasterPricingEntities dbContext = new MasterPricingEntities())
            {
                using (DbContextTransaction tx = dbContext.Database.BeginTransaction(IsolationLevel.ReadUncommitted))
                {
                    try
                    {
                        PropertyInfo[] dbProperties = typeof(Quest.Services.Dbio.MasterPricing.Tables).GetProperties().ToArray();
                        int totalRecords = dbContext.Tables.Where(BuildWhereClause(queryOptions, dbProperties)).Count();
                        List<Quest.Services.Dbio.MasterPricing.Tables> _countriesList = dbContext.Tables.Where(BuildWhereClause(queryOptions, dbProperties))
                                .OrderBy(BuildSortString(queryOptions.SortColumns))
                                .Skip(queryOptions.Paging.PageSize * (queryOptions.Paging.PageNumber - 1))
                                .Take(queryOptions.Paging.PageSize).ToList();
                        if (_countriesList == null)
                        {
                            return (new questStatus(Severity.Warning));
                        }
                        tableList = new List<Quest.Functional.MasterPricing.Table>();
                        foreach (Quest.Services.Dbio.MasterPricing.Tables _table in _countriesList)
                        {
                            Quest.Functional.MasterPricing.Table table = new Quest.Functional.MasterPricing.Table();
                            BufferMgr.TransferBuffer(_table, table);
                            tableList.Add(table);
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


        #region Tables
        /*----------------------------------------------------------------------------------------------------------------------------------
         * Tables
         *---------------------------------------------------------------------------------------------------------------------------------*/
        private questStatus create(MasterPricingEntities dbContext, Quest.Functional.MasterPricing.Table table, out TableId tableId)
        {
            // Initialize
            tableId = null;


            // Perform create
            try
            {
                Quest.Services.Dbio.MasterPricing.Tables _table = new Quest.Services.Dbio.MasterPricing.Tables();
                BufferMgr.TransferBuffer(table, _table);
                dbContext.Tables.Add(_table);
                dbContext.SaveChanges();
                if (_table.Id == 0)
                {
                    return (new questStatus(Severity.Error, "Quest.Functional.MasterPricing.Table not created"));
                }
                tableId = new TableId(_table.Id);
            }
            catch (System.Exception ex)
            {
                return (new questStatus(Severity.Fatal, String.Format("EXCEPTION: {0}.{1}: {2}",
                        this.GetType().Name, MethodBase.GetCurrentMethod().Name,
                        ex.InnerException != null ? ex.InnerException.Message : ex.Message)));
            }
            return (new questStatus(Severity.Success));
        }
        private questStatus create(MasterPricingEntities dbContext, List<Quest.Functional.MasterPricing.Table> tableList, out List<Quest.Functional.MasterPricing.Table> tableIdList)
        {
            // Initialize
            tableIdList = null;


            // Perform create
            try
            {
                List<Quest.Services.Dbio.MasterPricing.Tables> _tableList = new List<Tables>();
                foreach (Quest.Functional.MasterPricing.Table table in tableList)
                {
                    Quest.Services.Dbio.MasterPricing.Tables _table = new Quest.Services.Dbio.MasterPricing.Tables();
                    BufferMgr.TransferBuffer(table, _table);
                    _tableList.Add(_table);
                }
                dbContext.Tables.AddRange(_tableList);
                dbContext.SaveChanges();

                tableIdList = new List<Table>();
                foreach (Quest.Services.Dbio.MasterPricing.Tables _table in _tableList)
                {
                    Quest.Functional.MasterPricing.Table table = new Table();
                    BufferMgr.TransferBuffer(_table, table);
                    tableIdList.Add(table);
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
        private questStatus read(MasterPricingEntities dbContext, TableId tableId, out Quest.Services.Dbio.MasterPricing.Tables table)
        {
            // Initialize
            table = null;


            try
            {
                table = dbContext.Tables.Where(r => r.Id == tableId.Id).SingleOrDefault();
                if (table == null)
                {
                    return (new questStatus(Severity.Error, String.Format("ERROR: {0}.{1}: {2}",
                            this.GetType().Name, MethodBase.GetCurrentMethod().Name,
                            String.Format("Id {0} not found", tableId.Id))));
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
        private questStatus read(MasterPricingEntities dbContext, DatabaseId databaseId, string schema, string name, out Quest.Services.Dbio.MasterPricing.Tables table)
        {
            // Initialize
            table = null;


            try
            {
                table = dbContext.Tables.Where(r => r.DatabaseId == databaseId.Id && r.Schema == schema && r.Name == name).SingleOrDefault();
                if (table == null)
                {
                    return (new questStatus(Severity.Error, String.Format("ERROR: {0}.{1}: {2}",
                            this.GetType().Name, MethodBase.GetCurrentMethod().Name,
                            String.Format("Schema {0} Name {1} not found", schema, name))));
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
        private questStatus read(MasterPricingEntities dbContext, DatabaseId databaseId, out List<Quest.Services.Dbio.MasterPricing.Tables> tablesList)
        {
            // Initialize
            tablesList = null;


            try
            {
                tablesList = dbContext.Tables.Where(r => r.DatabaseId == databaseId.Id).ToList();
                if (tablesList == null)
                {
                    return (new questStatus(Severity.Error, String.Format("ERROR: {0}.{1}: {2}",
                            this.GetType().Name, MethodBase.GetCurrentMethod().Name,
                            String.Format("DatabaseId {0} not found", databaseId.Id))));
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
        private questStatus update(MasterPricingEntities dbContext, Quest.Functional.MasterPricing.Table table)
        {
            // Initialize 
            questStatus status = null;


            try
            {
                // Read the record.
                TableId tableId = new TableId(table.Id);
                Quest.Services.Dbio.MasterPricing.Tables _table = null;
                status = read(dbContext, tableId, out _table);
                if (! questStatusDef.IsSuccess(status))
                {
                    return (status);
                }

                // Update the record.
                BufferMgr.TransferBuffer(table, _table);
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
        private questStatus delete(MasterPricingEntities dbContext, TableId tableId)
        {
            // Initialize 
            questStatus status = null;


            try
            {
                // Read the record.
                Quest.Services.Dbio.MasterPricing.Tables _table = null;
                status = read(dbContext, tableId, out _table);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }

                // Delete the record.
                dbContext.Tables.Remove(_table);
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
        private questStatus delete(MasterPricingEntities dbContext, DatabaseId databaseId)
        {
            // Initialize 
            questStatus status = null;


            try
            {
                // Read the record.
                List<Quest.Services.Dbio.MasterPricing.Tables> _tablesList = null;
                status = read(dbContext, databaseId, out _tablesList);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }

                // Delete the record.
                dbContext.Tables.RemoveRange(_tablesList);
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
