using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Data.Entity.Validation;
using System.Data.Entity.Infrastructure;
using System.Dynamic;
using System.Linq;
using System.Linq.Dynamic;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Quest.Functional.Logging;
using Quest.Functional.ASM;
using Quest.Functional.FMS;
using Quest.Functional.MasterPricing;
using Quest.Util.Status;
using Quest.Util.Buffer;
using Quest.Util.Data;
using Quest.Services.Dbio.PWTrackerLogging;


namespace Quest.MPDW.Services.Data
{
    public class DbMgr
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
        public DbMgr()
        {
            initialize();
        }
        public DbMgr(UserSession userSession)
        {
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

        #region Transactions
        /*----------------------------------------------------------------------------------------------------------------------------------
         * Transactions
         *---------------------------------------------------------------------------------------------------------------------------------*/
        public questStatus GetUniqueTransactionName(string prefix, out string transactionName)
        {
            // Initialize
            transactionName = null;
            transactionName = (prefix == null ? "" : prefix) + Guid.NewGuid().ToString();
            return (new questStatus(Severity.Success));
        }
        public questStatus BeginTransaction(string transactionName, out DbMgrTransaction trans)
        {
            trans = new DbMgrTransaction(transactionName);
            return (trans.BeginTransaction());
        }
        public questStatus BeginTransaction(string database, string transactionName, out DbMgrTransaction trans)
        {
            trans = new DbMgrTransaction(database, transactionName);
            return (trans.BeginTransaction());
        }
        public questStatus RollbackTransaction(DbMgrTransaction trans)
        {
            return (trans.RollbackTransaction());
        }
        public questStatus CommitTransaction(DbMgrTransaction trans)
        {
            return (trans.CommitTransaction());
        }
        #endregion


        #region Databases
        /*----------------------------------------------------------------------------------------------------------------------------------
         * Databases
         *---------------------------------------------------------------------------------------------------------------------------------*/
        public questStatus OpenDatabase(Quest.Functional.MasterPricing.Database database, out SqlConnection sqlConnection)
        {
            // Initialize 
            questStatus status = null;
            sqlConnection = null;

            try
            {
                sqlConnection = new SqlConnection(database.ConnectionString);
                sqlConnection.Open();
            }
            catch (System.ArgumentException ex)
            {
                status = new questStatus(Severity.Fatal, String.Format("Cannot open database {0}: {1}", database.Name, ex.Message));
                return (status);
            }
            catch (System.Exception ex)
            {
                status = new questStatus(Severity.Fatal, String.Format("EXCEPTION: {0}.{1}: {2}",
                        this.GetType().ToString(), MethodInfo.GetCurrentMethod().Name, ex.Message));
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public string BracketIdentifier(string identifier)
        {
            if (identifier.StartsWith("[") && identifier.EndsWith("]"))
            {
                return (identifier);
            }
            string _identifier = "[" + identifier + "]";
            return (_identifier);
        }
        public string StripBrackets(string identifier)
        {
            string _identifer = identifier.Replace("[", "");
            identifier = _identifer.Replace("]", "");
            return (identifier);
        }
        #endregion


        #region I/O Support Routines
        /*----------------------------------------------------------------------------------------------------------------------------------
         * I/O Support Routines
         *---------------------------------------------------------------------------------------------------------------------------------*/
        public string BuildSortString(SortColumnList sortColumnList)
        {
            if (sortColumnList.Columns.Count == 0)
            {
                return ("1 ASC");
            }
            StringBuilder sbSort = new StringBuilder();
            for (int i = 0; i < sortColumnList.Columns.Count; i++)
            {
                SortColumn sortColumn = sortColumnList.Columns[i];
                sbSort.Append(sortColumn.Name + " ");
                if (sortColumn.Direction == SortDirection.DESC)
                {
                    sbSort.Append("DESC");
                }
                if (i + 1 < sortColumnList.Columns.Count)
                {
                    sbSort.Append(", ");
                }
            }
            return (sbSort.ToString());
        }
        public QueryResponse BuildQueryResponse(QueryOptions queryOptions, int count)
        {
            QueryResponse queryResponse = new QueryResponse();
            queryResponse.TotalRecords = count;
            queryResponse.PageNumber = queryOptions.Paging.PageNumber;
            queryResponse.PageSize = queryOptions.Paging.PageSize;
            queryResponse.TotalPages = (int)Math.Ceiling((double)queryResponse.TotalRecords / queryOptions.Paging.PageSize);

            return (queryResponse);
        }
        public string BuildSearchStringWhere(QueryOptions queryOptions, PropertyInfo[] dbProperties)
        {
            // NOTE: THIS ROUTINE IS NOT DONE, IT'S PASSING BACK AN EMPTY WHERE CLAUSE FOR NOW.
            // NOTE: THIS IS EVOLVING.  THE SEARCH STRING NEEDS TO WORK WITH ANY COLUMN DATATYPE TO WHICH ITS VALUE IS APPLICABLE/DOABLE/FEASIBLE.
            string _whereClause = "";
            string searchString = queryOptions.SearchOptions.SearchString;

            // Determine what column datatypes the search string can be applied to.
            // NOTE: STRING IS ALWAYS COMPATIBLE WITH ANY SEARCH STRING VALUE.
            int _integerSearchString = -1;
            bool bIntegerCompatible = int.TryParse(searchString, out _integerSearchString);

            decimal _decimalSearchString = -1.0m;
            bool bDecimalCompatible = decimal.TryParse(searchString, out _decimalSearchString);

            DateTime _dateTimeSearchString = new DateTime();
            bool bDateTimeCompatible = DateTime.TryParse(searchString, out _dateTimeSearchString);


            // Build WHERE clause for each column that can have the search string value applied to it.
            for (int idx = 0; idx < dbProperties.Length; idx++)
            {
                PropertyInfo propertyInfo = dbProperties[idx];
                if (propertyInfo.PropertyType == typeof(System.String))
                {

                }
                else if (propertyInfo.PropertyType == typeof(System.Int32))
                {

                }
                else if (propertyInfo.PropertyType == typeof(System.Int16))
                {

                }
                else if (propertyInfo.PropertyType == typeof(System.Int64))
                {

                }
                else if (propertyInfo.PropertyType == typeof(System.Decimal))
                {

                }
                else if (propertyInfo.PropertyType == typeof(System.Double))
                {

                }
                else if (propertyInfo.PropertyType == typeof(System.DateTime))
                {

                }
            }
            return (_whereClause);
        }
        public string BuildWhereClause(QueryOptions queryOptions, PropertyInfo[] dbProperties)
        {
            // NOTE: THIS IS A TEMPORARY, LIMITED SUPPORT UNTIL FULL-BLOWN DYNAMIC LINQ IS BUILT.
            if (queryOptions.SearchOptions.SearchFieldList.Count == 0)
            {
                if (string.IsNullOrEmpty(queryOptions.SearchOptions.SearchString))
                {
                    return ("1=1");
                }
                else
                {
                    return (BuildSearchStringWhere(queryOptions, dbProperties));
                }
            }

            StringBuilder sbWhere = new StringBuilder();
            for (int idx = 0; idx < queryOptions.SearchOptions.SearchFieldList.Count; idx++)
            {
                SearchField searchField = queryOptions.SearchOptions.SearchFieldList[idx];
                

                // Get Property Type
                PropertyInfo propertyInfo = dbProperties.Where(i => i.Name == searchField.Name).FirstOrDefault();
                String value = null;
                if (propertyInfo.PropertyType == typeof(System.String))
                {
                    value = String.Format("\"{0}\"", searchField.Value);
                }
                else if (propertyInfo.PropertyType == typeof(System.DateTime))
                {
                    DateTime dateTime = DateTime.MinValue;
                    if (! DateTime.TryParse(searchField.Value, out dateTime))
                    {
                        throw new Exception(String.Format("Invalid DateTime value: {0}", searchField.Value));
                    }
                    if (searchField.SearchOperation == SearchOperation.DateOnly)
                    {
                        value = dateTime.ToString("yyyy-MM-dd");
                    }
                    else
                    {
                        value = dateTime.ToString();
                    }
                }
                else if (propertyInfo.PropertyType == typeof(System.Boolean))
                {
                    int oneOrZero = -1;
                    if (int.TryParse(searchField.Value, out oneOrZero))  // Assuming value is 1 or 0
                    {
                        if (oneOrZero == 0)
                        {
                            value = "FALSE";
                        }
                        else
                        {
                            value = "TRUE";
                        }
                    }
                    else  // Value must be 'true' or 'false' after converting to lowercase
                    {
                        if (searchField.Value.ToLower() == "false")
                        {
                            value = "FALSE";
                        }
                        else
                        {
                            value = "TRUE";
                        }
                    }
                }
                else
                {
                    value = searchField.Value == null ? null : searchField.Value.ToString();
                }

                // Column name starts out the WHERE phrase unless it is a DateTime
                if (propertyInfo.PropertyType != typeof(System.DateTime))
                {
                    sbWhere.Append(String.Format("{0}", searchField.Name));
                }

                // Build based on operation
                switch (searchField.SearchOperation)
                {
                    case SearchOperation.Equal:
                        if (propertyInfo.PropertyType == typeof(System.DateTime))
                        {
                            sbWhere.Append(dateOperatorSubClause(searchField.Name, value, searchField.SearchOperation));
                        }
                        else
                        {
                            sbWhere.Append(String.Format(" = {0}", value));
                        }
                        break;
                    case SearchOperation.NotEquals:
                        if (propertyInfo.PropertyType == typeof(System.DateTime))
                        {
                            sbWhere.Append(dateOperatorSubClause(searchField.Name, value, searchField.SearchOperation));
                        }
                        else
                        {
                            sbWhere.Append(String.Format(" != {0}", value));
                        }
                        break;
                    case SearchOperation.LessThan:
                        if (propertyInfo.PropertyType == typeof(System.DateTime))
                        {
                            sbWhere.Append(dateOperatorSubClause(searchField.Name, value, searchField.SearchOperation));
                        }
                        else
                        {
                            sbWhere.Append(String.Format(" < {0}", value));
                        }
                        break;
                    case SearchOperation.LessThanOrEqual:
                        if (propertyInfo.PropertyType == typeof(System.DateTime))
                        {
                            sbWhere.Append(dateOperatorSubClause(searchField.Name, value, searchField.SearchOperation));
                        }
                        else
                        {
                            sbWhere.Append(String.Format(" <= {0}", value));
                        }
                        break;
                    case SearchOperation.GreaterThan:
                        if (propertyInfo.PropertyType == typeof(System.DateTime))
                        {
                            sbWhere.Append(dateOperatorSubClause(searchField.Name, value, searchField.SearchOperation));
                        }
                        else
                        {
                            sbWhere.Append(String.Format(" > {0}", value));
                        }
                        break;
                    case SearchOperation.GreaterThanOrEqual:
                        if (propertyInfo.PropertyType == typeof(System.DateTime))
                        {
                            sbWhere.Append(dateOperatorSubClause(searchField.Name, value, searchField.SearchOperation));
                        }
                        else
                        {
                            sbWhere.Append(String.Format(" >= {0}", value));
                        }
                        break;
                    case SearchOperation.Contains:
                        sbWhere.Append(String.Format(" LIKE '%{0}%' ", value));
                        break;
                    case SearchOperation.DoesNotContain:
                        sbWhere.Append(String.Format(" NOT LIKE '%{0}%' ", value));
                        break;
                    case SearchOperation.BeginsWith:
                        sbWhere.Append(String.Format(" LIKE '{0}%' ", value));
                        break;
                    case SearchOperation.DoesNotBeginWith:
                        sbWhere.Append(String.Format(" NOT LIKE '{0}%' ", value));
                        break;
                    case SearchOperation.EndsWith:
                        sbWhere.Append(String.Format(" LIKE '%{0}' ", value));
                        break;
                    case SearchOperation.DoesNotEndWith:
                        sbWhere.Append(String.Format(" NOT LIKE '%{0}' ", value));
                        break;
                    case SearchOperation.IsNull:
                        sbWhere.Append(String.Format(" = NULL "));
                        break;
                    case SearchOperation.IsNotNull:
                        sbWhere.Append(String.Format(" != NULL "));
                        break;
                    default:
                        throw (new System.Exception(String.Format("EXCEPTION: DbMgr.BuildWhereClause: Invalid Search Operator: {0}", 
                                Enum.GetName(typeof(SearchOperation), searchField.SearchOperation))));
                }
                if (idx + 1 < queryOptions.SearchOptions.SearchFieldList.Count)
                {
                    sbWhere.Append(" AND ");
                }
            }
            return (sbWhere.ToString());
        }
        public questStatus BuildQueryResponse(int count, QueryOptions queryOptions, out QueryResponse queryResponse)
        {
            queryResponse = new QueryResponse();
            try
            {
                queryResponse.TotalRecords = count;
                queryResponse.PageNumber = queryOptions.Paging.PageNumber;
                queryResponse.PageSize = queryOptions.Paging.PageSize;              
                if (queryOptions.Paging.PageSize == 0)
                {
                    queryOptions.Paging.PageSize = count > 0 ? count : 1;
                }

                int _totalPages = 1;
                if (count > 0)
                {
                    _totalPages = count / queryOptions.Paging.PageSize;
                    if (count % queryOptions.Paging.PageSize > 0)
                    {
                        _totalPages++;
                    }
                }
                queryResponse.TotalPages = _totalPages;
            }
            catch (System.Exception ex)
            {
                return (new questStatus(Severity.Fatal, String.Format("EXCEPTION: DbMgr.BuildQueryResponse: {0}", ex.InnerException != null ? ex.InnerException.Message : ex.Message)));
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus RemoveForeignKeySortColumns(PropertyInfo[] propertyInfoArray, List<SortColumn> sortColumnInList, out List<SortColumn> removedSortColumnList)
        {
            List<string> foreignKeyColumnList = new List<string>();
            List<PropertyInfo> propertyInfoList = propertyInfoArray.ToList();
            foreach (SortColumn sortColumn  in sortColumnInList)
            {
                PropertyInfo propertyInfo = propertyInfoList.Find(delegate(PropertyInfo pi) { return (pi.Name == sortColumn.Name); });
                if (propertyInfo == null)
                {
                    foreignKeyColumnList.Add(propertyInfo.Name);
                }
            }
            return (RemoveForeignKeySortColumns(foreignKeyColumnList, sortColumnInList, out removedSortColumnList));
        }
        public questStatus RemoveForeignKeySortColumns(string foreignKeyColumn, List<SortColumn> sortColumnInList, out List<SortColumn> removedSortColumnList)
        {
            List<string> foreignKeyColumnList = new List<string>();
            foreignKeyColumnList.Add(foreignKeyColumn);
            return (RemoveForeignKeySortColumns(foreignKeyColumnList, sortColumnInList, out removedSortColumnList));
        }
        public questStatus RemoveForeignKeySortColumns(List<string> foreignKeyColumnList, List<SortColumn> sortColumnInList, out List<SortColumn> removedSortColumnList)
        {
            // Initialize
            removedSortColumnList = new List<SortColumn>();


            // Remove sort columns
            foreach (string foriengKeyColumn in foreignKeyColumnList)
            {
                SortColumn sortColumn = sortColumnInList.Find(delegate(SortColumn _sc) { return (_sc.Name == foriengKeyColumn); });
                if (sortColumn != null)
                {
                    sortColumnInList.Remove(sortColumn);
                    removedSortColumnList.Add(sortColumn);
                }
            }
            return (new questStatus(Severity.Success));
        }
        #endregion


        #region SqlParameters
        /*----------------------------------------------------------------------------------------------------------------------------------
         * SqlParameters
         *---------------------------------------------------------------------------------------------------------------------------------*/
        public questStatus BuildIntegerArrayTableType(string parameterName, List<int> intList, out SqlParameter sqlParameter)
        {
            // Initialize
            sqlParameter = null;


            // Build type
            DataTable dtWellIds = new DataTable();
            dtWellIds.Columns.Add("n", typeof(int));
            foreach (int Id in intList)
            {
                dtWellIds.Rows.Add(Id);
            }

            // Build parameter
            sqlParameter = new SqlParameter(parameterName, SqlDbType.Structured);
            sqlParameter.TypeName = "dbo.IntegerArrayTableType";
            sqlParameter.Value = dtWellIds;

            return (new questStatus(Severity.Success));
        }
        #endregion


        #region SqlDataReader
        /*----------------------------------------------------------------------------------------------------------------------------------
         * SqlDataReader
         *---------------------------------------------------------------------------------------------------------------------------------*/
        public questStatus TransferSQLReader(SqlDataReader sqlDataReader, object destination, bool bIgnoreExceptions = false)
        {
            PropertyInfo[] piDestination = destination.GetType().GetProperties();
            foreach (PropertyInfo piD in piDestination)
            {
                if (piD.PropertyType == typeof(System.String))
                {
                    string _stringValue = sqlDataReader[piD.Name] == DBNull.Value ? null : sqlDataReader[piD.Name].ToString();
                    piD.SetValue(destination, _stringValue, null);
                }
                else if (piD.PropertyType == typeof(System.Int16))
                {
                    short _shortValue = Convert.ToInt16(sqlDataReader[piD.Name]);
                    piD.SetValue(destination, _shortValue, null);
                }
                else if (piD.PropertyType == typeof(System.Int32))
                {
                    int _intValue = Convert.ToInt32(sqlDataReader[piD.Name]);
                    piD.SetValue(destination, _intValue, null);
                }
                else if (piD.PropertyType == typeof(System.Int64))
                {
                    long _longValue = Convert.ToInt64(sqlDataReader[piD.Name]);
                    piD.SetValue(destination, _longValue, null);
                }
                else if (piD.PropertyType == typeof(System.DateTime))
                {
                    DateTime _dateTimeValue = Convert.ToDateTime(sqlDataReader[piD.Name]);
                    piD.SetValue(destination, _dateTimeValue, null);
                }
                else if (piD.PropertyType == typeof(System.Decimal))
                {
                    Decimal _decimalValue = Convert.ToDecimal(sqlDataReader[piD.Name]);
                    piD.SetValue(destination, _decimalValue, null);
                }
                else if (piD.PropertyType == typeof(System.Boolean))
                {
                    bool _boolValue = Convert.ToBoolean(sqlDataReader[piD.Name]);
                    piD.SetValue(destination, _boolValue, null);
                }
                else if (piD.PropertyType == typeof(System.Char))
                {
                    char _charValue = Convert.ToChar(sqlDataReader[piD.Name]);
                    piD.SetValue(destination, _charValue, null);
                }
                else if (piD.PropertyType == typeof(System.Byte))
                {
                    byte _byteValue = Convert.ToByte(sqlDataReader[piD.Name]);
                    piD.SetValue(destination, _byteValue, null);
                }
                else if (piD.PropertyType == typeof(System.Double))
                {
                    double _doubleValue = Convert.ToDouble(sqlDataReader[piD.Name]);
                    piD.SetValue(destination, _doubleValue, null);
                }
                else if (piD.PropertyType == typeof(System.Guid))
                {
                    Guid _guidValue = new Guid(sqlDataReader[piD.Name].ToString());
                    piD.SetValue(destination, _guidValue, null);
                }
                else if (piD.PropertyType == typeof(System.SByte))
                {
                    SByte _sbyteValue = Convert.ToSByte(sqlDataReader[piD.Name]);
                    piD.SetValue(destination, _sbyteValue, null);
                }
                else if (piD.PropertyType == typeof(System.Single))
                {
                    Single _singleValue = Convert.ToSingle(sqlDataReader[piD.Name]);
                    piD.SetValue(destination, _singleValue, null);
                }
                else if (piD.PropertyType.Name == "Nullable`1")
                {
                    if (piD.PropertyType == typeof(System.Int16))
                    {
                        if (sqlDataReader[piD.Name] != DBNull.Value)
                        {
                            short _shortValue = Convert.ToInt16(sqlDataReader[piD.Name]);
                            piD.SetValue(destination, _shortValue, null);
                        }
                    }
                    else if (piD.PropertyType.GenericTypeArguments[0].FullName == "System.Int32")
                    {
                        if (sqlDataReader[piD.Name] != DBNull.Value)
                        {
                            int _intValue = Convert.ToInt32(sqlDataReader[piD.Name]);
                            piD.SetValue(destination, _intValue, null);
                        }
                    }
                    else if (piD.PropertyType.GenericTypeArguments[0].FullName == "System.Int64")
                    {
                        if (sqlDataReader[piD.Name] != DBNull.Value)
                        {
                            long _longValue = Convert.ToInt64(sqlDataReader[piD.Name]);
                            piD.SetValue(destination, _longValue, null);
                        }
                    }
                    else if (piD.PropertyType.GenericTypeArguments[0].FullName == "System.DateTime")
                    {
                        if (sqlDataReader[piD.Name] != DBNull.Value)
                        {
                            DateTime _dateTimeValue = Convert.ToDateTime(sqlDataReader[piD.Name]);
                            piD.SetValue(destination, _dateTimeValue, null);
                        }
                    }
                    else if (piD.PropertyType.GenericTypeArguments[0].FullName == "System.Decimal")
                    {
                        if (sqlDataReader[piD.Name] != DBNull.Value)
                        {
                            Decimal _decimalValue = Convert.ToDecimal(sqlDataReader[piD.Name]);
                            piD.SetValue(destination, _decimalValue, null);
                        }
                    }
                    else if (piD.PropertyType.GenericTypeArguments[0].FullName == "System.Boolean")
                    {
                        if (sqlDataReader[piD.Name] != DBNull.Value)
                        {
                            bool _boolValue = Convert.ToBoolean(sqlDataReader[piD.Name]);
                            piD.SetValue(destination, _boolValue, null);
                        }
                    }
                    else if (piD.PropertyType.GenericTypeArguments[0].FullName == "System.Char")
                    {
                        if (sqlDataReader[piD.Name] != DBNull.Value)
                        {
                            char _charValue = Convert.ToChar(sqlDataReader[piD.Name]);
                            piD.SetValue(destination, _charValue, null);
                        }
                    }
                    else if (piD.PropertyType.GenericTypeArguments[0].FullName == "System.Byte")
                    {
                        if (sqlDataReader[piD.Name] != DBNull.Value)
                        {
                            byte _byteValue = Convert.ToByte(sqlDataReader[piD.Name]);
                            piD.SetValue(destination, _byteValue, null);
                        }
                    }
                    else if (piD.PropertyType.GenericTypeArguments[0].FullName == "System.Double")
                    {
                        if (sqlDataReader[piD.Name] != DBNull.Value)
                        {
                            double _doubleValue = Convert.ToDouble(sqlDataReader[piD.Name]);
                            piD.SetValue(destination, _doubleValue, null);
                        }
                    }
                    else if (piD.PropertyType.GenericTypeArguments[0].FullName == "System.Guid")
                    {
                        if (sqlDataReader[piD.Name] != DBNull.Value)
                        {
                            Guid _guidValue = new Guid(sqlDataReader[piD.Name].ToString());
                            piD.SetValue(destination, _guidValue, null);
                        }
                    }
                    else if (piD.PropertyType.GenericTypeArguments[0].FullName == "System.SByte")
                    {
                        if (sqlDataReader[piD.Name] != DBNull.Value)
                        {
                            SByte _sbyteValue = Convert.ToSByte(sqlDataReader[piD.Name]);
                            piD.SetValue(destination, _sbyteValue, null);
                        }
                    }
                    else if (piD.PropertyType.GenericTypeArguments[0].FullName == "System.Single")
                    {
                        if (sqlDataReader[piD.Name] != DBNull.Value)
                        {
                            Single _singleValue = Convert.ToSingle(sqlDataReader[piD.Name]);
                            piD.SetValue(destination, _singleValue, null);
                        }
                    }
                }
                else
                {
                    if (!bIgnoreExceptions)
                    {
                        throw new System.Exception(String.Format("EXCEPTION: {0}.{1} Unexpected property type: ({2})",
                                 this.GetType().ToString(), MethodInfo.GetCurrentMethod().Name,
                                 piD.PropertyType.Name));
                    }
                }
            }
            return (new questStatus(Severity.Success));
        }
        #endregion


        #region ErrorMessages
        /*----------------------------------------------------------------------------------------------------------------------------------
         * ErrorMessages
         *---------------------------------------------------------------------------------------------------------------------------------*/
        public string GetErrorMessage(Exception ex)
        {
            string errorMessage = "";
            if (ex is DbEntityValidationException) // EF5 validation 
            {
                errorMessage = getDbEntityValidationExceptionMessage((DbEntityValidationException)ex);
            }
            else if (ex is DbUpdateException)  // EF6 validation 
            {
                errorMessage = "DbUpdateException: " + getInnerMostErrorMessage(ex);
            }
            else
            {
                errorMessage = getInnerMostErrorMessage(ex);
            }
            return errorMessage;
        }
        #endregion


        #region ValidateRequiredFields
        /*----------------------------------------------------------------------------------------------------------------------------------
         * ValidateRequiredFields
         *---------------------------------------------------------------------------------------------------------------------------------*/
        public questStatus TrimAndValidate(Object source, List<string> requiredFieldList)
        {
            // Initialize.
            questStatus status = null;


            // Trim fields.
            try
            {
                BufferMgr.TrimRequiredFields(source);
            }
            catch (Exception ex)
            {
                status = new questStatus(Severity.Error, ex.Message);
                return (status);
            }


            // Validate required fields.
            status = this.ValidateRequiredFields(source, requiredFieldList);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus ValidateRequiredFields(Object source, List<string> requiredFieldList)
        {
            // initialize
            questStatus status;
            List<string> missingRequiredFieldList = new List<string>();
            string className = source.GetType().Name;

            foreach (string fieldName in requiredFieldList)
            {
                PropertyInfo piS = source.GetType().GetProperty(fieldName);
                if (piS == null)
                {
                    // Can't find the property - developer oversight. Changed a property lately?
                    string errorMessage = String.Format("InvalidRequiredDataMember: {0}.{1}", className, fieldName);
                    status = new questStatus(Severity.Error, errorMessage);
                    return status;
                }
                else
                {
                    string fieldType = piS.PropertyType.FullName;
                    switch (fieldType)
                    {
                        case "System.String":
                            string sValue = (string)piS.GetValue(source, null);
                            if (sValue == null)
                            {
                                missingRequiredFieldList.Add(fieldName);
                            }
                            else if (string.IsNullOrEmpty(sValue.Trim()))
                            {
                                missingRequiredFieldList.Add(fieldName);
                            }
                            break;
                        case "System.Int32":
                            int iValue = (int)piS.GetValue(source, null);
                            if (iValue < BaseId.VALID_ID)
                            {
                                missingRequiredFieldList.Add(fieldName);
                            }
                            break;
                        default:
                            break;
                    }
                }
            }
            if (missingRequiredFieldList.Count > 0)
            {
                string errorMessage = String.Format("MissingRequiredDataMember: {0}|{1}", className, String.Join(",", missingRequiredFieldList.ToArray<string>()));
                status = new questStatus(Severity.Error, errorMessage);
                return status;
            }
            return (new questStatus(Severity.Success));
        }
        #endregion


        #region Column Metadata
        /*----------------------------------------------------------------------------------------------------------------------------------
         * Column Metadata
         *---------------------------------------------------------------------------------------------------------------------------------*/
        public questStatus GetColumnInfo(SqlDataReader rdr, out List<Quest.Functional.MasterPricing.Column> columnList)
        {
            // Initialize


            // Get column info
            DataTable schemaTable = rdr.GetSchemaTable();
            columnList = new List<Quest.Functional.MasterPricing.Column>();
            foreach (DataRow myField in schemaTable.Rows)
            {
                Quest.Functional.MasterPricing.Column column = new Quest.Functional.MasterPricing.Column();
                column.Name = myField["ColumnName"].ToString();
                column.DisplayOrder = Convert.ToInt32(myField["ColumnOrdinal"]);
                column.ColumnSize = Convert.ToInt32(myField["ColumnSize"]);
                column.DataType = myField["DataType"].ToString();
                column.DataTypeName = myField["DataTypeName"].ToString();
                column.bIsIdentity = Convert.ToBoolean(myField["IsIdentity"]);
                column.bAllowDbNull = Convert.ToBoolean(myField["AllowDbNull"]);
                columnList.Add(column);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus GetDatabaseStoredProcedures(Quest.Functional.MasterPricing.Database database, out List<Quest.Functional.MasterPricing.StoredProcedure> storedProcedureList)
        {
            // Initialize
            questStatus status = null;
            storedProcedureList = null;


            try
            {
                using (SqlConnection conn = new SqlConnection(database.ConnectionString))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.Connection = conn;
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = " SELECT SCHEMA_NAME(schema_id) AS [SCHEMA], [NAME] FROM sys.procedures (NOLOCK) WHERE [type] = 'P' AND [NAME] LIKE 'Quest%' ";
                        using (SqlDataReader rdr = cmd.ExecuteReader())
                        {
                            storedProcedureList = new List<Quest.Functional.MasterPricing.StoredProcedure>();
                            while (rdr.Read())
                            {
                                Quest.Functional.MasterPricing.StoredProcedure storedProcedure = new Quest.Functional.MasterPricing.StoredProcedure();
                                string schema = BracketIdentifier(rdr["SCHEMA"].ToString());
                                string name = BracketIdentifier(rdr["NAME"].ToString());
                                storedProcedure.Name = schema + "." + name;
                                storedProcedureList.Add(storedProcedure);
                            }
                        }
                    }

                    // Get stored procedure parameters.
                    foreach (Quest.Functional.MasterPricing.StoredProcedure storedProcedure in storedProcedureList)
                    {
                        // Get sproc parameters.  If we can't get 'em, sproc is encrypted/locked, so don't offer it as an option.
                        List<Quest.Functional.MasterPricing.StoredProcedureParameter> storedProcedureParameterList = null;
                        string storedProcedureName = StripBrackets(storedProcedure.Name);
                        status = GetStoredProdecureParameters(database, storedProcedureName, out storedProcedureParameterList);
                        if (questStatusDef.IsSuccess(status))
                        {
                            storedProcedure.ParameterList = storedProcedureParameterList;
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

        public questStatus GetMetaParameters(Quest.Functional.MasterPricing.Database database, string storedProcedureName, out List<Quest.Functional.MasterPricing.FilterProcedureParameter> filterProcedureParameterList)
        {
            // Initialize 
            questStatus status = null;
            filterProcedureParameterList = null;

            List<Quest.Functional.MasterPricing.FilterProcedureParameter> allParameterList = null;
            status = GetStoredProdecureParameters(database, storedProcedureName, out allParameterList);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            filterProcedureParameterList = new List<FilterProcedureParameter>();
            foreach (Quest.Functional.MasterPricing.FilterProcedureParameter filterProcedureParameter in allParameterList)
            {
                if (filterProcedureParameter.ParameterName.StartsWith("@_"))
                {
                    filterProcedureParameterList.Add(filterProcedureParameter);
                }
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus GetStoredProdecureParameters(Quest.Functional.MasterPricing.Database database, string storedProcedureName, out List<Quest.Functional.MasterPricing.FilterProcedureParameter> filterProcedureParameterList)
        {
            // Initialize 
            questStatus status = null;
            filterProcedureParameterList = null;

            try
            {
                using (SqlConnection conn = new SqlConnection(database.ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand(storedProcedureName, conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        conn.Open();
                        SqlCommandBuilder.DeriveParameters(cmd);
                        filterProcedureParameterList = new List<FilterProcedureParameter>();
                        foreach (SqlParameter sqlParameter in cmd.Parameters)
                        {
                            FilterProcedureParameter filterProcedureParameter = new FilterProcedureParameter();
                            BufferMgr.TransferBuffer(sqlParameter, filterProcedureParameter, true);
                            filterProcedureParameter.DbType = Enum.GetName(typeof(DbType), sqlParameter.DbType);
                            filterProcedureParameter.SqlDbType = Enum.GetName(typeof(SqlDbType), sqlParameter.SqlDbType);
                            filterProcedureParameter.Precision[0] = sqlParameter.Precision;
                            filterProcedureParameter.Scale[0] = sqlParameter.Scale;
                            filterProcedureParameterList.Add(filterProcedureParameter);
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
        public questStatus GetStoredProdecureParameters(Quest.Functional.MasterPricing.Database database, string storedProcedureName, out List<Quest.Functional.MasterPricing.StoredProcedureParameter> storedProcedureParameterList)
        {
            // Initialize 
            questStatus status = null;
            storedProcedureParameterList = null;

            try
            {
                using (SqlConnection conn = new SqlConnection(database.ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand(storedProcedureName, conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        conn.Open();
                        SqlCommandBuilder.DeriveParameters(cmd);
                        storedProcedureParameterList = new List<Quest.Functional.MasterPricing.StoredProcedureParameter>();
                        foreach (SqlParameter sqlParameter in cmd.Parameters)
                        {
                            Quest.Functional.MasterPricing.StoredProcedureParameter storedProcedureParameter = new Quest.Functional.MasterPricing.StoredProcedureParameter();
                            BufferMgr.TransferBuffer(sqlParameter, storedProcedureParameter, true);
                            storedProcedureParameter.DbType = Enum.GetName(typeof(DbType), sqlParameter.DbType);
                            storedProcedureParameter.SqlDbType = Enum.GetName(typeof(SqlDbType), sqlParameter.SqlDbType);
                            storedProcedureParameter.Precision[0] = sqlParameter.Precision;
                            storedProcedureParameter.Scale[0] = sqlParameter.Scale;
                            storedProcedureParameterList.Add(storedProcedureParameter);
                        }
                        status = determineOptionalParameters(database, storedProcedureName, storedProcedureParameterList);
                        if (! questStatusDef.IsSuccess(status))
                        {
                            return (status);
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
        #endregion


        #region Utility Routines
        /*----------------------------------------------------------------------------------------------------------------------------------
         * Utility Routines
         *---------------------------------------------------------------------------------------------------------------------------------*/
        public questStatus BuildType(SqlDataReader rdr, out Dictionary<string, Quest.Functional.MasterPricing.Column> dynamicType)
        {
            // Initialize
            questStatus status = null;
            dynamicType = new Dictionary<string, Quest.Functional.MasterPricing.Column>();
            List<string> columnNameList = new List<string>();

            List<Quest.Functional.MasterPricing.Column> columnList = null;
            status = GetColumnInfo(rdr, out columnList);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            foreach (Quest.Functional.MasterPricing.Column column in columnList)
            {
                // If key already exists, append with increasing integer values.
                string keyName = column.Name;
                List<string> duplicateKeyNames = columnNameList.FindAll(delegate (string k) { return k == keyName; });
                if (duplicateKeyNames.Count == 0)
                {
                    columnNameList.Add(keyName);
                }
                else
                {
                    keyName += columnNameList.Count.ToString();
                }

                dynamicType.Add(keyName, column);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus GetRow(SqlDataReader rdr, out dynamic resultRow)
        {
            // Initialize
            questStatus status = null;
            resultRow = new ExpandoObject();


            // Get row data.
            for (int i = 0; i < rdr.FieldCount; i++)
            {
                ((IDictionary<String, Object>)resultRow)[rdr.GetName(i)] = rdr[rdr.GetName(i)].ToString();

                status = GetColumnData(rdr, i, resultRow);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus GetColumnData(SqlDataReader rdr, int idx, dynamic resultRow)
        {
            if (rdr[rdr.GetName(idx)] == DBNull.Value)
            {
                ((IDictionary<String, Object>)resultRow)[rdr.GetName(idx)] = null;
                return (new questStatus(Severity.Success));
            }

            Type _type = rdr.GetFieldType(idx);
            if (_type == typeof(int))
            {
                ((IDictionary<String, Object>)resultRow)[rdr.GetName(idx)] = Convert.ToInt32(rdr[rdr.GetName(idx)]);
            }
            else if (_type == typeof(string))
            {
                ((IDictionary<String, Object>)resultRow)[rdr.GetName(idx)] = rdr[rdr.GetName(idx)].ToString();
            }
            else if (_type == typeof(bool))
            {
                ((IDictionary<String, Object>)resultRow)[rdr.GetName(idx)] = Convert.ToBoolean(rdr[rdr.GetName(idx)]);
            }
            else if (_type == typeof(DateTime))
            {
                ((IDictionary<String, Object>)resultRow)[rdr.GetName(idx)] = Convert.ToDateTime(rdr[rdr.GetName(idx)]);
            }
            else if (_type == typeof(byte))
            {
                ((IDictionary<String, Object>)resultRow)[rdr.GetName(idx)] = Convert.ToByte(rdr[rdr.GetName(idx)]);
            }
            else if (_type == typeof(sbyte))
            {
                ((IDictionary<String, Object>)resultRow)[rdr.GetName(idx)] = Convert.ToByte(rdr[rdr.GetName(idx)]);
            }
            else if (_type == typeof(char))
            {
                ((IDictionary<String, Object>)resultRow)[rdr.GetName(idx)] = Convert.ToChar(rdr[rdr.GetName(idx)]);
            }
            else if (_type == typeof(decimal))
            {
                ((IDictionary<String, Object>)resultRow)[rdr.GetName(idx)] = Convert.ToDecimal(rdr[rdr.GetName(idx)]);
            }
            else if (_type == typeof(double))
            {
                ((IDictionary<String, Object>)resultRow)[rdr.GetName(idx)] = Convert.ToDouble(rdr[rdr.GetName(idx)]);
            }
            else if (_type == typeof(float))
            {
                ((IDictionary<String, Object>)resultRow)[rdr.GetName(idx)] = Convert.ToSingle(rdr[rdr.GetName(idx)]);
            }
            else if (_type == typeof(short))
            {
                ((IDictionary<String, Object>)resultRow)[rdr.GetName(idx)] = Convert.ToInt16(rdr[rdr.GetName(idx)]);
            }
            else if (_type == typeof(long))
            {
                ((IDictionary<String, Object>)resultRow)[rdr.GetName(idx)] = Convert.ToInt64(rdr[rdr.GetName(idx)]);
            }
            else if (_type == typeof(Guid))
            {
                ((IDictionary<String, Object>)resultRow)[rdr.GetName(idx)] = Guid.Parse(rdr[rdr.GetName(idx)].ToString());
            }
            else
            {
                return (new questStatus(Severity.Error, String.Format("ERROR: Unknown column type: {0}", _type.ToString())));
            }
            return (new questStatus(Severity.Success));
        }
        #endregion


        #region Logging
        /*----------------------------------------------------------------------------------------------------------------------------------
         * Logging
         *---------------------------------------------------------------------------------------------------------------------------------*/
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
            }
            catch (System.Exception ex)
            {
                status = new questStatus(Severity.Fatal, String.Format("EXCEPTION: {0}.{1}: {2}",
                        this.GetType().ToString(), MethodInfo.GetCurrentMethod().Name, ex.Message));
                throw new System.Exception(status.Message, ex);
            }
            return (new questStatus(Severity.Success));
        }
        private string dateOperatorSubClause(string columnName, string Date, SearchOperation searchOperation)
        {
            // Initialize
            string subClause = null;


            // Validate date
            DateTime _date = DateTime.MinValue;
            if (!DateTime.TryParse(Date, out _date))
            {
                throw (new System.Exception(String.Format("EXCEPTION: DbMgr.exactDateSubClause: Invalid Date: {0}", Date)));
            }

            // Determine operator
            string comparisonOperator = null;
            DateTime _endDate = DateTime.MinValue;
            switch (searchOperation)
            {
                case SearchOperation.Equal:
                    DateTime _startDate = DateTime.MinValue;
                    if (!DateTime.TryParse(Date, out _startDate))
                    {
                        throw (new System.Exception(String.Format("EXCEPTION: DbMgr.exactDateSubClause: Invalid Date: {0}", Date)));
                    }
                    _endDate = _startDate.AddDays(1);

                    subClause = String.Format("( {0} >= DateTime({1}, {2}, {3}) AND {0} < DateTime({4}, {5}, {6}) )", columnName,
                            _startDate.Year, _startDate.Month, _startDate.Day, _endDate.Year, _endDate.Month, _endDate.Day);
                    break;
                case SearchOperation.NotEquals:
                    _endDate = _date.AddDays(1);
                    subClause = String.Format("( {0} < DateTime({1}, {2}, {3}) OR {0} >= DateTime({4}, {5}, {6}) )", columnName,
                            _date.Year, _date.Month, _date.Day, _endDate.Year, _endDate.Month, _endDate.Day);
                    break;
                case SearchOperation.LessThan:
                    comparisonOperator = " < ";
                    break;
                case SearchOperation.LessThanOrEqual:
                    comparisonOperator = " < ";  // Less than the next day.
                    _date = _date.AddDays(1);
                    break;
                case SearchOperation.GreaterThan:
                    comparisonOperator = " > ";
                    _date = _date.AddDays(1);
                    break;
                case SearchOperation.GreaterThanOrEqual:
                    comparisonOperator = " >= ";
                    break;
                default:
                    throw (new System.Exception(String.Format("EXCEPTION: DbMgr.dateOperatorSubClause2: Invalid operator: {0}", Enum.GetName(typeof(SearchOperation), searchOperation))));
            }

            // Build subclause
            if (subClause == null)
            {
                subClause = String.Format("( {0} {1} DateTime({2}, {3}, {4}) )", columnName, comparisonOperator,
                        _date.Year, _date.Month, _date.Day);
            }
            return (subClause);
        }
        private string getInnerMostErrorMessage(Exception ex)
        {
            Exception realerror = ex;
            while (realerror.InnerException != null)
            {
                realerror = realerror.InnerException;
            }
            return (realerror.Message.ToString());
        }
        private string getDbEntityValidationExceptionMessage(DbEntityValidationException ex)
        {
            // Retrieve the error messages as a list of strings.
            var errorMessages = ex.EntityValidationErrors.SelectMany(x => x.ValidationErrors).Select(x => x.ErrorMessage);
            String fullErrorMessage = string.Join("; ", errorMessages);
            String exceptionMessage = string.Concat("DbEntityValidationException: ", fullErrorMessage);

            return (exceptionMessage.ToString());
        }
        private questStatus determineOptionalParameters(Quest.Functional.MasterPricing.Database database, string storedProcedureName, 
                List<Quest.Functional.MasterPricing.StoredProcedureParameter> storedProcedureParameterList)
        {
            // Initialize
            questStatus status = null;


            // NOTE: ADO.NET NOR SQL SERVER ACCURATELY REPORT OPTIONAL PARAMETERS.  THEREFORE, WE HAVE TO GET THE SPROC TEXT AND DETERMINE IT OURSELVES.
            using (SqlConnection conn = new SqlConnection(database.ConnectionString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand("sys.sp_helptext", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@objname", storedProcedureName);
                    DataSet ds = new DataSet();
                    SqlDataAdapter sqlDataAdapter = new SqlDataAdapter();
                    sqlDataAdapter.SelectCommand = cmd;
                    sqlDataAdapter.Fill(ds);

                    StringBuilder sbTSQL = new StringBuilder();
                    foreach (DataRow dataRow in ds.Tables[0].Rows)
                    {
                        sbTSQL.AppendLine(dataRow.ItemArray[0].ToString());
                    }

                    // Parse the entry mask.  NOTE: THE REAL WAY TO DO THIS IS VIA A T-SQL GRAMMAR.  BUT, THAT'S AN ANOTHER EFFORT WAAAAY OUT OF SCHEDULE.
                    //                              THEREFORE WE STRING PARSE AND HOPE FOR THE BEST.
                    Dictionary<string, bool> optionalParameters = null;
                    status = parseEntryMask(sbTSQL.ToString(), out optionalParameters);
                    if (!questStatusDef.IsSuccess(status))
                    {
                        return (new questStatus(status.Severity, "ERROR: parsing stored procedure {0}: {1}", storedProcedureName, status.Message));
                    }


                }

            }
            return (new questStatus(Severity.Success));
        }
        private questStatus parseEntryMask(string TSQL, out Dictionary<string, bool> optionalParameters)
        {
            // Initialize
            optionalParameters = new Dictionary<string, bool>();

            // Going to need an ANTLR grammar to do this right.
            // See this for HOW-TO and issues involved: https://github.com/antlr/grammars-v4/tree/master/tsql


            return (new questStatus(Severity.Success));
        }
        #endregion
    }
}
