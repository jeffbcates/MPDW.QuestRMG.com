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
using Quest.MPDW.Support.Models;
using Quest.Services.Business.Logging;


namespace Quest.MPDW.Support.Modelers
{
    public class HTTPRequestsListModeler : BaseListModeler
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
        public HTTPRequestsListModeler(HttpRequestBase httpRequestBase, UserSession userSession)
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
        public questStatus List(out HTTPRequestsListViewModel httpRequestsListViewModel)
        {
            // Initialize
            questStatus status = null;
            httpRequestsListViewModel = null;


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
            status = List(queryOptions, out httpRequestsListViewModel);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus List(QueryOptions queryOptions, out HTTPRequestsListViewModel httpRequestsListViewModel)
        {
            // Initialize
            questStatus status = null;
            httpRequestsListViewModel = null;


            // List
            QueryResponse queryResponse = null;
            List<Quest.Functional.Logging.HTTPRequestLog> httpRequestLogList = null;
            HTTPRequestLogsMgr httpRequestLogsMgr = new HTTPRequestLogsMgr(this.UserSession);
            status = httpRequestLogsMgr.List(queryOptions, out httpRequestLogList, out queryResponse);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }

            // Sort 
            httpRequestLogList.Sort(delegate (Quest.Functional.Logging.HTTPRequestLog i1, Quest.Functional.Logging.HTTPRequestLog i2) { return i1.Created.CompareTo(i2.Created); });


            // Transfer model.
            // TODO: USE BaseListModeler to xfer queryOptions to QueryOptionsViewModel.
            httpRequestsListViewModel = new HTTPRequestsListViewModel(this.UserSession);
            QueryResponseViewModel queryResponseViewModel = null;
            status = TransferQueryResponse(queryResponse, out queryResponseViewModel);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            httpRequestsListViewModel.QueryResponse = queryResponseViewModel;
            foreach (Quest.Functional.Logging.HTTPRequestLog user in httpRequestLogList)
            {
                HTTPRequestLineItemViewModel httpRequestLineItemViewModel = new HTTPRequestLineItemViewModel();
                BufferMgr.TransferBuffer(user, httpRequestLineItemViewModel);
                httpRequestsListViewModel.Items.Add(httpRequestLineItemViewModel);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Page(out HTTPRequestsListViewModel httpRequestsListViewModel)
        {
            // Initialize
            questStatus status = null;
            httpRequestsListViewModel = null;


            // Set up query options.
            // TEMPORARY: OPTIMIZE THIS
            List<SearchField> searchFieldList = new List<SearchField>();

            SearchOptions searchOptions = new SearchOptions();
            searchOptions.SearchFieldList = searchFieldList;

            QueryOptions queryOptions = new QueryOptions(100, 1);
            queryOptions.SearchOptions = searchOptions;
            QueryResponse queryResponse = null;


            // List
            List<Quest.Functional.Logging.HTTPRequestLog> httpRequestLogList = null;
            HTTPRequestLogsMgr httpRequestLogsMgr = new HTTPRequestLogsMgr(this.UserSession);
            status = httpRequestLogsMgr.List(queryOptions, out httpRequestLogList, out queryResponse);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }

            // Transfer model.
            httpRequestsListViewModel = new HTTPRequestsListViewModel(this.UserSession);
            foreach (Quest.Functional.Logging.HTTPRequestLog user in httpRequestLogList)
            {
                HTTPRequestLineItemViewModel httpRequestLineItemViewModel = new HTTPRequestLineItemViewModel();
                BufferMgr.TransferBuffer(user, httpRequestLineItemViewModel);
                httpRequestsListViewModel.Items.Add(httpRequestLineItemViewModel);
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