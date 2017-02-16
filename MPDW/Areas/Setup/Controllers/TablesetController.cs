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
using Quest.Functional.FMS;
using Quest.Functional.MasterPricing;
using Quest.MPDW.Services.Data;
using Quest.MPDW.Modelers;
using Quest.MasterPricing.Setup.Models;
using Quest.MasterPricing.Setup.Modelers;


namespace Quest.MasterPricing.Setup
{
    public class TablesetController : SetupBaseController
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
        public ActionResult Index(TablesetEditorViewModel editorViewModel)
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
             * Return view.
             *---------------------------------------------------------------------------------------------------------------------------------*/
            TablesetEditorViewModel tablesetEditorViewModel = new TablesetEditorViewModel(this.UserSession, editorViewModel);
            tablesetEditorViewModel.Id = editorViewModel.Id;
            if (editorViewModel.Id >= BaseId.VALID_ID)
            {
                tablesetEditorViewModel.questStatus = new questStatus(Severity.Warning);
            }
            else
            {
                tablesetEditorViewModel.questStatus = new questStatus(Severity.Success);
            }
            return View(tablesetEditorViewModel);
        }
        [HttpGet]
        public ActionResult Read(TablesetEditorViewModel editorViewModel)
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
            TablesetId tablesetId = new TablesetId(editorViewModel.Id);
            TablesetEditorViewModel tablesetEditorViewModel = null;
            TablesetEditorModeler tablesetEditorModeler = new Modelers.TablesetEditorModeler(this.Request, this.UserSession);
            status = tablesetEditorModeler.Read(tablesetId, out tablesetEditorViewModel);
            if (!questStatusDef.IsSuccess(status))
            {
                userMessageModeler = new UserMessageModeler(status);
                return Json(userMessageModeler, JsonRequestBehavior.AllowGet);
            }

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Return result.
             *---------------------------------------------------------------------------------------------------------------------------------*/
            status = new questStatus(Severity.Success);
            tablesetEditorViewModel.questStatus = status;
            return Json(tablesetEditorViewModel, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult Cancel(TablesetEditorViewModel editorViewModel)
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
            return (RedirectToAction("Index", "Tablesets", PropagateQueryString(Request)));
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
        public ActionResult Save(TablesetEditorViewModel tablesetEditorViewModel)
        {
            questStatus status = null;

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Log Operation
             *---------------------------------------------------------------------------------------------------------------------------------*/
            status = LogOperation();
            if (!questStatusDef.IsSuccess(status))
            {
                tablesetEditorViewModel.questStatus = status;
                return Json(tablesetEditorViewModel, JsonRequestBehavior.AllowGet);
            }

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Authorize
             *---------------------------------------------------------------------------------------------------------------------------------*/
            status = Authorize(tablesetEditorViewModel._ctx);
            if (!questStatusDef.IsSuccess(status))
            {
                tablesetEditorViewModel.questStatus = status;
                return Json(tablesetEditorViewModel, JsonRequestBehavior.AllowGet);
            }

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Perform operation.
             *---------------------------------------------------------------------------------------------------------------------------------*/
            bool bInitialCreation = tablesetEditorViewModel.Id < BaseId.VALID_ID ? true : false;
            TablesetEditorModeler tablesetEditorModeler = new TablesetEditorModeler(this.Request, this.UserSession);
            status = tablesetEditorModeler.Save(tablesetEditorViewModel);
            if (!questStatusDef.IsSuccess(status))
            {
                tablesetEditorViewModel.questStatus = status;
                return Json(tablesetEditorViewModel, JsonRequestBehavior.AllowGet);
            }

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Return result.
             *---------------------------------------------------------------------------------------------------------------------------------*/
            status = new questStatus(Severity.Success, "Tableset successfully" + (bInitialCreation ? " created" : " updated"));
            tablesetEditorViewModel.questStatus = status;
            return Json(tablesetEditorViewModel, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult Delete(TablesetEditorViewModel tablesetEditorViewModel)
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
            status = Authorize(tablesetEditorViewModel._ctx);
            if (!questStatusDef.IsSuccess(status))
            {
                // TODO
                throw new Exception("Authorize failed");
            }

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Perform operation.
             *---------------------------------------------------------------------------------------------------------------------------------*/
            TablesetId tablesetId = new TablesetId(tablesetEditorViewModel.Id);
            TablesetEditorModeler tablesetEditorModeler = new TablesetEditorModeler(this.Request, this.UserSession);
            status = tablesetEditorModeler.Delete(tablesetId);
            if (!questStatusDef.IsSuccess(status))
            {
                tablesetEditorViewModel.questStatus = status;
                return Json(tablesetEditorViewModel, JsonRequestBehavior.AllowGet);
            }

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Return result.
             *---------------------------------------------------------------------------------------------------------------------------------*/
            status = new questStatus(Severity.Success, "Tableset successfully deleted");
            tablesetEditorViewModel.questStatus = status;
            return Json(tablesetEditorViewModel, JsonRequestBehavior.AllowGet);
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
