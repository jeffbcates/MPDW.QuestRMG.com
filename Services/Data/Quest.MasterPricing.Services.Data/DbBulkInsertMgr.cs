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
using Quest.MasterPricing.Services.Data.Filters;
using Quest.MasterPricing.Services.Data.Database;
using Quest.Functional.Logging;
using Quest.Services.Data.Logging;


namespace Quest.MasterPricing.Services.Data.Bulk
{
    // TODO: REFACTOR USING DbLogsMgr

    public class DbBulkInsertMgr : DbSQLMgr
    {
        #region Declarations
        /*==================================================================================================================================
         * Declarations
         *=================================================================================================================================*/
        private UserSession _userSession = null;
        private LogSetting _logSetting = null;
        private DbBulkInsertLogsMgr _dbBulkInsertLogsMgr = null;

        #endregion


        #region Constructors
        /*==================================================================================================================================
         * Constructors
         *=================================================================================================================================*/
        public DbBulkInsertMgr(UserSession userSession)
            : base(userSession)
        {
            _userSession = userSession;
            initialize();
        }
        #endregion


        #region Properties
        /*==================================================================================================================================
         * Properties
         *=================================================================================================================================*/
        public bool bLogging
        {
            get
            {
                return (this._logSetting.bLogBulkInserts);
            }
        }
        public bool bLoggingPerRow
        {
            get
            {
                return (this._logSetting.bLogBulkInsertsPerRow);
            }
        }
        #endregion


        #region Public Methods
        /*==================================================================================================================================
         * Public Methods
         *=================================================================================================================================*/

        #region Filter Procedure Utility Routines
        /*----------------------------------------------------------------------------------------------------------------------------------
         * Filter Procedure Utility Routines
         *---------------------------------------------------------------------------------------------------------------------------------*/
        public questStatus GetFilterProcedure(BulkInsertRequest bulkInsertRequest, string Action, out FilterProcedure filterProcedure)
        {
            // Initialize
            questStatus status = null;
            filterProcedure = null;


            // Get the filter procedures.
            FilterId filterId = new FilterId(bulkInsertRequest.FilterId);
            List<FilterProcedure> filterProcedureList = null;
            DbFilterProceduresMgr dbFilterProceduresMgr = new DbFilterProceduresMgr(this.UserSession);
            status = dbFilterProceduresMgr.Read(filterId, out filterProcedureList);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }

            // Determine if given action exists.
            FilterProcedure INSERTsproc = filterProcedureList.Find(delegate (FilterProcedure fp) { return (fp.Action == Action); });
            if (INSERTsproc == null)
            {
                return (new questStatus(Severity.Warning, String.Format("No {0} filter procedure", Action)));
            }

            // Get parameters
            FilterProcedureId filterProcedureId = new FilterProcedureId(INSERTsproc.Id);
            List<FilterProcedureParameter> filterProcedureParameterList = null;
            DbFilterProcedureParametersMgr dbFilterProcedureParametersMgr = new DbFilterProcedureParametersMgr(this.UserSession);
            status = dbFilterProcedureParametersMgr.Read(filterProcedureId, out filterProcedureParameterList);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }

            // Return filter procedure with parameters.
            INSERTsproc.ParameterList = filterProcedureParameterList;
            filterProcedure = INSERTsproc;

            return (new questStatus(Severity.Success));
        }
        public questStatus PerformBulkInsertFilterProcedure(BulkInsertRequest bulkInsertRequest, FilterProcedure filterProcedure)
        {
            // Initialize
            questStatus status = null;
            BulkInsertLog bulkInsertLog = bLogging ? new BulkInsertLog() : null;
            BulkInsertLogId bulkInsertLogId = null;
            int numRows = 0;
            string logMessage = null;
            List<string> logParameterList = null;


            try
            {
                // Initialize log
                if (bLogging)
                {
                    bulkInsertLog.Event = "Initialize";
                    bulkInsertLog.UserSessionId = this.UserSession.Id;
                    bulkInsertLog.Username = this.UserSession.User.Username;
                    bulkInsertLog.Batch = Guid.NewGuid().ToString();
                    string Filter = null;
                    status = _dbBulkInsertLogsMgr.SetFilter(bulkInsertRequest.Filter, out Filter);
                    if (!questStatusDef.IsSuccess(status))
                    {
                        return (status);
                    }
                    bulkInsertLog.Filter = Filter;
                }

                // Get database connection string
                TablesetId tablesetId = new TablesetId(bulkInsertRequest.TablesetId);
                Tableset tableset = null;
                DbTablesetsMgr dbTablesetsMgr = new DbTablesetsMgr(this.UserSession);
                status = dbTablesetsMgr.Read(tablesetId, out tableset);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }
                if (bLogging)
                {
                    string Tableset = null;
                    status = _dbBulkInsertLogsMgr.SetTableset(tableset, out Tableset);
                    if (!questStatusDef.IsSuccess(status))
                    {
                        return (status);
                    }
                    bulkInsertLog.Tableset = Tableset;
                }

                DatabaseId databaseId = new DatabaseId(tableset.DatabaseId);
                Quest.Functional.MasterPricing.Database database = null;
                DbDatabasesMgr dbDatabasesMgr = new DbDatabasesMgr(this.UserSession);
                status = dbDatabasesMgr.Read(databaseId, out database);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }
                if (bLogging)
                {
                    string Database = null;
                    status = _dbBulkInsertLogsMgr.SetDatabase(database, out Database);
                    if (!questStatusDef.IsSuccess(status))
                    {
                        return (status);
                    }
                    bulkInsertLog.Database = Database;
                }

                // Connect and execute
                bool bTransaction = false;
                bulkInsertLog.Event = "Open";
                using (SqlConnection conn = new SqlConnection(database.ConnectionString))
                {
                    bulkInsertLog.Event = "Connect";
                    conn.Open();
                    SqlTransaction trans = null;
                    if (bTransaction)
                    {
                        trans = conn.BeginTransaction();
                        if (bLogging)
                        {
                            bulkInsertLog.Event = "BeginTransaction";
                        }
                    }
                    numRows = 0;
                    foreach (BulkInsertRow bulkInsertRow in bulkInsertRequest.Rows)
                    {
                        using (SqlCommand cmd = new SqlCommand(null, conn, trans))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.CommandText = filterProcedure.Name;

                            // Initialize logging info
                            if (bLogging)
                            {
                                logParameterList = new List<string>();
                                bulkInsertLog.Event = null;
                                bulkInsertLog.NumRows = numRows;
                                bulkInsertLog.Parameters = null;
                                bulkInsertLog.Message = null;
                                bulkInsertLog.Data = null;
                            }

                            // Build each parameter
                            List<string> dataValueList = new List<string>();
                            foreach (FilterProcedureParameter filterParam in filterProcedure.ParameterList)
                            {
                                if (bLogging)
                                {
                                    bulkInsertLog.Event = "Next Parameter: " + filterParam.ParameterName;
                                }

                                if (filterParam.Direction != "Input")
                                {
                                    continue;
                                }
                                if (bLogging)
                                {
                                    logParameterList.Add(String.Format(" Id: {0}, ParameterName: {1}, SqlDbType: {2} ", filterParam.Id, filterParam.ParameterName, filterParam.SqlDbType.ToString()));
                                    string parameterArray = null;
                                    _dbBulkInsertLogsMgr.SetArray(logParameterList, out parameterArray);
                                    bulkInsertLog.Parameters = parameterArray;
                                }

                                // If a meta-parameter, fill in its value and continue.
                                bool bIsMetaParameter = false;
                                SqlParameter sqlMetaParameter = null;
                                if (filterParam.ParameterName.Equals("@_Username", StringComparison.InvariantCultureIgnoreCase))
                                {
                                    bIsMetaParameter = true;
                                    sqlMetaParameter = new SqlParameter(filterParam.ParameterName, SqlDbType.NVarChar);
                                    sqlMetaParameter.Value = this.UserSession.User.Username;
                                }
                                else if (filterParam.ParameterName.Equals("@_UserSessionId", StringComparison.InvariantCultureIgnoreCase))
                                {
                                    bIsMetaParameter = true;
                                    sqlMetaParameter = new SqlParameter(filterParam.ParameterName, SqlDbType.Int);
                                    sqlMetaParameter.Value = this.UserSession.Id;
                                }
                                else if (filterParam.ParameterName.StartsWith("@_", StringComparison.InvariantCultureIgnoreCase))
                                {
                                    logMessage = String.Format("ERROR: unknown meta-parameter: {0}", filterParam.ParameterName);
                                    if (bLogging)
                                    {
                                        bulkInsertLog.Message = logMessage;
                                        _dbBulkInsertLogsMgr.Create(bulkInsertLog, out bulkInsertLogId);
                                    }
                                    return (new questStatus(Severity.Error, logMessage));
                                }
                                if (bIsMetaParameter)
                                {
                                    cmd.Parameters.Add(sqlMetaParameter);
                                    continue;
                                }


                                // Not a meta-parameter, so the bulk insert value should be provided.
                                BulkInsertColumnValue bulkInsertColumnValue = bulkInsertRow.Columns.Find(delegate (BulkInsertColumnValue cv) {
                                        return (cv.Name == filterParam.ParameterName); });
                                if (bulkInsertColumnValue == null)
                                {
                                    if (bTransaction)
                                    {
                                        trans.Rollback();
                                    }

                                    logMessage = String.Format("ERROR: sproc parameter {0} not found in bulk insert columns",
                                            filterParam.ParameterName);
                                    if (bLogging)
                                    {
                                        bulkInsertLog.Message = logMessage;
                                        _dbBulkInsertLogsMgr.Create(bulkInsertLog, out bulkInsertLogId);
                                    }
                                    return (new questStatus(Severity.Error, logMessage));
                                }
                                if (bLogging)
                                {
                                    bulkInsertLog.BulkInsertColumn = String.Format(" Name: {0}, Value: {1} ", bulkInsertColumnValue.Name, bulkInsertColumnValue.Value);
                                }



                                // Bind the parameter
                                // TODO:REFACTOR
                                SqlDbType sqlDbType = (SqlDbType)Enum.Parse(typeof(SqlDbType), filterParam.SqlDbType, true);
                                SqlParameter sqlParameter = new SqlParameter(filterParam.ParameterName, sqlDbType);


                                if (bLogging)
                                {
                                    bulkInsertLog.Event = "Set Parameter Value";
                                }

                                if (sqlDbType == SqlDbType.Bit)
                                {
                                    bool bValue = bulkInsertColumnValue.Value != "0";
                                    sqlParameter.Value = bValue;
                                }
                                else if (sqlDbType == SqlDbType.Int)
                                {
                                    int intValue = Convert.ToInt32(bulkInsertColumnValue.Value);
                                    sqlParameter.Value = intValue;
                                }
                                else if (sqlDbType == SqlDbType.NVarChar)
                                {
                                    if (bulkInsertColumnValue.Value == null)
                                    {
                                        sqlParameter.Value = DBNull.Value;
                                    }
                                    else
                                    {
                                        sqlParameter.Value = bulkInsertColumnValue.Value.ToString();
                                    }
                                }
                                else if (sqlDbType == SqlDbType.VarChar)
                                {
                                    if (bulkInsertColumnValue.Value == null)
                                    {
                                        sqlParameter.Value = DBNull.Value;
                                    }
                                    else
                                    {
                                        sqlParameter.Value = bulkInsertColumnValue.Value.ToString();
                                    }
                                }
                                else if (sqlDbType == SqlDbType.DateTime)
                                {
                                    if (bulkInsertColumnValue.Value == null)
                                    {
                                        sqlParameter.Value = DBNull.Value;
                                    }
                                    else
                                    {
                                        sqlParameter.Value = Convert.ToDateTime(bulkInsertColumnValue.Value);
                                    }
                                }
                                else if (sqlDbType == SqlDbType.DateTime2)
                                {
                                    if (bulkInsertColumnValue.Value == null)
                                    {
                                        sqlParameter.Value = DBNull.Value;
                                    }
                                    else
                                    {
                                        sqlParameter.Value = Convert.ToDateTime(bulkInsertColumnValue.Value);
                                    }
                                }
                                else if (sqlDbType == SqlDbType.Date)
                                {
                                    if (bulkInsertColumnValue.Value == null)
                                    {
                                        sqlParameter.Value = DBNull.Value;
                                    }
                                    else
                                    {
                                        sqlParameter.Value = Convert.ToDateTime(bulkInsertColumnValue.Value);
                                    }
                                }
                                else if (sqlDbType == SqlDbType.Decimal)
                                {
                                    if (bulkInsertColumnValue.Value == null)
                                    {
                                        sqlParameter.Value = DBNull.Value;
                                    }
                                    else
                                    {
                                        sqlParameter.Value = Convert.ToDecimal(bulkInsertColumnValue.Value);
                                    }
                                }
                                else
                                {
                                    if (bulkInsertColumnValue.Value == null)
                                    {
                                        sqlParameter.Value = DBNull.Value;
                                    }
                                    else
                                    {
                                        sqlParameter.Value = bulkInsertColumnValue.Value;
                                    }
                                }
                                cmd.Parameters.Add(sqlParameter);        
                                if (bLogging)
                                {
                                    dataValueList.Add(String.Format(" Name: {0}, Value: {1} ", sqlParameter.ParameterName,
                                            sqlParameter.Value == DBNull.Value ? "null" : sqlParameter.Value.ToString()));
                                }
                            }
                            try
                            {
                                if (bLogging)
                                {
                                    bulkInsertLog.Event = "ExecuteNonQuery";
                                }
                                int _numRows = cmd.ExecuteNonQuery();
                                if (_numRows != 1)
                                {
                                    if (bTransaction)
                                    {
                                        trans.Rollback();
                                    }
                                    logMessage = String.Format("ERROR: Bulk insert stored procedure failed: Rows: {0}", _numRows);
                                    if (bLogging)
                                    {
                                        bulkInsertLog.Message = logMessage;
                                        _dbBulkInsertLogsMgr.Create(bulkInsertLog, out bulkInsertLogId);
                                    }
                                    return (new questStatus(Severity.Error, logMessage));
                                }
                                else if (bLoggingPerRow)
                                {
                                    bulkInsertLog.Message = "Successful bulk insert";
                                    bulkInsertLog.NumRows = _numRows;
                                    bulkInsertLog.BulkInsertColumn = null;

                                    string valueArray = null;
                                    _dbBulkInsertLogsMgr.SetArray(dataValueList, out valueArray);
                                    bulkInsertLog.Data = valueArray;

                                    _dbBulkInsertLogsMgr.Create(bulkInsertLog, out bulkInsertLogId);
                                }
                            }
                            catch (SqlException ex)
                            {
                                if (bTransaction)
                                {
                                    trans.Rollback();
                                }
                                logMessage = String.Format("SQL EXCEPTION: Bulk insert stored procedure {1}: {2}",
                                        numRows, filterProcedure.Name, ex.Message);
                                if (bLogging)
                                {
                                    bulkInsertLog.Message = logMessage;
                                    _dbBulkInsertLogsMgr.Create(bulkInsertLog, out bulkInsertLogId);
                                }
                                return (new questStatus(Severity.Error, logMessage));
                            }
                            catch (System.Exception ex)
                            {
                                if (bTransaction)
                                {
                                    trans.Rollback();
                                }
                                logMessage = String.Format("After {0} rows, EXCEPTION: Bulk insert stored procedure {0}: {1}",
                                        numRows, filterProcedure.Name, ex.Message);
                                if (bLogging)
                                {
                                    bulkInsertLog.Message = logMessage;
                                    _dbBulkInsertLogsMgr.Create(bulkInsertLog, out bulkInsertLogId);
                                }
                                return (new questStatus(Severity.Error, logMessage));
                            }
                        }
                        numRows += 1;
                    }
                    if (bTransaction)
                    {
                        trans.Commit();
                    }
                    if (bLogging)
                    {
                        bulkInsertLog.Event = "COMMIT";
                        bulkInsertLog.NumRows = numRows;
                        bulkInsertLog.BulkInsertColumn = null;
                        bulkInsertLog.Message = "Bulk isnert operation successful";
                        bulkInsertLog.Data = null;

                        _dbBulkInsertLogsMgr.Create(bulkInsertLog, out bulkInsertLogId);
                    }
                }
            }
            catch (System.Exception ex)
            {
                logMessage = String.Format("EXCEPTION: Bulk Insert Operation: {0}.{1}: {2}",
                        this.GetType().Name, MethodBase.GetCurrentMethod().Name,
                        ex.InnerException != null ? ex.InnerException.Message : ex.Message);
                if (bLogging)
                {
                    bulkInsertLog.Event = bulkInsertLog.Event == null ? "EXCEPTION" : bulkInsertLog.Event;
                    bulkInsertLog.Message = logMessage;
                    bulkInsertLog.NumRows = numRows;

                    string parameterArray = null;
                    _dbBulkInsertLogsMgr.SetArray(logParameterList, out parameterArray);
                    bulkInsertLog.Parameters = parameterArray;

                    string Exception = null;
                    _dbBulkInsertLogsMgr.SetException(ex, out Exception);
                    bulkInsertLog.Data = Exception;

                    _dbBulkInsertLogsMgr.Create(bulkInsertLog, out bulkInsertLogId);
                }
                return (new questStatus(Severity.Fatal, logMessage));
            }
            return (new questStatus(Severity.Success));
        }
        #endregion


        #region SQL-based
        /*----------------------------------------------------------------------------------------------------------------------------------
         * SQL-based
         *---------------------------------------------------------------------------------------------------------------------------------*/
        public questStatus GenerateBulkInsertSQL(BulkInsertRequest bulkInsertRequest)
        {
            // Initialize
            questStatus status = null;
            Filter filter = null;


            // If filter not included, get it.
            if (bulkInsertRequest.Filter == null)
            {
                FilterId filterId = new FilterId(bulkInsertRequest.FilterId);
                DbFilterMgr dbFilterMgr = new DbFilterMgr(this.UserSession);
                status = dbFilterMgr.GetFilter(filterId, out filter);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }
                bulkInsertRequest.Filter = filter;
            }
            filter = bulkInsertRequest.Filter;


            // TEMPORARY: IF MORE THAN ONE TABLE, NOT SUPPORTED
            int numEntities = 0;
            status = getNumEntities(bulkInsertRequest, out numEntities);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            if (numEntities != 1)
            {
                return (new questStatus(Severity.Error, "Single-table inserts only are supported via SQL generation.  Use INSERT sproc."));
            }

            StringBuilder sbINSERTRows = new StringBuilder();
            for (int rdx=0; rdx < bulkInsertRequest.Rows.Count; rdx += 1)
            {
                BulkInsertRow bulkInsertRow = bulkInsertRequest.Rows[rdx];

                StringBuilder sbINSERTSql = new StringBuilder("INSERT INTO ");
                sbINSERTSql.AppendLine(String.Format(" [{0}].[{1}] ", filter.FilterTableList[0].TablesetTable.Table.Schema, filter.FilterTableList[0].TablesetTable.Table.Name));


                // Build column list
                sbINSERTSql.Append(" ( ");
                for (int cidx=0; cidx < bulkInsertRow.Columns.Count; cidx += 1)
                {
                    BulkInsertColumnValue bulkInsertColumnValue = bulkInsertRow.Columns[cidx];

                    status = getFilterColumn(bulkInsertColumnValue, filter);
                    if (!questStatusDef.IsSuccess(status))
                    {
                        return (new questStatus(String.Format("ERROR: getting FilterColumn on row {0}: {1}", (rdx+1), status.Message)));
                    }
                    if (bulkInsertColumnValue.FilterColumn.TablesetColumn.Column.bIsIdentity)
                    {
                        continue;
                    }
                    if (bulkInsertColumnValue.FilterColumn.TablesetColumn.Column.bIsAutoIncrement)
                    {
                        continue;
                    }
                    if (bulkInsertColumnValue.FilterColumn.TablesetColumn.Column.bIsReadOnly)
                    {
                        continue;
                    }

                    // Append column to INSERT clause
                    if (filter.FilterTableList.Count == 1)
                    {
                        sbINSERTSql.AppendLine(String.Format(" [{0}]", bulkInsertColumnValue.Name));
                    }
                    else
                    {
                        string[] pp = bulkInsertColumnValue.Name.Split('_');
                        sbINSERTSql.AppendLine(String.Format(" [{0}]", pp[1]));
                    }


                    // Comma between columns
                    if (cidx + 1 < bulkInsertRow.Columns.Count)
                    {
                        sbINSERTSql.Append(", ");
                    }
                    else
                    {
                        sbINSERTSql.Append(" ");
                    }
                }
                sbINSERTSql.Append(" ) ");


                // Build value list
                sbINSERTSql.Append(" VALUES ( ");
                for (int cidx = 0; cidx < bulkInsertRow.Columns.Count; cidx += 1)
                {
                    BulkInsertColumnValue bulkInsertColumnValue = bulkInsertRow.Columns[cidx];
                    if (bulkInsertColumnValue.FilterColumn.TablesetColumn.Column.bIsIdentity)
                    {
                        continue;
                    }
                    if (bulkInsertColumnValue.FilterColumn.TablesetColumn.Column.bIsAutoIncrement)
                    {
                        continue;
                    }
                    if (bulkInsertColumnValue.FilterColumn.TablesetColumn.Column.bIsReadOnly)
                    {
                        continue;
                    }

                    // Append to VALUES clause
                    if (bulkInsertColumnValue.FilterColumn.TablesetColumn.Column.DataTypeName.Contains("varchar"))
                    {
                        if (bulkInsertColumnValue.Value == null)
                        {
                            sbINSERTSql.Append(" NULL ");
                        }
                        else
                        {
                            sbINSERTSql.Append(" '" + bulkInsertColumnValue.Value + "' ");
                        }
                    }
                    else if (bulkInsertColumnValue.FilterColumn.TablesetColumn.Column.DataTypeName.Contains("date"))
                    {
                        if (bulkInsertColumnValue.Value == null)
                        {
                            sbINSERTSql.Append(" NULL ");
                        }
                        else
                        {
                            sbINSERTSql.Append(" '" + bulkInsertColumnValue.Value + "' ");
                        }
                    }
                    else
                    {
                        if (bulkInsertColumnValue.Value == null)
                        {
                            sbINSERTSql.Append(" NULL ");
                        }
                        else
                        {
                            sbINSERTSql.Append(bulkInsertColumnValue.Value);
                        }
                    }

                    // Comma between values
                    if (cidx + 1 < bulkInsertRow.Columns.Count)
                    {
                        sbINSERTSql.Append(", ");
                    }
                    else
                    {
                        sbINSERTSql.Append(" ");
                    }
                }
                sbINSERTSql.Append(" ) ");


                // Append to job SQL.
                sbINSERTRows.AppendLine(sbINSERTSql.ToString());
                sbINSERTRows.AppendLine();
            }
            bulkInsertRequest.SQL = sbINSERTRows.ToString();


            return (new questStatus(Severity.Success));
        }
        public questStatus PerformBulkInsert(BulkInsertRequest bulkInsertRequest)
        {
            // Initialize
            questStatus status = null;


            // Execute bulk insert SQL
            try
            {
                // Get tableset
                TablesetId tablesetId = new TablesetId(bulkInsertRequest.Filter.TablesetId);
                Tableset tableset = null;
                DbTablesetsMgr dbTablesetsMgr = new DbTablesetsMgr(this.UserSession);
                status = dbTablesetsMgr.Read(tablesetId, out tableset);
                if (!questStatusDef.IsSuccessOrWarning(status))
                {
                    return (status);
                }

                // Get database
                DatabaseId databaseId = new DatabaseId(tableset.DatabaseId);
                Quest.Functional.MasterPricing.Database database = null;
                DbDatabasesMgr dbDatabasesMgr = new DbDatabasesMgr(this.UserSession);
                status = dbDatabasesMgr.Read(databaseId, out database);
                if (!questStatusDef.IsSuccessOrWarning(status))
                {
                    return (status);
                }

                // Execute sql
                using (SqlConnection sqlConnection = new SqlConnection(database.ConnectionString))
                {
                    sqlConnection.Open();

                    using (SqlCommand cmd = sqlConnection.CreateCommand())
                    {
                        cmd.CommandText = bulkInsertRequest.SQL;
                        cmd.CommandType = CommandType.Text;

                        int numRows = cmd.ExecuteNonQuery();
                        if (numRows != bulkInsertRequest.Rows.Count)
                        {
                            return (new questStatus(Severity.Error, String.Format("ERROR: Bulk insert SQL execution failed: Rows: {0}", numRows)));
                        }
                    }
                }
            }
            catch (System.Exception ex)
            {
                return (new questStatus(Severity.Fatal, String.Format("EXCEPTION: executing bulk insert SQL {0} SQL: {1}",
                    bulkInsertRequest.SQL, ex.Message)));
            }
            return (new questStatus(Severity.Success));
        }
        #endregion


        #region Logging
        /*----------------------------------------------------------------------------------------------------------------------------------
         * Logging
         *---------------------------------------------------------------------------------------------------------------------------------*/
        public questStatus LogBulkInsertEvent(BulkInsertLog bulkInsertLog)
        {
            // Initialize
            questStatus status = null;


            // Check if logging on.
            if (!this._logSetting.bLogBulkInserts)
            {
                return (new questStatus(Severity.Warning, "Bulk Insert Logging OFF"));
            }


            // Log event
            BulkInsertLogId bulkInsertLogId = null;
            status = _dbBulkInsertLogsMgr.Create(bulkInsertLog, out bulkInsertLogId);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            bulkInsertLog.Id = bulkInsertLogId.Id;


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
            // Initialize
            questStatus status = null;
            try
            {
                initLogging();
            }
            catch (System.Exception ex)
            {
                status = new questStatus(Severity.Fatal, String.Format("EXCEPTION: {0}.{1}: {2}",
                        this.GetType().ToString(), MethodInfo.GetCurrentMethod().Name, ex.Message));
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        private questStatus initLogging()
        {
            // Initialize
            questStatus status = null;

            status = loadLogSettings();
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            status = initLogger();
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        private questStatus loadLogSettings()
        {
            // Initialize
            questStatus status = null;


            // Get log settings.
            LogSetting logSetting = null;
            DbLogSettingsMgr dbLogSettingsMgr = new DbLogSettingsMgr(this.UserSession);
            status = dbLogSettingsMgr.Read(out logSetting);
            if (!questStatusDef.IsSuccess(status))
            {
                this._logSetting = new LogSetting();
                return (status);
            }
            this._logSetting = logSetting;


            return (new questStatus(Severity.Success));
        }
        private questStatus initLogger()
        {
            // Initialize
            questStatus status = null;


            if (this._logSetting.bLogBulkInserts)
            {
                try
                {
                    _dbBulkInsertLogsMgr = new DbBulkInsertLogsMgr(this.UserSession);
                }
                catch (System.Exception ex)
                {
                    status = new questStatus(Severity.Fatal, String.Format("EXCEPTION: {0}.{1}: {2}",
                            this.GetType().ToString(), MethodInfo.GetCurrentMethod().Name, ex.Message));
                    return (status);
                }
            }
            return (new questStatus(Severity.Success));
        }

        private questStatus getNumEntities(BulkInsertRequest bulkInsertRequest, out int numEntities)
        {
            // Initialize
            numEntities = 0;


            // If one table, return 1.
            if (bulkInsertRequest.Filter.FilterTableList.Count == 1)
            {
                numEntities = 1;
                return (new questStatus(Severity.Success));
            }

            // Figure out number of entities.
            List<string> entityNameList = new List<string>();
            foreach (BulkInsertColumnValue bulkInsertColumnValue in bulkInsertRequest.Rows[0].Columns)
            {
                string[] pp = bulkInsertColumnValue.Name.Split('_');
                string entityName = pp[0];

                string existingEntityName = entityNameList.Find(delegate (string t) { return t == entityName; });
                if (existingEntityName == null)
                {
                    entityNameList.Add(entityName);
                }
            }
            numEntities = entityNameList.Count();

            return (new questStatus(Severity.Success));
        }
        private questStatus getFilterColumn(BulkInsertColumnValue bulkInsertColumnValue, Filter filter)
        {
            // Initialize
            string[] pp = null;
            string entityName = null;
            string columnName = null;
            FilterColumn filterColumn = null;


            if (filter.FilterTableList.Count == 1)
            {
                entityName = filter.FilterTableList[0].TablesetTable.Table.Name;
                columnName = bulkInsertColumnValue.Name;
            }
            else
            {
                pp = bulkInsertColumnValue.Name.Split('_');
                entityName = pp[0];
                columnName = pp[1];
                filterColumn = null;
            }




            // Get FilterColumn of given bulk insert column.
            FilterTable filterTable = filter.FilterTableList.Find(delegate (FilterTable t) { return t.TablesetTable.Table.Name == entityName; });
            if (filterTable != null)
            {
                filterColumn = filter.FilterColumnList.Find(delegate (FilterColumn c) {
                    return c.TablesetColumn.Column.Name == columnName && c.TablesetColumn.Column.EntityTypeId == EntityType.Table && c.TablesetColumn.Column.EntityId == filterTable.TablesetTable.Table.Id;
                });
                if (filterColumn == null)
                {
                    return (new questStatus(Severity.Error, String.Format("ERROR: filter column not found for bulk insert column {0}",
                            bulkInsertColumnValue.Name)));
                }
            }
            else
            {
                FilterView filterView = filter.FilterViewList.Find(delegate (FilterView v) { return v.TablesetView.View.Name == entityName; });
                if (filterView == null)
                {
                    return (new questStatus(Severity.Error, String.Format("ERROR: filter view not found for bulk insert column {0}",
                            bulkInsertColumnValue.Name)));
                }
                filterColumn = filter.FilterColumnList.Find(delegate (FilterColumn c) {
                    return c.TablesetColumn.Column.Name == columnName && c.TablesetColumn.Column.EntityTypeId == EntityType.View && c.TablesetColumn.Column.EntityId == filterView.TablesetView.View.Id;
                });
                if (filterColumn == null)
                {
                    return (new questStatus(Severity.Error, String.Format("ERROR: filter column not found for bulk insert column {0} (view search)",
                            bulkInsertColumnValue.Name)));
                }
            }
            bulkInsertColumnValue.FilterColumn = filterColumn;

            return (new questStatus(Severity.Success));
        }
        #endregion
    }
}