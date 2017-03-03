using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Quest.MPDW.Controllers;
using Quest.Util.Status;
using Quest.Util.Buffer;
using Quest.Functional.ASM;
using Quest.Functional.FMS;
using Quest.MPDW.Services.Data;
using Quest.MPDW.Models;
using Quest.MPDW.Modelers;
using Quest.MasterPricing.DataMgr.Models;
using Quest.MasterPricing.DataMgr.Modelers;
using Quest.Functional.MasterPricing;


namespace Quest.MasterPricing.DataMgr
{
    public class ResultsController : DataMgrBaseController
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
        #endregion


        #region GET Methods
        /*==================================================================================================================================
         * GET Methods
         *=================================================================================================================================*/

        #region Options
        //----------------------------------------------------------------------------------------------------------------------------------
        // Options
        //----------------------------------------------------------------------------------------------------------------------------------
        #endregion

        #endregion


        #region POST Methods
        /*==================================================================================================================================
         * POST Methods
         *=================================================================================================================================*/
        [HttpPost]
        public ActionResult Export(FilterResultsExportViewModel viewModel)
        {
            questStatus status = null;

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Log Operation
             *---------------------------------------------------------------------------------------------------------------------------------*/
            status = LogOperation();
            if (!questStatusDef.IsSuccess(status))
            {
                viewModel.questStatus = status;
                return Json(viewModel, JsonRequestBehavior.AllowGet);
            }

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Authorize
             *---------------------------------------------------------------------------------------------------------------------------------*/
            status = Authorize(viewModel._ctx);
            if (!questStatusDef.IsSuccess(status))
            {
                viewModel.questStatus = status;
                return Json(viewModel, JsonRequestBehavior.AllowGet);
            }

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Perform operation.
             *---------------------------------------------------------------------------------------------------------------------------------*/
            ResultsSet resultsSet = null;
            FilterPanelModeler filterPanelModeler = new FilterPanelModeler(Request, this.UserSession, viewModel);
            status = filterPanelModeler.Export(viewModel, out resultsSet);
            if (!questStatusDef.IsSuccess(status))
            {
                viewModel.questStatus = status;
                return Json(viewModel, JsonRequestBehavior.AllowGet);
            }

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Return result.
             *---------------------------------------------------------------------------------------------------------------------------------*/
            Response.ClearContent();
            Response.AddHeader("content-disposition", "atachment;filename=" + viewModel.Name.Replace(" ", "_") + ".xls");
            Response.AddHeader("Content-Type", "application/vnd.ms-excel");
            WriteTsv(resultsSet, Response.Output);
            Response.Flush();
            Response.End();
            return new EmptyResult();
        }
        public void WriteTsv(ResultsSet resultsSet, TextWriter output)
        {
            foreach (KeyValuePair<string, Column> kvp in resultsSet.ResultColumns)
            {
                output.Write(kvp.Key); // header
                output.Write("\t");
            }
            output.WriteLine();
            foreach (dynamic _dynRow in resultsSet.Data)
            {
                foreach (KeyValuePair<string, object> kvp in _dynRow)
                {
                    string value = kvp.Value == null ? "(null)" : kvp.Value.ToString();
                    output.Write(value);
                    output.Write("\t");

                }
                output.WriteLine();
            }
        }
        #endregion


        #region Private Methods
        /*==================================================================================================================================
         * Private Methods
         *=================================================================================================================================*/
        #endregion
    }
}
