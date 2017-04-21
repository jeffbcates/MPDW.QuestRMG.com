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
    public class DatabasesListModeler : BaseListModeler
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
        public DatabasesListModeler(HttpRequestBase databaseBase, UserSession userSession)
            : base(databaseBase, userSession)
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
            List<Quest.Functional.Logging.DatabaseLog> databaseLogList = null;
            DatabaseLogsMgr databaseLogsMgr = new DatabaseLogsMgr(this.UserSession);
            status = databaseLogsMgr.List(queryOptions, out databaseLogList, out queryResponse);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }

            // Sort 
            databaseLogList.Sort(delegate (Quest.Functional.Logging.DatabaseLog i1, Quest.Functional.Logging.DatabaseLog i2) { return i2.Created.CompareTo(i1.Created); });


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
            foreach (Quest.Functional.Logging.DatabaseLog databaseLog in databaseLogList)
            {
                DatabaseLineItemViewModel databaseLineItemViewModel = new DatabaseLineItemViewModel();
                BufferMgr.TransferBuffer(databaseLog, databaseLineItemViewModel);
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
            List<Quest.Functional.Logging.DatabaseLog> databaseLogList = null;
            DatabaseLogsMgr databaseLogsMgr = new DatabaseLogsMgr(this.UserSession);
            status = databaseLogsMgr.List(queryOptions, out databaseLogList, out queryResponse);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }

            // Transfer model.
            databasesListViewModel = new DatabasesListViewModel(this.UserSession);
            foreach (Quest.Functional.Logging.DatabaseLog databaseLog in databaseLogList)
            {
                DatabaseLineItemViewModel databaseLineItemViewModel = new DatabaseLineItemViewModel();
                BufferMgr.TransferBuffer(databaseLog, databaseLineItemViewModel);
                databasesListViewModel.Items.Add(databaseLineItemViewModel);
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
        public questStatus Clear(DatabasesListViewModel databasesListViewModel)
        {
            // Initialize
            questStatus status = null;


            DatabaseLogsMgr databaseLogsMgr = new DatabaseLogsMgr(this.UserSession);
            status = databaseLogsMgr.Clear();
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
            List<DatabaseLogId> databaseLogIdList = new List<DatabaseLogId>();
            foreach (BaseId baseId in deleteLogItemsViewModel.Items)
            {
                DatabaseLogId databaseLogId = new DatabaseLogId(baseId.Id);
                databaseLogIdList.Add(databaseLogId);
            }

            // Delete items
            DatabaseLogsMgr databaseLogsMgr = new DatabaseLogsMgr(this.UserSession);
            status = databaseLogsMgr.Delete(databaseLogIdList);
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