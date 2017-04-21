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
    public class UserEditorModeler : AdminBaseModeler
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
        public UserEditorModeler(HttpRequestBase httpRequestBase, UserSession userSession)
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
        public questStatus Read(UserId userId, out UserEditorViewModel userEditorViewModel)
        {
            // Initialize
            questStatus status = null;
            userEditorViewModel = null;


            // Read
            Quest.Functional.ASM.User user = null;
            UsersMgr usersMgr = new UsersMgr(this.UserSession);
            status = usersMgr.Read(userId, out user);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }

            // Transfer model.
            userEditorViewModel = new UserEditorViewModel(this.UserSession);
            BufferMgr.TransferBuffer(user, userEditorViewModel);



            return (new questStatus(Severity.Success));
        }
        public questStatus Delete(UserId userId)
        {
            // Initialize
            questStatus status = null;


            // Delete
            UsersMgr usersMgr = new UsersMgr(this.UserSession);
            status = usersMgr.Delete(userId);
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
        public questStatus GetUserOptions(out List<OptionValuePair> optionsList, string Value = null, string Name = null)
        {
            // Initialize
            questStatus status = null;
            optionsList = null;


            // TEMPORARY: OPTIMIZE THIS
            List<SearchField> searchFieldList = new List<SearchField>();
            SearchOptions searchOptions = new SearchOptions();
            searchOptions.SearchFieldList = searchFieldList;
            QueryOptions queryOptions = new QueryOptions();
            queryOptions.SearchOptions = searchOptions;
            QueryResponse queryResponse = null;


            // Get users for given tableset
            List<User> userList = null;
            UsersMgr usersMgr = new UsersMgr(this.UserSession);
            status = usersMgr.List(queryOptions, out userList, out queryResponse);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }

            // Sort 
            userList.Sort(delegate (User i1, User i2) { return i1.LastName.CompareTo(i2.LastName); });


            // Build options
            // Set selected if specified.
            optionsList = new List<OptionValuePair>();
            foreach (User user in userList)
            {
                OptionValuePair optionValuePair = new OptionValuePair();
                optionValuePair.Id = user.Id.ToString();
                optionValuePair.Label = user.LastName + ", " + user.FirstName;
                if (Value != null && Value == user.Id.ToString())
                {
                    optionValuePair.bSelected = true;
                }
                else if (Name != null && Name == user.Username)
                {
                    optionValuePair.bSelected = true;
                }
                optionsList.Add(optionValuePair);
            }

            // Insert default option
            status = AddDefaultOptions(optionsList, "-1", "Select one ...");
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
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