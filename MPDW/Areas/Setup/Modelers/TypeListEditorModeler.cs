using System;
using System.Configuration;
using System.Linq;
using System.Web;
using Quest.Util.Status;
using Quest.Util.Buffer;
using Quest.Util.Data;
using Quest.Functional.ASM;
using Quest.Functional.FMS;
using Quest.Functional.MasterPricing;
using Quest.MasterPricing.Setup.Models;
using Quest.MasterPricing.Services.Business.Database;


namespace Quest.MasterPricing.Setup.Modelers
{
    public class TypeListEditorModeler : SetupBaseModeler
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
        public TypeListEditorModeler(HttpRequestBase httpRequestBase, UserSession userSession)
            : base(httpRequestBase, userSession)
        {
            initialize();
        }
        #endregion


        #region Public Methods
        /*==================================================================================================================================
        * Public Methods
        *=================================================================================================================================*/

        #region CRUD 
        //----------------------------------------------------------------------------------------------------------------------------------
        // CRUD
        //----------------------------------------------------------------------------------------------------------------------------------
        public questStatus Save(TypeListEditorViewModel typeListEditorViewModel)
        {
            // Initialize
            questStatus status = null;


            // Transfer model
            Quest.Functional.MasterPricing.TypeList typeList = new Functional.MasterPricing.TypeList();
            BufferMgr.TransferBuffer(typeListEditorViewModel, typeList);


            // Determine if this is a create or update
            TypeListsMgr typeListsMgr = new TypeListsMgr(this.UserSession);
            if (typeListEditorViewModel.Id < BaseId.VALID_ID)
            {
                // Create
                TypeListId typeListId = null;
                status = typeListsMgr.Create(typeList, out typeListId);
                if (!questStatusDef.IsSuccess(status))
                {
                    FormatErrorMessage(status, typeListEditorViewModel);
                    return (status);
                }
                typeListEditorViewModel.Id = typeListId.Id;
            }
            else
            {
                // Update
                status = typeListsMgr.Update(typeList);
                if (!questStatusDef.IsSuccess(status))
                {
                    FormatErrorMessage(status, typeListEditorViewModel);
                    return (status);
                }
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Read(TypeListId typeListId, out TypeListEditorViewModel typeListEditorViewModel)
        {
            // Initialize
            questStatus status = null;
            typeListEditorViewModel = null;


            // Read
            Quest.Functional.MasterPricing.TypeList typeList = null;
            TypeListsMgr typeListsMgr = new TypeListsMgr(this.UserSession);
            status = typeListsMgr.Read(typeListId, out typeList);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }

            // Transfer model.
            typeListEditorViewModel = new TypeListEditorViewModel(this.UserSession);
            BufferMgr.TransferBuffer(typeList, typeListEditorViewModel);



            return (new questStatus(Severity.Success));
        }
        public questStatus Delete(TypeListId typeListId)
        {
            // Initialize
            questStatus status = null;


            // Delete
            TypeListsMgr typeListsMgr = new TypeListsMgr(this.UserSession);
            status = typeListsMgr.Delete(typeListId);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        #endregion


        #region Options
        //----------------------------------------------------------------------------------------------------------------------------------
        // Options
        //----------------------------------------------------------------------------------------------------------------------------------
        #endregion


        #region Validations
        //----------------------------------------------------------------------------------------------------------------------------------
        // Validations
        //----------------------------------------------------------------------------------------------------------------------------------
        // TODO: VALIDATE MODELS FOR REQUIRED FIELDS IN HERE.
        #endregion

        #endregion


        #region Private Methods
        /*==================================================================================================================================
        * Private Methods
        *=================================================================================================================================*/
        private questStatus initialize()
        {
            return (new questStatus(Severity.Success));
        }
        #endregion
    }
}