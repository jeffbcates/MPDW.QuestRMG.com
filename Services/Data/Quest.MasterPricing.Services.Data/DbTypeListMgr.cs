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
    public class DbTypeListMgr : DbSQLMgr
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
        public DbTypeListMgr()
            : base()
        {
            initialize();
        }
        public DbTypeListMgr(UserSession userSession)
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
        public questStatus GetTypeListOptions(TypeListId typeListId, List<TypeListArgument> typeListArgumentList, out List<OptionValuePair> typeListOptionList)
        {
            // Initialize
            questStatus status = null;
            typeListOptionList = null;


            // Get the typeList.
            TypeList typeList = null;
            DbTypeListsMgr dbTypeListsMgr = new DbTypeListsMgr(this.UserSession);
            status = dbTypeListsMgr.Read(typeListId, out typeList);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }


            // Get the database.
            DatabaseId databaseId = new DatabaseId(typeList.DatabaseId);
            Quest.Functional.MasterPricing.Database database = null;
            DbDatabasesMgr dbDatabasesMgr = new DbDatabasesMgr(this.UserSession);
            status = dbDatabasesMgr.Read(databaseId, out database);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }

            // Apply typeList arguments.
            if (typeListArgumentList == null)
            {
                typeListArgumentList = new List<TypeListArgument>();
            }
            string sql = typeList.SQL;
            foreach (TypeListArgument typeListArgument in typeListArgumentList)
            {
                sql = sql.Replace(typeListArgument.Name, typeListArgument.Value);
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
                            typeListOptionList = new List<OptionValuePair>();
                            while (rdr.Read())
                            {
                                OptionValuePair optionValuePair = new OptionValuePair();
                                optionValuePair.Id = rdr[0].ToString();
                                optionValuePair.Label = rdr[0].ToString();
                                typeListOptionList.Add(optionValuePair);
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