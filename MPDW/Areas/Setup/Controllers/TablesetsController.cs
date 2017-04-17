using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using Quest.Util.Status;
using Quest.Util.Buffer;
using Quest.MPDW.Models;
using Quest.MasterPricing.Setup.Models;
using Quest.MasterPricing.Setup.Modelers;
using Quest.Functional.MasterPricing;
using Quest.MasterPricing.DataMgr.Models;
using Quest.MasterPricing.DataMgr.Modelers;


namespace Quest.MasterPricing.Setup
{
    public class TablesetsController : SetupBaseController
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
            TablesetsListViewModel tablesetsListViewModel = new TablesetsListViewModel(this.UserSession, baseUserSessionViewModel);
            return View(tablesetsListViewModel);
        }
        [HttpGet]
        public ActionResult List(TablesetsListViewModel viewModel)
        {
            questStatus status = null;

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Log Operation
             *---------------------------------------------------------------------------------------------------------------------------------*/
            status = LogOperation();
            if (!questStatusDef.IsSuccess(status))
            {
                status = new questStatus(Severity.Success);
                viewModel.questStatus = status;
                return Json(viewModel, JsonRequestBehavior.AllowGet);
            }

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Authorize
             *---------------------------------------------------------------------------------------------------------------------------------*/
            status = Authorize(viewModel._ctx);
            if (!questStatusDef.IsSuccess(status))
            {
                status = new questStatus(Severity.Success);
                viewModel.questStatus = status;
                return Json(viewModel, JsonRequestBehavior.AllowGet);
            }

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Get list of items.
             *---------------------------------------------------------------------------------------------------------------------------------*/
            TablesetsListViewModel tablesetsListViewModelNEW = null;
            TablesetsListModeler tablesetListModeler = new TablesetsListModeler(this.Request, this.UserSession);
            status = tablesetListModeler.List(out tablesetsListViewModelNEW);
            if (!questStatusDef.IsSuccess(status))
            {
                viewModel.questStatus = status;
                return Json(viewModel, JsonRequestBehavior.AllowGet);
            }

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Return view
             *---------------------------------------------------------------------------------------------------------------------------------*/
            status = new questStatus(Severity.Success);
            tablesetsListViewModelNEW.questStatus = status;
            return Json(tablesetsListViewModelNEW, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult Configuration(TablesetEditorViewModel viewModel)
        {
            questStatus status = null;
            TablesetsListViewModel tablesetsListViewModel = null;

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Log Operation
             *---------------------------------------------------------------------------------------------------------------------------------*/
            status = LogOperation();
            if (!questStatusDef.IsSuccess(status))
            {
                tablesetsListViewModel = new TablesetsListViewModel(this.UserSession, viewModel);
                tablesetsListViewModel.questStatus = status;
                return (View("Index", viewModel));
            }

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Authorize
             *---------------------------------------------------------------------------------------------------------------------------------*/
            status = Authorize(viewModel._ctx);
            if (!questStatusDef.IsSuccess(status))
            {
                tablesetsListViewModel = new TablesetsListViewModel(this.UserSession, viewModel);
                tablesetsListViewModel.questStatus = status;
                return (View("Index", viewModel));
            }

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Read tableset configuration.
             *---------------------------------------------------------------------------------------------------------------------------------*/
            TablesetConfigurationViewModel tablesetConfigurationViewModel = null;
            TablesetId tablesetId = new TablesetId(viewModel.Id);
            TablesetConfigurationModeler tablesetConfigurationModeler = new TablesetConfigurationModeler(this.Request, this.UserSession, viewModel);
            status = tablesetConfigurationModeler.Read(tablesetId, out tablesetConfigurationViewModel);
            if (!questStatusDef.IsSuccess(status))
            {
                tablesetsListViewModel = new TablesetsListViewModel(this.UserSession, viewModel);
                tablesetsListViewModel.questStatus = status;
                return (View("Index", viewModel));
            }

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Return view.
             *---------------------------------------------------------------------------------------------------------------------------------*/
            return View("~/Areas/Setup/Views/TablesetConfiguration/Index.cshtml", tablesetConfigurationViewModel);
        }
        [HttpGet]
        public ActionResult Filters(TablesetEditorViewModel viewModel)
        {
            questStatus status = null;
            TablesetsListViewModel tablesetsListViewModel = null;

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Log Operation
             *---------------------------------------------------------------------------------------------------------------------------------*/
            status = LogOperation();
            if (!questStatusDef.IsSuccess(status))
            {
                tablesetsListViewModel = new TablesetsListViewModel(this.UserSession, viewModel);
                tablesetsListViewModel.questStatus = status;
                return (View("Index", viewModel));
            }

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Authorize
             *---------------------------------------------------------------------------------------------------------------------------------*/
            status = Authorize(viewModel._ctx);
            if (!questStatusDef.IsSuccess(status))
            {
                tablesetsListViewModel = new TablesetsListViewModel(this.UserSession, viewModel);
                tablesetsListViewModel.questStatus = status;
                return (View("Index", viewModel));
            }

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Read tableset data management info.
             *---------------------------------------------------------------------------------------------------------------------------------*/
            DataMgrBaseViewModel dataMgrBaseViewModel = new DataMgrBaseViewModel(this.UserSession);
            BufferMgr.TransferBuffer(viewModel, dataMgrBaseViewModel);

            TablesetId tablesetId = new TablesetId(viewModel.Id);
            DataMgrTablesetViewModel dataMgrTablesetViewModel = null;
            TablesetDataModeler tablesetDataModeler = new TablesetDataModeler(this.Request, this.UserSession, dataMgrBaseViewModel);
            status = tablesetDataModeler.Read(tablesetId, out dataMgrTablesetViewModel);
            if (!questStatusDef.IsSuccess(status))
            {
                dataMgrTablesetViewModel = new DataMgrTablesetViewModel(this.UserSession, viewModel);
                dataMgrTablesetViewModel.questStatus = status;
                return View(dataMgrTablesetViewModel);
            }

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Return view.
             *---------------------------------------------------------------------------------------------------------------------------------*/
            return View("~/Areas/DataMgr/Views/Tableset/Index.cshtml", dataMgrTablesetViewModel);
        }
        [HttpGet]
        public ActionResult Database(TablesetEditorViewModel viewModel)
        {
            questStatus status = null;
            TablesetsListViewModel tablesetsListViewModel = null;

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Log Operation
             *---------------------------------------------------------------------------------------------------------------------------------*/
            status = LogOperation();
            if (!questStatusDef.IsSuccess(status))
            {
                tablesetsListViewModel = new TablesetsListViewModel(this.UserSession, viewModel);
                tablesetsListViewModel.questStatus = status;
                return (View("Index", viewModel));
            }

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Authorize
             *---------------------------------------------------------------------------------------------------------------------------------*/
            status = Authorize(viewModel._ctx);
            if (!questStatusDef.IsSuccess(status))
            {
                tablesetsListViewModel = new TablesetsListViewModel(this.UserSession, viewModel);
                tablesetsListViewModel.questStatus = status;
                return (View("Index", viewModel));
            }

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Read tableset for the database Id
             *---------------------------------------------------------------------------------------------------------------------------------*/
            TablesetId tablesetId = new TablesetId(viewModel.Id);
            TablesetEditorViewModel tablesetEditorViewModel = null;
            TablesetEditorModeler tablesetEditorModeler = new TablesetEditorModeler(this.Request, this.UserSession);
            status = tablesetEditorModeler.Read(tablesetId, out tablesetEditorViewModel);
            if (!questStatusDef.IsSuccess(status))
            {
                tablesetsListViewModel = new TablesetsListViewModel(this.UserSession, viewModel);
                tablesetsListViewModel.questStatus = status;
                return (View("Index", viewModel));
            }

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Return view.
             *---------------------------------------------------------------------------------------------------------------------------------*/
            DatabaseEditorViewModel databaseEditorViewModel = new DatabaseEditorViewModel(this.UserSession, viewModel);
            databaseEditorViewModel.Id = tablesetEditorViewModel.DatabaseId;
            databaseEditorViewModel.questStatus = new questStatus(Severity.Warning);
            return View("~/Areas/Setup/Views/Database/Index.cshtml", databaseEditorViewModel);
        }

        #region Paging
        //----------------------------------------------------------------------------------------------------------------------------------
        // Paging
        //----------------------------------------------------------------------------------------------------------------------------------
        [HttpGet]
        public ActionResult First(TablesetsListViewModel requisitionListViewModel)
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
             * Get list of items.
             *---------------------------------------------------------------------------------------------------------------------------------*/
            TablesetsListViewModel tablesetsListViewModelNEW = null;
            TablesetsListModeler tablesetsListModeler = new TablesetsListModeler(this.Request, this.UserSession);
            status = tablesetsListModeler.List(out tablesetsListViewModelNEW);
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
            tablesetsListViewModelNEW.questStatus = status;
            return Json(tablesetsListViewModelNEW, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult Prev(TablesetsListViewModel requisitionListViewModel)
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
             * Get list of items.
             *---------------------------------------------------------------------------------------------------------------------------------*/
            // TODO.
            TablesetsListViewModel tablesetsListViewModelNEW = null;
            TablesetsListModeler tablesetsListModeler = new TablesetsListModeler(this.Request, this.UserSession);
            status = tablesetsListModeler.List(out tablesetsListViewModelNEW);
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
            tablesetsListViewModelNEW.questStatus = status;
            return Json(tablesetsListViewModelNEW, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult PageNum(TablesetsListViewModel requisitionListViewModel)
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
             * Get list of items.
             *---------------------------------------------------------------------------------------------------------------------------------*/
            // TODO.
            TablesetsListViewModel tablesetsListViewModelNEW = null;
            TablesetsListModeler tablesetsListModeler = new TablesetsListModeler(this.Request, this.UserSession);
            status = tablesetsListModeler.List(out tablesetsListViewModelNEW);
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
            tablesetsListViewModelNEW.questStatus = status;
            return Json(tablesetsListViewModelNEW, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult Next(TablesetsListViewModel requisitionListViewModel)
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
             * Get list of items.
             *---------------------------------------------------------------------------------------------------------------------------------*/
            // TODO.
            TablesetsListViewModel tablesetsListViewModelNEW = null;
            TablesetsListModeler tablesetsListModeler = new TablesetsListModeler(this.Request, this.UserSession);
            status = tablesetsListModeler.List(out tablesetsListViewModelNEW);
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
            tablesetsListViewModelNEW.questStatus = status;
            return Json(tablesetsListViewModelNEW, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult Last(TablesetsListViewModel requisitionListViewModel)
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
             * Get list of items.
             *---------------------------------------------------------------------------------------------------------------------------------*/
            // TODO.
            TablesetsListViewModel tablesetsListViewModelNEW = null;
            TablesetsListModeler tablesetsListModeler = new TablesetsListModeler(this.Request, this.UserSession);
            status = tablesetsListModeler.List(out tablesetsListViewModelNEW);
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
