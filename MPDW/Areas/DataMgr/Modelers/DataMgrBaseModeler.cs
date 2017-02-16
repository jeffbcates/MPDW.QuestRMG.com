using System;
using System.Text;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Quest.Util.Status;
using Quest.Util.Buffer;
using Quest.Util.Data;
using Quest.Functional.ASM;
using Quest.Functional.FMS;
using Quest.Functional.MasterPricing;
using Quest.MPDW.Models;
using Quest.MPDW.Modelers;
using Quest.MasterPricing.DataMgr.Models;
using Quest.MasterPricing.DataMgr.Modelers;
using Quest.MPDW.Services.Data;
using Quest.MasterPricing.Services.Business.Tablesets;
using Quest.MasterPricing.Services.Business.Filters;


namespace Quest.MasterPricing.DataMgr.Modelers
{
    public class DataMgrBaseModeler : BaseModeler
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
        public DataMgrBaseModeler(HttpRequestBase httpRequestBase, UserSession userSession)
            : base(httpRequestBase, userSession)
        {
            initialize();
        }
        #endregion


        #region Public Methods
        /*==================================================================================================================================
        * Public Methods
        *=================================================================================================================================*/


        #region Options
        //----------------------------------------------------------------------------------------------------------------------------------
        // Options
        //----------------------------------------------------------------------------------------------------------------------------------
        public questStatus GetFilterOptions(TablesetId tablesetId, out List<OptionValuePair> optionsList, string Value = null, string Name = null)
        {
            // Initialize
            questStatus status = null;
            optionsList = null;


            // Set up query options.
            // TEMPORARY: OPTIMIZE THIS
            SearchField searchField = new SearchField();
            searchField.Name = "TablesetId";
            searchField.SearchOperation = SearchOperation.Equal;
            searchField.Type = typeof(int);
            searchField.Value = tablesetId.Id.ToString();
            List<SearchField> searchFieldList = new List<SearchField>();
            searchFieldList.Add(searchField);

            SearchOptions searchOptions = new SearchOptions();
            searchOptions.SearchFieldList = searchFieldList;

            QueryOptions queryOptions = new QueryOptions(1000, 1);
            queryOptions.SearchOptions = searchOptions;
            QueryResponse queryResponse = null;


            // Get filters for given tableset
            List<Quest.Functional.MasterPricing.Filter> filterList = null;
            FiltersMgr filtersMgr = new FiltersMgr(this.UserSession);
            status = filtersMgr.List(queryOptions, out filterList, out queryResponse);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }

            // Sort 
            filterList.Sort(delegate (Quest.Functional.MasterPricing.Filter i1, Quest.Functional.MasterPricing.Filter i2) { return i1.Name.CompareTo(i2.Name); });


            // Build options
            // Set selected if specified.
            optionsList = new List<OptionValuePair>();
            foreach (Quest.Functional.MasterPricing.Filter filter in filterList)
            {
                OptionValuePair optionValuePair = new OptionValuePair();
                optionValuePair.Id = filter.Id.ToString();
                optionValuePair.Label = filter.Name;
                if (Value != null && Value == filter.Id.ToString())
                {
                    optionValuePair.bSelected = true;
                }
                else if (Name != null && Name == filter.Name)
                {
                    optionValuePair.bSelected = true;
                }
                optionsList.Add(optionValuePair);
            }

            // Insert default option
            status = AddDefaultOptions(optionsList, "-1", "Select one ...");
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        #endregion


        #region Formatting
        //----------------------------------------------------------------------------------------------------------------------------------
        // Formatting
        //----------------------------------------------------------------------------------------------------------------------------------
        public questStatus FormatBootstrapTreeviewNode(FilterTable filterTable, out BootstrapTreenodeViewModel bootstrapTreenodeViewModel)
        {
            // Initialize
            questStatus status = null;

            bootstrapTreenodeViewModel = new BootstrapTreenodeViewModel();
            bootstrapTreenodeViewModel.Id = filterTable.TablesetTable.Id;
            bootstrapTreenodeViewModel.type = "table";
            bootstrapTreenodeViewModel.icon = "fa fa-table padding-right-20";
            bootstrapTreenodeViewModel.text = "[" + filterTable.TablesetTable.Schema + "].[" + filterTable.TablesetTable.Name + "]";
            bootstrapTreenodeViewModel.Schema = filterTable.TablesetTable.Schema;
            bootstrapTreenodeViewModel.Name = filterTable.TablesetTable.Name;
            bootstrapTreenodeViewModel.selectable = "true";

            List<BootstrapTreenodeViewModel> columnNodeList = new List<BootstrapTreenodeViewModel>();
            foreach (FilterColumn filterColumn in filterTable.FilterColumnList)
            {
                BootstrapTreenodeViewModel columnNode = null;
                status = FormatBootstrapTreeviewNode(filterTable, filterColumn, out columnNode);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }
                columnNodeList.Add(columnNode);
            }
            bootstrapTreenodeViewModel.nodes.AddRange(columnNodeList);
            return (new questStatus(Severity.Success));
        }
        public questStatus FormatBootstrapTreeviewNode(TablesetTable tablesetTable, out BootstrapTreenodeViewModel bootstrapTreenodeViewModel)
        {
            // Initialize
            questStatus status = null;

            bootstrapTreenodeViewModel = new BootstrapTreenodeViewModel();
            bootstrapTreenodeViewModel.Id = tablesetTable.Id;
            bootstrapTreenodeViewModel.type = "table";
            bootstrapTreenodeViewModel.icon = "fa fa-table padding-right-20";
            bootstrapTreenodeViewModel.text = "[" + tablesetTable.Schema + "].[" + tablesetTable.Name + "]";
            bootstrapTreenodeViewModel.Schema = tablesetTable.Schema;
            bootstrapTreenodeViewModel.Name = tablesetTable.Name;
            bootstrapTreenodeViewModel.selectable = "true";

            List<BootstrapTreenodeViewModel> columnNodeList = new List<BootstrapTreenodeViewModel>();
            foreach (TablesetColumn tablesetColumn in tablesetTable.TablesetColumnList)
            {
                BootstrapTreenodeViewModel columnNode = null;
                status = FormatBootstrapTreeviewNode(tablesetTable, tablesetColumn, out columnNode);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }
                columnNodeList.Add(columnNode);
            }
            bootstrapTreenodeViewModel.nodes.AddRange(columnNodeList);
            return (new questStatus(Severity.Success));
        }
        public questStatus FormatBootstrapTreeviewNode(Table table, out BootstrapTreenodeViewModel bootstrapTreenodeViewModel)
        {
            // Initialize
            questStatus status = null;

            bootstrapTreenodeViewModel = new BootstrapTreenodeViewModel();
            bootstrapTreenodeViewModel.Id = table.Id;
            bootstrapTreenodeViewModel.type = "table";
            bootstrapTreenodeViewModel.icon = "fa fa-table padding-right-20";
            bootstrapTreenodeViewModel.text = "[" + table.Schema + "].[" + table.Name + "]";
            bootstrapTreenodeViewModel.Schema = table.Schema;
            bootstrapTreenodeViewModel.Name = table.Name;
            bootstrapTreenodeViewModel.selectable = "true";

            List<BootstrapTreenodeViewModel> columnNodeList = new List<BootstrapTreenodeViewModel>();
            foreach (Column column in table.ColumnList)
            {
                BootstrapTreenodeViewModel columnNode = null;
                status = FormatBootstrapTreeviewNode(table, column, out columnNode);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }
                columnNodeList.Add(columnNode);
            }
            bootstrapTreenodeViewModel.nodes.AddRange(columnNodeList);
            return (new questStatus(Severity.Success));
        }
        public questStatus FormatBootstrapTreeviewNode(FilterView filterView, out BootstrapTreenodeViewModel bootstrapTreenodeViewModel)
        {
            // Initialize
            questStatus status = null;

            bootstrapTreenodeViewModel = new BootstrapTreenodeViewModel();
            bootstrapTreenodeViewModel.Id = filterView.TablesetView.Id;
            bootstrapTreenodeViewModel.type = "view";
            bootstrapTreenodeViewModel.icon = "fa fa-th padding-right-20";
            bootstrapTreenodeViewModel.text = "[" + filterView.TablesetView.Schema + "].[" + filterView.TablesetView.Name + "]";
            bootstrapTreenodeViewModel.Schema = filterView.TablesetView.Schema;
            bootstrapTreenodeViewModel.Name = filterView.TablesetView.Name;
            bootstrapTreenodeViewModel.selectable = "true";

            List<BootstrapTreenodeViewModel> columnNodeList = new List<BootstrapTreenodeViewModel>();
            foreach (FilterColumn filterColumn in filterView.FilterColumnList)
            {
                BootstrapTreenodeViewModel columnNode = null;
                status = FormatBootstrapTreeviewNode(filterView, filterColumn, out columnNode);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }
                columnNodeList.Add(columnNode);
            }
            bootstrapTreenodeViewModel.nodes.AddRange(columnNodeList);
            return (new questStatus(Severity.Success));
        }
        public questStatus FormatBootstrapTreeviewNode(TablesetView tablesetView, out BootstrapTreenodeViewModel bootstrapTreenodeViewModel)
        {
            // Initialize
            questStatus status = null;

            bootstrapTreenodeViewModel = new BootstrapTreenodeViewModel();
            bootstrapTreenodeViewModel.Id = tablesetView.Id;
            bootstrapTreenodeViewModel.type = "view";
            bootstrapTreenodeViewModel.icon = "fa fa-th padding-right-20";
            bootstrapTreenodeViewModel.text = "[" + tablesetView.Schema + "].[" + tablesetView.Name + "]";
            bootstrapTreenodeViewModel.Schema = tablesetView.Schema;
            bootstrapTreenodeViewModel.Name = tablesetView.Name;
            bootstrapTreenodeViewModel.selectable = "true";

            List<BootstrapTreenodeViewModel> columnNodeList = new List<BootstrapTreenodeViewModel>();
            foreach (TablesetColumn tablesetColumn in tablesetView.TablesetColumnList)
            {
                BootstrapTreenodeViewModel columnNode = null;
                status = FormatBootstrapTreeviewNode(tablesetView, tablesetColumn, out columnNode);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }
                columnNodeList.Add(columnNode);
            }
            bootstrapTreenodeViewModel.nodes.AddRange(columnNodeList);
            return (new questStatus(Severity.Success));
        }
        public questStatus FormatBootstrapTreeviewNode(View view, out BootstrapTreenodeViewModel bootstrapTreenodeViewModel)
        {
            // Initialize
            questStatus status = null;

            bootstrapTreenodeViewModel = new BootstrapTreenodeViewModel();
            bootstrapTreenodeViewModel.Id = view.Id;
            bootstrapTreenodeViewModel.type = "view";
            bootstrapTreenodeViewModel.icon = "fa fa-th padding-right-20";
            bootstrapTreenodeViewModel.text = "[" + view.Schema + "].[" + view.Name + "]";
            bootstrapTreenodeViewModel.Schema = view.Schema;
            bootstrapTreenodeViewModel.Name = view.Name;
            bootstrapTreenodeViewModel.selectable = "true";

            List<BootstrapTreenodeViewModel> columnNodeList = new List<BootstrapTreenodeViewModel>();
            foreach (Column column in view.ColumnList)
            {
                BootstrapTreenodeViewModel columnNode = null;
                status = FormatBootstrapTreeviewNode(view, column, out columnNode);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }
                columnNodeList.Add(columnNode);
            }
            bootstrapTreenodeViewModel.nodes.AddRange(columnNodeList);
            return (new questStatus(Severity.Success));
        }
        public questStatus FormatBootstrapTreeviewNode(TablesetTable tablesetTable, TablesetColumn tablesetColumn, out BootstrapTreenodeViewModel bootstrapTreenodeViewModel)
        {
            // Initialize
            bootstrapTreenodeViewModel = new BootstrapTreenodeViewModel();
            bootstrapTreenodeViewModel.Id = tablesetColumn.Id;
            bootstrapTreenodeViewModel.ParentId = tablesetColumn.TableSetEntityId;
            bootstrapTreenodeViewModel.type = "column";
            bootstrapTreenodeViewModel.parentType = "table";
            bootstrapTreenodeViewModel.icon = "fa fa-square-o padding-right-20 ";
            bootstrapTreenodeViewModel.text = tablesetColumn.Name + " : " + GetColumnDataType(tablesetColumn.Column);
            bootstrapTreenodeViewModel.Schema = null;
            bootstrapTreenodeViewModel.Name = tablesetColumn.Name;
            bootstrapTreenodeViewModel.selectable = "true";

            return (new questStatus(Severity.Success));
        }
        public questStatus FormatBootstrapTreeviewNode(TablesetView tablesetView, TablesetColumn tablesetColumn, out BootstrapTreenodeViewModel bootstrapTreenodeViewModel)
        {
            // Initialize
            bootstrapTreenodeViewModel = new BootstrapTreenodeViewModel();
            bootstrapTreenodeViewModel.Id = tablesetColumn.Id;
            bootstrapTreenodeViewModel.ParentId = tablesetColumn.TableSetEntityId;
            bootstrapTreenodeViewModel.type = "column";
            bootstrapTreenodeViewModel.parentType = "view";
            bootstrapTreenodeViewModel.icon = "fa fa-square-o padding-right-20 ";
            bootstrapTreenodeViewModel.text = tablesetColumn.Name + " : " + GetColumnDataType(tablesetColumn.Column);
            bootstrapTreenodeViewModel.Schema = null;
            bootstrapTreenodeViewModel.Name = tablesetColumn.Name;
            bootstrapTreenodeViewModel.selectable = "true";

            return (new questStatus(Severity.Success));
        }
        public questStatus FormatBootstrapTreeviewNode(FilterTable filterTable, FilterColumn filterColumn, out BootstrapTreenodeViewModel bootstrapTreenodeViewModel)
        {
            // Initialize
            bootstrapTreenodeViewModel = new BootstrapTreenodeViewModel();
            bootstrapTreenodeViewModel.Id = filterColumn.TablesetColumn.Id;
            bootstrapTreenodeViewModel.ParentId = filterColumn.TablesetColumn.TableSetEntityId;
            bootstrapTreenodeViewModel.type = "column";
            bootstrapTreenodeViewModel.parentType = "table";
            bootstrapTreenodeViewModel.icon = "fa fa-square-o padding-right-20 ";
            bootstrapTreenodeViewModel.text = filterColumn.TablesetColumn.Name + " : " + GetColumnDataType(filterColumn.TablesetColumn.Column);
            bootstrapTreenodeViewModel.Schema = null;
            bootstrapTreenodeViewModel.Name = filterColumn.TablesetColumn.Name;
            bootstrapTreenodeViewModel.selectable = "true";

            return (new questStatus(Severity.Success));
        }
        public questStatus FormatBootstrapTreeviewNode(FilterView filterView, FilterColumn filterColumn, out BootstrapTreenodeViewModel bootstrapTreenodeViewModel)
        {
            // Initialize
            bootstrapTreenodeViewModel = new BootstrapTreenodeViewModel();
            bootstrapTreenodeViewModel.Id = filterColumn.TablesetColumn.Id;
            bootstrapTreenodeViewModel.ParentId = filterColumn.TablesetColumn.TableSetEntityId;
            bootstrapTreenodeViewModel.type = "column";
            bootstrapTreenodeViewModel.parentType = "view";
            bootstrapTreenodeViewModel.icon = "fa fa-square-o padding-right-20 ";
            bootstrapTreenodeViewModel.text = filterColumn.TablesetColumn.Name + " : " + GetColumnDataType(filterColumn.TablesetColumn.Column);
            bootstrapTreenodeViewModel.Schema = null;
            bootstrapTreenodeViewModel.Name = filterColumn.TablesetColumn.Name;
            bootstrapTreenodeViewModel.selectable = "true";

            return (new questStatus(Severity.Success));
        }
        public questStatus FormatBootstrapTreeviewNode(Table table, Column column, out BootstrapTreenodeViewModel bootstrapTreenodeViewModel)
        {
            // Initialize
            bootstrapTreenodeViewModel = new BootstrapTreenodeViewModel();
            bootstrapTreenodeViewModel.Id = column.Id;
            bootstrapTreenodeViewModel.ParentId = column.EntityId;
            bootstrapTreenodeViewModel.type = "column";
            bootstrapTreenodeViewModel.parentType = "table";
            bootstrapTreenodeViewModel.icon = "fa fa-square-o padding-right-20 ";
            bootstrapTreenodeViewModel.text = column.Name + " : " + GetColumnDataType(column);
            bootstrapTreenodeViewModel.Schema = null;
            bootstrapTreenodeViewModel.Name = column.Name;
            bootstrapTreenodeViewModel.selectable = "true";

            return (new questStatus(Severity.Success));
        }
        public questStatus FormatBootstrapTreeviewNode(View view, Column column, out BootstrapTreenodeViewModel bootstrapTreenodeViewModel)
        {
            // Initialize
            bootstrapTreenodeViewModel = new BootstrapTreenodeViewModel();
            bootstrapTreenodeViewModel.Id = column.Id;
            bootstrapTreenodeViewModel.ParentId = column.EntityId;
            bootstrapTreenodeViewModel.type = "column";
            bootstrapTreenodeViewModel.parentType = "view";
            bootstrapTreenodeViewModel.icon = "fa fa-square-o padding-right-20 ";
            bootstrapTreenodeViewModel.text = column.Name + " : " + GetColumnDataType(column);
            bootstrapTreenodeViewModel.Schema = null;
            bootstrapTreenodeViewModel.text = column.Name;
            bootstrapTreenodeViewModel.selectable = "true";

            return (new questStatus(Severity.Success));
        }
        public questStatus FormatBootstrapTreeviewNode(Lookup lookup, out BootstrapTreenodeViewModel lookupNodeViewModel)
        {
            // Setup main node.
            lookupNodeViewModel = new BootstrapTreenodeViewModel();
            lookupNodeViewModel.Id = lookup.Id;
            lookupNodeViewModel.type = "lookup";
            lookupNodeViewModel.icon = "fa fa-search padding-right-20";
            lookupNodeViewModel.text = lookup.Name;
            lookupNodeViewModel.selectable = "true";

            // Put info about the lookup underneath it
            BootstrapTreenodeViewModel labelNodeViewModel = new BootstrapTreenodeViewModel();
            labelNodeViewModel.Id = lookup.Id;
            labelNodeViewModel.type = "lookup-label";
            labelNodeViewModel.icon = "padding-right-40";
            labelNodeViewModel.text = "Label: " + lookup.Label;
            labelNodeViewModel.selectable = "false";
            lookupNodeViewModel.nodes.Add(labelNodeViewModel);

            BootstrapTreenodeViewModel sqlNodeViewModel = new BootstrapTreenodeViewModel();
            sqlNodeViewModel.Id = lookup.Id;
            sqlNodeViewModel.type = "lookup-sql";
            sqlNodeViewModel.icon = "padding-right-40";
            sqlNodeViewModel.text = "SQL: ... ";
            sqlNodeViewModel.selectable = "false";
            sqlNodeViewModel.title = lookup.SQL;
            lookupNodeViewModel.nodes.Add(sqlNodeViewModel);

            return (new questStatus(Severity.Success));
        }
        public questStatus FormatBootstrapTreeviewNode(TypeList typeList, out BootstrapTreenodeViewModel typeListNodeViewModel)
        {
            // Setup main node.
            typeListNodeViewModel = new BootstrapTreenodeViewModel();
            typeListNodeViewModel.Id = typeList.Id;
            typeListNodeViewModel.type = "typeList";
            typeListNodeViewModel.icon = "fa fa-search padding-right-20";
            typeListNodeViewModel.text = typeList.Name;
            typeListNodeViewModel.selectable = "true";

            // Put info about the typeList underneath it
            BootstrapTreenodeViewModel sqlNodeViewModel = new BootstrapTreenodeViewModel();
            sqlNodeViewModel.Id = typeList.Id;
            sqlNodeViewModel.type = "typeList-sql";
            sqlNodeViewModel.icon = "padding-right-40";
            sqlNodeViewModel.text = "SQL: ... ";
            sqlNodeViewModel.selectable = "false";
            sqlNodeViewModel.title = typeList.SQL;
            typeListNodeViewModel.nodes.Add(sqlNodeViewModel);

            return (new questStatus(Severity.Success));
        }
        public string GetColumnDataType(Column column)
        {
            StringBuilder sbIcons = new StringBuilder("");
            if (column.bIsIdentity)
            {
                sbIcons.Append("<span class=\"fa fa-key\" title=\"Identity column\"></span>");
            }
            if (column.bAllowDbNull)
            {
                sbIcons.Append("<span class=\"fa fa-check-square\" title=\"NULLs Allowed\"></span>");
            }
            return (column.DataTypeName + "(" + column.ColumnSize + ")  " + sbIcons.ToString());
        }
        public string GetSchema(string text)
        {
            text = text.Replace("[", "");
            text = text.Replace("]", "");
            string[] pp = text.Split('.');
            return (pp[0]);
        }
        public string GetName(string text)
        {
            text = text.Replace("[", "");
            text = text.Replace("]", "");
            string[] pp = text.Split('.');
            return (pp[1]);
        }
        public string GetColumnName(string text)
        {
            string[] pp = text.Split(':');
            return(pp[0].Trim());
        }
        #endregion


        #region Validations
        //----------------------------------------------------------------------------------------------------------------------------------
        // Validations
        //----------------------------------------------------------------------------------------------------------------------------------
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