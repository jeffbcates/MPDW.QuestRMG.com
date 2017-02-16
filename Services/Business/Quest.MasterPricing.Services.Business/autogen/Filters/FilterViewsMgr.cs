using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Threading.Tasks;
using Quest.Util.Status;
using Quest.Util.Buffer;
using Quest.Util.Data;
using Quest.Functional.ASM;
using Quest.Functional.MasterPricing;
using Quest.MPDW.Services.Data;
using Quest.MPDW.Services.Business;
using Quest.MasterPricing.Services.Data.Filters;


namespace Quest.MasterPricing.Services.Business.Filters
{
    public class FilterViewsMgr : MgrSessionBased
    {
        #region Declarations
        /*==================================================================================================================================
         * Declarations
         *=================================================================================================================================*/
        private DbFilterViewsMgr _dbFilterViewsMgr = null;

        #endregion


        #region Constructors
        /*==================================================================================================================================
         * Constructors
         *=================================================================================================================================*/
        public FilterViewsMgr(UserSession userSession)
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
        public questStatus Create(FilterView filterView, out FilterViewId filterViewId)
        {
            // Initialize
            questStatus status = null;
            filterViewId = null;


            // Create filterView
            status = _dbFilterViewsMgr.Create(filterView, out filterViewId);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Create(DbMgrTransaction trans, FilterView filterView, out FilterViewId filterViewId)
        {
            // Initialize
            questStatus status = null;
            filterViewId = null;


            // Create filterView
            status = _dbFilterViewsMgr.Create(trans, filterView, out filterViewId);
            if (!questStatusDef.IsSuccess(status))
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


            // Create filterView
            status = _dbFilterViewsMgr.Create(trans, filterViewList, out filterViewIdList);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Read(FilterViewId filterViewId, out FilterView filterView)
        {
            // Initialize
            questStatus status = null;
            filterView = null;


            // Read filterView
            status = _dbFilterViewsMgr.Read(filterViewId, out filterView);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Read(DbMgrTransaction trans, FilterViewId filterViewId, out FilterView filterView)
        {
            // Initialize
            questStatus status = null;
            filterView = null;


            // Read filterView
            status = _dbFilterViewsMgr.Read(trans, filterViewId, out filterView);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Update(FilterView filterView)
        {
            // Initialize
            questStatus status = null;


            // Update filterView
            status = _dbFilterViewsMgr.Update(filterView);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Update(DbMgrTransaction trans, FilterView filterView)
        {
            // Initialize
            questStatus status = null;


            // Update filterView
            status = _dbFilterViewsMgr.Update(trans, filterView);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Delete(FilterViewId filterViewId)
        {
            // Initialize
            questStatus status = null;


            // Delete filterView
            status = _dbFilterViewsMgr.Delete(filterViewId);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Delete(DbMgrTransaction trans, FilterViewId filterViewId)
        {
            // Initialize
            questStatus status = null;


            // Delete filterView
            status = _dbFilterViewsMgr.Delete(trans, filterViewId);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Delete(FilterId filterId)
        {
            // Initialize
            questStatus status = null;


            // Delete all viewViews in this filter.
            status = _dbFilterViewsMgr.Delete(filterId);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Delete(DbMgrTransaction trans, FilterId filterId)
        {
            // Initialize
            questStatus status = null;


            // Delete all viewViews in this view.
            status = _dbFilterViewsMgr.Delete(trans, filterId);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus List(QueryOptions queryOptions, out List<FilterView> filterViewList, out QueryResponse queryResponse)
        {
            // Initialize
            questStatus status = null;
            filterViewList = null;


            // List
            status = _dbFilterViewsMgr.List(queryOptions, out filterViewList, out queryResponse);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
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
                _dbFilterViewsMgr = new DbFilterViewsMgr(this.UserSession);
            }
            catch (System.Exception ex)
            {
                status = new questStatus(Severity.Fatal, String.Format("EXCEPTION: {0}.{1}: {2}",
                        this.GetType().ToString(), MethodInfo.GetCurrentMethod().Name, ex.Message));
                throw new System.Exception(status.Message, ex);
            }
            return (new questStatus(Severity.Success));
        }

        #endregion
    }
}

