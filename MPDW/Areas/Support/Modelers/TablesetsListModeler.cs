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
    public class TablesetsListModeler : BaseListModeler
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
        public TablesetsListModeler(HttpRequestBase tablesetBase, UserSession userSession)
            : base(tablesetBase, userSession)
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
        public questStatus List(out TablesetsListViewModel tablesetsListViewModel)
        {
            // Initialize
            questStatus status = null;
            tablesetsListViewModel = null;


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
            status = List(queryOptions, out tablesetsListViewModel);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus List(QueryOptions queryOptions, out TablesetsListViewModel tablesetsListViewModel)
        {
            // Initialize
            questStatus status = null;
            tablesetsListViewModel = null;


            // List
            QueryResponse queryResponse = null;
            List<Quest.Functional.Logging.TablesetLog> tablesetLogList = null;
            TablesetLogsMgr tablesetLogsMgr = new TablesetLogsMgr(this.UserSession);
            status = tablesetLogsMgr.List(queryOptions, out tablesetLogList, out queryResponse);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }

            // Sort 
            tablesetLogList.Sort(delegate (Quest.Functional.Logging.TablesetLog i1, Quest.Functional.Logging.TablesetLog i2) { return i2.Created.CompareTo(i1.Created); });


            // Transfer model.
            // TODO: USE BaseListModeler to xfer queryOptions to QueryOptionsViewModel.
            tablesetsListViewModel = new TablesetsListViewModel(this.UserSession);
            QueryResponseViewModel queryResponseViewModel = null;
            status = TransferQueryResponse(queryResponse, out queryResponseViewModel);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            tablesetsListViewModel.QueryResponse = queryResponseViewModel;
            foreach (Quest.Functional.Logging.TablesetLog tablesetLog in tablesetLogList)
            {
                TablesetLineItemViewModel tablesetLineItemViewModel = new TablesetLineItemViewModel();
                BufferMgr.TransferBuffer(tablesetLog, tablesetLineItemViewModel);
                tablesetsListViewModel.Items.Add(tablesetLineItemViewModel);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Page(out TablesetsListViewModel tablesetsListViewModel)
        {
            // Initialize
            questStatus status = null;
            tablesetsListViewModel = null;


            // Set up query options.
            // TEMPORARY: OPTIMIZE THIS
            List<SearchField> searchFieldList = new List<SearchField>();

            SearchOptions searchOptions = new SearchOptions();
            searchOptions.SearchFieldList = searchFieldList;

            QueryOptions queryOptions = new QueryOptions(100, 1);
            queryOptions.SearchOptions = searchOptions;
            QueryResponse queryResponse = null;


            // List
            List<Quest.Functional.Logging.TablesetLog> tablesetLogList = null;
            TablesetLogsMgr tablesetLogsMgr = new TablesetLogsMgr(this.UserSession);
            status = tablesetLogsMgr.List(queryOptions, out tablesetLogList, out queryResponse);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }

            // Transfer model.
            tablesetsListViewModel = new TablesetsListViewModel(this.UserSession);
            foreach (Quest.Functional.Logging.TablesetLog tablesetLog in tablesetLogList)
            {
                TablesetLineItemViewModel tablesetLineItemViewModel = new TablesetLineItemViewModel();
                BufferMgr.TransferBuffer(tablesetLog, tablesetLineItemViewModel);
                tablesetsListViewModel.Items.Add(tablesetLineItemViewModel);
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