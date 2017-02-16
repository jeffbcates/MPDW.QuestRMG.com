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
using Quest.Functional.MasterPricing;
using Quest.MPDW.Services.Data;
using Quest.MPDW.Services.Business;
using Quest.MasterPricing.Services.Data.Database;


namespace Quest.MasterPricing.Services.Business.Database
{
    public class TablesMgr : MgrSessionBased
    {
        #region Declarations
        /*==================================================================================================================================
         * Declarations
         *=================================================================================================================================*/
        private UserSession _userSession = null;
        private DbTablesMgr _dbTablesMgr = null;

        #endregion


        #region Constructors
        /*==================================================================================================================================
         * Constructors
         *=================================================================================================================================*/
        public TablesMgr(UserSession userSession)
            : base(userSession)
        {
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

        #region CRUD Operations
        //----------------------------------------------------------------------------------------------------------------------------------
        // CRUD Operations
        //----------------------------------------------------------------------------------------------------------------------------------
        public questStatus Create(Table table, out TableId tableId)
        {
            // Initialize
            tableId = null;
            questStatus status = null;


            // Create table
            status = _dbTablesMgr.Create(table, out tableId);
            if (! questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Create(DbMgrTransaction trans, Table table, out TableId tableId)
        {
            // Initialize
            questStatus status = null;
            tableId = null;


            // Create table
            status = _dbTablesMgr.Create(trans, table, out tableId);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Create(DbMgrTransaction trans, List<Quest.Functional.MasterPricing.Table> tableList, out List<Quest.Functional.MasterPricing.Table> tableIdList)
        {
            // Initialize
            questStatus status = null;
            tableIdList = null;


            // Create table
            status = _dbTablesMgr.Create(trans, tableList, out tableIdList);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Read(TableId tableId, out Table table)
        {
            // Initialize
            questStatus status = null;
            table = null;


            // Read table
            status = _dbTablesMgr.Read(tableId, out table);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Read(DbMgrTransaction trans, TableId tableId, out Table table)
        {
            // Initialize
            questStatus status = null;
            table = null;


            // Read table
            status = _dbTablesMgr.Read(trans, tableId, out table);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Update(Table table)
        {
            // Initialize
            questStatus status = null;


            // Update table
            status = _dbTablesMgr.Update(table);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Update(DbMgrTransaction trans, Table table)
        {
            // Initialize
            questStatus status = null;


            // Update table
            status = _dbTablesMgr.Update(trans, table);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Delete(TableId tableId)
        {
            // Initialize
            questStatus status = null;


            // Delete table
            status = _dbTablesMgr.Delete(tableId);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Delete(DbMgrTransaction trans, TableId tableId)
        {
            // Initialize
            questStatus status = null;


            // Delete table
            status = _dbTablesMgr.Delete(trans, tableId);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Delete(DatabaseId databaseId)
        {
            // Initialize
            questStatus status = null;


            // Delete table
            status = _dbTablesMgr.Delete(databaseId);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Delete(DbMgrTransaction trans, DatabaseId databaseId)
        {
            // Initialize
            questStatus status = null;


            // Delete table
            status = _dbTablesMgr.Delete(trans, databaseId);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus List(QueryOptions queryOptions, out List<Table> tableList, out QueryResponse queryResponse)
        {
            // Initialize
            questStatus status = null;
            tableList = null;


            // List usStates
            status = _dbTablesMgr.List(queryOptions, out tableList, out queryResponse);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        #endregion

        #region Business Operations
        //----------------------------------------------------------------------------------------------------------------------------------
        // Business Operations
        //----------------------------------------------------------------------------------------------------------------------------------
        public questStatus GetDatabaseTables()
        {
            return (new questStatus(Severity.Warning, "Not implemented"));
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
                _dbTablesMgr = new DbTablesMgr(this.UserSession);
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

