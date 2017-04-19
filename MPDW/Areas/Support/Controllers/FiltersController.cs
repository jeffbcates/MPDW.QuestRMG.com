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
    public class FiltersController : SupportBaseController
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
            FiltersViewModel filtersViewModel = new FiltersViewModel(this.UserSession, baseUserSessionViewModel);
            return View(filtersViewModel);
        }
        [HttpGet]
        public ActionResult List(FiltersListViewModel filtersListViewModel)
        {
            questStatus status = null;

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Log Operation
             *---------------------------------------------------------------------------------------------------------------------------------*/
            status = LogOperation();
            if (!questStatusDef.IsSuccess(status))
            {
                filtersListViewModel.questStatus = status;
                return Json(filtersListViewModel, JsonRequestBehavior.AllowGet);
            }

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Authorize
             *---------------------------------------------------------------------------------------------------------------------------------*/
            status = Authorize(filtersListViewModel._ctx);
            if (!questStatusDef.IsSuccess(status))
            {
                filtersListViewModel.questStatus = status;
                return Json(filtersListViewModel, JsonRequestBehavior.AllowGet);
            }

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Get list of items.
             *---------------------------------------------------------------------------------------------------------------------------------*/
            FiltersListViewModel filtersListViewModelNEW = null;
            FiltersListModeler filtersListModeler = new FiltersListModeler(this.Request, this.UserSession);
            status = filtersListModeler.List(out filtersListViewModelNEW);
            if (!questStatusDef.IsSuccess(status))
            {
                filtersListViewModel.questStatus = status;
                return Json(filtersListViewModel, JsonRequestBehavior.AllowGet);
            }

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Return result view model or as Excel
             *---------------------------------------------------------------------------------------------------------------------------------*/
            if (filtersListViewModel.bExportToExcel)
            {
                string filename = "FiltersLog_" + DateTime.Now.ToString();
                Response.ClearContent();
                Response.AddHeader("content-disposition", "atachment;filename=" + filename + ".xls");
                Response.AddHeader("Content-Type", "application/vnd.ms-excel");
                writeTsv(filtersListViewModelNEW, Response.Output);
                Response.Flush();
                Response.End();
                return new EmptyResult();
            }
            else {
                status = new questStatus(Severity.Success);
                filtersListViewModelNEW.questStatus = status;
                return Json(filtersListViewModelNEW, JsonRequestBehavior.AllowGet);
            }
        }

        #region Paging
        //----------------------------------------------------------------------------------------------------------------------------------
        // Paging
        //----------------------------------------------------------------------------------------------------------------------------------
        [HttpGet]
        public ActionResult First(FiltersListViewModel filtersListViewModel)
        {
            questStatus status = null;

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Log Operation
             *---------------------------------------------------------------------------------------------------------------------------------*/
            status = LogOperation();
            if (!questStatusDef.IsSuccess(status))
            {
                status = new questStatus(Severity.Success);
                filtersListViewModel.questStatus = status;
                return Json(filtersListViewModel, JsonRequestBehavior.AllowGet);
            }

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Authorize
             *---------------------------------------------------------------------------------------------------------------------------------*/
            status = Authorize(filtersListViewModel._ctx);
            if (!questStatusDef.IsSuccess(status))
            {
                status = new questStatus(Severity.Success);
                filtersListViewModel.questStatus = status;
                return Json(filtersListViewModel, JsonRequestBehavior.AllowGet);
            }

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Get list of items.
             *---------------------------------------------------------------------------------------------------------------------------------*/
            // TODO.
            FiltersListViewModel filtersListViewModelNEW = null;
            FiltersListModeler usersListModeler = new FiltersListModeler(this.Request, this.UserSession);
            status = usersListModeler.List(out filtersListViewModelNEW);
            if (!questStatusDef.IsSuccess(status))
            {
                status = new questStatus(Severity.Success);
                filtersListViewModel.questStatus = status;
                return Json(filtersListViewModel, JsonRequestBehavior.AllowGet);
            }

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Return view
             *---------------------------------------------------------------------------------------------------------------------------------*/
            status = new questStatus(Severity.Success);
            filtersListViewModelNEW.questStatus = status;
            return Json(filtersListViewModelNEW, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult Prev(FiltersListViewModel filtersListViewModel)
        {
            questStatus status = null;

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Log Operation
             *---------------------------------------------------------------------------------------------------------------------------------*/
            status = LogOperation();
            if (!questStatusDef.IsSuccess(status))
            {
                status = new questStatus(Severity.Success);
                filtersListViewModel.questStatus = status;
                return Json(filtersListViewModel, JsonRequestBehavior.AllowGet);
            }

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Authorize
             *---------------------------------------------------------------------------------------------------------------------------------*/
            status = Authorize(filtersListViewModel._ctx);
            if (!questStatusDef.IsSuccess(status))
            {
                status = new questStatus(Severity.Success);
                filtersListViewModel.questStatus = status;
                return Json(filtersListViewModel, JsonRequestBehavior.AllowGet);
            }

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Get list of items.
             *---------------------------------------------------------------------------------------------------------------------------------*/
            // TODO.
            FiltersListViewModel filtersListViewModelNEW = null;
            FiltersListModeler usersListModeler = new FiltersListModeler(this.Request, this.UserSession);
            status = usersListModeler.List(out filtersListViewModelNEW);
            if (!questStatusDef.IsSuccess(status))
            {
                status = new questStatus(Severity.Success);
                filtersListViewModel.questStatus = status;
                return Json(filtersListViewModel, JsonRequestBehavior.AllowGet);
            }

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Return view
             *---------------------------------------------------------------------------------------------------------------------------------*/
            status = new questStatus(Severity.Success);
            filtersListViewModelNEW.questStatus = status;
            return Json(filtersListViewModelNEW, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult PageNum(FiltersListViewModel filtersListViewModel)
        {
            questStatus status = null;

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Log Operation
             *---------------------------------------------------------------------------------------------------------------------------------*/
            status = LogOperation();
            if (!questStatusDef.IsSuccess(status))
            {
                status = new questStatus(Severity.Success);
                filtersListViewModel.questStatus = status;
                return Json(filtersListViewModel, JsonRequestBehavior.AllowGet);
            }

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Authorize
             *---------------------------------------------------------------------------------------------------------------------------------*/
            status = Authorize(filtersListViewModel._ctx);
            if (!questStatusDef.IsSuccess(status))
            {
                status = new questStatus(Severity.Success);
                filtersListViewModel.questStatus = status;
                return Json(filtersListViewModel, JsonRequestBehavior.AllowGet);
            }

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Get list of items.
             *---------------------------------------------------------------------------------------------------------------------------------*/
            // TODO.
            FiltersListViewModel filtersListViewModelNEW = null;
            FiltersListModeler usersListModeler = new FiltersListModeler(this.Request, this.UserSession);
            status = usersListModeler.List(out filtersListViewModelNEW);
            if (!questStatusDef.IsSuccess(status))
            {
                status = new questStatus(Severity.Success);
                filtersListViewModel.questStatus = status;
                return Json(filtersListViewModel, JsonRequestBehavior.AllowGet);
            }

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Return view
             *---------------------------------------------------------------------------------------------------------------------------------*/
            status = new questStatus(Severity.Success);
            filtersListViewModelNEW.questStatus = status;
            return Json(filtersListViewModelNEW, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult Next(FiltersListViewModel filtersListViewModel)
        {
            questStatus status = null;


            /*----------------------------------------------------------------------------------------------------------------------------------
             * Log Operation
             *---------------------------------------------------------------------------------------------------------------------------------*/
            status = LogOperation();
            if (!questStatusDef.IsSuccess(status))
            {
                status = new questStatus(Severity.Success);
                filtersListViewModel.questStatus = status;
                return Json(filtersListViewModel, JsonRequestBehavior.AllowGet);
            }

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Authorize
             *---------------------------------------------------------------------------------------------------------------------------------*/
            status = Authorize(filtersListViewModel._ctx);
            if (!questStatusDef.IsSuccess(status))
            {
                status = new questStatus(Severity.Success);
                filtersListViewModel.questStatus = status;
                return Json(filtersListViewModel, JsonRequestBehavior.AllowGet);
            }

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Get list of items.
             *---------------------------------------------------------------------------------------------------------------------------------*/
            // TODO.
            FiltersListViewModel filtersListViewModelNEW = null;
            FiltersListModeler usersListModeler = new FiltersListModeler(this.Request, this.UserSession);
            status = usersListModeler.List(out filtersListViewModelNEW);
            if (!questStatusDef.IsSuccess(status))
            {
                status = new questStatus(Severity.Success);
                filtersListViewModel.questStatus = status;
                return Json(filtersListViewModel, JsonRequestBehavior.AllowGet);
            }

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Return view
             *---------------------------------------------------------------------------------------------------------------------------------*/
            status = new questStatus(Severity.Success);
            filtersListViewModelNEW.questStatus = status;
            return Json(filtersListViewModelNEW, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult Last(FiltersListViewModel filtersListViewModel)
        {
            questStatus status = null;

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Log Operation
             *---------------------------------------------------------------------------------------------------------------------------------*/
            status = LogOperation();
            if (!questStatusDef.IsSuccess(status))
            {
                status = new questStatus(Severity.Success);
                filtersListViewModel.questStatus = status;
                return Json(filtersListViewModel, JsonRequestBehavior.AllowGet);
            }

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Authorize
             *---------------------------------------------------------------------------------------------------------------------------------*/
            status = Authorize(filtersListViewModel._ctx);
            if (!questStatusDef.IsSuccess(status))
            {
                status = new questStatus(Severity.Success);
                filtersListViewModel.questStatus = status;
                return Json(filtersListViewModel, JsonRequestBehavior.AllowGet);
            }

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Get list of items.
             *---------------------------------------------------------------------------------------------------------------------------------*/
            // TODO.
            FiltersListViewModel filtersListViewModelNEW = null;
            FiltersListModeler usersListModeler = new FiltersListModeler(this.Request, this.UserSession);
            status = usersListModeler.List(out filtersListViewModelNEW);
            if (!questStatusDef.IsSuccess(status))
            {
                status = new questStatus(Severity.Success);
                filtersListViewModel.questStatus = status;
                return Json(filtersListViewModel, JsonRequestBehavior.AllowGet);
            }

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Return view
             *---------------------------------------------------------------------------------------------------------------------------------*/
            status = new questStatus(Severity.Success);
            filtersListViewModelNEW.questStatus = status;
            return Json(filtersListViewModelNEW, JsonRequestBehavior.AllowGet);
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
        public ActionResult Clear(FiltersListViewModel viewModel)
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
            FiltersListModeler filtersListModeler = new FiltersListModeler(this.Request, this.UserSession);
            status = filtersListModeler.Clear(viewModel);
            if (!questStatusDef.IsSuccess(status))
            {
                viewModel.questStatus = status;
                return Json(viewModel, JsonRequestBehavior.AllowGet);
            }

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Return result.
             *---------------------------------------------------------------------------------------------------------------------------------*/
            status = new questStatus(Severity.Success, "Filter log successfully cleared");
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
            FiltersListModeler filtersListModeler = new FiltersListModeler(this.Request, this.UserSession);
            status = filtersListModeler.Delete(viewModel);
            if (!questStatusDef.IsSuccess(status))
            {
                viewModel.questStatus = status;
                return Json(viewModel, JsonRequestBehavior.AllowGet);
            }

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Return result.
             *---------------------------------------------------------------------------------------------------------------------------------*/
            status = new questStatus(Severity.Success, "Selected Filter log entries successfully deleted");
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
        private void writeTsv(FiltersListViewModel filtersListViewModel, TextWriter output)
        {
            PropertyInfo[] propertyInfos = typeof(FilterLineItemViewModel).GetProperties();
            foreach (PropertyInfo pi in propertyInfos)
            {
                output.Write(pi.Name); // header
                output.Write("\t");
            }
            output.WriteLine();
            foreach (FilterLineItemViewModel lineItem in filtersListViewModel.Items)
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
