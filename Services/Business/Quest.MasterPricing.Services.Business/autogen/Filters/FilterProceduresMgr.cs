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
    public class FilterProceduresMgr : MgrSessionBased
    {
        #region Declarations
        /*==================================================================================================================================
         * Declarations
         *=================================================================================================================================*/
        private DbFilterProceduresMgr _dbFilterProceduresMgr = null;

        #endregion


        #region Constructors
        /*==================================================================================================================================
         * Constructors
         *=================================================================================================================================*/
        public FilterProceduresMgr(UserSession userSession)
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
        public questStatus Create(DbMgrTransaction trans, Quest.Functional.MasterPricing.Database database, FilterProcedure filterProcedure, out FilterProcedureId filterProcedureId)
        {
            // Initialize
            questStatus status = null;
            filterProcedureId = null;


            // Create filterProcedure
            status = _dbFilterProceduresMgr.Create(trans, filterProcedure, out filterProcedureId);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }

            // Store parameters for procedure
            List<FilterProcedureParameter> filterProcedureParameterList = null;
            status = GetStoredProdecureParameters(database, filterProcedure.Name, out filterProcedureParameterList);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            FilterProcedureParametersMgr filterProcedureParametersMgr = new FilterProcedureParametersMgr(this.UserSession);
            foreach (FilterProcedureParameter filterProcedureParameter in filterProcedureParameterList)
            {
                filterProcedureParameter.FilterProcedureId = filterProcedureId.Id;
                FilterProcedureParameterId filterProcedureParameterId = null;
                status = filterProcedureParametersMgr.Create(trans, database, filterProcedureParameter, out filterProcedureParameterId);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Create(DbMgrTransaction trans, List<Quest.Functional.MasterPricing.FilterProcedure> filterProcedureList, out List<Quest.Functional.MasterPricing.FilterProcedure> filterProcedureIdList)
        {
            // Initialize
            questStatus status = null;
            filterProcedureIdList = null;


            // Create filterProcedure
            status = _dbFilterProceduresMgr.Create(trans, filterProcedureList, out filterProcedureIdList);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Read(FilterProcedureId filterProcedureId, out FilterProcedure filterProcedure)
        {
            // Initialize
            questStatus status = null;
            filterProcedure = null;


            // Read filterProcedure
            status = _dbFilterProceduresMgr.Read(filterProcedureId, out filterProcedure);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Read(DbMgrTransaction trans, FilterProcedureId filterProcedureId, out FilterProcedure filterProcedure)
        {
            // Initialize
            questStatus status = null;
            filterProcedure = null;


            // Read filterProcedure
            status = _dbFilterProceduresMgr.Read(trans, filterProcedureId, out filterProcedure);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Update(FilterProcedure filterProcedure)
        {
            // Initialize
            questStatus status = null;


            // Update filterProcedure
            status = _dbFilterProceduresMgr.Update(filterProcedure);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Update(DbMgrTransaction trans, FilterProcedure filterProcedure)
        {
            // Initialize
            questStatus status = null;


            // Update filterProcedure
            status = _dbFilterProceduresMgr.Update(trans, filterProcedure);
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


            // Delete filterProcedure
            status = _dbFilterProceduresMgr.Delete(filterProcedureId);
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


            // Delete filterProcedure
            status = _dbFilterProceduresMgr.Delete(trans, filterProcedureId);
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


            // Delete all procedureProcedures in this filter.
            status = _dbFilterProceduresMgr.Delete(filterId);
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


            // Delete all procedureProcedures in this procedure.
            status = _dbFilterProceduresMgr.Delete(trans, filterId);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus List(QueryOptions queryOptions, out List<FilterProcedure> filterProcedureList, out QueryResponse queryResponse)
        {
            // Initialize
            questStatus status = null;
            filterProcedureList = null;


            // List
            status = _dbFilterProceduresMgr.List(queryOptions, out filterProcedureList, out queryResponse);
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
                _dbFilterProceduresMgr = new DbFilterProceduresMgr(this.UserSession);
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

