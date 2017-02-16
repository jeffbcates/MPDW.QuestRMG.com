using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using Quest.Util.Status;
using Quest.Util.Buffer;
using Quest.Util.Data;
using Quest.Functional.ASM;
using Quest.Functional.MasterPricing;
using Quest.MPDW.Models.List;
using Quest.MPDW.Modelers;
using Quest.MasterPricing.Setup.Models;
using Quest.MasterPricing.Services.Business.Database;


namespace Quest.MasterPricing.Setup.Modelers
{
    public class DatabasesListModeler : BaseListModeler
    {
        #region Declarations
        /*==================================================================================================================================
        * Declarations
        *=================================================================================================================================*/
        #endregion


        #region Constructors
        /*==================================================================================================================================
        * ConstructorsDatabaseListModeler
        *=================================================================================================================================*/
        public DatabasesListModeler(HttpRequestBase httpRequestBase, UserSession userSession)
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
        public questStatus List(out DatabasesListViewModel databasesListViewModel)
        {
            // Initialize
            questStatus status = null;
            databasesListViewModel = null;


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
            status = List(queryOptions, out databasesListViewModel);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus List(QueryOptions queryOptions, out DatabasesListViewModel databasesListViewModel)
        {
            // Initialize
            questStatus status = null;
            databasesListViewModel = null;


            // List
            QueryResponse queryResponse = null;
            List<Quest.Functional.MasterPricing.Database> databaseList = null;
            DatabasesMgr databasesMgr = new DatabasesMgr(this.UserSession);
            status = databasesMgr.List(queryOptions, out databaseList, out queryResponse);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }

            // Sort 
            databaseList.Sort(delegate (Quest.Functional.MasterPricing.Database i1, Quest.Functional.MasterPricing.Database i2) { return i1.Name.CompareTo(i2.Name); });


            // Transfer model.
            // TODO: USE BaseListModeler to xfer queryOptions to QueryOptionsViewModel.
            databasesListViewModel = new DatabasesListViewModel(this.UserSession);
            QueryResponseViewModel queryResponseViewModel = null;
            status = TransferQueryResponse(queryResponse, out queryResponseViewModel);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            databasesListViewModel.QueryResponse = queryResponseViewModel;
            foreach (Quest.Functional.MasterPricing.Database database in databaseList)
            {
                DatabaseLineItemViewModel databaseLineItemViewModel = new DatabaseLineItemViewModel();
                BufferMgr.TransferBuffer(database, databaseLineItemViewModel);
                databasesListViewModel.Items.Add(databaseLineItemViewModel);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Page(out DatabasesListViewModel databasesListViewModel)
        {
            // Initialize
            questStatus status = null;
            databasesListViewModel = null;


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
            databasesListViewModel = new DatabasesListViewModel(this.UserSession);
            foreach (Quest.Functional.MasterPricing.Database database in databaseList)
            {
                DatabaseLineItemViewModel databaseLineItemViewModel = new DatabaseLineItemViewModel();
                BufferMgr.TransferBuffer(database, databaseLineItemViewModel);
                databasesListViewModel.Items.Add(databaseLineItemViewModel);
            }
            return (new questStatus(Severity.Success));
        }
        #endregion

        public questStatus RefreshSchema(DatabasesListViewModel databasesListViewModel)
        {
            // Initialize
            questStatus status = null;


            // Refresh schema(s)
            foreach (DatabaseLineItemViewModel databaseLineItemViewModel in databasesListViewModel.Items)
            {             
                DatabaseId databaseId = new DatabaseId(databaseLineItemViewModel.Id);
                DatabaseMgr databaseMgr = new DatabaseMgr(this.UserSession);
                status = databaseMgr.RefreshSchema(databaseId);
                if (!questStatusDef.IsSuccess(status))
                {
                    // TODO: DATABASE-SPECIFIC ERROR MESSAGE TO KNOW WHICH DATABASE FAILED.
                    return (status);
                }
            }
            return (new questStatus(Severity.Success));
        }

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