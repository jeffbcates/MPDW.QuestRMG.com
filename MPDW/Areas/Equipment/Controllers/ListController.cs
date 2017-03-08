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
using Quest.MPDW.Models;
using Quest.MPDW.Modelers;
using Quest.MPDW.Equipment.Models;


namespace Quest.MPDW.Equipment 
{
    public class ListController : EquipmentBaseController
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
        public ActionResult Index(EquipmentsListViewModel equipmentListViewModel)
        {
            questStatus status = null;


            /*----------------------------------------------------------------------------------------------------------------------------------
             * Log Operation
             *---------------------------------------------------------------------------------------------------------------------------------*/
            status = LogOperation();
            if (!questStatusDef.IsSuccess(status))
            {
                equipmentListViewModel.questStatus = status;
                return Json(equipmentListViewModel, JsonRequestBehavior.AllowGet);
            }

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Authorize
             *---------------------------------------------------------------------------------------------------------------------------------*/
            status = Authorize(equipmentListViewModel._ctx);
            if (!questStatusDef.IsSuccess(status))
            {
                equipmentListViewModel.questStatus = status;
                return Json(equipmentListViewModel, JsonRequestBehavior.AllowGet);
            }

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Get list of Locations for this user.
             *---------------------------------------------------------------------------------------------------------------------------------*/
            // TODO.
            EquipmentsListViewModel equipmentsListViewModel = new EquipmentsListViewModel(this.UserSession);

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Return view
             *---------------------------------------------------------------------------------------------------------------------------------*/
            status = new questStatus(Severity.Warning, "Not implemented yet");
            equipmentsListViewModel.questStatus = status;
            return Json(equipmentsListViewModel, JsonRequestBehavior.AllowGet);
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


