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
    public class GroupEditorModeler : AdminBaseModeler
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
        public GroupEditorModeler(HttpRequestBase httpRequestBase, UserSession userSession)
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
        public questStatus Save(GroupEditorViewModel groupEditorViewModel)
        {
            // Initialize
            questStatus status = null;


            // Transfer model
            Quest.Functional.ASM.Group group = new Quest.Functional.ASM.Group();
            BufferMgr.TransferBuffer(groupEditorViewModel, group);


            // Determine if this is a create or update
            GroupsMgr groupsMgr = new GroupsMgr(this.UserSession);
            if (groupEditorViewModel.Id < BaseId.VALID_ID)
            {
                // Create
                GroupId groupId = null;
                status = groupsMgr.Create(group, out groupId);
                if (!questStatusDef.IsSuccess(status))
                {
                    FormatErrorMessage(status, groupEditorViewModel);
                    return (status);
                }
                groupEditorViewModel.Id = groupId.Id;
            }
            else
            {
                // Update
                status = groupsMgr.Update(group);
                if (!questStatusDef.IsSuccess(status))
                {
                    FormatErrorMessage(status, groupEditorViewModel);
                    return (status);
                }
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Read(GroupId groupId, out GroupEditorViewModel groupEditorViewModel)
        {
            // Initialize
            questStatus status = null;
            groupEditorViewModel = null;


            // Read
            Quest.Functional.ASM.Group group = null;
            GroupsMgr groupsMgr = new GroupsMgr(this.UserSession);
            status = groupsMgr.Read(groupId, out group);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }

            // Transfer model.
            groupEditorViewModel = new GroupEditorViewModel(this.UserSession);
            BufferMgr.TransferBuffer(group, groupEditorViewModel);



            return (new questStatus(Severity.Success));
        }
        public questStatus Delete(GroupId groupId)
        {
            // Initialize
            questStatus status = null;


            // Delete
            GroupsMgr groupsMgr = new GroupsMgr(this.UserSession);
            status = groupsMgr.Delete(groupId);
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