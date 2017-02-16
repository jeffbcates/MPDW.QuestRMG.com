using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using Quest.MPDW.Controllers;
using Quest.Util.Status;
using Quest.Util.Buffer;
using Quest.Util.Data;
using Quest.Functional.MasterPricing;
using Quest.MPDW.Services.Data;
using Quest.MPDW.Models;
using Quest.MPDW.Modelers;
using Quest.MasterPricing.Setup.Models;
using Quest.MasterPricing.Setup.Modelers;


namespace Quest.MasterPricing.Setup
{
    public class DatabaseController : SetupBaseController
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
        public ActionResult Index(DatabaseEditorViewModel databaseEditorViewModel)
        {
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
            status = Authorize(databaseEditorViewModel._ctx);
            if (!questStatusDef.IsSuccess(status))
            {
                // TODO
                throw new Exception("Authorize failed");
            }

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Return view.
             *---------------------------------------------------------------------------------------------------------------------------------*/
            DatabaseEditorViewModel databaseEditorViewModel2 = new DatabaseEditorViewModel(this.UserSession, databaseEditorViewModel);
            databaseEditorViewModel2.Id = databaseEditorViewModel.Id;
            if (databaseEditorViewModel.Id >= BaseId.VALID_ID)
            {
                databaseEditorViewModel2.questStatus = new questStatus(Severity.Warning);
            }
            else
            {
                databaseEditorViewModel2.questStatus = new questStatus(Severity.Success);
            }
            return View(databaseEditorViewModel2);

        }
        [HttpGet]
        public ActionResult Read(DatabaseEditorViewModel editorViewModel)
        {
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
            status = Authorize(editorViewModel._ctx);
            if (!questStatusDef.IsSuccess(status))
            {
                // TODO
                throw new Exception("Authorize failed");
            }

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Perform operation.
             *---------------------------------------------------------------------------------------------------------------------------------*/
            DatabaseId databaseId = new DatabaseId(editorViewModel.Id);
            DatabaseEditorViewModel databaseEditorViewModel = null;
            DatabaseEditorModeler databaseEditorModeler = new Modelers.DatabaseEditorModeler(this.Request, this.UserSession);
            status = databaseEditorModeler.Read(databaseId, out databaseEditorViewModel);
            if (!questStatusDef.IsSuccess(status))
            {
                databaseEditorViewModel.questStatus = status;
                return Json(editorViewModel, JsonRequestBehavior.AllowGet);
            }

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Return result.
             *---------------------------------------------------------------------------------------------------------------------------------*/
            status = new questStatus(Severity.Success);
            databaseEditorViewModel.questStatus = status;
            return Json(databaseEditorViewModel, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult Cancel(DatabaseEditorViewModel editorViewModel)
        {
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
            status = Authorize(editorViewModel._ctx);
            if (!questStatusDef.IsSuccess(status))
            {
                // TODO
                throw new Exception("Authorize failed");
            }

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Direct user to list
             *---------------------------------------------------------------------------------------------------------------------------------*/
            return (RedirectToAction("Index", "Databases", PropagateQueryString(Request)));
        }
        [HttpGet]
        public ActionResult Tables(DatabaseEditorViewModel editorViewModel)
        {
            questStatus status = null;
            UserMessageModeler userMessageModeler = null;

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Log Operation
             *---------------------------------------------------------------------------------------------------------------------------------*/
            status = LogOperation();
            if (!questStatusDef.IsSuccess(status))
            {
                userMessageModeler = new UserMessageModeler(status);
                return Json(userMessageModeler, JsonRequestBehavior.AllowGet);
            }

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Authorize
             *---------------------------------------------------------------------------------------------------------------------------------*/
            status = Authorize(editorViewModel._ctx);
            if (!questStatusDef.IsSuccess(status))
            {
                userMessageModeler = new UserMessageModeler(status);
                return Json(userMessageModeler, JsonRequestBehavior.AllowGet);
            }

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Perform operation.
             *---------------------------------------------------------------------------------------------------------------------------------*/
            DatabaseId databaseId = new DatabaseId(editorViewModel.Id);
            List<BootstrapTreenodeViewModel> dbTableNodeList = null;
            TablesetConfigurationModeler tablesetConfigurationModeler = new TablesetConfigurationModeler(this.Request, this.UserSession, editorViewModel);
            status = tablesetConfigurationModeler.GetDatabaseTables(databaseId, out dbTableNodeList);
            if (!questStatusDef.IsSuccess(status))
            {
                userMessageModeler = new UserMessageModeler(status);
                return Json(userMessageModeler, JsonRequestBehavior.AllowGet);
            }

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Return result.
             *---------------------------------------------------------------------------------------------------------------------------------*/
            status = new questStatus(Severity.Success);
            return Json(dbTableNodeList, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult Views(DatabaseEditorViewModel editorViewModel)
        {
            questStatus status = null;
            UserMessageModeler userMessageModeler = null;

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Log Operation
             *---------------------------------------------------------------------------------------------------------------------------------*/
            status = LogOperation();
            if (!questStatusDef.IsSuccess(status))
            {
                userMessageModeler = new UserMessageModeler(status);
                return Json(userMessageModeler, JsonRequestBehavior.AllowGet);
            }

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Authorize
             *---------------------------------------------------------------------------------------------------------------------------------*/
            status = Authorize(editorViewModel._ctx);
            if (!questStatusDef.IsSuccess(status))
            {
                userMessageModeler = new UserMessageModeler(status);
                return Json(userMessageModeler, JsonRequestBehavior.AllowGet);
            }

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Perform operation.
             *---------------------------------------------------------------------------------------------------------------------------------*/
            DatabaseId databaseId = new DatabaseId(editorViewModel.Id);
            List<BootstrapTreenodeViewModel> dbViewNodeList = null;
            TablesetConfigurationModeler tablesetConfigurationModeler = new TablesetConfigurationModeler(this.Request, this.UserSession, editorViewModel);
            status = tablesetConfigurationModeler.GetDatabaseViews(databaseId, out dbViewNodeList);
            if (!questStatusDef.IsSuccess(status))
            {
                userMessageModeler = new UserMessageModeler(status);
                return Json(userMessageModeler, JsonRequestBehavior.AllowGet);
            }

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Return result.
             *---------------------------------------------------------------------------------------------------------------------------------*/
            status = new questStatus(Severity.Success);
            return Json(dbViewNodeList, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult Entities(DatabaseEditorViewModel editorViewModel)
        {
            questStatus status = null;
            UserMessageModeler userMessageModeler = null;

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Log Operation
             *---------------------------------------------------------------------------------------------------------------------------------*/
            status = LogOperation();
            if (!questStatusDef.IsSuccess(status))
            {
                userMessageModeler = new UserMessageModeler(status);
                return Json(userMessageModeler, JsonRequestBehavior.AllowGet);
            }

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Authorize
             *---------------------------------------------------------------------------------------------------------------------------------*/
            status = Authorize(editorViewModel._ctx);
            if (!questStatusDef.IsSuccess(status))
            {
                userMessageModeler = new UserMessageModeler(status);
                return Json(userMessageModeler, JsonRequestBehavior.AllowGet);
            }

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Perform operation.
             *---------------------------------------------------------------------------------------------------------------------------------*/
            DatabaseId databaseId = new DatabaseId(editorViewModel.Id);
            List<BootstrapTreenodeViewModel> dbTableNodeList = null;
            List<BootstrapTreenodeViewModel> dbViewNodeList = null;
            TablesetConfigurationModeler tablesetConfigurationModeler = new TablesetConfigurationModeler(this.Request, this.UserSession, editorViewModel);
            status = tablesetConfigurationModeler.GetDatabaseEntities(databaseId, out dbTableNodeList, out dbViewNodeList);
            if (!questStatusDef.IsSuccess(status))
            {
                userMessageModeler = new UserMessageModeler(status);
                return Json(userMessageModeler, JsonRequestBehavior.AllowGet);
            }

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Return result.
             *---------------------------------------------------------------------------------------------------------------------------------*/
            status = new questStatus(Severity.Success);
            return Json(new { Tables = dbTableNodeList, Views = dbViewNodeList }, JsonRequestBehavior.AllowGet);
        }


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

        #region CRUD Operations
        //----------------------------------------------------------------------------------------------------------------------------------
        // CRUD Operations
        //----------------------------------------------------------------------------------------------------------------------------------
        [HttpPost]
        public ActionResult Save(DatabaseEditorViewModel databaseEditorViewModel)
        {
            questStatus status = null;

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Log Operation
             *---------------------------------------------------------------------------------------------------------------------------------*/
            status = LogOperation();
            if (!questStatusDef.IsSuccess(status))
            {
                databaseEditorViewModel.questStatus = status;
                return Json(databaseEditorViewModel, JsonRequestBehavior.AllowGet);
            }

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Authorize
             *---------------------------------------------------------------------------------------------------------------------------------*/
            status = Authorize(databaseEditorViewModel._ctx);
            if (!questStatusDef.IsSuccess(status))
            {
                databaseEditorViewModel.questStatus = status;
                return Json(databaseEditorViewModel, JsonRequestBehavior.AllowGet);
            }

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Perform operation.
             *---------------------------------------------------------------------------------------------------------------------------------*/
            bool bInitialCreation = databaseEditorViewModel.Id < BaseId.VALID_ID ? true : false;
            DatabaseEditorModeler databaseEditorModeler = new DatabaseEditorModeler(this.Request, this.UserSession);
            status = databaseEditorModeler.Save(databaseEditorViewModel);
            if (!questStatusDef.IsSuccess(status))
            {
                databaseEditorViewModel.questStatus = status;
                return Json(databaseEditorViewModel, JsonRequestBehavior.AllowGet);
            }

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Return result.
             *---------------------------------------------------------------------------------------------------------------------------------*/
            status = new questStatus(Severity.Success, "Database successfully" + (bInitialCreation ? " created" : " updated"));
            databaseEditorViewModel.questStatus = status;
            return Json(databaseEditorViewModel, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult Delete(DatabaseEditorViewModel databaseEditorViewModel)
        {
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
            status = Authorize(databaseEditorViewModel._ctx);
            if (!questStatusDef.IsSuccess(status))
            {
                // TODO
                throw new Exception("Authorize failed");
            }

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Perform operation.
             *---------------------------------------------------------------------------------------------------------------------------------*/
            DatabaseId databaseId = new DatabaseId(databaseEditorViewModel.Id);
            DatabaseEditorModeler databaseEditorModeler = new DatabaseEditorModeler(this.Request, this.UserSession);
            status = databaseEditorModeler.Delete(databaseId);
            if (!questStatusDef.IsSuccess(status))
            {
                databaseEditorViewModel.questStatus = status;
                return Json(databaseEditorViewModel, JsonRequestBehavior.AllowGet);
            }

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Return result.
             *---------------------------------------------------------------------------------------------------------------------------------*/
            status = new questStatus(Severity.Success, "Database successfully deleted");
            databaseEditorViewModel.questStatus = status;
            return Json(databaseEditorViewModel, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult RefreshSchema(DatabaseEditorViewModel databaseEditorViewModel)
        {
            questStatus status = null;

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Log Operation
             *---------------------------------------------------------------------------------------------------------------------------------*/
            status = LogOperation();
            if (!questStatusDef.IsSuccess(status))
            {
                databaseEditorViewModel.questStatus = status;
                return Json(databaseEditorViewModel, JsonRequestBehavior.AllowGet);
            }

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Authorize
             *---------------------------------------------------------------------------------------------------------------------------------*/
            status = Authorize(databaseEditorViewModel._ctx);
            if (!questStatusDef.IsSuccess(status))
            {
                databaseEditorViewModel.questStatus = status;
                return Json(databaseEditorViewModel, JsonRequestBehavior.AllowGet);
            }

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Perform operation.
             *---------------------------------------------------------------------------------------------------------------------------------*/
            DatabaseEditorModeler databaseEditorModeler = new DatabaseEditorModeler(this.Request, this.UserSession);
            status = databaseEditorModeler.RefreshSchema(databaseEditorViewModel);
            if (!questStatusDef.IsSuccess(status))
            {
                databaseEditorViewModel.questStatus = status;
                return Json(databaseEditorViewModel, JsonRequestBehavior.AllowGet);
            }

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Return result.
             *---------------------------------------------------------------------------------------------------------------------------------*/
            status = new questStatus(Severity.Success, "Database scehema successfully refreshed");
            databaseEditorViewModel.questStatus = status;
            return Json(databaseEditorViewModel, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #endregion


        #region Private Methods
        /*==================================================================================================================================
         * Private Methods
         *=================================================================================================================================*/
        #endregion
    }
}
