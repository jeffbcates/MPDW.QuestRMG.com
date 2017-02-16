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
    public class TablesController : DatabaseBaseController
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
            Quest.MasterPricing.Database.Models.TablesListViewModel tablesListViewModel = new Quest.MasterPricing.Database.Models.TablesListViewModel(this.UserSession, databaseEditorViewModel);
            tablesListViewModel.DatabaseId = databaseEditorViewModel.Id;
            return View(tablesListViewModel);

        }
        [HttpGet]
        public ActionResult List(Quest.MasterPricing.Database.Models.TablesListViewModel tablesListViewModel)
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
            status = Authorize(tablesListViewModel._ctx);
            if (!questStatusDef.IsSuccess(status))
            {
                userMessageModeler = new UserMessageModeler(status);
                return Json(userMessageModeler, JsonRequestBehavior.AllowGet);
            }

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Get database tables.
             *---------------------------------------------------------------------------------------------------------------------------------*/
            DatabaseId databaseId = new DatabaseId(tablesListViewModel.DatabaseId);
            Quest.MasterPricing.Database.Models.TablesListViewModel tablesListViewModelNEW = null;
            TablesListModeler tablesListModeler = new TablesListModeler(this.Request, this.UserSession);
            status = tablesListModeler.List(databaseId, out tablesListViewModelNEW);
            if (!questStatusDef.IsSuccess(status))
            {
                userMessageModeler = new UserMessageModeler(status);
                return Json(userMessageModeler, JsonRequestBehavior.AllowGet);
            }

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Return data.
             *---------------------------------------------------------------------------------------------------------------------------------*/
            status = new questStatus(Severity.Success);
            tablesListViewModelNEW.questStatus = status;
            return Json(tablesListViewModelNEW, JsonRequestBehavior.AllowGet);
        }


        #region Paging
        //----------------------------------------------------------------------------------------------------------------------------------
        // Paging
        //----------------------------------------------------------------------------------------------------------------------------------
        [HttpGet]
        public ActionResult First(Quest.MasterPricing.Database.Models.TablesListViewModel tablesListViewModel)
        {
            questStatus status = null;

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Log Operation
             *---------------------------------------------------------------------------------------------------------------------------------*/
            status = LogOperation();
            if (!questStatusDef.IsSuccess(status))
            {
                status = new questStatus(Severity.Success);
                tablesListViewModel.questStatus = status;
                return Json(tablesListViewModel, JsonRequestBehavior.AllowGet);
            }

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Authorize
             *---------------------------------------------------------------------------------------------------------------------------------*/
            status = Authorize(tablesListViewModel._ctx);
            if (!questStatusDef.IsSuccess(status))
            {
                status = new questStatus(Severity.Success);
                tablesListViewModel.questStatus = status;
                return Json(tablesListViewModel, JsonRequestBehavior.AllowGet);
            }

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Get list of requisitions for this user.
             *---------------------------------------------------------------------------------------------------------------------------------*/
            DatabaseId databaseId = new DatabaseId(tablesListViewModel.DatabaseId);
            Quest.MasterPricing.Database.Models.TablesListViewModel tablesetsListViewModelNEW = null;

            AuthorId authorId = new AuthorId(this.UserSession.UserId);
            TablesListModeler tablesListModeler = new TablesListModeler(this.Request, this.UserSession);
            status = tablesListModeler.List(databaseId, out tablesetsListViewModelNEW);
            if (!questStatusDef.IsSuccess(status))
            {
                status = new questStatus(Severity.Success);
                tablesListViewModel.questStatus = status;
                return Json(tablesListViewModel, JsonRequestBehavior.AllowGet);
            }

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Return view
             *---------------------------------------------------------------------------------------------------------------------------------*/
            status = new questStatus(Severity.Success);
            tablesetsListViewModelNEW.questStatus = status;
            return Json(tablesetsListViewModelNEW, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult Prev(Quest.MasterPricing.Database.Models.TablesListViewModel tablesListViewModel)
        {
            questStatus status = null;

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Log Operation
             *---------------------------------------------------------------------------------------------------------------------------------*/
            status = LogOperation();
            if (!questStatusDef.IsSuccess(status))
            {
                status = new questStatus(Severity.Success);
                tablesListViewModel.questStatus = status;
                return Json(tablesListViewModel, JsonRequestBehavior.AllowGet);
            }

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Authorize
             *---------------------------------------------------------------------------------------------------------------------------------*/
            status = Authorize(tablesListViewModel._ctx);
            if (!questStatusDef.IsSuccess(status))
            {
                status = new questStatus(Severity.Success);
                tablesListViewModel.questStatus = status;
                return Json(tablesListViewModel, JsonRequestBehavior.AllowGet);
            }

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Get list of requisitions for this user.
             *---------------------------------------------------------------------------------------------------------------------------------*/
            DatabaseId databaseId = new DatabaseId(tablesListViewModel.DatabaseId);
            Quest.MasterPricing.Database.Models.TablesListViewModel tablesetsListViewModelNEW = null;
            TablesListModeler tablesListModeler = new TablesListModeler(this.Request, this.UserSession);
            status = tablesListModeler.List(databaseId, out tablesetsListViewModelNEW);
            if (!questStatusDef.IsSuccess(status))
            {
                status = new questStatus(Severity.Success);
                tablesListViewModel.questStatus = status;
                return Json(tablesListViewModel, JsonRequestBehavior.AllowGet);
            }

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Return view
             *---------------------------------------------------------------------------------------------------------------------------------*/
            status = new questStatus(Severity.Success);
            tablesetsListViewModelNEW.questStatus = status;
            return Json(tablesetsListViewModelNEW, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult PageNum(Quest.MasterPricing.Database.Models.TablesListViewModel tablesListViewModel)
        {
            questStatus status = null;

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Log Operation
             *---------------------------------------------------------------------------------------------------------------------------------*/
            status = LogOperation();
            if (!questStatusDef.IsSuccess(status))
            {
                status = new questStatus(Severity.Success);
                tablesListViewModel.questStatus = status;
                return Json(tablesListViewModel, JsonRequestBehavior.AllowGet);
            }

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Authorize
             *---------------------------------------------------------------------------------------------------------------------------------*/
            status = Authorize(tablesListViewModel._ctx);
            if (!questStatusDef.IsSuccess(status))
            {
                status = new questStatus(Severity.Success);
                tablesListViewModel.questStatus = status;
                return Json(tablesListViewModel, JsonRequestBehavior.AllowGet);
            }

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Get list of requisitions for this user.
             *---------------------------------------------------------------------------------------------------------------------------------*/
            DatabaseId databaseId = new DatabaseId(tablesListViewModel.DatabaseId);
            Quest.MasterPricing.Database.Models.TablesListViewModel tablesetsListViewModelNEW = null;
            TablesListModeler tablesListModeler = new TablesListModeler(this.Request, this.UserSession);
            status = tablesListModeler.List(databaseId, out tablesetsListViewModelNEW);
            if (!questStatusDef.IsSuccess(status))
            {
                status = new questStatus(Severity.Success);
                tablesListViewModel.questStatus = status;
                return Json(tablesListViewModel, JsonRequestBehavior.AllowGet);
            }

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Return view
             *---------------------------------------------------------------------------------------------------------------------------------*/
            status = new questStatus(Severity.Success);
            tablesetsListViewModelNEW.questStatus = status;
            return Json(tablesetsListViewModelNEW, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult Next(Quest.MasterPricing.Database.Models.TablesListViewModel tablesListViewModel)
        {
            questStatus status = null;


            /*----------------------------------------------------------------------------------------------------------------------------------
             * Log Operation
             *---------------------------------------------------------------------------------------------------------------------------------*/
            status = LogOperation();
            if (!questStatusDef.IsSuccess(status))
            {
                status = new questStatus(Severity.Success);
                tablesListViewModel.questStatus = status;
                return Json(tablesListViewModel, JsonRequestBehavior.AllowGet);
            }

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Authorize
             *---------------------------------------------------------------------------------------------------------------------------------*/
            status = Authorize(tablesListViewModel._ctx);
            if (!questStatusDef.IsSuccess(status))
            {
                status = new questStatus(Severity.Success);
                tablesListViewModel.questStatus = status;
                return Json(tablesListViewModel, JsonRequestBehavior.AllowGet);
            }

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Get list of requisitions for this user.
             *---------------------------------------------------------------------------------------------------------------------------------*/
            DatabaseId databaseId = new DatabaseId(tablesListViewModel.DatabaseId);
            Quest.MasterPricing.Database.Models.TablesListViewModel tablesetsListViewModelNEW = null;
            TablesListModeler tablesListModeler = new TablesListModeler(this.Request, this.UserSession);
            status = tablesListModeler.List(databaseId, out tablesetsListViewModelNEW);
            if (!questStatusDef.IsSuccess(status))
            {
                status = new questStatus(Severity.Success);
                tablesListViewModel.questStatus = status;
                return Json(tablesListViewModel, JsonRequestBehavior.AllowGet);
            }

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Return view
             *---------------------------------------------------------------------------------------------------------------------------------*/
            status = new questStatus(Severity.Success);
            tablesetsListViewModelNEW.questStatus = status;
            return Json(tablesetsListViewModelNEW, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult Last(Quest.MasterPricing.Database.Models.TablesListViewModel tablesListViewModel)
        {
            questStatus status = null;

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Log Operation
             *---------------------------------------------------------------------------------------------------------------------------------*/
            status = LogOperation();
            if (!questStatusDef.IsSuccess(status))
            {
                status = new questStatus(Severity.Success);
                tablesListViewModel.questStatus = status;
                return Json(tablesListViewModel, JsonRequestBehavior.AllowGet);
            }

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Authorize
             *---------------------------------------------------------------------------------------------------------------------------------*/
            status = Authorize(tablesListViewModel._ctx);
            if (!questStatusDef.IsSuccess(status))
            {
                status = new questStatus(Severity.Success);
                tablesListViewModel.questStatus = status;
                return Json(tablesListViewModel, JsonRequestBehavior.AllowGet);
            }

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Get list of requisitions for this user.
             *---------------------------------------------------------------------------------------------------------------------------------*/
            DatabaseId databaseId = new DatabaseId(tablesListViewModel.DatabaseId);
            Quest.MasterPricing.Database.Models.TablesListViewModel tablesetsListViewModelNEW = null;
            TablesListModeler tablesListModeler = new TablesListModeler(this.Request, this.UserSession);
            status = tablesListModeler.List(databaseId, out tablesetsListViewModelNEW);
            if (!questStatusDef.IsSuccess(status))
            {
                status = new questStatus(Severity.Success);
                tablesListViewModel.questStatus = status;
                return Json(tablesListViewModel, JsonRequestBehavior.AllowGet);
            }

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Return view
             *---------------------------------------------------------------------------------------------------------------------------------*/
            status = new questStatus(Severity.Success);
            tablesetsListViewModelNEW.questStatus = status;
            return Json(tablesetsListViewModelNEW, JsonRequestBehavior.AllowGet);
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
