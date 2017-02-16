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
    public class TablesetsMgr : MgrSessionBased
    {
        #region Declarations
        /*==================================================================================================================================
         * Declarations
         *=================================================================================================================================*/
        private DbTablesetsMgr _dbTablesetsMgr = null;

        #endregion


        #region Constructors
        /*==================================================================================================================================
         * Constructors
         *=================================================================================================================================*/
        public TablesetsMgr(UserSession userSession)
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
        public questStatus Create(Tableset tableset, out TablesetId tablesetId)
        {
            // Initialize
            tablesetId = null;
            questStatus status = null;


            // Create tableset
            status = _dbTablesetsMgr.Create(tableset, out tablesetId);
            if (! questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Create(DbMgrTransaction trans, Tableset tableset, out TablesetId tablesetId)
        {
            // Initialize
            tablesetId = null;
            questStatus status = null;


            // Create tableset
            status = _dbTablesetsMgr.Create(trans, tableset, out tablesetId);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Read(TablesetId tablesetId, out Tableset tableset)
        {
            // Initialize
            tableset = null;
            questStatus status = null;


            // Read tableset
            status = _dbTablesetsMgr.Read(tablesetId, out tableset);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Read(DbMgrTransaction trans, TablesetId tablesetId, out Tableset tableset)
        {
            // Initialize
            tableset = null;
            questStatus status = null;


            // Read tableset
            status = _dbTablesetsMgr.Read(trans, tablesetId, out tableset);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Update(Tableset tableset)
        {
            // Initialize
            questStatus status = null;


            // Update tableset
            status = _dbTablesetsMgr.Update(tableset);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Update(DbMgrTransaction trans, Tableset tableset)
        {
            // Initialize
            questStatus status = null;


            // Update tableset
            status = _dbTablesetsMgr.Update(trans, tableset);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Delete(TablesetId tablesetId)
        {
            // Initialize
            questStatus status = null;


            // Delete tableset
            status = _dbTablesetsMgr.Delete(tablesetId);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Delete(DbMgrTransaction trans, TablesetId tablesetId)
        {
            // Initialize
            questStatus status = null;


            // Delete tableset
            status = _dbTablesetsMgr.Delete(trans, tablesetId);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus List(QueryOptions queryOptions, out List<Tableset> tablesetList, out QueryResponse queryResponse)
        {
            // Initialize
            questStatus status = null;
            tablesetList = null;


            // List
            status = _dbTablesetsMgr.List(queryOptions, out tablesetList, out queryResponse);
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
                _dbTablesetsMgr = new DbTablesetsMgr(this.UserSession);
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

