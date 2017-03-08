using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using Quest.Util.Status;
using Quest.Util.Buffer;
using Quest.Util.Data;
using Quest.Functional.ASM;
using Quest.Functional.FMS;
using Quest.MPDW.Admin.Models;
using Quest.MPDW.Services.Business.Accounts;


namespace Quest.MPDW.Admin.Modelers
{
    public class PrivilegeEditorModeler : AdminBaseModeler
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
        public PrivilegeEditorModeler(HttpRequestBase httpRequestBase, UserSession userSession)
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
        public questStatus Save(PrivilegeEditorViewModel privilegeEditorViewModel)
        {
            // Initialize
            questStatus status = null;


            // Transfer model
            Quest.Functional.ASM.Privilege privilege = new Quest.Functional.ASM.Privilege();
            BufferMgr.TransferBuffer(privilegeEditorViewModel, privilege);


            // Determine if this is a create or update
            PrivilegesMgr privilegesMgr = new PrivilegesMgr(this.UserSession);
            if (privilegeEditorViewModel.Id < BaseId.VALID_ID)
            {
                // Create
                PrivilegeId privilegeId = null;
                status = privilegesMgr.Create(privilege, out privilegeId);
                if (!questStatusDef.IsSuccess(status))
                {
                    FormatErrorMessage(status, privilegeEditorViewModel);
                    return (status);
                }
                privilegeEditorViewModel.Id = privilegeId.Id;
            }
            else
            {
                // Update
                status = privilegesMgr.Update(privilege);
                if (!questStatusDef.IsSuccess(status))
                {
                    FormatErrorMessage(status, privilegeEditorViewModel);
                    return (status);
                }
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Read(PrivilegeId privilegeId, out PrivilegeEditorViewModel privilegeEditorViewModel)
        {
            // Initialize
            questStatus status = null;
            privilegeEditorViewModel = null;


            // Read
            Quest.Functional.ASM.Privilege privilege = null;
            PrivilegesMgr privilegesMgr = new PrivilegesMgr(this.UserSession);
            status = privilegesMgr.Read(privilegeId, out privilege);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }

            // Transfer model.
            privilegeEditorViewModel = new PrivilegeEditorViewModel(this.UserSession);
            BufferMgr.TransferBuffer(privilege, privilegeEditorViewModel);



            return (new questStatus(Severity.Success));
        }
        public questStatus Delete(PrivilegeId privilegeId)
        {
            // Initialize
            questStatus status = null;


            // Delete
            PrivilegesMgr privilegesMgr = new PrivilegesMgr(this.UserSession);
            status = privilegesMgr.Delete(privilegeId);
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