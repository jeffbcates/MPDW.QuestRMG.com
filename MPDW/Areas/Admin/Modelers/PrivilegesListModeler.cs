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
    public class PrivilegesListModeler : BaseListModeler
    {
        #region Declarations
        /*==================================================================================================================================
        * Declarations
        *=================================================================================================================================*/
        #endregion


        #region Constructors
        /*==================================================================================================================================
        * ConstructorsPrivilegeListModeler
        *=================================================================================================================================*/
        public PrivilegesListModeler(HttpRequestBase httpRequestBase, UserSession userSession)
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
        public questStatus List(out PrivilegesListViewModel privilegesListViewModel)
        {
            // Initialize
            questStatus status = null;
            privilegesListViewModel = null;


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
            status = List(queryOptions, out privilegesListViewModel);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus List(QueryOptions queryOptions, out PrivilegesListViewModel privilegesListViewModel)
        {
            // Initialize
            questStatus status = null;
            privilegesListViewModel = null;


            // List
            QueryResponse queryResponse = null;
            List<Quest.Functional.ASM.Privilege> privilegeList = null;
            PrivilegesMgr privilegesMgr = new PrivilegesMgr(this.UserSession);
            status = privilegesMgr.List(queryOptions, out privilegeList, out queryResponse);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }

            // Sort 
            privilegeList.Sort(delegate (Quest.Functional.ASM.Privilege i1, Quest.Functional.ASM.Privilege i2) { return i1.Name.CompareTo(i2.Name); });


            // Transfer model.
            // TODO: USE BaseListModeler to xfer queryOptions to QueryOptionsViewModel.
            privilegesListViewModel = new PrivilegesListViewModel(this.UserSession);
            QueryResponseViewModel queryResponseViewModel = null;
            status = TransferQueryResponse(queryResponse, out queryResponseViewModel);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            privilegesListViewModel.QueryResponse = queryResponseViewModel;
            foreach (Quest.Functional.ASM.Privilege privilege in privilegeList)
            {
                PrivilegeLineItemViewModel privilegeLineItemViewModel = new PrivilegeLineItemViewModel();
                BufferMgr.TransferBuffer(privilege, privilegeLineItemViewModel);
                privilegesListViewModel.Items.Add(privilegeLineItemViewModel);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Page(out PrivilegesListViewModel privilegesListViewModel)
        {
            // Initialize
            questStatus status = null;
            privilegesListViewModel = null;


            // Set up query options.
            // TEMPORARY: OPTIMIZE THIS
            List<SearchField> searchFieldList = new List<SearchField>();

            SearchOptions searchOptions = new SearchOptions();
            searchOptions.SearchFieldList = searchFieldList;

            QueryOptions queryOptions = new QueryOptions(100, 1);
            queryOptions.SearchOptions = searchOptions;
            QueryResponse queryResponse = null;


            // List
            List<Quest.Functional.ASM.Privilege> privilegeList = null;
            PrivilegesMgr privilegesMgr = new PrivilegesMgr(this.UserSession);
            status = privilegesMgr.List(queryOptions, out privilegeList, out queryResponse);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }

            // Transfer model.
            privilegesListViewModel = new PrivilegesListViewModel(this.UserSession);
            foreach (Quest.Functional.ASM.Privilege privilege in privilegeList)
            {
                PrivilegeLineItemViewModel privilegeLineItemViewModel = new PrivilegeLineItemViewModel();
                BufferMgr.TransferBuffer(privilege, privilegeLineItemViewModel);
                privilegesListViewModel.Items.Add(privilegeLineItemViewModel);
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