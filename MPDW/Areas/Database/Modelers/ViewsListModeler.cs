using System;
using System.Collections.Generic;
using System.Web;
using Quest.Util.Status;
using Quest.Util.Buffer;
using Quest.Util.Data;
using Quest.Functional.ASM;
using Quest.MPDW.Models.List;
using Quest.MPDW.Modelers;
using Quest.MasterPricing.Database.Models;
using Quest.Functional.MasterPricing;
using Quest.MasterPricing.Services.Business.Database;


namespace Quest.MasterPricing.Database.Modelers
{
    public class ViewsListModeler : BaseListModeler
    {
        #region Declarations
        /*==================================================================================================================================
         * Declarations
         *=================================================================================================================================*/
        private DatabaseBaseViewModel _dataMgrBaseViewModel = null;

        #endregion


        #region Constructors
        /*==================================================================================================================================
        * ConstructorsRequisitionListModeler
        *=================================================================================================================================*/
        public ViewsListModeler(HttpRequestBase httpRequestBase, UserSession userSession)
            : base(httpRequestBase, userSession)
        {
            initialize();
        }
        public ViewsListModeler(HttpRequestBase httpRequestBase, UserSession userSession, DatabaseBaseViewModel dataMgrBaseViewModel)
            : base(httpRequestBase, userSession)
        {
            this._dataMgrBaseViewModel = dataMgrBaseViewModel;
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
        public questStatus List(DatabaseId databaseId, out ViewsListViewModel viewsListViewModel)
        {
            // Initialize
            questStatus status = null;
            viewsListViewModel = null;


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
            SearchField searchField = new SearchField();
            searchField.Name = "DatabaseId";
            searchField.SearchOperation = SearchOperation.Equal;
            searchField.Type = typeof(int);
            searchField.Value = databaseId.Id.ToString();
            searchFieldList.Add(searchField);

            SearchOptions searchOptions = new SearchOptions();
            searchOptions.SearchFieldList = searchFieldList;
            queryOptions.SearchOptions = searchOptions;
            QueryResponse queryResponse = null;


            // Get Database viewsets
            List<View> viewsetList = null;
            ViewsMgr viewsMgr = new ViewsMgr(this.UserSession);
            status = viewsMgr.List(queryOptions, out viewsetList, out queryResponse);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }


            // Sort by name
            viewsetList.Sort(delegate (View i1, View i2) { return i1.Name.CompareTo(i2.Name); });


            // Transfer model.
            // TODO: USE BaseListModeler to xfer queryOptions to QueryOptionsViewModel.
            viewsListViewModel = new ViewsListViewModel(this.UserSession);
            QueryResponseViewModel queryResponseViewModel = null;
            status = TransferQueryResponse(queryResponse, out queryResponseViewModel);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            viewsListViewModel.QueryResponse = queryResponseViewModel;
            foreach (Quest.Functional.MasterPricing.View view in viewsetList)
            {
                ViewLineItemViewModel viewLineItemViewModel = new ViewLineItemViewModel();
                BufferMgr.TransferBuffer(view, viewLineItemViewModel);
                viewsListViewModel.Items.Add(viewLineItemViewModel);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Page(out ViewsListViewModel viewsListViewModel)
        {
            // Initialize
            questStatus status = null;
            viewsListViewModel = null;


            // Set up query options.
            // TEMPORARY: OPTIMIZE THIS
            List<SearchField> searchFieldList = new List<SearchField>();

            SearchOptions searchOptions = new SearchOptions();
            searchOptions.SearchFieldList = searchFieldList;

            QueryOptions queryOptions = new QueryOptions(100, 1);
            queryOptions.SearchOptions = searchOptions;
            QueryResponse queryResponse = null;


            // List
            List<Quest.Functional.MasterPricing.Database> databaseList = null;
            DatabasesMgr databasesMgr = new DatabasesMgr(this.UserSession);
            status = databasesMgr.List(queryOptions, out databaseList, out queryResponse);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }

            // Transfer model.
            viewsListViewModel = new ViewsListViewModel(this.UserSession);
            foreach (Quest.Functional.MasterPricing.Database database in databaseList)
            {
                ViewLineItemViewModel viewLineItemViewModel = new ViewLineItemViewModel();
                BufferMgr.TransferBuffer(database, viewLineItemViewModel);
                viewsListViewModel.Items.Add(viewLineItemViewModel);
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