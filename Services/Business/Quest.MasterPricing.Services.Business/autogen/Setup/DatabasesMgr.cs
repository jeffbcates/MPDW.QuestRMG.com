using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Threading.Tasks;
using Quest.Util.Status;
using Quest.Util.Buffer;
using Quest.Util.Data;
using Quest.Functional.ASM;
using Quest.Functional.FMS;
using Quest.Functional.MasterPricing;
using Quest.MPDW.Services.Data;
using Quest.MPDW.Services.Business;
using Quest.MasterPricing.Services.Data.Database;


namespace Quest.MasterPricing.Services.Business.Database
{
    public class DatabasesMgr : MgrSessionBased
    {
        #region Declarations
        /*==================================================================================================================================
         * Declarations
         *=================================================================================================================================*/
        private DbDatabasesMgr _dbDatabasesMgr = null;

        #endregion


        #region Constructors
        /*==================================================================================================================================
         * Constructors
         *=================================================================================================================================*/
        public DatabasesMgr(UserSession userSession)
            : base(userSession)
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
        public questStatus Create(Quest.Functional.MasterPricing.Database database, out Quest.Functional.MasterPricing.DatabaseId databaseId)
        {
            // Initialize
            databaseId = null;
            questStatus status = null;


            // Create database
            status = _dbDatabasesMgr.Create(database, out databaseId);
            if (! questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Read(Quest.Functional.MasterPricing.DatabaseId databaseId, out Quest.Functional.MasterPricing.Database database)
        {
            // Initialize
            database = null;
            questStatus status = null;


            // Read database
            status = _dbDatabasesMgr.Read(databaseId, out database);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Update(Quest.Functional.MasterPricing.Database database)
        {
            // Initialize
            questStatus status = null;


            // Update database
            status = _dbDatabasesMgr.Update(database);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Delete(Quest.Functional.MasterPricing.DatabaseId databaseId)
        {
            // Initialize
            questStatus status = null;


            // Delete database
            status = _dbDatabasesMgr.Delete(databaseId);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus List(QueryOptions queryOptions, out List<Quest.Functional.MasterPricing.Database> databaseList, out QueryResponse queryResponse)
        {
            // Initialize
            questStatus status = null;
            databaseList = null;


            // List
            status = _dbDatabasesMgr.List(queryOptions, out databaseList, out queryResponse);
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
                _dbDatabasesMgr = new DbDatabasesMgr(this.UserSession);
            }
            catch (System.Exception ex)
            {
                status = new questStatus(Severity.Fatal, String.Format("EXCEPTION: {0}.{1}: {2}",
                        this.GetType().ToString(), MethodInfo.GetCurrentMethod().Name, ex.Message));
                throw new System.Exception(status.Message, ex);
            }
            return (new questStatus(Severity.Success));
        }

        #endregion
    }
}

