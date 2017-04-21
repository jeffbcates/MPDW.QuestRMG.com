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
using Quest.MasterPricing.Filter.Models;
using Quest.MasterPricing.Filter.Modelers;
using Quest.MPDW.Services.Data;
using Quest.MasterPricing.Services.Business.Tablesets;
using Quest.MasterPricing.Services.Business.Filters;


namespace Quest.MasterPricing.Filter.Modelers
{
    public class FilterBaseModeler : BaseModeler
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
        public FilterBaseModeler(HttpRequestBase httpRequestBase, UserSession userSession)
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
        public questStatus GetFilterOperatorOptions(out List<OptionValuePair> optionsList, string Value = null, string Name = null)
        {
            // Initialize
            questStatus status = null;
            optionsList = null;


            // Get filter operators
            QueryOptions queryOptions = new QueryOptions(1000, 1);
            List<FilterOperator> filterOperatorList = null;
            QueryResponse queryResponse = null;
            FilterOperatorsMgr filterOperatorsMgr = new FilterOperatorsMgr(this.UserSession);
            status = filterOperatorsMgr.List(queryOptions, out filterOperatorList, out queryResponse);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }

            // Sort 
            // LEAVE IN ID ORDER.

            // Build options
            // Set selected if specified.
            optionsList = new List<OptionValuePair>();
            foreach (FilterOperator filterOperator in filterOperatorList)
            {
                OptionValuePair optionValuePair = new OptionValuePair();
                optionValuePair.Id = filterOperator.Id.ToString();
                optionValuePair.Label = filterOperator.Name;
                if (Value != null && Value == filterOperator.Id.ToString())
                {
                    optionValuePair.bSelected = true;
                }
                else if (Name != null && Name == filterOperator.Name)
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