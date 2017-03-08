using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Linq;
using System.Linq.Dynamic;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Quest.Util.Status;
using Quest.Util.Buffer;
using Quest.Util.Data;
using Quest.Functional.ASM;
using Quest.Functional.FMS;
using Quest.Functional.MasterPricing;
using Quest.MPDW.Services.Data;
using Quest.Services.Dbio.MasterPricing;
using Quest.MasterPricing.Services.Data.Database;


namespace Quest.MasterPricing.Services.Data.Setup
{
    public class DbDatabaseMgr : DbMgr
    {
        #region Declarations
        /*==================================================================================================================================
         * Declarations
         *=================================================================================================================================*/
        private UserSession _userSession = null;

        #endregion


        #region Constructors
        /*==================================================================================================================================
         * Constructors
         *=================================================================================================================================*/
        public DbDatabaseMgr()
            : base()
        {
            initialize();
        }
        public DbDatabaseMgr(UserSession userSession)
            : base()
        {
            _userSession = userSession;
            initialize();
        }
        #endregion


        #region Properties
        /*==================================================================================================================================
         * Properties
         *=================================================================================================================================*/
        public UserSession UserSession
        {
            get
            {
                return (this._userSession);
            }
        }
        #endregion


        #region Public Methods
        /*==================================================================================================================================
         * Public Methods
         *=================================================================================================================================*/

        #region Identifer-based Usage
        //
        // Identifer-based Usage
        //
        public questStatus GetDatabaseMetainfo(DatabaseId databaseId, out DatabaseMetaInfo databaseMetaInfo)
        {
            // Initialize
            questStatus status = null;
            databaseMetaInfo = null;


            // Get table info
            Dictionary<DBTable, List<Column>> dbTableDictionary = null;
            status = GetDatabaseColumns(databaseId, out dbTableDictionary);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }


            // Get view info
            Dictionary<DBView, List<Column>> dbViewDictionary = null;
            status = GetDatabaseColumns(databaseId, out dbViewDictionary);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }


            // Get stored procedure info
            List<StoredProcedure> storedProcedureList = null;
            status = GetDatabaseStoredProcedures(databaseId, out storedProcedureList);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }


            // Builld metainfo block
            databaseMetaInfo = new DatabaseMetaInfo();
            databaseMetaInfo.DatabaseId = databaseId;
            databaseMetaInfo.DBTableDictionary = dbTableDictionary;
            databaseMetaInfo.DBViewDictionary = dbViewDictionary;
            databaseMetaInfo.StoredProcedureList = storedProcedureList;

            return (new questStatus(Severity.Success));
        }
        public questStatus RefreshSchema(DatabaseId databaseId)
        {
            // Initialize
            questStatus status = null;


            // Get tables
            Dictionary<DBTable, List<Column>> dictDBTableColumns = null;
            status = GetDatabaseColumns(databaseId, out dictDBTableColumns);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }

            // Get views
            Dictionary<DBView, List<Column>> dictDBViewColumns = null;
            status = GetDatabaseColumns(databaseId, out dictDBViewColumns);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }

            // Get stored procedures
            List<Quest.Functional.MasterPricing.StoredProcedure> storedProcedureList = null;
            status = GetDatabaseStoredProcedures(databaseId, out storedProcedureList);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }

            /*
             * Start transaction
             */
            string transactionName = null;
            status = GetUniqueTransactionName("StoreDatabaseMetadata", out transactionName);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            DbMgrTransaction trans = null;
            status = BeginTransaction(transactionName, out trans);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }


            /*
             * Instantiate data managers to remove then readd database entity info.
             */
            DbTablesMgr dbTablesMgr = new DbTablesMgr(this.UserSession);
            DbViewsMgr dbViewsMgr = new DbViewsMgr(this.UserSession);
            DbColumnsMgr dbColumnsMgr = new DbColumnsMgr(this.UserSession);
            DbStoredProceduresMgr dbStoredProceduresMgr = new DbStoredProceduresMgr(this.UserSession);
            DbStoredProcedureParametersMgr dbStoredProcedureParametersMgr = new DbStoredProcedureParametersMgr(this.UserSession);


            #region Remove database metadata
            /*
             * Remove database metadata
             */
            // Tables
            foreach (KeyValuePair<DBTable, List<Column>> kvp in dictDBTableColumns)
            {
                TableId tableId = new TableId(kvp.Key.Id);
                status = dbColumnsMgr.Delete(trans, tableId);
                if (!questStatusDef.IsSuccess(status))
                {
                    RollbackTransaction(trans);
                    return (status);
                }
            }
            status = dbTablesMgr.Delete(trans, databaseId);
            if (!questStatusDef.IsSuccess(status))
            {
                RollbackTransaction(trans);
                return (status);
            }

            // Views
            foreach (KeyValuePair<DBView, List<Column>> kvp in dictDBViewColumns)
            {
                ViewId viewId = new ViewId(kvp.Key.Id);
                status = dbColumnsMgr.Delete(trans, viewId);
                if (!questStatusDef.IsSuccess(status))
                {
                    RollbackTransaction(trans);
                    return (status);
                }
            }
            status = dbViewsMgr.Delete(trans, databaseId);
            if (!questStatusDef.IsSuccess(status))
            {
                RollbackTransaction(trans);
                return (status);
            }

            // Stored procedures
            foreach (StoredProcedure storedProcedure in storedProcedureList)
            {
                StoredProcedureId storedProcedureId = new StoredProcedureId(storedProcedure.Id);
                status = dbStoredProcedureParametersMgr.Delete(trans, storedProcedureId);
                if (!questStatusDef.IsSuccess(status))
                {
                    RollbackTransaction(trans);
                    return (status);
                }
            }
            dbStoredProceduresMgr.Delete(trans, databaseId);
            status = dbViewsMgr.Delete(trans, databaseId);
            if (!questStatusDef.IsSuccess(status))
            {
                RollbackTransaction(trans);
                return (status);
            }
            #endregion


            #region Store database metadata
            /*
             * Store database metadata
             */
            // Tables
            foreach (KeyValuePair<DBTable, List<Column>> kvp in dictDBTableColumns)
            {
                Table table = new Table();
                BufferMgr.TransferBuffer(kvp.Key, table);
                table.DatabaseId = databaseId.Id;
                TableId tableId = null;
                status = dbTablesMgr.Create(trans, table, out tableId);
                if (!questStatusDef.IsSuccess(status))
                {
                    RollbackTransaction(trans);
                    return (status);
                }
                foreach (Column column in kvp.Value)
                {
                    column.EntityTypeId = EntityType.Table;
                    column.EntityId = tableId.Id;
                    ColumnId columnId = null;
                    status = dbColumnsMgr.Create(column, out columnId);
                    if (!questStatusDef.IsSuccess(status))
                    {
                        RollbackTransaction(trans);
                        return (status);
                    }
                }
            }

            // Views
            foreach (KeyValuePair<DBView, List<Column>> kvp in dictDBViewColumns)
            {
                View view = new View();
                BufferMgr.TransferBuffer(kvp.Key, view);
                view.DatabaseId = databaseId.Id;
                ViewId viewId = null;
                status = dbViewsMgr.Create(trans, view, out viewId);
                if (!questStatusDef.IsSuccess(status))
                {
                    RollbackTransaction(trans);
                    return (status);
                }
                foreach (Column column in kvp.Value)
                {
                    column.EntityTypeId = EntityType.View;
                    column.EntityId = viewId.Id;
                    ColumnId columnId = null;
                    status = dbColumnsMgr.Create(column, out columnId);
                    if (!questStatusDef.IsSuccess(status))
                    {
                        RollbackTransaction(trans);
                        return (status);
                    }
                }
            }

            // Stored Procedures
            foreach (StoredProcedure storedProcedure in storedProcedureList)
            {
                StoredProcedureId storedProcedureId = null;
                storedProcedure.DatabaseId = databaseId.Id;
                status = dbStoredProceduresMgr.Create(trans, storedProcedure, out storedProcedureId);
                if (!questStatusDef.IsSuccess(status))
                {
                    RollbackTransaction(trans);
                    return (status);
                }
                foreach (StoredProcedureParameter storedProcedureParameter in storedProcedure.ParameterList)
                {
                    storedProcedureParameter.StoredProcedureId = storedProcedureId.Id;
                    StoredProcedureParameterId storedProcedureParameterId = null;
                    status = dbStoredProcedureParametersMgr.Create(storedProcedureParameter, out storedProcedureParameterId);
                    if (!questStatusDef.IsSuccess(status))
                    {
                        RollbackTransaction(trans);
                        return (status);
                    }
                }
            }
            #endregion


            // Update database refresh date/time
            Quest.Functional.MasterPricing.Database database = null;
            DbDatabasesMgr dbDatabasesMgr = new DbDatabasesMgr(this.UserSession);
            status = dbDatabasesMgr.Read(databaseId, out database);
            if (!questStatusDef.IsSuccess(status))
            {
                RollbackTransaction(trans);
                return (status);
            }
            database.LastRefresh = DateTime.Now;
            status = dbDatabasesMgr.Update(database);
            if (!questStatusDef.IsSuccess(status))
            {
                RollbackTransaction(trans);
                return (status);
            }


            // Commit transaction
            status = CommitTransaction(trans);
            if (!questStatusDef.IsSuccess(status))
            {
                RollbackTransaction(trans);
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus ReadDatabaseEntities(DatabaseId databaseId, out DatabaseEntities databaseEntities)
        {
            // Initialize
            questStatus status = null;
            databaseEntities = null;

            
            // Read database
            DbDatabasesMgr dbDatabasesMgr = new DbDatabasesMgr(this.UserSession);
            Quest.Functional.MasterPricing.Database database = null;
            status = dbDatabasesMgr.Read(databaseId, out database);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }

            DbColumnsMgr dbColumnsMgr = new DbColumnsMgr(this.UserSession);


            // Read database tables
            DbTablesMgr dbTablesMgr = new DbTablesMgr(this.UserSession);
            List<Table> tableList = null;
            status = dbTablesMgr.Read(databaseId, out tableList);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            foreach (Table table in tableList)
            {
                TableId tableId = new TableId(table.Id);
                List<Column> columnList = null;
                status = dbColumnsMgr.Read(tableId, out columnList);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }
                table.ColumnList = columnList;
            }


            // Read database views
            DbViewsMgr dbViewsMgr = new DbViewsMgr(this.UserSession);
            List<View> viewList = null;
            status = dbViewsMgr.Read(databaseId, out viewList);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            foreach (View view in viewList)
            {
                ViewId viewId = new ViewId(view.Id);
                List<Column> columnList = null;
                status = dbColumnsMgr.Read(viewId, out columnList);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }
                view.ColumnList = columnList;
            }

            // Read stored procedures
            DbStoredProceduresMgr dbStoredProceduresMgr = new DbStoredProceduresMgr(this.UserSession);
            List<StoredProcedure> storedProcedureList = null;
            status = dbStoredProceduresMgr.Read(databaseId, out storedProcedureList);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            DbStoredProcedureParametersMgr dbStoredProcedureParametersMgr = new DbStoredProcedureParametersMgr(this.UserSession);
            foreach (StoredProcedure storedProcedure in storedProcedureList)
            {
                StoredProcedureId storedProcedureId = new StoredProcedureId(storedProcedure.Id);
                List<StoredProcedureParameter> storedProcedureParameterList = null;
                status = dbStoredProcedureParametersMgr.Read(storedProcedureId, out storedProcedureParameterList);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }
                storedProcedure.ParameterList = storedProcedureParameterList;
            }

            // Return entities.
            databaseEntities = new DatabaseEntities();
            databaseEntities.Database = database;
            databaseEntities.TableList = tableList;
            databaseEntities.ViewList = viewList;
            databaseEntities.StoredProcedureList = storedProcedureList;

            return (new questStatus(Severity.Success));
        }
        #endregion

        public questStatus OpenDatabase(Quest.Functional.MasterPricing.DatabaseId databaseId, out SqlConnection sqlConnection)
        {
            // Initialize 
            questStatus status = null;
            sqlConnection = null;


            // Read database
            Quest.Functional.MasterPricing.Database database = null;
            status = readDatabase(databaseId, out database);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }

            try
            {
                sqlConnection = new SqlConnection(database.ConnectionString);
                sqlConnection.Open();
            }
            catch (System.ArgumentException ex)
            {
                status = new questStatus(Severity.Fatal, String.Format("Cannot open database {0}.  Verify your connection string.", database.Name));
                return (status);
            }
            catch (System.Exception ex)
            {
                status = new questStatus(Severity.Fatal, String.Format("Cannot open database {0}: {1}", database.Name, ex.Message));
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus GetDatabaseTables(Quest.Functional.MasterPricing.DatabaseId databaseId, out List<DBTable> dbTableList)
        {
            // Initialize 
            questStatus status = null;
            dbTableList = null;


            // Get database tables
            status = getDatabaseTables(databaseId, out dbTableList);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus GetDatabaseViews(Quest.Functional.MasterPricing.DatabaseId databaseId, out List<DBView> dbViewList)
        {
            // Initialize 
            questStatus status = null;
            dbViewList = null;


            // Get database views
            status = getDatabaseViews(databaseId, out dbViewList);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus GetDatabaseColumns(Quest.Functional.MasterPricing.DatabaseId databaseId, out Dictionary<DBTable, List<Column>> dictDBColumn)
        {
            // Initialize 
            questStatus status = null;
            dictDBColumn = null;


            // Get columns
            status = getDatabaseColumns(databaseId, out dictDBColumn);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus GetDatabaseColumns(Quest.Functional.MasterPricing.DatabaseId databaseId, out Dictionary<DBView, List<Column>> dictDBColumn)
        {
            // Initialize 
            questStatus status = null;
            dictDBColumn = null;


            // Get columns
            status = status = getDatabaseColumns(databaseId, out dictDBColumn);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus GetDatabaseStoredProcedures(Quest.Functional.MasterPricing.DatabaseId databaseId, out List<StoredProcedure> storedProcedureList)
        {
            // Initialize 
            questStatus status = null;
            storedProcedureList = null;


            // Get database
            Quest.Functional.MasterPricing.Database database = null;
            DbDatabasesMgr dbDatabasesMgr = new DbDatabasesMgr(this._userSession);
            status = dbDatabasesMgr.Read(databaseId, out database);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }

            // Get database stored procedures
            status = GetDatabaseStoredProcedures(database, out storedProcedureList);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus StoreDatabaseTables(Quest.Functional.MasterPricing.DatabaseId databaseId, List<Table> tableList, out List<Table> tableIdList)
        {
            // Initialize
            questStatus status = null;
            tableIdList = null;
            DbTablesMgr dbTablesMgr = new DbTablesMgr(this._userSession);
            string transactionName = null;
            DbMgrTransaction trans = null;

            try
            {
                // Start transaction
                status = GetUniqueTransactionName("StoreDatabaseTables", out transactionName);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }
                status = dbTablesMgr.BeginTransaction(transactionName, out trans);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }

                // Store tables
                tableIdList = new List<Table>();
                foreach (Table table in tableList)
                {
                    table.DatabaseId = databaseId.Id;
                    TableId tableId = null;
                    status = dbTablesMgr.Create(trans, table, out tableId);
                    if (!questStatusDef.IsSuccess(status))
                    {
                        dbTablesMgr.RollbackTransaction(trans);
                        return (status);
                    }
                    Table newTable = new Table();
                    BufferMgr.TransferBuffer(table, newTable);
                    newTable.Id = tableId.Id;
                    tableIdList.Add(newTable);
                }
                dbTablesMgr.CommitTransaction(trans);
            }
            catch (System.Exception ex)
            {
                dbTablesMgr.RollbackTransaction(trans);
                status = new questStatus(Severity.Fatal, String.Format("EXCEPTION: {0}.{1}: {2}",
                        this.GetType().ToString(), MethodInfo.GetCurrentMethod().Name, ex.Message));
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus StoreDatabaseViews(Quest.Functional.MasterPricing.DatabaseId databaseId, List<View> viewList, out List<View> viewIdList)
        {
            // Initialize
            questStatus status = null;
            viewIdList = null;
            DbViewsMgr dbViewsMgr = new DbViewsMgr(this._userSession);
            string transactionName = null;
            DbMgrTransaction trans = null;

            try
            {
                // Start transaction
                status = GetUniqueTransactionName("StoreDatabaseViews", out transactionName);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }
                status = dbViewsMgr.BeginTransaction(transactionName, out trans);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }

                // Store views
                viewIdList = new List<View>();
                foreach (View view in viewList)
                {
                    view.DatabaseId = databaseId.Id;
                    ViewId viewId = null;
                    status = dbViewsMgr.Create(trans, view, out viewId);
                    if (!questStatusDef.IsSuccess(status))
                    {
                        dbViewsMgr.RollbackTransaction(trans);
                        return (status);
                    }
                    View newView = new View();
                    BufferMgr.TransferBuffer(view, newView);
                    newView.Id = viewId.Id;
                    viewIdList.Add(newView);
                }
                dbViewsMgr.CommitTransaction(trans);
            }
            catch (System.Exception ex)
            {
                dbViewsMgr.RollbackTransaction(trans);
                status = new questStatus(Severity.Fatal, String.Format("EXCEPTION: {0}.{1}: {2}",
                        this.GetType().ToString(), MethodInfo.GetCurrentMethod().Name, ex.Message));
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus StoreDatabaseColumns(Quest.Functional.MasterPricing.DatabaseId databaseId, Dictionary<Table, List<Column>> dictDBColumn, out List<Table> tableIdList)
        {
            // Initialize 
            questStatus status = null;
            tableIdList = null;
            DbTablesMgr dbTablesMgr = new DbTablesMgr(this._userSession);
            string transactionName = null;
            DbMgrTransaction trans = null;

            try
            {
                // Start transaction
                status = GetUniqueTransactionName("StoreDatabaseTableColumns", out transactionName);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }
                status = dbTablesMgr.BeginTransaction(transactionName, out trans);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }

                // Store tables and columns
                tableIdList = new List<Table>();
                DbColumnsMgr dbColumnsMgr = new DbColumnsMgr(this._userSession);
                foreach (KeyValuePair<Table, List<Column>> kvp in dictDBColumn)
                {
                    // Store table
                    Table table = kvp.Key;
                    table.DatabaseId = databaseId.Id;
                    TableId tableId = null;
                    status = dbTablesMgr.Create(trans, table, out tableId);
                    if (!questStatusDef.IsSuccess(status))
                    {
                        dbTablesMgr.RollbackTransaction(trans);
                        return (status);
                    }
                    Table newTable = new Table();
                    BufferMgr.TransferBuffer(table, newTable);
                    newTable.Id = tableId.Id;

                    // Store columns
                    foreach (Column column in kvp.Value)
                    {
                        column.EntityTypeId = EntityType.Table;
                        column.EntityId = tableId.Id;
                        ColumnId columnId = null;
                        status = dbColumnsMgr.Create(trans, column, out columnId);
                        if (!questStatusDef.IsSuccess(status))
                        {
                            dbTablesMgr.RollbackTransaction(trans);
                            return (status);
                        }
                        Column newColumn = new Column();
                        BufferMgr.TransferBuffer(column, newColumn);
                        newColumn.Id = columnId.Id;
                        newTable.ColumnList.Add(newColumn);
                    }
                    tableIdList.Add(newTable);
                }
                dbTablesMgr.CommitTransaction(trans);
            }
            catch (System.Exception ex)
            {
                dbTablesMgr.RollbackTransaction(trans);
                status = new questStatus(Severity.Fatal, String.Format("EXCEPTION: {0}.{1}: {2}",
                        this.GetType().ToString(), MethodInfo.GetCurrentMethod().Name, ex.Message));
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus StoreDatabaseColumns(Quest.Functional.MasterPricing.DatabaseId databaseId, Dictionary<View, List<Column>> dictDBColumn, out List<View> viewIdList)
        {
            // Initialize 
            questStatus status = null;
            viewIdList = null;
            DbViewsMgr dbViewsMgr = new DbViewsMgr(this._userSession);
            string transactionName = null;
            DbMgrTransaction trans = null;

            try
            {
                // Start transaction
                status = GetUniqueTransactionName("StoreDatabaseViewColumns", out transactionName);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }
                status = dbViewsMgr.BeginTransaction(transactionName, out trans);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }

                // Store views and columns
                viewIdList = new List<View>();
                DbColumnsMgr dbColumnsMgr = new DbColumnsMgr(this._userSession);
                foreach (KeyValuePair<View, List<Column>> kvp in dictDBColumn)
                {
                    // Store view
                    View view = kvp.Key;
                    view.DatabaseId = databaseId.Id;
                    ViewId viewId = null;
                    status = dbViewsMgr.Create(trans, view, out viewId);
                    if (!questStatusDef.IsSuccess(status))
                    {
                        dbViewsMgr.RollbackTransaction(trans);
                        return (status);
                    }
                    View newView = new View();
                    BufferMgr.TransferBuffer(view, newView);
                    newView.Id = viewId.Id;

                    // Store columns
                    foreach (Column column in kvp.Value)
                    {
                        column.EntityTypeId = EntityType.Table;
                        column.EntityId = viewId.Id;
                        ColumnId columnId = null;
                        status = dbColumnsMgr.Create(trans, column, out columnId);
                        if (!questStatusDef.IsSuccess(status))
                        {
                            dbViewsMgr.RollbackTransaction(trans);
                            return (status);
                        }
                        Column newColumn = new Column();
                        BufferMgr.TransferBuffer(column, newColumn);
                        newColumn.Id = columnId.Id;
                        newView.ColumnList.Add(newColumn);
                    }
                    viewIdList.Add(newView);
                }
                dbViewsMgr.CommitTransaction(trans);
            }
            catch (System.Exception ex)
            {
                dbViewsMgr.RollbackTransaction(trans);
                status = new questStatus(Severity.Fatal, String.Format("EXCEPTION: {0}.{1}: {2}",
                        this.GetType().ToString(), MethodInfo.GetCurrentMethod().Name, ex.Message));
                return (status);
            }
            return (new questStatus(Severity.Success));
        }

        public questStatus Delete(DatabaseId databaseId)
        {
            // Initialize
            questStatus status = null;


            /*
             * Start transaction
             */
            string transactionName = null;
            status = GetUniqueTransactionName("DeleteDatabase_" + databaseId.Id.ToString(), out transactionName);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            DbMgrTransaction trans = null;
            status = BeginTransaction(transactionName, out trans);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }


            /*
             * Get database schema
             */
            Quest.Functional.MasterPricing.Database database = null;
            status = GetDatabaseSchema(trans, databaseId, out database);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }

            #region Stored Procedures
            //
            // Stored Procedures
            //
            // Delete all stored procedure parameters
            DbStoredProcedureParametersMgr dbStoredProcedureParametersMgr = new DbStoredProcedureParametersMgr(this.UserSession);
            foreach (StoredProcedure storedProcedure in database.StoredProcedureList)
            {
                StoredProcedureId storedProcedureId = new StoredProcedureId(storedProcedure.Id);
                status = dbStoredProcedureParametersMgr.Delete(trans, storedProcedureId);
                if (!questStatusDef.IsSuccess(status))
                {
                    RollbackTransaction(trans);
                    return (status);
                }

                // Delete stored procedure
                DbStoredProceduresMgr dbStoredProceduresMgr = new DbStoredProceduresMgr(this.UserSession);
                status = dbStoredProceduresMgr.Delete(trans, storedProcedureId);
                if (!questStatusDef.IsSuccess(status))
                {
                    RollbackTransaction(trans);
                    return (status);
                }
            }

            #endregion


            #region Tables
            //
            // Tables
            //
            // Delete all table columns
            DbColumnsMgr dbColumnsMgr = new DbColumnsMgr(this.UserSession);
            foreach (Table table in database.TableList)
            {
                TableId tableId = new TableId(table.Id);
                status = dbColumnsMgr.Delete(trans, tableId);
                if (!questStatusDef.IsSuccess(status))
                {
                    RollbackTransaction(trans);
                    return (status);
                }

                // Delete table
                DbTablesMgr dbTablesMgr = new DbTablesMgr(this.UserSession);
                status = dbTablesMgr.Delete(trans, tableId);
                if (!questStatusDef.IsSuccess(status))
                {
                    RollbackTransaction(trans);
                    return (status);
                }
            }
            #endregion


            #region Views
            //
            // Views
            //

            // Delete all view columns
            foreach (View view in database.ViewList)
            {
                ViewId viewId = new ViewId(view.Id);
                status = dbColumnsMgr.Delete(trans, viewId);
                if (!questStatusDef.IsSuccess(status))
                {
                    RollbackTransaction(trans);
                    return (status);
                }

                // Delete view
                DbViewsMgr dbViewsMgr = new DbViewsMgr(this.UserSession);
                status = dbViewsMgr.Delete(trans, viewId);
                if (!questStatusDef.IsSuccess(status))
                {
                    RollbackTransaction(trans);
                    return (status);
                }
            }
            #endregion


            #region Database
            //
            // Database
            //
            DbDatabasesMgr dbDatabasesMgr = new DbDatabasesMgr(this.UserSession);
            status = dbDatabasesMgr.Delete(trans, databaseId);
            if (!questStatusDef.IsSuccess(status))
            {
                RollbackTransaction(trans);
                return (status);
            }
            #endregion



            // Commit transaction
            status = CommitTransaction(trans);
            if (!questStatusDef.IsSuccess(status))
            {
                RollbackTransaction(trans);
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus GetDatabaseSchema(DatabaseId databaseId, out Quest.Functional.MasterPricing.Database database)
        {
            // Initialize
            questStatus status = null;
            database = null;



            // Start transaction
            string transactionName = null;
            status = GetUniqueTransactionName("GetDatabaseSchema_" + databaseId.Id.ToString(), out transactionName);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            DbMgrTransaction trans = null;
            status = BeginTransaction(transactionName, out trans);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }


            // Get schema.
            status = GetDatabaseSchema(trans, databaseId, out database);
            if (!questStatusDef.IsSuccess(status))
            {
                RollbackTransaction(trans);
                return (status);
            }


            // Commit transaction
            status = CommitTransaction(trans);
            if (!questStatusDef.IsSuccess(status))
            {
                RollbackTransaction(trans);
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus GetDatabaseSchema(DbMgrTransaction trans, DatabaseId databaseId, out Quest.Functional.MasterPricing.Database database)
        {
            // Initialize
            questStatus status = null;
            database = null;



            #region Database
            //
            // Database
            //
            DbDatabasesMgr dbDatabasesMgr = new DbDatabasesMgr(this.UserSession);
            status = dbDatabasesMgr.Read(trans, databaseId, out database);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            #endregion


            #region Tables
            //
            // Tables
            //
            DbTablesMgr dbTablesMgr = new DbTablesMgr(this.UserSession);
            List<Table> tableList = null;
            status = dbTablesMgr.Read(trans, databaseId, out tableList);
            if (!questStatusDef.IsSuccess(status))
            {
                RollbackTransaction(trans);
                return (status);
            }

            // Get all table columns
            DbColumnsMgr dbColumnsMgr = new DbColumnsMgr(this.UserSession);
            foreach (Table table in tableList)
            {
                TableId tableId = new TableId(table.Id);
                List<Column> columnList = null;
                status = dbColumnsMgr.Read(trans, tableId, out columnList);
                if (!questStatusDef.IsSuccess(status))
                {
                    RollbackTransaction(trans);
                    return (status);
                }
                table.ColumnList = columnList;
            }
            database.TableList = tableList;
            #endregion


            #region Views
            //
            // Views
            //
            DbViewsMgr dbViewsMgr = new DbViewsMgr(this.UserSession);
            List<View> viewList = null;
            status = dbViewsMgr.Read(trans, databaseId, out viewList);
            if (!questStatusDef.IsSuccess(status))
            {
                RollbackTransaction(trans);
                return (status);
            }

            // Get all view columns
            foreach (View view in viewList)
            {
                ViewId viewId = new ViewId(view.Id);
                List<Column> columnList = null;
                status = dbColumnsMgr.Read(trans, viewId, out columnList);
                if (!questStatusDef.IsSuccess(status))
                {
                    RollbackTransaction(trans);
                    return (status);
                }
                view.ColumnList = columnList;
            }
            database.ViewList = viewList;
            #endregion


            #region Stored Procedures
            //
            // Stored Procedures
            //
            // Get all stored procedures
            DbStoredProceduresMgr dbStoredProceduresMgr = new DbStoredProceduresMgr(this.UserSession);
            List<StoredProcedure> storedProcedureList = null;
            status = dbStoredProceduresMgr.Read(trans, databaseId, out storedProcedureList);
            if (!questStatusDef.IsSuccess(status))
            {
                RollbackTransaction(trans);
                return (status);
            }

            // Get all stored procedure parameters
            DbStoredProcedureParametersMgr dbStoredProcedureParametersMgr = new DbStoredProcedureParametersMgr(this.UserSession);
            foreach (StoredProcedure storedProcedure in storedProcedureList)
            {
                StoredProcedureId storedProcedureId = new StoredProcedureId(storedProcedure.Id);
                List<StoredProcedureParameter> storeProcedureParameterList = null;
                status = dbStoredProcedureParametersMgr.Read(trans, storedProcedureId, out storeProcedureParameterList);
                if (!questStatusDef.IsSuccess(status))
                {
                    RollbackTransaction(trans);
                    return (status);
                }
                storedProcedure.ParameterList = storeProcedureParameterList;
            }
            database.StoredProcedureList = storedProcedureList;
            #endregion


            return (new questStatus(Severity.Success));
        }
        #endregion


        #region Private Methods
        /*==================================================================================================================================
         * Private Methods
         *=================================================================================================================================*/
        private questStatus initialize()
        {
            // Initialize
            questStatus status = null;
            try
            {
            }
            catch (System.Exception ex)
            {
                status = new questStatus(Severity.Fatal, String.Format("EXCEPTION: {0}.{1}: {2}",
                        this.GetType().ToString(), MethodInfo.GetCurrentMethod().Name, ex.Message));
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        private questStatus readDatabase(Quest.Functional.MasterPricing.DatabaseId databaseId, out Quest.Functional.MasterPricing.Database database)
        {
            // Initialize 
            questStatus status = null;
            database = null;

            // Read the database.
            DbDatabasesMgr dbDatabasesMgr = new DbDatabasesMgr(this._userSession);
            status = dbDatabasesMgr.Read(databaseId, out database);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        private questStatus closeDatabase(SqlConnection sqlConnection)
        {
            // Initialize 
            questStatus status = null;

            try
            {
                sqlConnection.Close();
            }
            catch (System.Exception ex)
            {
                status = new questStatus(Severity.Fatal, String.Format("EXCEPTION: {0}.{1}: {2}",
                        this.GetType().ToString(), MethodInfo.GetCurrentMethod().Name, ex.Message));
                return (status);
            }
            return (new questStatus(Severity.Success));
        }


        #region SQL Server Metadata
        //
        // SQL Server Metadata
        //
        private questStatus getDatabaseTables(Quest.Functional.MasterPricing.DatabaseId datbaseId, out List<DBTable> dbTableList)
        {
            // Initialize
            questStatus status = null;
            dbTableList = null;
            SqlConnection sqlConnection = null;

            try {

                // Open database
                status = OpenDatabase(datbaseId, out sqlConnection);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }

                // Get tables
                status = getDatabaseTables(sqlConnection, out dbTableList);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }
            }
            catch (System.Exception ex)
            {
                status = new questStatus(Severity.Fatal, String.Format("EXCEPTION: {0}.{1}: {2}",
                        this.GetType().ToString(), MethodInfo.GetCurrentMethod().Name, ex.Message));
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        private questStatus getDatabaseViews(Quest.Functional.MasterPricing.DatabaseId datbaseId, out List<DBView> dbViewList)
        {
            // Initialize
            questStatus status = null;
            dbViewList = null;
            Quest.Functional.MasterPricing.Database database = null;
            SqlConnection sqlConnection = null;

            try
            {

                // Read database
                status = readDatabase(datbaseId, out database);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }

                // Open database
                status = OpenDatabase(datbaseId, out sqlConnection);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }

                // Get views
                status = getDatabaseViews(sqlConnection, out dbViewList);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }
            }
            catch (System.Exception ex)
            {
                status = new questStatus(Severity.Fatal, String.Format("EXCEPTION: {0}.{1}: {2}",
                        this.GetType().ToString(), MethodInfo.GetCurrentMethod().Name, ex.Message));
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        private questStatus getDatabaseTables(SqlConnection sqlConnection, out List<DBTable> dbTableList)
        {
            // Initialize
            dbTableList = null;


            // Get tables
            using (SqlCommand cmd = sqlConnection.CreateCommand())
            {
                cmd.CommandText = "SELECT S.[NAME] AS [SCHEMA], T.[NAME] AS [NAME] " +
                                  "    FROM SYS.TABLES T " +
                                  "    INNER JOIN SYS.schemas S ON T.schema_id = S.schema_id " +
                                  "    ORDER BY S.[NAME], T.[NAME];";

                cmd.CommandType = CommandType.Text;
                using (SqlDataReader rdr = cmd.ExecuteReader())
                {
                    dbTableList = new List<DBTable>();
                    while (rdr.Read())
                    {
                        DBTable dbTable = new DBTable();
                        dbTable.Schema = rdr["SCHEMA"].ToString();
                        dbTable.Name = rdr["NAME"].ToString();
                        dbTableList.Add(dbTable);
                    }
                }
            }
            return (new questStatus(Severity.Success));
        }
        private questStatus getDatabaseViews(SqlConnection sqlConnection, out List<DBView> dbViewList)
        {
            // Initialize
            dbViewList = null;


            // Get tables
            using (SqlCommand cmd = sqlConnection.CreateCommand())
            {
                cmd.CommandText = "SELECT S.[NAME] AS [SCHEMA], V.[NAME] AS [NAME] " +
                                  "    FROM SYS.VIEWS V " +
                                  "    INNER JOIN SYS.schemas S ON V.schema_id = S.schema_id " +
                                  "    WHERE V.[NAME] LIKE 'Quest%' " +                         // <=== TEMPORARY: MAKE CONFIGURABLE.
                                  "    ORDER BY S.[NAME], V.[NAME];";

                cmd.CommandType = CommandType.Text;
                using (SqlDataReader rdr = cmd.ExecuteReader())
                {
                    dbViewList = new List<DBView>();
                    while (rdr.Read())
                    {
                        DBView dbView = new DBView();
                        dbView.Schema = rdr["SCHEMA"].ToString();
                        dbView.Name = rdr["NAME"].ToString();
                        dbViewList.Add(dbView);
                    }
                }
            }
            return (new questStatus(Severity.Success));
        }
        private questStatus getDatabaseColumns(Quest.Functional.MasterPricing.DatabaseId databaseId, out Dictionary<DBTable, List<Column>> dictDBColumns)
        {
            // Initialize
            questStatus status = null;
            dictDBColumns = null;
            List<DBTable> dbTableList = null;
            SqlConnection sqlConnection = null;


            try
            {
                // Open database
                status = OpenDatabase(databaseId, out sqlConnection);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }

                // Get tables.
                status = getDatabaseTables(databaseId, out dbTableList);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }
                dictDBColumns = new Dictionary<DBTable, List<Column>>();


                // Get column information.
                foreach (DBTable dbTable in dbTableList)
                {
                    using (SqlCommand sqlCommand = sqlConnection.CreateCommand())
                    {
                        sqlCommand.CommandText = String.Format("SELECT TOP 1 * FROM [{0}].[{1}]; ", dbTable.Schema, dbTable.Name);
                        sqlCommand.CommandType = CommandType.Text;
                        using (SqlDataReader rdr = sqlCommand.ExecuteReader())
                        {
                            DataTable schemaTable = rdr.GetSchemaTable();
                            List<Column> columnList = new List<Column>();
                            foreach (DataRow myField in schemaTable.Rows)
                            {
                                Column column = new Column();
                                column.Name = myField["ColumnName"].ToString();
                                column.DisplayOrder = Convert.ToInt32(myField["ColumnOrdinal"]);
                                column.ColumnSize = Convert.ToInt32(myField["ColumnSize"]);
                                column.DataType = myField["DataType"].ToString();
                                column.DataTypeName = myField["DataTypeName"].ToString();
                                column.bIsIdentity = Convert.ToBoolean(myField["IsIdentity"]);
                                column.bAllowDbNull = Convert.ToBoolean(myField["AllowDbNull"]);
                                columnList.Add(column);
                            }
                            dictDBColumns.Add(dbTable, columnList);
                        }
                    }
                }
            }
            catch (System.Exception ex)
            {
                status = new questStatus(Severity.Fatal, String.Format("EXCEPTION: {0}.{1}: {2}",
                        this.GetType().ToString(), MethodInfo.GetCurrentMethod().Name, ex.Message));
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        private questStatus getDatabaseColumns(Quest.Functional.MasterPricing.DatabaseId databaseId, out Dictionary<DBView, List<Column>> dictDBColumns)
        {
            // Initialize
            questStatus status = null;
            dictDBColumns = null;
            List<DBView> dbViewList = null;
            SqlConnection sqlConnection = null;
            DBView dbView = null;


            try
            {
                // Open database
                status = OpenDatabase(databaseId, out sqlConnection);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }

                // Get views.
                status = getDatabaseViews(databaseId, out dbViewList);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }
                dictDBColumns = new Dictionary<DBView, List<Column>>();


                // Get column information.
                for (int vidx = 0; vidx < dbViewList.Count; vidx += 1)
                {
                    dbView = dbViewList[vidx];
                    using (SqlCommand sqlCommand = sqlConnection.CreateCommand())
                    {
                        sqlCommand.CommandText = String.Format("SELECT TOP 1 * FROM [{0}].[{1}] (NOLOCK) ", dbView.Schema, dbView.Name);
                        sqlCommand.CommandType = CommandType.Text;
                        using (SqlDataReader rdr = sqlCommand.ExecuteReader())
                        {
                            DataTable schemaTable = rdr.GetSchemaTable();
                            List<Column> columnList = new List<Column>();
                            foreach (DataRow myField in schemaTable.Rows)
                            {
                                Column column = new Column();
                                column.Name = myField["ColumnName"].ToString();
                                column.DisplayOrder = Convert.ToInt32(myField["ColumnOrdinal"]);
                                column.ColumnSize = Convert.ToInt32(myField["ColumnSize"]);
                                column.DataType = myField["DataType"].ToString();
                                column.DataTypeName = myField["DataTypeName"].ToString();
                                column.bIsIdentity = Convert.ToBoolean(myField["IsIdentity"]);
                                column.bAllowDbNull = Convert.ToBoolean(myField["AllowDbNull"]);
                                columnList.Add(column);
                            }
                            dictDBColumns.Add(dbView, columnList);
                        }
                    }
                }
            }
            catch (System.Exception ex)
            {
                status = new questStatus(Severity.Fatal, String.Format("EXCEPTION: loading view [{0}].[{1}] columns {2}.{3}: {4}",
                        dbView.Schema, dbView.Name, this.GetType().ToString(), MethodInfo.GetCurrentMethod().Name, ex.Message));
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        #endregion

        #endregion
    }
}
