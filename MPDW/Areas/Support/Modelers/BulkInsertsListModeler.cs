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
    public class BulkInsertsListModeler : BaseListModeler
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
        public BulkInsertsListModeler(HttpRequestBase bulkInsertBase, UserSession userSession)
            : base(bulkInsertBase, userSession)
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
        public questStatus List(out BulkInsertsListViewModel bulkInsertsListViewModel)
        {
            // Initialize
            questStatus status = null;
            bulkInsertsListViewModel = null;


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
            status = List(queryOptions, out bulkInsertsListViewModel);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus List(QueryOptions queryOptions, out BulkInsertsListViewModel bulkInsertsListViewModel)
        {
            // Initialize
            questStatus status = null;
            bulkInsertsListViewModel = null;


            // List
            QueryResponse queryResponse = null;
            List<Quest.Functional.Logging.BulkInsertLog> bulkInsertLogList = null;
            BulkInsertLogsMgr bulkInsertLogsMgr = new BulkInsertLogsMgr(this.UserSession);
            status = bulkInsertLogsMgr.List(queryOptions, out bulkInsertLogList, out queryResponse);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }

            // Sort 
            bulkInsertLogList.Sort(delegate (Quest.Functional.Logging.BulkInsertLog i1, Quest.Functional.Logging.BulkInsertLog i2) { return i1.Created.CompareTo(i2.Created); });


            // Transfer model.
            // TODO: USE BaseListModeler to xfer queryOptions to QueryOptionsViewModel.
            bulkInsertsListViewModel = new BulkInsertsListViewModel(this.UserSession);
            QueryResponseViewModel queryResponseViewModel = null;
            status = TransferQueryResponse(queryResponse, out queryResponseViewModel);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            bulkInsertsListViewModel.QueryResponse = queryResponseViewModel;
            foreach (Quest.Functional.Logging.BulkInsertLog user in bulkInsertLogList)
            {
                BulkInsertLineItemViewModel bulkInsertLineItemViewModel = new BulkInsertLineItemViewModel();
                BufferMgr.TransferBuffer(user, bulkInsertLineItemViewModel);
                bulkInsertsListViewModel.Items.Add(bulkInsertLineItemViewModel);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Page(out BulkInsertsListViewModel bulkInsertsListViewModel)
        {
            // Initialize
            questStatus status = null;
            bulkInsertsListViewModel = null;


            // Set up query options.
            // TEMPORARY: OPTIMIZE THIS
            List<SearchField> searchFieldList = new List<SearchField>();

            SearchOptions searchOptions = new SearchOptions();
            searchOptions.SearchFieldList = searchFieldList;

            QueryOptions queryOptions = new QueryOptions(100, 1);
            queryOptions.SearchOptions = searchOptions;
            QueryResponse queryResponse = null;


            // List
            List<Quest.Functional.Logging.BulkInsertLog> bulkInsertLogList = null;
            BulkInsertLogsMgr bulkInsertLogsMgr = new BulkInsertLogsMgr(this.UserSession);
            status = bulkInsertLogsMgr.List(queryOptions, out bulkInsertLogList, out queryResponse);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }

            // Transfer model.
            bulkInsertsListViewModel = new BulkInsertsListViewModel(this.UserSession);
            foreach (Quest.Functional.Logging.BulkInsertLog user in bulkInsertLogList)
            {
                BulkInsertLineItemViewModel bulkInsertLineItemViewModel = new BulkInsertLineItemViewModel();
                BufferMgr.TransferBuffer(user, bulkInsertLineItemViewModel);
                bulkInsertsListViewModel.Items.Add(bulkInsertLineItemViewModel);
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