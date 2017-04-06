using System;
using System.Text;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Quest.Util.Status;
using Quest.Util.Buffer;
using Quest.Util.Data;
using Quest.Functional.ASM;
using Quest.Functional.FMS;
using Quest.Functional.MasterPricing;
using Quest.MPDW.Models;
using Quest.MPDW.Modelers;
using Quest.MasterPricing.DataMgr.Models;
using Quest.MasterPricing.DataMgr.Modelers;
using Quest.MPDW.Services.Data;
using Quest.MasterPricing.Services.Business.Tablesets;
using Quest.MasterPricing.Services.Business.Filters;


namespace Quest.MPDW.Admin.Modelers
{
    public class AdminBaseModeler : BaseModeler
    {
        #region Declarations
        /*==================================================================================================================================
        * Declarations
        *=================================================================================================================================*/
        #endregion


        #region Constructors
        /*==================================================================================================================================
        * Constructors
        *=================================================================================================================================*/
        public AdminBaseModeler(HttpRequestBase httpRequestBase, UserSession userSession)
            : base(httpRequestBase, userSession)
        {
            initialize();
        }
        #endregion


        #region Public Methods
        /*==================================================================================================================================
        * Public Methods
        *=================================================================================================================================*/


        #region Options
        //----------------------------------------------------------------------------------------------------------------------------------
        // Options
        //----------------------------------------------------------------------------------------------------------------------------------
        #endregion


        #region Formatting
        //----------------------------------------------------------------------------------------------------------------------------------
        // Formatting
        //----------------------------------------------------------------------------------------------------------------------------------
        public questStatus FormatBootstrapTreeviewNode(Group group, out BootstrapTreenodeViewModel bootstrapTreenodeViewModel)
        {
            // Initialize
            bootstrapTreenodeViewModel = new BootstrapTreenodeViewModel();
            bootstrapTreenodeViewModel.Id = group.Id;
            bootstrapTreenodeViewModel.type = "group";
            bootstrapTreenodeViewModel.icon = "fa fa-group padding-right-20";
            bootstrapTreenodeViewModel.text = group.Name;
            bootstrapTreenodeViewModel.selectable = "true";

            return (new questStatus(Severity.Success));
        }
        public questStatus FormatBootstrapTreeviewNode(Privilege privilege, out BootstrapTreenodeViewModel bootstrapTreenodeViewModel)
        {
            // Initialize
            bootstrapTreenodeViewModel = new BootstrapTreenodeViewModel();
            bootstrapTreenodeViewModel.Id = privilege.Id;
            bootstrapTreenodeViewModel.type = "privilege";
            bootstrapTreenodeViewModel.icon = "fa fa-unlock padding-right-20";
            bootstrapTreenodeViewModel.text = privilege.Name;
            bootstrapTreenodeViewModel.selectable = "true";

            return (new questStatus(Severity.Success));
        }
        public questStatus FormatBootstrapTreeviewNode(User user, out BootstrapTreenodeViewModel bootstrapTreenodeViewModel)
        {
            // Initialize
            bootstrapTreenodeViewModel = new BootstrapTreenodeViewModel();
            bootstrapTreenodeViewModel.Id = user.Id;
            bootstrapTreenodeViewModel.type = "user";
            bootstrapTreenodeViewModel.icon = "fa fa-user padding-right-20";
            bootstrapTreenodeViewModel.text = user.FirstName + " " + user.LastName;
            bootstrapTreenodeViewModel.selectable = "true";

            return (new questStatus(Severity.Success));
        }
        #endregion


        #region Validations
        //----------------------------------------------------------------------------------------------------------------------------------
        // Validations
        //----------------------------------------------------------------------------------------------------------------------------------
        #endregion


        #region Transfers
        //----------------------------------------------------------------------------------------------------------------------------------
        // Transfers
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