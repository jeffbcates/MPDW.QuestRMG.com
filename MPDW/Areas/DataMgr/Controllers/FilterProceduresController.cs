using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
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
    public class FilterProceduresController : DataMgrBaseController
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

        #region Options
        //----------------------------------------------------------------------------------------------------------------------------------
        // Options
        //----------------------------------------------------------------------------------------------------------------------------------
        [HttpGet]
        public ActionResult ProcedureOptions(FilterEditorViewModel filterEditorViewModel)
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
            FilterProcedureModeler filterProcedureModeler = new FilterProcedureModeler(this.Request, this.UserSession);
            status = filterProcedureModeler.GetFilterProcedureOptions(tablesetId, out optionValuePairList, filterEditorViewModel.FilterId.ToString());
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
        #endregion


        #region Private Methods
        /*==================================================================================================================================
         * Private Methods
         *=================================================================================================================================*/
        #endregion
    }
}
