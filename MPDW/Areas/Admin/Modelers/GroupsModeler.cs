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
using Quest.MPDW.Models;
using Quest.MPDW.Admin.Models;
using Quest.MPDW.Services.Business.Accounts;


namespace Quest.MPDW.Admin.Modelers
{
    public class GroupsModeler : AdminBaseModeler
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
        public GroupsModeler(HttpRequestBase httpRequestBase, UserSession userSession)
            : base(httpRequestBase, userSession)
        {
            initialize();
        }
        #endregion


        #region Public Methods
        /*==================================================================================================================================
        * Public Methods
        *=================================================================================================================================*/

        #region LOAD 
        //----------------------------------------------------------------------------------------------------------------------------------
        // LOAD
        //----------------------------------------------------------------------------------------------------------------------------------
        public questStatus Load(out List<BootstrapTreenodeViewModel> groupNodeList)
        {
            // Initialize
            questStatus status = null;
            groupNodeList = null;


            // Set up query options.
            // TEMPORARY: OPTIMIZE THIS
            List<SearchField> searchFieldList = new List<SearchField>();
            SearchOptions searchOptions = new SearchOptions();
            searchOptions.SearchFieldList = searchFieldList;
            QueryOptions queryOptions = new QueryOptions();
            queryOptions.SearchOptions = searchOptions;
            QueryResponse queryResponse = null;


            // Get groups.
            List<Group> groupList = null;
            GroupsMgr groupsMgr = new GroupsMgr(this.UserSession);
            status = groupsMgr.List(queryOptions, out groupList, out queryResponse);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }

            // Transfer model
            groupNodeList = new List<BootstrapTreenodeViewModel>();
            foreach (Group group in groupList)
            {
                BootstrapTreenodeViewModel groupNode = null;
                FormatBootstrapTreeviewNode(group, out groupNode);
                groupNodeList.Add(groupNode);
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