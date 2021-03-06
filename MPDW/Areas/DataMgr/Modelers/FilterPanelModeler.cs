﻿using System;
using System.Collections.Generic;
using System.Data;
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
using Quest.MasterPricing.Services.Business.Tablesets;


namespace Quest.MasterPricing.DataMgr.Modelers
{
    public class FilterPanelModeler : DataMgrBaseModeler
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
        public FilterPanelModeler(HttpRequestBase httpRequestBase, UserSession userSession)
            : base(httpRequestBase, userSession)
        {
            initialize();
        }
        public FilterPanelModeler(HttpRequestBase httpRequestBase, UserSession userSession, DataMgrBaseViewModel dataMgrBaseViewModel)
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
        public questStatus Save(FilterPanelViewModel filterPanelViewModel)
        {
            // Initialize
            questStatus status = null;
            Quest.Functional.MasterPricing.Filter filter = null;
            FilterId filterId = null;


            // Validate
            if (filterPanelViewModel.Editor.TablesetId < BaseId.VALID_ID)
            {
                return (new questStatus(Severity.Error, "Invalid Tableset Id"));
            }
            if (filterPanelViewModel.Editor.FilterId < BaseId.VALID_ID)
            {
                // For headless agents that want to save all-in-one, not via the Filters panel.
                filter = new Functional.MasterPricing.Filter();
                BufferMgr.TransferBuffer(filterPanelViewModel.Editor, filter);

                filterId = null;
                FiltersMgr filtersMgr = new FiltersMgr(this.UserSession);
                status = filtersMgr.Create(filter, out filterId);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }
                filterPanelViewModel.Editor.Id = filterId.Id;
                filterPanelViewModel.Editor.FilterId = filterId.Id;
            }


            // Read tableset configuration
            TablesetId tablesetId = new TablesetId(filterPanelViewModel.Editor.TablesetId);
            TablesetDataManagement tablesetDataManagement = null;
            TablesetMgr tablesetMgr = new TablesetMgr(this.UserSession);
            status = tablesetMgr.ReadTablesetDataManagement(tablesetId, out tablesetDataManagement);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }


            #region Transfer filter entities
            //
            // Transfer filter entities
            //
            filter = new Functional.MasterPricing.Filter();
            filter.TablesetId = filterPanelViewModel.Editor.TablesetId;
            filter.Id = filterPanelViewModel.Editor.Id;
            foreach (BootstrapTreenodeViewModel filterEntity in filterPanelViewModel.Entities)
            {
                if (filterEntity.type == "table")
                {
                    FilterTable filterTable = new FilterTable();
                    filterTable.FilterId = filterPanelViewModel.Editor.Id;
                    filterTable.Schema = filterEntity.Schema;
                    filterTable.Name = filterEntity.Name;
                    filter.FilterTableList.Add(filterTable);
                }
                else if (filterEntity.type == "view")
                {
                    FilterView filterView = new FilterView();
                    filterView.FilterId = filterPanelViewModel.Editor.Id;
                    filterView.Schema = filterEntity.Schema;
                    filterView.Name = filterEntity.Name;
                    filter.FilterViewList.Add(filterView);
                }
                else if (filterEntity.type == "column")
                {
                    FilterColumn filterColumn = new FilterColumn();
                    filterColumn.FilterId = filterPanelViewModel.Editor.Id;
                    filterColumn.Name = filterEntity.Name;
                    filterColumn.TablesetEntityId = filterEntity.ParentId;

                    if (filterEntity.parentType == "table")
                    {
                        TablesetTable parentTablesetTable = tablesetDataManagement.TablesetConfiguration.TablesetTables.Find(delegate (TablesetTable tt) { return tt.Id == filterEntity.ParentId; });
                        if (parentTablesetTable == null)
                        {
                            return (new questStatus(Severity.Error, String.Format("ERROR: column filter entity \"{0}\"  (Id {1})   table parent Id {2} not found.",
                                filterEntity.text, filterEntity.Id, filterEntity.ParentId)));
                        }
                        filterColumn.FilterEntityTypeId = FilterEntityType.Table;

                        // If column table not in filter entities, add it.
                        BootstrapTreenodeViewModel filterColumnTable = filterPanelViewModel.Entities.Find(delegate (BootstrapTreenodeViewModel n) { return n.type == "table" && n.Id == filterEntity.ParentId; });
                        if (filterColumnTable == null)
                        {
                            FilterTable filterTable = filter.FilterTableList.Find(delegate (FilterTable t) { return filterEntity.ParentId == t.TablesetTable.Id; });
                            if (filterTable == null)
                            {
                                filterTable = new FilterTable();
                                filterTable.FilterId = filterPanelViewModel.Editor.Id;
                                filterTable.TablesetTable = parentTablesetTable;
                                filterTable.Schema = parentTablesetTable.Schema;
                                filterTable.Name = parentTablesetTable.Name;
                                filter.FilterTableList.Add(filterTable);
                            }
                        }
                    }
                    else if (filterEntity.parentType == "view")
                    {
                        TablesetView parentTablesetView = tablesetDataManagement.TablesetConfiguration.TablesetViews.Find(delegate (TablesetView tv) { return tv.Id == filterEntity.ParentId; });
                        if (parentTablesetView == null)
                        {
                            return (new questStatus(Severity.Error, String.Format("ERROR: column filter entity \"{0}\"  (Id {1})   view parent Id {2} not found.",
                                filterEntity.text, filterEntity.Id, filterEntity.ParentId)));
                        }
                        filterColumn.FilterEntityTypeId = FilterEntityType.View;

                        // If column view not in filter entities, add it.
                        BootstrapTreenodeViewModel filterColumnView = filterPanelViewModel.Entities.Find(delegate (BootstrapTreenodeViewModel n) { return n.type == "view" && n.Id == filterEntity.ParentId; });
                        if (filterColumnView == null)
                        {
                            FilterView filterView = filter.FilterViewList.Find(delegate (FilterView v) { return filterEntity.ParentId == v.TablesetView.Id; });
                            if (filterView == null)
                            {
                                filterView = new FilterView();
                                filterView.FilterId = filterPanelViewModel.Editor.Id;
                                filterView.TablesetView = parentTablesetView;
                                filterView.Schema = parentTablesetView.Schema;
                                filterView.Name = parentTablesetView.Name;
                                filter.FilterViewList.Add(filterView);
                            }
                        }
                    }
                    else
                    {
                        return (new questStatus(Severity.Error, String.Format("ERROR: column filter entity \"{0}\"  (Id {1})  unknown parent parent type: {2}",
                            filterEntity.text, filterEntity.Id, filterEntity.ParentId, (filterEntity.parentType == null ? "null" : filterEntity.parentType))));
                    }
                    filter.FilterColumnList.Add(filterColumn);
                }
                else
                {
                    return (new questStatus(Severity.Error, String.Format("Invalid filter entity type: {0}", filterEntity.type)));
                }
            }
            #endregion


            #region Transfer filter items
            //
            // Transfer filter items
            //
            foreach (FilterItemViewModel filterItemViewModel in filterPanelViewModel.Items)
            {
                // Filter Item
                FilterItem filterItem = new FilterItem();
                filterItem.FilterId = filterPanelViewModel.Editor.FilterId;
                if (filterItemViewModel.Entity.type == "table")
                {
                    filterItem.FilterEntityTypeId = FilterEntityType.Table;
                }
                if (filterItemViewModel.Entity.type == "view")
                {
                    filterItem.FilterEntityTypeId = FilterEntityType.View;
                }
                else if (filterItemViewModel.Entity.type == "column")
                {
                    filterItem.FilterEntityTypeId = FilterEntityType.Column;
                }
                filterItem.TablesetColumnId = filterItemViewModel.Entity.Id;
                filterItem.Label = filterItemViewModel.Label;
                filterItem.ParameterName = filterItemViewModel.ParameterName;
                filterItem.bHidden = filterItemViewModel.bHidden;
                filterItem.bBulkUpdateValueRequired = filterItemViewModel.bBulkUpdateValueRequired;

                // Joins
                foreach (FilterItemJoinViewModel filterItemJoinViewModel in filterItemViewModel.Joins)
                {
                    FilterItemJoin filterItemJoin = new FilterItemJoin();
                    filterItemJoin.ColumnId = filterItemJoinViewModel.ColumnId;
                    filterItemJoin.JoinType = filterItemJoinViewModel.JoinType;
                    filterItemJoin.Identifier = filterItemJoinViewModel.Identifier;
                    filterItem.JoinList.Add(filterItemJoin);
                }

                // Operations
                foreach (FilterOperationViewModel filterOperationViewModel in filterItemViewModel.Operations)
                {
                    FilterOperation filterOperation = new FilterOperation();
                    filterOperation.FilterOperatorId = filterOperationViewModel.Operator;

                    // Values
                    foreach (FilterValueViewModel filterValueViewModel in filterOperationViewModel.Values)
                    {
                        FilterValue filterValue = new FilterValue();
                        filterValue.Value = filterValueViewModel.Value;
                        filterOperation.ValueList.Add(filterValue);
                    }
                    filterItem.OperationList.Add(filterOperation);
                }

                // Lookup
                if (filterItemViewModel.Lookup.Id >= BaseId.VALID_ID)
                {
                    filterItem.LookupId = filterItemViewModel.Lookup.Id;
                }

                // TypeList
                if (filterItemViewModel.TypeList.Id >= BaseId.VALID_ID)
                {
                    filterItem.TypeListId = filterItemViewModel.TypeList.Id;
                }
                filter.FilterItemList.Add(filterItem);
            }
            #endregion


            #region  Transfer filter procedures
            //
            // Transfer filter procedures
            //
            foreach (FilterProcedureViewModel filterProcedureViewModel in filterPanelViewModel.Procedures)
            {
                if (filterProcedureViewModel.Id > BaseId.INVALID_ID)
                {
                    FilterProcedure filterProcedure = new FilterProcedure();
                    BufferMgr.TransferBuffer(filterProcedureViewModel, filterProcedure);
                    filterProcedure.FilterId = filterPanelViewModel.Editor.FilterId;
                    filter.FilterProcedureList.Add(filterProcedure);
                }
            }
            #endregion


            #region Save filter
            //
            // Save filter
            //
            filterId = new FilterId(filterPanelViewModel.Editor.FilterId);
            FilterMgr filterMgr = new FilterMgr(this.UserSession);
            status = filterMgr.Save(filterId, filter);
            if (!questStatusDef.IsSuccess(status))
            {
                if (questStatusDef.IsWarning(status))
                {
                    return (status);
                }
                return (new questStatus(status.Severity, String.Format("Error saving filter items: {0}", status.Message)));
            }
            #endregion


            #region Return procedure parameters
            //
            // Return procedure parameters
            //

            // Get filter database
            Quest.Functional.MasterPricing.Database database = null;
            filterMgr.GetFilterDatabase(filterId, out database);
            if (filter.FilterProcedureList.Count > 0)
            {
                foreach (FilterProcedureViewModel filterProcedureViewModel in filterPanelViewModel.Procedures)
                {
                    if ((filterProcedureViewModel.Id == BaseId.INVALID_ID) || (string.IsNullOrEmpty(filterProcedureViewModel.Name)))
                    {
                        continue;
                    }

                    // Get procedure parameters
                    List<Quest.Functional.MasterPricing.FilterProcedureParameter> filterProcedureParameterList = null;
                    status = filterMgr.GetStoredProdecureParameters(database, filterProcedureViewModel.Name, out filterProcedureParameterList);
                    if (!questStatusDef.IsSuccess(status))
                    {
                        return (status);
                    }
                    foreach (FilterProcedureParameter filterProcedureParameter in filterProcedureParameterList)
                    {
                        FilterProcedureParameterViewModel filterProcedureParameterViewModel = new FilterProcedureParameterViewModel();
                        BufferMgr.TransferBuffer(filterProcedureParameter, filterProcedureParameterViewModel, true);
                        filterProcedureParameterViewModel.Precision = filterProcedureParameter.Precision[0];
                        filterProcedureParameterViewModel.Scale = filterProcedureParameter.Scale[0];
                        filterProcedureViewModel.Parameters.Add(filterProcedureParameterViewModel);
                    }
                }
            }
            #endregion


            // Return warning if no items in filter.
            if (filter.FilterItemList.Count == 0)
            {
                return (new questStatus(Severity.Warning, "Filter successfully saved, but has no items"));
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Run(RunFilterRequest runFilterRequest, out FilterRunViewModel filterRunViewModel)
        {
            // Initialize
            questStatus status = null;
            filterRunViewModel = null;


            // Execute filter
            ResultsSet resultsSet = null;
            FilterMgr filterMgr = new FilterMgr(this.UserSession);
            status = filterMgr.ExecuteFilter(runFilterRequest, out resultsSet);
            if (!questStatusDef.IsSuccess(status))
            {
                return (new questStatus(status.Severity, String.Format("Error executing filter Id={0}: {1}",
                    runFilterRequest.FilterId.Id, status.Message)));
            }

            // Transfer results to view model.
            status = TransferResults(runFilterRequest, resultsSet, out filterRunViewModel);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Copy(FilterCopyViewModel viewModel, out FilterCopyViewModel filterCopyViewModel)
        {
            // Initialize
            questStatus status = null;
            filterCopyViewModel = null;


            // Copy the filter.
            FilterId filterId = new FilterId(viewModel.FilterId);
            FilterId newFilterId = null;
            FilterMgr filterMgr = new FilterMgr(this.UserSession);
            status = filterMgr.Copy(filterId, out newFilterId);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }

            // Return new filter Id
            filterCopyViewModel = new FilterCopyViewModel();
            filterCopyViewModel.FilterId = viewModel.FilterId;
            filterCopyViewModel.NewFilterId = newFilterId.Id;

            return (new questStatus(Severity.Success));
        }
        public questStatus Export(FilterResultsExportViewModel filterResultsExportViewModel, out ResultsSet resultsSet)
        {
            // Initialize
            questStatus status = null;
            resultsSet = null;
            if (filterResultsExportViewModel._ResultsOptions == null)
            {
                filterResultsExportViewModel._ResultsOptions = new ResultsOptionsViewModel();
                filterResultsExportViewModel._ResultsOptions.RowLimit = "";
                filterResultsExportViewModel._ResultsOptions.ColLimit = "";
            }

            // Fill out a run rqeuest
            RunFilterRequest runFilterRequest = new RunFilterRequest();
            runFilterRequest.FilterId.Id = filterResultsExportViewModel.Id;
            if ((filterResultsExportViewModel.RowLimit != null) && (filterResultsExportViewModel.RowLimit.Trim().Length > 0))
            {
                runFilterRequest.RowLimit = filterResultsExportViewModel.RowLimit.Trim();
            }
            else if ((filterResultsExportViewModel._ResultsOptions.RowLimit != null)  && (filterResultsExportViewModel._ResultsOptions.RowLimit.Trim().Length > 0))
            {
                runFilterRequest.RowLimit = filterResultsExportViewModel._ResultsOptions.RowLimit.Trim();
            }
            if ((filterResultsExportViewModel.ColLimit != null)  && (filterResultsExportViewModel.ColLimit.Trim().Length > 0))
            {
                runFilterRequest.ColLimit = filterResultsExportViewModel.ColLimit.Trim();
            }
            else if ((filterResultsExportViewModel._ResultsOptions.ColLimit != null)  && (filterResultsExportViewModel._ResultsOptions.ColLimit.Trim().Length > 0))
            {
                runFilterRequest.ColLimit = filterResultsExportViewModel._ResultsOptions.ColLimit.Trim();
            }


            // Execute filter
            resultsSet = null;
            FilterMgr filterMgr = new FilterMgr(this.UserSession);
            status = filterMgr.ExecuteFilter(runFilterRequest, out resultsSet);
            if (!questStatusDef.IsSuccess(status))
            {
                return (new questStatus(status.Severity, String.Format("Error executing filter Id={0}: {1}",
                    runFilterRequest.FilterId.Id, status.Message)));
            }
            return (new questStatus(Severity.Success));
        }

        public questStatus Run(FilterRunViewModel filterRunViewModel, out FilterRunViewModel filterRunResultsViewModel, out ResultsSet resultsSet)
        {
            // Initialize
            questStatus status = null;
            filterRunResultsViewModel = null;
            resultsSet = null;


            // NOTE: THIS IS FOR RUNNING A FILTER THAT IS --NOT-- IN THE DATABASE.  HOWEVER IT IS *BASED* ON A FILTER IN THE DATABASE, THUS A VALID FILTER ID MUST BE GIVEN.
            FilterMgr filterMgr = new FilterMgr(this.UserSession);


            // Get the filter
            FilterId filterId = new FilterId(filterRunViewModel.FilterId);
            Quest.Functional.MasterPricing.Filter filterFROMDatabase = null;
            status = filterMgr.GetFilter(filterId, out filterFROMDatabase);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }

            // Merge view model into filter from database.
            Quest.Functional.MasterPricing.Filter filter = null;
            status = MergeFilterEditorViewModel(filterRunViewModel, filterFROMDatabase, out filter);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }

            // Verify the filter
            status = filterMgr.Verify(filter);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }


            // Generate filter SQL
            Quest.Functional.MasterPricing.Filter filterWithSQL = null;
            status = filterMgr.GenerateFilterSQL(filter, out filterWithSQL);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }

            // Run the filter
            RunFilterRequest runFilterRequest = new RunFilterRequest();  // Filter Id doesn't matter, non-DB filter run request.
            runFilterRequest.RowLimit = filterRunViewModel._ResultsOptions.RowLimit;
            runFilterRequest.ColLimit = filterRunViewModel._ResultsOptions.ColLimit;
            status = filterMgr.Run(runFilterRequest, filterWithSQL, out resultsSet);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }

            // Transfer results to view model.
            status = TransferResults(resultsSet, out filterRunResultsViewModel);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            if (string.IsNullOrEmpty(filterRunViewModel.Name))
            {
                filterRunResultsViewModel.Name = filterFROMDatabase.Name;
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
        #endregion
    }
}