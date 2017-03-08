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
using Quest.MasterPricing.Services.Business.Tablesets;
using Quest.MasterPricing.Services.Business.Database;


namespace Quest.MasterPricing.Setup.Modelers
{
    public class TablesetEditorModeler : SetupBaseModeler
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
        public TablesetEditorModeler(HttpRequestBase httpRequestBase, UserSession userSession)
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
        public questStatus Save(TablesetEditorViewModel tablesetEditorViewModel)
        {
            // Initialize
            questStatus status = null;


            // Transfer model
            Quest.Functional.MasterPricing.Tableset tableset = new Functional.MasterPricing.Tableset();
            BufferMgr.TransferBuffer(tablesetEditorViewModel, tableset);


            // Determine if this is a create or update
            TablesetsMgr tablesetsMgr = new TablesetsMgr(this.UserSession);
            if (tablesetEditorViewModel.Id < BaseId.VALID_ID)
            {
                // Create
                TablesetId tablesetId = null;
                status = tablesetsMgr.Create(tableset, out tablesetId);
                if (!questStatusDef.IsSuccess(status))
                {
                    FormatErrorMessage(status, tablesetEditorViewModel);
                    return (status);
                }
                tablesetEditorViewModel.Id = tablesetId.Id;
            }
            else
            {
                // Update
                status = tablesetsMgr.Update(tableset);
                if (!questStatusDef.IsSuccess(status))
                {
                    FormatErrorMessage(status, tablesetEditorViewModel);
                    return (status);
                }
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Read(TablesetId tablesetId, out TablesetEditorViewModel tablesetEditorViewModel)
        {
            // Initialize
            questStatus status = null;
            tablesetEditorViewModel = null;


            // Read
            Quest.Functional.MasterPricing.Tableset tableset = null;
            TablesetsMgr tablesetsMgr = new TablesetsMgr(this.UserSession);
            status = tablesetsMgr.Read(tablesetId, out tableset);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }

            // Transfer model.
            tablesetEditorViewModel = new TablesetEditorViewModel(this.UserSession);
            BufferMgr.TransferBuffer(tableset, tablesetEditorViewModel);


            return (new questStatus(Severity.Success));
        }
        public questStatus Delete(TablesetId tablesetId)
        {
            // Initialize
            questStatus status = null;


            // Delete
            TablesetMgr tablesetMgr = new TablesetMgr(this.UserSession);
            status = tablesetMgr.Delete(tablesetId);
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