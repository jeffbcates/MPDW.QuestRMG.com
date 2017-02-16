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
    public class DbFilterOperatorsMgr : DbMgrSessionBased
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
        public DbFilterOperatorsMgr(UserSession userSession)
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
        public questStatus Create(Quest.Functional.MasterPricing.FilterOperator filterOperator, out FilterOperatorId filterOperatorId)
        {
            // Initialize
            questStatus status = null;
            filterOperatorId = null;


            // Data rules.


            // Create the filterOperator
            using (MasterPricingEntities dbContext = new MasterPricingEntities())
            {
                status = create(dbContext, filterOperator, out filterOperatorId);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Create(DbMgrTransaction trans, Quest.Functional.MasterPricing.FilterOperator filterOperator, out FilterOperatorId filterOperatorId)
        {
            // Initialize
            questStatus status = null;
            filterOperatorId = null;


            // Data rules.


            // Create the filterOperator in this transaction.
            status = create((MasterPricingEntities)trans.DbContext, filterOperator, out filterOperatorId);
            if (! questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Create(DbMgrTransaction trans, List<Quest.Functional.MasterPricing.FilterOperator> filterOperatorList, out List<Quest.Functional.MasterPricing.FilterOperator> filterOperatorIdList)
        {
            // Initialize
            questStatus status = null;
            filterOperatorIdList = null;


            // Data rules.


            // Create the filterOperators in this transaction.
            status = create((MasterPricingEntities)trans.DbContext, filterOperatorList, out filterOperatorIdList);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Read(FilterOperatorId filterOperatorId, out Quest.Functional.MasterPricing.FilterOperator filterOperator)
        {
            // Initialize
            questStatus status = null;
            filterOperator = null;


            // Perform read
            using (MasterPricingEntities dbContext = new MasterPricingEntities())
            {
                Quest.Services.Dbio.MasterPricing.FilterOperators _filterOperators = null;
                status = read(dbContext, filterOperatorId, out _filterOperators);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }
                filterOperator = new Quest.Functional.MasterPricing.FilterOperator();
                BufferMgr.TransferBuffer(_filterOperators, filterOperator);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Read(DbMgrTransaction trans, FilterOperatorId filterOperatorId, out Quest.Functional.MasterPricing.FilterOperator filterOperator)
        {
            // Initialize
            questStatus status = null;
            filterOperator = null;


            // Perform read
            Quest.Services.Dbio.MasterPricing.FilterOperators _filterOperators = null;
            status = read((MasterPricingEntities)trans.DbContext, filterOperatorId, out _filterOperators);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            filterOperator = new Quest.Functional.MasterPricing.FilterOperator();
            BufferMgr.TransferBuffer(_filterOperators, filterOperator);

            return (new questStatus(Severity.Success));
        }
        public questStatus Update(Quest.Functional.MasterPricing.FilterOperator filterOperator)
        {
            // Initialize
            questStatus status = null;


            // Perform update.
            using (MasterPricingEntities dbContext = new MasterPricingEntities())
            {
                status = update(dbContext, filterOperator);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Update(DbMgrTransaction trans, Quest.Functional.MasterPricing.FilterOperator filterOperator)
        {
            // Initialize
            questStatus status = null;
            bool bCreateTransaction = trans == null;


            // Perform update in this transaction.
            status = update((MasterPricingEntities)trans.DbContext, filterOperator);
            if (!questStatusDef.IsSuccess(status))
            {
                RollbackTransaction(trans);
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Delete(FilterOperatorId filterOperatorId)
        {
            // Initialize
            questStatus status = null;


            // Perform delete.
            using (MasterPricingEntities dbContext = new MasterPricingEntities())
            {
                status = delete(dbContext, filterOperatorId);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Delete(DbMgrTransaction trans, FilterOperatorId filterOperatorId)
        {
            // Initialize
            questStatus status = null;


            // Perform delete in this transaction.
            status = delete((MasterPricingEntities)trans.DbContext, filterOperatorId);
            if (! questStatusDef.IsSuccess(status))
            {
                RollbackTransaction(trans);
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus List(QueryOptions queryOptions, out List<Quest.Functional.MasterPricing.FilterOperator> filterOperatorList, out QueryResponse queryResponse)
        {
            // Initialize
            questStatus status = null;
            filterOperatorList = null;
            queryResponse = null;


            using (MasterPricingEntities dbContext = new MasterPricingEntities())
            {
                using (DbContextTransaction tx = dbContext.Database.BeginTransaction(IsolationLevel.ReadUncommitted))
                {
                    try
                    {
                        PropertyInfo[] dbProperties = typeof(Quest.Services.Dbio.MasterPricing.FilterOperators).GetProperties().ToArray();
                        int totalRecords = dbContext.FilterOperators.Where(BuildWhereClause(queryOptions, dbProperties)).Count();
                        List<Quest.Services.Dbio.MasterPricing.FilterOperators> _countriesList = dbContext.FilterOperators.Where(BuildWhereClause(queryOptions, dbProperties))
                                .OrderBy(BuildSortString(queryOptions.SortColumns))
                                .Skip(queryOptions.Paging.PageSize * (queryOptions.Paging.PageNumber - 1))
                                .Take(queryOptions.Paging.PageSize).ToList();
                        if (_countriesList == null)
                        {
                            return (new questStatus(Severity.Warning));
                        }
                        filterOperatorList = new List<Quest.Functional.MasterPricing.FilterOperator>();
                        foreach (Quest.Services.Dbio.MasterPricing.FilterOperators _filterOperators in _countriesList)
                        {
                            Quest.Functional.MasterPricing.FilterOperator filterOperator = new Quest.Functional.MasterPricing.FilterOperator();
                            BufferMgr.TransferBuffer(_filterOperators, filterOperator);
                            filterOperatorList.Add(filterOperator);
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
        private questStatus create(MasterPricingEntities dbContext, Quest.Functional.MasterPricing.FilterOperator filterOperator, out FilterOperatorId filterOperatorId)
        {
            // Initialize
            filterOperatorId = null;


            // Perform create
            try
            {
                Quest.Services.Dbio.MasterPricing.FilterOperators _filterOperators = new Quest.Services.Dbio.MasterPricing.FilterOperators();
                BufferMgr.TransferBuffer(filterOperator, _filterOperators);
                dbContext.FilterOperators.Add(_filterOperators);
                dbContext.SaveChanges();
                if (_filterOperators.Id == 0)
                {
                    return (new questStatus(Severity.Error, "Quest.Functional.MasterPricing.FilterOperator not created"));
                }
                filterOperatorId = new FilterOperatorId(_filterOperators.Id);
            }
            catch (System.Exception ex)
            {
                return (new questStatus(Severity.Fatal, String.Format("EXCEPTION: {0}.{1}: {2}",
                        this.GetType().Name, MethodBase.GetCurrentMethod().Name,
                        ex.InnerException != null ? ex.InnerException.Message : ex.Message)));
            }
            return (new questStatus(Severity.Success));
        }
        private questStatus create(MasterPricingEntities dbContext, List<Quest.Functional.MasterPricing.FilterOperator> filterOperatorList, out List<Quest.Functional.MasterPricing.FilterOperator> filterOperatorIdList)
        {
            // Initialize
            filterOperatorIdList = null;


            // Perform create
            try
            {
                List<Quest.Services.Dbio.MasterPricing.FilterOperators> _filterOperatorList = new List<Quest.Services.Dbio.MasterPricing.FilterOperators>();
                foreach (Quest.Functional.MasterPricing.FilterOperator filterOperator in filterOperatorList)
                {
                    Quest.Services.Dbio.MasterPricing.FilterOperators _filterOperator = new Quest.Services.Dbio.MasterPricing.FilterOperators();
                    BufferMgr.TransferBuffer(filterOperator, _filterOperator);
                    _filterOperatorList.Add(_filterOperator);
                }
                dbContext.FilterOperators.AddRange(_filterOperatorList);
                dbContext.SaveChanges();

                filterOperatorIdList = new List<FilterOperator>();
                foreach (Quest.Services.Dbio.MasterPricing.FilterOperators _filterOperator in _filterOperatorList)
                {
                    Quest.Functional.MasterPricing.FilterOperator filterOperator = new FilterOperator();
                    filterOperator.Id = _filterOperator.Id;
                    filterOperatorIdList.Add(filterOperator);
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
        private questStatus read(MasterPricingEntities dbContext, FilterOperatorId filterOperatorId, out Quest.Services.Dbio.MasterPricing.FilterOperators filterOperator)
        {
            // Initialize
            filterOperator = null;


            try
            {
                filterOperator = dbContext.FilterOperators.Where(r => r.Id == filterOperatorId.Id).SingleOrDefault();
                if (filterOperator == null)
                {
                    return (new questStatus(Severity.Error, String.Format("ERROR: {0}.{1}: {2}",
                            this.GetType().Name, MethodBase.GetCurrentMethod().Name,
                            String.Format("Id {0} not found", filterOperatorId.Id))));
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
        private questStatus update(MasterPricingEntities dbContext, Quest.Functional.MasterPricing.FilterOperator filterOperator)
        {
            // Initialize 
            questStatus status = null;


            try
            {
                // Read the record.
                FilterOperatorId filterOperatorId = new FilterOperatorId(filterOperator.Id);
                Quest.Services.Dbio.MasterPricing.FilterOperators _filterOperators = null;
                status = read(dbContext, filterOperatorId, out _filterOperators);
                if (! questStatusDef.IsSuccess(status))
                {
                    return (status);
                }

                // Update the record.
                BufferMgr.TransferBuffer(filterOperator, _filterOperators);
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
        private questStatus delete(MasterPricingEntities dbContext, FilterOperatorId filterOperatorId)
        {
            // Initialize 
            questStatus status = null;


            try
            {
                // Read the record.
                Quest.Services.Dbio.MasterPricing.FilterOperators _filterOperators = null;
                status = read(dbContext, filterOperatorId, out _filterOperators);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }

                // Delete the record.
                dbContext.FilterOperators.Remove(_filterOperators);
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
