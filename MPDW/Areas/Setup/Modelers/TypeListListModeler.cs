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
using Quest.Functional.MasterPricing;
using Quest.MasterPricing.Setup.Models;
using Quest.MPDW.Models.List;
using Quest.MPDW.Modelers;
using Quest.MasterPricing.Services.Business.Database;


namespace Quest.MasterPricing.Setup.Modelers
{
    public class TypeListsListModeler : BaseListModeler
    {
        #region Declarations
        /*==================================================================================================================================
        * Declarations
        *=================================================================================================================================*/
        #endregion


        #region Constructors
        /*==================================================================================================================================
        * ConstructorsTypeListListModeler
        *=================================================================================================================================*/
        public TypeListsListModeler(HttpRequestBase httpRequestBase, UserSession userSession)
            : base(httpRequestBase, userSession)
        {
            initialize();
        }
        #endregion


        #region Public Methods
        /*==================================================================================================================================
        * Public Methods
        *=================================================================================================================================*/

        #region CRUD 
        //----------------------------------------------------------------------------------------------------------------------------------
        // CRUD
        //----------------------------------------------------------------------------------------------------------------------------------
        public questStatus List(out TypeListsListViewModel typeListsListViewModel)
        {
            // Initialize
            questStatus status = null;
            typeListsListViewModel = null;


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
            status = List(queryOptions, out typeListsListViewModel);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus List(QueryOptions queryOptions, out TypeListsListViewModel typeListsListViewModel)
        {
            // Initialize
            questStatus status = null;
            typeListsListViewModel = null;


            // List
            QueryResponse queryResponse = null;
            List<Quest.Functional.MasterPricing.TypeList> typeListList = null;
            TypeListsMgr typeListsMgr = new TypeListsMgr(this.UserSession);
            status = typeListsMgr.List(queryOptions, out typeListList, out queryResponse);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }

            // Sort 
            typeListList.Sort(delegate (Quest.Functional.MasterPricing.TypeList i1, Quest.Functional.MasterPricing.TypeList i2) { return i1.Name.CompareTo(i2.Name); });


            // Transfer model.
            // TODO: USE BaseListModeler to xfer queryOptions to QueryOptionsViewModel.
            typeListsListViewModel = new TypeListsListViewModel(this.UserSession);
            QueryResponseViewModel queryResponseViewModel = null;
            status = TransferQueryResponse(queryResponse, out queryResponseViewModel);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            typeListsListViewModel.QueryResponse = queryResponseViewModel;
            foreach (Quest.Functional.MasterPricing.TypeList typeList in typeListList)
            {
                TypeListLineItemViewModel typeListLineItemViewModel = new TypeListLineItemViewModel();
                BufferMgr.TransferBuffer(typeList, typeListLineItemViewModel);
                typeListsListViewModel.Items.Add(typeListLineItemViewModel);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Page(out TypeListsListViewModel typeListsListViewModel)
        {
            // Initialize
            questStatus status = null;
            typeListsListViewModel = null;


            // Set up query options.
            // TEMPORARY: OPTIMIZE THIS
            List<SearchField> searchFieldList = new List<SearchField>();

            SearchOptions searchOptions = new SearchOptions();
            searchOptions.SearchFieldList = searchFieldList;

            QueryOptions queryOptions = new QueryOptions(100, 1);
            queryOptions.SearchOptions = searchOptions;
            QueryResponse queryResponse = null;


            // List
            List<Quest.Functional.MasterPricing.TypeList> typeListList = null;
            TypeListsMgr typeListsMgr = new TypeListsMgr(this.UserSession);
            status = typeListsMgr.List(queryOptions, out typeListList, out queryResponse);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }

            // Transfer model.
            typeListsListViewModel = new TypeListsListViewModel(this.UserSession);
            foreach (Quest.Functional.MasterPricing.TypeList typeList in typeListList)
            {
                TypeListLineItemViewModel typeListLineItemViewModel = new TypeListLineItemViewModel();
                BufferMgr.TransferBuffer(typeList, typeListLineItemViewModel);
                typeListsListViewModel.Items.Add(typeListLineItemViewModel);
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