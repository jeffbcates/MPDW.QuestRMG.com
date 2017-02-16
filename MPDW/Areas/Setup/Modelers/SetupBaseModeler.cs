using System;
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
using Quest.MasterPricing.Setup.Models;
using Quest.MasterPricing.Setup.Modelers;
using Quest.MPDW.Services.Data;
using Quest.MasterPricing.Services.Business.Database;


namespace Quest.MasterPricing.Setup.Modelers
{
    public class SetupBaseModeler : BaseModeler
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
        public SetupBaseModeler(HttpRequestBase httpRequestBase, UserSession userSession)
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
        public questStatus GetDatabaseOptions(out List<OptionValuePair> optionsList, string Value = null, string Name = null)
        {
            // Initialize
            questStatus status = null;
            optionsList = null;


            // Get database
            QueryOptions queryOptions = new QueryOptions(100, 1);
            List<Quest.Functional.MasterPricing.Database> databaseTypeList = null;
            QueryResponse queryResponse = null;
            DatabasesMgr databasesMgr = new DatabasesMgr(this.UserSession);
            status = databasesMgr.List(queryOptions, out databaseTypeList, out queryResponse);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }

            // Sort 
            databaseTypeList.Sort(delegate (Quest.Functional.MasterPricing.Database i1, Quest.Functional.MasterPricing.Database i2) { return i1.Name.CompareTo(i2.Name); });


            // Build options
            // Set selected if specified.
            optionsList = new List<OptionValuePair>();
            foreach (Quest.Functional.MasterPricing.Database Database in databaseTypeList)
            {
                OptionValuePair optionValuePair = new OptionValuePair();
                optionValuePair.Id = Database.Id.ToString();
                optionValuePair.Label = Database.Name;
                if (Value != null && Value == Database.Id.ToString())
                {
                    optionValuePair.bSelected = true;
                }
                else if (Name != null && Name == Database.Name)
                {
                    optionValuePair.bSelected = true;
                }
                optionsList.Add(optionValuePair);
            }

            // Insert default option
            status = AddDefaultOptions(optionsList, "-1", "Select one ...");
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus GetTablesetOptions(out List<OptionValuePair> optionsList, string Value = null, string Name = null)
        {
            // Initialize
            questStatus status = null;
            optionsList = null;


            // Get tableset
            QueryOptions queryOptions = new QueryOptions(100, 1);
            List<Tableset> tablesetTypeList = null;
            QueryResponse queryResponse = null;
            TablesetsMgr tablesetsMgr = new TablesetsMgr(this.UserSession);
            status = tablesetsMgr.List(queryOptions, out tablesetTypeList, out queryResponse);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }

            // Sort 
            tablesetTypeList.Sort(delegate (Tableset i1, Tableset i2) { return i1.Name.CompareTo(i2.Name); });


            // Build options
            // Set selected if specified.
            optionsList = new List<OptionValuePair>();
            foreach (Tableset Tableset in tablesetTypeList)
            {
                OptionValuePair optionValuePair = new OptionValuePair();
                optionValuePair.Id = Tableset.Id.ToString();
                optionValuePair.Label = Tableset.Name;
                if (Value != null && Value == Tableset.Id.ToString())
                {
                    optionValuePair.bSelected = true;
                }
                else if (Name != null && Name == Tableset.Name)
                {
                    optionValuePair.bSelected = true;
                }
                optionsList.Add(optionValuePair);
            }

            // Insert default option
            status = AddDefaultOptions(optionsList, "-1", "Select one ...");
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        #endregion


        #region Formatting
        //----------------------------------------------------------------------------------------------------------------------------------
        // Formatting
        //----------------------------------------------------------------------------------------------------------------------------------
        public questStatus FormatBootstrapTreeviewNode(Table table, out BootstrapTreenodeViewModel bootstrapTreenodeViewModel)
        {
            // Initialize
            bootstrapTreenodeViewModel = new BootstrapTreenodeViewModel();
            bootstrapTreenodeViewModel.Id = table.Id;
            bootstrapTreenodeViewModel.type = "table";
            bootstrapTreenodeViewModel.text = "[" + table.Schema + "].[" + table.Name + "]";
            bootstrapTreenodeViewModel.selectable = "true";

            return (new questStatus(Severity.Success));
        }
        public questStatus FormatBootstrapTreeviewNode(DBTable dbTable, out BootstrapTreenodeViewModel bootstrapTreenodeViewModel)
        {
            // Initialize
            bootstrapTreenodeViewModel = new BootstrapTreenodeViewModel();
            bootstrapTreenodeViewModel.Id = dbTable.Id;
            bootstrapTreenodeViewModel.type = "table";
            bootstrapTreenodeViewModel.text = "[" + dbTable.Schema + "].[" + dbTable.Name + "]";
            bootstrapTreenodeViewModel.selectable = "true";

            return (new questStatus(Severity.Success));
        }
        public questStatus FormatBootstrapTreeviewNode(View view, out BootstrapTreenodeViewModel bootstrapTreenodeViewModel)
        {
            // Initialize
            bootstrapTreenodeViewModel = new BootstrapTreenodeViewModel();
            bootstrapTreenodeViewModel.Id = view.Id;
            bootstrapTreenodeViewModel.type = "view";
            bootstrapTreenodeViewModel.text = "[" + view.Schema + "].[" + view.Name + "]";
            bootstrapTreenodeViewModel.selectable = "true";

            return (new questStatus(Severity.Success));
        }
        public questStatus FormatBootstrapTreeviewNode(DBView dbView, out BootstrapTreenodeViewModel bootstrapTreenodeViewModel)
        {
            // Initialize
            bootstrapTreenodeViewModel = new BootstrapTreenodeViewModel();
            bootstrapTreenodeViewModel.Id = dbView.Id;
            bootstrapTreenodeViewModel.type = "view";
            bootstrapTreenodeViewModel.text = "[" + dbView.Schema + "].[" + dbView.Name + "]";
            bootstrapTreenodeViewModel.selectable = "true";

            return (new questStatus(Severity.Success));
        }
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