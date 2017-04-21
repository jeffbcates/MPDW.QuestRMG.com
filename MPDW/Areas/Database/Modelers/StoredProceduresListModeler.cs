using System;
using System.Collections.Generic;
using System.Web;
using Quest.Util.Status;
using Quest.Util.Buffer;
using Quest.Util.Data;
using Quest.Functional.ASM;
using Quest.Functional.FMS;
using Quest.MPDW.Models.List;
using Quest.MPDW.Modelers;
using Quest.MasterPricing.Database.Models;
using Quest.Functional.MasterPricing;
using Quest.MasterPricing.Services.Business.Database;


namespace Quest.MasterPricing.Database.Modelers
{
    public class StoredProceduresListModeler : BaseListModeler
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
        public StoredProceduresListModeler(HttpRequestBase httpRequestBase, UserSession userSession)
            : base(httpRequestBase, userSession)
        {
            initialize();
        }
        public StoredProceduresListModeler(HttpRequestBase httpRequestBase, UserSession userSession, DatabaseBaseViewModel databaseBaseViewModel)
            : base(httpRequestBase, userSession)
        {
            this._dataMgrBaseViewModel = databaseBaseViewModel;
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
        public questStatus List(DatabaseId databaseId, out StoredProceduresListViewModel storedProceduresListViewModel)
        {
            // Initialize
            questStatus status = null;
            storedProceduresListViewModel = null;


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


            // Get Database storedProcedures
            List<StoredProcedure> storedProceduresetList = null;
            StoredProceduresMgr storedProceduresMgr = new StoredProceduresMgr(this.UserSession);
            status = storedProceduresMgr.List(queryOptions, out storedProceduresetList, out queryResponse);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }


            // Sort by name
            storedProceduresetList.Sort(delegate (StoredProcedure i1, StoredProcedure i2) { return i1.Name.CompareTo(i2.Name); });


            // Transfer model.
            // TODO: USE BaseListModeler to xfer queryOptions to QueryOptionsStoredProcedureModel.
            storedProceduresListViewModel = new StoredProceduresListViewModel(this.UserSession);
            QueryResponseViewModel queryResponseViewModel = null;
            status = TransferQueryResponse(queryResponse, out queryResponseViewModel);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            storedProceduresListViewModel.QueryResponse = queryResponseViewModel;
            foreach (Quest.Functional.MasterPricing.StoredProcedure storedProcedure in storedProceduresetList)
            {
                StoredProcedureLineItemViewModel storedProcedureLineItemViewModel = new StoredProcedureLineItemViewModel();
                BufferMgr.TransferBuffer(storedProcedure, storedProcedureLineItemViewModel);
                storedProceduresListViewModel.Items.Add(storedProcedureLineItemViewModel);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Page(out StoredProceduresListViewModel storedProceduresListViewModel)
        {
            // Initialize
            questStatus status = null;
            storedProceduresListViewModel = null;


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
            storedProceduresListViewModel = new StoredProceduresListViewModel(this.UserSession);
            foreach (Quest.Functional.MasterPricing.Database database in databaseList)
            {
                StoredProcedureLineItemViewModel storedProcedureLineItemViewModel = new StoredProcedureLineItemViewModel();
                BufferMgr.TransferBuffer(database, storedProcedureLineItemViewModel);
                storedProceduresListViewModel.Items.Add(storedProcedureLineItemViewModel);
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