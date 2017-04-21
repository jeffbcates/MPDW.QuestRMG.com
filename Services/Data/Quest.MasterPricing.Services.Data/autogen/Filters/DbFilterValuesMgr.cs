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


namespace Quest.MasterPricing.Services.Data.Filters
{
    public class DbFilterValuesMgr : DbMgrSessionBased
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
        public DbFilterValuesMgr(UserSession userSession)
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
        public questStatus Create(Quest.Functional.MasterPricing.FilterValue filterValue, out FilterValueId filterValueId)
        {
            // Initialize
            questStatus status = null;
            filterValueId = null;


            // Data rules.


            // Create the filterValue
            using (MasterPricingEntities dbContext = new MasterPricingEntities())
            {
                status = create(dbContext, filterValue, out filterValueId);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Create(DbMgrTransaction trans, Quest.Functional.MasterPricing.FilterValue filterValue, out FilterValueId filterValueId)
        {
            // Initialize
            questStatus status = null;
            filterValueId = null;


            // Data rules.


            // Create the filterValue in this transaction.
            status = create((MasterPricingEntities)trans.DbContext, filterValue, out filterValueId);
            if (! questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Create(DbMgrTransaction trans, List<Quest.Functional.MasterPricing.FilterValue> filterValueList, out List<Quest.Functional.MasterPricing.FilterValue> filterValueIdList)
        {
            // Initialize
            questStatus status = null;
            filterValueIdList = null;


            // Data rules.


            // Create the filterValues in this transaction.
            status = create((MasterPricingEntities)trans.DbContext, filterValueList, out filterValueIdList);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Read(FilterValueId filterValueId, out Quest.Functional.MasterPricing.FilterValue filterValue)
        {
            // Initialize
            questStatus status = null;
            filterValue = null;


            // Perform read
            using (MasterPricingEntities dbContext = new MasterPricingEntities())
            {
                Quest.Services.Dbio.MasterPricing.FilterValues _filterValues = null;
                status = read(dbContext, filterValueId, out _filterValues);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }
                filterValue = new Quest.Functional.MasterPricing.FilterValue();
                BufferMgr.TransferBuffer(_filterValues, filterValue);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Read(DbMgrTransaction trans, FilterValueId filterValueId, out Quest.Functional.MasterPricing.FilterValue filterValue)
        {
            // Initialize
            questStatus status = null;
            filterValue = null;


            // Perform read
            Quest.Services.Dbio.MasterPricing.FilterValues _filterValues = null;
            status = read((MasterPricingEntities)trans.DbContext, filterValueId, out _filterValues);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            filterValue = new Quest.Functional.MasterPricing.FilterValue();
            BufferMgr.TransferBuffer(_filterValues, filterValue);

            return (new questStatus(Severity.Success));
        }
        public questStatus Read(FilterOperationId filterOperationId, out List<Quest.Functional.MasterPricing.FilterValue> filterValueList)
        {
            // Initialize
            questStatus status = null;
            filterValueList = null;


            // Perform read
            using (MasterPricingEntities dbContext = new MasterPricingEntities())
            {
                List<Quest.Services.Dbio.MasterPricing.FilterValues> _filterValuesList = null;
                status = read(dbContext, filterOperationId, out _filterValuesList);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }
                filterValueList = new List<FilterValue>();
                foreach (Quest.Services.Dbio.MasterPricing.FilterValues _filterValue in _filterValuesList)
                {
                    Quest.Functional.MasterPricing.FilterValue filterValue = new Quest.Functional.MasterPricing.FilterValue();
                    BufferMgr.TransferBuffer(_filterValue, filterValue);
                    filterValueList.Add(filterValue);
                }
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Read(DbMgrTransaction trans, FilterOperationId filterOperationId, out List<Quest.Functional.MasterPricing.FilterValue> filterValueList)
        {
            // Initialize
            questStatus status = null;
            filterValueList = null;


            // Perform read
            List<Quest.Services.Dbio.MasterPricing.FilterValues> _filterValuesList = null;
            status = read((MasterPricingEntities)trans.DbContext, filterOperationId, out _filterValuesList);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            filterValueList = new List<FilterValue>();
            foreach (Quest.Services.Dbio.MasterPricing.FilterValues _filterValue in _filterValuesList)
            {
                Quest.Functional.MasterPricing.FilterValue filterValue = new Quest.Functional.MasterPricing.FilterValue();
                BufferMgr.TransferBuffer(_filterValue, filterValue);
                filterValueList.Add(filterValue);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Update(Quest.Functional.MasterPricing.FilterValue filterValue)
        {
            // Initialize
            questStatus status = null;


            // Perform update.
            using (MasterPricingEntities dbContext = new MasterPricingEntities())
            {
                status = update(dbContext, filterValue);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Update(DbMgrTransaction trans, Quest.Functional.MasterPricing.FilterValue filterValue)
        {
            // Initialize
            questStatus status = null;
            bool bCreateTransaction = trans == null;


            // Perform update in this transaction.
            status = update((MasterPricingEntities)trans.DbContext, filterValue);
            if (!questStatusDef.IsSuccess(status))
            {
                RollbackTransaction(trans);
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Delete(FilterValueId filterValueId)
        {
            // Initialize
            questStatus status = null;


            // Perform delete.
            using (MasterPricingEntities dbContext = new MasterPricingEntities())
            {
                status = delete(dbContext, filterValueId);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Delete(DbMgrTransaction trans, FilterValueId filterValueId)
        {
            // Initialize
            questStatus status = null;


            // Perform delete in this transaction.
            status = delete((MasterPricingEntities)trans.DbContext, filterValueId);
            if (! questStatusDef.IsSuccess(status))
            {
                RollbackTransaction(trans);
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Delete(FilterOperationId filterOperationId)
        {
            // Initialize
            questStatus status = null;


            // Perform delete.
            using (MasterPricingEntities dbContext = new MasterPricingEntities())
            {
                status = delete(dbContext, filterOperationId);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Delete(DbMgrTransaction trans, FilterOperationId filterOperationId)
        {
            // Initialize
            questStatus status = null;


            // Perform delete in this transaction.
            status = delete((MasterPricingEntities)trans.DbContext, filterOperationId);
            if (!questStatusDef.IsSuccess(status))
            {
                RollbackTransaction(trans);
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus List(QueryOptions queryOptions, out List<Quest.Functional.MasterPricing.FilterValue> filterValueList, out QueryResponse queryResponse)
        {
            // Initialize
            questStatus status = null;
            filterValueList = null;
            queryResponse = null;


            using (MasterPricingEntities dbContext = new MasterPricingEntities())
            {
                using (DbContextTransaction tx = dbContext.Database.BeginTransaction(IsolationLevel.ReadUncommitted))
                {
                    try
                    {
                        PropertyInfo[] dbProperties = typeof(Quest.Services.Dbio.MasterPricing.FilterValues).GetProperties().ToArray();
                        int totalRecords = dbContext.FilterValues.Where(BuildWhereClause(queryOptions, dbProperties)).Count();
                        List<Quest.Services.Dbio.MasterPricing.FilterValues> _countriesList = dbContext.FilterValues.Where(BuildWhereClause(queryOptions, dbProperties))
                                .OrderBy(BuildSortString(queryOptions.SortColumns))
                                .Skip(queryOptions.Paging.PageSize * (queryOptions.Paging.PageNumber - 1))
                                .Take(queryOptions.Paging.PageSize).ToList();
                        if (_countriesList == null)
                        {
                            return (new questStatus(Severity.Warning));
                        }
                        filterValueList = new List<Quest.Functional.MasterPricing.FilterValue>();
                        foreach (Quest.Services.Dbio.MasterPricing.FilterValues _filterValues in _countriesList)
                        {
                            Quest.Functional.MasterPricing.FilterValue filterValue = new Quest.Functional.MasterPricing.FilterValue();
                            BufferMgr.TransferBuffer(_filterValues, filterValue);
                            filterValueList.Add(filterValue);
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


        #region FilterColumns
        /*----------------------------------------------------------------------------------------------------------------------------------
         * FilterColumns
         *---------------------------------------------------------------------------------------------------------------------------------*/
        private questStatus create(MasterPricingEntities dbContext, Quest.Functional.MasterPricing.FilterValue filterValue, out FilterValueId filterValueId)
        {
            // Initialize
            filterValueId = null;


            // Perform create
            try
            {
                Quest.Services.Dbio.MasterPricing.FilterValues _filterValues = new Quest.Services.Dbio.MasterPricing.FilterValues();
                BufferMgr.TransferBuffer(filterValue, _filterValues);
                dbContext.FilterValues.Add(_filterValues);
                dbContext.SaveChanges();
                if (_filterValues.Id == 0)
                {
                    return (new questStatus(Severity.Error, "Quest.Functional.MasterPricing.FilterValue not created"));
                }
                filterValueId = new FilterValueId(_filterValues.Id);
            }
            catch (System.Exception ex)
            {
                return (new questStatus(Severity.Fatal, String.Format("EXCEPTION: {0}.{1}: {2}",
                        this.GetType().Name, MethodBase.GetCurrentMethod().Name,
                        ex.InnerException != null ? ex.InnerException.Message : ex.Message)));
            }
            return (new questStatus(Severity.Success));
        }
        private questStatus create(MasterPricingEntities dbContext, List<Quest.Functional.MasterPricing.FilterValue> filterValueList, out List<Quest.Functional.MasterPricing.FilterValue> filterValueIdList)
        {
            // Initialize
            filterValueIdList = null;


            // Perform create
            try
            {
                List<Quest.Services.Dbio.MasterPricing.FilterValues> _filterValueList = new List<Quest.Services.Dbio.MasterPricing.FilterValues>();
                foreach (Quest.Functional.MasterPricing.FilterValue filterValue in filterValueList)
                {
                    Quest.Services.Dbio.MasterPricing.FilterValues _filterValue = new Quest.Services.Dbio.MasterPricing.FilterValues();
                    BufferMgr.TransferBuffer(filterValue, _filterValue);
                    _filterValueList.Add(_filterValue);
                }
                dbContext.FilterValues.AddRange(_filterValueList);
                dbContext.SaveChanges();

                filterValueIdList = new List<FilterValue>();
                foreach (Quest.Services.Dbio.MasterPricing.FilterValues _filterValue in _filterValueList)
                {
                    Quest.Functional.MasterPricing.FilterValue filterValue = new FilterValue();
                    filterValue.Id = _filterValue.Id;
                    filterValueIdList.Add(filterValue);
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
        private questStatus read(MasterPricingEntities dbContext, FilterValueId filterValueId, out Quest.Services.Dbio.MasterPricing.FilterValues filterValue)
        {
            // Initialize
            filterValue = null;


            try
            {
                filterValue = dbContext.FilterValues.Where(r => r.Id == filterValueId.Id).SingleOrDefault();
                if (filterValue == null)
                {
                    return (new questStatus(Severity.Error, String.Format("ERROR: {0}.{1}: {2}",
                            this.GetType().Name, MethodBase.GetCurrentMethod().Name,
                            String.Format("Id {0} not found", filterValueId.Id))));
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
        private questStatus read(MasterPricingEntities dbContext, FilterOperationId filterOperationId, out List<Quest.Services.Dbio.MasterPricing.FilterValues> filterValueList)
        {
            // Initialize
            filterValueList = null;


            try
            {
                filterValueList = dbContext.FilterValues.Where(r => r.FilterOperationId == filterOperationId.Id).ToList();
                if (filterValueList == null)
                {
                    return (new questStatus(Severity.Error, String.Format("ERROR: {0}.{1}: {2}",
                            this.GetType().Name, MethodBase.GetCurrentMethod().Name,
                            String.Format("FilterOperationId {0} not found", filterOperationId.Id))));
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
        private questStatus update(MasterPricingEntities dbContext, Quest.Functional.MasterPricing.FilterValue filterValue)
        {
            // Initialize 
            questStatus status = null;


            try
            {
                // Read the record.
                FilterValueId filterValueId = new FilterValueId(filterValue.Id);
                Quest.Services.Dbio.MasterPricing.FilterValues _filterValues = null;
                status = read(dbContext, filterValueId, out _filterValues);
                if (! questStatusDef.IsSuccess(status))
                {
                    return (status);
                }

                // Update the record.
                BufferMgr.TransferBuffer(filterValue, _filterValues);
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
        private questStatus delete(MasterPricingEntities dbContext, FilterValueId filterValueId)
        {
            try
            {
                dbContext.FilterValues.RemoveRange(dbContext.FilterValues.Where(r => r.Id == filterValueId.Id));
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
        private questStatus delete(MasterPricingEntities dbContext, FilterOperationId filterOperationId)
        {
            try
            {
                dbContext.FilterValues.RemoveRange(dbContext.FilterValues.Where(r => r.FilterOperationId == filterOperationId.Id));
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
