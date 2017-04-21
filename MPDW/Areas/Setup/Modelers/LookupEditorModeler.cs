using System;
using System.Configuration;
using System.Linq;
using System.Web;
using Quest.Util.Status;
using Quest.Util.Buffer;
using Quest.Util.Data;
using Quest.Functional.ASM;
using Quest.Functional.FMS;
using Quest.Functional.MasterPricing;
using Quest.MasterPricing.Setup.Models;
using Quest.MasterPricing.Services.Business.Database;


namespace Quest.MasterPricing.Setup.Modelers
{
    public class LookupEditorModeler : SetupBaseModeler
    {
        #region Declarations
        /*==================================================================================================================================
        * Declarations
        *=================================================================================================================================*/
        #endregion


        #region Constructors
        /*==================================================================================================================================
        * Constructors
        *=================================================================================================================================*/
        public LookupEditorModeler(HttpRequestBase httpRequestBase, UserSession userSession)
            : base(httpRequestBase, userSession)
        {
            initialize();
        }
        #endregion


        #region Public Methods
        /*==================================================================================================================================
        * Public Methods
        *=================================================================================================================================*/

        #region CRUD 
        //----------------------------------------------------------------------------------------------------------------------------------
        // CRUD
        //----------------------------------------------------------------------------------------------------------------------------------
        public questStatus Save(LookupEditorViewModel lookupEditorViewModel)
        {
            // Initialize
            questStatus status = null;


            // Transfer model
            Quest.Functional.MasterPricing.Lookup lookup = new Functional.MasterPricing.Lookup();
            BufferMgr.TransferBuffer(lookupEditorViewModel, lookup);


            // Determine if this is a create or update
            LookupsMgr lookupsMgr = new LookupsMgr(this.UserSession);
            if (lookupEditorViewModel.Id < BaseId.VALID_ID)
            {
                // Create
                LookupId lookupId = null;
                status = lookupsMgr.Create(lookup, out lookupId);
                if (!questStatusDef.IsSuccess(status))
                {
                    FormatErrorMessage(status, lookupEditorViewModel);
                    return (status);
                }
                lookupEditorViewModel.Id = lookupId.Id;
            }
            else
            {
                // Update
                status = lookupsMgr.Update(lookup);
                if (!questStatusDef.IsSuccess(status))
                {
                    FormatErrorMessage(status, lookupEditorViewModel);
                    return (status);
                }
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Read(LookupId lookupId, out LookupEditorViewModel lookupEditorViewModel)
        {
            // Initialize
            questStatus status = null;
            lookupEditorViewModel = null;


            // Read
            Quest.Functional.MasterPricing.Lookup lookup = null;
            LookupsMgr lookupsMgr = new LookupsMgr(this.UserSession);
            status = lookupsMgr.Read(lookupId, out lookup);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }

            // Transfer model.
            lookupEditorViewModel = new LookupEditorViewModel(this.UserSession);
            BufferMgr.TransferBuffer(lookup, lookupEditorViewModel);



            return (new questStatus(Severity.Success));
        }
        public questStatus Delete(LookupId lookupId)
        {
            // Initialize
            questStatus status = null;


            // Delete
            LookupsMgr lookupsMgr = new LookupsMgr(this.UserSession);
            status = lookupsMgr.Delete(lookupId);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        #endregion


        #region Options
        //----------------------------------------------------------------------------------------------------------------------------------
        // Options
        //----------------------------------------------------------------------------------------------------------------------------------
        #endregion


        #region Validations
        //----------------------------------------------------------------------------------------------------------------------------------
        // Validations
        //----------------------------------------------------------------------------------------------------------------------------------
        // TODO: VALIDATE MODELS FOR REQUIRED FIELDS IN HERE.
        #endregion

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