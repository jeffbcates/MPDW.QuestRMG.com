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
    public class FiltersListModeler : BaseListModeler
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
        public FiltersListModeler(HttpRequestBase filterBase, UserSession userSession)
            : base(filterBase, userSession)
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
        public questStatus List(out FiltersListViewModel filtersListViewModel)
        {
            // Initialize
            questStatus status = null;
            filtersListViewModel = null;


            // Get query options from query string
            QueryOptions queryOptions = null;
            BaseListModeler baseListModeler = new BaseListModeler(this.HttpRequestBase, new UserSession());
            status = baseListModeler.ParsePagingOptions(this.HttpRequestBase, out queryOptions);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }

            // List
            status = List(queryOptions, out filtersListViewModel);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus List(QueryOptions queryOptions, out FiltersListViewModel filtersListViewModel)
        {
            // Initialize
            questStatus status = null;
            filtersListViewModel = null;


            // List
            QueryResponse queryResponse = null;
            List<Quest.Functional.Logging.FilterLog> filterLogList = null;
            FilterLogsMgr filterLogsMgr = new FilterLogsMgr(this.UserSession);
            status = filterLogsMgr.List(queryOptions, out filterLogList, out queryResponse);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }

            // Sort 
            filterLogList.Sort(delegate (Quest.Functional.Logging.FilterLog i1, Quest.Functional.Logging.FilterLog i2) { return i2.Created.CompareTo(i1.Created); });


            // Transfer model.
            // TODO: USE BaseListModeler to xfer queryOptions to QueryOptionsViewModel.
            filtersListViewModel = new FiltersListViewModel(this.UserSession);
            QueryResponseViewModel queryResponseViewModel = null;
            status = TransferQueryResponse(queryResponse, out queryResponseViewModel);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            filtersListViewModel.QueryResponse = queryResponseViewModel;
            foreach (Quest.Functional.Logging.FilterLog filterLog in filterLogList)
            {
                FilterLineItemViewModel filterLineItemViewModel = new FilterLineItemViewModel();
                BufferMgr.TransferBuffer(filterLog, filterLineItemViewModel);
                filtersListViewModel.Items.Add(filterLineItemViewModel);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Page(out FiltersListViewModel filtersListViewModel)
        {
            // Initialize
            questStatus status = null;
            filtersListViewModel = null;


            // Set up query options.
            // TEMPORARY: OPTIMIZE THIS
            List<SearchField> searchFieldList = new List<SearchField>();

            SearchOptions searchOptions = new SearchOptions();
            searchOptions.SearchFieldList = searchFieldList;

            QueryOptions queryOptions = new QueryOptions(100, 1);
            queryOptions.SearchOptions = searchOptions;
            QueryResponse queryResponse = null;


            // List
            List<Quest.Functional.Logging.FilterLog> filterLogList = null;
            FilterLogsMgr filterLogsMgr = new FilterLogsMgr(this.UserSession);
            status = filterLogsMgr.List(queryOptions, out filterLogList, out queryResponse);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }

            // Transfer model.
            filtersListViewModel = new FiltersListViewModel(this.UserSession);
            foreach (Quest.Functional.Logging.FilterLog filterLog in filterLogList)
            {
                FilterLineItemViewModel filterLineItemViewModel = new FilterLineItemViewModel();
                BufferMgr.TransferBuffer(filterLog, filterLineItemViewModel);
                filtersListViewModel.Items.Add(filterLineItemViewModel);
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
        public questStatus Clear(FiltersListViewModel filtersListViewModel)
        {
            // Initialize
            questStatus status = null;


            FilterLogsMgr filterLogsMgr = new FilterLogsMgr(this.UserSession);
            status = filterLogsMgr.Clear();
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
            List<FilterLogId> filterLogIdList = new List<FilterLogId>();
            foreach (BaseId baseId in deleteLogItemsViewModel.Items)
            {
                FilterLogId filterLogId = new FilterLogId(baseId.Id);
                filterLogIdList.Add(filterLogId);
            }

            // Delete items
            FilterLogsMgr filterLogsMgr = new FilterLogsMgr(this.UserSession);
            status = filterLogsMgr.Delete(filterLogIdList);
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