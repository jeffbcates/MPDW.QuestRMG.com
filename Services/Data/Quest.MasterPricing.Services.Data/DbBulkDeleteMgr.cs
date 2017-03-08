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


namespace Quest.MasterPricing.Services.Data.Bulk
{
    public class DbBulkDeleteMgr : DbSQLMgr
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
        public DbBulkDeleteMgr()
            : base()
        {
            initialize();
        }
        public DbBulkDeleteMgr(UserSession userSession)
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
        public questStatus GenerateBulkDeleteSQL(BulkDeleteRequest bulkDeleteRequest)
        {
            // Initialize

            ////bulkInsertRequest.Filter.Filter


            return (new questStatus(Severity.Success));
        }
        public questStatus PerformBulkDelete(BulkDeleteRequest bulkDeleteRequest)
        {
            // Initialize

            ////bulkInsertRequest.Filter.Filter


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