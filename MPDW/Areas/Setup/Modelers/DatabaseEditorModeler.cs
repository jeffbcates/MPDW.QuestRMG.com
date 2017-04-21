using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
    public class DatabaseEditorModeler : SetupBaseModeler
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
        public DatabaseEditorModeler(HttpRequestBase httpRequestBase, UserSession userSession)
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
        public questStatus Save(DatabaseEditorViewModel databaseEditorViewModel)
        {
            // Initialize
            questStatus status = null;


            // Transfer model
            Quest.Functional.MasterPricing.Database database = new Quest.Functional.MasterPricing.Database();
            BufferMgr.TransferBuffer(databaseEditorViewModel, database);


            // Determine if this is a create or update
            DatabaseMgr databaseMgr = new DatabaseMgr(this.UserSession);
            if (databaseEditorViewModel.Id < BaseId.VALID_ID)
            {
                // Create
                DatabaseId databaseId = null;
                status = databaseMgr.Create(database, out databaseId);
                if (!questStatusDef.IsSuccess(status))
                {
                    if (databaseId != null && databaseId.Id >= BaseId.VALID_ID)
                    {
                        databaseEditorViewModel.Id = databaseId.Id;
                    }
                    FormatErrorMessage(status, databaseEditorViewModel);
                    return (status);
                }
                databaseEditorViewModel.Id = databaseId.Id;
            }
            else
            {
                // Update
                status = databaseMgr.Update(database);
                if (!questStatusDef.IsSuccess(status))
                {
                    FormatErrorMessage(status, databaseEditorViewModel);
                    return (status);
                }
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Read(DatabaseId databaseId, out DatabaseEditorViewModel databaseEditorViewModel)
        {
            // Initialize
            questStatus status = null;
            databaseEditorViewModel = null;


            // Read
            Quest.Functional.MasterPricing.Database database = null;
            DatabasesMgr databasesMgr = new DatabasesMgr(this.UserSession);
            status = databasesMgr.Read(databaseId, out database);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }

            // Transfer model.
            databaseEditorViewModel = new DatabaseEditorViewModel(this.UserSession);
            BufferMgr.TransferBuffer(database, databaseEditorViewModel);



            return (new questStatus(Severity.Success));
        }
        public questStatus Delete(DatabaseId databaseId)
        {
            // Initialize
            questStatus status = null;


            // Delete
            DatabaseMgr databaseMgr = new DatabaseMgr(this.UserSession);
            status = databaseMgr.Delete(databaseId);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        #endregion

        public questStatus RefreshSchema(DatabaseEditorViewModel databaseEditorViewModel)
        {
            // Initialize
            questStatus status = null;


            // Refresh schema
            DatabaseId databaseId = new DatabaseId(databaseEditorViewModel.Id);
            DatabaseMgr databaseMgr = new DatabaseMgr(this.UserSession);
            status = databaseMgr.RefreshSchema(databaseId);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }

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