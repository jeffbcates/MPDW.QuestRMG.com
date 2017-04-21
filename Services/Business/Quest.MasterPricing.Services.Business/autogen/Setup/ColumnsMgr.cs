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
    public class ColumnsMgr : MgrSessionBased
    {
        #region Declarations
        /*==================================================================================================================================
         * Declarations
         *=================================================================================================================================*/
        private DbColumnsMgr _dbColumnsMgr = null;

        #endregion


        #region Constructors
        /*==================================================================================================================================
         * Constructors
         *=================================================================================================================================*/
        public ColumnsMgr(UserSession userSession)
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
        public questStatus Create(Column column, out ColumnId columnId)
        {
            // Initialize
            questStatus status = null;
            columnId = null;


            // Create column
            status = _dbColumnsMgr.Create(column, out columnId);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Create(DbMgrTransaction trans, Column column, out ColumnId columnId)
        {
            // Initialize
            questStatus status = null;
            columnId = null;


            // Create column
            status = _dbColumnsMgr.Create(trans, column, out columnId);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Create(DbMgrTransaction trans, List<Quest.Functional.MasterPricing.Column> columnList, out List<Quest.Functional.MasterPricing.Column> columnIdList)
        {
            // Initialize
            questStatus status = null;
            columnIdList = null;


            // Create column
            status = _dbColumnsMgr.Create(trans, columnList, out columnIdList);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Read(ColumnId columnId, out Column column)
        {
            // Initialize
            questStatus status = null;
            column = null;


            // Read column
            status = _dbColumnsMgr.Read(columnId, out column);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Read(DbMgrTransaction trans, ColumnId columnId, out Column column)
        {
            // Initialize
            questStatus status = null;
            column = null;


            // Read column
            status = _dbColumnsMgr.Read(trans, columnId, out column);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Read(TableId tableId, out List<Quest.Functional.MasterPricing.Column> columnList)
        {
            // Initialize
            questStatus status = null;
            columnList = null;


            // Read column
            status = _dbColumnsMgr.Read(tableId, out columnList);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Read(DbMgrTransaction trans, TableId tableId, out List<Quest.Functional.MasterPricing.Column> columnList)
        {
            // Initialize
            questStatus status = null;
            columnList = null;


            // Read column
            status = _dbColumnsMgr.Read(trans, tableId, out columnList);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Read(ViewId viewId, out List<Quest.Functional.MasterPricing.Column> columnList)
        {
            // Initialize
            questStatus status = null;
            columnList = null;


            // Read column
            status = _dbColumnsMgr.Read(viewId, out columnList);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Read(DbMgrTransaction trans, ViewId viewId, out List<Quest.Functional.MasterPricing.Column> columnList)
        {
            // Initialize
            questStatus status = null;
            columnList = null;


            // Read column
            status = _dbColumnsMgr.Read(trans, viewId, out columnList);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }


        public questStatus Update(Column column)
        {
            // Initialize
            questStatus status = null;


            // Update column
            status = _dbColumnsMgr.Update(column);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Update(DbMgrTransaction trans, Column column)
        {
            // Initialize
            questStatus status = null;


            // Update column
            status = _dbColumnsMgr.Update(trans, column);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Delete(ColumnId columnId)
        {
            // Initialize
            questStatus status = null;


            // Delete column
            status = _dbColumnsMgr.Delete(columnId);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Delete(DbMgrTransaction trans, ColumnId columnId)
        {
            // Initialize
            questStatus status = null;


            // Delete column
            status = _dbColumnsMgr.Delete(trans, columnId);
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


            // Delete all columns in this table.
            status = _dbColumnsMgr.Delete(tableId);
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


            // Delete all columns in this table.
            status = _dbColumnsMgr.Delete(trans, tableId);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Delete(ViewId viewId)
        {
            // Initialize
            questStatus status = null;


            // Delete all columns in this view.
            status = _dbColumnsMgr.Delete(viewId);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Delete(DbMgrTransaction trans, ViewId viewId)
        {
            // Initialize
            questStatus status = null;


            // Delete all columns in this view.
            status = _dbColumnsMgr.Delete(trans, viewId);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus List(QueryOptions queryOptions, out List<Column> columnList, out QueryResponse queryResponse)
        {
            // Initialize
            questStatus status = null;
            columnList = null;


            // List
            status = _dbColumnsMgr.List(queryOptions, out columnList, out queryResponse);
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
                _dbColumnsMgr = new DbColumnsMgr(this.UserSession);
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

