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


            // Validate
            if (filterPanelViewModel.Editor.TablesetId < BaseId.VALID_ID)
            {
                return (new questStatus(Severity.Error, "Invalid Tableset Id"));
            }
            if (filterPanelViewModel.Editor.FilterId < BaseId.VALID_ID)
            {
                return (new questStatus(Severity.Error, "Please select a filter."));
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
            Quest.Functional.MasterPricing.Filter filter = new Functional.MasterPricing.Filter();
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
            FilterId filterId = new FilterId(filterPanelViewModel.Editor.FilterId);
            FilterMgr filterMgr = new FilterMgr(this.UserSession);
            status = filterMgr.Save(filterId, filter);
            if (!questStatusDef.IsSuccess(status))
            {
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

            // Get number of entities.
            int numEntities = 0;
            status = getNumEntities(resultsSet, out numEntities);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }


            // Transfer model
            filterRunViewModel = new FilterRunViewModel();
            filterRunViewModel.Id = runFilterRequest.FilterId.Id;
            filterRunViewModel.FilterId = runFilterRequest.FilterId.Id;
            foreach (KeyValuePair<string, Column> kvp in resultsSet.ResultColumns)
            {
                ColumnHeaderViewModel columnHeaderViewModel = new ColumnHeaderViewModel();
                BufferMgr.TransferBuffer(kvp.Value, columnHeaderViewModel, true);
                columnHeaderViewModel.Name = kvp.Value.Name;
                columnHeaderViewModel.Label = makeColumnLabel(kvp, numEntities);
                columnHeaderViewModel.Type = kvp.Value.DataTypeName;
                filterRunViewModel.Results.Columns.Add(columnHeaderViewModel);
            }
            foreach (dynamic _dynRow in resultsSet.Data)
            {
                DynamicRowViewModel dynamicRowViewModel = new DynamicRowViewModel();
                int cidx = 0;
                foreach (KeyValuePair<string, object> kvp in _dynRow)
                {
                    ColumnValueViewModel columnValueViewModel = new ColumnValueViewModel();
                    columnValueViewModel.Name = filterRunViewModel.Results.Columns[cidx].Name;
                    columnValueViewModel.Label = filterRunViewModel.Results.Columns[cidx].Label;
                    columnValueViewModel.Value = kvp.Value == null ? "(null)" : kvp.Value.ToString();
                    dynamicRowViewModel.ColumnValues.Add(columnValueViewModel);
                    cidx += 1;
                }
                filterRunViewModel.Results.Items.Add(dynamicRowViewModel);
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
        private questStatus getNumEntities(ResultsSet resultsSet, out int numEntities)
        {
            // Initialize
            numEntities = 0;


            List<string> entityNameList = new List<string>();
            foreach (dynamic _dynRow in resultsSet.Data)
            {
                DynamicRowViewModel dynamicRowViewModel = new DynamicRowViewModel();
                foreach (KeyValuePair<string, object> kvp in _dynRow)
                {
                    string[] pp = kvp.Key.Split('_');
                    string entityName = pp[0];
                    string existingEntityName = entityNameList.Find(delegate (string t) { return t == entityName; });
                    if (existingEntityName == null)
                    {
                        entityNameList.Add(entityName);
                    }
                }
            }
            numEntities = entityNameList.Count;

            return (new questStatus(Severity.Success));
        }
        private string makeColumnLabel(KeyValuePair<string, Column> kvp, int numEntities)
        {
            string label = null;
            if (kvp.Value.Label != null)
            {
                return (kvp.Value.Label);
            }
            if (numEntities == 1)
            {
                string[] pp = kvp.Key.Split('_');
                label = pp[1];
            }
            else
            {
                label = kvp.Key;
            }
            return (label);
        }
        #endregion
    }
}