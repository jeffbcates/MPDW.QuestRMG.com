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
using Quest.Functional.FMS;
using Quest.Functional.MasterPricing;
using Quest.MPDW.Services.Data;
using Quest.MPDW.Services.Business;
using Quest.MasterPricing.Services.Data.Filters;


namespace Quest.MasterPricing.Services.Business.Filters
{
    public class FilterOperatorsMgr : MgrSessionBased
    {
        #region Declarations
        /*==================================================================================================================================
         * Declarations
         *=================================================================================================================================*/
        private DbFilterOperatorsMgr _dbFilterOperatorsMgr = null;

        #endregion


        #region Constructors
        /*==================================================================================================================================
         * Constructors
         *=================================================================================================================================*/
        public FilterOperatorsMgr(UserSession userSession)
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
        public questStatus Create(FilterOperator filterOperator, out FilterOperatorId filterOperatorId)
        {
            // Initialize
            questStatus status = null;
            filterOperatorId = null;


            // Create filterOperator
            status = _dbFilterOperatorsMgr.Create(filterOperator, out filterOperatorId);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Create(DbMgrTransaction trans, FilterOperator filterOperator, out FilterOperatorId filterOperatorId)
        {
            // Initialize
            questStatus status = null;
            filterOperatorId = null;


            // Create filterOperator
            status = _dbFilterOperatorsMgr.Create(trans, filterOperator, out filterOperatorId);
            if (!questStatusDef.IsSuccess(status))
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


            // Create filterOperator
            status = _dbFilterOperatorsMgr.Create(trans, filterOperatorList, out filterOperatorIdList);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Read(FilterOperatorId filterOperatorId, out FilterOperator filterOperator)
        {
            // Initialize
            questStatus status = null;
            filterOperator = null;


            // Read filterOperator
            status = _dbFilterOperatorsMgr.Read(filterOperatorId, out filterOperator);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Read(DbMgrTransaction trans, FilterOperatorId filterOperatorId, out FilterOperator filterOperator)
        {
            // Initialize
            questStatus status = null;
            filterOperator = null;


            // Read filterOperator
            status = _dbFilterOperatorsMgr.Read(trans, filterOperatorId, out filterOperator);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Update(FilterOperator filterOperator)
        {
            // Initialize
            questStatus status = null;


            // Update filterOperator
            status = _dbFilterOperatorsMgr.Update(filterOperator);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Update(DbMgrTransaction trans, FilterOperator filterOperator)
        {
            // Initialize
            questStatus status = null;


            // Update filterOperator
            status = _dbFilterOperatorsMgr.Update(trans, filterOperator);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Delete(FilterOperatorId filterOperatorId)
        {
            // Initialize
            questStatus status = null;


            // Delete filterOperator
            status = _dbFilterOperatorsMgr.Delete(filterOperatorId);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Delete(DbMgrTransaction trans, FilterOperatorId filterOperatorId)
        {
            // Initialize
            questStatus status = null;


            // Delete filterOperator
            status = _dbFilterOperatorsMgr.Delete(trans, filterOperatorId);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus List(QueryOptions queryOptions, out List<FilterOperator> filterOperatorList, out QueryResponse queryResponse)
        {
            // Initialize
            questStatus status = null;
            filterOperatorList = null;


            // List 
            status = _dbFilterOperatorsMgr.List(queryOptions, out filterOperatorList, out queryResponse);
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
                _dbFilterOperatorsMgr = new DbFilterOperatorsMgr(this.UserSession);
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

