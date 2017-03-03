using System;
using System.Collections.Generic;
using System.Web;
using Quest.Util.Status;
using Quest.Util.Buffer;
using Quest.Util.Data;
using Quest.Functional.ASM;
using Quest.Functional.FMS;
using Quest.Functional.MasterPricing;
using Quest.MPDW.Models;
using Quest.MPDW.Modelers;
using Quest.MasterPricing.DataMgr.Models;
using Quest.MasterPricing.Services.Business.Filters;


namespace Quest.MasterPricing.DataMgr.Modelers
{
    public class FilterEditorModeler : DataMgrBaseModeler
    {
        #region Declarations
        /*==================================================================================================================================
         * Declarations
         *=================================================================================================================================*/
        private DataMgrBaseViewModel _dataMgrBaseViewModel = null;

        #endregion


        #region Constructors
        /*==================================================================================================================================
        * ConstructorsRequisitionListModeler
        *=================================================================================================================================*/
        public FilterEditorModeler(HttpRequestBase httpRequestBase, UserSession userSession)
            : base(httpRequestBase, userSession)
        {
            initialize();
        }
        public FilterEditorModeler(HttpRequestBase httpRequestBase, UserSession userSession, DataMgrBaseViewModel dataMgrBaseViewModel)
            : base(httpRequestBase, userSession)
        {
            this._dataMgrBaseViewModel = dataMgrBaseViewModel;
            initialize();
        }
        #endregion


        #region Public Methods
        /*==================================================================================================================================
         * Public Methods
         *=================================================================================================================================*/
        public questStatus Save(FilterEditorViewModel filterEditorViewModel)
        {
            // Initialize
            questStatus status = null;

            // Validate
            if (filterEditorViewModel.TablesetId < BaseId.VALID_ID)
            {
                return (new questStatus(Severity.Error, "Invalid Tableset Id"));
            }

            // Transfer model
            Quest.Functional.MasterPricing.Filter filter = new Functional.MasterPricing.Filter();
            BufferMgr.TransferBuffer(filterEditorViewModel, filter);


            // Determine if this is a create or update
            FiltersMgr filtersMgr = new FiltersMgr(this.UserSession);
            if (filterEditorViewModel.Id < BaseId.VALID_ID)
            {
                // Create
                FilterId filterId = null;
                status = filtersMgr.Create(filter, out filterId);
                if (!questStatusDef.IsSuccess(status))
                {
                    FormatErrorMessage(status, filterEditorViewModel);
                    return (status);
                }
                filterEditorViewModel.Id = filterId.Id;
            }
            else
            {
                // Update
                status = filtersMgr.Update(filter);
                if (!questStatusDef.IsSuccess(status))
                {
                    FormatErrorMessage(status, filterEditorViewModel);
                    return (status);
                }
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Read(FilterId filterId, out FilterEditorViewModel filterEditorViewModel)
        {
            // Initialize
            questStatus status = null;
            filterEditorViewModel = null;


            // Read filter
            Quest.Functional.MasterPricing.Filter filter = null;
            FilterMgr filterMgr = new FilterMgr(this.UserSession);
            status = filterMgr.GetFilter(filterId, out filter);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }

            // Transfer model
            filterEditorViewModel = new FilterEditorViewModel();
            BufferMgr.TransferBuffer(filter, filterEditorViewModel);
            foreach (FilterTable filterTable in filter.FilterTableList)
            {
                BootstrapTreenodeViewModel bootstrapTreenodeViewModel = null;
                status = FormatBootstrapTreeviewNode(filterTable, out bootstrapTreenodeViewModel);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }
                filterEditorViewModel.Entities.Add(bootstrapTreenodeViewModel);
            }
            foreach (FilterView filterView in filter.FilterViewList)
            {
                BootstrapTreenodeViewModel bootstrapTreenodeViewModel = null;
                status = FormatBootstrapTreeviewNode(filterView, out bootstrapTreenodeViewModel);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }
                filterEditorViewModel.Entities.Add(bootstrapTreenodeViewModel);
            }
            foreach (FilterItem filterItem in filter.FilterItemList)
            {
                FilterItemViewModel filterItemViewModel = new FilterItemViewModel();
                filterItemViewModel.Id = filterItem.Id;
                ////filterItemViewModel.Name = filterItem.TablesetColumn.Name;
                filterItemViewModel.Name = filterItem.FilterColumn.TablesetColumn.Column.Name;
                filterItemViewModel.Label = filterItem.Label;
                filterItemViewModel.ParameterName = filterItem.ParameterName;

                filterItemViewModel.Entity.type = "column";  // TODO: ALL THAT'S SUPPORTED RIGHT NOW.
                filterItemViewModel.Entity.Schema = null;
                filterItemViewModel.Entity.Name = filterItem.FilterColumn.TablesetColumn.Column.Name;

                filterItemViewModel.ParentEntity.type = filterItem.FilterColumn.ParentEntityType.type;
                filterItemViewModel.ParentEntity.Schema = filterItem.FilterColumn.ParentEntityType.Schema;
                filterItemViewModel.ParentEntity.Name = filterItem.FilterColumn.ParentEntityType.Name;



                // Joins
                foreach (FilterItemJoin filterItemJoin in filterItem.JoinList)
                {
                    FilterItemJoinViewModel filterItemJoinViewModel = new FilterItemJoinViewModel();
                    BufferMgr.TransferBuffer(filterItemJoin, filterItemJoinViewModel);
                    filterItemJoinViewModel.Identifier = '[' + filterItemJoin.TargetSchema + "].[" + filterItemJoin.TargetEntityName + "].[" + filterItemJoin.TargetColumnName + "]";
                    filterItemViewModel.Joins.Add(filterItemJoinViewModel);
                }


                // Operations
                foreach (FilterOperation filterOperation in filterItem.OperationList)
                {
                    FilterOperationViewModel filterOperationViewModel = new FilterOperationViewModel();
                    filterOperationViewModel.Id = filterOperation.Id;
                    filterOperationViewModel.Operator = filterOperation.FilterOperatorId;

                    // Views
                    foreach (FilterValue filterValue in filterOperation.ValueList)
                    {
                        FilterValueViewModel filterValueViewModel = new FilterValueViewModel();
                        filterValueViewModel.Id = filterValue.Id;
                        filterValueViewModel.Value = filterValue.Value;
                        filterOperationViewModel.Values.Add(filterValueViewModel);
                    }
                    filterItemViewModel.Operations.Add(filterOperationViewModel);
                }

                // Lookup
                if (filterItem.LookupId.HasValue)
                {
                    FilterItemLookupViewModel filterItemLookupViewModel = new FilterItemLookupViewModel();
                    filterItemLookupViewModel.Id = filterItem.Lookup.Id;
                    filterItemLookupViewModel.Name = filterItem.Lookup.Name;
                    filterItemViewModel.Lookup = filterItemLookupViewModel;
                }

                // TypeList
                if (filterItem.TypeListId.HasValue)
                {
                    FilterItemTypeListViewModel filterItemTypeListViewModel = new FilterItemTypeListViewModel();
                    filterItemTypeListViewModel.Id = filterItem.TypeList.Id;
                    filterItemTypeListViewModel.Name = filterItem.TypeList.Name;
                    filterItemViewModel.TypeList = filterItemTypeListViewModel;
                }

                filterEditorViewModel.Items.Add(filterItemViewModel);
            }

            foreach (FilterProcedure filterProcedure in filter.FilterProcedureList)
            {
                FilterProcedureViewModel filterProcedureViewModel = new FilterProcedureViewModel();
                BufferMgr.TransferBuffer(filterProcedure, filterProcedureViewModel);
                foreach (FilterProcedureParameter filterProcedureParameter in filterProcedure.ParameterList)
                {
                    FilterProcedureParameterViewModel filterProcedureParameterViewModel = new FilterProcedureParameterViewModel();
                    BufferMgr.TransferBuffer(filterProcedureParameter, filterProcedureParameterViewModel, true);
                    filterProcedureParameterViewModel.Precision = filterProcedureParameter.Precision[0];
                    filterProcedureParameterViewModel.Scale = filterProcedureParameter.Scale[0];
                    filterProcedureViewModel.Parameters.Add(filterProcedureParameterViewModel);
                }
                filterEditorViewModel.Procedures.Add(filterProcedureViewModel);
            }

            return (new questStatus(Severity.Success));
        }
        public questStatus Read(FilterId filterId, out FilterSQLViewModel filterSQLViewModel)
        {
            // Initialize
            questStatus status = null;
            filterSQLViewModel = null;

            // Read filter
            Quest.Functional.MasterPricing.Filter filter = null;
            FilterMgr filterMgr = new FilterMgr(this.UserSession);
            status = filterMgr.GetFilter(filterId, out filter);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }

            // Transfer model
            filterSQLViewModel = new FilterSQLViewModel();
            BufferMgr.TransferBuffer(filter, filterSQLViewModel);


            if (string.IsNullOrEmpty(filterSQLViewModel.SQL))
            {
                return (new questStatus(Severity.Warning, "Filter has no SQL"));
            }

            return (new questStatus(Severity.Success));
        }
        public questStatus Delete(FilterId filterId)
        {
            // Initialize
            questStatus status = null;


            // Delete
            FiltersMgr filtersMgr = new FiltersMgr(this.UserSession);
            status = filtersMgr.Delete(filterId);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        #endregion


        #region Private Methods
        /*==================================================================================================================================
         * Private Methods
         *=================================================================================================================================*/
        private questStatus initialize()
        {
            return (new questStatus(Severity.Success));
        }
        private questStatus getTablesetColumn(Quest.Functional.MasterPricing.Filter filter)
        {
            return (new questStatus(Severity.Success));
        }
        ////private questStatus getJoinTableTarget(Quest.Functional.MasterPricing.Filter filter, FilterColumn joinFilterColumn, out TablesetTable tablesetTable, out TablesetColumn tablesetColumn)
        ////{
        ////    // Initialize
        ////    tablesetTable = null;
        ////    tablesetColumn = null;


        ////    foreach (FilterTable filterTable in filter.FilterTableList)
        ////    {
        ////        foreach (TablesetColumn _tablesetColumn in filterTable.TablesetTable.TablesetColumnList)
        ////        {
        ////            if (joinFilterColumn.TablesetColumnId == _tablesetColumn.Id)
        ////            {
        ////                tablesetTable = filterTable.TablesetTable;
        ////                tablesetColumn = _tablesetColumn;
        ////                return (new questStatus(Severity.Success));
        ////            }
        ////        }
        ////    }
        ////    return (new questStatus(Severity.Error, "JOIN FilterColumn table target not found"));
        ////}
        ////private questStatus getJoinViewTarget(Quest.Functional.MasterPricing.Filter filter, FilterColumn joinFilterColumn, out TablesetView tablesetView, out TablesetColumn tablesetColumn)
        ////{
        ////    // Initialize
        ////    tablesetView = null;
        ////    tablesetColumn = null;


        ////    foreach (FilterView filterView in filter.FilterViewList)
        ////    {
        ////        foreach (TablesetColumn _tablesetColumn in filterView.TablesetView.TablesetColumnList)
        ////        {
        ////            if (joinFilterColumn.TablesetColumnId == _tablesetColumn.Id)
        ////            {
        ////                tablesetView = filterView.TablesetView;
        ////                tablesetColumn = _tablesetColumn;
        ////                return (new questStatus(Severity.Success));
        ////            }
        ////        }
        ////    }
        ////    return (new questStatus(Severity.Error, "JOIN FilterColumn view target not found"));
        ////}
        #endregion
    }
}