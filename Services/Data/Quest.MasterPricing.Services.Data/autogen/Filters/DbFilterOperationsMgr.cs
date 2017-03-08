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
    public class DbFilterOperationsMgr : DbMgrSessionBased
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
        public DbFilterOperationsMgr(UserSession userSession)
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
        public questStatus Create(Quest.Functional.MasterPricing.FilterOperation filterOperation, out FilterOperationId filterOperationId)
        {
            // Initialize
            questStatus status = null;
            filterOperationId = null;


            // Data rules.


            // Create the filterOperation
            using (MasterPricingEntities dbContext = new MasterPricingEntities())
            {
                status = create(dbContext, filterOperation, out filterOperationId);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Create(DbMgrTransaction trans, Quest.Functional.MasterPricing.FilterOperation filterOperation, out FilterOperationId filterOperationId)
        {
            // Initialize
            questStatus status = null;
            filterOperationId = null;


            // Data rules.


            // Create the filterOperation in this transaction.
            status = create((MasterPricingEntities)trans.DbContext, filterOperation, out filterOperationId);
            if (! questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Create(DbMgrTransaction trans, List<Quest.Functional.MasterPricing.FilterOperation> filterOperationList, out List<Quest.Functional.MasterPricing.FilterOperation> filterOperationIdList)
        {
            // Initialize
            questStatus status = null;
            filterOperationIdList = null;


            // Data rules.


            // Create the filterOperations in this transaction.
            status = create((MasterPricingEntities)trans.DbContext, filterOperationList, out filterOperationIdList);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Read(FilterOperationId filterOperationId, out Quest.Functional.MasterPricing.FilterOperation filterOperation)
        {
            // Initialize
            questStatus status = null;
            filterOperation = null;


            // Perform read
            using (MasterPricingEntities dbContext = new MasterPricingEntities())
            {
                Quest.Services.Dbio.MasterPricing.FilterOperations _filterOperations = null;
                status = read(dbContext, filterOperationId, out _filterOperations);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }
                filterOperation = new Quest.Functional.MasterPricing.FilterOperation();
                BufferMgr.TransferBuffer(_filterOperations, filterOperation);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Read(DbMgrTransaction trans, FilterOperationId filterOperationId, out Quest.Functional.MasterPricing.FilterOperation filterOperation)
        {
            // Initialize
            questStatus status = null;
            filterOperation = null;


            // Perform read
            Quest.Services.Dbio.MasterPricing.FilterOperations _filterOperations = null;
            status = read((MasterPricingEntities)trans.DbContext, filterOperationId, out _filterOperations);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            filterOperation = new Quest.Functional.MasterPricing.FilterOperation();
            BufferMgr.TransferBuffer(_filterOperations, filterOperation);

            return (new questStatus(Severity.Success));
        }
        public questStatus Read(FilterItemId filterItemId, out List<Quest.Functional.MasterPricing.FilterOperation> filterOperationList)
        {
            // Initialize
            questStatus status = null;
            filterOperationList = null;


            // Perform read
            using (MasterPricingEntities dbContext = new MasterPricingEntities())
            {
                List<Quest.Services.Dbio.MasterPricing.FilterOperations> _filterOperationsList = null;
                status = read(dbContext, filterItemId, out _filterOperationsList);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }
                filterOperationList = new List<FilterOperation>();
                foreach (Quest.Services.Dbio.MasterPricing.FilterOperations _filterOperation in _filterOperationsList)
                {
                    Quest.Functional.MasterPricing.FilterOperation filterOperation = new Quest.Functional.MasterPricing.FilterOperation();
                    BufferMgr.TransferBuffer(_filterOperation, filterOperation);
                    filterOperationList.Add(filterOperation);
                }
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Read(DbMgrTransaction trans, FilterItemId filterItemId, out List<Quest.Functional.MasterPricing.FilterOperation> filterOperationList)
        {
            // Initialize
            questStatus status = null;
            filterOperationList = null;


            // Perform read
            List<Quest.Services.Dbio.MasterPricing.FilterOperations> _filterOperationsList = null;
            status = read((MasterPricingEntities)trans.DbContext, filterItemId, out _filterOperationsList);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            filterOperationList = new List<FilterOperation>();
            foreach (Quest.Services.Dbio.MasterPricing.FilterOperations _filterOperation in _filterOperationsList)
            {
                Quest.Functional.MasterPricing.FilterOperation filterOperation = new Quest.Functional.MasterPricing.FilterOperation();
                BufferMgr.TransferBuffer(_filterOperation, filterOperation);
                filterOperationList.Add(filterOperation);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Update(Quest.Functional.MasterPricing.FilterOperation filterOperation)
        {
            // Initialize
            questStatus status = null;


            // Perform update.
            using (MasterPricingEntities dbContext = new MasterPricingEntities())
            {
                status = update(dbContext, filterOperation);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Update(DbMgrTransaction trans, Quest.Functional.MasterPricing.FilterOperation filterOperation)
        {
            // Initialize
            questStatus status = null;
            bool bCreateTransaction = trans == null;


            // Perform update in this transaction.
            status = update((MasterPricingEntities)trans.DbContext, filterOperation);
            if (!questStatusDef.IsSuccess(status))
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
            if (! questStatusDef.IsSuccess(status))
            {
                RollbackTransaction(trans);
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Delete(FilterItemId filterItemId)
        {
            // Initialize
            questStatus status = null;


            // Perform delete.
            using (MasterPricingEntities dbContext = new MasterPricingEntities())
            {
                status = delete(dbContext, filterItemId);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Delete(DbMgrTransaction trans, FilterItemId filterItemId)
        {
            // Initialize
            questStatus status = null;


            // Perform delete in this transaction.
            status = delete((MasterPricingEntities)trans.DbContext, filterItemId);
            if (!questStatusDef.IsSuccess(status))
            {
                RollbackTransaction(trans);
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus List(QueryOptions queryOptions, out List<Quest.Functional.MasterPricing.FilterOperation> filterOperationList, out QueryResponse queryResponse)
        {
            // Initialize
            questStatus status = null;
            filterOperationList = null;
            queryResponse = null;


            using (MasterPricingEntities dbContext = new MasterPricingEntities())
            {
                using (DbContextTransaction tx = dbContext.Database.BeginTransaction(IsolationLevel.ReadUncommitted))
                {
                    try
                    {
                        PropertyInfo[] dbProperties = typeof(Quest.Services.Dbio.MasterPricing.FilterOperations).GetProperties().ToArray();
                        int totalRecords = dbContext.FilterOperations.Where(BuildWhereClause(queryOptions, dbProperties)).Count();
                        List<Quest.Services.Dbio.MasterPricing.FilterOperations> _countriesList = dbContext.FilterOperations.Where(BuildWhereClause(queryOptions, dbProperties))
                                .OrderBy(BuildSortString(queryOptions.SortColumns))
                                .Skip(queryOptions.Paging.PageSize * (queryOptions.Paging.PageNumber - 1))
                                .Take(queryOptions.Paging.PageSize).ToList();
                        if (_countriesList == null)
                        {
                            return (new questStatus(Severity.Warning));
                        }
                        filterOperationList = new List<Quest.Functional.MasterPricing.FilterOperation>();
                        foreach (Quest.Services.Dbio.MasterPricing.FilterOperations _filterOperations in _countriesList)
                        {
                            Quest.Functional.MasterPricing.FilterOperation filterOperation = new Quest.Functional.MasterPricing.FilterOperation();
                            BufferMgr.TransferBuffer(_filterOperations, filterOperation);
                            filterOperationList.Add(filterOperation);
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
        private questStatus create(MasterPricingEntities dbContext, Quest.Functional.MasterPricing.FilterOperation filterOperation, out FilterOperationId filterOperationId)
        {
            // Initialize
            filterOperationId = null;


            // Perform create
            try
            {
                Quest.Services.Dbio.MasterPricing.FilterOperations _filterOperations = new Quest.Services.Dbio.MasterPricing.FilterOperations();
                BufferMgr.TransferBuffer(filterOperation, _filterOperations);
                dbContext.FilterOperations.Add(_filterOperations);
                dbContext.SaveChanges();
                if (_filterOperations.Id == 0)
                {
                    return (new questStatus(Severity.Error, "Quest.Functional.MasterPricing.FilterOperation not created"));
                }
                filterOperationId = new FilterOperationId(_filterOperations.Id);
            }
            catch (System.Exception ex)
            {
                return (new questStatus(Severity.Fatal, String.Format("EXCEPTION: {0}.{1}: {2}",
                        this.GetType().Name, MethodBase.GetCurrentMethod().Name,
                        ex.InnerException != null ? ex.InnerException.Message : ex.Message)));
            }
            return (new questStatus(Severity.Success));
        }
        private questStatus create(MasterPricingEntities dbContext, List<Quest.Functional.MasterPricing.FilterOperation> filterOperationList, out List<Quest.Functional.MasterPricing.FilterOperation> filterOperationIdList)
        {
            // Initialize
            filterOperationIdList = null;


            // Perform create
            try
            {
                List<Quest.Services.Dbio.MasterPricing.FilterOperations> _filterOperationList = new List<Quest.Services.Dbio.MasterPricing.FilterOperations>();
                foreach (Quest.Functional.MasterPricing.FilterOperation filterOperation in filterOperationList)
                {
                    Quest.Services.Dbio.MasterPricing.FilterOperations _filterOperation = new Quest.Services.Dbio.MasterPricing.FilterOperations();
                    BufferMgr.TransferBuffer(filterOperation, _filterOperation);
                    _filterOperationList.Add(_filterOperation);
                }
                dbContext.FilterOperations.AddRange(_filterOperationList);
                dbContext.SaveChanges();

                filterOperationIdList = new List<FilterOperation>();
                foreach (Quest.Services.Dbio.MasterPricing.FilterOperations _filterOperation in _filterOperationList)
                {
                    Quest.Functional.MasterPricing.FilterOperation filterOperation = new FilterOperation();
                    filterOperation.Id = _filterOperation.Id;
                    filterOperationIdList.Add(filterOperation);
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
        private questStatus read(MasterPricingEntities dbContext, FilterOperationId filterOperationId, out Quest.Services.Dbio.MasterPricing.FilterOperations filterOperation)
        {
            // Initialize
            filterOperation = null;


            try
            {
                filterOperation = dbContext.FilterOperations.Where(r => r.Id == filterOperationId.Id).SingleOrDefault();
                if (filterOperation == null)
                {
                    return (new questStatus(Severity.Error, String.Format("ERROR: {0}.{1}: {2}",
                            this.GetType().Name, MethodBase.GetCurrentMethod().Name,
                            String.Format("Id {0} not found", filterOperationId.Id))));
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
        private questStatus read(MasterPricingEntities dbContext, FilterItemId filterItemId, out List<Quest.Services.Dbio.MasterPricing.FilterOperations> filterOperationList)
        {
            // Initialize
            filterOperationList = null;


            try
            {
                filterOperationList = dbContext.FilterOperations.Where(r => r.FilterItemId == filterItemId.Id).ToList();
                if (filterOperationList == null)
                {
                    return (new questStatus(Severity.Error, String.Format("ERROR: {0}.{1}: {2}",
                            this.GetType().Name, MethodBase.GetCurrentMethod().Name,
                            String.Format("FilterItemId {0} not found", filterItemId.Id))));
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
        private questStatus update(MasterPricingEntities dbContext, Quest.Functional.MasterPricing.FilterOperation filterOperation)
        {
            // Initialize 
            questStatus status = null;


            try
            {
                // Read the record.
                FilterOperationId filterOperationId = new FilterOperationId(filterOperation.Id);
                Quest.Services.Dbio.MasterPricing.FilterOperations _filterOperations = null;
                status = read(dbContext, filterOperationId, out _filterOperations);
                if (! questStatusDef.IsSuccess(status))
                {
                    return (status);
                }

                // Update the record.
                BufferMgr.TransferBuffer(filterOperation, _filterOperations);
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
                dbContext.FilterOperations.RemoveRange(dbContext.FilterOperations.Where(r => r.Id == filterOperationId.Id));
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
        private questStatus delete(MasterPricingEntities dbContext, FilterItemId filterItemId)
        {
            try
            {
                dbContext.FilterOperations.RemoveRange(dbContext.FilterOperations.Where(r => r.FilterItemId == filterItemId.Id));
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
