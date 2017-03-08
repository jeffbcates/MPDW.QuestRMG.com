using System;
using System.Collections.Generic;
using System.Web;
using Quest.Util.Status;
using Quest.Util.Buffer;
using Quest.Util.Data;
using Quest.Functional.ASM;
using Quest.Functional.FMS;
using Quest.MPDW.Modelers;
using Quest.MasterPricing.DataMgr.Models;
using Quest.Functional.MasterPricing;
using Quest.MasterPricing.Services.Business.Tablesets;


namespace Quest.MasterPricing.DataMgr.Modelers
{
    public class TablesetModeler : DataMgrBaseModeler
    {
        #region Declarations
        /*==================================================================================================================================
         * Declarations
         *=================================================================================================================================*/
        private DataMgrBaseViewModel _dataMgrBaseViewModel = null;

        #endregion


        #region Constructors
        /*==================================================================================================================================
        * ConstructorsRequisitionListModeler
        *=================================================================================================================================*/
        public TablesetModeler(HttpRequestBase httpRequestBase, UserSession userSession)
            : base(httpRequestBase, userSession)
        {
            initialize();
        }
        public TablesetModeler(HttpRequestBase httpRequestBase, UserSession userSession, DataMgrBaseViewModel dataMgrBaseViewModel)
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
        public questStatus List(UserId userId, out TablesetListViewModel tablesetListViewModel)
        {
            // Initialize
            questStatus status = null;
            tablesetListViewModel = null;


            // Set up query options.
            // TEMPORARY: OPTIMIZE THIS
            List<SearchField> searchFieldList = new List<SearchField>();
            SearchField searchField = new SearchField();
            searchField.Name = "bEnabled";
            searchField.SearchOperation = SearchOperation.Equal;
            searchField.Type = typeof(bool);
            searchField.Value = "1".ToString();
            searchFieldList.Add(searchField);

            SearchOptions searchOptions = new SearchOptions();
            searchOptions.SearchFieldList = searchFieldList;

            QueryOptions queryOptions = new QueryOptions(100, 1);
            queryOptions.SearchOptions = searchOptions;
            QueryResponse queryResponse = null;


            // Get DataMgr tablesets
            List<Tableset> tablesetList = null;
            TablesetMgr tablesetMgr = new TablesetMgr();
            status = tablesetMgr.List(queryOptions, out tablesetList, out queryResponse);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }


            // Sort by name
            tablesetList.Sort(delegate (Tableset i1, Tableset i2) { return i1.Name.CompareTo(i2.Name); });


            // Build model
            tablesetListViewModel = new TablesetListViewModel(new UserSession());
            foreach (Tableset tableset in tablesetList)
            {
                TablesetLineItemViewModel tablesetLineItemViewModel = new TablesetLineItemViewModel();
                BufferMgr.TransferBuffer(tableset, tablesetLineItemViewModel);
                tablesetListViewModel.Items.Add(tablesetLineItemViewModel);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus List(TablesetId tablesetId, out FilterTableTreeviewViewModel filterTableTreeviewViewModel)
        {
            // Initialize
            filterTableTreeviewViewModel = new FilterTableTreeviewViewModel(this.UserSession, this._dataMgrBaseViewModel);
            filterTableTreeviewViewModel.TablesetId = tablesetId.Id;
            

            // TODO


            return (new questStatus(Severity.Success));
        }
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