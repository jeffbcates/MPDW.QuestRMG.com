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
using Quest.MPDW.Models;
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
        public questStatus Read(UserEditorViewModel userEditorViewModel, out UserGroupsViewModel userGroupsViewModel)
        {
            // Initialize
            questStatus status = null;
            userGroupsViewModel = null;


            // Read the user
            UserId userId = new UserId(userEditorViewModel.Id);
            UserEditorViewModel _userEditorViewModel = null;
            UserEditorModeler userEditorModeler = new UserEditorModeler(this.HttpRequestBase, this.UserSession);
            status = userEditorModeler.Read(userId, out _userEditorViewModel);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }

            // Get groups.
            List<BootstrapTreenodeViewModel> groupNodeList = null;
            GroupsModeler groupsModeler = new GroupsModeler(this.HttpRequestBase, this.UserSession);
            status = groupsModeler.Load(out groupNodeList);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }

            // Get user groups.
            List<BootstrapTreenodeViewModel> userGroupNodeList = null;
            status = Load(userEditorViewModel, out userGroupNodeList);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }

            //
            // Transfer model
            //
            userGroupsViewModel = new UserGroupsViewModel(this.UserSession, userEditorViewModel);
            userGroupsViewModel.User = _userEditorViewModel;
            userGroupsViewModel.UserGroups = userGroupNodeList;

            // Groups - Do not add groups already assigned to user.
            foreach (BootstrapTreenodeViewModel groupNode in groupNodeList)
            {
                BootstrapTreenodeViewModel _userGroup = userGroupNodeList.Find(delegate (BootstrapTreenodeViewModel ug) { return (ug.Id == groupNode.Id); });
                if (_userGroup != null) { continue; }
                userGroupsViewModel.Groups.Add(groupNode);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Load(UserEditorViewModel userEditorViewModel, out List<BootstrapTreenodeViewModel> userGroupsNodeList)
        {
            // Initialize
            questStatus status = null;
            userGroupsNodeList = null;


            // Get user groups
            UserId userId = new UserId(userEditorViewModel.Id);
            List<Group> userGroupList = null;
            AccountMgr accountMgr = new AccountMgr(this.UserSession);
            status = accountMgr.GetUserGroups(userId, out userGroupList);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }

            // Transfer model
            userGroupsNodeList = new List<BootstrapTreenodeViewModel>();
            foreach (Group group in userGroupList)
            {
                BootstrapTreenodeViewModel userGroupNode = null;
                FormatBootstrapTreeviewNode(group, out userGroupNode);
                userGroupsNodeList.Add(userGroupNode);
            }
            return (new questStatus(Severity.Success));
        }
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