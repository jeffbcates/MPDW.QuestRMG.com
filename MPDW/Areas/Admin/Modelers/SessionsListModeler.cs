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
    public class SessionsListModeler : BaseListModeler
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
        public SessionsListModeler(HttpRequestBase httpRequestBase, UserSession userSession)
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
        public questStatus List(out SessionsListViewModel sessionsListViewModel)
        {
            // Initialize
            questStatus status = null;
            sessionsListViewModel = null;


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
            status = List(queryOptions, out sessionsListViewModel);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus List(QueryOptions queryOptions, out SessionsListViewModel sessionsListViewModel)
        {
            // Initialize
            questStatus status = null;
            sessionsListViewModel = null;


            // List
            QueryResponse queryResponse = null;
            List<Quest.Functional.ASM.UserSession> userSessionList = null;
            UserSessionMgr userSessionMgr = new UserSessionMgr(this.UserSession);
            status = userSessionMgr.List(queryOptions, out userSessionList, out queryResponse);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }

            // Sort 
            userSessionList.Sort(delegate (Quest.Functional.ASM.UserSession i1, Quest.Functional.ASM.UserSession i2) { return i1.Created.CompareTo(i2.Created); });


            // Transfer model.
            // TODO: USE BaseListModeler to xfer queryOptions to QueryOptionsViewModel.
            sessionsListViewModel = new SessionsListViewModel(this.UserSession);
            QueryResponseViewModel queryResponseViewModel = null;
            status = TransferQueryResponse(queryResponse, out queryResponseViewModel);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            sessionsListViewModel.QueryResponse = queryResponseViewModel;
            foreach (Quest.Functional.ASM.UserSession user in userSessionList)
            {
                SessionLineItemViewModel sessionLineItemViewModel = new SessionLineItemViewModel();
                BufferMgr.TransferBuffer(user, sessionLineItemViewModel);
                sessionsListViewModel.Items.Add(sessionLineItemViewModel);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Page(out SessionsListViewModel sessionsListViewModel)
        {
            // Initialize
            questStatus status = null;
            sessionsListViewModel = null;


            // Set up query options.
            // TEMPORARY: OPTIMIZE THIS
            List<SearchField> searchFieldList = new List<SearchField>();

            SearchOptions searchOptions = new SearchOptions();
            searchOptions.SearchFieldList = searchFieldList;

            QueryOptions queryOptions = new QueryOptions(100, 1);
            queryOptions.SearchOptions = searchOptions;
            QueryResponse queryResponse = null;


            // List
            List<Quest.Functional.ASM.UserSession> userSessionList = null;
            UserSessionMgr userSessionMgr = new UserSessionMgr(this.UserSession);
            status = userSessionMgr.List(queryOptions, out userSessionList, out queryResponse);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }

            // Transfer model.
            sessionsListViewModel = new SessionsListViewModel(this.UserSession);
            foreach (Quest.Functional.ASM.UserSession user in userSessionList)
            {
                SessionLineItemViewModel sessionLineItemViewModel = new SessionLineItemViewModel();
                BufferMgr.TransferBuffer(user, sessionLineItemViewModel);
                sessionsListViewModel.Items.Add(sessionLineItemViewModel);
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