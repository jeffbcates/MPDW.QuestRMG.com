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
    public class ExceptionsListModeler : BaseListModeler
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
        public ExceptionsListModeler(HttpRequestBase exceptionBase, UserSession userSession)
            : base(exceptionBase, userSession)
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
        public questStatus List(out ExceptionsListViewModel exceptionsListViewModel)
        {
            // Initialize
            questStatus status = null;
            exceptionsListViewModel = null;


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
            status = List(queryOptions, out exceptionsListViewModel);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus List(QueryOptions queryOptions, out ExceptionsListViewModel exceptionsListViewModel)
        {
            // Initialize
            questStatus status = null;
            exceptionsListViewModel = null;


            // List
            QueryResponse queryResponse = null;
            List<Quest.Functional.Logging.ExceptionLog> exceptionLogList = null;
            ExceptionLogsMgr exceptionLogsMgr = new ExceptionLogsMgr(this.UserSession);
            status = exceptionLogsMgr.List(queryOptions, out exceptionLogList, out queryResponse);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }

            // Sort 
            exceptionLogList.Sort(delegate (Quest.Functional.Logging.ExceptionLog i1, Quest.Functional.Logging.ExceptionLog i2) { return i2.Created.CompareTo(i1.Created); });


            // Transfer model.
            // TODO: USE BaseListModeler to xfer queryOptions to QueryOptionsViewModel.
            exceptionsListViewModel = new ExceptionsListViewModel(this.UserSession);
            QueryResponseViewModel queryResponseViewModel = null;
            status = TransferQueryResponse(queryResponse, out queryResponseViewModel);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            exceptionsListViewModel.QueryResponse = queryResponseViewModel;
            foreach (Quest.Functional.Logging.ExceptionLog exceptionLog in exceptionLogList)
            {
                ExceptionLineItemViewModel exceptionLineItemViewModel = new ExceptionLineItemViewModel();
                BufferMgr.TransferBuffer(exceptionLog, exceptionLineItemViewModel);
                exceptionsListViewModel.Items.Add(exceptionLineItemViewModel);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Page(out ExceptionsListViewModel exceptionsListViewModel)
        {
            // Initialize
            questStatus status = null;
            exceptionsListViewModel = null;


            // Set up query options.
            // TEMPORARY: OPTIMIZE THIS
            List<SearchField> searchFieldList = new List<SearchField>();

            SearchOptions searchOptions = new SearchOptions();
            searchOptions.SearchFieldList = searchFieldList;

            QueryOptions queryOptions = new QueryOptions(100, 1);
            queryOptions.SearchOptions = searchOptions;
            QueryResponse queryResponse = null;


            // List
            List<Quest.Functional.Logging.ExceptionLog> exceptionLogList = null;
            ExceptionLogsMgr exceptionLogsMgr = new ExceptionLogsMgr(this.UserSession);
            status = exceptionLogsMgr.List(queryOptions, out exceptionLogList, out queryResponse);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }

            // Transfer model.
            exceptionsListViewModel = new ExceptionsListViewModel(this.UserSession);
            foreach (Quest.Functional.Logging.ExceptionLog exceptionLog in exceptionLogList)
            {
                ExceptionLineItemViewModel exceptionLineItemViewModel = new ExceptionLineItemViewModel();
                BufferMgr.TransferBuffer(exceptionLog, exceptionLineItemViewModel);
                exceptionsListViewModel.Items.Add(exceptionLineItemViewModel);
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