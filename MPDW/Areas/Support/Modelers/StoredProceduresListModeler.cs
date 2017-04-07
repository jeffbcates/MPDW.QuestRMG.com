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
    public class StoredProceduresListModeler : BaseListModeler
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
        public StoredProceduresListModeler(HttpRequestBase storedProcedureBase, UserSession userSession)
            : base(storedProcedureBase, userSession)
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
        public questStatus List(out StoredProceduresListViewModel storedProceduresListViewModel)
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
            SearchOptions searchOptions = new SearchOptions();
            searchOptions.SearchFieldList = searchFieldList;
            queryOptions.SearchOptions = searchOptions;


            // List
            status = List(queryOptions, out storedProceduresListViewModel);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus List(QueryOptions queryOptions, out StoredProceduresListViewModel storedProceduresListViewModel)
        {
            // Initialize
            questStatus status = null;
            storedProceduresListViewModel = null;


            // List
            QueryResponse queryResponse = null;
            List<Quest.Functional.Logging.StoredProcedureLog> storedProcedureLogList = null;
            StoredProcedureLogsMgr storedProcedureLogsMgr = new StoredProcedureLogsMgr(this.UserSession);
            status = storedProcedureLogsMgr.List(queryOptions, out storedProcedureLogList, out queryResponse);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }

            // Sort 
            storedProcedureLogList.Sort(delegate (Quest.Functional.Logging.StoredProcedureLog i1, Quest.Functional.Logging.StoredProcedureLog i2) { return i2.Created.CompareTo(i1.Created); });


            // Transfer model.
            // TODO: USE BaseListModeler to xfer queryOptions to QueryOptionsViewModel.
            storedProceduresListViewModel = new StoredProceduresListViewModel(this.UserSession);
            QueryResponseViewModel queryResponseViewModel = null;
            status = TransferQueryResponse(queryResponse, out queryResponseViewModel);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            storedProceduresListViewModel.QueryResponse = queryResponseViewModel;
            foreach (Quest.Functional.Logging.StoredProcedureLog storedProcedureLog in storedProcedureLogList)
            {
                StoredProcedureLineItemViewModel storedProcedureLineItemViewModel = new StoredProcedureLineItemViewModel();
                BufferMgr.TransferBuffer(storedProcedureLog, storedProcedureLineItemViewModel);
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
            List<Quest.Functional.Logging.StoredProcedureLog> storedProcedureLogList = null;
            StoredProcedureLogsMgr storedProcedureLogsMgr = new StoredProcedureLogsMgr(this.UserSession);
            status = storedProcedureLogsMgr.List(queryOptions, out storedProcedureLogList, out queryResponse);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }

            // Transfer model.
            storedProceduresListViewModel = new StoredProceduresListViewModel(this.UserSession);
            foreach (Quest.Functional.Logging.StoredProcedureLog storedProcedureLog in storedProcedureLogList)
            {
                StoredProcedureLineItemViewModel storedProcedureLineItemViewModel = new StoredProcedureLineItemViewModel();
                BufferMgr.TransferBuffer(storedProcedureLog, storedProcedureLineItemViewModel);
                storedProceduresListViewModel.Items.Add(storedProcedureLineItemViewModel);
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