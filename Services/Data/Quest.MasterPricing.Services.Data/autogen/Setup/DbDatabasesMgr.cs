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


namespace Quest.MasterPricing.Services.Data.Database
{
    public class DbDatabasesMgr : DbMgrSessionBased
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
        public DbDatabasesMgr(UserSession userSession)
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
        public questStatus Create(Quest.Functional.MasterPricing.Database database, out Quest.Functional.MasterPricing.DatabaseId databaseId)
        {
            // Initialize
            questStatus status = null;
            databaseId = null;


            // Data rules.


            // Create the database
            using (MasterPricingEntities dbContext = new MasterPricingEntities())
            {
                status = create(dbContext, database, out databaseId);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Create(DbMgrTransaction trans, Quest.Functional.MasterPricing.Database database, out Quest.Functional.MasterPricing.DatabaseId databaseId)
        {
            // Initialize
            questStatus status = null;
            databaseId = null;


            // Data rules.


            // Create the database in this transaction.
            status = create((MasterPricingEntities)trans.DbContext, database, out databaseId);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Read(Quest.Functional.MasterPricing.DatabaseId databaseId, out Quest.Functional.MasterPricing.Database database)
        {
            // Initialize
            questStatus status = null;
            database = null;


            // Perform read
            using (MasterPricingEntities dbContext = new MasterPricingEntities())
            {
                Quest.Services.Dbio.MasterPricing.Databases _database = null;
                status = read(dbContext, databaseId, out _database);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }
                database = new Quest.Functional.MasterPricing.Database();
                BufferMgr.TransferBuffer(_database, database);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Read(string name, out Quest.Functional.MasterPricing.Database database)
        {
            // Initialize
            questStatus status = null;
            database = null;


            // Perform read
            using (MasterPricingEntities dbContext = new MasterPricingEntities())
            {
                Quest.Services.Dbio.MasterPricing.Databases _database = null;
                status = read(dbContext, name, out _database);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }
                database = new Quest.Functional.MasterPricing.Database();
                BufferMgr.TransferBuffer(_database, database);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Read(DbMgrTransaction trans, Quest.Functional.MasterPricing.DatabaseId databaseId, out Quest.Functional.MasterPricing.Database database)
        {
            // Initialize
            questStatus status = null;
            database = null;


            // Perform read.
            using (MasterPricingEntities dbContext = new MasterPricingEntities())
            {
                Quest.Services.Dbio.MasterPricing.Databases _database = null;
                status = read((MasterPricingEntities)trans.DbContext, databaseId, out _database);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }
                database = new Quest.Functional.MasterPricing.Database();
                BufferMgr.TransferBuffer(_database, database);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Update(Quest.Functional.MasterPricing.Database database)
        {
            // Initialize
            questStatus status = null;


            // Perform update.
            using (MasterPricingEntities dbContext = new MasterPricingEntities())
            {
                status = update(dbContext, database);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Update(DbMgrTransaction trans, Quest.Functional.MasterPricing.Database database)
        {
            // Initialize
            questStatus status = null;
            bool bCreateTransaction = trans == null;


            // Perform update in this transaction.
            status = update((MasterPricingEntities)trans.DbContext, database);
            if (!questStatusDef.IsSuccess(status))
            {
                RollbackTransaction(trans);
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Delete(Quest.Functional.MasterPricing.DatabaseId databaseId)
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
        public questStatus Delete(DbMgrTransaction trans, Quest.Functional.MasterPricing.DatabaseId databaseId)
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
        public questStatus List(QueryOptions queryOptions, out List<Quest.Functional.MasterPricing.Database> databaseList, out QueryResponse queryResponse)
        {
            // Initialize
            questStatus status = null;
            databaseList = null;
            queryResponse = null;


            using (MasterPricingEntities dbContext = new MasterPricingEntities())
            {
                using (DbContextTransaction tx = dbContext.Database.BeginTransaction(IsolationLevel.ReadUncommitted))
                {
                    try
                    {
                        PropertyInfo[] dbProperties = typeof(Quest.Services.Dbio.MasterPricing.Databases).GetProperties().ToArray();
                        int totalRecords = dbContext.Databases.Where(BuildWhereClause(queryOptions, dbProperties)).Count();
                        List<Quest.Services.Dbio.MasterPricing.Databases> _databasesList = dbContext.Databases.Where(BuildWhereClause(queryOptions, dbProperties))
                                .OrderBy(BuildSortString(queryOptions.SortColumns))
                                .Skip(queryOptions.Paging.PageSize * (queryOptions.Paging.PageNumber - 1))
                                .Take(queryOptions.Paging.PageSize).ToList();
                        if (_databasesList == null)
                        {
                            return (new questStatus(Severity.Warning));
                        }
                        databaseList = new List<Quest.Functional.MasterPricing.Database>();
                        foreach (Quest.Services.Dbio.MasterPricing.Databases _database in _databasesList)
                        {
                            Quest.Functional.MasterPricing.Database database = new Quest.Functional.MasterPricing.Database();
                            BufferMgr.TransferBuffer(_database, database);
                            databaseList.Add(database);
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


        #region Databases
        /*----------------------------------------------------------------------------------------------------------------------------------
         * Databases
         *---------------------------------------------------------------------------------------------------------------------------------*/
        private questStatus create(MasterPricingEntities dbContext, Quest.Functional.MasterPricing.Database database, out DatabaseId databaseId)
        {
            // Initialize
            databaseId = null;


            // Perform create
            try
            {
                Quest.Services.Dbio.MasterPricing.Databases _database = new Quest.Services.Dbio.MasterPricing.Databases();
                BufferMgr.TransferBuffer(database, _database);
                dbContext.Databases.Add(_database);
                dbContext.SaveChanges();
                if (_database.Id == 0)
                {
                    return (new questStatus(Severity.Error, "Quest.Functional.MasterPricing.Database not created"));
                }
                databaseId = new DatabaseId(_database.Id);
            }
            catch (System.Exception ex)
            {
                return (new questStatus(Severity.Fatal, String.Format("EXCEPTION: {0}.{1}: {2}",
                        this.GetType().Name, MethodBase.GetCurrentMethod().Name,
                        ex.InnerException != null ? ex.InnerException.Message : ex.Message)));
            }
            return (new questStatus(Severity.Success));
        }
        private questStatus read(MasterPricingEntities dbContext, Quest.Functional.MasterPricing.DatabaseId databaseId, out Quest.Services.Dbio.MasterPricing.Databases database)
        {
            // Initialize
            database = null;


            try
            {
                database = dbContext.Databases.Where(r => r.Id == databaseId.Id).SingleOrDefault();
                if (database == null)
                {
                    return (new questStatus(Severity.Error, String.Format("ERROR: {0}.{1}: {2}",
                            this.GetType().Name, MethodBase.GetCurrentMethod().Name,
                            String.Format("Id {0} not found", databaseId.Id))));
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
        private questStatus read(MasterPricingEntities dbContext, string name, out Quest.Services.Dbio.MasterPricing.Databases database)
        {
            // Initialize
            database = null;


            try
            {
                database = dbContext.Databases.Where(r => r.Name == name).SingleOrDefault();
                if (database == null)
                {
                    return (new questStatus(Severity.Error, String.Format("ERROR: {0}.{1}: {2}",
                            this.GetType().Name, MethodBase.GetCurrentMethod().Name,
                            String.Format("Name {0} not found", name))));
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
        private questStatus update(MasterPricingEntities dbContext, Quest.Functional.MasterPricing.Database database)
        {
            // Initialize 
            questStatus status = null;


            try
            {
                // Read the record.
                DatabaseId databaseId = new DatabaseId(database.Id);
                Quest.Services.Dbio.MasterPricing.Databases _database = null;
                status = read(dbContext, databaseId, out _database);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }

                // Update the record.
                BufferMgr.TransferBuffer(database, _database);
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
        private questStatus delete(MasterPricingEntities dbContext, Quest.Functional.MasterPricing.DatabaseId databaseId)
        {
            // Initialize 
            questStatus status = null;


            try
            {
                // Read the record.
                Quest.Services.Dbio.MasterPricing.Databases _database = null;
                status = read(dbContext, databaseId, out _database);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }

                // Delete the record.
                dbContext.Databases.Remove(_database);
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
