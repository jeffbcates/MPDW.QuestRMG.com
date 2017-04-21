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
    public class DbFilterViewsMgr : DbMgrSessionBased
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
        public DbFilterViewsMgr(UserSession userSession)
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
        public questStatus Create(Quest.Functional.MasterPricing.FilterView filterView, out FilterViewId filterViewId)
        {
            // Initialize
            questStatus status = null;
            filterViewId = null;


            // Data rules.


            // Create the filterView
            using (MasterPricingEntities dbContext = new MasterPricingEntities())
            {
                status = create(dbContext, filterView, out filterViewId);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Create(DbMgrTransaction trans, Quest.Functional.MasterPricing.FilterView filterView, out FilterViewId filterViewId)
        {
            // Initialize
            questStatus status = null;
            filterViewId = null;


            // Data rules.


            // Create the filterView in this transaction.
            status = create((MasterPricingEntities)trans.DbContext, filterView, out filterViewId);
            if (! questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Create(DbMgrTransaction trans, List<Quest.Functional.MasterPricing.FilterView> filterViewList, out List<Quest.Functional.MasterPricing.FilterView> filterViewIdList)
        {
            // Initialize
            questStatus status = null;
            filterViewIdList = null;


            // Data rules.


            // Create the filterViews in this transaction.
            status = create((MasterPricingEntities)trans.DbContext, filterViewList, out filterViewIdList);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Read(FilterViewId filterViewId, out Quest.Functional.MasterPricing.FilterView filterView)
        {
            // Initialize
            questStatus status = null;
            filterView = null;


            // Perform read
            using (MasterPricingEntities dbContext = new MasterPricingEntities())
            {
                Quest.Services.Dbio.MasterPricing.FilterViews _filterViews = null;
                status = read(dbContext, filterViewId, out _filterViews);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }
                filterView = new Quest.Functional.MasterPricing.FilterView();
                BufferMgr.TransferBuffer(_filterViews, filterView);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Read(FilterViewNameIdentifier filterViewNameIdentifier, out Quest.Functional.MasterPricing.FilterView filterView)
        {
            // Initialize
            questStatus status = null;
            filterView = null;


            // Perform read
            using (MasterPricingEntities dbContext = new MasterPricingEntities())
            {
                Quest.Services.Dbio.MasterPricing.FilterViews _filterViews = null;
                status = read(dbContext, filterViewNameIdentifier, out _filterViews);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }
                filterView = new Quest.Functional.MasterPricing.FilterView();
                BufferMgr.TransferBuffer(_filterViews, filterView);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Read(DbMgrTransaction trans, FilterViewId filterViewId, out Quest.Functional.MasterPricing.FilterView filterView)
        {
            // Initialize
            questStatus status = null;
            filterView = null;


            // Perform read
            Quest.Services.Dbio.MasterPricing.FilterViews _filterViews = null;
            status = read((MasterPricingEntities)trans.DbContext, filterViewId, out _filterViews);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            filterView = new Quest.Functional.MasterPricing.FilterView();
            BufferMgr.TransferBuffer(_filterViews, filterView);

            return (new questStatus(Severity.Success));
        }
        public questStatus Read(FilterId filterId, out List<Quest.Functional.MasterPricing.FilterView> filterViewList)
        {
            // Initialize
            questStatus status = null;
            filterViewList = null;


            // Perform read
            using (MasterPricingEntities dbContext = new MasterPricingEntities())
            {
                List<Quest.Services.Dbio.MasterPricing.FilterViews> _filterViewsList = null;
                status = read(dbContext, filterId, out _filterViewsList);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }
                filterViewList = new List<FilterView>();
                foreach (Quest.Services.Dbio.MasterPricing.FilterViews _filterView in _filterViewsList)
                {
                    Quest.Functional.MasterPricing.FilterView filterView = new Quest.Functional.MasterPricing.FilterView();
                    BufferMgr.TransferBuffer(_filterView, filterView);
                    filterViewList.Add(filterView);
                }
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Read(DbMgrTransaction trans, FilterId filterId, out List<Quest.Functional.MasterPricing.FilterView> filterViewList)
        {
            // Initialize
            questStatus status = null;
            filterViewList = null;


            // Perform read
            List<Quest.Services.Dbio.MasterPricing.FilterViews> _filterViewsList = null;
            status = read((MasterPricingEntities)trans.DbContext, filterId, out _filterViewsList);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            filterViewList = new List<FilterView>();
            foreach (Quest.Services.Dbio.MasterPricing.FilterViews _filterView in _filterViewsList)
            {
                Quest.Functional.MasterPricing.FilterView filterView = new Quest.Functional.MasterPricing.FilterView();
                BufferMgr.TransferBuffer(_filterView, filterView);
                filterViewList.Add(filterView);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Update(Quest.Functional.MasterPricing.FilterView filterView)
        {
            // Initialize
            questStatus status = null;


            // Perform update.
            using (MasterPricingEntities dbContext = new MasterPricingEntities())
            {
                status = update(dbContext, filterView);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Update(DbMgrTransaction trans, Quest.Functional.MasterPricing.FilterView filterView)
        {
            // Initialize
            questStatus status = null;
            bool bCreateTransaction = trans == null;


            // Perform update in this transaction.
            status = update((MasterPricingEntities)trans.DbContext, filterView);
            if (!questStatusDef.IsSuccess(status))
            {
                RollbackTransaction(trans);
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Delete(FilterViewId filterViewId)
        {
            // Initialize
            questStatus status = null;


            // Perform delete.
            using (MasterPricingEntities dbContext = new MasterPricingEntities())
            {
                status = delete(dbContext, filterViewId);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Delete(DbMgrTransaction trans, FilterViewId filterViewId)
        {
            // Initialize
            questStatus status = null;


            // Perform delete in this transaction.
            status = delete((MasterPricingEntities)trans.DbContext, filterViewId);
            if (! questStatusDef.IsSuccess(status))
            {
                RollbackTransaction(trans);
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Delete(FilterId filterId)
        {
            // Initialize
            questStatus status = null;


            // Perform delete.
            using (MasterPricingEntities dbContext = new MasterPricingEntities())
            {
                status = delete(dbContext, filterId);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Delete(DbMgrTransaction trans, FilterId filterId)
        {
            // Initialize
            questStatus status = null;


            // Perform delete in this transaction.
            status = delete((MasterPricingEntities)trans.DbContext, filterId);
            if (!questStatusDef.IsSuccess(status))
            {
                RollbackTransaction(trans);
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus List(QueryOptions queryOptions, out List<Quest.Functional.MasterPricing.FilterView> filterViewList, out QueryResponse queryResponse)
        {
            // Initialize
            questStatus status = null;
            filterViewList = null;
            queryResponse = null;


            using (MasterPricingEntities dbContext = new MasterPricingEntities())
            {
                using (DbContextTransaction tx = dbContext.Database.BeginTransaction(IsolationLevel.ReadUncommitted))
                {
                    try
                    {
                        PropertyInfo[] dbProperties = typeof(Quest.Services.Dbio.MasterPricing.FilterViews).GetProperties().ToArray();
                        int totalRecords = dbContext.FilterViews.Where(BuildWhereClause(queryOptions, dbProperties)).Count();
                        List<Quest.Services.Dbio.MasterPricing.FilterViews> _countriesList = dbContext.FilterViews.Where(BuildWhereClause(queryOptions, dbProperties))
                                .OrderBy(BuildSortString(queryOptions.SortColumns))
                                .Skip(queryOptions.Paging.PageSize * (queryOptions.Paging.PageNumber - 1))
                                .Take(queryOptions.Paging.PageSize).ToList();
                        if (_countriesList == null)
                        {
                            return (new questStatus(Severity.Warning));
                        }
                        filterViewList = new List<Quest.Functional.MasterPricing.FilterView>();
                        foreach (Quest.Services.Dbio.MasterPricing.FilterViews _filterViews in _countriesList)
                        {
                            Quest.Functional.MasterPricing.FilterView filterView = new Quest.Functional.MasterPricing.FilterView();
                            BufferMgr.TransferBuffer(_filterViews, filterView);
                            filterViewList.Add(filterView);
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


        #region FilterViews
        /*----------------------------------------------------------------------------------------------------------------------------------
         * FilterViews
         *---------------------------------------------------------------------------------------------------------------------------------*/
        private questStatus create(MasterPricingEntities dbContext, Quest.Functional.MasterPricing.FilterView filterView, out FilterViewId filterViewId)
        {
            // Initialize
            filterViewId = null;


            // Perform create
            try
            {
                Quest.Services.Dbio.MasterPricing.FilterViews _filterViews = new Quest.Services.Dbio.MasterPricing.FilterViews();
                BufferMgr.TransferBuffer(filterView, _filterViews);
                dbContext.FilterViews.Add(_filterViews);
                dbContext.SaveChanges();
                if (_filterViews.Id == 0)
                {
                    return (new questStatus(Severity.Error, "Quest.Functional.MasterPricing.FilterView not created"));
                }
                filterViewId = new FilterViewId(_filterViews.Id);
            }
            catch (System.Exception ex)
            {
                return (new questStatus(Severity.Fatal, String.Format("EXCEPTION: {0}.{1}: {2}",
                        this.GetType().Name, MethodBase.GetCurrentMethod().Name,
                        ex.InnerException != null ? ex.InnerException.Message : ex.Message)));
            }
            return (new questStatus(Severity.Success));
        }
        private questStatus create(MasterPricingEntities dbContext, List<Quest.Functional.MasterPricing.FilterView> filterViewList, out List<Quest.Functional.MasterPricing.FilterView> filterViewIdList)
        {
            // Initialize
            filterViewIdList = null;


            // Perform create
            try
            {
                List<Quest.Services.Dbio.MasterPricing.FilterViews> _filterViewList = new List<Quest.Services.Dbio.MasterPricing.FilterViews>();
                foreach (Quest.Functional.MasterPricing.FilterView filterView in filterViewList)
                {
                    Quest.Services.Dbio.MasterPricing.FilterViews _filterView = new Quest.Services.Dbio.MasterPricing.FilterViews();
                    BufferMgr.TransferBuffer(filterView, _filterView);
                    _filterViewList.Add(_filterView);
                }
                dbContext.FilterViews.AddRange(_filterViewList);
                dbContext.SaveChanges();

                filterViewIdList = new List<FilterView>();
                foreach (Quest.Services.Dbio.MasterPricing.FilterViews _filterView in _filterViewList)
                {
                    Quest.Functional.MasterPricing.FilterView filterView = new FilterView();
                    filterView.Id = _filterView.Id;
                    filterViewIdList.Add(filterView);
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
        private questStatus read(MasterPricingEntities dbContext, FilterViewId filterViewId, out Quest.Services.Dbio.MasterPricing.FilterViews filterView)
        {
            // Initialize
            filterView = null;


            try
            {
                filterView = dbContext.FilterViews.Where(r => r.Id == filterViewId.Id).SingleOrDefault();
                if (filterView == null)
                {
                    return (new questStatus(Severity.Error, String.Format("ERROR: {0}.{1}: {2}",
                            this.GetType().Name, MethodBase.GetCurrentMethod().Name,
                            String.Format("Id {0} not found", filterViewId.Id))));
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
        private questStatus read(MasterPricingEntities dbContext, FilterViewNameIdentifier filterViewNameIdentifier, out Quest.Services.Dbio.MasterPricing.FilterViews filterView)
        {
            // Initialize
            filterView = null;


            try
            {
                filterView = dbContext.FilterViews.Where(r => r.FilterId == filterViewNameIdentifier.FilterId.Id && r.Schema == filterViewNameIdentifier.Schema && r.Name == filterViewNameIdentifier.Name).SingleOrDefault();
                if (filterView == null)
                {
                    return (new questStatus(Severity.Error, String.Format("ERROR: {0}.{1}: {2}",
                            this.GetType().Name, MethodBase.GetCurrentMethod().Name,
                            String.Format("FilterViewNameIdentifier FilterId:{0}  Schema:{1}  Name:{2}  not found",
                                filterViewNameIdentifier.FilterId.Id, filterViewNameIdentifier.Schema, filterViewNameIdentifier.Name))));
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
        private questStatus read(MasterPricingEntities dbContext, FilterId filterId, out List<Quest.Services.Dbio.MasterPricing.FilterViews> filterViewList)
        {
            // Initialize
            filterViewList = null;


            try
            {
                filterViewList = dbContext.FilterViews.Where(r => r.FilterId == filterId.Id).ToList();
                if (filterViewList == null)
                {
                    return (new questStatus(Severity.Error, String.Format("ERROR: {0}.{1}: {2}",
                            this.GetType().Name, MethodBase.GetCurrentMethod().Name,
                            String.Format("FilterId {0} not found", filterId.Id))));
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
        private questStatus update(MasterPricingEntities dbContext, Quest.Functional.MasterPricing.FilterView filterView)
        {
            // Initialize 
            questStatus status = null;


            try
            {
                // Read the record.
                FilterViewId filterViewId = new FilterViewId(filterView.Id);
                Quest.Services.Dbio.MasterPricing.FilterViews _filterViews = null;
                status = read(dbContext, filterViewId, out _filterViews);
                if (! questStatusDef.IsSuccess(status))
                {
                    return (status);
                }

                // Update the record.
                BufferMgr.TransferBuffer(filterView, _filterViews);
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
        private questStatus delete(MasterPricingEntities dbContext, FilterViewId filterViewId)
        {
            try
            {
                dbContext.FilterViews.RemoveRange(dbContext.FilterViews.Where(r => r.Id == filterViewId.Id));
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
        private questStatus delete(MasterPricingEntities dbContext, FilterId filterId)
        {
            try
            {
                dbContext.FilterViews.RemoveRange(dbContext.FilterViews.Where(r => r.FilterId == filterId.Id));
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
