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
using Quest.Functional.MasterPricing;
using Quest.MPDW.Services.Data;
using Quest.Services.Dbio.MasterPricing;
using Quest.MasterPricing.Services.Data.Filters;


namespace Quest.MasterPricing.Services.Data.Database
{
    public class DbTablesetMgr : DbMgr
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
        public DbTablesetMgr()
            : base()
        {
            initialize();
        }
        public DbTablesetMgr(UserSession userSession)
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
        #endregion


        #region Public Methods
        /*==================================================================================================================================
         * Public Methods
         *=================================================================================================================================*/
        public questStatus List(QueryOptions queryOptions, out List<Quest.Functional.MasterPricing.Tableset> tablesetList, out QueryResponse queryResponse)
        {
            // Initialize
            questStatus status = null;
            tablesetList = null;
            queryResponse = null;

            string assemblyName = Assembly.GetCallingAssembly().FullName;

            // Get tablesets
            using (MasterPricingEntities dbContext = new MasterPricingEntities())
            {
                using (DbContextTransaction tx = dbContext.Database.BeginTransaction(IsolationLevel.ReadUncommitted))
                {
                    try
                    {
                        PropertyInfo[] dbProperties = typeof(Quest.Services.Dbio.MasterPricing.Tablesets).GetProperties().ToArray();
                        int totalRecords = dbContext.Tablesets.Where(BuildWhereClause(queryOptions, dbProperties)).Count();
                        List<Quest.Services.Dbio.MasterPricing.Tablesets> _tablesetList = dbContext.Tablesets.Where(BuildWhereClause(queryOptions, dbProperties))
                                .OrderBy(BuildSortString(queryOptions.SortColumns))
                                .Skip(queryOptions.Paging.PageSize * (queryOptions.Paging.PageNumber - 1))
                                .Take(queryOptions.Paging.PageSize).ToList();
                        if (_tablesetList == null)
                        {
                            return (new questStatus(Severity.Warning));
                        }
                        tablesetList = new List<Tableset>();
                        foreach (Quest.Services.Dbio.MasterPricing.Tablesets _instantMessageContact in _tablesetList)
                        {
                            Tableset tableset = new Tableset();
                            BufferMgr.TransferBuffer(_instantMessageContact, tableset);
                            tablesetList.Add(tableset);
                        }
                        status = BuildQueryResponse(totalRecords, queryOptions, out queryResponse);
                        if (!questStatusDef.IsSuccess(status))
                        {
                            return (status);
                        }
                    }
                    catch (System.Exception ex)
                    {
                        return (new questStatus(Severity.Fatal, String.Format("EXCEPTION: {0}.{1}: {2}",
                                this.GetType().Name, MethodBase.GetCurrentMethod().Name,
                                ex.InnerException != null ? ex.InnerException.Message : ex.Message)));
                    }
                }
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus ReadTablesetConfiguration(TablesetId tablesetId, out TablesetConfiguration tablesetConfiguration)
        {
            // Initialize
            questStatus status = null;
            tablesetConfiguration = null;


            // Read tableset
            Tableset tableset = null;
            DbTablesetsMgr dbTablesetsMgr = new DbTablesetsMgr(this._userSession);
            status = dbTablesetsMgr.Read(tablesetId, out tableset);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }


            // Read database
            DatabaseId databaseId = new DatabaseId(tableset.DatabaseId);
            Quest.Functional.MasterPricing.Database database = null;
            DbDatabasesMgr dbDatabasesMgr = new DbDatabasesMgr(this._userSession);
            status = dbDatabasesMgr.Read(databaseId, out database);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }



            /*
             * Load tableset entities.
             */
            DbTablesetColumnsMgr dbTablesetColumnsMgr = new DbTablesetColumnsMgr(this._userSession);


            // Read tables in tableset
            List<Quest.Functional.MasterPricing.TablesetTable> tablesetTableList = null;
            DbTablesetTablesMgr dbTablesetTablesMgr = new DbTablesetTablesMgr(this._userSession);
            status = dbTablesetTablesMgr.Read(tablesetId, out tablesetTableList);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }

            // Read table info.
            EntityType entityType = new EntityType();
            entityType.Id = EntityType.Table;
            DbTablesMgr dbTablesMgr = new DbTablesMgr(this._userSession);
            DbColumnsMgr dbColumnsMgr = new DbColumnsMgr(this._userSession);
            foreach (TablesetTable tablesetTable in tablesetTableList)
            {
                TablesetId tableSetId = new TablesetId(tablesetTable.Id);
                List<Quest.Functional.MasterPricing.TablesetColumn> tablesetColumnList = null;

                TableSetEntityId tableSetEntityId = new TableSetEntityId(tablesetTable.Id);
                status = dbTablesetColumnsMgr.Read(entityType, tableSetEntityId, out tablesetColumnList);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }
                tablesetTable.TablesetColumnList = tablesetColumnList;

                Table table = null;
                status = dbTablesMgr.Read(databaseId, tablesetTable.Schema, tablesetTable.Name, out table);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }
                tablesetTable.Table = table;


                // Get column metadata
                EntityTypeId entityTypeId = new EntityTypeId(EntityType.Table);
                EntityId entityId = new EntityId(table.Id);
                foreach (TablesetColumn tablesetColumn in tablesetColumnList)
                {
                    Column column = null;
                    status = dbColumnsMgr.Read(entityTypeId, entityId, tablesetColumn.Name, out column);
                    if (!questStatusDef.IsSuccess(status))
                    {
                        return (status);
                    }
                    tablesetColumn.Column = column;
                }
            }

            // Read views in tableset
            List<Quest.Functional.MasterPricing.TablesetView> tablesetViewList = null;
            DbTablesetViewsMgr dbTablesetViewsMgr = new DbTablesetViewsMgr(this._userSession);
            status = dbTablesetViewsMgr.Read(tablesetId, out tablesetViewList);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            // Read views columns in tableset
            entityType.Id = EntityType.View;
            DbViewsMgr dbViewsMgr = new DbViewsMgr(this._userSession);
            foreach (TablesetView tablesetView in tablesetViewList)
            {
                TablesetViewId tablesetViewId = new TablesetViewId(tablesetView.Id);
                List<Quest.Functional.MasterPricing.TablesetColumn> tablesetColumnList = null;

                TableSetEntityId tableSetEntityId = new TableSetEntityId(tablesetView.Id);
                status = dbTablesetColumnsMgr.Read(entityType, tableSetEntityId, out tablesetColumnList);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }
                tablesetView.TablesetColumnList = tablesetColumnList;

                View view = null;
                status = dbViewsMgr.Read(databaseId, tablesetView.Schema, tablesetView.Name, out view);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }
                tablesetView.View = view;


                // Get column metadata
                EntityTypeId entityTypeId = new EntityTypeId(EntityType.View);
                EntityId entityId = new EntityId(view.Id);
                foreach (TablesetColumn tablesetColumn in tablesetColumnList)
                {
                    Column column = null;
                    status = dbColumnsMgr.Read(entityTypeId, entityId, tablesetColumn.Name, out column);
                    if (!questStatusDef.IsSuccess(status))
                    {
                        return (status);
                    }
                    tablesetColumn.Column = column;
                }
            }

            // Build tableset configuration
            tablesetConfiguration = new TablesetConfiguration();
            tablesetConfiguration.Tableset = tableset;
            tablesetConfiguration.Database = database;
            tablesetConfiguration.TablesetTables = tablesetTableList;
            tablesetConfiguration.TablesetViews = tablesetViewList;


            return (new questStatus(Severity.Success));
        }
        public questStatus ReadTablesetDataManagement(TablesetId tablesetId, out TablesetDataManagement tablesetDataManagement)
        {
            // Initialize
            questStatus status = null;
            tablesetDataManagement = null;


            // Read tableset configuration
            TablesetConfiguration tablesetConfiguration = null;
            status = ReadTablesetConfiguration(tablesetId, out tablesetConfiguration);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }


            // Build tableset data management.
            tablesetDataManagement = new TablesetDataManagement();
            tablesetDataManagement.TablesetConfiguration = tablesetConfiguration;


            // TODO: REFACTOR SOME STUFF. FOCUSING ON UI RIGHT NOW, NO TIME FOR IT HERE.
            // Read tableset filters
            List<Filter> filterList = null;
            DbFiltersMgr dbFiltersMgr = new DbFiltersMgr(this._userSession);
            status = dbFiltersMgr.Read(tablesetId, out filterList);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            tablesetDataManagement.FilterList = filterList;


            return (new questStatus(Severity.Success));
        }






        public questStatus GetTablesetTable(DatabaseId databaseId, FilterTableTablesetTableId filterTableTablesetTableId, out TablesetTable tablesetTable)
        {
            // Initialize
            questStatus status = null;
            tablesetTable = null;


            // Get tableset table
            DbTablesetTablesMgr dbTablesetTablesMgr = new DbTablesetTablesMgr(this._userSession);
            status = dbTablesetTablesMgr.Read(filterTableTablesetTableId, out tablesetTable);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }

            // Get tableset columns
            EntityType entityType = new EntityType();
            entityType.Id = EntityType.Table;
            TableSetEntityId tableSetEntityId = new TableSetEntityId(tablesetTable.Id);
            DbTablesetColumnsMgr dbTablesetColumnsMgr = new DbTablesetColumnsMgr(this._userSession);
            List<TablesetColumn> tablesetColumnList = null;

            TablesetColumnId tablesetColumnId = new TablesetColumnId();
            status = dbTablesetColumnsMgr.Read(entityType, tableSetEntityId, out tablesetColumnList);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            tablesetTable.TablesetColumnList = tablesetColumnList;


            // Get table
            Table table = null;
            status = GetTableInfo(databaseId, tablesetTable, out table);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            tablesetTable.Table = table;


            // Get column info
            DbColumnsMgr dbColumnsMgr = new DbColumnsMgr(this._userSession);
            EntityTypeId entityTypeId = new EntityTypeId(EntityType.Table);
            EntityId entityId = new EntityId(table.Id);
            foreach (TablesetColumn tablesetColumn in tablesetColumnList)
            {
                Column column = null;
                status = dbColumnsMgr.Read(entityTypeId, entityId, tablesetColumn.Name, out column);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }
                tablesetColumn.Column = column;
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus GetTableInfo(DatabaseId databaseId, TablesetTable tablesetTable, out Table table)
        {
            // Initialize
            questStatus status = null;
            table = null;


            // Get table
            DbTablesMgr dbTablesMgr = new DbTablesMgr(this._userSession);
            status = dbTablesMgr.Read(databaseId, tablesetTable.Schema, tablesetTable.Name, out table);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            tablesetTable.Table = table;


            // Get table columns
            TableId tableId = new TableId(table.Id);
            List<Column> columnList = null;
            DbColumnsMgr dbColumnsMgr = new DbColumnsMgr(this._userSession);
            status = dbColumnsMgr.Read(tableId, out columnList);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            table.ColumnList = columnList;

            return (new questStatus(Severity.Success));
        }
        public questStatus GetTablesetView(DatabaseId databaseId, FilterViewTablesetViewId filterViewTablesetViewId, out TablesetView tablesetView)
        {
            // Initialize
            questStatus status = null;
            tablesetView = null;


            // Get tableset view
            DbTablesetViewsMgr dbTablesetViewsMgr = new DbTablesetViewsMgr(this._userSession);
            status = dbTablesetViewsMgr.Read(filterViewTablesetViewId, out tablesetView);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }

            // Get tableset columns
            EntityType entityType = new EntityType();
            entityType.Id = EntityType.View;
            TableSetEntityId tableSetEntityId = new TableSetEntityId(tablesetView.Id);
            DbTablesetColumnsMgr dbTablesetColumnsMgr = new DbTablesetColumnsMgr(this._userSession);
            List<TablesetColumn> tablesetColumnList = null;

            TablesetColumnId tablesetColumnId = new TablesetColumnId();
            status = dbTablesetColumnsMgr.Read(entityType, tableSetEntityId, out tablesetColumnList);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            tablesetView.TablesetColumnList = tablesetColumnList;


            // Get view
            View view = null;
            status = GetViewInfo(databaseId, tablesetView, out view);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            tablesetView.View = view;


            // Get column info
            DbColumnsMgr dbColumnsMgr = new DbColumnsMgr(this._userSession);
            EntityTypeId entityTypeId = new EntityTypeId(EntityType.View);
            EntityId entityId = new EntityId(view.Id);
            foreach (TablesetColumn tablesetColumn in tablesetColumnList)
            {
                Column column = null;
                status = dbColumnsMgr.Read(entityTypeId, entityId, tablesetColumn.Name, out column);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }
                tablesetColumn.Column = column;
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus GetViewInfo(DatabaseId databaseId, TablesetView tablesetView, out View view)
        {
            // Initialize
            questStatus status = null;
            view = null;


            // Get view
            DbViewsMgr dbViewsMgr = new DbViewsMgr(this._userSession);
            status = dbViewsMgr.Read(databaseId, tablesetView.Schema, tablesetView.Name, out view);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            tablesetView.View = view;


            // Get view columns
            ViewId viewId = new ViewId(view.Id);
            List<Column> columnList = null;
            DbColumnsMgr dbColumnsMgr = new DbColumnsMgr(this._userSession);
            status = dbColumnsMgr.Read(viewId, out columnList);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            view.ColumnList = columnList;

            return (new questStatus(Severity.Success));
        }


        /* Used or useful? */
        public questStatus GetColumnInfo(TablesetColumn tablesetColumn, out Column column)
        {
            // Initialize
            questStatus status = null;
            column = null;


            DbTablesetTablesMgr dbTablesetTablesMgr = new DbTablesetTablesMgr(this._userSession);
            DbTablesetViewsMgr dbTablesetViewsMgr = new DbTablesetViewsMgr(this._userSession);
            TablesetId tablesetId = null;
            TablesetTable tablesetTable = null;
            TablesetView tablesetView = null;
            if (tablesetColumn.EntityTypeId == EntityType.Table)
            {
                // Get TablesetTable
                TablesetTableId tablesetTableId = new TablesetTableId(tablesetColumn.TableSetEntityId);
                status = dbTablesetTablesMgr.Read(tablesetTableId, out tablesetTable);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }
                tablesetId = new TablesetId(tablesetTable.TablesetId);
            }
            else if (tablesetColumn.EntityTypeId == EntityType.View)
            {
                // Get TablesetView
                TablesetViewId tablesetViewId = new TablesetViewId(tablesetColumn.TableSetEntityId);
                status = dbTablesetViewsMgr.Read(tablesetViewId, out tablesetView);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }
                tablesetId = new TablesetId(tablesetView.TablesetId);
            }
            else
            {
                return (new questStatus(Severity.Error, String.Format("ERROR: unsupported EntityTypeId {0} for TablesetColumn {1}",
                    tablesetColumn.EntityTypeId, tablesetColumn.Id)));
            }

            // Get the Tableset
            DbTablesetsMgr dbTablesetsMgr = new DbTablesetsMgr(this._userSession);
            Tableset tableset = null;
            status = dbTablesetsMgr.Read(tablesetId, out tableset);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }

            // Get the database
            DatabaseId databaseId = new DatabaseId(tableset.DatabaseId);
            Quest.Functional.MasterPricing.Database database = null;
            DbDatabasesMgr dbDatabasesMgr = new DbDatabasesMgr(this._userSession);
            status = dbDatabasesMgr.Read(databaseId, out database);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }


            // Get the Table or View by identifier within this database
            DbTablesMgr dbTablesMgr = new DbTablesMgr(this._userSession);
            DbViewsMgr dbViewsMgr = new DbViewsMgr(this._userSession);
            EntityTypeId entityTypeId = null;
            EntityId entityId = null;
            if (tablesetColumn.EntityTypeId == EntityType.Table)
            {
                TableId tableId = new TableId();
                Table table = null;
                status = dbTablesMgr.Read(databaseId, tablesetTable.Schema, tablesetTable.Name, out table);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }
                entityTypeId = new EntityTypeId(EntityType.Table);
                entityId = new EntityId(table.Id);
            }
            else if (tablesetColumn.EntityTypeId == EntityType.View)
            {
            }

            // Get column info
            DbColumnsMgr dbColumnsMgr = new DbColumnsMgr(this._userSession);
            status = dbColumnsMgr.Read(entityTypeId, entityId, tablesetColumn.Name, out column);
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
        private questStatus readDatabase(DatabaseId databaseId, out Quest.Functional.MasterPricing.Database database)
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
        private questStatus getDatabaseTables(DatabaseId datbaseId, out List<DBTable> dbTableList)
        {
            // Initialize
            questStatus status = null;
            dbTableList = null;
            Quest.Functional.MasterPricing.Database database = null;
            SqlConnection sqlConnection = null;

            try {

                // Read database
                status = readDatabase(datbaseId, out database);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }

                // Open database
                status = OpenDatabase(database, out sqlConnection);
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
        private questStatus getDatabaseViews(DatabaseId datbaseId, out List<DBView> dbViewList)
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
                status = OpenDatabase(database, out sqlConnection);
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
        private questStatus getDatabaseColumns(DatabaseId databaseId, out Dictionary<DBTable, List<Column>> dictDBColumns)
        {
            // Initialize
            questStatus status = null;
            dictDBColumns = null;
            List<DBTable> dbTableList = null;
            SqlConnection sqlConnection = null;


            try
            {
                // Get database
                Quest.Functional.MasterPricing.Database database = null;
                DbDatabasesMgr dbDatabasesMgr = new DbDatabasesMgr(this._userSession);
                status = dbDatabasesMgr.Read(databaseId, out database);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }

                // Open database
                status = OpenDatabase(database, out sqlConnection);
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
        private questStatus getDatabaseColumns(DatabaseId databaseId, out Dictionary<DBView, List<Column>> dictDBColumns)
        {
            // Initialize
            questStatus status = null;
            dictDBColumns = null;
            List<DBView> dbViewList = null;
            SqlConnection sqlConnection = null;
            DBView dbView = null;


            try
            {
                // Get database
                Quest.Functional.MasterPricing.Database database = null;
                DbDatabasesMgr dbDatabasesMgr = new DbDatabasesMgr(this._userSession);
                status = dbDatabasesMgr.Read(databaseId, out database);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }

                // Open database
                status = OpenDatabase(database, out sqlConnection);
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
                for (int vidx=0; vidx < dbViewList.Count; vidx += 1)
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
        private questStatus readTable(TableId tableId, out Quest.Functional.MasterPricing.Table table)
        {
            // Initialize 
            questStatus status = null;
            table = null;

            // Read the table.
            DbTablesMgr dbTablesMgr = new DbTablesMgr(this._userSession);
            status = dbTablesMgr.Read(tableId, out table);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        #endregion
    }
}
