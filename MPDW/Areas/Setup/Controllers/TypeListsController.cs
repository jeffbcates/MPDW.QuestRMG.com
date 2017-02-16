using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using Quest.Util.Status;
using Quest.Util.Buffer;
using Quest.Functional.FMS;
using Quest.MPDW.Models;
using Quest.MasterPricing.Setup.Models;
using Quest.MasterPricing.Setup.Modelers;


namespace Quest.MasterPricing.Setup
{
    public class TypeListsController : SetupBaseController
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
            TypeListsListViewModel typeListsListViewModel = new TypeListsListViewModel(this.UserSession, baseUserSessionViewModel);
            return View(typeListsListViewModel);
        }
        [HttpGet]
        public ActionResult List(TypeListsListViewModel typeListsListViewModel)
        {
            questStatus status = null;

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Log Operation
             *---------------------------------------------------------------------------------------------------------------------------------*/
            status = LogOperation();
            if (!questStatusDef.IsSuccess(status))
            {
                status = new questStatus(Severity.Success);
                typeListsListViewModel.questStatus = status;
                return Json(typeListsListViewModel, JsonRequestBehavior.AllowGet);
            }

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Authorize
             *---------------------------------------------------------------------------------------------------------------------------------*/
            status = Authorize(typeListsListViewModel._ctx);
            if (!questStatusDef.IsSuccess(status))
            {
                status = new questStatus(Severity.Success);
                typeListsListViewModel.questStatus = status;
                return Json(typeListsListViewModel, JsonRequestBehavior.AllowGet);
            }

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Get list of requisitions for this user.
             *---------------------------------------------------------------------------------------------------------------------------------*/
            TypeListsListViewModel typeListsListViewModelNEW = null;
            TypeListsListModeler typeListListModeler = new TypeListsListModeler(this.Request, this.UserSession);
            status = typeListListModeler.List(out typeListsListViewModelNEW);
            if (!questStatusDef.IsSuccess(status))
            {
                //status = new questStatus(Severity.Success);
                typeListsListViewModel.questStatus = status;
                return Json(typeListsListViewModel, JsonRequestBehavior.AllowGet);
            }

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Return view
             *---------------------------------------------------------------------------------------------------------------------------------*/
            status = new questStatus(Severity.Success);
            typeListsListViewModelNEW.questStatus = status;
            return Json(typeListsListViewModelNEW, JsonRequestBehavior.AllowGet);
        }

        #region Paging
        //----------------------------------------------------------------------------------------------------------------------------------
        // Paging
        //----------------------------------------------------------------------------------------------------------------------------------
        [HttpGet]
        public ActionResult First(TypeListsListViewModel requisitionListViewModel)
        {
            questStatus status = null;

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Log Operation
             *---------------------------------------------------------------------------------------------------------------------------------*/
            status = LogOperation();
            if (!questStatusDef.IsSuccess(status))
            {
                status = new questStatus(Severity.Success);
                requisitionListViewModel.questStatus = status;
                return Json(requisitionListViewModel, JsonRequestBehavior.AllowGet);
            }

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Authorize
             *---------------------------------------------------------------------------------------------------------------------------------*/
            status = Authorize(requisitionListViewModel._ctx);
            if (!questStatusDef.IsSuccess(status))
            {
                status = new questStatus(Severity.Success);
                requisitionListViewModel.questStatus = status;
                return Json(requisitionListViewModel, JsonRequestBehavior.AllowGet);
            }

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Get list of requisitions for this user.
             *---------------------------------------------------------------------------------------------------------------------------------*/
            // TODO.
            TypeListsListViewModel typeListsListViewModelNEW = null;

            AuthorId authorId = new AuthorId(this.UserSession.UserId);
            TypeListsListModeler typeListsListModeler = new TypeListsListModeler(this.Request, this.UserSession);
            status = typeListsListModeler.List(out typeListsListViewModelNEW);
            if (!questStatusDef.IsSuccess(status))
            {
                status = new questStatus(Severity.Success);
                requisitionListViewModel.questStatus = status;
                return Json(requisitionListViewModel, JsonRequestBehavior.AllowGet);
            }

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Return view
             *---------------------------------------------------------------------------------------------------------------------------------*/
            status = new questStatus(Severity.Success);
            typeListsListViewModelNEW.questStatus = status;
            return Json(typeListsListViewModelNEW, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult Prev(TypeListsListViewModel requisitionListViewModel)
        {
            questStatus status = null;

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Log Operation
             *---------------------------------------------------------------------------------------------------------------------------------*/
            status = LogOperation();
            if (!questStatusDef.IsSuccess(status))
            {
                status = new questStatus(Severity.Success);
                requisitionListViewModel.questStatus = status;
                return Json(requisitionListViewModel, JsonRequestBehavior.AllowGet);
            }

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Authorize
             *---------------------------------------------------------------------------------------------------------------------------------*/
            status = Authorize(requisitionListViewModel._ctx);
            if (!questStatusDef.IsSuccess(status))
            {
                status = new questStatus(Severity.Success);
                requisitionListViewModel.questStatus = status;
                return Json(requisitionListViewModel, JsonRequestBehavior.AllowGet);
            }

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Get list of requisitions for this user.
             *---------------------------------------------------------------------------------------------------------------------------------*/
            // TODO.
            TypeListsListViewModel typeListsListViewModelNEW = null;
            TypeListsListModeler typeListsListModeler = new TypeListsListModeler(this.Request, this.UserSession);
            status = typeListsListModeler.List(out typeListsListViewModelNEW);
            if (!questStatusDef.IsSuccess(status))
            {
                status = new questStatus(Severity.Success);
                requisitionListViewModel.questStatus = status;
                return Json(requisitionListViewModel, JsonRequestBehavior.AllowGet);
            }

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Return view
             *---------------------------------------------------------------------------------------------------------------------------------*/
            status = new questStatus(Severity.Success);
            typeListsListViewModelNEW.questStatus = status;
            return Json(typeListsListViewModelNEW, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult PageNum(TypeListsListViewModel requisitionListViewModel)
        {
            questStatus status = null;

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Log Operation
             *---------------------------------------------------------------------------------------------------------------------------------*/
            status = LogOperation();
            if (!questStatusDef.IsSuccess(status))
            {
                status = new questStatus(Severity.Success);
                requisitionListViewModel.questStatus = status;
                return Json(requisitionListViewModel, JsonRequestBehavior.AllowGet);
            }

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Authorize
             *---------------------------------------------------------------------------------------------------------------------------------*/
            status = Authorize(requisitionListViewModel._ctx);
            if (!questStatusDef.IsSuccess(status))
            {
                status = new questStatus(Severity.Success);
                requisitionListViewModel.questStatus = status;
                return Json(requisitionListViewModel, JsonRequestBehavior.AllowGet);
            }

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Get list of requisitions for this user.
             *---------------------------------------------------------------------------------------------------------------------------------*/
            // TODO.
            TypeListsListViewModel typeListsListViewModelNEW = null;
            TypeListsListModeler typeListsListModeler = new TypeListsListModeler(this.Request, this.UserSession);
            status = typeListsListModeler.List(out typeListsListViewModelNEW);
            if (!questStatusDef.IsSuccess(status))
            {
                status = new questStatus(Severity.Success);
                requisitionListViewModel.questStatus = status;
                return Json(requisitionListViewModel, JsonRequestBehavior.AllowGet);
            }

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Return view
             *---------------------------------------------------------------------------------------------------------------------------------*/
            status = new questStatus(Severity.Success);
            typeListsListViewModelNEW.questStatus = status;
            return Json(typeListsListViewModelNEW, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult Next(TypeListsListViewModel requisitionListViewModel)
        {
            questStatus status = null;


            /*----------------------------------------------------------------------------------------------------------------------------------
             * Log Operation
             *---------------------------------------------------------------------------------------------------------------------------------*/
            status = LogOperation();
            if (!questStatusDef.IsSuccess(status))
            {
                status = new questStatus(Severity.Success);
                requisitionListViewModel.questStatus = status;
                return Json(requisitionListViewModel, JsonRequestBehavior.AllowGet);
            }

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Authorize
             *---------------------------------------------------------------------------------------------------------------------------------*/
            status = Authorize(requisitionListViewModel._ctx);
            if (!questStatusDef.IsSuccess(status))
            {
                status = new questStatus(Severity.Success);
                requisitionListViewModel.questStatus = status;
                return Json(requisitionListViewModel, JsonRequestBehavior.AllowGet);
            }

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Get list of requisitions for this user.
             *---------------------------------------------------------------------------------------------------------------------------------*/
            // TODO.
            TypeListsListViewModel typeListsListViewModelNEW = null;
            TypeListsListModeler typeListsListModeler = new TypeListsListModeler(this.Request, this.UserSession);
            status = typeListsListModeler.List(out typeListsListViewModelNEW);
            if (!questStatusDef.IsSuccess(status))
            {
                status = new questStatus(Severity.Success);
                requisitionListViewModel.questStatus = status;
                return Json(requisitionListViewModel, JsonRequestBehavior.AllowGet);
            }

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Return view
             *---------------------------------------------------------------------------------------------------------------------------------*/
            status = new questStatus(Severity.Success);
            typeListsListViewModelNEW.questStatus = status;
            return Json(typeListsListViewModelNEW, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult Last(TypeListsListViewModel requisitionListViewModel)
        {
            questStatus status = null;

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Log Operation
             *---------------------------------------------------------------------------------------------------------------------------------*/
            status = LogOperation();
            if (!questStatusDef.IsSuccess(status))
            {
                status = new questStatus(Severity.Success);
                requisitionListViewModel.questStatus = status;
                return Json(requisitionListViewModel, JsonRequestBehavior.AllowGet);
            }

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Authorize
             *---------------------------------------------------------------------------------------------------------------------------------*/
            status = Authorize(requisitionListViewModel._ctx);
            if (!questStatusDef.IsSuccess(status))
            {
                status = new questStatus(Severity.Success);
                requisitionListViewModel.questStatus = status;
                return Json(requisitionListViewModel, JsonRequestBehavior.AllowGet);
            }

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Get list of requisitions for this user.
             *---------------------------------------------------------------------------------------------------------------------------------*/
            // TODO.
            TypeListsListViewModel typeListsListViewModelNEW = null;
            TypeListsListModeler typeListsListModeler = new TypeListsListModeler(this.Request, this.UserSession);
            status = typeListsListModeler.List(out typeListsListViewModelNEW);
            if (!questStatusDef.IsSuccess(status))
            {
                status = new questStatus(Severity.Success);
                requisitionListViewModel.questStatus = status;
                return Json(requisitionListViewModel, JsonRequestBehavior.AllowGet);
            }

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Return view
             *---------------------------------------------------------------------------------------------------------------------------------*/
            status = new questStatus(Severity.Success);
            typeListsListViewModelNEW.questStatus = status;
            return Json(typeListsListViewModelNEW, JsonRequestBehavior.AllowGet);
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
