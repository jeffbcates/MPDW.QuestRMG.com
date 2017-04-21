using System;
using System.Collections.Generic;
using System.Web;
using Quest.Util.Status;
using Quest.Util.Buffer;
using Quest.Util.Data;
using Quest.Functional.ASM;
using Quest.Functional.FMS;
using Quest.MPDW.Modelers;
using Quest.MasterPricing.DataMgr.Models;
using Quest.Functional.MasterPricing;
using Quest.MasterPricing.Services.Business.Database;


namespace Quest.MasterPricing.DataMgr.Modelers
{
    public class LookupModeler : DataMgrBaseModeler
    {
        #region Declarations
        /*==================================================================================================================================
         * Declarations
         *=================================================================================================================================*/
        private DataMgrBaseViewModel _dataMgrBaseViewModel = null;

        #endregion


        #region Constructors
        /*==================================================================================================================================
        * ConstructorsRequisitionListModeler
        *=================================================================================================================================*/
        public LookupModeler(HttpRequestBase httpRequestBase, UserSession userSession)
            : base(httpRequestBase, userSession)
        {
            initialize();
        }
        public LookupModeler(HttpRequestBase httpRequestBase, UserSession userSession, DataMgrBaseViewModel dataMgrBaseViewModel)
            : base(httpRequestBase, userSession)
        {
            this._dataMgrBaseViewModel = dataMgrBaseViewModel;
            initialize();
        }
        #endregion


        #region Public Methods
        /*==================================================================================================================================
         * Public Methods
         *=================================================================================================================================*/
        public questStatus Read(LookupViewModel lookupViewModel, out List<OptionValuePair> lookupOptionList)
        {
            // Initialize
            questStatus status = null;
            lookupOptionList = null;


            // Create lookup request
            LookupRequest lookupRequest = new LookupRequest();
            lookupRequest.LookupId = new LookupId(lookupViewModel.Id);
            if (lookupViewModel.FilterItemId >= BaseId.VALID_ID)
            {
                lookupRequest.FilterItemId = new FilterItemId(lookupViewModel.FilterItemId);
            }

            // Get lookup options
            LookupId lookupId = new LookupId(lookupViewModel.Id);
            List<LookupArgument> lookupArgumentList = null;    // TODO
            LookupMgr lookupMgr = new LookupMgr(this.UserSession);
            status = lookupMgr.GetLookupOptions(lookupRequest, lookupArgumentList, out lookupOptionList);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }

            // Insert default option
            status = AddDefaultOptions(lookupOptionList, "-1", "Select one ...");
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
            return (new questStatus(Severity.Success));
        }
        #endregion
    }
}