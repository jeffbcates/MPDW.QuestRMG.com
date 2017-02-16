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
using Quest.MasterPricing.Services.Data.Database;


namespace Quest.MasterPricing.Services.Data.Filters
{
    public class DbLookupMgr : DbSQLMgr
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
        public DbLookupMgr()
            : base()
        {
            initialize();
        }
        public DbLookupMgr(UserSession userSession)
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
        #endregion


        #region Public Methods
        /*==================================================================================================================================
         * Public Methods
         *=================================================================================================================================*/
        public questStatus GetLookupOptions(LookupRequest lookupRequest, List<LookupArgument> lookupArgumentList, out List<OptionValuePair> lookupOptionList)
        {
            // Initialize
            questStatus status = null;
            lookupOptionList = null;



            // Get the lookup.
            Lookup lookup = null;
            DbLookupsMgr dbLookupsMgr = new DbLookupsMgr(this.UserSession);
            status = dbLookupsMgr.Read(lookupRequest.LookupId, out lookup);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }


            // Get the database.
            DatabaseId databaseId = new DatabaseId(lookup.DatabaseId);
            Quest.Functional.MasterPricing.Database database = null;
            DbDatabasesMgr dbDatabasesMgr = new DbDatabasesMgr(this.UserSession);
            status = dbDatabasesMgr.Read(databaseId, out database);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }

            // Apply lookup arguments.
            if (lookupArgumentList == null)
            {
                lookupArgumentList = new List<LookupArgument>();
            }
            string sql = lookup.SQL;
            foreach (LookupArgument lookupArgument in lookupArgumentList)
            {
                sql = sql.Replace(lookupArgument.Name, lookupArgument.Value);
            }

            try
            {
                using (SqlConnection conn = new SqlConnection(database.ConnectionString))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(null, conn))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = sql;
                        using (SqlDataReader rdr = cmd.ExecuteReader())
                        {
                            lookupOptionList = new List<OptionValuePair>();
                            while (rdr.Read())
                            {
                                dynamic resultRow = null;
                                status = GetRow(rdr, out resultRow);
                                if (!questStatusDef.IsSuccess(status))
                                {
                                    return (status);
                                }
                                OptionValuePair optionValuePair = new OptionValuePair();

                                try
                                {
                                    optionValuePair.Id = ((IDictionary<string, object>)resultRow)[lookup.KeyField].ToString();
                                }
                                catch (System.Exception)
                                {
                                    return (new questStatus(Severity.Error, String.Format("ERROR: Lookup key field {0} not found in key SQL {1}",
                                            lookup.KeyField, lookup.SQL)));
                                }
                                string[] textFieldList = lookup.TextFields.Split(',');
                                StringBuilder sbOptionText = new StringBuilder();
                                for (int idx=0; idx < textFieldList.Length; idx += 1)
                                {
                                    string textField = textFieldList[idx];
                                    try
                                    {
                                        sbOptionText.Append(((IDictionary<string, object>)resultRow)[textField].ToString());
                                        if (idx + 1 < textFieldList.Length)
                                        {
                                            sbOptionText.Append(" - ");
                                        }
                                    }
                                    catch (System.Exception)
                                    {
                                        return (new questStatus(Severity.Error, String.Format("ERROR: Lookup text field {0} not found in key SQL {1}",
                                                textField, lookup.SQL)));
                                    }
                                    optionValuePair.Label = sbOptionText.ToString();
                                }
                                lookupOptionList.Add(optionValuePair);
                            }
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


            // Apply filter if specified
            if (lookupRequest.FilterItemId != null && lookupRequest.FilterItemId.Id >= BaseId.VALID_ID)
            {
                List<OptionValuePair> filteredLookupOptionList = null;
                status = ApplyFilterToLookResults(lookupRequest, lookupOptionList, out filteredLookupOptionList);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }
                lookupOptionList = filteredLookupOptionList;
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus ApplyFilterToLookResults(LookupRequest lookupRequest, List<OptionValuePair> lookupOptionList, out List<OptionValuePair> filteredLookupOptionList)
        {
            // THIS IS NOW BROKEN THANKS TO IDENTIFIER-BASED REQUIREMENTS.
            // CHANGED THIS TO READ "SHALLOW" COPY OF FILTER ITEM.  HOPE IT WORKS OUT 


            // Initialize
            questStatus status = null;
            filteredLookupOptionList = null;


            // Must have a valid filter Id
            if (lookupRequest.FilterItemId == null || lookupRequest.FilterItemId.Id < BaseId.VALID_ID)
            {
                return (new questStatus(Severity.Error, String.Format("Invalid LookupRequest {0} FilterItemId", lookupRequest.LookupId.Id)));
            }


            // Get the filter items
            FilterItem filterItem = null;
            DbFilterItemsMgr dbFilterItemsMgr = new DbFilterItemsMgr(this.UserSession);
            status = dbFilterItemsMgr.Read(lookupRequest.FilterItemId, out filterItem);
            if (!questStatusDef.IsSuccess(status))
            {
                return(status);
            }

            // Accumulate all filter values.
            List<FilterValue> filterValueList = new List<FilterValue>();
            if (filterItem.LookupId == lookupRequest.LookupId.Id)
            {
                foreach (FilterOperation filterOperation in filterItem.OperationList)
                {
                    if (filterOperation.ValueList.Count > 0)
                    {
                        filterValueList.AddRange(filterOperation.ValueList);
                    }
                }
            }

            // If no filter values, return full list.
            if (filterValueList.Count == 0)
            {
                filteredLookupOptionList = lookupOptionList;
                return (new questStatus(Severity.Success));
            }

            // Filter lookup by filter item values.
            filteredLookupOptionList = new List<OptionValuePair>();
            foreach (FilterValue filterValue in filterValueList)
            {
                OptionValuePair optionValuePair = lookupOptionList.Find(delegate (OptionValuePair o) { return o.Id == filterValue.Value; });
                if (optionValuePair != null)
                {
                    filteredLookupOptionList.Add(optionValuePair);
                }
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
        #endregion
    }
}