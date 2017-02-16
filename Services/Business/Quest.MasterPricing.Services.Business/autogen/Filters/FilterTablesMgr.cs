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
    public class FilterTablesMgr : MgrSessionBased
    {
        #region Declarations
        /*==================================================================================================================================
         * Declarations
         *=================================================================================================================================*/
        private DbFilterTablesMgr _dbFilterTablesMgr = null;

        #endregion


        #region Constructors
        /*==================================================================================================================================
         * Constructors
         *=================================================================================================================================*/
        public FilterTablesMgr(UserSession userSession)
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
        public questStatus Create(FilterTable filterTable, out FilterTableId filterTableId)
        {
            // Initialize
            questStatus status = null;
            filterTableId = null;


            // Create filterTable
            status = _dbFilterTablesMgr.Create(filterTable, out filterTableId);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Create(DbMgrTransaction trans, FilterTable filterTable, out FilterTableId filterTableId)
        {
            // Initialize
            questStatus status = null;
            filterTableId = null;


            // Create filterTable
            status = _dbFilterTablesMgr.Create(trans, filterTable, out filterTableId);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Create(DbMgrTransaction trans, List<Quest.Functional.MasterPricing.FilterTable> filterTableList, out List<Quest.Functional.MasterPricing.FilterTable> filterTableIdList)
        {
            // Initialize
            questStatus status = null;
            filterTableIdList = null;


            // Create filterTable
            status = _dbFilterTablesMgr.Create(trans, filterTableList, out filterTableIdList);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Read(FilterTableId filterTableId, out FilterTable filterTable)
        {
            // Initialize
            questStatus status = null;
            filterTable = null;


            // Read filterTable
            status = _dbFilterTablesMgr.Read(filterTableId, out filterTable);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Read(DbMgrTransaction trans, FilterTableId filterTableId, out FilterTable filterTable)
        {
            // Initialize
            questStatus status = null;
            filterTable = null;


            // Read filterTable
            status = _dbFilterTablesMgr.Read(trans, filterTableId, out filterTable);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Update(FilterTable filterTable)
        {
            // Initialize
            questStatus status = null;


            // Update filterTable
            status = _dbFilterTablesMgr.Update(filterTable);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Update(DbMgrTransaction trans, FilterTable filterTable)
        {
            // Initialize
            questStatus status = null;


            // Update filterTable
            status = _dbFilterTablesMgr.Update(trans, filterTable);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Delete(FilterTableId filterTableId)
        {
            // Initialize
            questStatus status = null;


            // Delete filterTable
            status = _dbFilterTablesMgr.Delete(filterTableId);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Delete(DbMgrTransaction trans, FilterTableId filterTableId)
        {
            // Initialize
            questStatus status = null;


            // Delete filterTable
            status = _dbFilterTablesMgr.Delete(trans, filterTableId);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Delete(FilterId filterId)
        {
            // Initialize
            questStatus status = null;


            // Delete all tableTables in this filter.
            status = _dbFilterTablesMgr.Delete(filterId);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Delete(DbMgrTransaction trans, FilterId filterId)
        {
            // Initialize
            questStatus status = null;


            // Delete all tableTables in this table.
            status = _dbFilterTablesMgr.Delete(trans, filterId);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus List(QueryOptions queryOptions, out List<FilterTable> filterTableList, out QueryResponse queryResponse)
        {
            // Initialize
            questStatus status = null;
            filterTableList = null;


            // List
            status = _dbFilterTablesMgr.List(queryOptions, out filterTableList, out queryResponse);
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
                _dbFilterTablesMgr = new DbFilterTablesMgr(this.UserSession);
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

