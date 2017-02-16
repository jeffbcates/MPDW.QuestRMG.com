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


namespace Quest.MasterPricing.Services.Data.Filters
{
    public class DbFilterProcedureParametersMgr : DbMgrSessionBased
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
        public DbFilterProcedureParametersMgr(UserSession userSession)
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
        public questStatus Create(Quest.Functional.MasterPricing.FilterProcedureParameter filterProcedureParameter, out FilterProcedureParameterId filterProcedureParameterId)
        {
            // Initialize
            questStatus status = null;
            filterProcedureParameterId = null;


            // Data rules.


            // Create the filterProcedureParameter
            using (MasterPricingEntities dbContext = new MasterPricingEntities())
            {
                status = create(dbContext, filterProcedureParameter, out filterProcedureParameterId);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Create(DbMgrTransaction trans, Quest.Functional.MasterPricing.FilterProcedureParameter filterProcedureParameter, out FilterProcedureParameterId filterProcedureParameterId)
        {
            // Initialize
            questStatus status = null;
            filterProcedureParameterId = null;


            // Data rules.


            // Create the filterProcedureParameter in this transaction.
            status = create((MasterPricingEntities)trans.DbContext, filterProcedureParameter, out filterProcedureParameterId);
            if (! questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Create(DbMgrTransaction trans, List<Quest.Functional.MasterPricing.FilterProcedureParameter> filterProcedureParameterList, out List<Quest.Functional.MasterPricing.FilterProcedureParameter> filterProcedureParameterIdList)
        {
            // Initialize
            questStatus status = null;
            filterProcedureParameterIdList = null;


            // Data rules.


            // Create the filterProcedureParameters in this transaction.
            status = create((MasterPricingEntities)trans.DbContext, filterProcedureParameterList, out filterProcedureParameterIdList);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Read(FilterProcedureParameterId filterProcedureParameterId, out Quest.Functional.MasterPricing.FilterProcedureParameter filterProcedureParameter)
        {
            // Initialize
            questStatus status = null;
            filterProcedureParameter = null;


            // Perform read
            using (MasterPricingEntities dbContext = new MasterPricingEntities())
            {
                Quest.Services.Dbio.MasterPricing.FilterProcedureParameters _filterProcedureParameters = null;
                status = read(dbContext, filterProcedureParameterId, out _filterProcedureParameters);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }
                filterProcedureParameter = new Quest.Functional.MasterPricing.FilterProcedureParameter();
                BufferMgr.TransferBuffer(_filterProcedureParameters, filterProcedureParameter);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Read(DbMgrTransaction trans, FilterProcedureParameterId filterProcedureParameterId, out Quest.Functional.MasterPricing.FilterProcedureParameter filterProcedureParameter)
        {
            // Initialize
            questStatus status = null;
            filterProcedureParameter = null;


            // Perform read
            Quest.Services.Dbio.MasterPricing.FilterProcedureParameters _filterProcedureParameters = null;
            status = read((MasterPricingEntities)trans.DbContext, filterProcedureParameterId, out _filterProcedureParameters);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            filterProcedureParameter = new Quest.Functional.MasterPricing.FilterProcedureParameter();
            BufferMgr.TransferBuffer(_filterProcedureParameters, filterProcedureParameter);

            return (new questStatus(Severity.Success));
        }
        public questStatus Read(FilterProcedureId filterProcedureId, out List<Quest.Functional.MasterPricing.FilterProcedureParameter> filterProcedureParameterList)
        {
            // Initialize
            questStatus status = null;
            filterProcedureParameterList = null;


            // Perform read
            using (MasterPricingEntities dbContext = new MasterPricingEntities())
            {
                List<Quest.Services.Dbio.MasterPricing.FilterProcedureParameters> _filterProcedureParametersList = null;
                status = read(dbContext, filterProcedureId, out _filterProcedureParametersList);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }
                filterProcedureParameterList = new List<FilterProcedureParameter>();
                foreach (Quest.Services.Dbio.MasterPricing.FilterProcedureParameters _filterProcedureParameter in _filterProcedureParametersList)
                {
                    Quest.Functional.MasterPricing.FilterProcedureParameter filterProcedureParameter = new Quest.Functional.MasterPricing.FilterProcedureParameter();
                    BufferMgr.TransferBuffer(_filterProcedureParameter, filterProcedureParameter);
                    filterProcedureParameterList.Add(filterProcedureParameter);
                }
            }
            return (new questStatus(Severity.Success));
        }

        public questStatus Read(DbMgrTransaction trans, FilterProcedureId filterProcedureId, out List<Quest.Functional.MasterPricing.FilterProcedureParameter> filterProcedureParameterList)
        {
            // Initialize
            questStatus status = null;
            filterProcedureParameterList = null;


            // Perform read
            List<Quest.Services.Dbio.MasterPricing.FilterProcedureParameters> _filterProcedureParametersList = null;
            status = read((MasterPricingEntities)trans.DbContext, filterProcedureId, out _filterProcedureParametersList);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            filterProcedureParameterList = new List<FilterProcedureParameter>();
            foreach (Quest.Services.Dbio.MasterPricing.FilterProcedureParameters _filterProcedureParameter in _filterProcedureParametersList)
            {
                Quest.Functional.MasterPricing.FilterProcedureParameter filterProcedureParameter = new Quest.Functional.MasterPricing.FilterProcedureParameter();
                BufferMgr.TransferBuffer(_filterProcedureParameter, filterProcedureParameter);
                filterProcedureParameterList.Add(filterProcedureParameter);
            }
            return (new questStatus(Severity.Success));
        }

        public questStatus Update(Quest.Functional.MasterPricing.FilterProcedureParameter filterProcedureParameter)
        {
            // Initialize
            questStatus status = null;


            // Perform update.
            using (MasterPricingEntities dbContext = new MasterPricingEntities())
            {
                status = update(dbContext, filterProcedureParameter);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Update(DbMgrTransaction trans, Quest.Functional.MasterPricing.FilterProcedureParameter filterProcedureParameter)
        {
            // Initialize
            questStatus status = null;
            bool bCreateTransaction = trans == null;


            // Perform update in this transaction.
            status = update((MasterPricingEntities)trans.DbContext, filterProcedureParameter);
            if (!questStatusDef.IsSuccess(status))
            {
                RollbackTransaction(trans);
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Delete(FilterProcedureParameterId filterProcedureParameterId)
        {
            // Initialize
            questStatus status = null;


            // Perform delete.
            using (MasterPricingEntities dbContext = new MasterPricingEntities())
            {
                status = delete(dbContext, filterProcedureParameterId);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Delete(DbMgrTransaction trans, FilterProcedureParameterId filterProcedureParameterId)
        {
            // Initialize
            questStatus status = null;


            // Perform delete in this transaction.
            status = delete((MasterPricingEntities)trans.DbContext, filterProcedureParameterId);
            if (! questStatusDef.IsSuccess(status))
            {
                RollbackTransaction(trans);
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Delete(FilterProcedureId filterProcedureId)
        {
            // Initialize
            questStatus status = null;


            // Perform delete.
            using (MasterPricingEntities dbContext = new MasterPricingEntities())
            {
                status = delete(dbContext, filterProcedureId);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Delete(DbMgrTransaction trans, FilterProcedureId filterProcedureId)
        {
            // Initialize
            questStatus status = null;


            // Perform delete in this transaction.
            status = delete((MasterPricingEntities)trans.DbContext, filterProcedureId);
            if (!questStatusDef.IsSuccess(status))
            {
                RollbackTransaction(trans);
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus List(QueryOptions queryOptions, out List<Quest.Functional.MasterPricing.FilterProcedureParameter> filterProcedureParameterList, out QueryResponse queryResponse)
        {
            // Initialize
            questStatus status = null;
            filterProcedureParameterList = null;
            queryResponse = null;


            using (MasterPricingEntities dbContext = new MasterPricingEntities())
            {
                using (DbContextTransaction tx = dbContext.Database.BeginTransaction(IsolationLevel.ReadUncommitted))
                {
                    try
                    {
                        PropertyInfo[] dbProperties = typeof(Quest.Services.Dbio.MasterPricing.FilterProcedureParameters).GetProperties().ToArray();
                        int totalRecords = dbContext.FilterProcedureParameters.Where(BuildWhereClause(queryOptions, dbProperties)).Count();
                        List<Quest.Services.Dbio.MasterPricing.FilterProcedureParameters> _countriesList = dbContext.FilterProcedureParameters.Where(BuildWhereClause(queryOptions, dbProperties))
                                .OrderBy(BuildSortString(queryOptions.SortColumns))
                                .Skip(queryOptions.Paging.PageSize * (queryOptions.Paging.PageNumber - 1))
                                .Take(queryOptions.Paging.PageSize).ToList();
                        if (_countriesList == null)
                        {
                            return (new questStatus(Severity.Warning));
                        }
                        filterProcedureParameterList = new List<Quest.Functional.MasterPricing.FilterProcedureParameter>();
                        foreach (Quest.Services.Dbio.MasterPricing.FilterProcedureParameters _filterProcedureParameters in _countriesList)
                        {
                            Quest.Functional.MasterPricing.FilterProcedureParameter filterProcedureParameter = new Quest.Functional.MasterPricing.FilterProcedureParameter();
                            BufferMgr.TransferBuffer(_filterProcedureParameters, filterProcedureParameter);
                            filterProcedureParameterList.Add(filterProcedureParameter);
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


        #region FilterProcedureParameters
        /*----------------------------------------------------------------------------------------------------------------------------------
         * FilterProcedureParameters
         *---------------------------------------------------------------------------------------------------------------------------------*/
        private questStatus create(MasterPricingEntities dbContext, Quest.Functional.MasterPricing.FilterProcedureParameter filterProcedureParameter, out FilterProcedureParameterId filterProcedureParameterId)
        {
            // Initialize
            filterProcedureParameterId = null;


            // Perform create
            try
            {
                Quest.Services.Dbio.MasterPricing.FilterProcedureParameters _filterProcedureParameters = new Quest.Services.Dbio.MasterPricing.FilterProcedureParameters();
                BufferMgr.TransferBuffer(filterProcedureParameter, _filterProcedureParameters, true);
                dbContext.FilterProcedureParameters.Add(_filterProcedureParameters);
                dbContext.SaveChanges();
                if (_filterProcedureParameters.Id == 0)
                {
                    return (new questStatus(Severity.Error, "Quest.Functional.MasterPricing.FilterProcedureParameter not created"));
                }
                filterProcedureParameterId = new FilterProcedureParameterId(_filterProcedureParameters.Id);
            }
            catch (System.Exception ex)
            {
                return (new questStatus(Severity.Fatal, String.Format("EXCEPTION: {0}.{1}: {2}",
                        this.GetType().Name, MethodBase.GetCurrentMethod().Name,
                        ex.InnerException != null ? ex.InnerException.Message : ex.Message)));
            }
            return (new questStatus(Severity.Success));
        }
        private questStatus create(MasterPricingEntities dbContext, List<Quest.Functional.MasterPricing.FilterProcedureParameter> filterProcedureParameterList, out List<Quest.Functional.MasterPricing.FilterProcedureParameter> filterProcedureParameterIdList)
        {
            // Initialize
            filterProcedureParameterIdList = null;


            // Perform create
            try
            {
                List<Quest.Services.Dbio.MasterPricing.FilterProcedureParameters> _filterProcedureParameterList = new List<Quest.Services.Dbio.MasterPricing.FilterProcedureParameters>();
                foreach (Quest.Functional.MasterPricing.FilterProcedureParameter filterProcedureParameter in filterProcedureParameterList)
                {
                    Quest.Services.Dbio.MasterPricing.FilterProcedureParameters _filterProcedureParameter = new Quest.Services.Dbio.MasterPricing.FilterProcedureParameters();
                    BufferMgr.TransferBuffer(filterProcedureParameter, _filterProcedureParameter);
                    _filterProcedureParameterList.Add(_filterProcedureParameter);
                }
                dbContext.FilterProcedureParameters.AddRange(_filterProcedureParameterList);
                dbContext.SaveChanges();

                filterProcedureParameterIdList = new List<FilterProcedureParameter>();
                foreach (Quest.Services.Dbio.MasterPricing.FilterProcedureParameters _filterProcedureParameter in _filterProcedureParameterList)
                {
                    Quest.Functional.MasterPricing.FilterProcedureParameter filterProcedureParameter = new FilterProcedureParameter();
                    filterProcedureParameter.Id = _filterProcedureParameter.Id;
                    filterProcedureParameterIdList.Add(filterProcedureParameter);
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
        private questStatus read(MasterPricingEntities dbContext, FilterProcedureParameterId filterProcedureParameterId, out Quest.Services.Dbio.MasterPricing.FilterProcedureParameters filterProcedureParameter)
        {
            // Initialize
            filterProcedureParameter = null;


            try
            {
                filterProcedureParameter = dbContext.FilterProcedureParameters.Where(r => r.Id == filterProcedureParameterId.Id).SingleOrDefault();
                if (filterProcedureParameter == null)
                {
                    return (new questStatus(Severity.Error, String.Format("ERROR: {0}.{1}: {2}",
                            this.GetType().Name, MethodBase.GetCurrentMethod().Name,
                            String.Format("Id {0} not found", filterProcedureParameterId.Id))));
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
        private questStatus read(MasterPricingEntities dbContext, FilterProcedureId filterProcedureId, out List<Quest.Services.Dbio.MasterPricing.FilterProcedureParameters> filterProcedureParameterList)
        {
            // Initialize
            filterProcedureParameterList = null;


            try
            {
                filterProcedureParameterList = dbContext.FilterProcedureParameters.Where(r => r.FilterProcedureId == filterProcedureId.Id).ToList();
                if (filterProcedureParameterList == null)
                {
                    return (new questStatus(Severity.Error, String.Format("ERROR: {0}.{1}: {2}",
                            this.GetType().Name, MethodBase.GetCurrentMethod().Name,
                            String.Format("FilterProcedureId {0} not found", filterProcedureId.Id))));
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
        private questStatus update(MasterPricingEntities dbContext, Quest.Functional.MasterPricing.FilterProcedureParameter filterProcedureParameter)
        {
            // Initialize 
            questStatus status = null;


            try
            {
                // Read the record.
                FilterProcedureParameterId filterProcedureParameterId = new FilterProcedureParameterId(filterProcedureParameter.Id);
                Quest.Services.Dbio.MasterPricing.FilterProcedureParameters _filterProcedureParameters = null;
                status = read(dbContext, filterProcedureParameterId, out _filterProcedureParameters);
                if (! questStatusDef.IsSuccess(status))
                {
                    return (status);
                }

                // Update the record.
                BufferMgr.TransferBuffer(filterProcedureParameter, _filterProcedureParameters);
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
        private questStatus delete(MasterPricingEntities dbContext, FilterProcedureParameterId filterProcedureParameterId)
        {
            // Initialize 
            questStatus status = null;


            try
            {
                // Read the record.
                Quest.Services.Dbio.MasterPricing.FilterProcedureParameters _filterProcedureParameters = null;
                status = read(dbContext, filterProcedureParameterId, out _filterProcedureParameters);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }

                // Delete the record.
                dbContext.FilterProcedureParameters.Remove(_filterProcedureParameters);
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
        private questStatus delete(MasterPricingEntities dbContext, FilterProcedureId filterProcedureId)
        {
            // Initialize 
            questStatus status = null;


            try
            {
                // Read all filterProcedureParametersList for this filterProcedure.
                List<Quest.Services.Dbio.MasterPricing.FilterProcedureParameters> _filterProcedureParametersList = null;
                status = read(dbContext, filterProcedureId, out _filterProcedureParametersList);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }

                // Delete the records.
                dbContext.FilterProcedureParameters.RemoveRange(_filterProcedureParametersList);
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
