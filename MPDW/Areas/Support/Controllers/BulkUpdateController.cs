using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Quest.MPDW.Controllers;
using Quest.Util.Status;
using Quest.Util.Buffer;
using Quest.MPDW.Services.Data;
using Quest.MPDW.Models;
using Quest.MPDW.Modelers;
using Quest.MPDW.Support.Models;
using Quest.MPDW.Support.Modelers;


namespace Quest.MPDW.Support
{
    public class BulkUpdatesController : SupportBaseController
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
        public ActionResult Index(BaseUserSessionViewModel baseUserSessionViewModel)
        {
            // Initialize
            questStatus status = null;

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Log Operation
             *---------------------------------------------------------------------------------------------------------------------------------*/
            status = LogOperation();
            if (!questStatusDef.IsSuccess(status))
            {
                baseUserSessionViewModel.questStatus = status;
                return (View("Index", baseUserSessionViewModel));
            }

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Authorize
             *---------------------------------------------------------------------------------------------------------------------------------*/
            status = Authorize(baseUserSessionViewModel._ctx);
            if (!questStatusDef.IsSuccess(status))
            {
                baseUserSessionViewModel.questStatus = status;
                return (View("Index", baseUserSessionViewModel));
            }

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Return view.
             *---------------------------------------------------------------------------------------------------------------------------------*/
            BulkUpdatesViewModel bulkUpdatesViewModel = new BulkUpdatesViewModel(this.UserSession, baseUserSessionViewModel);
            return View(bulkUpdatesViewModel);
        }
        [HttpGet]
        public ActionResult List(BulkUpdatesListViewModel bulkUpdatesListViewModel)
        {
            questStatus status = null;

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Log Operation
             *---------------------------------------------------------------------------------------------------------------------------------*/
            status = LogOperation();
            if (!questStatusDef.IsSuccess(status))
            {
                bulkUpdatesListViewModel.questStatus = status;
                return Json(bulkUpdatesListViewModel, JsonRequestBehavior.AllowGet);
            }

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Authorize
             *---------------------------------------------------------------------------------------------------------------------------------*/
            status = Authorize(bulkUpdatesListViewModel._ctx);
            if (!questStatusDef.IsSuccess(status))
            {
                bulkUpdatesListViewModel.questStatus = status;
                return Json(bulkUpdatesListViewModel, JsonRequestBehavior.AllowGet);
            }

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Get list of items.
             *---------------------------------------------------------------------------------------------------------------------------------*/
            BulkUpdatesListViewModel bulkUpdatesListViewModelNEW = null;
            BulkUpdatesListModeler bulkUpdatesListModeler = new BulkUpdatesListModeler(this.Request, this.UserSession);
            status = bulkUpdatesListModeler.List(out bulkUpdatesListViewModelNEW);
            if (!questStatusDef.IsSuccess(status))
            {
                bulkUpdatesListViewModel.questStatus = status;
                return Json(bulkUpdatesListViewModel, JsonRequestBehavior.AllowGet);
            }

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Return view
             *---------------------------------------------------------------------------------------------------------------------------------*/
            status = new questStatus(Severity.Success);
            bulkUpdatesListViewModelNEW.questStatus = status;
            return Json(bulkUpdatesListViewModelNEW, JsonRequestBehavior.AllowGet);
        }

        #region Paging
        //----------------------------------------------------------------------------------------------------------------------------------
        // Paging
        //----------------------------------------------------------------------------------------------------------------------------------
        [HttpGet]
        public ActionResult First(BulkUpdatesListViewModel bulkUpdatesListViewModel)
        {
            questStatus status = null;

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Log Operation
             *---------------------------------------------------------------------------------------------------------------------------------*/
            status = LogOperation();
            if (!questStatusDef.IsSuccess(status))
            {
                status = new questStatus(Severity.Success);
                bulkUpdatesListViewModel.questStatus = status;
                return Json(bulkUpdatesListViewModel, JsonRequestBehavior.AllowGet);
            }

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Authorize
             *---------------------------------------------------------------------------------------------------------------------------------*/
            status = Authorize(bulkUpdatesListViewModel._ctx);
            if (!questStatusDef.IsSuccess(status))
            {
                status = new questStatus(Severity.Success);
                bulkUpdatesListViewModel.questStatus = status;
                return Json(bulkUpdatesListViewModel, JsonRequestBehavior.AllowGet);
            }

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Get list of items.
             *---------------------------------------------------------------------------------------------------------------------------------*/
            // TODO.
            BulkUpdatesListViewModel bulkUpdatesListViewModelNEW = null;
            BulkUpdatesListModeler usersListModeler = new BulkUpdatesListModeler(this.Request, this.UserSession);
            status = usersListModeler.List(out bulkUpdatesListViewModelNEW);
            if (!questStatusDef.IsSuccess(status))
            {
                status = new questStatus(Severity.Success);
                bulkUpdatesListViewModel.questStatus = status;
                return Json(bulkUpdatesListViewModel, JsonRequestBehavior.AllowGet);
            }

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Return view
             *---------------------------------------------------------------------------------------------------------------------------------*/
            status = new questStatus(Severity.Success);
            bulkUpdatesListViewModelNEW.questStatus = status;
            return Json(bulkUpdatesListViewModelNEW, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult Prev(BulkUpdatesListViewModel bulkUpdatesListViewModel)
        {
            questStatus status = null;

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Log Operation
             *---------------------------------------------------------------------------------------------------------------------------------*/
            status = LogOperation();
            if (!questStatusDef.IsSuccess(status))
            {
                status = new questStatus(Severity.Success);
                bulkUpdatesListViewModel.questStatus = status;
                return Json(bulkUpdatesListViewModel, JsonRequestBehavior.AllowGet);
            }

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Authorize
             *---------------------------------------------------------------------------------------------------------------------------------*/
            status = Authorize(bulkUpdatesListViewModel._ctx);
            if (!questStatusDef.IsSuccess(status))
            {
                status = new questStatus(Severity.Success);
                bulkUpdatesListViewModel.questStatus = status;
                return Json(bulkUpdatesListViewModel, JsonRequestBehavior.AllowGet);
            }

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Get list of items.
             *---------------------------------------------------------------------------------------------------------------------------------*/
            // TODO.
            BulkUpdatesListViewModel bulkUpdatesListViewModelNEW = null;
            BulkUpdatesListModeler usersListModeler = new BulkUpdatesListModeler(this.Request, this.UserSession);
            status = usersListModeler.List(out bulkUpdatesListViewModelNEW);
            if (!questStatusDef.IsSuccess(status))
            {
                status = new questStatus(Severity.Success);
                bulkUpdatesListViewModel.questStatus = status;
                return Json(bulkUpdatesListViewModel, JsonRequestBehavior.AllowGet);
            }

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Return view
             *---------------------------------------------------------------------------------------------------------------------------------*/
            status = new questStatus(Severity.Success);
            bulkUpdatesListViewModelNEW.questStatus = status;
            return Json(bulkUpdatesListViewModelNEW, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult PageNum(BulkUpdatesListViewModel bulkUpdatesListViewModel)
        {
            questStatus status = null;

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Log Operation
             *---------------------------------------------------------------------------------------------------------------------------------*/
            status = LogOperation();
            if (!questStatusDef.IsSuccess(status))
            {
                status = new questStatus(Severity.Success);
                bulkUpdatesListViewModel.questStatus = status;
                return Json(bulkUpdatesListViewModel, JsonRequestBehavior.AllowGet);
            }

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Authorize
             *---------------------------------------------------------------------------------------------------------------------------------*/
            status = Authorize(bulkUpdatesListViewModel._ctx);
            if (!questStatusDef.IsSuccess(status))
            {
                status = new questStatus(Severity.Success);
                bulkUpdatesListViewModel.questStatus = status;
                return Json(bulkUpdatesListViewModel, JsonRequestBehavior.AllowGet);
            }

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Get list of items.
             *---------------------------------------------------------------------------------------------------------------------------------*/
            // TODO.
            BulkUpdatesListViewModel bulkUpdatesListViewModelNEW = null;
            BulkUpdatesListModeler usersListModeler = new BulkUpdatesListModeler(this.Request, this.UserSession);
            status = usersListModeler.List(out bulkUpdatesListViewModelNEW);
            if (!questStatusDef.IsSuccess(status))
            {
                status = new questStatus(Severity.Success);
                bulkUpdatesListViewModel.questStatus = status;
                return Json(bulkUpdatesListViewModel, JsonRequestBehavior.AllowGet);
            }

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Return view
             *---------------------------------------------------------------------------------------------------------------------------------*/
            status = new questStatus(Severity.Success);
            bulkUpdatesListViewModelNEW.questStatus = status;
            return Json(bulkUpdatesListViewModelNEW, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult Next(BulkUpdatesListViewModel bulkUpdatesListViewModel)
        {
            questStatus status = null;


            /*----------------------------------------------------------------------------------------------------------------------------------
             * Log Operation
             *---------------------------------------------------------------------------------------------------------------------------------*/
            status = LogOperation();
            if (!questStatusDef.IsSuccess(status))
            {
                status = new questStatus(Severity.Success);
                bulkUpdatesListViewModel.questStatus = status;
                return Json(bulkUpdatesListViewModel, JsonRequestBehavior.AllowGet);
            }

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Authorize
             *---------------------------------------------------------------------------------------------------------------------------------*/
            status = Authorize(bulkUpdatesListViewModel._ctx);
            if (!questStatusDef.IsSuccess(status))
            {
                status = new questStatus(Severity.Success);
                bulkUpdatesListViewModel.questStatus = status;
                return Json(bulkUpdatesListViewModel, JsonRequestBehavior.AllowGet);
            }

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Get list of items.
             *---------------------------------------------------------------------------------------------------------------------------------*/
            // TODO.
            BulkUpdatesListViewModel bulkUpdatesListViewModelNEW = null;
            BulkUpdatesListModeler usersListModeler = new BulkUpdatesListModeler(this.Request, this.UserSession);
            status = usersListModeler.List(out bulkUpdatesListViewModelNEW);
            if (!questStatusDef.IsSuccess(status))
            {
                status = new questStatus(Severity.Success);
                bulkUpdatesListViewModel.questStatus = status;
                return Json(bulkUpdatesListViewModel, JsonRequestBehavior.AllowGet);
            }

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Return view
             *---------------------------------------------------------------------------------------------------------------------------------*/
            status = new questStatus(Severity.Success);
            bulkUpdatesListViewModelNEW.questStatus = status;
            return Json(bulkUpdatesListViewModelNEW, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult Last(BulkUpdatesListViewModel bulkUpdatesListViewModel)
        {
            questStatus status = null;

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Log Operation
             *---------------------------------------------------------------------------------------------------------------------------------*/
            status = LogOperation();
            if (!questStatusDef.IsSuccess(status))
            {
                status = new questStatus(Severity.Success);
                bulkUpdatesListViewModel.questStatus = status;
                return Json(bulkUpdatesListViewModel, JsonRequestBehavior.AllowGet);
            }

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Authorize
             *---------------------------------------------------------------------------------------------------------------------------------*/
            status = Authorize(bulkUpdatesListViewModel._ctx);
            if (!questStatusDef.IsSuccess(status))
            {
                status = new questStatus(Severity.Success);
                bulkUpdatesListViewModel.questStatus = status;
                return Json(bulkUpdatesListViewModel, JsonRequestBehavior.AllowGet);
            }

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Get list of items.
             *---------------------------------------------------------------------------------------------------------------------------------*/
            // TODO.
            BulkUpdatesListViewModel bulkUpdatesListViewModelNEW = null;
            BulkUpdatesListModeler usersListModeler = new BulkUpdatesListModeler(this.Request, this.UserSession);
            status = usersListModeler.List(out bulkUpdatesListViewModelNEW);
            if (!questStatusDef.IsSuccess(status))
            {
                status = new questStatus(Severity.Success);
                bulkUpdatesListViewModel.questStatus = status;
                return Json(bulkUpdatesListViewModel, JsonRequestBehavior.AllowGet);
            }

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Return view
             *---------------------------------------------------------------------------------------------------------------------------------*/
            status = new questStatus(Severity.Success);
            bulkUpdatesListViewModelNEW.questStatus = status;
            return Json(bulkUpdatesListViewModelNEW, JsonRequestBehavior.AllowGet);
        }
        #endregion


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
