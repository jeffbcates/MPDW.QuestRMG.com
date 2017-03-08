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
using Quest.MPDW.Admin.Models;
using Quest.MPDW.Services.Business.Accounts;


namespace Quest.MPDW.Admin.Modelers
{
    public class GroupsListModeler : BaseListModeler
    {
        #region Declarations
        /*==================================================================================================================================
        * Declarations
        *=================================================================================================================================*/
        #endregion


        #region Constructors
        /*==================================================================================================================================
        * ConstructorsGroupListModeler
        *=================================================================================================================================*/
        public GroupsListModeler(HttpRequestBase httpRequestBase, UserSession userSession)
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
        public questStatus List(out GroupsListViewModel groupsListViewModel)
        {
            // Initialize
            questStatus status = null;
            groupsListViewModel = null;


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
            status = List(queryOptions, out groupsListViewModel);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus List(QueryOptions queryOptions, out GroupsListViewModel groupsListViewModel)
        {
            // Initialize
            questStatus status = null;
            groupsListViewModel = null;


            // List
            QueryResponse queryResponse = null;
            List<Quest.Functional.ASM.Group> groupList = null;
            GroupsMgr groupsMgr = new GroupsMgr(this.UserSession);
            status = groupsMgr.List(queryOptions, out groupList, out queryResponse);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }

            // Sort 
            groupList.Sort(delegate (Quest.Functional.ASM.Group i1, Quest.Functional.ASM.Group i2) { return i1.Name.CompareTo(i2.Name); });


            // Transfer model.
            // TODO: USE BaseListModeler to xfer queryOptions to QueryOptionsViewModel.
            groupsListViewModel = new GroupsListViewModel(this.UserSession);
            QueryResponseViewModel queryResponseViewModel = null;
            status = TransferQueryResponse(queryResponse, out queryResponseViewModel);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            groupsListViewModel.QueryResponse = queryResponseViewModel;
            foreach (Quest.Functional.ASM.Group group in groupList)
            {
                GroupLineItemViewModel groupLineItemViewModel = new GroupLineItemViewModel();
                BufferMgr.TransferBuffer(group, groupLineItemViewModel);
                groupsListViewModel.Items.Add(groupLineItemViewModel);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Page(out GroupsListViewModel groupsListViewModel)
        {
            // Initialize
            questStatus status = null;
            groupsListViewModel = null;


            // Set up query options.
            // TEMPORARY: OPTIMIZE THIS
            List<SearchField> searchFieldList = new List<SearchField>();

            SearchOptions searchOptions = new SearchOptions();
            searchOptions.SearchFieldList = searchFieldList;

            QueryOptions queryOptions = new QueryOptions(100, 1);
            queryOptions.SearchOptions = searchOptions;
            QueryResponse queryResponse = null;


            // List
            List<Quest.Functional.ASM.Group> groupList = null;
            GroupsMgr groupsMgr = new GroupsMgr(this.UserSession);
            status = groupsMgr.List(queryOptions, out groupList, out queryResponse);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }

            // Transfer model.
            groupsListViewModel = new GroupsListViewModel(this.UserSession);
            foreach (Quest.Functional.ASM.Group group in groupList)
            {
                GroupLineItemViewModel groupLineItemViewModel = new GroupLineItemViewModel();
                BufferMgr.TransferBuffer(group, groupLineItemViewModel);
                groupsListViewModel.Items.Add(groupLineItemViewModel);
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