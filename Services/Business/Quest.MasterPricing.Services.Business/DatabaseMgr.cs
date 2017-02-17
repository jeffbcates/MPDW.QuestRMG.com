using System;
using System.IO;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Mail;
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
using Quest.MasterPricing.Services.Data.Setup;


namespace Quest.MasterPricing.Services.Business.Database
{
    public class DatabaseMgr : Mgr
    {
        #region Declarations
        /*==================================================================================================================================
         * Declarations
         *=================================================================================================================================*/
        private UserSession _userSession = null;
        private DbDatabaseMgr _dbDatabaseMgr = null;

        #endregion


        #region Constructors
        /*==================================================================================================================================
         * Constructors
         *=================================================================================================================================*/
        public DatabaseMgr()
            : base()
        {
            initialize();
        }
        public DatabaseMgr(UserSession userSession)
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
        public questStatus Create(Quest.Functional.MasterPricing.Database database, out DatabaseId databaseId)
        {
            // Initialize
            questStatus status = null;
            databaseId = null;


            // Create the database
            DatabasesMgr databasesMgr = new DatabasesMgr(this.UserSession);
            status = databasesMgr.Create(database, out databaseId);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }

            // Populate the database metadata
            status = RefreshSchema(databaseId);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Update(Quest.Functional.MasterPricing.Database database, bool bRefreshSchema = false)
        {
            // Initialize
            questStatus status = null;


            // Update the database
            DatabasesMgr databasesMgr = new DatabasesMgr(this.UserSession);
            status = databasesMgr.Update(database);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }

            // Populate the database metadata
            if (bRefreshSchema)
            {
                status = RefreshSchema(database);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }
            }
            return (new questStatus(Severity.Success));
        }


        public questStatus GetDatabaseMetainfo(DatabaseId databaseId, out DatabaseMetaInfo databaseMetaInfo)
        {
            // Initialize
            questStatus status = null;


            // Get database metainfo.
            status = _dbDatabaseMgr.GetDatabaseMetainfo(databaseId, out databaseMetaInfo);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus RefreshSchema(Quest.Functional.MasterPricing.Database database)
        {
            DatabaseId databaseId = new DatabaseId(database.Id);
            return (RefreshSchema(databaseId));
        }
        public questStatus RefreshSchema(DatabaseId databaseId)
        {
            // Initialize
            questStatus status = null;

            status = _dbDatabaseMgr.RefreshSchema(databaseId);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus ReadDatabaseEntities(DatabaseId databaseId, out DatabaseEntities databaseEntities)
        {
            // Initialize
            questStatus status = null;

            status = _dbDatabaseMgr.ReadDatabaseEntities(databaseId, out databaseEntities);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }

        public questStatus DeleteDatabase(DatabaseId databaseId)
        {
            // TODO: DELETE ---EVERYTHING--- RELATED TO THE DATABASE, IN A SINGLE TRANSACTION

            return (new questStatus(Severity.Warning, "Not implemented"));
        }
        #endregion


        public questStatus GetDatabaseTables(DatabaseId databaseId, out List<DBTable> dbTableList)
        {
            // Initialize 
            questStatus status = null;
            dbTableList = null;


            // Get database tables
            status = _dbDatabaseMgr.GetDatabaseTables(databaseId, out dbTableList);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus GetDatabaseViews(DatabaseId databaseId, out List<DBView> dbViewList)
        {
            // Initialize 
            questStatus status = null;
            dbViewList = null;


            // Get database views
            status = _dbDatabaseMgr.GetDatabaseViews(databaseId, out dbViewList);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus GetDatabaseColumns(DatabaseId databaseId, out Dictionary<DBTable, List<Column>> dictDBTableColumn, out Dictionary<DBView, List<Column>> dictDBViewColumn)
        {
            // Initialize 
            questStatus status = null;
            dictDBTableColumn = null;
            dictDBViewColumn = null;


            // Get database columns
            status = _dbDatabaseMgr.GetDatabaseColumns(databaseId, out dictDBTableColumn);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }

            // Get database columns
            status = _dbDatabaseMgr.GetDatabaseColumns(databaseId, out dictDBViewColumn);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus GetDatabaseColumns(DatabaseId databaseId, out Dictionary<DBTable, List<Column>> dictDBColumn)
        {
            // Initialize 
            questStatus status = null;
            dictDBColumn = null;


            // Get database columns
            status = _dbDatabaseMgr.GetDatabaseColumns(databaseId, out dictDBColumn);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus GetDatabaseColumns(DatabaseId databaseId, out Dictionary<DBView, List<Column>> dictDBColumn)
        {
            // Initialize 
            questStatus status = null;
            dictDBColumn = null;


            // Get database columns
            status = _dbDatabaseMgr.GetDatabaseColumns(databaseId, out dictDBColumn);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }

        public questStatus StoreDatabaseTables(DatabaseId databaseId, List<Table> tableList, out List<Table> tableIdList)
        {
            // Initialize
            questStatus status = null;
            tableIdList = null;


            // Store database tables
            status = _dbDatabaseMgr.StoreDatabaseTables(databaseId, tableList, out tableIdList);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus StoreDatabaseViews(DatabaseId databaseId, List<View> viewList, out List<View> viewIdList)
        {
            // Initialize
            questStatus status = null;
            viewIdList = null;


            // Store database views
            status = _dbDatabaseMgr.StoreDatabaseViews(databaseId, viewList, out viewIdList);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus StoreDatabaseColumns(DatabaseId databaseId, Dictionary<Table, List<Column>> dictDBColumn, out List<Table> tableIdList)
        {
            questStatus status = null;
            tableIdList = null;


            // Store database tables
            status = _dbDatabaseMgr.StoreDatabaseColumns(databaseId, dictDBColumn, out tableIdList);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus StoreDatabaseColumns(DatabaseId databaseId, Dictionary<View, List<Column>> dictDBColumn, out List<View> viewIdList)
        {
            // Initialize
            questStatus status = null;
            viewIdList = null;


            // Store database view columns
            status = _dbDatabaseMgr.StoreDatabaseColumns(databaseId, dictDBColumn, out viewIdList);
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


            // Delete database
            DbDatabaseMgr dbDatabaseMgr = new DbDatabaseMgr(this.UserSession);
            status = dbDatabaseMgr.Delete(databaseId);
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
                _dbDatabaseMgr = new DbDatabaseMgr(this.UserSession);
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

