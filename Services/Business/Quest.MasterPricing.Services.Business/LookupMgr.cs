﻿using System;
using System.IO;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Mail;
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
using Quest.MasterPricing.Services.Business.Tablesets;
using Quest.MasterPricing.Services.Data.Filters;


namespace Quest.MasterPricing.Services.Business.Database
{
    public class LookupMgr : Mgr
    {
        #region Declarations
        /*==================================================================================================================================
         * Declarations
         *=================================================================================================================================*/
        private DbLookupMgr _dbLookupMgr = null;
        private UserSession _userSession = null;

        #endregion


        #region Constructors
        /*==================================================================================================================================
         * Constructors
         *=================================================================================================================================*/
        public LookupMgr()
            : base()
        {
            initialize();
        }
        public LookupMgr(UserSession userSession)
            : base(userSession)
        {
            _userSession = userSession;
            initialize();
        }
        #endregion


        #region Properties
        /*==================================================================================================================================
         * Properties
         *=================================================================================================================================*/
        public UserSession UserSession
        {
            get
            {
                return (this._userSession);
            }
        }
        #endregion


        #region Public Methods
        /*==================================================================================================================================
         * Public Methods
         *=================================================================================================================================*/
        public questStatus GetLookupOptions(LookupRequest lookupRequest, List<LookupArgument> lookupArgumentList, out List<OptionValuePair> lookupOptionList)
        {
            // Initialize
            questStatus status = null;


            // Get lookup options
            status = _dbLookupMgr.GetLookupOptions(lookupRequest, lookupArgumentList, out lookupOptionList);
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
                _dbLookupMgr = new DbLookupMgr();
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
