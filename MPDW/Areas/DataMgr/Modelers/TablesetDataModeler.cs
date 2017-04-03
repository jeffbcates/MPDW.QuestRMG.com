using System;
using System.Collections.Generic;
using System.Web;
using Quest.Util.Status;
using Quest.Util.Buffer;
using Quest.Util.Data;
using Quest.Functional.ASM;
using Quest.Functional.FMS;
using Quest.MPDW.Models;
using Quest.MPDW.Modelers;
using Quest.MasterPricing.DataMgr.Models;
using Quest.Functional.MasterPricing;
using Quest.MasterPricing.Services.Business.Tablesets;
using Quest.MasterPricing.Services.Business.Database;
using Quest.MasterPricing.Services.Business.Filters;


namespace Quest.MasterPricing.DataMgr.Modelers
{
    public class TablesetDataModeler : DataMgrBaseModeler
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
        public TablesetDataModeler(HttpRequestBase httpRequestBase, UserSession userSession, DataMgrBaseViewModel dataMgrBaseViewModel)
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
        public questStatus Read(TablesetId tablesetId, out DataMgrTablesetViewModel dataMgrTablesetViewModel)
        {
            // Initialize
            questStatus status = null;
            dataMgrTablesetViewModel = null;


            // Read tableset data management.
            TablesetDataManagement tablesetDataManagement = null;
            TablesetMgr tablesetMgr = new TablesetMgr(this.UserSession);
            status = tablesetMgr.ReadTablesetDataManagement(tablesetId, out tablesetDataManagement);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }

            // List lookups
            DatabaseId databaseId = new DatabaseId(tablesetDataManagement.TablesetConfiguration.Database.Id);
            List<BootstrapTreenodeViewModel> lookupNodeList = null;
            status = ListLookups(databaseId, out lookupNodeList);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }

            // List TypeLists
            List<BootstrapTreenodeViewModel> typeListNodeList = null;
            status = ListTypeLists(databaseId, out typeListNodeList);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }

            // Load all folders
            FolderId folderId = null;
            List<FilterFolder> filterFolderList = null;
            FilterFoldersMgr filterFoldersMgr = new FilterFoldersMgr(this.UserSession);
            status = filterFoldersMgr.Load(folderId, out filterFolderList);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }


            // Build model
            dataMgrTablesetViewModel = new DataMgrTablesetViewModel(this.UserSession, this._dataMgrBaseViewModel);
            dataMgrTablesetViewModel.TablesetId = tablesetId.Id;
            BufferMgr.TransferBuffer(tablesetDataManagement.TablesetConfiguration.Tableset, dataMgrTablesetViewModel.Tableset);
            if (dataMgrTablesetViewModel.Tableset.Summary == null) { dataMgrTablesetViewModel.Tableset.Summary = ""; }
            dataMgrTablesetViewModel.Tableset.LastRefresh = "";
            dataMgrTablesetViewModel.Tableset.LastRefresh = tablesetDataManagement.TablesetConfiguration.Tableset.LastRefresh.HasValue ?
                    tablesetDataManagement.TablesetConfiguration.Tableset.LastRefresh.Value.ToString("MM/dd/yyyy HH:mm:ss") : "";


            foreach (TablesetTable tablesetTable in tablesetDataManagement.TablesetConfiguration.TablesetTables)
            {
                tablesetTable.bColumnsSelectable = false;
                BootstrapTreenodeViewModel bootstrapTreenodeViewModel = null;
                status = FormatBootstrapTreeviewNode(tablesetTable, out bootstrapTreenodeViewModel);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }
                dataMgrTablesetViewModel.TableList.Add(bootstrapTreenodeViewModel);
            }
            foreach (TablesetView tablesetView in tablesetDataManagement.TablesetConfiguration.TablesetViews)
            {
                BootstrapTreenodeViewModel bootstrapTreenodeViewModel = null;
                status = FormatBootstrapTreeviewNode(tablesetView, out bootstrapTreenodeViewModel);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }
                dataMgrTablesetViewModel.ViewList.Add(bootstrapTreenodeViewModel);
            }

            dataMgrTablesetViewModel.Lookups = lookupNodeList;
            dataMgrTablesetViewModel.TypeLists = typeListNodeList;


            foreach (FilterFolder filterFolder in filterFolderList)
            {
                BootstrapTreenodeViewModel bootstrapTreenodeViewModel = null;
                status = FormatBootstrapTreeviewNode(filterFolder, out bootstrapTreenodeViewModel);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }
                dataMgrTablesetViewModel.Folders.Add(bootstrapTreenodeViewModel);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus ListLookups(DatabaseId databaseId, out List<BootstrapTreenodeViewModel> lookupNodeList)
        {
            // Initialize
            questStatus status = null;
            lookupNodeList = null;


            // Set up query options.
            // TEMPORARY: OPTIMIZE THIS
            QueryOptions queryOptions = new QueryOptions();
            List<SearchField> searchFieldList = new List<SearchField>();
            SearchField searchField = new SearchField();
            searchField.Name = "DatabaseId";
            searchField.SearchOperation = SearchOperation.Equal;
            searchField.Type = typeof(int);
            searchField.Value = databaseId.Id.ToString();
            searchFieldList.Add(searchField);
            SearchOptions searchOptions = new SearchOptions();
            searchOptions.SearchFieldList = searchFieldList;
            queryOptions.SearchOptions = searchOptions;


            // Read lookups
            LookupsMgr lookupsMgr = new LookupsMgr(this.UserSession);
            QueryResponse queryResponse = null;
            List<Lookup> lookupList = null;
            status = lookupsMgr.List(queryOptions, out lookupList, out queryResponse);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }


            // Transfer model.
            lookupNodeList = new List<BootstrapTreenodeViewModel>();
            foreach (Lookup lookup in lookupList)
            {
                BootstrapTreenodeViewModel lookupNode = null;
                FormatBootstrapTreeviewNode(lookup, out lookupNode);
                lookupNodeList.Add(lookupNode);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus ListTypeLists(DatabaseId databaseId, out List<BootstrapTreenodeViewModel> typeListNodeList)
        {
            // Initialize
            questStatus status = null;
            typeListNodeList = null;


            // Set up query options.
            // TEMPORARY: OPTIMIZE THIS
            QueryOptions queryOptions = new QueryOptions();
            List<SearchField> searchFieldList = new List<SearchField>();
            SearchField searchField = new SearchField();
            searchField.Name = "DatabaseId";
            searchField.SearchOperation = SearchOperation.Equal;
            searchField.Type = typeof(int);
            searchField.Value = databaseId.Id.ToString();
            searchFieldList.Add(searchField);
            SearchOptions searchOptions = new SearchOptions();
            searchOptions.SearchFieldList = searchFieldList;
            queryOptions.SearchOptions = searchOptions;


            // Read typeLists
            TypeListsMgr typeListsMgr = new TypeListsMgr(this.UserSession);
            QueryResponse queryResponse = null;
            List<TypeList> typeListList = null;
            status = typeListsMgr.List(queryOptions, out typeListList, out queryResponse);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }


            // Transfer model.
            typeListNodeList = new List<BootstrapTreenodeViewModel>();
            foreach (TypeList typeList in typeListList)
            {
                BootstrapTreenodeViewModel typeListNode = null;
                FormatBootstrapTreeviewNode(typeList, out typeListNode);
                typeListNodeList.Add(typeListNode);
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