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
using Quest.MasterPricing.Database.Models;
using Quest.MasterPricing.Database.Modelers;
using Quest.MPDW.Services.Data;
using Quest.MasterPricing.Services.Business.Database;
using Quest.MasterPricing.Services.Business.Filters;


namespace Quest.MasterPricing.Database.Modelers
{
    public class DatabaseBaseModeler : BaseModeler
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
        public DatabaseBaseModeler(HttpRequestBase httpRequestBase, UserSession userSession)
            : base(httpRequestBase, userSession)
        {
            initialize();
        }
        #endregion


        #region Public Methods
        /*==================================================================================================================================
        * Public Methods
        *=================================================================================================================================*/
        public questStatus GetDatabase(DatabaseId databaseId, out DatabaseBaseViewModel databaseBaseViewModel)
        {
            // Initialize
            questStatus status = null;
            databaseBaseViewModel = null;


            // Read the database
            Quest.Functional.MasterPricing.Database database = null;
            DatabasesMgr databasesMgr = new DatabasesMgr(this.UserSession);
            status = databasesMgr.Read(databaseId, out database);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }

            // Transfer model.
            databaseBaseViewModel = new DatabaseBaseViewModel(this.UserSession);
            BufferMgr.TransferBuffer(database, databaseBaseViewModel);
            databaseBaseViewModel.LastRefresh = database.LastRefresh.HasValue ?
                    database.LastRefresh.Value.ToString("MM/dd/yyyy HH:mm:ss") : "";

            return (new questStatus(Severity.Success));
        }


        #region Options
        //----------------------------------------------------------------------------------------------------------------------------------
        // Options
        //----------------------------------------------------------------------------------------------------------------------------------
        #endregion


        #region Formatting
        //----------------------------------------------------------------------------------------------------------------------------------
        // Formatting
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