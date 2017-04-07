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
    public class PortalRequestsListModeler : BaseListModeler
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
        public PortalRequestsListModeler(HttpRequestBase portalRequestBase, UserSession userSession)
            : base(portalRequestBase, userSession)
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
        public questStatus List(out PortalRequestsListViewModel portalRequestsListViewModel)
        {
            // Initialize
            questStatus status = null;
            portalRequestsListViewModel = null;


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
            status = List(queryOptions, out portalRequestsListViewModel);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus List(QueryOptions queryOptions, out PortalRequestsListViewModel portalRequestsListViewModel)
        {
            // Initialize
            questStatus status = null;
            portalRequestsListViewModel = null;


            // List
            QueryResponse queryResponse = null;
            List<Quest.Functional.Logging.PortalRequestLog> portalRequestLogList = null;
            PortalRequestLogsMgr portalRequestLogsMgr = new PortalRequestLogsMgr(this.UserSession);
            status = portalRequestLogsMgr.List(queryOptions, out portalRequestLogList, out queryResponse);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }

            // Sort 
            portalRequestLogList.Sort(delegate (Quest.Functional.Logging.PortalRequestLog i1, Quest.Functional.Logging.PortalRequestLog i2) { return i1.Created.CompareTo(i2.Created); });


            // Transfer model.
            // TODO: USE BaseListModeler to xfer queryOptions to QueryOptionsViewModel.
            portalRequestsListViewModel = new PortalRequestsListViewModel(this.UserSession);
            QueryResponseViewModel queryResponseViewModel = null;
            status = TransferQueryResponse(queryResponse, out queryResponseViewModel);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            portalRequestsListViewModel.QueryResponse = queryResponseViewModel;
            foreach (Quest.Functional.Logging.PortalRequestLog user in portalRequestLogList)
            {
                PortalRequestLineItemViewModel portalRequestLineItemViewModel = new PortalRequestLineItemViewModel();
                BufferMgr.TransferBuffer(user, portalRequestLineItemViewModel);
                portalRequestsListViewModel.Items.Add(portalRequestLineItemViewModel);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Page(out PortalRequestsListViewModel portalRequestsListViewModel)
        {
            // Initialize
            questStatus status = null;
            portalRequestsListViewModel = null;


            // Set up query options.
            // TEMPORARY: OPTIMIZE THIS
            List<SearchField> searchFieldList = new List<SearchField>();

            SearchOptions searchOptions = new SearchOptions();
            searchOptions.SearchFieldList = searchFieldList;

            QueryOptions queryOptions = new QueryOptions(100, 1);
            queryOptions.SearchOptions = searchOptions;
            QueryResponse queryResponse = null;


            // List
            List<Quest.Functional.Logging.PortalRequestLog> portalRequestLogList = null;
            PortalRequestLogsMgr portalRequestLogsMgr = new PortalRequestLogsMgr(this.UserSession);
            status = portalRequestLogsMgr.List(queryOptions, out portalRequestLogList, out queryResponse);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }

            // Transfer model.
            portalRequestsListViewModel = new PortalRequestsListViewModel(this.UserSession);
            foreach (Quest.Functional.Logging.PortalRequestLog user in portalRequestLogList)
            {
                PortalRequestLineItemViewModel portalRequestLineItemViewModel = new PortalRequestLineItemViewModel();
                BufferMgr.TransferBuffer(user, portalRequestLineItemViewModel);
                portalRequestsListViewModel.Items.Add(portalRequestLineItemViewModel);
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