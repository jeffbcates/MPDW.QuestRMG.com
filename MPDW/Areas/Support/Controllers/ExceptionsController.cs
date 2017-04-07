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
    public class ExceptionsController : SupportBaseController
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
            ExceptionsViewModel exceptionsViewModel = new ExceptionsViewModel(this.UserSession, baseUserSessionViewModel);
            return View(exceptionsViewModel);
        }
        [HttpGet]
        public ActionResult List(ExceptionsListViewModel exceptionsListViewModel)
        {
            questStatus status = null;

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Log Operation
             *---------------------------------------------------------------------------------------------------------------------------------*/
            status = LogOperation();
            if (!questStatusDef.IsSuccess(status))
            {
                exceptionsListViewModel.questStatus = status;
                return Json(exceptionsListViewModel, JsonRequestBehavior.AllowGet);
            }

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Authorize
             *---------------------------------------------------------------------------------------------------------------------------------*/
            status = Authorize(exceptionsListViewModel._ctx);
            if (!questStatusDef.IsSuccess(status))
            {
                exceptionsListViewModel.questStatus = status;
                return Json(exceptionsListViewModel, JsonRequestBehavior.AllowGet);
            }

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Get list of items.
             *---------------------------------------------------------------------------------------------------------------------------------*/
            ExceptionsListViewModel tablesetsListViewModelNEW = null;
            ExceptionsListModeler exceptionsListModeler = new ExceptionsListModeler(this.Request, this.UserSession);
            status = exceptionsListModeler.List(out tablesetsListViewModelNEW);
            if (!questStatusDef.IsSuccess(status))
            {
                exceptionsListViewModel.questStatus = status;
                return Json(exceptionsListViewModel, JsonRequestBehavior.AllowGet);
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
        public ActionResult First(ExceptionsListViewModel exceptionsListViewModel)
        {
            questStatus status = null;

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Log Operation
             *---------------------------------------------------------------------------------------------------------------------------------*/
            status = LogOperation();
            if (!questStatusDef.IsSuccess(status))
            {
                status = new questStatus(Severity.Success);
                exceptionsListViewModel.questStatus = status;
                return Json(exceptionsListViewModel, JsonRequestBehavior.AllowGet);
            }

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Authorize
             *---------------------------------------------------------------------------------------------------------------------------------*/
            status = Authorize(exceptionsListViewModel._ctx);
            if (!questStatusDef.IsSuccess(status))
            {
                status = new questStatus(Severity.Success);
                exceptionsListViewModel.questStatus = status;
                return Json(exceptionsListViewModel, JsonRequestBehavior.AllowGet);
            }

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Get list of items.
             *---------------------------------------------------------------------------------------------------------------------------------*/
            // TODO.
            ExceptionsListViewModel tablesetsListViewModelNEW = null;
            ExceptionsListModeler usersListModeler = new ExceptionsListModeler(this.Request, this.UserSession);
            status = usersListModeler.List(out tablesetsListViewModelNEW);
            if (!questStatusDef.IsSuccess(status))
            {
                status = new questStatus(Severity.Success);
                exceptionsListViewModel.questStatus = status;
                return Json(exceptionsListViewModel, JsonRequestBehavior.AllowGet);
            }

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Return view
             *---------------------------------------------------------------------------------------------------------------------------------*/
            status = new questStatus(Severity.Success);
            tablesetsListViewModelNEW.questStatus = status;
            return Json(tablesetsListViewModelNEW, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult Prev(ExceptionsListViewModel exceptionsListViewModel)
        {
            questStatus status = null;

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Log Operation
             *---------------------------------------------------------------------------------------------------------------------------------*/
            status = LogOperation();
            if (!questStatusDef.IsSuccess(status))
            {
                status = new questStatus(Severity.Success);
                exceptionsListViewModel.questStatus = status;
                return Json(exceptionsListViewModel, JsonRequestBehavior.AllowGet);
            }

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Authorize
             *---------------------------------------------------------------------------------------------------------------------------------*/
            status = Authorize(exceptionsListViewModel._ctx);
            if (!questStatusDef.IsSuccess(status))
            {
                status = new questStatus(Severity.Success);
                exceptionsListViewModel.questStatus = status;
                return Json(exceptionsListViewModel, JsonRequestBehavior.AllowGet);
            }

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Get list of items.
             *---------------------------------------------------------------------------------------------------------------------------------*/
            // TODO.
            ExceptionsListViewModel tablesetsListViewModelNEW = null;
            ExceptionsListModeler usersListModeler = new ExceptionsListModeler(this.Request, this.UserSession);
            status = usersListModeler.List(out tablesetsListViewModelNEW);
            if (!questStatusDef.IsSuccess(status))
            {
                status = new questStatus(Severity.Success);
                exceptionsListViewModel.questStatus = status;
                return Json(exceptionsListViewModel, JsonRequestBehavior.AllowGet);
            }

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Return view
             *---------------------------------------------------------------------------------------------------------------------------------*/
            status = new questStatus(Severity.Success);
            tablesetsListViewModelNEW.questStatus = status;
            return Json(tablesetsListViewModelNEW, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult PageNum(ExceptionsListViewModel exceptionsListViewModel)
        {
            questStatus status = null;

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Log Operation
             *---------------------------------------------------------------------------------------------------------------------------------*/
            status = LogOperation();
            if (!questStatusDef.IsSuccess(status))
            {
                status = new questStatus(Severity.Success);
                exceptionsListViewModel.questStatus = status;
                return Json(exceptionsListViewModel, JsonRequestBehavior.AllowGet);
            }

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Authorize
             *---------------------------------------------------------------------------------------------------------------------------------*/
            status = Authorize(exceptionsListViewModel._ctx);
            if (!questStatusDef.IsSuccess(status))
            {
                status = new questStatus(Severity.Success);
                exceptionsListViewModel.questStatus = status;
                return Json(exceptionsListViewModel, JsonRequestBehavior.AllowGet);
            }

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Get list of items.
             *---------------------------------------------------------------------------------------------------------------------------------*/
            // TODO.
            ExceptionsListViewModel tablesetsListViewModelNEW = null;
            ExceptionsListModeler usersListModeler = new ExceptionsListModeler(this.Request, this.UserSession);
            status = usersListModeler.List(out tablesetsListViewModelNEW);
            if (!questStatusDef.IsSuccess(status))
            {
                status = new questStatus(Severity.Success);
                exceptionsListViewModel.questStatus = status;
                return Json(exceptionsListViewModel, JsonRequestBehavior.AllowGet);
            }

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Return view
             *---------------------------------------------------------------------------------------------------------------------------------*/
            status = new questStatus(Severity.Success);
            tablesetsListViewModelNEW.questStatus = status;
            return Json(tablesetsListViewModelNEW, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult Next(ExceptionsListViewModel exceptionsListViewModel)
        {
            questStatus status = null;


            /*----------------------------------------------------------------------------------------------------------------------------------
             * Log Operation
             *---------------------------------------------------------------------------------------------------------------------------------*/
            status = LogOperation();
            if (!questStatusDef.IsSuccess(status))
            {
                status = new questStatus(Severity.Success);
                exceptionsListViewModel.questStatus = status;
                return Json(exceptionsListViewModel, JsonRequestBehavior.AllowGet);
            }

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Authorize
             *---------------------------------------------------------------------------------------------------------------------------------*/
            status = Authorize(exceptionsListViewModel._ctx);
            if (!questStatusDef.IsSuccess(status))
            {
                status = new questStatus(Severity.Success);
                exceptionsListViewModel.questStatus = status;
                return Json(exceptionsListViewModel, JsonRequestBehavior.AllowGet);
            }

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Get list of items.
             *---------------------------------------------------------------------------------------------------------------------------------*/
            // TODO.
            ExceptionsListViewModel tablesetsListViewModelNEW = null;
            ExceptionsListModeler usersListModeler = new ExceptionsListModeler(this.Request, this.UserSession);
            status = usersListModeler.List(out tablesetsListViewModelNEW);
            if (!questStatusDef.IsSuccess(status))
            {
                status = new questStatus(Severity.Success);
                exceptionsListViewModel.questStatus = status;
                return Json(exceptionsListViewModel, JsonRequestBehavior.AllowGet);
            }

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Return view
             *---------------------------------------------------------------------------------------------------------------------------------*/
            status = new questStatus(Severity.Success);
            tablesetsListViewModelNEW.questStatus = status;
            return Json(tablesetsListViewModelNEW, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult Last(ExceptionsListViewModel exceptionsListViewModel)
        {
            questStatus status = null;

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Log Operation
             *---------------------------------------------------------------------------------------------------------------------------------*/
            status = LogOperation();
            if (!questStatusDef.IsSuccess(status))
            {
                status = new questStatus(Severity.Success);
                exceptionsListViewModel.questStatus = status;
                return Json(exceptionsListViewModel, JsonRequestBehavior.AllowGet);
            }

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Authorize
             *---------------------------------------------------------------------------------------------------------------------------------*/
            status = Authorize(exceptionsListViewModel._ctx);
            if (!questStatusDef.IsSuccess(status))
            {
                status = new questStatus(Severity.Success);
                exceptionsListViewModel.questStatus = status;
                return Json(exceptionsListViewModel, JsonRequestBehavior.AllowGet);
            }

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Get list of items.
             *---------------------------------------------------------------------------------------------------------------------------------*/
            // TODO.
            ExceptionsListViewModel tablesetsListViewModelNEW = null;
            ExceptionsListModeler usersListModeler = new ExceptionsListModeler(this.Request, this.UserSession);
            status = usersListModeler.List(out tablesetsListViewModelNEW);
            if (!questStatusDef.IsSuccess(status))
            {
                status = new questStatus(Severity.Success);
                exceptionsListViewModel.questStatus = status;
                return Json(exceptionsListViewModel, JsonRequestBehavior.AllowGet);
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
