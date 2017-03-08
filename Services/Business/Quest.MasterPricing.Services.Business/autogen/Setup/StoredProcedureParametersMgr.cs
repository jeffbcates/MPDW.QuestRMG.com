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
using Quest.MasterPricing.Services.Data.Database;


namespace Quest.MasterPricing.Services.Business.Database
{
    public class StoredProcedureParametersMgr : MgrSessionBased
    {
        #region Declarations
        /*==================================================================================================================================
         * Declarations
         *=================================================================================================================================*/
        private DbStoredProcedureParametersMgr _dbStoredProcedureParametersMgr = null;

        #endregion


        #region Constructors
        /*==================================================================================================================================
         * Constructors
         *=================================================================================================================================*/
        public StoredProcedureParametersMgr(UserSession userSession)
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
        public questStatus Create(DbMgrTransaction trans, Quest.Functional.MasterPricing.Database database, Quest.Functional.MasterPricing.StoredProcedureParameter storedProcedureParameter, out Quest.Functional.MasterPricing.StoredProcedureParameterId storedProcedureParameterId)
        {
            // Initialize
            questStatus status = null;
            storedProcedureParameterId = null;


            // Create storedProcedureParameter
            status = _dbStoredProcedureParametersMgr.Create(trans, storedProcedureParameter, out storedProcedureParameterId);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Create(DbMgrTransaction trans, List<Quest.Functional.MasterPricing.StoredProcedureParameter> storedProcedureParameterList, out List<Quest.Functional.MasterPricing.StoredProcedureParameter> storedProcedureParameterIdList)
        {
            // Initialize
            questStatus status = null;
            storedProcedureParameterIdList = null;


            // Create storedProcedureParameter
            status = _dbStoredProcedureParametersMgr.Create(trans, storedProcedureParameterList, out storedProcedureParameterIdList);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Read(StoredProcedureParameterId storedProcedureParameterId, out StoredProcedureParameter storedProcedureParameter)
        {
            // Initialize
            questStatus status = null;
            storedProcedureParameter = null;


            // Read storedProcedureParameter
            status = _dbStoredProcedureParametersMgr.Read(storedProcedureParameterId, out storedProcedureParameter);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Read(DbMgrTransaction trans, StoredProcedureParameterId storedProcedureParameterId, out StoredProcedureParameter storedProcedureParameter)
        {
            // Initialize
            questStatus status = null;
            storedProcedureParameter = null;


            // Read storedProcedureParameter
            status = _dbStoredProcedureParametersMgr.Read(trans, storedProcedureParameterId, out storedProcedureParameter);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Update(StoredProcedureParameter storedProcedureParameter)
        {
            // Initialize
            questStatus status = null;


            // Update storedProcedureParameter
            status = _dbStoredProcedureParametersMgr.Update(storedProcedureParameter);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Update(DbMgrTransaction trans, StoredProcedureParameter storedProcedureParameter)
        {
            // Initialize
            questStatus status = null;


            // Update storedProcedureParameter
            status = _dbStoredProcedureParametersMgr.Update(trans, storedProcedureParameter);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Delete(StoredProcedureParameterId storedProcedureParameterId)
        {
            // Initialize
            questStatus status = null;


            // Delete storedProcedureParameter
            status = _dbStoredProcedureParametersMgr.Delete(storedProcedureParameterId);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Delete(DbMgrTransaction trans, StoredProcedureParameterId storedProcedureParameterId)
        {
            // Initialize
            questStatus status = null;


            // Delete storedProcedureParameter
            status = _dbStoredProcedureParametersMgr.Delete(trans, storedProcedureParameterId);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Delete(StoredProcedureId storedProcedureId)
        {
            // Initialize
            questStatus status = null;


            // Delete all procedureParameterProcedureParameters in this stored.
            status = _dbStoredProcedureParametersMgr.Delete(storedProcedureId);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Delete(DbMgrTransaction trans, StoredProcedureId storedProcedureId)
        {
            // Initialize
            questStatus status = null;


            // Delete all procedureParameterProcedureParameters in this procedureParameter.
            status = _dbStoredProcedureParametersMgr.Delete(trans, storedProcedureId);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus List(QueryOptions queryOptions, out List<StoredProcedureParameter> storedProcedureParameterList, out QueryResponse queryResponse)
        {
            // Initialize
            questStatus status = null;
            storedProcedureParameterList = null;


            // List
            status = _dbStoredProcedureParametersMgr.List(queryOptions, out storedProcedureParameterList, out queryResponse);
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
                _dbStoredProcedureParametersMgr = new DbStoredProcedureParametersMgr(this.UserSession);
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

