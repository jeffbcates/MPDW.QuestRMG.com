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
    public class UserGroupsModeler : AdminBaseModeler
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
        public UserGroupsModeler(HttpRequestBase httpRequestBase, UserSession userSession)
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
        public questStatus Save(UserEditorViewModel userEditorViewModel)
        {
            // Initialize
            questStatus status = null;


            // Transfer model
            Quest.Functional.ASM.User user = new Quest.Functional.ASM.User();
            BufferMgr.TransferBuffer(userEditorViewModel, user);


            // Determine if this is a create or update
            UsersMgr usersMgr = new UsersMgr(this.UserSession);
            if (userEditorViewModel.Id < BaseId.VALID_ID)
            {
                // Create
                UserId userId = null;
                status = usersMgr.Create(user, out userId);
                if (!questStatusDef.IsSuccess(status))
                {
                    FormatErrorMessage(status, userEditorViewModel);
                    return (status);
                }
                userEditorViewModel.Id = userId.Id;
            }
            else
            {
                // Update
                status = usersMgr.Update(user);
                if (!questStatusDef.IsSuccess(status))
                {
                    FormatErrorMessage(status, userEditorViewModel);
                    return (status);
                }
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Read(UserId userId, out UserGroupsViewModel userGroupsViewModel)
        {
            // Initialize
            questStatus status = null;
            userGroupsViewModel = null;


            // Read
            Quest.Functional.ASM.User user = null;
            UsersMgr usersMgr = new UsersMgr(this.UserSession);
            status = usersMgr.Read(userId, out user);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }

            // Transfer model.
            userGroupsViewModel = new UserGroupsViewModel(this.UserSession);
            BufferMgr.TransferBuffer(user, userGroupsViewModel.User);



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