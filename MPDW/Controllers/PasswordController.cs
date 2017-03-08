using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Quest.Util.Status;
using Quest.Util.Buffer;
using Quest.MPDW.Models;
using Quest.MPDW.Modelers;
using Quest.MPDW.Services.Data;


namespace Quest.MPDW.Controllers
{
    public class PasswordController : BaseController
    {
        #region Declarations
        /*==================================================================================================================================
         * Declarations
         *=================================================================================================================================*/
        #endregion


        #region GET Methods
        /*==================================================================================================================================
         * GET Methods
         *=================================================================================================================================*/
        [AllowAnonymous]
        [HttpGet]
        public ActionResult Forgot()
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
            status = Authorize();
            if (!questStatusDef.IsSuccess(status))
            {
                // TODO
                throw new Exception("Authorize failed");
            }

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Return view
             *---------------------------------------------------------------------------------------------------------------------------------*/
            ForgotPasswordViewModel forgotPasswordViewModel = new ForgotPasswordViewModel();
            return View(forgotPasswordViewModel);
        }
        [AllowAnonymous]
        [HttpGet]
        public ActionResult Change()
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
            status = Authorize();
            if (!questStatusDef.IsSuccess(status))
            {
                // TODO
                throw new Exception("Authorize failed");
            }

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Return view
             *---------------------------------------------------------------------------------------------------------------------------------*/
            ChangePasswordViewModel changePasswordViewModel = new ChangePasswordViewModel();
            return View(changePasswordViewModel);
        }
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
