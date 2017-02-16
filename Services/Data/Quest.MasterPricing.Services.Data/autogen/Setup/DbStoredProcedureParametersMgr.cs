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
    public class DbStoredProcedureParametersMgr : DbMgrSessionBased
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
        public DbStoredProcedureParametersMgr(UserSession userSession)
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
        public questStatus Create(Quest.Functional.MasterPricing.StoredProcedureParameter storedProcedureParameter, out StoredProcedureParameterId storedProcedureParameterId)
        {
            // Initialize
            questStatus status = null;
            storedProcedureParameterId = null;


            // Data rules.


            // Create the storedProcedureParameter
            using (MasterPricingEntities dbContext = new MasterPricingEntities())
            {
                status = create(dbContext, storedProcedureParameter, out storedProcedureParameterId);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Create(DbMgrTransaction trans, Quest.Functional.MasterPricing.StoredProcedureParameter storedProcedureParameter, out StoredProcedureParameterId storedProcedureParameterId)
        {
            // Initialize
            questStatus status = null;
            storedProcedureParameterId = null;


            // Data rules.


            // Create the storedProcedureParameter in this transaction.
            status = create((MasterPricingEntities)trans.DbContext, storedProcedureParameter, out storedProcedureParameterId);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Create(DbMgrTransaction trans, List<Quest.Functional.MasterPricing.StoredProcedureParameter> storedProcedureParameterList, out List<Quest.Functional.MasterPricing.StoredProcedureParameter> storedProcedureParameterIdList)
        {
            // Initialize
            questStatus status = null;
            storedProcedureParameterIdList = null;


            // Data rules.


            // Create the storedProcedureParameters in this transaction.
            status = create((MasterPricingEntities)trans.DbContext, storedProcedureParameterList, out storedProcedureParameterIdList);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Read(StoredProcedureParameterId storedProcedureParameterId, out Quest.Functional.MasterPricing.StoredProcedureParameter storedProcedureParameter)
        {
            // Initialize
            questStatus status = null;
            storedProcedureParameter = null;


            // Perform read
            using (MasterPricingEntities dbContext = new MasterPricingEntities())
            {
                Quest.Services.Dbio.MasterPricing.StoredProcedureParameters _storedProcedureParameters = null;
                status = read(dbContext, storedProcedureParameterId, out _storedProcedureParameters);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }
                storedProcedureParameter = new Quest.Functional.MasterPricing.StoredProcedureParameter();
                BufferMgr.TransferBuffer(_storedProcedureParameters, storedProcedureParameter);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Read(DbMgrTransaction trans, StoredProcedureParameterId storedProcedureParameterId, out Quest.Functional.MasterPricing.StoredProcedureParameter storedProcedureParameter)
        {
            // Initialize
            questStatus status = null;
            storedProcedureParameter = null;


            // Perform read
            Quest.Services.Dbio.MasterPricing.StoredProcedureParameters _storedProcedureParameters = null;
            status = read((MasterPricingEntities)trans.DbContext, storedProcedureParameterId, out _storedProcedureParameters);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            storedProcedureParameter = new Quest.Functional.MasterPricing.StoredProcedureParameter();
            BufferMgr.TransferBuffer(_storedProcedureParameters, storedProcedureParameter);

            return (new questStatus(Severity.Success));
        }
        public questStatus Read(StoredProcedureId storedProcedureId, out List<Quest.Functional.MasterPricing.StoredProcedureParameter> storedProcedureParameterList)
        {
            // Initialize
            questStatus status = null;
            storedProcedureParameterList = null;


            // Perform read
            using (MasterPricingEntities dbContext = new MasterPricingEntities())
            {
                List<Quest.Services.Dbio.MasterPricing.StoredProcedureParameters> _storedProcedureParametersList = null;
                status = read(dbContext, storedProcedureId, out _storedProcedureParametersList);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }
                storedProcedureParameterList = new List<StoredProcedureParameter>();
                foreach (Quest.Services.Dbio.MasterPricing.StoredProcedureParameters _storedProcedureParameter in _storedProcedureParametersList)
                {
                    Quest.Functional.MasterPricing.StoredProcedureParameter storedProcedureParameter = new Quest.Functional.MasterPricing.StoredProcedureParameter();
                    BufferMgr.TransferBuffer(_storedProcedureParameter, storedProcedureParameter);
                    storedProcedureParameterList.Add(storedProcedureParameter);
                }
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Read(DbMgrTransaction trans, StoredProcedureId storedProcedureId, out List<Quest.Functional.MasterPricing.StoredProcedureParameter> storedProcedureParameterList)
        {
            // Initialize
            questStatus status = null;
            storedProcedureParameterList = null;


            // Perform read
            List<Quest.Services.Dbio.MasterPricing.StoredProcedureParameters> _storedProcedureParametersList = null;
            status = read((MasterPricingEntities)trans.DbContext, storedProcedureId, out _storedProcedureParametersList);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            storedProcedureParameterList = new List<StoredProcedureParameter>();
            foreach (Quest.Services.Dbio.MasterPricing.StoredProcedureParameters _storedProcedureParameter in _storedProcedureParametersList)
            {
                Quest.Functional.MasterPricing.StoredProcedureParameter storedProcedureParameter = new Quest.Functional.MasterPricing.StoredProcedureParameter();
                BufferMgr.TransferBuffer(_storedProcedureParameter, storedProcedureParameter);
                storedProcedureParameterList.Add(storedProcedureParameter);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Update(Quest.Functional.MasterPricing.StoredProcedureParameter storedProcedureParameter)
        {
            // Initialize
            questStatus status = null;


            // Perform update.
            using (MasterPricingEntities dbContext = new MasterPricingEntities())
            {
                status = update(dbContext, storedProcedureParameter);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Update(DbMgrTransaction trans, Quest.Functional.MasterPricing.StoredProcedureParameter storedProcedureParameter)
        {
            // Initialize
            questStatus status = null;
            bool bCreateTransaction = trans == null;


            // Perform update in this transaction.
            status = update((MasterPricingEntities)trans.DbContext, storedProcedureParameter);
            if (!questStatusDef.IsSuccess(status))
            {
                RollbackTransaction(trans);
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Delete(StoredProcedureParameterId storedProcedureParameterId)
        {
            // Initialize
            questStatus status = null;


            // Perform delete.
            using (MasterPricingEntities dbContext = new MasterPricingEntities())
            {
                status = delete(dbContext, storedProcedureParameterId);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Delete(DbMgrTransaction trans, StoredProcedureParameterId storedProcedureParameterId)
        {
            // Initialize
            questStatus status = null;


            // Perform delete in this transaction.
            status = delete((MasterPricingEntities)trans.DbContext, storedProcedureParameterId);
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
        public questStatus List(QueryOptions queryOptions, out List<Quest.Functional.MasterPricing.StoredProcedureParameter> storedProcedureParameterList, out QueryResponse queryResponse)
        {
            // Initialize
            questStatus status = null;
            storedProcedureParameterList = null;
            queryResponse = null;


            using (MasterPricingEntities dbContext = new MasterPricingEntities())
            {
                using (DbContextTransaction tx = dbContext.Database.BeginTransaction(IsolationLevel.ReadUncommitted))
                {
                    try
                    {
                        PropertyInfo[] dbProperties = typeof(Quest.Services.Dbio.MasterPricing.StoredProcedureParameters).GetProperties().ToArray();
                        int totalRecords = dbContext.StoredProcedureParameters.Where(BuildWhereClause(queryOptions, dbProperties)).Count();
                        List<Quest.Services.Dbio.MasterPricing.StoredProcedureParameters> _countriesList = dbContext.StoredProcedureParameters.Where(BuildWhereClause(queryOptions, dbProperties))
                                .OrderBy(BuildSortString(queryOptions.SortColumns))
                                .Skip(queryOptions.Paging.PageSize * (queryOptions.Paging.PageNumber - 1))
                                .Take(queryOptions.Paging.PageSize).ToList();
                        if (_countriesList == null)
                        {
                            return (new questStatus(Severity.Warning));
                        }
                        storedProcedureParameterList = new List<Quest.Functional.MasterPricing.StoredProcedureParameter>();
                        foreach (Quest.Services.Dbio.MasterPricing.StoredProcedureParameters _storedProcedureParameters in _countriesList)
                        {
                            Quest.Functional.MasterPricing.StoredProcedureParameter storedProcedureParameter = new Quest.Functional.MasterPricing.StoredProcedureParameter();
                            BufferMgr.TransferBuffer(_storedProcedureParameters, storedProcedureParameter);
                            storedProcedureParameterList.Add(storedProcedureParameter);
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


        #region StoredProcedureParameters
        /*----------------------------------------------------------------------------------------------------------------------------------
         * StoredProcedureParameters
         *---------------------------------------------------------------------------------------------------------------------------------*/
        private questStatus create(MasterPricingEntities dbContext, Quest.Functional.MasterPricing.StoredProcedureParameter storedProcedureParameter, out StoredProcedureParameterId storedProcedureParameterId)
        {
            // Initialize
            storedProcedureParameterId = null;


            // Perform create
            try
            {
                Quest.Services.Dbio.MasterPricing.StoredProcedureParameters _storedProcedureParameters = new Quest.Services.Dbio.MasterPricing.StoredProcedureParameters();
                BufferMgr.TransferBuffer(storedProcedureParameter, _storedProcedureParameters);
                dbContext.StoredProcedureParameters.Add(_storedProcedureParameters);
                dbContext.SaveChanges();
                if (_storedProcedureParameters.Id == 0)
                {
                    return (new questStatus(Severity.Error, "Quest.Functional.MasterPricing.StoredProcedureParameter not created"));
                }
                storedProcedureParameterId = new StoredProcedureParameterId(_storedProcedureParameters.Id);
            }
            catch (System.Exception ex)
            {
                return (new questStatus(Severity.Fatal, String.Format("EXCEPTION: {0}.{1}: {2}",
                        this.GetType().Name, MethodBase.GetCurrentMethod().Name,
                        ex.InnerException != null ? ex.InnerException.Message : ex.Message)));
            }
            return (new questStatus(Severity.Success));
        }
        private questStatus create(MasterPricingEntities dbContext, List<Quest.Functional.MasterPricing.StoredProcedureParameter> storedProcedureParameterList, out List<Quest.Functional.MasterPricing.StoredProcedureParameter> storedProcedureParameterIdList)
        {
            // Initialize
            storedProcedureParameterIdList = null;


            // Perform create
            try
            {
                List<Quest.Services.Dbio.MasterPricing.StoredProcedureParameters> _storedProcedureParameterList = new List<Quest.Services.Dbio.MasterPricing.StoredProcedureParameters>();
                foreach (Quest.Functional.MasterPricing.StoredProcedureParameter storedProcedureParameter in storedProcedureParameterList)
                {
                    Quest.Services.Dbio.MasterPricing.StoredProcedureParameters _storedProcedureParameter = new Quest.Services.Dbio.MasterPricing.StoredProcedureParameters();
                    BufferMgr.TransferBuffer(storedProcedureParameter, _storedProcedureParameter);
                    _storedProcedureParameterList.Add(_storedProcedureParameter);
                }
                dbContext.StoredProcedureParameters.AddRange(_storedProcedureParameterList);
                dbContext.SaveChanges();

                storedProcedureParameterIdList = new List<StoredProcedureParameter>();
                foreach (Quest.Services.Dbio.MasterPricing.StoredProcedureParameters _storedProcedureParameter in _storedProcedureParameterList)
                {
                    Quest.Functional.MasterPricing.StoredProcedureParameter storedProcedureParameter = new StoredProcedureParameter();
                    storedProcedureParameter.Id = _storedProcedureParameter.Id;
                    storedProcedureParameterIdList.Add(storedProcedureParameter);
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
        private questStatus read(MasterPricingEntities dbContext, StoredProcedureParameterId storedProcedureParameterId, out Quest.Services.Dbio.MasterPricing.StoredProcedureParameters storedProcedureParameter)
        {
            // Initialize
            storedProcedureParameter = null;


            try
            {
                storedProcedureParameter = dbContext.StoredProcedureParameters.Where(r => r.Id == storedProcedureParameterId.Id).SingleOrDefault();
                if (storedProcedureParameter == null)
                {
                    return (new questStatus(Severity.Error, String.Format("ERROR: {0}.{1}: {2}",
                            this.GetType().Name, MethodBase.GetCurrentMethod().Name,
                            String.Format("Id {0} not found", storedProcedureParameterId.Id))));
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
        private questStatus read(MasterPricingEntities dbContext, StoredProcedureId storedProcedureId, out List<Quest.Services.Dbio.MasterPricing.StoredProcedureParameters> storedProcedureParameterList)
        {
            // Initialize
            storedProcedureParameterList = null;


            try
            {
                storedProcedureParameterList = dbContext.StoredProcedureParameters.Where(r => r.StoredProcedureId == storedProcedureId.Id).ToList();
                if (storedProcedureParameterList == null)
                {
                    return (new questStatus(Severity.Error, String.Format("ERROR: {0}.{1}: {2}",
                            this.GetType().Name, MethodBase.GetCurrentMethod().Name,
                            String.Format("StoredProcedureId {0} not found", storedProcedureId.Id))));
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
        private questStatus update(MasterPricingEntities dbContext, Quest.Functional.MasterPricing.StoredProcedureParameter storedProcedureParameter)
        {
            // Initialize 
            questStatus status = null;


            try
            {
                // Read the record.
                StoredProcedureParameterId storedProcedureParameterId = new StoredProcedureParameterId(storedProcedureParameter.Id);
                Quest.Services.Dbio.MasterPricing.StoredProcedureParameters _storedProcedureParameters = null;
                status = read(dbContext, storedProcedureParameterId, out _storedProcedureParameters);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }

                // Update the record.
                BufferMgr.TransferBuffer(storedProcedureParameter, _storedProcedureParameters);
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
        private questStatus delete(MasterPricingEntities dbContext, StoredProcedureParameterId storedProcedureParameterId)
        {
            // Initialize 
            questStatus status = null;


            try
            {
                // Read the record.
                Quest.Services.Dbio.MasterPricing.StoredProcedureParameters _storedProcedureParameters = null;
                status = read(dbContext, storedProcedureParameterId, out _storedProcedureParameters);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }

                // Delete the record.
                dbContext.StoredProcedureParameters.Remove(_storedProcedureParameters);
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
                // Read all storedProcedureParameters for this stored.
                List<Quest.Services.Dbio.MasterPricing.StoredProcedureParameters> _storedProcedureParametersList = null;
                status = read(dbContext, storedProcedureId, out _storedProcedureParametersList);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }

                // Delete the records.
                dbContext.StoredProcedureParameters.RemoveRange(_storedProcedureParametersList);
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
