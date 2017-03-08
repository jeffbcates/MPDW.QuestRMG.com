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
using Quest.MPDW.Models.List;
using Quest.MPDW.Modelers;
using Quest.MPDW.Admin.Models;
using Quest.MPDW.Services.Business.Accounts;


namespace Quest.MPDW.Admin.Modelers
{
    public class UsersListModeler : BaseListModeler
    {
        #region Declarations
        /*==================================================================================================================================
        * Declarations
        *=================================================================================================================================*/
        #endregion


        #region Constructors
        /*==================================================================================================================================
        * ConstructorsUserListModeler
        *=================================================================================================================================*/
        public UsersListModeler(HttpRequestBase httpRequestBase, UserSession userSession)
            : base(httpRequestBase, userSession)
        {
            initialize();
        }
        #endregion


        #region Public Methods
        /*==================================================================================================================================
        * Public Methods
        *=================================================================================================================================*/

        #region LIST 
        //----------------------------------------------------------------------------------------------------------------------------------
        // LIST
        //----------------------------------------------------------------------------------------------------------------------------------
        public questStatus List(out UsersListViewModel usersListViewModel)
        {
            // Initialize
            questStatus status = null;
            usersListViewModel = null;


            // Get query options from query string
            QueryOptions queryOptions = null;
            BaseListModeler baseListModeler = new BaseListModeler(this.HttpRequestBase, new UserSession());
            status = baseListModeler.ParsePagingOptions(this.HttpRequestBase, out queryOptions);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }

            // Set up query options.
            // TEMPORARY: OPTIMIZE THIS
            List<SearchField> searchFieldList = new List<SearchField>();
            SearchOptions searchOptions = new SearchOptions();
            searchOptions.SearchFieldList = searchFieldList;
            queryOptions.SearchOptions = searchOptions;


            // List
            status = List(queryOptions, out usersListViewModel);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus List(QueryOptions queryOptions, out UsersListViewModel usersListViewModel)
        {
            // Initialize
            questStatus status = null;
            usersListViewModel = null;


            // List
            QueryResponse queryResponse = null;
            List<Quest.Functional.ASM.User> userList = null;
            UsersMgr usersMgr = new UsersMgr(this.UserSession);
            status = usersMgr.List(queryOptions, out userList, out queryResponse);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }

            // Sort 
            userList.Sort(delegate (Quest.Functional.ASM.User i1, Quest.Functional.ASM.User i2) { return i1.LastName.CompareTo(i2.LastName); });


            // Transfer model.
            // TODO: USE BaseListModeler to xfer queryOptions to QueryOptionsViewModel.
            usersListViewModel = new UsersListViewModel(this.UserSession);
            QueryResponseViewModel queryResponseViewModel = null;
            status = TransferQueryResponse(queryResponse, out queryResponseViewModel);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            usersListViewModel.QueryResponse = queryResponseViewModel;
            foreach (Quest.Functional.ASM.User user in userList)
            {
                UserLineItemViewModel userLineItemViewModel = new UserLineItemViewModel();
                BufferMgr.TransferBuffer(user, userLineItemViewModel);
                usersListViewModel.Items.Add(userLineItemViewModel);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Page(out UsersListViewModel usersListViewModel)
        {
            // Initialize
            questStatus status = null;
            usersListViewModel = null;


            // Set up query options.
            // TEMPORARY: OPTIMIZE THIS
            List<SearchField> searchFieldList = new List<SearchField>();

            SearchOptions searchOptions = new SearchOptions();
            searchOptions.SearchFieldList = searchFieldList;

            QueryOptions queryOptions = new QueryOptions(100, 1);
            queryOptions.SearchOptions = searchOptions;
            QueryResponse queryResponse = null;


            // List
            List<Quest.Functional.ASM.User> userList = null;
            UsersMgr usersMgr = new UsersMgr(this.UserSession);
            status = usersMgr.List(queryOptions, out userList, out queryResponse);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }

            // Transfer model.
            usersListViewModel = new UsersListViewModel(this.UserSession);
            foreach (Quest.Functional.ASM.User user in userList)
            {
                UserLineItemViewModel userLineItemViewModel = new UserLineItemViewModel();
                BufferMgr.TransferBuffer(user, userLineItemViewModel);
                usersListViewModel.Items.Add(userLineItemViewModel);
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