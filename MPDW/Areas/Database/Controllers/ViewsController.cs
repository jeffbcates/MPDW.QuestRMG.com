using System;
using System.Collections.Generic;
using System.Linq;
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
using Quest.MasterPricing.Database.Models;
using Quest.MasterPricing.Setup.Models;
using Quest.MasterPricing.Database.Modelers;
using Quest.Functional.MasterPricing;


namespace Quest.MasterPricing.Database
{
    public class ViewsController : DatabaseBaseController
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
        public ActionResult Index(DatabaseEditorViewModel databaseEditorViewModel)
        {
            questStatus status = null;

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Log Operation
             *---------------------------------------------------------------------------------------------------------------------------------*/
            status = LogOperation();
            if (!questStatusDef.IsSuccess(status))
            {
                databaseEditorViewModel.questStatus = status;
                return (View("Index", databaseEditorViewModel));
            }

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Authorize
             *---------------------------------------------------------------------------------------------------------------------------------*/
            status = Authorize(databaseEditorViewModel._ctx);
            if (!questStatusDef.IsSuccess(status))
            {
                databaseEditorViewModel.questStatus = status;
                return (View("Index", databaseEditorViewModel));
            }

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Return view.
             *---------------------------------------------------------------------------------------------------------------------------------*/
            Quest.MasterPricing.Database.Models.ViewsListViewModel viewsListViewModel = new Quest.MasterPricing.Database.Models.ViewsListViewModel(this.UserSession, databaseEditorViewModel);
            viewsListViewModel.DatabaseId = databaseEditorViewModel.Id;
            return View(viewsListViewModel);

        }
        [HttpGet]
        public ActionResult List(Quest.MasterPricing.Database.Models.ViewsListViewModel viewsListViewModel)
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
            status = Authorize(viewsListViewModel._ctx);
            if (!questStatusDef.IsSuccess(status))
            {
                userMessageModeler = new UserMessageModeler(status);
                return Json(userMessageModeler, JsonRequestBehavior.AllowGet);
            }

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Get database views.
             *---------------------------------------------------------------------------------------------------------------------------------*/
            DatabaseId databaseId = new DatabaseId(viewsListViewModel.DatabaseId);
            Quest.MasterPricing.Database.Models.ViewsListViewModel viewsListViewModelNEW = null;
            ViewsListModeler viewsListModeler = new ViewsListModeler(this.Request, this.UserSession);
            status = viewsListModeler.List(databaseId, out viewsListViewModelNEW);
            if (!questStatusDef.IsSuccess(status))
            {
                userMessageModeler = new UserMessageModeler(status);
                return Json(userMessageModeler, JsonRequestBehavior.AllowGet);
            }

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Return data.
             *---------------------------------------------------------------------------------------------------------------------------------*/
            status = new questStatus(Severity.Success);
            viewsListViewModelNEW.questStatus = status;
            return Json(viewsListViewModelNEW, JsonRequestBehavior.AllowGet);
        }


        #region Paging
        //----------------------------------------------------------------------------------------------------------------------------------
        // Paging
        //----------------------------------------------------------------------------------------------------------------------------------
        [HttpGet]
        public ActionResult First(Quest.MasterPricing.Database.Models.ViewsListViewModel viewsListViewModel)
        {
            questStatus status = null;

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Log Operation
             *---------------------------------------------------------------------------------------------------------------------------------*/
            status = LogOperation();
            if (!questStatusDef.IsSuccess(status))
            {
                status = new questStatus(Severity.Success);
                viewsListViewModel.questStatus = status;
                return Json(viewsListViewModel, JsonRequestBehavior.AllowGet);
            }

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Authorize
             *---------------------------------------------------------------------------------------------------------------------------------*/
            status = Authorize(viewsListViewModel._ctx);
            if (!questStatusDef.IsSuccess(status))
            {
                status = new questStatus(Severity.Success);
                viewsListViewModel.questStatus = status;
                return Json(viewsListViewModel, JsonRequestBehavior.AllowGet);
            }

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Get list of requisitions for this user.
             *---------------------------------------------------------------------------------------------------------------------------------*/
            DatabaseId databaseId = new DatabaseId(viewsListViewModel.DatabaseId);
            Quest.MasterPricing.Database.Models.ViewsListViewModel viewsetsListViewModelNEW = null;

            AuthorId authorId = new AuthorId(this.UserSession.UserId);
            ViewsListModeler viewsListModeler = new ViewsListModeler(this.Request, this.UserSession);
            status = viewsListModeler.List(databaseId, out viewsetsListViewModelNEW);
            if (!questStatusDef.IsSuccess(status))
            {
                status = new questStatus(Severity.Success);
                viewsListViewModel.questStatus = status;
                return Json(viewsListViewModel, JsonRequestBehavior.AllowGet);
            }

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Return view
             *---------------------------------------------------------------------------------------------------------------------------------*/
            status = new questStatus(Severity.Success);
            viewsetsListViewModelNEW.questStatus = status;
            return Json(viewsetsListViewModelNEW, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult Prev(Quest.MasterPricing.Database.Models.ViewsListViewModel viewsListViewModel)
        {
            questStatus status = null;

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Log Operation
             *---------------------------------------------------------------------------------------------------------------------------------*/
            status = LogOperation();
            if (!questStatusDef.IsSuccess(status))
            {
                status = new questStatus(Severity.Success);
                viewsListViewModel.questStatus = status;
                return Json(viewsListViewModel, JsonRequestBehavior.AllowGet);
            }

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Authorize
             *---------------------------------------------------------------------------------------------------------------------------------*/
            status = Authorize(viewsListViewModel._ctx);
            if (!questStatusDef.IsSuccess(status))
            {
                status = new questStatus(Severity.Success);
                viewsListViewModel.questStatus = status;
                return Json(viewsListViewModel, JsonRequestBehavior.AllowGet);
            }

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Get list of requisitions for this user.
             *---------------------------------------------------------------------------------------------------------------------------------*/
            DatabaseId databaseId = new DatabaseId(viewsListViewModel.DatabaseId);
            Quest.MasterPricing.Database.Models.ViewsListViewModel viewsetsListViewModelNEW = null;
            ViewsListModeler viewsListModeler = new ViewsListModeler(this.Request, this.UserSession);
            status = viewsListModeler.List(databaseId, out viewsetsListViewModelNEW);
            if (!questStatusDef.IsSuccess(status))
            {
                status = new questStatus(Severity.Success);
                viewsListViewModel.questStatus = status;
                return Json(viewsListViewModel, JsonRequestBehavior.AllowGet);
            }

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Return view
             *---------------------------------------------------------------------------------------------------------------------------------*/
            status = new questStatus(Severity.Success);
            viewsetsListViewModelNEW.questStatus = status;
            return Json(viewsetsListViewModelNEW, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult PageNum(Quest.MasterPricing.Database.Models.ViewsListViewModel viewsListViewModel)
        {
            questStatus status = null;

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Log Operation
             *---------------------------------------------------------------------------------------------------------------------------------*/
            status = LogOperation();
            if (!questStatusDef.IsSuccess(status))
            {
                status = new questStatus(Severity.Success);
                viewsListViewModel.questStatus = status;
                return Json(viewsListViewModel, JsonRequestBehavior.AllowGet);
            }

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Authorize
             *---------------------------------------------------------------------------------------------------------------------------------*/
            status = Authorize(viewsListViewModel._ctx);
            if (!questStatusDef.IsSuccess(status))
            {
                status = new questStatus(Severity.Success);
                viewsListViewModel.questStatus = status;
                return Json(viewsListViewModel, JsonRequestBehavior.AllowGet);
            }

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Get list of requisitions for this user.
             *---------------------------------------------------------------------------------------------------------------------------------*/
            DatabaseId databaseId = new DatabaseId(viewsListViewModel.DatabaseId);
            Quest.MasterPricing.Database.Models.ViewsListViewModel viewsetsListViewModelNEW = null;
            ViewsListModeler viewsListModeler = new ViewsListModeler(this.Request, this.UserSession);
            status = viewsListModeler.List(databaseId, out viewsetsListViewModelNEW);
            if (!questStatusDef.IsSuccess(status))
            {
                status = new questStatus(Severity.Success);
                viewsListViewModel.questStatus = status;
                return Json(viewsListViewModel, JsonRequestBehavior.AllowGet);
            }

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Return view
             *---------------------------------------------------------------------------------------------------------------------------------*/
            status = new questStatus(Severity.Success);
            viewsetsListViewModelNEW.questStatus = status;
            return Json(viewsetsListViewModelNEW, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult Next(Quest.MasterPricing.Database.Models.ViewsListViewModel viewsListViewModel)
        {
            questStatus status = null;


            /*----------------------------------------------------------------------------------------------------------------------------------
             * Log Operation
             *---------------------------------------------------------------------------------------------------------------------------------*/
            status = LogOperation();
            if (!questStatusDef.IsSuccess(status))
            {
                status = new questStatus(Severity.Success);
                viewsListViewModel.questStatus = status;
                return Json(viewsListViewModel, JsonRequestBehavior.AllowGet);
            }

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Authorize
             *---------------------------------------------------------------------------------------------------------------------------------*/
            status = Authorize(viewsListViewModel._ctx);
            if (!questStatusDef.IsSuccess(status))
            {
                status = new questStatus(Severity.Success);
                viewsListViewModel.questStatus = status;
                return Json(viewsListViewModel, JsonRequestBehavior.AllowGet);
            }

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Get list of requisitions for this user.
             *---------------------------------------------------------------------------------------------------------------------------------*/
            DatabaseId databaseId = new DatabaseId(viewsListViewModel.DatabaseId);
            Quest.MasterPricing.Database.Models.ViewsListViewModel viewsetsListViewModelNEW = null;
            ViewsListModeler viewsListModeler = new ViewsListModeler(this.Request, this.UserSession);
            status = viewsListModeler.List(databaseId, out viewsetsListViewModelNEW);
            if (!questStatusDef.IsSuccess(status))
            {
                status = new questStatus(Severity.Success);
                viewsListViewModel.questStatus = status;
                return Json(viewsListViewModel, JsonRequestBehavior.AllowGet);
            }

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Return view
             *---------------------------------------------------------------------------------------------------------------------------------*/
            status = new questStatus(Severity.Success);
            viewsetsListViewModelNEW.questStatus = status;
            return Json(viewsetsListViewModelNEW, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult Last(Quest.MasterPricing.Database.Models.ViewsListViewModel viewsListViewModel)
        {
            questStatus status = null;

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Log Operation
             *---------------------------------------------------------------------------------------------------------------------------------*/
            status = LogOperation();
            if (!questStatusDef.IsSuccess(status))
            {
                status = new questStatus(Severity.Success);
                viewsListViewModel.questStatus = status;
                return Json(viewsListViewModel, JsonRequestBehavior.AllowGet);
            }

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Authorize
             *---------------------------------------------------------------------------------------------------------------------------------*/
            status = Authorize(viewsListViewModel._ctx);
            if (!questStatusDef.IsSuccess(status))
            {
                status = new questStatus(Severity.Success);
                viewsListViewModel.questStatus = status;
                return Json(viewsListViewModel, JsonRequestBehavior.AllowGet);
            }

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Get list of requisitions for this user.
             *---------------------------------------------------------------------------------------------------------------------------------*/
            DatabaseId databaseId = new DatabaseId(viewsListViewModel.DatabaseId);
            Quest.MasterPricing.Database.Models.ViewsListViewModel viewsetsListViewModelNEW = null;
            ViewsListModeler viewsListModeler = new ViewsListModeler(this.Request, this.UserSession);
            status = viewsListModeler.List(databaseId, out viewsetsListViewModelNEW);
            if (!questStatusDef.IsSuccess(status))
            {
                status = new questStatus(Severity.Success);
                viewsListViewModel.questStatus = status;
                return Json(viewsListViewModel, JsonRequestBehavior.AllowGet);
            }

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Return view
             *---------------------------------------------------------------------------------------------------------------------------------*/
            status = new questStatus(Severity.Success);
            viewsetsListViewModelNEW.questStatus = status;
            return Json(viewsetsListViewModelNEW, JsonRequestBehavior.AllowGet);
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
