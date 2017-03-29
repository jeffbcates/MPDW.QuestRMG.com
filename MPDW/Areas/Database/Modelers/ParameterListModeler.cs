using System;
using System.Collections.Generic;
using System.Web;
using Quest.Util.Status;
using Quest.Util.Buffer;
using Quest.Util.Data;
using Quest.Functional.ASM;
using Quest.Functional.FMS;
using Quest.MPDW.Models.List;
using Quest.MPDW.Modelers;
using Quest.MasterPricing.Database.Models;
using Quest.Functional.MasterPricing;
using Quest.MasterPricing.Services.Business.Database;
using Quest.MPDW.Services.Business;
using Quest.MPDW.Services.Data;
using Quest.MasterPricing.Services.Business.Filters;


namespace Quest.MasterPricing.Database.Modelers
{
    public class ParameterListModeler : BaseListModeler
    {
        #region Declarations
        /*==================================================================================================================================
         * Declarations
         *=================================================================================================================================*/
        private ParameterListViewModel _parameterListViewModel = null;

        #endregion


        #region Constructors
        /*==================================================================================================================================
        * ConstructorsRequisitionListModeler
        *=================================================================================================================================*/
        public ParameterListModeler(HttpRequestBase httpRequestBase, UserSession userSession)
            : base(httpRequestBase, userSession)
        {
            initialize();
        }
        public ParameterListModeler(HttpRequestBase httpRequestBase, UserSession userSession, ParameterListViewModel parameterListViewModel)
            : base(httpRequestBase, userSession)
        {
            this._parameterListViewModel = parameterListViewModel;
            initialize();
        }
        #endregion


        #region Public Methods
        /*==================================================================================================================================
         * Public Methods
         *=================================================================================================================================*/

        #region LIST 
        //----------------------------------------------------------------------------------------------------------------------------------
        // LIST
        //----------------------------------------------------------------------------------------------------------------------------------
        public questStatus GetParamerListInfo(ParameterListViewModel viewModel, out ParameterListViewModel parameterListViewModel)
        {
            // Initialize
            questStatus status = null;
            parameterListViewModel = null;
            DatabaseId databaseId = new DatabaseId(viewModel.DatabaseId);


            // Read the database
            DatabaseBaseViewModel databaseBaseViewModel = null;
            DatabaseBaseModeler databaseBaseModeler = new DatabaseBaseModeler(this.HttpRequestBase, this.UserSession);
            status = databaseBaseModeler.GetDatabase(databaseId, out databaseBaseViewModel);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            
            // Read the stored procedure
            StoredProceduresListViewModel storedProceduresListViewModel = null;
            StoredProceduresListModeler storedProceduresListModeler = new StoredProceduresListModeler(this.HttpRequestBase, this.UserSession);
            status = storedProceduresListModeler.List(databaseId, out storedProceduresListViewModel);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }


            // Get stored procedure info
            StoredProcedureId storedProcedureId = new StoredProcedureId(viewModel.Id);


            // Read the stored procedure info
            StoredProcedure storedProcedure = null;
            StoredProceduresMgr storedProceduresMgr = new StoredProceduresMgr(this.UserSession);
            status = storedProceduresMgr.Read(storedProcedureId, out storedProcedure);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }

            // Get stored procedure parameters
            viewModel.StoredProcedureId = viewModel.Id;
            ParameterListViewModel _parameterListViewModel = null;
            status = List(viewModel, out _parameterListViewModel);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }


            // Sort by display order
            // NOTE: ORDER IN DATABASE -SHOULD- BE ORDER IN SPROC.


            // Transfer model.
            parameterListViewModel = new ParameterListViewModel(this.UserSession, viewModel);
            parameterListViewModel.DatabaseId = databaseId.Id;
            parameterListViewModel.Database = databaseBaseViewModel;

            foreach (StoredProcedureLineItemViewModel storedProcedureLineItemViewModel in storedProceduresListViewModel.Items)
            {
                OptionValuePair opv = new OptionValuePair();
                opv.Id = storedProcedureLineItemViewModel.Id.ToString();
                opv.Label = storedProcedureLineItemViewModel.Name;
                parameterListViewModel.StoredProcedureOptions.Add(opv);
            }

            parameterListViewModel.ParentEntityType = viewModel.ParentEntityType;
            parameterListViewModel.StoredProcedureId = viewModel.StoredProcedureId;

            StoredProcedureViewModel storedProcedureViewModel = new StoredProcedureViewModel();
            BufferMgr.TransferBuffer(storedProcedure, storedProcedureViewModel);
            parameterListViewModel.StoredProcedure = storedProcedureViewModel;
            parameterListViewModel.Items = _parameterListViewModel.Items;

            return (new questStatus(Severity.Success));
        }
        public questStatus MakeRequired(MakeRequiredViewModel makeRequiredViewModel)
        {
            // Initialize
            questStatus status = null;
            Mgr mgr = new Mgr(this.UserSession);
            DbMgrTransaction trans = null;


            // BEGIN TRANSACTION
            status = mgr.BeginTransaction("MakeRequired" + Guid.NewGuid().ToString(), out trans);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }


            // Update every parameter as required or not.
            StoredProcedureParametersMgr storedProcedureParametersMgr = new StoredProcedureParametersMgr(this.UserSession);
            foreach (BaseId baseId in makeRequiredViewModel.Items)
            {
                // Get the parameter
                StoredProcedureParameterId storedProcedureParameterId = new StoredProcedureParameterId(baseId.Id);
                StoredProcedureParameter storedProcedureParameter = null;
                status = storedProcedureParametersMgr.Read(trans, storedProcedureParameterId, out storedProcedureParameter);
                if (!questStatusDef.IsSuccess(status))
                {
                    mgr.RollbackTransaction(trans);
                    return (status);
                }

                // Make sure same stored procedure
                if (makeRequiredViewModel.StoredProcedureId != storedProcedureParameter.StoredProcedureId)
                {
                    mgr.RollbackTransaction(trans);
                    return (new questStatus(Severity.Error, "All parameter Id's must be from the same stored procedure!"));
                }

                // Set required flag accordingly and update.
                storedProcedureParameter.bRequired = true;
                status = storedProcedureParametersMgr.Update(trans, storedProcedureParameter);
                if (!questStatusDef.IsSuccess(status))
                {
                    mgr.RollbackTransaction(trans);
                    return (status);
                }
            }

            // Set all the OTHER parameters to false
            List<StoredProcedureParameter> storedProcedureParameterList = null;
            StoredProcedureId storedProcedureId = new StoredProcedureId(makeRequiredViewModel.StoredProcedureId);
            status = storedProcedureParametersMgr.Read(trans, storedProcedureId, out storedProcedureParameterList);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            foreach (StoredProcedureParameter storedProcedureParameter in storedProcedureParameterList)
            {
                BaseId baseId = makeRequiredViewModel.Items.Find(delegate (BaseId _baseId) { return (_baseId.Id == storedProcedureParameter.Id); });
                if (baseId == null)
                {
                    storedProcedureParameter.bRequired = false;
                    status = storedProcedureParametersMgr.Update(trans, storedProcedureParameter);
                    if (!questStatusDef.IsSuccess(status))
                    {
                        mgr.RollbackTransaction(trans);
                        return (status);
                    }
                }
            }

            // COMMIT TRANSACTION
            // THIS HAS TO BE COMMITTED HERE TO AVOID DEADLOCKING ON UPDATING THE FILTERS, NEXT.
            status = mgr.CommitTransaction(trans);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }

            ////// BEGIN TRANSACTION
            ////status = mgr.BeginTransaction("MakeRequired_RefreshFilters" + Guid.NewGuid().ToString(), out trans);
            ////if (!questStatusDef.IsSuccess(status))
            ////{
            ////    return (status);
            ////}


            ////// Refresh filters using this sproc.
            ////List<Quest.Functional.MasterPricing.FilterId> updatedFilterIds = null;
            ////StoredProceduresMgr storedProceduresMgr = new StoredProceduresMgr(this.UserSession);
            ////status = storedProceduresMgr.RefreshFilters(storedProcedureId, out updatedFilterIds);
            ////if (!questStatusDef.IsSuccess(status))
            ////{
            ////    mgr.RollbackTransaction(trans);
            ////    return (status);
            ////}


            ////// COMMIT TRANSACTION
            ////status = mgr.CommitTransaction(trans);
            ////if (!questStatusDef.IsSuccess(status))
            ////{
            ////    return (status);
            ////}


            ////// TODO: REFACTOR TO GET ALL-IN-ONE TRANSACTION
            ////foreach (FilterId filterId in updatedFilterIds)
            ////{
            ////    Quest.Functional.MasterPricing.Filter filterWithSQL = null;
            ////    FilterMgr filterMgr = new FilterMgr(this.UserSession);
            ////    status = filterMgr.GenerateFilterSQL(filterId, out filterWithSQL);
            ////    if (!questStatusDef.IsSuccess(status))
            ////    {
            ////        return (status);
            ////    }

            ////    // Update filter
            ////    FiltersMgr filtersMgr = new FiltersMgr(this.UserSession);
            ////    status = filtersMgr.Update(filterWithSQL);
            ////    if (!questStatusDef.IsSuccess(status))
            ////    {
            ////        return (status);
            ////    }
            ////}

            return (new questStatus(Severity.Success));
        }
        public questStatus List(ParameterListViewModel viewModel, out ParameterListViewModel parameterListViewModel)
        {
            // Initialize
            questStatus status = null;
            parameterListViewModel = null;


            // Get stored procedure parameters
            StoredProcedureId storedProcedureId = new StoredProcedureId(viewModel.StoredProcedureId);
            List<StoredProcedureParameter> storedProcedureParameterList = null;
            StoredProcedureParametersMgr storedProcedureParametersMgr = new StoredProcedureParametersMgr(this.UserSession);
            status = storedProcedureParametersMgr.Read(storedProcedureId, out storedProcedureParameterList);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }

            // Sort by display order
            // NOTE: ORDER IN DATABASE -SHOULD- BE ORDER IN SPROC.


            // Transfer model.
            parameterListViewModel = new ParameterListViewModel(this.UserSession, viewModel);
            parameterListViewModel.StoredProcedureId = viewModel.StoredProcedureId;
            foreach (StoredProcedureParameter storedProcedureParameter in storedProcedureParameterList)
            {
                ParameterLineItemViewModel parameterLineItemViewModel = new ParameterLineItemViewModel();
                BufferMgr.TransferBuffer(storedProcedureParameter, parameterLineItemViewModel);
                parameterLineItemViewModel.bRequired = storedProcedureParameter.bRequired ? "Yes" : "No";
                parameterListViewModel.Items.Add(parameterLineItemViewModel);
            }
            return (new questStatus(Severity.Success));
        }
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