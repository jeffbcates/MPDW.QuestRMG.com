using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Quest.MPDW.Controllers;
using Quest.Util.Status;
using Quest.Util.Buffer;
using Quest.Functional.ASM;
using Quest.Functional.FMS;
using Quest.MPDW.Services.Data;
using Quest.MPDW.Models;
using Quest.MPDW.Modelers;
using Quest.MasterPricing.Database.Models;
using Quest.MasterPricing.Setup.Models;
using Quest.MasterPricing.Database.Modelers;
using Quest.Functional.MasterPricing;


namespace Quest.MasterPricing.Database
{
    public class ParametersController : BaseController
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
        public ActionResult Index(ParameterListViewModel viewModel)
        {
            questStatus status = null;

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Log Operation
             *---------------------------------------------------------------------------------------------------------------------------------*/
            status = LogOperation();
            if (!questStatusDef.IsSuccess(status))
            {
                viewModel.questStatus = status;
                return (View("Index", viewModel));
            }

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Authorize
             *---------------------------------------------------------------------------------------------------------------------------------*/
            status = Authorize(viewModel._ctx);
            if (!questStatusDef.IsSuccess(status))
            {
                viewModel.questStatus = status;
                return (View("Index", viewModel));
            }

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Read the database info.
             *---------------------------------------------------------------------------------------------------------------------------------*/
            ParameterListViewModel parameterListViewModel = null;
            ParameterListModeler parameterListModeler = new ParameterListModeler(this.Request, this.UserSession);
            status = parameterListModeler.GetParamerListInfo(viewModel, out parameterListViewModel);
            if (!questStatusDef.IsSuccess(status))
            {
                viewModel.questStatus = status;
                return (View("Index", viewModel));
            }

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Return view.
             *---------------------------------------------------------------------------------------------------------------------------------*/
            parameterListViewModel.questStatus = new questStatus(Severity.Success);
            return View(parameterListViewModel);
        }
        [HttpGet]
        public ActionResult List(ParameterListViewModel viewModel)
        {
            // Initialize
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
            status = Authorize(viewModel._ctx);
            if (!questStatusDef.IsSuccess(status))
            {
                userMessageModeler = new UserMessageModeler(status);
                return Json(userMessageModeler, JsonRequestBehavior.AllowGet);
            }

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Get stored procedure parameters.
             *---------------------------------------------------------------------------------------------------------------------------------*/
            ParameterListViewModel parameterListViewModel = null;
            ParameterListModeler parameterListModeler = new ParameterListModeler(this.Request, this.UserSession);
            status = parameterListModeler.List(viewModel, out parameterListViewModel);
            if (!questStatusDef.IsSuccess(status))
            {
                userMessageModeler = new UserMessageModeler(status);
                return Json(userMessageModeler, JsonRequestBehavior.AllowGet);
            }

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Return data.
             *---------------------------------------------------------------------------------------------------------------------------------*/
            status = new questStatus(Severity.Success);
            parameterListViewModel.questStatus = status;
            return Json(parameterListViewModel, JsonRequestBehavior.AllowGet);
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
        [HttpPost]
        public ActionResult MakeRequired(MakeRequiredViewModel viewModel)
        {
            questStatus status = null;

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Log Operation
             *---------------------------------------------------------------------------------------------------------------------------------*/
            status = LogOperation();
            if (!questStatusDef.IsSuccess(status))
            {
                viewModel.questStatus = status;
                return Json(viewModel);
            }

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Authorize
             *---------------------------------------------------------------------------------------------------------------------------------*/
            status = Authorize(viewModel._ctx);
            if (!questStatusDef.IsSuccess(status))
            {
                viewModel.questStatus = status;
                return Json(viewModel);
            }

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Perform operation.
             *---------------------------------------------------------------------------------------------------------------------------------*/
            ParameterListModeler parameterListModeler = new ParameterListModeler(this.Request, this.UserSession);
            status = parameterListModeler.MakeRequired(viewModel);
            if (!questStatusDef.IsSuccess(status))
            {
                viewModel.questStatus = status;
                return Json(viewModel);
            }

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Return result.
             *---------------------------------------------------------------------------------------------------------------------------------*/
            status = new questStatus(Severity.Success, "Parameters successfull made as required.");
            viewModel.questStatus = status;
            return Json(viewModel);
        }
        #endregion


        #region Private Methods
        /*==================================================================================================================================
         * Private Methods
         *=================================================================================================================================*/
        #endregion
    }
}
