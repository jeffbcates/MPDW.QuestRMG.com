using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using Quest.Util.Status;
using Quest.Util.Buffer;
using Quest.MPDW.Models;
using Quest.MasterPricing.Setup.Models;
using Quest.MasterPricing.Setup.Modelers;


namespace Quest.MasterPricing.Setup
{
    public class DatabasesController : SetupBaseController
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
            DatabasesListViewModel tablesetsListViewModel = new DatabasesListViewModel(this.UserSession, baseUserSessionViewModel);
            return View(tablesetsListViewModel);
        }
        [HttpGet]
        public ActionResult List(DatabasesListViewModel tablesetsListViewModel)
        {
            questStatus status = null;

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Log Operation
             *---------------------------------------------------------------------------------------------------------------------------------*/
            status = LogOperation();
            if (!questStatusDef.IsSuccess(status))
            {
                ////status = new questStatus(Severity.Success);
                tablesetsListViewModel.questStatus = status;
                return Json(tablesetsListViewModel, JsonRequestBehavior.AllowGet);
            }

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Authorize
             *---------------------------------------------------------------------------------------------------------------------------------*/
            status = Authorize(tablesetsListViewModel._ctx);
            if (!questStatusDef.IsSuccess(status))
            {
                ////status = new questStatus(Severity.Success);
                tablesetsListViewModel.questStatus = status;
                return Json(tablesetsListViewModel, JsonRequestBehavior.AllowGet);
            }

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Get list of items.
             *---------------------------------------------------------------------------------------------------------------------------------*/
            DatabasesListViewModel tablesetsListViewModelNEW = null;
            DatabasesListModeler databaseListModeler = new DatabasesListModeler(this.Request, this.UserSession);
            status = databaseListModeler.List(out tablesetsListViewModelNEW);
            if (!questStatusDef.IsSuccess(status))
            {
                tablesetsListViewModel.questStatus = status;
                return Json(tablesetsListViewModel, JsonRequestBehavior.AllowGet);
            }

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Return view
             *---------------------------------------------------------------------------------------------------------------------------------*/
            status = new questStatus(Severity.Success);
            tablesetsListViewModelNEW.questStatus = status;
            return Json(tablesetsListViewModelNEW, JsonRequestBehavior.AllowGet);
        }

        #region Paging
        //----------------------------------------------------------------------------------------------------------------------------------
        // Paging
        //----------------------------------------------------------------------------------------------------------------------------------
        [HttpGet]
        public ActionResult First(DatabasesListViewModel databasesListViewModel)
        {
            questStatus status = null;

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Log Operation
             *---------------------------------------------------------------------------------------------------------------------------------*/
            status = LogOperation();
            if (!questStatusDef.IsSuccess(status))
            {
                status = new questStatus(Severity.Success);
                databasesListViewModel.questStatus = status;
                return Json(databasesListViewModel, JsonRequestBehavior.AllowGet);
            }

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Authorize
             *---------------------------------------------------------------------------------------------------------------------------------*/
            status = Authorize(databasesListViewModel._ctx);
            if (!questStatusDef.IsSuccess(status))
            {
                status = new questStatus(Severity.Success);
                databasesListViewModel.questStatus = status;
                return Json(databasesListViewModel, JsonRequestBehavior.AllowGet);
            }

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Get list of items.
             *---------------------------------------------------------------------------------------------------------------------------------*/
            DatabasesListViewModel tablesetsListViewModelNEW = null;
            DatabasesListModeler databasesListModeler = new DatabasesListModeler(this.Request, this.UserSession);
            status = databasesListModeler.List(out tablesetsListViewModelNEW);
            if (!questStatusDef.IsSuccess(status))
            {
                status = new questStatus(Severity.Success);
                databasesListViewModel.questStatus = status;
                return Json(databasesListViewModel, JsonRequestBehavior.AllowGet);
            }

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Return view
             *---------------------------------------------------------------------------------------------------------------------------------*/
            status = new questStatus(Severity.Success);
            tablesetsListViewModelNEW.questStatus = status;
            return Json(tablesetsListViewModelNEW, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult Prev(DatabasesListViewModel databasesListViewModel)
        {
            questStatus status = null;

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Log Operation
             *---------------------------------------------------------------------------------------------------------------------------------*/
            status = LogOperation();
            if (!questStatusDef.IsSuccess(status))
            {
                status = new questStatus(Severity.Success);
                databasesListViewModel.questStatus = status;
                return Json(databasesListViewModel, JsonRequestBehavior.AllowGet);
            }

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Authorize
             *---------------------------------------------------------------------------------------------------------------------------------*/
            status = Authorize(databasesListViewModel._ctx);
            if (!questStatusDef.IsSuccess(status))
            {
                status = new questStatus(Severity.Success);
                databasesListViewModel.questStatus = status;
                return Json(databasesListViewModel, JsonRequestBehavior.AllowGet);
            }

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Get list of items.
             *---------------------------------------------------------------------------------------------------------------------------------*/
            // TODO.
            DatabasesListViewModel tablesetsListViewModelNEW = null;
            DatabasesListModeler databasesListModeler = new DatabasesListModeler(this.Request, this.UserSession);
            status = databasesListModeler.List(out tablesetsListViewModelNEW);
            if (!questStatusDef.IsSuccess(status))
            {
                status = new questStatus(Severity.Success);
                databasesListViewModel.questStatus = status;
                return Json(databasesListViewModel, JsonRequestBehavior.AllowGet);
            }

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Return view
             *---------------------------------------------------------------------------------------------------------------------------------*/
            status = new questStatus(Severity.Success);
            tablesetsListViewModelNEW.questStatus = status;
            return Json(tablesetsListViewModelNEW, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult PageNum(DatabasesListViewModel databasesListViewModel)
        {
            questStatus status = null;

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Log Operation
             *---------------------------------------------------------------------------------------------------------------------------------*/
            status = LogOperation();
            if (!questStatusDef.IsSuccess(status))
            {
                status = new questStatus(Severity.Success);
                databasesListViewModel.questStatus = status;
                return Json(databasesListViewModel, JsonRequestBehavior.AllowGet);
            }

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Authorize
             *---------------------------------------------------------------------------------------------------------------------------------*/
            status = Authorize(databasesListViewModel._ctx);
            if (!questStatusDef.IsSuccess(status))
            {
                status = new questStatus(Severity.Success);
                databasesListViewModel.questStatus = status;
                return Json(databasesListViewModel, JsonRequestBehavior.AllowGet);
            }

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Get list of items.
             *---------------------------------------------------------------------------------------------------------------------------------*/
            // TODO.
            DatabasesListViewModel tablesetsListViewModelNEW = null;
            DatabasesListModeler databasesListModeler = new DatabasesListModeler(this.Request, this.UserSession);
            status = databasesListModeler.List(out tablesetsListViewModelNEW);
            if (!questStatusDef.IsSuccess(status))
            {
                status = new questStatus(Severity.Success);
                databasesListViewModel.questStatus = status;
                return Json(databasesListViewModel, JsonRequestBehavior.AllowGet);
            }

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Return view
             *---------------------------------------------------------------------------------------------------------------------------------*/
            status = new questStatus(Severity.Success);
            tablesetsListViewModelNEW.questStatus = status;
            return Json(tablesetsListViewModelNEW, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult Next(DatabasesListViewModel databasesListViewModel)
        {
            questStatus status = null;


            /*----------------------------------------------------------------------------------------------------------------------------------
             * Log Operation
             *---------------------------------------------------------------------------------------------------------------------------------*/
            status = LogOperation();
            if (!questStatusDef.IsSuccess(status))
            {
                status = new questStatus(Severity.Success);
                databasesListViewModel.questStatus = status;
                return Json(databasesListViewModel, JsonRequestBehavior.AllowGet);
            }

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Authorize
             *---------------------------------------------------------------------------------------------------------------------------------*/
            status = Authorize(databasesListViewModel._ctx);
            if (!questStatusDef.IsSuccess(status))
            {
                status = new questStatus(Severity.Success);
                databasesListViewModel.questStatus = status;
                return Json(databasesListViewModel, JsonRequestBehavior.AllowGet);
            }

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Get list of items.
             *---------------------------------------------------------------------------------------------------------------------------------*/
            // TODO.
            DatabasesListViewModel tablesetsListViewModelNEW = null;
            DatabasesListModeler databasesListModeler = new DatabasesListModeler(this.Request, this.UserSession);
            status = databasesListModeler.List(out tablesetsListViewModelNEW);
            if (!questStatusDef.IsSuccess(status))
            {
                status = new questStatus(Severity.Success);
                databasesListViewModel.questStatus = status;
                return Json(databasesListViewModel, JsonRequestBehavior.AllowGet);
            }

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Return view
             *---------------------------------------------------------------------------------------------------------------------------------*/
            status = new questStatus(Severity.Success);
            tablesetsListViewModelNEW.questStatus = status;
            return Json(tablesetsListViewModelNEW, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult Last(DatabasesListViewModel databasesListViewModel)
        {
            questStatus status = null;

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Log Operation
             *---------------------------------------------------------------------------------------------------------------------------------*/
            status = LogOperation();
            if (!questStatusDef.IsSuccess(status))
            {
                status = new questStatus(Severity.Success);
                databasesListViewModel.questStatus = status;
                return Json(databasesListViewModel, JsonRequestBehavior.AllowGet);
            }

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Authorize
             *---------------------------------------------------------------------------------------------------------------------------------*/
            status = Authorize(databasesListViewModel._ctx);
            if (!questStatusDef.IsSuccess(status))
            {
                status = new questStatus(Severity.Success);
                databasesListViewModel.questStatus = status;
                return Json(databasesListViewModel, JsonRequestBehavior.AllowGet);
            }

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Get list of items.
             *---------------------------------------------------------------------------------------------------------------------------------*/
            // TODO.
            DatabasesListViewModel tablesetsListViewModelNEW = null;
            DatabasesListModeler databasesListModeler = new DatabasesListModeler(this.Request, this.UserSession);
            status = databasesListModeler.List(out tablesetsListViewModelNEW);
            if (!questStatusDef.IsSuccess(status))
            {
                status = new questStatus(Severity.Success);
                databasesListViewModel.questStatus = status;
                return Json(databasesListViewModel, JsonRequestBehavior.AllowGet);
            }

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Return view
             *---------------------------------------------------------------------------------------------------------------------------------*/
            status = new questStatus(Severity.Success);
            tablesetsListViewModelNEW.questStatus = status;
            return Json(tablesetsListViewModelNEW, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #endregion


        #region POST Methods
        /*==================================================================================================================================
         * POST Methods
         *=================================================================================================================================*/
        [HttpPost]
        public ActionResult RefreshSchema(DatabasesListViewModel viewModel)
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
            DatabasesListModeler databasesListModeler = new DatabasesListModeler(this.Request, this.UserSession);
            status = databasesListModeler.RefreshSchema(viewModel);
            if (!questStatusDef.IsSuccess(status))
            {
                viewModel.questStatus = status;
                return Json(viewModel, JsonRequestBehavior.AllowGet);
            }

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Return result.
             *---------------------------------------------------------------------------------------------------------------------------------*/
            status = new questStatus(Severity.Success, "Database scehema(s) successfully refreshed");
            viewModel.questStatus = status;
            return Json(viewModel, JsonRequestBehavior.AllowGet);
        }


        #region CRUD Operations
        //----------------------------------------------------------------------------------------------------------------------------------
        // CRUD Operations
        //----------------------------------------------------------------------------------------------------------------------------------
        #endregion

        #endregion


        #region Private Methods
        /*==================================================================================================================================
         * Private Methods
         *=================================================================================================================================*/
        #endregion
    }
}
