using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using Quest.Util.Status;
using Quest.Util.Buffer;
using Quest.MPDW.Models;
using Quest.MPDW.Admin.Models;
using Quest.MPDW.Admin.Modelers;


namespace Quest.MPDW.Admin
{
    public class PrivilegesController : AdminBaseController
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
            PrivilegesListViewModel tablesetsListViewModel = new PrivilegesListViewModel(this.UserSession, baseUserSessionViewModel);
            return View(tablesetsListViewModel);
        }
        [HttpGet]
        public ActionResult List(PrivilegesListViewModel tablesetsListViewModel)
        {
            questStatus status = null;

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Log Operation
             *---------------------------------------------------------------------------------------------------------------------------------*/
            status = LogOperation();
            if (!questStatusDef.IsSuccess(status))
            {
                tablesetsListViewModel.questStatus = status;
                return Json(tablesetsListViewModel, JsonRequestBehavior.AllowGet);
            }

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Authorize
             *---------------------------------------------------------------------------------------------------------------------------------*/
            status = Authorize(tablesetsListViewModel._ctx);
            if (!questStatusDef.IsSuccess(status))
            {
                tablesetsListViewModel.questStatus = status;
                return Json(tablesetsListViewModel, JsonRequestBehavior.AllowGet);
            }

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Get list of items.
             *---------------------------------------------------------------------------------------------------------------------------------*/
            PrivilegesListViewModel tablesetsListViewModelNEW = null;
            PrivilegesListModeler privilegeListModeler = new PrivilegesListModeler(this.Request, this.UserSession);
            status = privilegeListModeler.List(out tablesetsListViewModelNEW);
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
        public ActionResult First(PrivilegesListViewModel privilegesListViewModel)
        {
            questStatus status = null;

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Log Operation
             *---------------------------------------------------------------------------------------------------------------------------------*/
            status = LogOperation();
            if (!questStatusDef.IsSuccess(status))
            {
                status = new questStatus(Severity.Success);
                privilegesListViewModel.questStatus = status;
                return Json(privilegesListViewModel, JsonRequestBehavior.AllowGet);
            }

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Authorize
             *---------------------------------------------------------------------------------------------------------------------------------*/
            status = Authorize(privilegesListViewModel._ctx);
            if (!questStatusDef.IsSuccess(status))
            {
                status = new questStatus(Severity.Success);
                privilegesListViewModel.questStatus = status;
                return Json(privilegesListViewModel, JsonRequestBehavior.AllowGet);
            }

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Get list of items.
             *---------------------------------------------------------------------------------------------------------------------------------*/
            // TODO.
            PrivilegesListViewModel tablesetsListViewModelNEW = null;
            PrivilegesListModeler privilegesListModeler = new PrivilegesListModeler(this.Request, this.UserSession);
            status = privilegesListModeler.List(out tablesetsListViewModelNEW);
            if (!questStatusDef.IsSuccess(status))
            {
                status = new questStatus(Severity.Success);
                privilegesListViewModel.questStatus = status;
                return Json(privilegesListViewModel, JsonRequestBehavior.AllowGet);
            }

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Return view
             *---------------------------------------------------------------------------------------------------------------------------------*/
            status = new questStatus(Severity.Success);
            tablesetsListViewModelNEW.questStatus = status;
            return Json(tablesetsListViewModelNEW, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult Prev(PrivilegesListViewModel privilegesListViewModel)
        {
            questStatus status = null;

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Log Operation
             *---------------------------------------------------------------------------------------------------------------------------------*/
            status = LogOperation();
            if (!questStatusDef.IsSuccess(status))
            {
                status = new questStatus(Severity.Success);
                privilegesListViewModel.questStatus = status;
                return Json(privilegesListViewModel, JsonRequestBehavior.AllowGet);
            }

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Authorize
             *---------------------------------------------------------------------------------------------------------------------------------*/
            status = Authorize(privilegesListViewModel._ctx);
            if (!questStatusDef.IsSuccess(status))
            {
                status = new questStatus(Severity.Success);
                privilegesListViewModel.questStatus = status;
                return Json(privilegesListViewModel, JsonRequestBehavior.AllowGet);
            }

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Get list of items.
             *---------------------------------------------------------------------------------------------------------------------------------*/
            // TODO.
            PrivilegesListViewModel tablesetsListViewModelNEW = null;
            PrivilegesListModeler privilegesListModeler = new PrivilegesListModeler(this.Request, this.UserSession);
            status = privilegesListModeler.List(out tablesetsListViewModelNEW);
            if (!questStatusDef.IsSuccess(status))
            {
                status = new questStatus(Severity.Success);
                privilegesListViewModel.questStatus = status;
                return Json(privilegesListViewModel, JsonRequestBehavior.AllowGet);
            }

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Return view
             *---------------------------------------------------------------------------------------------------------------------------------*/
            status = new questStatus(Severity.Success);
            tablesetsListViewModelNEW.questStatus = status;
            return Json(tablesetsListViewModelNEW, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult PageNum(PrivilegesListViewModel privilegesListViewModel)
        {
            questStatus status = null;

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Log Operation
             *---------------------------------------------------------------------------------------------------------------------------------*/
            status = LogOperation();
            if (!questStatusDef.IsSuccess(status))
            {
                status = new questStatus(Severity.Success);
                privilegesListViewModel.questStatus = status;
                return Json(privilegesListViewModel, JsonRequestBehavior.AllowGet);
            }

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Authorize
             *---------------------------------------------------------------------------------------------------------------------------------*/
            status = Authorize(privilegesListViewModel._ctx);
            if (!questStatusDef.IsSuccess(status))
            {
                status = new questStatus(Severity.Success);
                privilegesListViewModel.questStatus = status;
                return Json(privilegesListViewModel, JsonRequestBehavior.AllowGet);
            }

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Get list of items.
             *---------------------------------------------------------------------------------------------------------------------------------*/
            // TODO.
            PrivilegesListViewModel tablesetsListViewModelNEW = null;
            PrivilegesListModeler privilegesListModeler = new PrivilegesListModeler(this.Request, this.UserSession);
            status = privilegesListModeler.List(out tablesetsListViewModelNEW);
            if (!questStatusDef.IsSuccess(status))
            {
                status = new questStatus(Severity.Success);
                privilegesListViewModel.questStatus = status;
                return Json(privilegesListViewModel, JsonRequestBehavior.AllowGet);
            }

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Return view
             *---------------------------------------------------------------------------------------------------------------------------------*/
            status = new questStatus(Severity.Success);
            tablesetsListViewModelNEW.questStatus = status;
            return Json(tablesetsListViewModelNEW, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult Next(PrivilegesListViewModel privilegesListViewModel)
        {
            questStatus status = null;


            /*----------------------------------------------------------------------------------------------------------------------------------
             * Log Operation
             *---------------------------------------------------------------------------------------------------------------------------------*/
            status = LogOperation();
            if (!questStatusDef.IsSuccess(status))
            {
                status = new questStatus(Severity.Success);
                privilegesListViewModel.questStatus = status;
                return Json(privilegesListViewModel, JsonRequestBehavior.AllowGet);
            }

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Authorize
             *---------------------------------------------------------------------------------------------------------------------------------*/
            status = Authorize(privilegesListViewModel._ctx);
            if (!questStatusDef.IsSuccess(status))
            {
                status = new questStatus(Severity.Success);
                privilegesListViewModel.questStatus = status;
                return Json(privilegesListViewModel, JsonRequestBehavior.AllowGet);
            }

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Get list of items.
             *---------------------------------------------------------------------------------------------------------------------------------*/
            // TODO.
            PrivilegesListViewModel tablesetsListViewModelNEW = null;
            PrivilegesListModeler privilegesListModeler = new PrivilegesListModeler(this.Request, this.UserSession);
            status = privilegesListModeler.List(out tablesetsListViewModelNEW);
            if (!questStatusDef.IsSuccess(status))
            {
                status = new questStatus(Severity.Success);
                privilegesListViewModel.questStatus = status;
                return Json(privilegesListViewModel, JsonRequestBehavior.AllowGet);
            }

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Return view
             *---------------------------------------------------------------------------------------------------------------------------------*/
            status = new questStatus(Severity.Success);
            tablesetsListViewModelNEW.questStatus = status;
            return Json(tablesetsListViewModelNEW, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult Last(PrivilegesListViewModel privilegesListViewModel)
        {
            questStatus status = null;

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Log Operation
             *---------------------------------------------------------------------------------------------------------------------------------*/
            status = LogOperation();
            if (!questStatusDef.IsSuccess(status))
            {
                status = new questStatus(Severity.Success);
                privilegesListViewModel.questStatus = status;
                return Json(privilegesListViewModel, JsonRequestBehavior.AllowGet);
            }

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Authorize
             *---------------------------------------------------------------------------------------------------------------------------------*/
            status = Authorize(privilegesListViewModel._ctx);
            if (!questStatusDef.IsSuccess(status))
            {
                status = new questStatus(Severity.Success);
                privilegesListViewModel.questStatus = status;
                return Json(privilegesListViewModel, JsonRequestBehavior.AllowGet);
            }

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Get list of items.
             *---------------------------------------------------------------------------------------------------------------------------------*/
            // TODO.
            PrivilegesListViewModel tablesetsListViewModelNEW = null;
            PrivilegesListModeler privilegesListModeler = new PrivilegesListModeler(this.Request, this.UserSession);
            status = privilegesListModeler.List(out tablesetsListViewModelNEW);
            if (!questStatusDef.IsSuccess(status))
            {
                status = new questStatus(Severity.Success);
                privilegesListViewModel.questStatus = status;
                return Json(privilegesListViewModel, JsonRequestBehavior.AllowGet);
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
