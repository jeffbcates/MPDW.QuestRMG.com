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
using Quest.Functional.ASM;
using Quest.Functional.FMS;
using Quest.MPDW.Services.Data;
using Quest.MPDW.Models;
using Quest.MPDW.Modelers;
using Quest.MPDW.Admin.Models;
using Quest.MPDW.Admin.Modelers;


namespace Quest.MPDW.Admin
{
    public class AdminBaseController : BaseController
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
        public ActionResult UserOptions(UserEditorViewModel editorViewModel)
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
            status = Authorize(editorViewModel._ctx);
            if (!questStatusDef.IsSuccess(status))
            {
                UserMessageModeler userMessageModeler = new UserMessageModeler(status);
                return Json(userMessageModeler.UserMessage, JsonRequestBehavior.AllowGet);
            }

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Perform Operation
             *---------------------------------------------------------------------------------------------------------------------------------*/
            string userId = editorViewModel.Id < BaseId.VALID_ID ? null : new UserId(editorViewModel.Id).ToString();
            string username = editorViewModel.Username == null ? null : editorViewModel.Username.ToString();
            List<OptionValuePair> optionValuePairList = null;
            UserEditorModeler userEditorModeler = new UserEditorModeler(this.Request, this.UserSession);
            status = userEditorModeler.GetUserOptions(out optionValuePairList, userId, username);
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
