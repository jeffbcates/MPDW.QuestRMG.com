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
    public class LookupsMgr : MgrSessionBased
    {
        #region Declarations
        /*==================================================================================================================================
         * Declarations
         *=================================================================================================================================*/
        private DbLookupsMgr _dbLookupsMgr = null;

        #endregion


        #region Constructors
        /*==================================================================================================================================
         * Constructors
         *=================================================================================================================================*/
        public LookupsMgr(UserSession userSession)
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
        public questStatus Create(Lookup lookup, out LookupId lookupId)
        {
            // Initialize
            lookupId = null;
            questStatus status = null;


            // Create lookup
            status = _dbLookupsMgr.Create(lookup, out lookupId);
            if (! questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Create(DbMgrTransaction trans, Lookup lookup, out LookupId lookupId)
        {
            // Initialize
            lookupId = null;
            questStatus status = null;


            // Create lookup
            status = _dbLookupsMgr.Create(trans, lookup, out lookupId);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Read(LookupId lookupId, out Lookup lookup)
        {
            // Initialize
            lookup = null;
            questStatus status = null;


            // Read lookup
            status = _dbLookupsMgr.Read(lookupId, out lookup);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Read(DbMgrTransaction trans, LookupId lookupId, out Lookup lookup)
        {
            // Initialize
            lookup = null;
            questStatus status = null;


            // Read lookup
            status = _dbLookupsMgr.Read(trans, lookupId, out lookup);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Update(Lookup lookup)
        {
            // Initialize
            questStatus status = null;


            // Update lookup
            status = _dbLookupsMgr.Update(lookup);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Update(DbMgrTransaction trans, Lookup lookup)
        {
            // Initialize
            questStatus status = null;


            // Update lookup
            status = _dbLookupsMgr.Update(trans, lookup);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Delete(LookupId lookupId)
        {
            // Initialize
            questStatus status = null;


            // Delete lookup
            status = _dbLookupsMgr.Delete(lookupId);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Delete(DbMgrTransaction trans, LookupId lookupId)
        {
            // Initialize
            questStatus status = null;


            // Delete lookup
            status = _dbLookupsMgr.Delete(trans, lookupId);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus List(QueryOptions queryOptions, out List<Lookup> lookupList, out QueryResponse queryResponse)
        {
            // Initialize
            questStatus status = null;
            lookupList = null;


            // List usStates
            status = _dbLookupsMgr.List(queryOptions, out lookupList, out queryResponse);
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
                _dbLookupsMgr = new DbLookupsMgr(this.UserSession);
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

