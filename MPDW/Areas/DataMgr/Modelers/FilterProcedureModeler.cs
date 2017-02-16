using System;
using System.Collections.Generic;
using System.Web;
using Quest.Util.Status;
using Quest.Util.Buffer;
using Quest.Util.Data;
using Quest.Functional.ASM;
using Quest.Functional.FMS;
using Quest.Functional.MasterPricing;
using Quest.MPDW.Models;
using Quest.MPDW.Modelers;
using Quest.MasterPricing.DataMgr.Models;
using Quest.MasterPricing.Services.Business.Database;


namespace Quest.MasterPricing.DataMgr.Modelers
{
    public class FilterProcedureModeler : DataMgrBaseModeler
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
        public FilterProcedureModeler(HttpRequestBase httpRequestBase, UserSession userSession)
            : base(httpRequestBase, userSession)
        {
            initialize();
        }
        public FilterProcedureModeler(HttpRequestBase httpRequestBase, UserSession userSession, DataMgrBaseViewModel dataMgrBaseViewModel)
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
        public questStatus GetFilterProcedureOptions(TablesetId tablesetId, out List<OptionValuePair> optionsList, string Value = null, string Name = null)
        {
            // Initialize
            questStatus status = null;
            optionsList = null;



            ////// Get stored procedures for filter's database.
            ////FilterMgr filterMgr = new FilterMgr(this.UserSession);
            ////List<StoredProcedure> storedProcedureList = null;
            ////status = filterMgr.GetDatabaseStoredProcedures(tablesetId, out storedProcedureList);
            ////if (!questStatusDef.IsSuccess(status))
            ////{
            ////    return (status);
            ////}

            // Get the database Id.
            Tableset tableset = null;
            TablesetsMgr tablesetsMgr = new TablesetsMgr(this.UserSession);
            status = tablesetsMgr.Read(tablesetId, out tableset);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }

            // Get the stored procedures.
            DatabaseId databaseId = new DatabaseId(tableset.DatabaseId);
            List<StoredProcedure> storedProcedureList = null;
            StoredProceduresMgr storedProceduresMgr = new StoredProceduresMgr(this.UserSession);
            status = storedProceduresMgr.Read(databaseId, out storedProcedureList);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }


            // Sort 
            storedProcedureList.Sort(delegate (StoredProcedure i1, StoredProcedure i2) { return i1.Name.CompareTo(i2.Name); });


            // Build options
            // Set selected if specified.
            optionsList = new List<OptionValuePair>();
            foreach (StoredProcedure storedProcedure in storedProcedureList)
            {
                OptionValuePair optionValuePair = new OptionValuePair();
                optionValuePair.Id = storedProcedure.Id.ToString();
                optionValuePair.Label = storedProcedure.Name;
                if (Value != null && Value == storedProcedure.Id.ToString())
                {
                    optionValuePair.bSelected = true;
                }
                else if (Name != null && Name == storedProcedure.Name)
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