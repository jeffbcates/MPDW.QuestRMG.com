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
using Quest.MasterPricing.Services.Data.Filters;


namespace Quest.MasterPricing.Services.Business.Filters
{
    public class FiltersMgr : MgrSessionBased
    {
        #region Declarations
        /*==================================================================================================================================
         * Declarations
         *=================================================================================================================================*/
        private DbFiltersMgr _dbFiltersMgr = null;

        #endregion


        #region Constructors
        /*==================================================================================================================================
         * Constructors
         *=================================================================================================================================*/
        public FiltersMgr(UserSession userSession)
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
        public questStatus Create(Filter filter, out FilterId filterId)
        {
            // Initialize
            questStatus status = null;
            filterId = null;


            // Create filter
            status = _dbFiltersMgr.Create(filter, out filterId);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Create(DbMgrTransaction trans, Filter filter, out FilterId filterId)
        {
            // Initialize
            questStatus status = null;
            filterId = null;


            // Create filter
            status = _dbFiltersMgr.Create(trans, filter, out filterId);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Read(FilterId filterId, out Filter filter)
        {
            // Initialize
            questStatus status = null;
            filter = null;


            // Read filter
            status = _dbFiltersMgr.Read(filterId, out filter);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Read(DbMgrTransaction trans, FilterId filterId, out Filter filter)
        {
            // Initialize
            questStatus status = null;
            filter = null;


            // Read filter
            status = _dbFiltersMgr.Read(trans, filterId, out filter);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Read(TablesetId tablesetId, out List<Quest.Functional.MasterPricing.Filter> filterList)
        {
            // Initialize
            questStatus status = null;
            filterList = null;


            // Read filter
            status = _dbFiltersMgr.Read(tablesetId, out filterList);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Read(DbMgrTransaction trans, TablesetId tablesetId, out List<Quest.Functional.MasterPricing.Filter> filterList)
        {
            // Initialize
            questStatus status = null;
            filterList = null;


            // Read filter
            status = _dbFiltersMgr.Read(trans, tablesetId, out filterList);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Read(string name, out Quest.Functional.MasterPricing.Filter filter)
        {
            // Initialize
            questStatus status = null;
            filter = null;


            // Read filter
            status = _dbFiltersMgr.Read(name, out filter);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Read(DbMgrTransaction trans, string name, out Quest.Functional.MasterPricing.Filter filter)
        {
            // Initialize
            questStatus status = null;
            filter = null;


            // Read filter
            status = _dbFiltersMgr.Read(trans, name, out filter);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Read(FolderId folderId, out List<Quest.Functional.MasterPricing.Filter> filterList)
        {
            // Initialize
            questStatus status = null;
            filterList = null;


            // Read filter
            status = _dbFiltersMgr.Read(folderId, out filterList);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Read(DbMgrTransaction trans, FolderId folderId, out List<Quest.Functional.MasterPricing.Filter> filterList)
        {
            // Initialize
            questStatus status = null;
            filterList = null;


            // Read filter
            status = _dbFiltersMgr.Read(trans, folderId, out filterList);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Update(Filter filter)
        {
            // Initialize
            questStatus status = null;


            // Update filter
            status = _dbFiltersMgr.Update(filter);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Update(DbMgrTransaction trans, Filter filter)
        {
            // Initialize
            questStatus status = null;


            // Update filter
            status = _dbFiltersMgr.Update(trans, filter);
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

            // Delete filter
            DbFilterMgr dbFilterMgr = new DbFilterMgr(this.UserSession);
            status = dbFilterMgr.Delete(filterId);
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


            // Delete filter
            status = _dbFiltersMgr.Delete(trans, filterId);
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


            // Delete all filters in this table.
            status = _dbFiltersMgr.Delete(tablesetId);
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


            // Delete all filters in this table.
            status = _dbFiltersMgr.Delete(trans, tablesetId);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus List(QueryOptions queryOptions, out List<Filter> filterList, out QueryResponse queryResponse)
        {
            // Initialize
            questStatus status = null;
            filterList = null;


            // List
            status = _dbFiltersMgr.List(queryOptions, out filterList, out queryResponse);
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
                _dbFiltersMgr = new DbFiltersMgr(this.UserSession);
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

