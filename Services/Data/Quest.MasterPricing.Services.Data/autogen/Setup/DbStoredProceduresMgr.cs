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
    public class DbStoredProceduresMgr : DbMgrSessionBased
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
        public DbStoredProceduresMgr(UserSession userSession)
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
        public questStatus Create(Quest.Functional.MasterPricing.StoredProcedure storedProcedure, out StoredProcedureId storedProcedureId)
        {
            // Initialize
            questStatus status = null;
            storedProcedureId = null;


            // Data rules.


            // Create the storedProcedure
            using (MasterPricingEntities dbContext = new MasterPricingEntities())
            {
                status = create(dbContext, storedProcedure, out storedProcedureId);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Create(DbMgrTransaction trans, Quest.Functional.MasterPricing.StoredProcedure storedProcedure, out StoredProcedureId storedProcedureId)
        {
            // Initialize
            questStatus status = null;
            storedProcedureId = null;


            // Data rules.


            // Create the storedProcedure in this transaction.
            status = create((MasterPricingEntities)trans.DbContext, storedProcedure, out storedProcedureId);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Create(DbMgrTransaction trans, List<Quest.Functional.MasterPricing.StoredProcedure> storedProcedureList, out List<Quest.Functional.MasterPricing.StoredProcedure> storedProcedureIdList)
        {
            // Initialize
            questStatus status = null;
            storedProcedureIdList = null;


            // Data rules.


            // Create the storedProcedures in this transaction.
            status = create((MasterPricingEntities)trans.DbContext, storedProcedureList, out storedProcedureIdList);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Read(StoredProcedureId storedProcedureId, out Quest.Functional.MasterPricing.StoredProcedure storedProcedure)
        {
            // Initialize
            questStatus status = null;
            storedProcedure = null;


            // Perform read
            using (MasterPricingEntities dbContext = new MasterPricingEntities())
            {
                Quest.Services.Dbio.MasterPricing.StoredProcedures _storedProcedures = null;
                status = read(dbContext, storedProcedureId, out _storedProcedures);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }
                storedProcedure = new Quest.Functional.MasterPricing.StoredProcedure();
                BufferMgr.TransferBuffer(_storedProcedures, storedProcedure);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Read(DbMgrTransaction trans, StoredProcedureId storedProcedureId, out Quest.Functional.MasterPricing.StoredProcedure storedProcedure)
        {
            // Initialize
            questStatus status = null;
            storedProcedure = null;


            // Perform read
            Quest.Services.Dbio.MasterPricing.StoredProcedures _storedProcedures = null;
            status = read((MasterPricingEntities)trans.DbContext, storedProcedureId, out _storedProcedures);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            storedProcedure = new Quest.Functional.MasterPricing.StoredProcedure();
            BufferMgr.TransferBuffer(_storedProcedures, storedProcedure);

            return (new questStatus(Severity.Success));
        }
        public questStatus Read(DatabaseId databaseId, out List<Quest.Functional.MasterPricing.StoredProcedure> storedProcedureList)
        {
            // Initialize
            questStatus status = null;
            storedProcedureList = null;


            // Perform read
            using (MasterPricingEntities dbContext = new MasterPricingEntities())
            {
                List<Quest.Services.Dbio.MasterPricing.StoredProcedures> _storedProceduresList = null;
                status = read(dbContext, databaseId, out _storedProceduresList);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }
                storedProcedureList = new List<StoredProcedure>();
                foreach (Quest.Services.Dbio.MasterPricing.StoredProcedures _storedProcedure in _storedProceduresList)
                {
                    Quest.Functional.MasterPricing.StoredProcedure storedProcedure = new Quest.Functional.MasterPricing.StoredProcedure();
                    BufferMgr.TransferBuffer(_storedProcedure, storedProcedure);
                    storedProcedureList.Add(storedProcedure);
                }
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Read(DbMgrTransaction trans, DatabaseId databaseId, out List<Quest.Functional.MasterPricing.StoredProcedure> storedProcedureList)
        {
            // Initialize
            questStatus status = null;
            storedProcedureList = null;


            // Perform read
            List<Quest.Services.Dbio.MasterPricing.StoredProcedures> _storedProceduresList = null;
            status = read((MasterPricingEntities)trans.DbContext, databaseId, out _storedProceduresList);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            storedProcedureList = new List<StoredProcedure>();
            foreach (Quest.Services.Dbio.MasterPricing.StoredProcedures _storedProcedure in _storedProceduresList)
            {
                Quest.Functional.MasterPricing.StoredProcedure storedProcedure = new Quest.Functional.MasterPricing.StoredProcedure();
                BufferMgr.TransferBuffer(_storedProcedure, storedProcedure);
                storedProcedureList.Add(storedProcedure);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Update(Quest.Functional.MasterPricing.StoredProcedure storedProcedure)
        {
            // Initialize
            questStatus status = null;


            // Perform update.
            using (MasterPricingEntities dbContext = new MasterPricingEntities())
            {
                status = update(dbContext, storedProcedure);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Update(DbMgrTransaction trans, Quest.Functional.MasterPricing.StoredProcedure storedProcedure)
        {
            // Initialize
            questStatus status = null;
            bool bCreateTransaction = trans == null;


            // Perform update in this transaction.
            status = update((MasterPricingEntities)trans.DbContext, storedProcedure);
            if (!questStatusDef.IsSuccess(status))
            {
                RollbackTransaction(trans);
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Delete(StoredProcedureId storedProcedureId)
        {
            // Initialize
            questStatus status = null;


            // Perform delete.
            using (MasterPricingEntities dbContext = new MasterPricingEntities())
            {
                status = delete(dbContext, storedProcedureId);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Delete(DbMgrTransaction trans, StoredProcedureId storedProcedureId)
        {
            // Initialize
            questStatus status = null;


            // Perform delete in this transaction.
            status = delete((MasterPricingEntities)trans.DbContext, storedProcedureId);
            if (!questStatusDef.IsSuccess(status))
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
        public questStatus List(QueryOptions queryOptions, out List<Quest.Functional.MasterPricing.StoredProcedure> storedProcedureList, out QueryResponse queryResponse)
        {
            // Initialize
            questStatus status = null;
            storedProcedureList = null;
            queryResponse = null;


            using (MasterPricingEntities dbContext = new MasterPricingEntities())
            {
                using (DbContextTransaction tx = dbContext.Database.BeginTransaction(IsolationLevel.ReadUncommitted))
                {
                    try
                    {
                        PropertyInfo[] dbProperties = typeof(Quest.Services.Dbio.MasterPricing.StoredProcedures).GetProperties().ToArray();
                        int totalRecords = dbContext.StoredProcedures.Where(BuildWhereClause(queryOptions, dbProperties)).Count();
                        List<Quest.Services.Dbio.MasterPricing.StoredProcedures> _countriesList = dbContext.StoredProcedures.Where(BuildWhereClause(queryOptions, dbProperties))
                                .OrderBy(BuildSortString(queryOptions.SortColumns))
                                .Skip(queryOptions.Paging.PageSize * (queryOptions.Paging.PageNumber - 1))
                                .Take(queryOptions.Paging.PageSize).ToList();
                        if (_countriesList == null)
                        {
                            return (new questStatus(Severity.Warning));
                        }
                        storedProcedureList = new List<Quest.Functional.MasterPricing.StoredProcedure>();
                        foreach (Quest.Services.Dbio.MasterPricing.StoredProcedures _storedProcedures in _countriesList)
                        {
                            Quest.Functional.MasterPricing.StoredProcedure storedProcedure = new Quest.Functional.MasterPricing.StoredProcedure();
                            BufferMgr.TransferBuffer(_storedProcedures, storedProcedure);
                            storedProcedureList.Add(storedProcedure);
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


        #region StoredProcedures
        /*----------------------------------------------------------------------------------------------------------------------------------
         * StoredProcedures
         *---------------------------------------------------------------------------------------------------------------------------------*/
        private questStatus create(MasterPricingEntities dbContext, Quest.Functional.MasterPricing.StoredProcedure storedProcedure, out StoredProcedureId storedProcedureId)
        {
            // Initialize
            storedProcedureId = null;


            // Perform create
            try
            {
                Quest.Services.Dbio.MasterPricing.StoredProcedures _storedProcedures = new Quest.Services.Dbio.MasterPricing.StoredProcedures();
                BufferMgr.TransferBuffer(storedProcedure, _storedProcedures);
                dbContext.StoredProcedures.Add(_storedProcedures);
                dbContext.SaveChanges();
                if (_storedProcedures.Id == 0)
                {
                    return (new questStatus(Severity.Error, "Quest.Functional.MasterPricing.StoredProcedure not created"));
                }
                storedProcedureId = new StoredProcedureId(_storedProcedures.Id);
            }
            catch (System.Exception ex)
            {
                return (new questStatus(Severity.Fatal, String.Format("EXCEPTION: {0}.{1}: {2}",
                        this.GetType().Name, MethodBase.GetCurrentMethod().Name,
                        ex.InnerException != null ? ex.InnerException.Message : ex.Message)));
            }
            return (new questStatus(Severity.Success));
        }
        private questStatus create(MasterPricingEntities dbContext, List<Quest.Functional.MasterPricing.StoredProcedure> storedProcedureList, out List<Quest.Functional.MasterPricing.StoredProcedure> storedProcedureIdList)
        {
            // Initialize
            storedProcedureIdList = null;


            // Perform create
            try
            {
                List<Quest.Services.Dbio.MasterPricing.StoredProcedures> _storedProcedureList = new List<Quest.Services.Dbio.MasterPricing.StoredProcedures>();
                foreach (Quest.Functional.MasterPricing.StoredProcedure storedProcedure in storedProcedureList)
                {
                    Quest.Services.Dbio.MasterPricing.StoredProcedures _storedProcedure = new Quest.Services.Dbio.MasterPricing.StoredProcedures();
                    BufferMgr.TransferBuffer(storedProcedure, _storedProcedure);
                    _storedProcedureList.Add(_storedProcedure);
                }
                dbContext.StoredProcedures.AddRange(_storedProcedureList);
                dbContext.SaveChanges();

                storedProcedureIdList = new List<StoredProcedure>();
                foreach (Quest.Services.Dbio.MasterPricing.StoredProcedures _storedProcedure in _storedProcedureList)
                {
                    Quest.Functional.MasterPricing.StoredProcedure storedProcedure = new StoredProcedure();
                    storedProcedure.Id = _storedProcedure.Id;
                    storedProcedureIdList.Add(storedProcedure);
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
        private questStatus read(MasterPricingEntities dbContext, StoredProcedureId storedProcedureId, out Quest.Services.Dbio.MasterPricing.StoredProcedures storedProcedure)
        {
            // Initialize
            storedProcedure = null;


            try
            {
                storedProcedure = dbContext.StoredProcedures.Where(r => r.Id == storedProcedureId.Id).SingleOrDefault();
                if (storedProcedure == null)
                {
                    return (new questStatus(Severity.Error, String.Format("ERROR: {0}.{1}: {2}",
                            this.GetType().Name, MethodBase.GetCurrentMethod().Name,
                            String.Format("Id {0} not found", storedProcedureId.Id))));
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
        private questStatus read(MasterPricingEntities dbContext, DatabaseId databaseId, out List<Quest.Services.Dbio.MasterPricing.StoredProcedures> storedProcedureList)
        {
            // Initialize
            storedProcedureList = null;


            try
            {
                storedProcedureList = dbContext.StoredProcedures.Where(r => r.DatabaseId == databaseId.Id).ToList();
                if (storedProcedureList == null)
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
        private questStatus update(MasterPricingEntities dbContext, Quest.Functional.MasterPricing.StoredProcedure storedProcedure)
        {
            // Initialize 
            questStatus status = null;


            try
            {
                // Read the record.
                StoredProcedureId storedProcedureId = new StoredProcedureId(storedProcedure.Id);
                Quest.Services.Dbio.MasterPricing.StoredProcedures _storedProcedures = null;
                status = read(dbContext, storedProcedureId, out _storedProcedures);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }

                // Update the record.
                BufferMgr.TransferBuffer(storedProcedure, _storedProcedures);
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
        private questStatus delete(MasterPricingEntities dbContext, StoredProcedureId storedProcedureId)
        {
            // Initialize 
            questStatus status = null;


            try
            {
                // Read the record.
                Quest.Services.Dbio.MasterPricing.StoredProcedures _storedProcedures = null;
                status = read(dbContext, storedProcedureId, out _storedProcedures);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }

                ////// Delete parameters.
                ////DbStoredProcedureParametersMgr dbStoredProcedureParametersMgr = new DbStoredProcedureParametersMgr(this.UserSession);
                ////status = dbStoredProcedureParametersMgr.Delete(storedProcedureId);
                ////if (!questStatusDef.IsSuccess(status))
                ////{
                ////    return (status);
                ////}


                // Delete the record.
                dbContext.StoredProcedures.Remove(_storedProcedures);
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
                // Read all storedProcedures for this stored.
                List<Quest.Services.Dbio.MasterPricing.StoredProcedures> _storedProceduresList = null;
                status = read(dbContext, databaseId, out _storedProceduresList);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }

                // Delete their parameters.
                DbStoredProcedureParametersMgr dbStoredProcedureParametersMgr = new DbStoredProcedureParametersMgr(this.UserSession);
                foreach (Quest.Services.Dbio.MasterPricing.StoredProcedures _storedProcedure in _storedProceduresList)
                {
                    StoredProcedureId storedProcedureId = new StoredProcedureId(_storedProcedure.Id);
                    status = dbStoredProcedureParametersMgr.Delete(storedProcedureId);
                    if (!questStatusDef.IsSuccess(status))
                    {
                        return (status);
                    }
                }

                // Delete the records.
                dbContext.StoredProcedures.RemoveRange(_storedProceduresList);
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
