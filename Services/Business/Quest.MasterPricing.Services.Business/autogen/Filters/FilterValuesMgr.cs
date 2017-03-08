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
    public class FilterValuesMgr : MgrSessionBased
    {
        #region Declarations
        /*==================================================================================================================================
         * Declarations
         *=================================================================================================================================*/
        private DbFilterValuesMgr _dbFilterValuesMgr = null;

        #endregion


        #region Constructors
        /*==================================================================================================================================
         * Constructors
         *=================================================================================================================================*/
        public FilterValuesMgr(UserSession userSession)
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
        public questStatus Create(FilterValue filterValue, out FilterValueId filterValueId)
        {
            // Initialize
            questStatus status = null;
            filterValueId = null;


            // Create filterValue
            status = _dbFilterValuesMgr.Create(filterValue, out filterValueId);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Create(DbMgrTransaction trans, FilterValue filterValue, out FilterValueId filterValueId)
        {
            // Initialize
            questStatus status = null;
            filterValueId = null;


            // Create filterValue
            status = _dbFilterValuesMgr.Create(trans, filterValue, out filterValueId);
            if (!questStatusDef.IsSuccess(status))
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


            // Create filterValue
            status = _dbFilterValuesMgr.Create(trans, filterValueList, out filterValueIdList);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Read(FilterValueId filterValueId, out FilterValue filterValue)
        {
            // Initialize
            questStatus status = null;
            filterValue = null;


            // Read filterValue
            status = _dbFilterValuesMgr.Read(filterValueId, out filterValue);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Read(DbMgrTransaction trans, FilterValueId filterValueId, out FilterValue filterValue)
        {
            // Initialize
            questStatus status = null;
            filterValue = null;


            // Read filterValue
            status = _dbFilterValuesMgr.Read(trans, filterValueId, out filterValue);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Read(FilterOperationId filterOperationId, out List<FilterValue> filterValueList)
        {
            // Initialize
            questStatus status = null;
            filterValueList = null;


            // Read filterValue
            status = _dbFilterValuesMgr.Read(filterOperationId, out filterValueList);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Read(DbMgrTransaction trans, FilterOperationId filterOperationId, out List<FilterValue> filterValueList)
        {
            // Initialize
            questStatus status = null;
            filterValueList = null;


            // Read filterValue
            status = _dbFilterValuesMgr.Read(trans, filterOperationId, out filterValueList);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Update(FilterValue filterValue)
        {
            // Initialize
            questStatus status = null;


            // Update filterValue
            status = _dbFilterValuesMgr.Update(filterValue);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Update(DbMgrTransaction trans, FilterValue filterValue)
        {
            // Initialize
            questStatus status = null;


            // Update filterValue
            status = _dbFilterValuesMgr.Update(trans, filterValue);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Delete(FilterValueId filterValueId)
        {
            // Initialize
            questStatus status = null;


            // Delete filterValue
            status = _dbFilterValuesMgr.Delete(filterValueId);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Delete(DbMgrTransaction trans, FilterValueId filterValueId)
        {
            // Initialize
            questStatus status = null;


            // Delete filterValue
            status = _dbFilterValuesMgr.Delete(trans, filterValueId);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Delete(FilterOperationId filterOperationId)
        {
            // Initialize
            questStatus status = null;


            // Delete all tableTables in this table.
            status = _dbFilterValuesMgr.Delete(filterOperationId);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Delete(DbMgrTransaction trans, FilterOperationId filterOperationId)
        {
            // Initialize
            questStatus status = null;


            // Delete all tableTables in this table.
            status = _dbFilterValuesMgr.Delete(trans, filterOperationId);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus List(QueryOptions queryOptions, out List<FilterValue> filterValueList, out QueryResponse queryResponse)
        {
            // Initialize
            questStatus status = null;
            filterValueList = null;


            // List
            status = _dbFilterValuesMgr.List(queryOptions, out filterValueList, out queryResponse);
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
                _dbFilterValuesMgr = new DbFilterValuesMgr(this.UserSession);
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

