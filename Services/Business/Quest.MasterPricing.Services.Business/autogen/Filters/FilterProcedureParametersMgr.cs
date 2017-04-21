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
    public class FilterProcedureParametersMgr : MgrSessionBased
    {
        #region Declarations
        /*==================================================================================================================================
         * Declarations
         *=================================================================================================================================*/
        private DbFilterProcedureParametersMgr _dbFilterProcedureParametersMgr = null;

        #endregion


        #region Constructors
        /*==================================================================================================================================
         * Constructors
         *=================================================================================================================================*/
        public FilterProcedureParametersMgr(UserSession userSession)
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
        public questStatus Create(Quest.Functional.MasterPricing.Database database, FilterProcedureParameter filterProcedureParameter, out FilterProcedureParameterId filterProcedureParameterId)
        {
            // Initialize
            questStatus status = null;
            filterProcedureParameterId = null;


            // Create filterProcedureParameter
            status = _dbFilterProcedureParametersMgr.Create(filterProcedureParameter, out filterProcedureParameterId);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Create(DbMgrTransaction trans, Quest.Functional.MasterPricing.Database database, FilterProcedureParameter filterProcedureParameter, out FilterProcedureParameterId filterProcedureParameterId)
        {
            // Initialize
            questStatus status = null;
            filterProcedureParameterId = null;


            // Create filterProcedureParameter
            status = _dbFilterProcedureParametersMgr.Create(trans, filterProcedureParameter, out filterProcedureParameterId);
            if (!questStatusDef.IsSuccess(status))
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


            // Create filterProcedureParameter
            status = _dbFilterProcedureParametersMgr.Create(trans, filterProcedureParameterList, out filterProcedureParameterIdList);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Read(FilterProcedureParameterId filterProcedureParameterId, out FilterProcedureParameter filterProcedureParameter)
        {
            // Initialize
            questStatus status = null;
            filterProcedureParameter = null;


            // Read filterProcedureParameter
            status = _dbFilterProcedureParametersMgr.Read(filterProcedureParameterId, out filterProcedureParameter);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Read(DbMgrTransaction trans, FilterProcedureParameterId filterProcedureParameterId, out FilterProcedureParameter filterProcedureParameter)
        {
            // Initialize
            questStatus status = null;
            filterProcedureParameter = null;


            // Read filterProcedureParameter
            status = _dbFilterProcedureParametersMgr.Read(trans, filterProcedureParameterId, out filterProcedureParameter);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Update(FilterProcedureParameter filterProcedureParameter)
        {
            // Initialize
            questStatus status = null;


            // Update filterProcedureParameter
            status = _dbFilterProcedureParametersMgr.Update(filterProcedureParameter);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Update(DbMgrTransaction trans, FilterProcedureParameter filterProcedureParameter)
        {
            // Initialize
            questStatus status = null;


            // Update filterProcedureParameter
            status = _dbFilterProcedureParametersMgr.Update(trans, filterProcedureParameter);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Delete(FilterProcedureParameterId filterProcedureParameterId)
        {
            // Initialize
            questStatus status = null;


            // Delete filterProcedureParameter
            status = _dbFilterProcedureParametersMgr.Delete(filterProcedureParameterId);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Delete(DbMgrTransaction trans, FilterProcedureParameterId filterProcedureParameterId)
        {
            // Initialize
            questStatus status = null;


            // Delete filterProcedureParameter
            status = _dbFilterProcedureParametersMgr.Delete(trans, filterProcedureParameterId);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Delete(FilterProcedureId filterProcedureId)
        {
            // Initialize
            questStatus status = null;


            // Delete all procedureProcedures in this filter.
            status = _dbFilterProcedureParametersMgr.Delete(filterProcedureId);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Delete(DbMgrTransaction trans, FilterProcedureId filterProcedureId)
        {
            // Initialize
            questStatus status = null;


            // Delete all procedureProcedures in this procedure.
            status = _dbFilterProcedureParametersMgr.Delete(trans, filterProcedureId);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus List(QueryOptions queryOptions, out List<FilterProcedureParameter> filterProcedureParameterList, out QueryResponse queryResponse)
        {
            // Initialize
            questStatus status = null;
            filterProcedureParameterList = null;


            // List
            status = _dbFilterProcedureParametersMgr.List(queryOptions, out filterProcedureParameterList, out queryResponse);
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
                _dbFilterProcedureParametersMgr = new DbFilterProcedureParametersMgr(this.UserSession);
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

