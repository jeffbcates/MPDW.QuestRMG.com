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
    public class FilterOperationsMgr : MgrSessionBased
    {
        #region Declarations
        /*==================================================================================================================================
         * Declarations
         *=================================================================================================================================*/
        private DbFilterOperationsMgr _dbFilterOperationsMgr = null;

        #endregion


        #region Constructors
        /*==================================================================================================================================
         * Constructors
         *=================================================================================================================================*/
        public FilterOperationsMgr(UserSession userSession)
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
        public questStatus Create(FilterOperation filterOperation, out FilterOperationId filterOperationId)
        {
            // Initialize
            questStatus status = null;
            filterOperationId = null;


            // Create filterOperation
            status = _dbFilterOperationsMgr.Create(filterOperation, out filterOperationId);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Create(DbMgrTransaction trans, FilterOperation filterOperation, out FilterOperationId filterOperationId)
        {
            // Initialize
            questStatus status = null;
            filterOperationId = null;


            // Create filterOperation
            status = _dbFilterOperationsMgr.Create(trans, filterOperation, out filterOperationId);
            if (!questStatusDef.IsSuccess(status))
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


            // Create filterOperation
            status = _dbFilterOperationsMgr.Create(trans, filterOperationList, out filterOperationIdList);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Read(FilterOperationId filterOperationId, out FilterOperation filterOperation)
        {
            // Initialize
            questStatus status = null;
            filterOperation = null;


            // Read filterOperation
            status = _dbFilterOperationsMgr.Read(filterOperationId, out filterOperation);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Read(DbMgrTransaction trans, FilterOperationId filterOperationId, out FilterOperation filterOperation)
        {
            // Initialize
            questStatus status = null;
            filterOperation = null;


            // Read filterOperation
            status = _dbFilterOperationsMgr.Read(trans, filterOperationId, out filterOperation);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Update(FilterOperation filterOperation)
        {
            // Initialize
            questStatus status = null;


            // Update filterOperation
            status = _dbFilterOperationsMgr.Update(filterOperation);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Update(DbMgrTransaction trans, FilterOperation filterOperation)
        {
            // Initialize
            questStatus status = null;


            // Update filterOperation
            status = _dbFilterOperationsMgr.Update(trans, filterOperation);
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


            // Delete filterOperation
            status = _dbFilterOperationsMgr.Delete(filterOperationId);
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


            // Delete filterOperation
            status = _dbFilterOperationsMgr.Delete(trans, filterOperationId);
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
            status = _dbFilterOperationsMgr.Delete(filterItemId);
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
            status = _dbFilterOperationsMgr.Delete(trans, filterItemId);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus List(QueryOptions queryOptions, out List<FilterOperation> filterOperationList, out QueryResponse queryResponse)
        {
            // Initialize
            questStatus status = null;
            filterOperationList = null;


            // List usStates
            status = _dbFilterOperationsMgr.List(queryOptions, out filterOperationList, out queryResponse);
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
                _dbFilterOperationsMgr = new DbFilterOperationsMgr(this.UserSession);
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

