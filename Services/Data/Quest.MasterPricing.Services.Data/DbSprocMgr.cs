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
using Quest.MasterPricing.Services.Data.Database;


namespace Quest.MasterPricing.Services.Data.Filters
{
    public class DbSprocMgr : DbMgr
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
        public DbSprocMgr()
            : base()
        {
            initialize();
        }
        public DbSprocMgr(UserSession userSession)
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
        public questStatus ExecuteStoredProdecure(StoredProcedureRequest storedProcedureRequest)
        {
            // Initialize 
            questStatus status = null;


            try
            {
                using (SqlConnection conn = new SqlConnection(storedProcedureRequest.Database.ConnectionString))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        SqlCommandBuilder.DeriveParameters(cmd);
                        List<StoredProcedureParameter> storedProcedureParameterList = new List<StoredProcedureParameter>();
                        foreach (SqlParameter sqlParameter in cmd.Parameters)
                        {
                            StoredProcedureParameter storedProcedureParameter = new StoredProcedureParameter();
                            BufferMgr.TransferBuffer(sqlParameter, storedProcedureParameter, true);
                            storedProcedureParameter.DbType = Enum.GetName(typeof(DbType), sqlParameter.DbType);
                            storedProcedureParameter.SqlDbType = Enum.GetName(typeof(SqlDbType), sqlParameter.SqlDbType);
                            storedProcedureParameter.Precision[0] = sqlParameter.Precision;
                            storedProcedureParameter.Scale[0] = sqlParameter.Scale;
                            storedProcedureParameterList.Add(storedProcedureParameter);
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