﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Quest.MPDW.Controllers;
using Quest.Util.Status;
using Quest.Util.Buffer;
using Quest.Functional.FMS;
using Quest.MPDW.Services.Data;
using Quest.MPDW.Modelers;
using Quest.MasterPricing.Setup.Modelers;


namespace Quest.MasterPricing.Setup
{
    public class SetupBaseController : BaseController
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

        #region Options
        //----------------------------------------------------------------------------------------------------------------------------------
        // Options
        //----------------------------------------------------------------------------------------------------------------------------------
        [HttpGet]
        public ActionResult DatabaseIdOptions()
        {
            questStatus status = null;


            /*----------------------------------------------------------------------------------------------------------------------------------
             * Log Operation
             *---------------------------------------------------------------------------------------------------------------------------------*/
            status = LogOperation();
            if (!questStatusDef.IsSuccess(status))
            {
                UserMessageModeler userMessageModeler = new UserMessageModeler(status);
                return Json(userMessageModeler.UserMessage, JsonRequestBehavior.AllowGet);
            }

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Authorize
             *---------------------------------------------------------------------------------------------------------------------------------*/
            status = Authorize();
            if (!questStatusDef.IsSuccess(status))
            {
                UserMessageModeler userMessageModeler = new UserMessageModeler(status);
                return Json(userMessageModeler.UserMessage, JsonRequestBehavior.AllowGet);
            }

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Perform Operation
             *---------------------------------------------------------------------------------------------------------------------------------*/
            List<OptionValuePair> optionValuePairList = null;
            DatabaseEditorModeler databaseEditorModeler = new DatabaseEditorModeler(this.Request, this.UserSession);
            status = databaseEditorModeler.GetDatabaseOptions(out optionValuePairList);
            if (!questStatusDef.IsSuccess(status))
            {
                UserMessageModeler userMessageModeler = new UserMessageModeler(status);
                return Json(userMessageModeler.UserMessage, JsonRequestBehavior.AllowGet);
            }

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Respond
             *---------------------------------------------------------------------------------------------------------------------------------*/
            return Json(optionValuePairList, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult TablesetIdOptions()
        {
            questStatus status = null;


            /*----------------------------------------------------------------------------------------------------------------------------------
             * Log Operation
             *---------------------------------------------------------------------------------------------------------------------------------*/
            status = LogOperation();
            if (!questStatusDef.IsSuccess(status))
            {
                UserMessageModeler userMessageModeler = new UserMessageModeler(status);
                return Json(userMessageModeler.UserMessage, JsonRequestBehavior.AllowGet);
            }

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Authorize
             *---------------------------------------------------------------------------------------------------------------------------------*/
            status = Authorize();
            if (!questStatusDef.IsSuccess(status))
            {
                UserMessageModeler userMessageModeler = new UserMessageModeler(status);
                return Json(userMessageModeler.UserMessage, JsonRequestBehavior.AllowGet);
            }

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Perform Operation
             *---------------------------------------------------------------------------------------------------------------------------------*/
            List<OptionValuePair> optionValuePairList = null;
            TablesetEditorModeler tablesetEditorModeler = new TablesetEditorModeler(this.Request, this.UserSession);
            status = tablesetEditorModeler.GetTablesetOptions(out optionValuePairList);
            if (!questStatusDef.IsSuccess(status))
            {
                UserMessageModeler userMessageModeler = new UserMessageModeler(status);
                return Json(userMessageModeler.UserMessage, JsonRequestBehavior.AllowGet);
            }

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Respond
             *---------------------------------------------------------------------------------------------------------------------------------*/
            return Json(optionValuePairList, JsonRequestBehavior.AllowGet);
        }
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
