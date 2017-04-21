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
using Quest.Functional.Logging;
using Quest.MPDW.Models.List;
using Quest.MPDW.Modelers;
using Quest.MPDW.Support.Models;
using Quest.Services.Business.Logging;


namespace Quest.MPDW.Support.Modelers
{
    public class BulkUpdatesListModeler : BaseListModeler
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
        public BulkUpdatesListModeler(HttpRequestBase bulkUpdateBase, UserSession userSession)
            : base(bulkUpdateBase, userSession)
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
        public questStatus List(out BulkUpdatesListViewModel bulkUpdatesListViewModel)
        {
            // Initialize
            questStatus status = null;
            bulkUpdatesListViewModel = null;


            // Get query options from query string
            QueryOptions queryOptions = null;
            BaseListModeler baseListModeler = new BaseListModeler(this.HttpRequestBase, new UserSession());
            status = baseListModeler.ParsePagingOptions(this.HttpRequestBase, out queryOptions);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }

            // List
            status = List(queryOptions, out bulkUpdatesListViewModel);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus List(QueryOptions queryOptions, out BulkUpdatesListViewModel bulkUpdatesListViewModel)
        {
            // Initialize
            questStatus status = null;
            bulkUpdatesListViewModel = null;


            // List
            QueryResponse queryResponse = null;
            List<Quest.Functional.Logging.BulkUpdateLog> bulkUpdateLogList = null;
            BulkUpdateLogsMgr bulkUpdateLogsMgr = new BulkUpdateLogsMgr(this.UserSession);
            status = bulkUpdateLogsMgr.List(queryOptions, out bulkUpdateLogList, out queryResponse);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }

            // Sort 
            bulkUpdateLogList.Sort(delegate (Quest.Functional.Logging.BulkUpdateLog i1, Quest.Functional.Logging.BulkUpdateLog i2) { return i2.Created.CompareTo(i1.Created); });


            // Transfer model.
            // TODO: USE BaseListModeler to xfer queryOptions to QueryOptionsViewModel.
            bulkUpdatesListViewModel = new BulkUpdatesListViewModel(this.UserSession);
            QueryResponseViewModel queryResponseViewModel = null;
            status = TransferQueryResponse(queryResponse, out queryResponseViewModel);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            bulkUpdatesListViewModel.QueryResponse = queryResponseViewModel;
            foreach (Quest.Functional.Logging.BulkUpdateLog bulkUpdateLog in bulkUpdateLogList)
            {
                BulkUpdateLineItemViewModel bulkUpdateLineItemViewModel = new BulkUpdateLineItemViewModel();
                BufferMgr.TransferBuffer(bulkUpdateLog, bulkUpdateLineItemViewModel);
                bulkUpdatesListViewModel.Items.Add(bulkUpdateLineItemViewModel);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Page(out BulkUpdatesListViewModel bulkUpdatesListViewModel)
        {
            // Initialize
            questStatus status = null;
            bulkUpdatesListViewModel = null;


            // Set up query options.
            // TEMPORARY: OPTIMIZE THIS
            List<SearchField> searchFieldList = new List<SearchField>();

            SearchOptions searchOptions = new SearchOptions();
            searchOptions.SearchFieldList = searchFieldList;

            QueryOptions queryOptions = new QueryOptions(100, 1);
            queryOptions.SearchOptions = searchOptions;
            QueryResponse queryResponse = null;


            // List
            List<Quest.Functional.Logging.BulkUpdateLog> bulkUpdateLogList = null;
            BulkUpdateLogsMgr bulkUpdateLogsMgr = new BulkUpdateLogsMgr(this.UserSession);
            status = bulkUpdateLogsMgr.List(queryOptions, out bulkUpdateLogList, out queryResponse);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }

            // Transfer model.
            bulkUpdatesListViewModel = new BulkUpdatesListViewModel(this.UserSession);
            foreach (Quest.Functional.Logging.BulkUpdateLog bulkUpdateLog in bulkUpdateLogList)
            {
                BulkUpdateLineItemViewModel bulkUpdateLineItemViewModel = new BulkUpdateLineItemViewModel();
                BufferMgr.TransferBuffer(bulkUpdateLog, bulkUpdateLineItemViewModel);
                bulkUpdatesListViewModel.Items.Add(bulkUpdateLineItemViewModel);
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


        #region Commands
        //----------------------------------------------------------------------------------------------------------------------------------
        // Commands
        //----------------------------------------------------------------------------------------------------------------------------------
        public questStatus Clear(BulkUpdatesListViewModel bulkUpdatesListViewModel)
        {
            // Initialize
            questStatus status = null;


            BulkUpdateLogsMgr bulkUpdateLogsMgr = new BulkUpdateLogsMgr(this.UserSession);
            status = bulkUpdateLogsMgr.Clear();
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Delete(DeleteLogItemsViewModel deleteLogItemsViewModel)
        {
            // Initialize
            questStatus status = null;


            // Build id list
            List<BulkUpdateLogId> bulkUpdateLogIdList = new List<BulkUpdateLogId>();
            foreach (BaseId baseId in deleteLogItemsViewModel.Items)
            {
                BulkUpdateLogId bulkUpdateLogId = new BulkUpdateLogId(baseId.Id);
                bulkUpdateLogIdList.Add(bulkUpdateLogId);
            }

            // Delete items
            BulkUpdateLogsMgr bulkUpdateLogsMgr = new BulkUpdateLogsMgr(this.UserSession);
            status = bulkUpdateLogsMgr.Delete(bulkUpdateLogIdList);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
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