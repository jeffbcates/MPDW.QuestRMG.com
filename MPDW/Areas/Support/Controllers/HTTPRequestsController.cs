﻿using System;
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
    public class HTTPRequestsController : SupportBaseController
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
                // TODO
                throw new Exception("LogOperation failed");
            }

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Authorize
             *---------------------------------------------------------------------------------------------------------------------------------*/
            status = Authorize(baseUserSessionViewModel._ctx);
            if (!questStatusDef.IsSuccess(status))
            {
                // TODO
                throw new Exception("Authorize failed");
            }

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Return view.
             *---------------------------------------------------------------------------------------------------------------------------------*/
            HTTPRequestsViewModel httpRequestsViewModel = new HTTPRequestsViewModel(this.UserSession, baseUserSessionViewModel);
            return View(httpRequestsViewModel);
        }

        [HttpGet]
        public ActionResult List(HTTPRequestsListViewModel httpRequestsListViewModel)
        {
            questStatus status = null;

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Log Operation
             *---------------------------------------------------------------------------------------------------------------------------------*/
            status = LogOperation();
            if (!questStatusDef.IsSuccess(status))
            {
                httpRequestsListViewModel.questStatus = status;
                return Json(httpRequestsListViewModel, JsonRequestBehavior.AllowGet);
            }

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Authorize
             *---------------------------------------------------------------------------------------------------------------------------------*/
            status = Authorize(httpRequestsListViewModel._ctx);
            if (!questStatusDef.IsSuccess(status))
            {
                httpRequestsListViewModel.questStatus = status;
                return Json(httpRequestsListViewModel, JsonRequestBehavior.AllowGet);
            }

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Get list of items.
             *---------------------------------------------------------------------------------------------------------------------------------*/
            HTTPRequestsListViewModel tablesetsListViewModelNEW = null;
            HTTPRequestsListModeler httpRequestsListModeler = new HTTPRequestsListModeler(this.Request, this.UserSession);
            status = httpRequestsListModeler.List(out tablesetsListViewModelNEW);
            if (!questStatusDef.IsSuccess(status))
            {
                httpRequestsListViewModel.questStatus = status;
                return Json(httpRequestsListViewModel, JsonRequestBehavior.AllowGet);
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
        public ActionResult First(HTTPRequestsListViewModel httpRequestsListViewModel)
        {
            questStatus status = null;

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Log Operation
             *---------------------------------------------------------------------------------------------------------------------------------*/
            status = LogOperation();
            if (!questStatusDef.IsSuccess(status))
            {
                status = new questStatus(Severity.Success);
                httpRequestsListViewModel.questStatus = status;
                return Json(httpRequestsListViewModel, JsonRequestBehavior.AllowGet);
            }

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Authorize
             *---------------------------------------------------------------------------------------------------------------------------------*/
            status = Authorize(httpRequestsListViewModel._ctx);
            if (!questStatusDef.IsSuccess(status))
            {
                status = new questStatus(Severity.Success);
                httpRequestsListViewModel.questStatus = status;
                return Json(httpRequestsListViewModel, JsonRequestBehavior.AllowGet);
            }

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Get list of items.
             *---------------------------------------------------------------------------------------------------------------------------------*/
            // TODO.
            HTTPRequestsListViewModel tablesetsListViewModelNEW = null;
            HTTPRequestsListModeler usersListModeler = new HTTPRequestsListModeler(this.Request, this.UserSession);
            status = usersListModeler.List(out tablesetsListViewModelNEW);
            if (!questStatusDef.IsSuccess(status))
            {
                status = new questStatus(Severity.Success);
                httpRequestsListViewModel.questStatus = status;
                return Json(httpRequestsListViewModel, JsonRequestBehavior.AllowGet);
            }

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Return view
             *---------------------------------------------------------------------------------------------------------------------------------*/
            status = new questStatus(Severity.Success);
            tablesetsListViewModelNEW.questStatus = status;
            return Json(tablesetsListViewModelNEW, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult Prev(HTTPRequestsListViewModel httpRequestsListViewModel)
        {
            questStatus status = null;

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Log Operation
             *---------------------------------------------------------------------------------------------------------------------------------*/
            status = LogOperation();
            if (!questStatusDef.IsSuccess(status))
            {
                status = new questStatus(Severity.Success);
                httpRequestsListViewModel.questStatus = status;
                return Json(httpRequestsListViewModel, JsonRequestBehavior.AllowGet);
            }

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Authorize
             *---------------------------------------------------------------------------------------------------------------------------------*/
            status = Authorize(httpRequestsListViewModel._ctx);
            if (!questStatusDef.IsSuccess(status))
            {
                status = new questStatus(Severity.Success);
                httpRequestsListViewModel.questStatus = status;
                return Json(httpRequestsListViewModel, JsonRequestBehavior.AllowGet);
            }

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Get list of items.
             *---------------------------------------------------------------------------------------------------------------------------------*/
            // TODO.
            HTTPRequestsListViewModel tablesetsListViewModelNEW = null;
            HTTPRequestsListModeler usersListModeler = new HTTPRequestsListModeler(this.Request, this.UserSession);
            status = usersListModeler.List(out tablesetsListViewModelNEW);
            if (!questStatusDef.IsSuccess(status))
            {
                status = new questStatus(Severity.Success);
                httpRequestsListViewModel.questStatus = status;
                return Json(httpRequestsListViewModel, JsonRequestBehavior.AllowGet);
            }

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Return view
             *---------------------------------------------------------------------------------------------------------------------------------*/
            status = new questStatus(Severity.Success);
            tablesetsListViewModelNEW.questStatus = status;
            return Json(tablesetsListViewModelNEW, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult PageNum(HTTPRequestsListViewModel httpRequestsListViewModel)
        {
            questStatus status = null;

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Log Operation
             *---------------------------------------------------------------------------------------------------------------------------------*/
            status = LogOperation();
            if (!questStatusDef.IsSuccess(status))
            {
                status = new questStatus(Severity.Success);
                httpRequestsListViewModel.questStatus = status;
                return Json(httpRequestsListViewModel, JsonRequestBehavior.AllowGet);
            }

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Authorize
             *---------------------------------------------------------------------------------------------------------------------------------*/
            status = Authorize(httpRequestsListViewModel._ctx);
            if (!questStatusDef.IsSuccess(status))
            {
                status = new questStatus(Severity.Success);
                httpRequestsListViewModel.questStatus = status;
                return Json(httpRequestsListViewModel, JsonRequestBehavior.AllowGet);
            }

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Get list of items.
             *---------------------------------------------------------------------------------------------------------------------------------*/
            // TODO.
            HTTPRequestsListViewModel tablesetsListViewModelNEW = null;
            HTTPRequestsListModeler usersListModeler = new HTTPRequestsListModeler(this.Request, this.UserSession);
            status = usersListModeler.List(out tablesetsListViewModelNEW);
            if (!questStatusDef.IsSuccess(status))
            {
                status = new questStatus(Severity.Success);
                httpRequestsListViewModel.questStatus = status;
                return Json(httpRequestsListViewModel, JsonRequestBehavior.AllowGet);
            }

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Return view
             *---------------------------------------------------------------------------------------------------------------------------------*/
            status = new questStatus(Severity.Success);
            tablesetsListViewModelNEW.questStatus = status;
            return Json(tablesetsListViewModelNEW, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult Next(HTTPRequestsListViewModel httpRequestsListViewModel)
        {
            questStatus status = null;


            /*----------------------------------------------------------------------------------------------------------------------------------
             * Log Operation
             *---------------------------------------------------------------------------------------------------------------------------------*/
            status = LogOperation();
            if (!questStatusDef.IsSuccess(status))
            {
                status = new questStatus(Severity.Success);
                httpRequestsListViewModel.questStatus = status;
                return Json(httpRequestsListViewModel, JsonRequestBehavior.AllowGet);
            }

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Authorize
             *---------------------------------------------------------------------------------------------------------------------------------*/
            status = Authorize(httpRequestsListViewModel._ctx);
            if (!questStatusDef.IsSuccess(status))
            {
                status = new questStatus(Severity.Success);
                httpRequestsListViewModel.questStatus = status;
                return Json(httpRequestsListViewModel, JsonRequestBehavior.AllowGet);
            }

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Get list of items.
             *---------------------------------------------------------------------------------------------------------------------------------*/
            // TODO.
            HTTPRequestsListViewModel tablesetsListViewModelNEW = null;
            HTTPRequestsListModeler usersListModeler = new HTTPRequestsListModeler(this.Request, this.UserSession);
            status = usersListModeler.List(out tablesetsListViewModelNEW);
            if (!questStatusDef.IsSuccess(status))
            {
                status = new questStatus(Severity.Success);
                httpRequestsListViewModel.questStatus = status;
                return Json(httpRequestsListViewModel, JsonRequestBehavior.AllowGet);
            }

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Return view
             *---------------------------------------------------------------------------------------------------------------------------------*/
            status = new questStatus(Severity.Success);
            tablesetsListViewModelNEW.questStatus = status;
            return Json(tablesetsListViewModelNEW, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult Last(HTTPRequestsListViewModel httpRequestsListViewModel)
        {
            questStatus status = null;

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Log Operation
             *---------------------------------------------------------------------------------------------------------------------------------*/
            status = LogOperation();
            if (!questStatusDef.IsSuccess(status))
            {
                status = new questStatus(Severity.Success);
                httpRequestsListViewModel.questStatus = status;
                return Json(httpRequestsListViewModel, JsonRequestBehavior.AllowGet);
            }

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Authorize
             *---------------------------------------------------------------------------------------------------------------------------------*/
            status = Authorize(httpRequestsListViewModel._ctx);
            if (!questStatusDef.IsSuccess(status))
            {
                status = new questStatus(Severity.Success);
                httpRequestsListViewModel.questStatus = status;
                return Json(httpRequestsListViewModel, JsonRequestBehavior.AllowGet);
            }

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Get list of items.
             *---------------------------------------------------------------------------------------------------------------------------------*/
            // TODO.
            HTTPRequestsListViewModel tablesetsListViewModelNEW = null;
            HTTPRequestsListModeler usersListModeler = new HTTPRequestsListModeler(this.Request, this.UserSession);
            status = usersListModeler.List(out tablesetsListViewModelNEW);
            if (!questStatusDef.IsSuccess(status))
            {
                status = new questStatus(Severity.Success);
                httpRequestsListViewModel.questStatus = status;
                return Json(httpRequestsListViewModel, JsonRequestBehavior.AllowGet);
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
