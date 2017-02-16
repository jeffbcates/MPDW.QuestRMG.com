using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using Quest.Util.Status;
using Quest.Util.Buffer;
using Quest.Util.Data;
using Quest.Functional.ASM;
using Quest.Functional.MasterPricing;
using Quest.MPDW.Models;
using Quest.MasterPricing.Setup.Models;
using Quest.MasterPricing.Services.Business.Tablesets;
using Quest.MasterPricing.Services.Business.Database;


namespace Quest.MasterPricing.Setup.Modelers
{
    public class TablesetConfigurationModeler : SetupBaseModeler
    {
        #region Declarations
        /*==================================================================================================================================
        * Declarations
        *=================================================================================================================================*/
        private SetupBaseViewModel _setupBaseViewModel = null;
        private TablesetEditorViewModel _tablesetEditorViewModel = null;
        private TablesetConfigurationViewModel _tablesetConfigurationViewModel = null;

        #endregion


        #region Constructors
        /*==================================================================================================================================
        * Constructors
        *=================================================================================================================================*/
        public TablesetConfigurationModeler(HttpRequestBase httpRequestBase, UserSession userSession, SetupBaseViewModel setupBaseViewModel)
            : base(httpRequestBase, userSession)
        {
            this._setupBaseViewModel = setupBaseViewModel;
            initialize();
        }
        public TablesetConfigurationModeler(HttpRequestBase httpRequestBase, UserSession userSession, TablesetEditorViewModel tablesetEditorViewModel)
            : base(httpRequestBase, userSession)
        {
            this._tablesetEditorViewModel = tablesetEditorViewModel;
            this._setupBaseViewModel = tablesetEditorViewModel;
            initialize();
        }
        public TablesetConfigurationModeler(HttpRequestBase httpRequestBase, UserSession userSession, TablesetConfigurationViewModel tablesetConfigurationViewModel)
            : base(httpRequestBase, userSession)
        {
            this._tablesetConfigurationViewModel = tablesetConfigurationViewModel;
            this._setupBaseViewModel = tablesetConfigurationViewModel;
            initialize();
        }


        //TablesetConfigurationViewModel
        #endregion


        #region Public Methods
        /*==================================================================================================================================
        * Public Methods
        *=================================================================================================================================*/

        #region CRUD 
        //----------------------------------------------------------------------------------------------------------------------------------
        // CRUD
        //----------------------------------------------------------------------------------------------------------------------------------
        public questStatus GetDatabaseEntities(DatabaseId databaseId, out List<BootstrapTreenodeViewModel> dbTableNodeList, out List<BootstrapTreenodeViewModel> dbViewNodeList)
        {
            // Initialize
            questStatus status = null;
            dbTableNodeList = null;
            dbViewNodeList = null;

            // Tables
            status = GetDatabaseTables(databaseId, out dbTableNodeList);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }

            // Views
            status = GetDatabaseViews(databaseId, out dbViewNodeList);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus GetDatabaseTables(DatabaseId databaseId, out List<BootstrapTreenodeViewModel> dbTableNodeList)
        {
            // Initialize
            questStatus status = null;
            dbTableNodeList = null;


            // Get db tables
            List<DBTable> dbTableList = null;
            DatabaseMgr databaseMgr = new DatabaseMgr(this.UserSession);
            status = databaseMgr.GetDatabaseTables(databaseId, out dbTableList);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }

            // Format into bootstrap nodes
            dbTableNodeList = new List<BootstrapTreenodeViewModel>();
            foreach (DBTable dbTable in dbTableList)
            {
                BootstrapTreenodeViewModel bootstrapTreenodeViewModel = null;
                status = FormatBootstrapTreeviewNode(dbTable, out bootstrapTreenodeViewModel);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }
                dbTableNodeList.Add(bootstrapTreenodeViewModel);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus GetDatabaseViews(DatabaseId databaseId, out List<BootstrapTreenodeViewModel> dbViewNodeList)
        {
            // Initialize
            questStatus status = null;
            dbViewNodeList = null;


            // Get db tables
            List<DBTable> dbTableList = null;
            DatabaseMgr databaseMgr = new DatabaseMgr(this.UserSession);
            status = databaseMgr.GetDatabaseTables(databaseId, out dbTableList);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }

            // Format into bootstrap nodes
            dbViewNodeList = new List<BootstrapTreenodeViewModel>();
            foreach (DBTable dbTable in dbTableList)
            {
                BootstrapTreenodeViewModel bootstrapTreenodeViewModel = null;
                status = FormatBootstrapTreeviewNode(dbTable, out bootstrapTreenodeViewModel);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }
                dbViewNodeList.Add(bootstrapTreenodeViewModel);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Save(TablesetConfigurationViewModel tablesetConfigurationViewModel)
        {
            // Initialize
            questStatus status = null;
            TablesetId tablesetId = null;


            // Validate: must have a valid TablesetId
            if (tablesetConfigurationViewModel.Tableset.Id < BaseId.VALID_ID)
            {
                return (new questStatus(Severity.Error, "Invalid Tableset identity"));
            }
            tablesetId = new TablesetId(tablesetConfigurationViewModel.Tableset.Id);
            Tableset tableset = null;
            TablesetsMgr tablesetsMgr = new TablesetsMgr(this.UserSession);
            status = tablesetsMgr.Read(tablesetId, out tableset);
            if (!questStatusDef.IsSuccess(status))
            {
                return (new questStatus(Severity.Error, String.Format("Tableset identity not found: {0}", tablesetId.Id)));
            }


            // Transfer model
            if (tablesetConfigurationViewModel.TableList == null)
            {
                tablesetConfigurationViewModel.TableList = new List<BootstrapTreenodeViewModel>();
            }
            TablesetConfiguration tablesetConfiguration = new TablesetConfiguration();
            BufferMgr.TransferBuffer(tablesetConfigurationViewModel, tablesetConfiguration, true);
            foreach (BootstrapTreenodeViewModel _table in tablesetConfigurationViewModel.TableList)
            {
                TablesetTable tablesetTable = new TablesetTable();
                string[] parts = _table.text.Split(new[] { '[', '.', ']' }, StringSplitOptions.RemoveEmptyEntries);
                tablesetTable.Schema = parts[0];
                tablesetTable.Name = parts[1];
                tablesetConfiguration.TablesetTables.Add(tablesetTable);
            }
            foreach (BootstrapTreenodeViewModel _view in tablesetConfigurationViewModel.ViewList)
            {
                TablesetView tablesetView = new TablesetView();
                string[] parts = _view.text.Split(new[] { '[', '.', ']' }, StringSplitOptions.RemoveEmptyEntries);
                tablesetView.Schema = parts[0];
                tablesetView.Name = parts[1];
                tablesetConfiguration.TablesetViews.Add(tablesetView);
            }


            // Save
            tablesetId = null;
            TablesetMgr tablesetMgr = new TablesetMgr(this.UserSession);
            status = tablesetMgr.SaveTablesetConfiguration(tablesetConfiguration, out tablesetId);
            if (!questStatusDef.IsSuccess(status))
            {
                FormatErrorMessage(status, tablesetConfigurationViewModel);
                return (status);
            }
            tablesetConfigurationViewModel.Tableset.Id = tablesetId.Id;

            return (new questStatus(Severity.Success));
        }
        public questStatus Read(TablesetId tablesetId, out TablesetConfigurationViewModel tablesetConfigurationViewModel)
        {
            // Initialize
            questStatus status = null;
            tablesetConfigurationViewModel = null;


            // Read
            TablesetConfiguration tablesetConfiguration = null;
            TablesetMgr tablesetMgr = new TablesetMgr(this.UserSession);
            status = tablesetMgr.ReadTablesetConfiguration(tablesetId, out tablesetConfiguration);
            if (!questStatusDef.IsSuccess(status))
            {
                // Return what we can of the configuration, but with the error status.
                if (tablesetConfiguration != null)
                {
                    tablesetConfigurationViewModel = new TablesetConfigurationViewModel(this.UserSession, this._setupBaseViewModel);
                    BufferMgr.TransferBuffer(tablesetConfiguration, tablesetConfigurationViewModel, true);
                    if (this._tablesetEditorViewModel != null)
                    {
                        tablesetConfigurationViewModel.Id = this._tablesetEditorViewModel.Id;
                        tablesetConfigurationViewModel.TablesetId = this._tablesetEditorViewModel.Id;
                    }
                    else if (this._tablesetConfigurationViewModel != null)
                    {
                        tablesetConfigurationViewModel.Id = this._tablesetConfigurationViewModel.Id;
                        tablesetConfigurationViewModel.TablesetId = this._tablesetConfigurationViewModel.Id;
                    }
                }
                if (status.Message.Contains("DbDatabasesMgr") && (status.Message.Contains("not found")))
                {
                    status = new questStatus(Severity.Error, "Database not found for this table set.");
                }
                return (status);
            }

            // Transfer model.
            tablesetConfigurationViewModel = new TablesetConfigurationViewModel(this.UserSession, this._setupBaseViewModel);
            BufferMgr.TransferBuffer(tablesetConfiguration, tablesetConfigurationViewModel, true);
            tablesetConfigurationViewModel.Id = tablesetConfiguration.Tableset.Id;
            tablesetConfigurationViewModel.TablesetId = tablesetConfiguration.Tableset.Id;


            // Tables
            foreach (TablesetTable tablesetTable in tablesetConfiguration.TablesetTables)
            {
                BootstrapTreenodeViewModel bootstrapTreenodeViewModel = null;
                status = FormatBootstrapTreeviewNode(tablesetTable.Table, out bootstrapTreenodeViewModel);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }
                tablesetConfigurationViewModel.TableList.Add(bootstrapTreenodeViewModel);
            }
            foreach (Table dbTable in tablesetConfiguration.DBTableList)
            {
                BootstrapTreenodeViewModel bootstrapTreenodeViewModel = null;
                status = FormatBootstrapTreeviewNode(dbTable, out bootstrapTreenodeViewModel);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }
                tablesetConfigurationViewModel.DBTableList.Add(bootstrapTreenodeViewModel);
            }

            // Views
            foreach (TablesetView tablesetView in tablesetConfiguration.TablesetViews)
            {
                BootstrapTreenodeViewModel bootstrapTreenodeViewModel = null;
                status = FormatBootstrapTreeviewNode(tablesetView.View, out bootstrapTreenodeViewModel);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }
                tablesetConfigurationViewModel.ViewList.Add(bootstrapTreenodeViewModel);
            }
            foreach (View dbView in tablesetConfiguration.DBViewList)
            {
                BootstrapTreenodeViewModel bootstrapTreenodeViewModel = null;
                status = FormatBootstrapTreeviewNode(dbView, out bootstrapTreenodeViewModel);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }
                tablesetConfigurationViewModel.DBViewList.Add(bootstrapTreenodeViewModel);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Delete(TablesetId tablesetId)
        {
            // Initialize
            questStatus status = null;


            // Delete
            TablesetsMgr tablesetsMgr = new TablesetsMgr(this.UserSession);
            status = tablesetsMgr.Delete(tablesetId);
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