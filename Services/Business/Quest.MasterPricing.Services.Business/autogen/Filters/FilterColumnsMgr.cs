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
using Quest.MasterPricing.Services.Data.Filters;


namespace Quest.MasterPricing.Services.Business.Filters
{
    public class FilterColumnsMgr : MgrSessionBased
    {
        #region Declarations
        /*==================================================================================================================================
         * Declarations
         *=================================================================================================================================*/
        private DbFilterColumnsMgr _dbFilterColumnsMgr = null;

        #endregion


        #region Constructors
        /*==================================================================================================================================
         * Constructors
         *=================================================================================================================================*/
        public FilterColumnsMgr(UserSession userSession)
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
        public questStatus Create(FilterColumn filterColumn, out FilterColumnId filterColumnId)
        {
            // Initialize
            questStatus status = null;
            filterColumnId = null;


            // Create filterColumn
            status = _dbFilterColumnsMgr.Create(filterColumn, out filterColumnId);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Create(DbMgrTransaction trans, FilterColumn filterColumn, out FilterColumnId filterColumnId)
        {
            // Initialize
            questStatus status = null;
            filterColumnId = null;


            // Create filterColumn
            status = _dbFilterColumnsMgr.Create(trans, filterColumn, out filterColumnId);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Create(DbMgrTransaction trans, List<Quest.Functional.MasterPricing.FilterColumn> filterColumnList, out List<Quest.Functional.MasterPricing.FilterColumn> filterColumnIdList)
        {
            // Initialize
            questStatus status = null;
            filterColumnIdList = null;


            // Create filterColumn
            status = _dbFilterColumnsMgr.Create(trans, filterColumnList, out filterColumnIdList);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Read(FilterColumnId filterColumnId, out FilterColumn filterColumn)
        {
            // Initialize
            questStatus status = null;
            filterColumn = null;


            // Read filterColumn
            status = _dbFilterColumnsMgr.Read(filterColumnId, out filterColumn);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Read(DbMgrTransaction trans, FilterColumnId filterColumnId, out FilterColumn filterColumn)
        {
            // Initialize
            questStatus status = null;
            filterColumn = null;


            // Read filterColumn
            status = _dbFilterColumnsMgr.Read(trans, filterColumnId, out filterColumn);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Update(FilterColumn filterColumn)
        {
            // Initialize
            questStatus status = null;


            // Update filterColumn
            status = _dbFilterColumnsMgr.Update(filterColumn);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Update(DbMgrTransaction trans, FilterColumn filterColumn)
        {
            // Initialize
            questStatus status = null;


            // Update filterColumn
            status = _dbFilterColumnsMgr.Update(trans, filterColumn);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Delete(FilterColumnId filterColumnId)
        {
            // Initialize
            questStatus status = null;


            // Delete filterColumn
            status = _dbFilterColumnsMgr.Delete(filterColumnId);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Delete(DbMgrTransaction trans, FilterColumnId filterColumnId)
        {
            // Initialize
            questStatus status = null;


            // Delete filterColumn
            status = _dbFilterColumnsMgr.Delete(trans, filterColumnId);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Delete(FilterId filterId, FilterTableId filterTableId)
        {
            // Initialize
            questStatus status = null;


            // Delete all tableTables in this table.
            status = _dbFilterColumnsMgr.Delete(filterId, filterTableId);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Delete(DbMgrTransaction trans, FilterId filterId, FilterTableId filterTableId)
        {
            // Initialize
            questStatus status = null;


            // Delete all tableTables in this table.
            status = _dbFilterColumnsMgr.Delete(trans, filterId, filterTableId);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Delete(FilterId filterId, FilterViewId filterViewId)
        {
            // Initialize
            questStatus status = null;


            // Delete all tableTables in this table.
            status = _dbFilterColumnsMgr.Delete(filterId, filterViewId);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Delete(DbMgrTransaction trans, FilterId filterId, FilterViewId filterViewId)
        {
            // Initialize
            questStatus status = null;


            // Delete all tableTables in this table.
            status = _dbFilterColumnsMgr.Delete(trans, filterId, filterViewId);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus List(QueryOptions queryOptions, out List<FilterColumn> filterColumnList, out QueryResponse queryResponse)
        {
            // Initialize
            questStatus status = null;
            filterColumnList = null;


            // List
            status = _dbFilterColumnsMgr.List(queryOptions, out filterColumnList, out queryResponse);
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
                _dbFilterColumnsMgr = new DbFilterColumnsMgr(this.UserSession);
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

