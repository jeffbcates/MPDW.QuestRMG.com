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
    public class TablesListModeler : BaseListModeler
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
        public TablesListModeler(HttpRequestBase httpRequestBase, UserSession userSession)
            : base(httpRequestBase, userSession)
        {
            initialize();
        }
        public TablesListModeler(HttpRequestBase httpRequestBase, UserSession userSession, DatabaseBaseViewModel dataMgrBaseViewModel)
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
        public questStatus List(DatabaseId databaseId, out TablesListViewModel tablesListViewModel)
        {
            // Initialize
            questStatus status = null;
            tablesListViewModel = null;


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


            // Get Database tablesets
            List<Table> tablesetList = null;
            TablesMgr tablesMgr = new TablesMgr(this.UserSession);
            status = tablesMgr.List(queryOptions, out tablesetList, out queryResponse);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }


            // Sort by name
            tablesetList.Sort(delegate (Table i1, Table i2) { return i1.Name.CompareTo(i2.Name); });


            // Transfer model.
            // TODO: USE BaseListModeler to xfer queryOptions to QueryOptionsViewModel.
            tablesListViewModel = new TablesListViewModel(this.UserSession);
            QueryResponseViewModel queryResponseViewModel = null;
            status = TransferQueryResponse(queryResponse, out queryResponseViewModel);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            tablesListViewModel.QueryResponse = queryResponseViewModel;
            foreach (Quest.Functional.MasterPricing.Table table in tablesetList)
            {
                TableLineItemViewModel tableLineItemViewModel = new TableLineItemViewModel();
                BufferMgr.TransferBuffer(table, tableLineItemViewModel);
                tablesListViewModel.Items.Add(tableLineItemViewModel);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Page(out TablesListViewModel tablesListViewModel)
        {
            // Initialize
            questStatus status = null;
            tablesListViewModel = null;


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
            tablesListViewModel = new TablesListViewModel(this.UserSession);
            foreach (Quest.Functional.MasterPricing.Database database in databaseList)
            {
                TableLineItemViewModel tableLineItemViewModel = new TableLineItemViewModel();
                BufferMgr.TransferBuffer(database, tableLineItemViewModel);
                tablesListViewModel.Items.Add(tableLineItemViewModel);
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