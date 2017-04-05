using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Web;
using Quest.Util.Status;
using Quest.Util.Buffer;
using Quest.Util.Data;
using Quest.Functional.ASM;
using Quest.Functional.FMS;
using Quest.MPDW.Models;
using Quest.MPDW.Admin.Models;
using Quest.MPDW.Services.Data;
using Quest.MPDW.Services.Business;
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
        public questStatus Read(UserEditorViewModel viewModel, out UserGroupsViewModel userGroupsViewModel)
        {
            UserGroupsViewModel _userGroupsViewModel = new UserGroupsViewModel(this.UserSession, viewModel);
            _userGroupsViewModel.User.Id = viewModel.Id;
            return (Read(_userGroupsViewModel, out userGroupsViewModel));
        }
        public questStatus Read(UserGroupsViewModel viewModel, out UserGroupsViewModel userGroupsViewModel)
        {
            // Initialize
            questStatus status = null;
            userGroupsViewModel = null;
            UserId userId = new UserId(viewModel.User.Id);


            // Get the user
            User user = null;
            UsersMgr usersMgr = new UsersMgr(this.UserSession);
            status = usersMgr.Read(userId, out user);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }


            // Get groups
            List<SearchField> searchFieldList = new List<SearchField>();
            SearchOptions searchOptions = new SearchOptions();
            searchOptions.SearchFieldList = searchFieldList;
            QueryOptions queryOptions = new QueryOptions();
            queryOptions.SearchOptions = searchOptions;
            QueryResponse queryResponse = null;

            List<Group> groupList = null;
            GroupsMgr groupsMgr = new GroupsMgr(this.UserSession);
            status = groupsMgr.List(queryOptions, out groupList, out queryResponse);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }


            // Get user groups
            List<Group> userGroupList = null;
            AccountMgr accountMgr = new AccountMgr(this.UserSession);
            status = accountMgr.GetUserGroups(userId, out userGroupList);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }

            // Remove from groups lists whatever groups the user is already in.
            List<Group> unassignedGroupList = new List<Group>();
            foreach (Group group in groupList)
            {
                Group userGroup = userGroupList.Find(delegate (Group g) { return (g.Id == group.Id); });
                if (userGroup == null)
                {
                    unassignedGroupList.Add(group);
                }
            }

            // Transfer model
            userGroupsViewModel = new UserGroupsViewModel(this.UserSession, viewModel);
            UserEditorViewModel userEditorViewModel = new UserEditorViewModel();
            BufferMgr.TransferBuffer(user, userEditorViewModel);
            userGroupsViewModel.User = userEditorViewModel;
            foreach (Group group in unassignedGroupList)
            {
                BootstrapTreenodeViewModel groupNode = null;
                status = FormatBootstrapTreeviewNode(group, out groupNode);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }
                userGroupsViewModel.Groups.Add(groupNode);
            }
            foreach (Group group in userGroupList)
            {
                BootstrapTreenodeViewModel userGroupNode = null;
                status = FormatBootstrapTreeviewNode(group, out userGroupNode);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }
                userGroupsViewModel.UserGroups.Add(userGroupNode);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Save(UserGroupsViewModel userGroupsViewModel)
        {
            // Initialize
            questStatus status = null;
            DbMgrTransaction trans = null;
            Mgr mgr = new Mgr(this.UserSession);

            try
            {
                // BEGIN TRANSACTION
                status = mgr.BeginTransaction("SaveUserGroups_" + userGroupsViewModel.Id.ToString() + "_" + Guid.NewGuid().ToString(), out trans);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }


                // Remove all the user's groups.
                UserId userId = new UserId(userGroupsViewModel.User.Id);
                GroupUsersMgr groupUsersMgr = new GroupUsersMgr(this.UserSession);
                status = groupUsersMgr.Delete(trans, userId);
                if (!questStatusDef.IsSuccess(status))
                {
                    mgr.RollbackTransaction(trans);
                    return (status);
                }

                // Save all the groups the user is assigned.
                GroupUser groupUser = new GroupUser();
                groupUser.User.Id = userGroupsViewModel.User.Id;
                foreach (BootstrapTreenodeViewModel userGroupNode in userGroupsViewModel.UserGroups)
                {
                    GroupUserId groupUserId = null;
                    groupUser.Group.Id = userGroupNode.Id;
                    status = groupUsersMgr.Create(trans, groupUser, out groupUserId);
                    if (!questStatusDef.IsSuccess(status))
                    {
                        mgr.RollbackTransaction(trans);
                        return (status);
                    }
                }

                // COMMIT TRANSACTION
                status = mgr.CommitTransaction(trans);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }
            }
            catch (System.Exception ex)
            {
                if (trans != null)
                {
                    mgr.RollbackTransaction(trans);
                }
                return (new questStatus(Severity.Fatal, String.Format("EXCEPTION: {0}.{1}: {2}",
                        this.GetType().Name, MethodBase.GetCurrentMethod().Name,
                        ex.InnerException != null ? ex.InnerException.Message : ex.Message)));
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