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
    public class SessionsController : AdminBaseController
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
        public ActionResult Index(UserEditorViewModel viewModel)
        {
            questStatus status = null;

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Log Operation
             *---------------------------------------------------------------------------------------------------------------------------------*/
            status = LogOperation();
            if (!questStatusDef.IsSuccess(status))
            {
                viewModel.questStatus = status;
                return (View("Index", viewModel));
            }

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Authorize
             *---------------------------------------------------------------------------------------------------------------------------------*/
            status = Authorize(viewModel._ctx);
            if (!questStatusDef.IsSuccess(status))
            {
                viewModel.questStatus = status;
                return (View("Index", viewModel));
            }

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Get user sessions.
             *---------------------------------------------------------------------------------------------------------------------------------*/
             // TODO:

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Return view.
             *---------------------------------------------------------------------------------------------------------------------------------*/
            SessionsListViewModel sessionsListViewModel = new SessionsListViewModel(this.UserSession, viewModel);
            return View(sessionsListViewModel);
        }
        [HttpGet]
        public ActionResult List(SessionsListViewModel viewModel)
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
             * Get list of items.
             *---------------------------------------------------------------------------------------------------------------------------------*/
            SessionsListViewModel sessionsListViewModelNew = null;
            SessionsListModeler sessionsListModeler = new SessionsListModeler(this.Request, this.UserSession);
            status = sessionsListModeler.List(out sessionsListViewModelNew);
            if (!questStatusDef.IsSuccess(status))
            {
                viewModel.questStatus = status;
                return Json(viewModel, JsonRequestBehavior.AllowGet);
            }

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Return view
             *---------------------------------------------------------------------------------------------------------------------------------*/
            status = new questStatus(Severity.Success);
            sessionsListViewModelNew.questStatus = status;
            return Json(sessionsListViewModelNew, JsonRequestBehavior.AllowGet);
        }

        #region Paging
        //----------------------------------------------------------------------------------------------------------------------------------
        // Paging
        //----------------------------------------------------------------------------------------------------------------------------------
        [HttpGet]
        public ActionResult First(SessionsListViewModel usersListViewModel)
        {
            questStatus status = null;

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Log Operation
             *---------------------------------------------------------------------------------------------------------------------------------*/
            status = LogOperation();
            if (!questStatusDef.IsSuccess(status))
            {
                status = new questStatus(Severity.Success);
                usersListViewModel.questStatus = status;
                return Json(usersListViewModel, JsonRequestBehavior.AllowGet);
            }

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Authorize
             *---------------------------------------------------------------------------------------------------------------------------------*/
            status = Authorize(usersListViewModel._ctx);
            if (!questStatusDef.IsSuccess(status))
            {
                status = new questStatus(Severity.Success);
                usersListViewModel.questStatus = status;
                return Json(usersListViewModel, JsonRequestBehavior.AllowGet);
            }

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Get list of items.
             *---------------------------------------------------------------------------------------------------------------------------------*/
            // TODO.
            SessionsListViewModel sessionsListViewModelNew = null;
            SessionsListModeler usersListModeler = new SessionsListModeler(this.Request, this.UserSession);
            status = usersListModeler.List(out sessionsListViewModelNew);
            if (!questStatusDef.IsSuccess(status))
            {
                status = new questStatus(Severity.Success);
                usersListViewModel.questStatus = status;
                return Json(usersListViewModel, JsonRequestBehavior.AllowGet);
            }

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Return view
             *---------------------------------------------------------------------------------------------------------------------------------*/
            status = new questStatus(Severity.Success);
            sessionsListViewModelNew.questStatus = status;
            return Json(sessionsListViewModelNew, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult Prev(SessionsListViewModel usersListViewModel)
        {
            questStatus status = null;

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Log Operation
             *---------------------------------------------------------------------------------------------------------------------------------*/
            status = LogOperation();
            if (!questStatusDef.IsSuccess(status))
            {
                status = new questStatus(Severity.Success);
                usersListViewModel.questStatus = status;
                return Json(usersListViewModel, JsonRequestBehavior.AllowGet);
            }

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Authorize
             *---------------------------------------------------------------------------------------------------------------------------------*/
            status = Authorize(usersListViewModel._ctx);
            if (!questStatusDef.IsSuccess(status))
            {
                status = new questStatus(Severity.Success);
                usersListViewModel.questStatus = status;
                return Json(usersListViewModel, JsonRequestBehavior.AllowGet);
            }

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Get list of items.
             *---------------------------------------------------------------------------------------------------------------------------------*/
            // TODO.
            SessionsListViewModel sessionsListViewModelNew = null;
            SessionsListModeler usersListModeler = new SessionsListModeler(this.Request, this.UserSession);
            status = usersListModeler.List(out sessionsListViewModelNew);
            if (!questStatusDef.IsSuccess(status))
            {
                status = new questStatus(Severity.Success);
                usersListViewModel.questStatus = status;
                return Json(usersListViewModel, JsonRequestBehavior.AllowGet);
            }

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Return view
             *---------------------------------------------------------------------------------------------------------------------------------*/
            status = new questStatus(Severity.Success);
            sessionsListViewModelNew.questStatus = status;
            return Json(sessionsListViewModelNew, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult PageNum(SessionsListViewModel usersListViewModel)
        {
            questStatus status = null;

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Log Operation
             *---------------------------------------------------------------------------------------------------------------------------------*/
            status = LogOperation();
            if (!questStatusDef.IsSuccess(status))
            {
                status = new questStatus(Severity.Success);
                usersListViewModel.questStatus = status;
                return Json(usersListViewModel, JsonRequestBehavior.AllowGet);
            }

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Authorize
             *---------------------------------------------------------------------------------------------------------------------------------*/
            status = Authorize(usersListViewModel._ctx);
            if (!questStatusDef.IsSuccess(status))
            {
                status = new questStatus(Severity.Success);
                usersListViewModel.questStatus = status;
                return Json(usersListViewModel, JsonRequestBehavior.AllowGet);
            }

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Get list of items.
             *---------------------------------------------------------------------------------------------------------------------------------*/
            // TODO.
            SessionsListViewModel sessionsListViewModelNew = null;
            SessionsListModeler usersListModeler = new SessionsListModeler(this.Request, this.UserSession);
            status = usersListModeler.List(out sessionsListViewModelNew);
            if (!questStatusDef.IsSuccess(status))
            {
                status = new questStatus(Severity.Success);
                usersListViewModel.questStatus = status;
                return Json(usersListViewModel, JsonRequestBehavior.AllowGet);
            }

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Return view
             *---------------------------------------------------------------------------------------------------------------------------------*/
            status = new questStatus(Severity.Success);
            sessionsListViewModelNew.questStatus = status;
            return Json(sessionsListViewModelNew, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult Next(SessionsListViewModel usersListViewModel)
        {
            questStatus status = null;


            /*----------------------------------------------------------------------------------------------------------------------------------
             * Log Operation
             *---------------------------------------------------------------------------------------------------------------------------------*/
            status = LogOperation();
            if (!questStatusDef.IsSuccess(status))
            {
                status = new questStatus(Severity.Success);
                usersListViewModel.questStatus = status;
                return Json(usersListViewModel, JsonRequestBehavior.AllowGet);
            }

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Authorize
             *---------------------------------------------------------------------------------------------------------------------------------*/
            status = Authorize(usersListViewModel._ctx);
            if (!questStatusDef.IsSuccess(status))
            {
                status = new questStatus(Severity.Success);
                usersListViewModel.questStatus = status;
                return Json(usersListViewModel, JsonRequestBehavior.AllowGet);
            }

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Get list of items.
             *---------------------------------------------------------------------------------------------------------------------------------*/
            // TODO.
            SessionsListViewModel sessionsListViewModelNew = null;
            SessionsListModeler usersListModeler = new SessionsListModeler(this.Request, this.UserSession);
            status = usersListModeler.List(out sessionsListViewModelNew);
            if (!questStatusDef.IsSuccess(status))
            {
                status = new questStatus(Severity.Success);
                usersListViewModel.questStatus = status;
                return Json(usersListViewModel, JsonRequestBehavior.AllowGet);
            }

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Return view
             *---------------------------------------------------------------------------------------------------------------------------------*/
            status = new questStatus(Severity.Success);
            sessionsListViewModelNew.questStatus = status;
            return Json(sessionsListViewModelNew, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult Last(SessionsListViewModel usersListViewModel)
        {
            questStatus status = null;

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Log Operation
             *---------------------------------------------------------------------------------------------------------------------------------*/
            status = LogOperation();
            if (!questStatusDef.IsSuccess(status))
            {
                status = new questStatus(Severity.Success);
                usersListViewModel.questStatus = status;
                return Json(usersListViewModel, JsonRequestBehavior.AllowGet);
            }

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Authorize
             *---------------------------------------------------------------------------------------------------------------------------------*/
            status = Authorize(usersListViewModel._ctx);
            if (!questStatusDef.IsSuccess(status))
            {
                status = new questStatus(Severity.Success);
                usersListViewModel.questStatus = status;
                return Json(usersListViewModel, JsonRequestBehavior.AllowGet);
            }

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Get list of items.
             *---------------------------------------------------------------------------------------------------------------------------------*/
            // TODO.
            SessionsListViewModel sessionsListViewModelNew = null;
            SessionsListModeler usersListModeler = new SessionsListModeler(this.Request, this.UserSession);
            status = usersListModeler.List(out sessionsListViewModelNew);
            if (!questStatusDef.IsSuccess(status))
            {
                status = new questStatus(Severity.Success);
                usersListViewModel.questStatus = status;
                return Json(usersListViewModel, JsonRequestBehavior.AllowGet);
            }

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Return view
             *---------------------------------------------------------------------------------------------------------------------------------*/
            status = new questStatus(Severity.Success);
            sessionsListViewModelNew.questStatus = status;
            return Json(sessionsListViewModelNew, JsonRequestBehavior.AllowGet);
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
