using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Mvc;
using Quest.MPDW.Controllers;
using Quest.Util.Status;
using Quest.Util.Buffer;
using Quest.Util.Data;
using Quest.Functional.ASM;
using Quest.Functional.FMS;
using Quest.Functional.MasterPricing;
using Quest.MPDW.Services.Data;
using Quest.MPDW.Models;
using Quest.MPDW.Modelers;
using Quest.MasterPricing.DataMgr.Models;
using Quest.MasterPricing.DataMgr.Modelers;


namespace Quest.MasterPricing.DataMgr
{
    public class FilterController : DataMgrBaseController
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
        [HttpGet]
        public ActionResult Read(FilterEditorViewModel viewModel)
        {
            questStatus status = null;
            UserMessageModeler userMessageModeler = null;

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Log Operation
             *---------------------------------------------------------------------------------------------------------------------------------*/
            status = LogOperation();
            if (!questStatusDef.IsSuccess(status))
            {
                userMessageModeler = new UserMessageModeler(status);
                return Json(userMessageModeler, JsonRequestBehavior.AllowGet);
            }

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Authorize
             *---------------------------------------------------------------------------------------------------------------------------------*/
            status = Authorize(viewModel._ctx);
            if (!questStatusDef.IsSuccess(status))
            {
                userMessageModeler = new UserMessageModeler(status);
                return Json(userMessageModeler, JsonRequestBehavior.AllowGet);
            }

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Perform operation.
             *---------------------------------------------------------------------------------------------------------------------------------*/
            FilterId filterId = new FilterId(viewModel.Id);
            FilterEditorViewModel filterEditorViewModel = null;
            FilterEditorModeler filterEditorModeler = new FilterEditorModeler(this.Request, this.UserSession);
            status = filterEditorModeler.Read(filterId, out filterEditorViewModel);
            if (!questStatusDef.IsSuccess(status))
            {
                userMessageModeler = new UserMessageModeler(status);
                return Json(userMessageModeler, JsonRequestBehavior.AllowGet);
            }

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Return result.
             *---------------------------------------------------------------------------------------------------------------------------------*/
            status = new questStatus(Severity.Success);
            filterEditorViewModel.questStatus = status;
            return Json(filterEditorViewModel, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult Cancel(FilterEditorViewModel viewModel)
        {
            questStatus status = null;

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Log Operation
             *---------------------------------------------------------------------------------------------------------------------------------*/
            status = LogOperation();
            if (!questStatusDef.IsSuccess(status))
            {
                // TODO
                throw new Exception("LogOperation failed");
            }

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Authorize
             *---------------------------------------------------------------------------------------------------------------------------------*/
            status = Authorize(viewModel._ctx);
            if (!questStatusDef.IsSuccess(status))
            {
                // TODO
                throw new Exception("Authorize failed");
            }

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Direct user to list
             *---------------------------------------------------------------------------------------------------------------------------------*/
            return (RedirectToAction("Index", "DataMgr", PropagateQueryString(Request)));
        }


        #region Options
        //----------------------------------------------------------------------------------------------------------------------------------
        // Options
        //----------------------------------------------------------------------------------------------------------------------------------
        [HttpGet]
        public ActionResult FilterIdOptions(FilterEditorViewModel filterEditorViewModel)
        {
            questStatus status = null;


            /*----------------------------------------------------------------------------------------------------------------------------------
             * Log Operation
             *---------------------------------------------------------------------------------------------------------------------------------*/
            status = LogOperation();
            if (!questStatusDef.IsSuccess(status))
            {
                UserMessageModeler userMessageModeler = new UserMessageModeler(status);
                return Json(userMessageModeler.UserMessage, JsonRequestBehavior.AllowGet);
            }

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Authorize
             *---------------------------------------------------------------------------------------------------------------------------------*/
            status = Authorize(filterEditorViewModel._ctx);
            if (!questStatusDef.IsSuccess(status))
            {
                UserMessageModeler userMessageModeler = new UserMessageModeler(status);
                return Json(userMessageModeler.UserMessage, JsonRequestBehavior.AllowGet);
            }

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Perform Operation
             *---------------------------------------------------------------------------------------------------------------------------------*/
            TablesetId tablesetId = new TablesetId(filterEditorViewModel.TablesetId);
            List<OptionValuePair> optionValuePairList = null;
            FilterEditorModeler filterEditorModeler = new FilterEditorModeler(this.Request, this.UserSession);
            status = filterEditorModeler.GetFilterOptions(tablesetId, out optionValuePairList, filterEditorViewModel.FilterId.ToString());
            if (!questStatusDef.IsSuccess(status))
            {
                UserMessageModeler userMessageModeler = new UserMessageModeler(status);
                return Json(userMessageModeler.UserMessage, JsonRequestBehavior.AllowGet);
            }

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Respond
             *---------------------------------------------------------------------------------------------------------------------------------*/
            return Json(optionValuePairList, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #endregion


        #region POST Methods
        /*==================================================================================================================================
         * POST Methods
         *=================================================================================================================================*/
        [HttpPost]
        public ActionResult Save(FilterEditorViewModel filterEditorViewModel)
        {
            questStatus status = null;

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Log Operation
             *---------------------------------------------------------------------------------------------------------------------------------*/
            status = LogOperation();
            if (!questStatusDef.IsSuccess(status))
            {
                filterEditorViewModel.questStatus = status;
                return Json(filterEditorViewModel, JsonRequestBehavior.AllowGet);
            }

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Authorize
             *---------------------------------------------------------------------------------------------------------------------------------*/
            status = Authorize(filterEditorViewModel._ctx);
            if (!questStatusDef.IsSuccess(status))
            {
                filterEditorViewModel.questStatus = status;
                return Json(filterEditorViewModel, JsonRequestBehavior.AllowGet);
            }

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Perform operation.
             *---------------------------------------------------------------------------------------------------------------------------------*/
            bool bInitialCreation = filterEditorViewModel.Id < BaseId.VALID_ID ? true : false;
            FilterEditorModeler filterEditorModeler = new FilterEditorModeler(this.Request, this.UserSession);
            status = filterEditorModeler.Save(filterEditorViewModel);
            if (!questStatusDef.IsSuccess(status))
            {
                filterEditorViewModel.questStatus = status;
                return Json(filterEditorViewModel, JsonRequestBehavior.AllowGet);
            }


            /*----------------------------------------------------------------------------------------------------------------------------------
             * Return result.
             *---------------------------------------------------------------------------------------------------------------------------------*/
            status = new questStatus(Severity.Success, "Filter successfully" + (bInitialCreation ? " created" : " updated"));
            filterEditorViewModel.questStatus = status;
            return Json(filterEditorViewModel, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult Delete(FilterEditorViewModel filterEditorViewModel)
        {
            questStatus status = null;

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Log Operation
             *---------------------------------------------------------------------------------------------------------------------------------*/
            status = LogOperation();
            if (!questStatusDef.IsSuccess(status))
            {
                // TODO
                throw new Exception("LogOperation failed");
            }

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Authorize
             *---------------------------------------------------------------------------------------------------------------------------------*/
            status = Authorize(filterEditorViewModel._ctx);
            if (!questStatusDef.IsSuccess(status))
            {
                // TODO
                throw new Exception("Authorize failed");
            }

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Perform operation.
             *---------------------------------------------------------------------------------------------------------------------------------*/
            FilterId filterId = new FilterId(filterEditorViewModel.Id);
            FilterEditorModeler filterEditorModeler = new FilterEditorModeler(this.Request, this.UserSession);
            status = filterEditorModeler.Delete(filterId);
            if (!questStatusDef.IsSuccess(status))
            {
                filterEditorViewModel.questStatus = status;
                return Json(filterEditorViewModel, JsonRequestBehavior.AllowGet);
            }

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Return result.
             *---------------------------------------------------------------------------------------------------------------------------------*/
            status = new questStatus(Severity.Success, "Filter successfully deleted");
            filterEditorViewModel.questStatus = status;
            return Json(filterEditorViewModel, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult Run(FilterRunViewModel viewModel)
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
            if (viewModel.Items.Count == 0)
            {
                string items = Request.Form["Items"].ToString();
                JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
                List<FilterItemViewModel> itemsList = javaScriptSerializer.Deserialize<List<FilterItemViewModel>>(items);
                viewModel.Items = itemsList;


                string resultsOptions = Request.Form["_ResultsOptions"].ToString();
                ResultsOptionsViewModel _ResultsOptions = javaScriptSerializer.Deserialize<ResultsOptionsViewModel>(resultsOptions);
                viewModel._ResultsOptions = _ResultsOptions;
            }
            FilterRunViewModel filterRunViewModel = null;
            ResultsSet resultsSet = null;
            FilterPanelModeler filterPanelModeler = new FilterPanelModeler(Request, this.UserSession, viewModel);
            status = filterPanelModeler.Run(viewModel, out filterRunViewModel, out resultsSet);
            if (!questStatusDef.IsSuccess(status))
            {
                viewModel.questStatus = status;
                return Json(viewModel, JsonRequestBehavior.AllowGet);
            }

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Return result view model or as Excel
             *---------------------------------------------------------------------------------------------------------------------------------*/
            if (viewModel.bExportToExcel)
            {
                Response.ClearContent();
                Response.AddHeader("content-disposition", "atachment;filename=" + filterRunViewModel.Name.Replace(" ", "_") + ".xls");
                Response.AddHeader("Content-Type", "application/vnd.ms-excel");
                WriteTsv(resultsSet, Response.Output);
                Response.Flush();
                Response.End();
                return new EmptyResult();
            }
            else {
                status = new questStatus(Severity.Success, "Filter successfully run");
                filterRunViewModel.questStatus = status;
                return Json(filterRunViewModel, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public ActionResult ExportRun(FilterRunViewModel viewModel)
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
            FilterRunViewModel filterRunViewModel = null;
            ResultsSet resultsSet = null;
            FilterPanelModeler filterPanelModeler = new FilterPanelModeler(Request, this.UserSession, viewModel);
            status = filterPanelModeler.Run(viewModel, out filterRunViewModel, out resultsSet);
            if (!questStatusDef.IsSuccess(status))
            {
                viewModel.questStatus = status;
                return Json(viewModel, JsonRequestBehavior.AllowGet);
            }

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Return result view model or as Excel
             *---------------------------------------------------------------------------------------------------------------------------------*/
            Response.ClearContent();
            Response.AddHeader("content-disposition", "atachment;filename=" + filterRunViewModel.Name.Replace(" ", "_") + ".xls");
            Response.AddHeader("Content-Type", "application/vnd.ms-excel");
            WriteTsv(resultsSet, Response.Output);
            Response.Flush();
            Response.End();
            return new EmptyResult();
        }
        [HttpPost]
        public ActionResult Copy(FilterCopyViewModel viewModel)
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
            FilterCopyViewModel filterCopyViewModel = null;
            FilterPanelModeler filterPanelModeler = new FilterPanelModeler(Request, this.UserSession, viewModel);
            status = filterPanelModeler.Copy(viewModel, out filterCopyViewModel);
            if (!questStatusDef.IsSuccess(status))
            {
                viewModel.questStatus = status;
                return Json(viewModel, JsonRequestBehavior.AllowGet);
            }

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Return result.
             *---------------------------------------------------------------------------------------------------------------------------------*/
            status = new questStatus(Severity.Success, "Filter successfully copied.  Click on the tableset again to load it.");
            filterCopyViewModel.questStatus = status;
            return Json(filterCopyViewModel, JsonRequestBehavior.AllowGet);
        }
        #endregion


        #region Private Methods
        /*==================================================================================================================================
         * Private Methods
         *=================================================================================================================================*/
        #endregion
    }
}
