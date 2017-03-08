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
    public class FilterItemJoinsMgr : MgrSessionBased
    {
        #region Declarations
        /*==================================================================================================================================
         * Declarations
         *=================================================================================================================================*/
        private DbFilterItemJoinsMgr _dbFilterItemJoinsMgr = null;

        #endregion


        #region Constructors
        /*==================================================================================================================================
         * Constructors
         *=================================================================================================================================*/
        public FilterItemJoinsMgr(UserSession userSession)
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
        public questStatus Create(FilterItemJoin filterItemJoin, out FilterItemJoinId filterItemJoinId)
        {
            // Initialize
            questStatus status = null;
            filterItemJoinId = null;


            // Create filterItemJoin
            status = _dbFilterItemJoinsMgr.Create(filterItemJoin, out filterItemJoinId);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Create(DbMgrTransaction trans, FilterItemJoin filterItemJoin, out FilterItemJoinId filterItemJoinId)
        {
            // Initialize
            questStatus status = null;
            filterItemJoinId = null;


            // Create filterItemJoin
            status = _dbFilterItemJoinsMgr.Create(trans, filterItemJoin, out filterItemJoinId);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Create(DbMgrTransaction trans, List<Quest.Functional.MasterPricing.FilterItemJoin> filterItemJoinList, out List<Quest.Functional.MasterPricing.FilterItemJoin> filterItemJoinIdList)
        {
            // Initialize
            questStatus status = null;
            filterItemJoinIdList = null;


            // Create filterItemJoin
            status = _dbFilterItemJoinsMgr.Create(trans, filterItemJoinList, out filterItemJoinIdList);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Read(FilterItemJoinId filterItemJoinId, out FilterItemJoin filterItemJoin)
        {
            // Initialize
            questStatus status = null;
            filterItemJoin = null;


            // Read filterItemJoin
            status = _dbFilterItemJoinsMgr.Read(filterItemJoinId, out filterItemJoin);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Read(DbMgrTransaction trans, FilterItemJoinId filterItemJoinId, out FilterItemJoin filterItemJoin)
        {
            // Initialize
            questStatus status = null;
            filterItemJoin = null;


            // Read filterItemJoin
            status = _dbFilterItemJoinsMgr.Read(trans, filterItemJoinId, out filterItemJoin);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Update(FilterItemJoin filterItemJoin)
        {
            // Initialize
            questStatus status = null;


            // Update filterItemJoin
            status = _dbFilterItemJoinsMgr.Update(filterItemJoin);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Update(DbMgrTransaction trans, FilterItemJoin filterItemJoin)
        {
            // Initialize
            questStatus status = null;


            // Update filterItemJoin
            status = _dbFilterItemJoinsMgr.Update(trans, filterItemJoin);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Delete(FilterItemJoinId filterItemJoinId)
        {
            // Initialize
            questStatus status = null;


            // Delete filterItemJoin
            status = _dbFilterItemJoinsMgr.Delete(filterItemJoinId);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Delete(DbMgrTransaction trans, FilterItemJoinId filterItemJoinId)
        {
            // Initialize
            questStatus status = null;


            // Delete filterItemJoin
            status = _dbFilterItemJoinsMgr.Delete(trans, filterItemJoinId);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Delete(FilterItemId filterItemId)
        {
            // Initialize
            questStatus status = null;


            // Delete all tableTables in this table.
            status = _dbFilterItemJoinsMgr.Delete(filterItemId);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Delete(DbMgrTransaction trans, FilterItemId filterItemId)
        {
            // Initialize
            questStatus status = null;


            // Delete all tableTables in this table.
            status = _dbFilterItemJoinsMgr.Delete(trans, filterItemId);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus List(QueryOptions queryOptions, out List<FilterItemJoin> filterItemJoinList, out QueryResponse queryResponse)
        {
            // Initialize
            questStatus status = null;
            filterItemJoinList = null;


            // List usStates
            status = _dbFilterItemJoinsMgr.List(queryOptions, out filterItemJoinList, out queryResponse);
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
                _dbFilterItemJoinsMgr = new DbFilterItemJoinsMgr(this.UserSession);
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

