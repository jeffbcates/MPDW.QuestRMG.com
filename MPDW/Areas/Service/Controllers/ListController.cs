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
using Quest.MPDW.Services.Data;
using Quest.MPDW.Models;
using Quest.MPDW.Modelers;
using Quest.MPDW.Service.Models;


namespace Quest.MPDW.Service 
{
    public class ListController : ServiceBaseController
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
        public ActionResult Index(ServicesListViewModel serviceListViewModel)
        {
            questStatus status = null;


            /*----------------------------------------------------------------------------------------------------------------------------------
             * Log Operation
             *---------------------------------------------------------------------------------------------------------------------------------*/
            status = LogOperation();
            if (!questStatusDef.IsSuccess(status))
            {
                serviceListViewModel.questStatus = status;
                return Json(serviceListViewModel, JsonRequestBehavior.AllowGet);
            }

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Authorize
             *---------------------------------------------------------------------------------------------------------------------------------*/
            status = Authorize(serviceListViewModel._ctx);
            if (!questStatusDef.IsSuccess(status))
            {
                serviceListViewModel.questStatus = status;
                return Json(serviceListViewModel, JsonRequestBehavior.AllowGet);
            }

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Get list of Locations for this user.
             *---------------------------------------------------------------------------------------------------------------------------------*/
            // TODO.
            ServicesListViewModel servicesListViewModel = new ServicesListViewModel(this.UserSession);

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Return view
             *---------------------------------------------------------------------------------------------------------------------------------*/
            status = new questStatus(Severity.Warning, "Not implemented yet");
            servicesListViewModel.questStatus = status;
            return Json(servicesListViewModel, JsonRequestBehavior.AllowGet);
        }


        #region Paging
        //----------------------------------------------------------------------------------------------------------------------------------
        // Paging
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
        #endregion

        #endregion


        #region Private Methods
        /*==================================================================================================================================
         * Private Methods
         *=================================================================================================================================*/
        #endregion
    }
}


