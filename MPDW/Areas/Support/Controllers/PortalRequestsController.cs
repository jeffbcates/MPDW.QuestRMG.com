using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
    public class PortalRequestsController : SupportBaseController
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
            PortalRequestsViewModel portalRequestsViewModel = new PortalRequestsViewModel(this.UserSession, baseUserSessionViewModel);
            return View(portalRequestsViewModel);
        }
        [HttpGet]
        public ActionResult List(PortalRequestsListViewModel portalRequestsListViewModel)
        {
            questStatus status = null;

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Log Operation
             *---------------------------------------------------------------------------------------------------------------------------------*/
            status = LogOperation();
            if (!questStatusDef.IsSuccess(status))
            {
                portalRequestsListViewModel.questStatus = status;
                return Json(portalRequestsListViewModel, JsonRequestBehavior.AllowGet);
            }

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Authorize
             *---------------------------------------------------------------------------------------------------------------------------------*/
            status = Authorize(portalRequestsListViewModel._ctx);
            if (!questStatusDef.IsSuccess(status))
            {
                portalRequestsListViewModel.questStatus = status;
                return Json(portalRequestsListViewModel, JsonRequestBehavior.AllowGet);
            }

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Get list of items.
             *---------------------------------------------------------------------------------------------------------------------------------*/
            PortalRequestsListViewModel portalRequestsListViewModelNEW = null;
            PortalRequestsListModeler portalRequestsListModeler = new PortalRequestsListModeler(this.Request, this.UserSession);
            status = portalRequestsListModeler.List(out portalRequestsListViewModelNEW);
            if (!questStatusDef.IsSuccess(status))
            {
                portalRequestsListViewModel.questStatus = status;
                return Json(portalRequestsListViewModel, JsonRequestBehavior.AllowGet);
            }

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Return result view model or as Excel
             *---------------------------------------------------------------------------------------------------------------------------------*/
            if (portalRequestsListViewModel.bExportToExcel)
            {
                string filename = "PortalRequestsLog_" + DateTime.Now.ToString();
                Response.ClearContent();
                Response.AddHeader("content-disposition", "atachment;filename=" + filename + ".xls");
                Response.AddHeader("Content-Type", "application/vnd.ms-excel");
                writeTsv(portalRequestsListViewModelNEW, Response.Output);
                Response.Flush();
                Response.End();
                return new EmptyResult();
            }
            else {
                status = new questStatus(Severity.Success);
                portalRequestsListViewModelNEW.questStatus = status;
                return Json(portalRequestsListViewModelNEW, JsonRequestBehavior.AllowGet);
            }
        }

        #region Paging
        //----------------------------------------------------------------------------------------------------------------------------------
        // Paging
        //----------------------------------------------------------------------------------------------------------------------------------
        [HttpGet]
        public ActionResult First(PortalRequestsListViewModel portalRequestsListViewModel)
        {
            questStatus status = null;

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Log Operation
             *---------------------------------------------------------------------------------------------------------------------------------*/
            status = LogOperation();
            if (!questStatusDef.IsSuccess(status))
            {
                status = new questStatus(Severity.Success);
                portalRequestsListViewModel.questStatus = status;
                return Json(portalRequestsListViewModel, JsonRequestBehavior.AllowGet);
            }

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Authorize
             *---------------------------------------------------------------------------------------------------------------------------------*/
            status = Authorize(portalRequestsListViewModel._ctx);
            if (!questStatusDef.IsSuccess(status))
            {
                status = new questStatus(Severity.Success);
                portalRequestsListViewModel.questStatus = status;
                return Json(portalRequestsListViewModel, JsonRequestBehavior.AllowGet);
            }

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Get list of items.
             *---------------------------------------------------------------------------------------------------------------------------------*/
            // TODO.
            PortalRequestsListViewModel portalRequestsListViewModelNEW = null;
            PortalRequestsListModeler usersListModeler = new PortalRequestsListModeler(this.Request, this.UserSession);
            status = usersListModeler.List(out portalRequestsListViewModelNEW);
            if (!questStatusDef.IsSuccess(status))
            {
                status = new questStatus(Severity.Success);
                portalRequestsListViewModel.questStatus = status;
                return Json(portalRequestsListViewModel, JsonRequestBehavior.AllowGet);
            }

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Return view
             *---------------------------------------------------------------------------------------------------------------------------------*/
            status = new questStatus(Severity.Success);
            portalRequestsListViewModelNEW.questStatus = status;
            return Json(portalRequestsListViewModelNEW, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult Prev(PortalRequestsListViewModel portalRequestsListViewModel)
        {
            questStatus status = null;

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Log Operation
             *---------------------------------------------------------------------------------------------------------------------------------*/
            status = LogOperation();
            if (!questStatusDef.IsSuccess(status))
            {
                status = new questStatus(Severity.Success);
                portalRequestsListViewModel.questStatus = status;
                return Json(portalRequestsListViewModel, JsonRequestBehavior.AllowGet);
            }

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Authorize
             *---------------------------------------------------------------------------------------------------------------------------------*/
            status = Authorize(portalRequestsListViewModel._ctx);
            if (!questStatusDef.IsSuccess(status))
            {
                status = new questStatus(Severity.Success);
                portalRequestsListViewModel.questStatus = status;
                return Json(portalRequestsListViewModel, JsonRequestBehavior.AllowGet);
            }

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Get list of items.
             *---------------------------------------------------------------------------------------------------------------------------------*/
            // TODO.
            PortalRequestsListViewModel portalRequestsListViewModelNEW = null;
            PortalRequestsListModeler usersListModeler = new PortalRequestsListModeler(this.Request, this.UserSession);
            status = usersListModeler.List(out portalRequestsListViewModelNEW);
            if (!questStatusDef.IsSuccess(status))
            {
                status = new questStatus(Severity.Success);
                portalRequestsListViewModel.questStatus = status;
                return Json(portalRequestsListViewModel, JsonRequestBehavior.AllowGet);
            }

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Return view
             *---------------------------------------------------------------------------------------------------------------------------------*/
            status = new questStatus(Severity.Success);
            portalRequestsListViewModelNEW.questStatus = status;
            return Json(portalRequestsListViewModelNEW, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult PageNum(PortalRequestsListViewModel portalRequestsListViewModel)
        {
            questStatus status = null;

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Log Operation
             *---------------------------------------------------------------------------------------------------------------------------------*/
            status = LogOperation();
            if (!questStatusDef.IsSuccess(status))
            {
                status = new questStatus(Severity.Success);
                portalRequestsListViewModel.questStatus = status;
                return Json(portalRequestsListViewModel, JsonRequestBehavior.AllowGet);
            }

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Authorize
             *---------------------------------------------------------------------------------------------------------------------------------*/
            status = Authorize(portalRequestsListViewModel._ctx);
            if (!questStatusDef.IsSuccess(status))
            {
                status = new questStatus(Severity.Success);
                portalRequestsListViewModel.questStatus = status;
                return Json(portalRequestsListViewModel, JsonRequestBehavior.AllowGet);
            }

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Get list of items.
             *---------------------------------------------------------------------------------------------------------------------------------*/
            // TODO.
            PortalRequestsListViewModel portalRequestsListViewModelNEW = null;
            PortalRequestsListModeler usersListModeler = new PortalRequestsListModeler(this.Request, this.UserSession);
            status = usersListModeler.List(out portalRequestsListViewModelNEW);
            if (!questStatusDef.IsSuccess(status))
            {
                status = new questStatus(Severity.Success);
                portalRequestsListViewModel.questStatus = status;
                return Json(portalRequestsListViewModel, JsonRequestBehavior.AllowGet);
            }

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Return view
             *---------------------------------------------------------------------------------------------------------------------------------*/
            status = new questStatus(Severity.Success);
            portalRequestsListViewModelNEW.questStatus = status;
            return Json(portalRequestsListViewModelNEW, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult Next(PortalRequestsListViewModel portalRequestsListViewModel)
        {
            questStatus status = null;


            /*----------------------------------------------------------------------------------------------------------------------------------
             * Log Operation
             *---------------------------------------------------------------------------------------------------------------------------------*/
            status = LogOperation();
            if (!questStatusDef.IsSuccess(status))
            {
                status = new questStatus(Severity.Success);
                portalRequestsListViewModel.questStatus = status;
                return Json(portalRequestsListViewModel, JsonRequestBehavior.AllowGet);
            }

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Authorize
             *---------------------------------------------------------------------------------------------------------------------------------*/
            status = Authorize(portalRequestsListViewModel._ctx);
            if (!questStatusDef.IsSuccess(status))
            {
                status = new questStatus(Severity.Success);
                portalRequestsListViewModel.questStatus = status;
                return Json(portalRequestsListViewModel, JsonRequestBehavior.AllowGet);
            }

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Get list of items.
             *---------------------------------------------------------------------------------------------------------------------------------*/
            // TODO.
            PortalRequestsListViewModel portalRequestsListViewModelNEW = null;
            PortalRequestsListModeler usersListModeler = new PortalRequestsListModeler(this.Request, this.UserSession);
            status = usersListModeler.List(out portalRequestsListViewModelNEW);
            if (!questStatusDef.IsSuccess(status))
            {
                status = new questStatus(Severity.Success);
                portalRequestsListViewModel.questStatus = status;
                return Json(portalRequestsListViewModel, JsonRequestBehavior.AllowGet);
            }

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Return view
             *---------------------------------------------------------------------------------------------------------------------------------*/
            status = new questStatus(Severity.Success);
            portalRequestsListViewModelNEW.questStatus = status;
            return Json(portalRequestsListViewModelNEW, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult Last(PortalRequestsListViewModel portalRequestsListViewModel)
        {
            questStatus status = null;

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Log Operation
             *---------------------------------------------------------------------------------------------------------------------------------*/
            status = LogOperation();
            if (!questStatusDef.IsSuccess(status))
            {
                status = new questStatus(Severity.Success);
                portalRequestsListViewModel.questStatus = status;
                return Json(portalRequestsListViewModel, JsonRequestBehavior.AllowGet);
            }

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Authorize
             *---------------------------------------------------------------------------------------------------------------------------------*/
            status = Authorize(portalRequestsListViewModel._ctx);
            if (!questStatusDef.IsSuccess(status))
            {
                status = new questStatus(Severity.Success);
                portalRequestsListViewModel.questStatus = status;
                return Json(portalRequestsListViewModel, JsonRequestBehavior.AllowGet);
            }

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Get list of items.
             *---------------------------------------------------------------------------------------------------------------------------------*/
            // TODO.
            PortalRequestsListViewModel portalRequestsListViewModelNEW = null;
            PortalRequestsListModeler usersListModeler = new PortalRequestsListModeler(this.Request, this.UserSession);
            status = usersListModeler.List(out portalRequestsListViewModelNEW);
            if (!questStatusDef.IsSuccess(status))
            {
                status = new questStatus(Severity.Success);
                portalRequestsListViewModel.questStatus = status;
                return Json(portalRequestsListViewModel, JsonRequestBehavior.AllowGet);
            }

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Return view
             *---------------------------------------------------------------------------------------------------------------------------------*/
            status = new questStatus(Severity.Success);
            portalRequestsListViewModelNEW.questStatus = status;
            return Json(portalRequestsListViewModelNEW, JsonRequestBehavior.AllowGet);
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
        [HttpPost]
        public ActionResult Clear(PortalRequestsListViewModel viewModel)
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
            PortalRequestsListModeler portalRequestsListModeler = new PortalRequestsListModeler(this.Request, this.UserSession);
            status = portalRequestsListModeler.Clear(viewModel);
            if (!questStatusDef.IsSuccess(status))
            {
                viewModel.questStatus = status;
                return Json(viewModel, JsonRequestBehavior.AllowGet);
            }

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Return result.
             *---------------------------------------------------------------------------------------------------------------------------------*/
            status = new questStatus(Severity.Success, "PortalRequest log successfully cleared");
            viewModel.questStatus = status;
            return Json(viewModel, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult Delete(DeleteLogItemsViewModel viewModel)
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
            PortalRequestsListModeler portalRequestsListModeler = new PortalRequestsListModeler(this.Request, this.UserSession);
            status = portalRequestsListModeler.Delete(viewModel);
            if (!questStatusDef.IsSuccess(status))
            {
                viewModel.questStatus = status;
                return Json(viewModel, JsonRequestBehavior.AllowGet);
            }

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Return result.
             *---------------------------------------------------------------------------------------------------------------------------------*/
            status = new questStatus(Severity.Success, "Selected PortalRequest log entries successfully deleted");
            viewModel.questStatus = status;
            return Json(viewModel, JsonRequestBehavior.AllowGet);
        }
        #endregion


        #region Private Methods
        /*==================================================================================================================================
         * Private Methods
         *=================================================================================================================================*/

        #region Export Routines
        //----------------------------------------------------------------------------------------------------------------------------------
        // Export Routines
        //----------------------------------------------------------------------------------------------------------------------------------
        private void writeTsv(PortalRequestsListViewModel portalRequestsListViewModel, TextWriter output)
        {
            PropertyInfo[] propertyInfos = typeof(PortalRequestLineItemViewModel).GetProperties();
            foreach (PropertyInfo pi in propertyInfos)
            {
                output.Write(pi.Name); // header
                output.Write("\t");
            }
            output.WriteLine();
            foreach (PortalRequestLineItemViewModel lineItem in portalRequestsListViewModel.Items)
            {
                foreach (PropertyInfo pi in propertyInfos)
                {
                    object _value = pi.GetValue(lineItem);
                    string value = _value == null ? "(null)" : _value.ToString().Replace("\t", " ").Replace("\r", " ").Replace("\n", " ");
                    output.Write(value);
                    output.Write("\t");
                }
                output.WriteLine();
            }
        }
        #endregion

        #endregion
    }
}
