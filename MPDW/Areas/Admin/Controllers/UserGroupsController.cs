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
    public class UserGroupsController : AdminBaseController
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
        public ActionResult Load(UserEditorViewModel viewModel)
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
            List<BootstrapTreenodeViewModel> userGroupsNodeList = null;
            UserGroupsModeler userGroupsModeler = new UserGroupsModeler(this.Request, this.UserSession);
            status = userGroupsModeler.Load(viewModel, out userGroupsNodeList);
            if (!questStatusDef.IsSuccess(status))
            {
                viewModel.questStatus = status;
                return Json(viewModel, JsonRequestBehavior.AllowGet);
            }

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Return view
             *---------------------------------------------------------------------------------------------------------------------------------*/
            return Json(userGroupsNodeList, JsonRequestBehavior.AllowGet);
        }

        #region Paging
        //----------------------------------------------------------------------------------------------------------------------------------
        // Paging
        //----------------------------------------------------------------------------------------------------------------------------------
        [HttpGet]
        public ActionResult First(GroupsListViewModel groupsListViewModel)
        {
            questStatus status = null;

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Log Operation
             *---------------------------------------------------------------------------------------------------------------------------------*/
            status = LogOperation();
            if (!questStatusDef.IsSuccess(status))
            {
                status = new questStatus(Severity.Success);
                groupsListViewModel.questStatus = status;
                return Json(groupsListViewModel, JsonRequestBehavior.AllowGet);
            }

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Authorize
             *---------------------------------------------------------------------------------------------------------------------------------*/
            status = Authorize(groupsListViewModel._ctx);
            if (!questStatusDef.IsSuccess(status))
            {
                status = new questStatus(Severity.Success);
                groupsListViewModel.questStatus = status;
                return Json(groupsListViewModel, JsonRequestBehavior.AllowGet);
            }

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Get list of items.
             *---------------------------------------------------------------------------------------------------------------------------------*/
            // TODO.
            GroupsListViewModel tablesetsListViewModelNEW = null;
            GroupsListModeler groupsListModeler = new GroupsListModeler(this.Request, this.UserSession);
            status = groupsListModeler.List(out tablesetsListViewModelNEW);
            if (!questStatusDef.IsSuccess(status))
            {
                status = new questStatus(Severity.Success);
                groupsListViewModel.questStatus = status;
                return Json(groupsListViewModel, JsonRequestBehavior.AllowGet);
            }

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Return view
             *---------------------------------------------------------------------------------------------------------------------------------*/
            status = new questStatus(Severity.Success);
            tablesetsListViewModelNEW.questStatus = status;
            return Json(tablesetsListViewModelNEW, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult Prev(GroupsListViewModel groupsListViewModel)
        {
            questStatus status = null;

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Log Operation
             *---------------------------------------------------------------------------------------------------------------------------------*/
            status = LogOperation();
            if (!questStatusDef.IsSuccess(status))
            {
                status = new questStatus(Severity.Success);
                groupsListViewModel.questStatus = status;
                return Json(groupsListViewModel, JsonRequestBehavior.AllowGet);
            }

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Authorize
             *---------------------------------------------------------------------------------------------------------------------------------*/
            status = Authorize(groupsListViewModel._ctx);
            if (!questStatusDef.IsSuccess(status))
            {
                status = new questStatus(Severity.Success);
                groupsListViewModel.questStatus = status;
                return Json(groupsListViewModel, JsonRequestBehavior.AllowGet);
            }

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Get list of items.
             *---------------------------------------------------------------------------------------------------------------------------------*/
            // TODO.
            GroupsListViewModel tablesetsListViewModelNEW = null;
            GroupsListModeler groupsListModeler = new GroupsListModeler(this.Request, this.UserSession);
            status = groupsListModeler.List(out tablesetsListViewModelNEW);
            if (!questStatusDef.IsSuccess(status))
            {
                status = new questStatus(Severity.Success);
                groupsListViewModel.questStatus = status;
                return Json(groupsListViewModel, JsonRequestBehavior.AllowGet);
            }

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Return view
             *---------------------------------------------------------------------------------------------------------------------------------*/
            status = new questStatus(Severity.Success);
            tablesetsListViewModelNEW.questStatus = status;
            return Json(tablesetsListViewModelNEW, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult PageNum(GroupsListViewModel groupsListViewModel)
        {
            questStatus status = null;

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Log Operation
             *---------------------------------------------------------------------------------------------------------------------------------*/
            status = LogOperation();
            if (!questStatusDef.IsSuccess(status))
            {
                status = new questStatus(Severity.Success);
                groupsListViewModel.questStatus = status;
                return Json(groupsListViewModel, JsonRequestBehavior.AllowGet);
            }

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Authorize
             *---------------------------------------------------------------------------------------------------------------------------------*/
            status = Authorize(groupsListViewModel._ctx);
            if (!questStatusDef.IsSuccess(status))
            {
                status = new questStatus(Severity.Success);
                groupsListViewModel.questStatus = status;
                return Json(groupsListViewModel, JsonRequestBehavior.AllowGet);
            }

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Get list of items.
             *---------------------------------------------------------------------------------------------------------------------------------*/
            // TODO.
            GroupsListViewModel tablesetsListViewModelNEW = null;
            GroupsListModeler groupsListModeler = new GroupsListModeler(this.Request, this.UserSession);
            status = groupsListModeler.List(out tablesetsListViewModelNEW);
            if (!questStatusDef.IsSuccess(status))
            {
                status = new questStatus(Severity.Success);
                groupsListViewModel.questStatus = status;
                return Json(groupsListViewModel, JsonRequestBehavior.AllowGet);
            }

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Return view
             *---------------------------------------------------------------------------------------------------------------------------------*/
            status = new questStatus(Severity.Success);
            tablesetsListViewModelNEW.questStatus = status;
            return Json(tablesetsListViewModelNEW, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult Next(GroupsListViewModel groupsListViewModel)
        {
            questStatus status = null;


            /*----------------------------------------------------------------------------------------------------------------------------------
             * Log Operation
             *---------------------------------------------------------------------------------------------------------------------------------*/
            status = LogOperation();
            if (!questStatusDef.IsSuccess(status))
            {
                status = new questStatus(Severity.Success);
                groupsListViewModel.questStatus = status;
                return Json(groupsListViewModel, JsonRequestBehavior.AllowGet);
            }

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Authorize
             *---------------------------------------------------------------------------------------------------------------------------------*/
            status = Authorize(groupsListViewModel._ctx);
            if (!questStatusDef.IsSuccess(status))
            {
                status = new questStatus(Severity.Success);
                groupsListViewModel.questStatus = status;
                return Json(groupsListViewModel, JsonRequestBehavior.AllowGet);
            }

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Get list of items.
             *---------------------------------------------------------------------------------------------------------------------------------*/
            // TODO.
            GroupsListViewModel tablesetsListViewModelNEW = null;
            GroupsListModeler groupsListModeler = new GroupsListModeler(this.Request, this.UserSession);
            status = groupsListModeler.List(out tablesetsListViewModelNEW);
            if (!questStatusDef.IsSuccess(status))
            {
                status = new questStatus(Severity.Success);
                groupsListViewModel.questStatus = status;
                return Json(groupsListViewModel, JsonRequestBehavior.AllowGet);
            }

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Return view
             *---------------------------------------------------------------------------------------------------------------------------------*/
            status = new questStatus(Severity.Success);
            tablesetsListViewModelNEW.questStatus = status;
            return Json(tablesetsListViewModelNEW, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult Last(GroupsListViewModel groupsListViewModel)
        {
            questStatus status = null;

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Log Operation
             *---------------------------------------------------------------------------------------------------------------------------------*/
            status = LogOperation();
            if (!questStatusDef.IsSuccess(status))
            {
                status = new questStatus(Severity.Success);
                groupsListViewModel.questStatus = status;
                return Json(groupsListViewModel, JsonRequestBehavior.AllowGet);
            }

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Authorize
             *---------------------------------------------------------------------------------------------------------------------------------*/
            status = Authorize(groupsListViewModel._ctx);
            if (!questStatusDef.IsSuccess(status))
            {
                status = new questStatus(Severity.Success);
                groupsListViewModel.questStatus = status;
                return Json(groupsListViewModel, JsonRequestBehavior.AllowGet);
            }

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Get list of items.
             *---------------------------------------------------------------------------------------------------------------------------------*/
            // TODO.
            GroupsListViewModel tablesetsListViewModelNEW = null;
            GroupsListModeler groupsListModeler = new GroupsListModeler(this.Request, this.UserSession);
            status = groupsListModeler.List(out tablesetsListViewModelNEW);
            if (!questStatusDef.IsSuccess(status))
            {
                status = new questStatus(Severity.Success);
                groupsListViewModel.questStatus = status;
                return Json(groupsListViewModel, JsonRequestBehavior.AllowGet);
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
