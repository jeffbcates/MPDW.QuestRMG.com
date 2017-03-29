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
    public class TablesetController : DataMgrBaseController
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
        public ActionResult Index(DataMgrTablesetViewModel viewModel)
        {
            questStatus status = null;
            DataMgrTablesetViewModel dataMgrTablesetViewModel = null;

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Log Operation
             *---------------------------------------------------------------------------------------------------------------------------------*/
            status = LogOperation();
            if (!questStatusDef.IsSuccess(status))
            {
                dataMgrTablesetViewModel = new DataMgrTablesetViewModel(this.UserSession, viewModel);
                dataMgrTablesetViewModel.questStatus = status;
                return View(dataMgrTablesetViewModel);
            }

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Authorize
             *---------------------------------------------------------------------------------------------------------------------------------*/
            status = Authorize(viewModel._ctx);
            if (!questStatusDef.IsSuccess(status))
            {
                dataMgrTablesetViewModel = new DataMgrTablesetViewModel(this.UserSession, viewModel);
                dataMgrTablesetViewModel.questStatus = status;
                return View(dataMgrTablesetViewModel);
            }

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Redirect to Tablesets if no tablesetId specified.
             *---------------------------------------------------------------------------------------------------------------------------------*/
             if (viewModel.Id < BaseId.VALID_ID)
            {
                return (RedirectToAction("Index", "DataMgr", PropagateQueryString(Request)));
            }

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Read tableset data management info.
             *---------------------------------------------------------------------------------------------------------------------------------*/
            TablesetId tablesetId = new TablesetId(viewModel.Id);
            TablesetDataModeler tablesetDataModeler = new TablesetDataModeler(this.Request, this.UserSession, viewModel);
            status = tablesetDataModeler.Read(tablesetId, out dataMgrTablesetViewModel);
            if (!questStatusDef.IsSuccess(status))
            {
                dataMgrTablesetViewModel = new DataMgrTablesetViewModel(this.UserSession, viewModel);
                dataMgrTablesetViewModel.questStatus = status;
                return View(dataMgrTablesetViewModel);
            }
     
            /*----------------------------------------------------------------------------------------------------------------------------------
             * Return view.
             *---------------------------------------------------------------------------------------------------------------------------------*/
            return View(dataMgrTablesetViewModel);
        }
        [HttpGet]
        public ActionResult FilterFolders(DataMgrTablesetViewModel viewModel)
        {
            questStatus status = null;
            DataMgrTablesetViewModel dataMgrTablesetViewModel = null;

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Log Operation
             *---------------------------------------------------------------------------------------------------------------------------------*/
            status = LogOperation();
            if (!questStatusDef.IsSuccess(status))
            {
                dataMgrTablesetViewModel = new DataMgrTablesetViewModel(this.UserSession, viewModel);
                dataMgrTablesetViewModel.questStatus = status;
                return View(dataMgrTablesetViewModel);
            }

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Authorize
             *---------------------------------------------------------------------------------------------------------------------------------*/
            status = Authorize(viewModel._ctx);
            if (!questStatusDef.IsSuccess(status))
            {
                dataMgrTablesetViewModel = new DataMgrTablesetViewModel(this.UserSession, viewModel);
                dataMgrTablesetViewModel.questStatus = status;
                return View(dataMgrTablesetViewModel);
            }

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Redirect to Tablesets if no tablesetId specified.
             *---------------------------------------------------------------------------------------------------------------------------------*/
            if (viewModel.Id < BaseId.VALID_ID)
            {
                return (RedirectToAction("Index", "DataMgr", PropagateQueryString(Request)));
            }

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Read tableset data management info.
             *---------------------------------------------------------------------------------------------------------------------------------*/
            TablesetId tablesetId = new TablesetId(viewModel.Id);
            TablesetDataModeler tablesetDataModeler = new TablesetDataModeler(this.Request, this.UserSession, viewModel);
            status = tablesetDataModeler.Read(tablesetId, out dataMgrTablesetViewModel);
            if (!questStatusDef.IsSuccess(status))
            {
                dataMgrTablesetViewModel = new DataMgrTablesetViewModel(this.UserSession, viewModel);
                dataMgrTablesetViewModel.questStatus = status;
                return View(dataMgrTablesetViewModel);
            }

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Return view.
             *---------------------------------------------------------------------------------------------------------------------------------*/
            return View(dataMgrTablesetViewModel);
        }
        [HttpGet]
        public ActionResult List(BaseUserSessionViewModel baseUserSessionViewModel)
        {
            // Initialize
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
            status = Authorize(baseUserSessionViewModel._ctx);
            if (!questStatusDef.IsSuccess(status))
            {
                userMessageModeler = new UserMessageModeler(status);
                return Json(userMessageModeler, JsonRequestBehavior.AllowGet);
            }

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Get tablesets
             *---------------------------------------------------------------------------------------------------------------------------------*/
            UserId userId = new UserId(this.UserSession.UserId);
            TablesetListViewModel tablesetListViewModel = null;
            TablesetModeler tablesetModeler = new TablesetModeler(this.Request, this.UserSession);
            status = tablesetModeler.List(userId, out tablesetListViewModel);
            if (!questStatusDef.IsSuccess(status))
            {
                userMessageModeler = new UserMessageModeler(status);
                return Json(userMessageModeler, JsonRequestBehavior.AllowGet);
            }

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Return data.
             *---------------------------------------------------------------------------------------------------------------------------------*/
            status = new questStatus(Severity.Success);
            tablesetListViewModel.questStatus = status;
            return Json(tablesetListViewModel, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult Cancel(DataMgrTablesetViewModel viewModel)
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

        [HttpGet]
        public ActionResult Entities(DataMgrTablesetViewModel viewModel)
        {
            questStatus status = null;
            DataMgrTablesetViewModel dataMgrTablesetViewModel = null;
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
             * Redirect to Tablesets if no tablesetId specified.
             *---------------------------------------------------------------------------------------------------------------------------------*/
            if (viewModel.Id < BaseId.VALID_ID)
            {
                status = new questStatus(Severity.Error, "Invalid Id value.  Must be 1 or greater.");
                userMessageModeler = new UserMessageModeler(status);
                return Json(userMessageModeler, JsonRequestBehavior.AllowGet);
            }

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Read tableset data management info.
             *---------------------------------------------------------------------------------------------------------------------------------*/
            TablesetId tablesetId = new TablesetId(viewModel.Id);
            TablesetDataModeler tablesetDataModeler = new TablesetDataModeler(this.Request, this.UserSession, viewModel);
            status = tablesetDataModeler.Read(tablesetId, out dataMgrTablesetViewModel);
            if (!questStatusDef.IsSuccess(status))
            {
                dataMgrTablesetViewModel = new DataMgrTablesetViewModel(this.UserSession, viewModel);
                dataMgrTablesetViewModel.questStatus = status;
                return View(dataMgrTablesetViewModel);
            }

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Return view.
             *---------------------------------------------------------------------------------------------------------------------------------*/
            status = new questStatus(Severity.Success);
            dataMgrTablesetViewModel.questStatus = status;
            return Json(dataMgrTablesetViewModel, JsonRequestBehavior.AllowGet);
        }


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
        #endregion


        #region Private Methods
        /*==================================================================================================================================
         * Private Methods
         *=================================================================================================================================*/
        #endregion
    }
}
