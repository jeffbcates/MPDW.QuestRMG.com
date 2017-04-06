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
    public class UserPrivilegesModeler : AdminBaseModeler
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
        public UserPrivilegesModeler(HttpRequestBase httpRequestBase, UserSession userSession)
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
        public questStatus Read(UserEditorViewModel viewModel, out UserPrivilegesViewModel userPrivilegesViewModel)
        {
            UserPrivilegesViewModel _userPrivilegesViewModel = new UserPrivilegesViewModel(this.UserSession, viewModel);
            _userPrivilegesViewModel.Id = viewModel.Id;
            return (Read(_userPrivilegesViewModel, out userPrivilegesViewModel));
        }
        public questStatus Read(UserPrivilegesViewModel viewModel, out UserPrivilegesViewModel userPrivilegesViewModel)
        {
            // Initialize
            questStatus status = null;
            userPrivilegesViewModel = null;
            UserId userId = new UserId(viewModel.Id);


            // Get the user
            User user = null;
            UsersMgr usersMgr = new UsersMgr(this.UserSession);
            status = usersMgr.Read(userId, out user);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }


            // Get privileges
            List<SearchField> searchFieldList = new List<SearchField>();
            SearchOptions searchOptions = new SearchOptions();
            searchOptions.SearchFieldList = searchFieldList;
            QueryOptions queryOptions = new QueryOptions();
            queryOptions.SearchOptions = searchOptions;
            QueryResponse queryResponse = null;

            List<Privilege> privilegeList = null;
            PrivilegesMgr privilegesMgr = new PrivilegesMgr(this.UserSession);
            status = privilegesMgr.List(queryOptions, out privilegeList, out queryResponse);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }


            // Get user privileges
            List<Privilege> userPrivilegeList = null;
            AccountMgr accountMgr = new AccountMgr(this.UserSession);
            status = accountMgr.GetUserPrivileges(userId, out userPrivilegeList);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }

            // Remove from privileges lists whatever privileges the user is already in.
            List<Privilege> unassignedPrivilegeList = new List<Privilege>();
            foreach (Privilege privilege in privilegeList)
            {
                Privilege userPrivilege = userPrivilegeList.Find(delegate (Privilege g) { return (g.Id == privilege.Id); });
                if (userPrivilege == null)
                {
                    unassignedPrivilegeList.Add(privilege);
                }
            }

            // Sort privilege lists
            unassignedPrivilegeList.Sort(delegate (Privilege i1, Privilege i2) { return i1.Name.CompareTo(i2.Name); });
            userPrivilegeList.Sort(delegate (Privilege i1, Privilege i2) { return i1.Name.CompareTo(i2.Name); });


            // Transfer model
            userPrivilegesViewModel = new UserPrivilegesViewModel(this.UserSession, viewModel);
            UserEditorViewModel userEditorViewModel = new UserEditorViewModel();
            BufferMgr.TransferBuffer(user, userEditorViewModel);
            userPrivilegesViewModel.User = userEditorViewModel;
            foreach (Privilege privilege in unassignedPrivilegeList)
            {
                BootstrapTreenodeViewModel privilegeNode = null;
                status = FormatBootstrapTreeviewNode(privilege, out privilegeNode);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }
                privilegeNode.icon = "fa fa-lock padding-right-20";
                userPrivilegesViewModel.Privileges.Add(privilegeNode);
            }
            foreach (Privilege privilege in userPrivilegeList)
            {
                BootstrapTreenodeViewModel userPrivilegeNode = null;
                status = FormatBootstrapTreeviewNode(privilege, out userPrivilegeNode);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }
                userPrivilegeNode.icon = "fa fa-unlock padding-right-20";
                userPrivilegesViewModel.UserPrivileges.Add(userPrivilegeNode);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Save(UserPrivilegesViewModel userPrivilegesViewModel)
        {
            // Initialize
            questStatus status = null;
            DbMgrTransaction trans = null;
            Mgr mgr = new Mgr(this.UserSession);

            try
            {
                // BEGIN TRANSACTION
                status = mgr.BeginTransaction("FMS", "SaveUserPrivileges_" + userPrivilegesViewModel.Id.ToString() + "_" + Guid.NewGuid().ToString(), out trans);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }


                // Remove all the user's privileges.
                UserId userId = new UserId(userPrivilegesViewModel.Id);
                UserPrivilegesMgr userPrivilegesMgr = new UserPrivilegesMgr(this.UserSession);
                status = userPrivilegesMgr.Delete(trans, userId);
                if (!questStatusDef.IsSuccess(status))
                {
                    mgr.RollbackTransaction(trans);
                    return (status);
                }

                // Save all the privileges the user is assigned.
                UserPrivilege userPrivilege = new UserPrivilege();
                userPrivilege.User.Id = userPrivilegesViewModel.User.Id;
                foreach (BootstrapTreenodeViewModel userPrivilegeNode in userPrivilegesViewModel.UserPrivileges)
                {
                    UserPrivilegeId userPrivilegeId = null;
                    userPrivilege.Privilege.Id = userPrivilegeNode.Id;
                    status = userPrivilegesMgr.Create(trans, userPrivilege, out userPrivilegeId);
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