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


namespace Quest.MasterPricing.Services.Data.Filters
{
    public class DbFilterItemsMgr : DbMgrSessionBased
    {
        #region Declarations
        /*==================================================================================================================================
         * Declarations
         *=================================================================================================================================*/
        #endregion


        #region Constructors
        /*==================================================================================================================================
         * Constructors
         *=================================================================================================================================*/
        public DbFilterItemsMgr(UserSession userSession)
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
        public questStatus Create(Quest.Functional.MasterPricing.FilterItem filterItem, out FilterItemId filterItemId)
        {
            // Initialize
            questStatus status = null;
            filterItemId = null;


            // Data rules.


            // Create the filterItem
            using (MasterPricingEntities dbContext = new MasterPricingEntities())
            {
                status = create(dbContext, filterItem, out filterItemId);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Create(DbMgrTransaction trans, Quest.Functional.MasterPricing.FilterItem filterItem, out FilterItemId filterItemId)
        {
            // Initialize
            questStatus status = null;
            filterItemId = null;


            // Data rules.


            // Create the filterItem in this transaction.
            status = create((MasterPricingEntities)trans.DbContext, filterItem, out filterItemId);
            if (! questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Create(DbMgrTransaction trans, List<Quest.Functional.MasterPricing.FilterItem> filterItemList, out List<Quest.Functional.MasterPricing.FilterItem> filterItemIdList)
        {
            // Initialize
            questStatus status = null;
            filterItemIdList = null;


            // Data rules.


            // Create the filterItems in this transaction.
            status = create((MasterPricingEntities)trans.DbContext, filterItemList, out filterItemIdList);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Read(FilterItemId filterItemId, out Quest.Functional.MasterPricing.FilterItem filterItem)
        {
            // Initialize
            questStatus status = null;
            filterItem = null;


            // Perform read
            using (MasterPricingEntities dbContext = new MasterPricingEntities())
            {
                Quest.Services.Dbio.MasterPricing.FilterItems _filterItems = null;
                status = read(dbContext, filterItemId, out _filterItems);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }
                filterItem = new Quest.Functional.MasterPricing.FilterItem();
                BufferMgr.TransferBuffer(_filterItems, filterItem);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Read(DbMgrTransaction trans, FilterItemId filterItemId, out Quest.Functional.MasterPricing.FilterItem filterItem)
        {
            // Initialize
            questStatus status = null;
            filterItem = null;


            // Perform read
            Quest.Services.Dbio.MasterPricing.FilterItems _filterItems = null;
            status = read((MasterPricingEntities)trans.DbContext, filterItemId, out _filterItems);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            filterItem = new Quest.Functional.MasterPricing.FilterItem();
            BufferMgr.TransferBuffer(_filterItems, filterItem);

            return (new questStatus(Severity.Success));
        }
        public questStatus Read(FilterId filterId, out List<Quest.Functional.MasterPricing.FilterItem> filterItemList)
        {
            // Initialize
            questStatus status = null;
            filterItemList = null;


            // Perform read
            using (MasterPricingEntities dbContext = new MasterPricingEntities())
            {
                List<Quest.Services.Dbio.MasterPricing.FilterItems> _filterItemsList = null;
                status = read(dbContext, filterId, out _filterItemsList);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }
                filterItemList = new List<FilterItem>();
                foreach (Quest.Services.Dbio.MasterPricing.FilterItems _filterItem in _filterItemsList)
                {
                    Quest.Functional.MasterPricing.FilterItem filterItem = new Quest.Functional.MasterPricing.FilterItem();
                    BufferMgr.TransferBuffer(_filterItem, filterItem);
                    filterItemList.Add(filterItem);
                }
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Read(DbMgrTransaction trans, FilterId filterId, out List<Quest.Functional.MasterPricing.FilterItem> filterItemList)
        {
            // Initialize
            questStatus status = null;
            filterItemList = null;


            // Perform read
            List<Quest.Services.Dbio.MasterPricing.FilterItems> _filterItemsList = null;
            status = read((MasterPricingEntities)trans.DbContext, filterId, out _filterItemsList);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            filterItemList = new List<FilterItem>();
            foreach (Quest.Services.Dbio.MasterPricing.FilterItems _filterItem in _filterItemsList)
            {
                Quest.Functional.MasterPricing.FilterItem filterItem = new Quest.Functional.MasterPricing.FilterItem();
                BufferMgr.TransferBuffer(_filterItem, filterItem);
                filterItemList.Add(filterItem);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Update(Quest.Functional.MasterPricing.FilterItem filterItem)
        {
            // Initialize
            questStatus status = null;


            // Perform update.
            using (MasterPricingEntities dbContext = new MasterPricingEntities())
            {
                status = update(dbContext, filterItem);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Update(DbMgrTransaction trans, Quest.Functional.MasterPricing.FilterItem filterItem)
        {
            // Initialize
            questStatus status = null;
            bool bCreateTransaction = trans == null;


            // Perform update in this transaction.
            status = update((MasterPricingEntities)trans.DbContext, filterItem);
            if (!questStatusDef.IsSuccess(status))
            {
                RollbackTransaction(trans);
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Delete(FilterItemId filterItemId)
        {
            // Initialize
            questStatus status = null;


            // Perform delete.
            using (MasterPricingEntities dbContext = new MasterPricingEntities())
            {
                status = delete(dbContext, filterItemId);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Delete(DbMgrTransaction trans, FilterItemId filterItemId)
        {
            // Initialize
            questStatus status = null;


            // Perform delete in this transaction.
            status = delete((MasterPricingEntities)trans.DbContext, filterItemId);
            if (! questStatusDef.IsSuccess(status))
            {
                RollbackTransaction(trans);
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Delete(FilterId filterId)
        {
            // Initialize
            questStatus status = null;


            // Perform delete.
            using (MasterPricingEntities dbContext = new MasterPricingEntities())
            {
                status = delete(dbContext, filterId);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Delete(DbMgrTransaction trans, FilterId filterId)
        {
            // Initialize
            questStatus status = null;


            // Perform delete in this transaction.
            status = delete((MasterPricingEntities)trans.DbContext, filterId);
            if (!questStatusDef.IsSuccess(status))
            {
                RollbackTransaction(trans);
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus List(QueryOptions queryOptions, out List<Quest.Functional.MasterPricing.FilterItem> filterItemList, out QueryResponse queryResponse)
        {
            // Initialize
            questStatus status = null;
            filterItemList = null;
            queryResponse = null;


            using (MasterPricingEntities dbContext = new MasterPricingEntities())
            {
                using (DbContextTransaction tx = dbContext.Database.BeginTransaction(IsolationLevel.ReadUncommitted))
                {
                    try
                    {
                        PropertyInfo[] dbProperties = typeof(Quest.Services.Dbio.MasterPricing.FilterItems).GetProperties().ToArray();
                        int totalRecords = dbContext.FilterItems.Where(BuildWhereClause(queryOptions, dbProperties)).Count();
                        List<Quest.Services.Dbio.MasterPricing.FilterItems> _countriesList = dbContext.FilterItems.Where(BuildWhereClause(queryOptions, dbProperties))
                                .OrderBy(BuildSortString(queryOptions.SortColumns))
                                .Skip(queryOptions.Paging.PageSize * (queryOptions.Paging.PageNumber - 1))
                                .Take(queryOptions.Paging.PageSize).ToList();
                        if (_countriesList == null)
                        {
                            return (new questStatus(Severity.Warning));
                        }
                        filterItemList = new List<Quest.Functional.MasterPricing.FilterItem>();
                        foreach (Quest.Services.Dbio.MasterPricing.FilterItems _filterItems in _countriesList)
                        {
                            Quest.Functional.MasterPricing.FilterItem filterItem = new Quest.Functional.MasterPricing.FilterItem();
                            BufferMgr.TransferBuffer(_filterItems, filterItem);
                            filterItemList.Add(filterItem);
                        }
                        status = BuildQueryResponse(totalRecords, queryOptions, out queryResponse);
                        if (!questStatusDef.IsSuccess(status))
                        {
                            return (status);
                        }
                    }
                    catch (System.Exception ex)
                    {
                        return (new questStatus(Severity.Fatal, String.Format("EXCEPTION: {0}.{1}: {2}",
                                this.GetType().Name, MethodBase.GetCurrentMethod().Name,
                                ex.InnerException != null ? ex.InnerException.Message : ex.Message)));
                    }
                }
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
                throw new System.Exception(status.Message, ex);
            }
            return (new questStatus(Severity.Success));
        }


        #region FilterColumns
        /*----------------------------------------------------------------------------------------------------------------------------------
         * FilterColumns
         *---------------------------------------------------------------------------------------------------------------------------------*/
        private questStatus create(MasterPricingEntities dbContext, Quest.Functional.MasterPricing.FilterItem filterItem, out FilterItemId filterItemId)
        {
            // Initialize
            filterItemId = null;


            // Perform create
            try
            {
                Quest.Services.Dbio.MasterPricing.FilterItems _filterItems = new Quest.Services.Dbio.MasterPricing.FilterItems();
                BufferMgr.TransferBuffer(filterItem, _filterItems);
                dbContext.FilterItems.Add(_filterItems);
                dbContext.SaveChanges();
                if (_filterItems.Id == 0)
                {
                    return (new questStatus(Severity.Error, "Quest.Functional.MasterPricing.FilterItem not created"));
                }
                filterItemId = new FilterItemId(_filterItems.Id);
            }
            catch (System.Exception ex)
            {
                return (new questStatus(Severity.Fatal, String.Format("EXCEPTION: {0}.{1}: {2}",
                        this.GetType().Name, MethodBase.GetCurrentMethod().Name,
                        ex.InnerException != null ? ex.InnerException.Message : ex.Message)));
            }
            return (new questStatus(Severity.Success));
        }
        private questStatus create(MasterPricingEntities dbContext, List<Quest.Functional.MasterPricing.FilterItem> filterItemList, out List<Quest.Functional.MasterPricing.FilterItem> filterItemIdList)
        {
            // Initialize
            filterItemIdList = null;


            // Perform create
            try
            {
                List<Quest.Services.Dbio.MasterPricing.FilterItems> _filterItemList = new List<Quest.Services.Dbio.MasterPricing.FilterItems>();
                foreach (Quest.Functional.MasterPricing.FilterItem filterItem in filterItemList)
                {
                    Quest.Services.Dbio.MasterPricing.FilterItems _filterItem = new Quest.Services.Dbio.MasterPricing.FilterItems();
                    BufferMgr.TransferBuffer(filterItem, _filterItem);
                    _filterItemList.Add(_filterItem);
                }
                dbContext.FilterItems.AddRange(_filterItemList);
                dbContext.SaveChanges();

                filterItemIdList = new List<FilterItem>();
                foreach (Quest.Services.Dbio.MasterPricing.FilterItems _filterItem in _filterItemList)
                {
                    Quest.Functional.MasterPricing.FilterItem filterItem = new FilterItem();
                    filterItem.Id = _filterItem.Id;
                    filterItemIdList.Add(filterItem);
                }
            }
            catch (System.Exception ex)
            {
                return (new questStatus(Severity.Fatal, String.Format("EXCEPTION: {0}.{1}: {2}",
                        this.GetType().Name, MethodBase.GetCurrentMethod().Name,
                        ex.InnerException != null ? ex.InnerException.Message : ex.Message)));
            }
            return (new questStatus(Severity.Success));
        }
        private questStatus read(MasterPricingEntities dbContext, FilterItemId filterItemId, out Quest.Services.Dbio.MasterPricing.FilterItems filterItem)
        {
            // Initialize
            filterItem = null;


            try
            {
                filterItem = dbContext.FilterItems.Where(r => r.Id == filterItemId.Id).SingleOrDefault();
                if (filterItem == null)
                {
                    return (new questStatus(Severity.Error, String.Format("ERROR: {0}.{1}: {2}",
                            this.GetType().Name, MethodBase.GetCurrentMethod().Name,
                            String.Format("Id {0} not found", filterItemId.Id))));
                }
            }
            catch (System.Exception ex)
            {
                return (new questStatus(Severity.Fatal, String.Format("EXCEPTION: {0}.{1}: {2}",
                        this.GetType().Name, MethodBase.GetCurrentMethod().Name,
                        ex.InnerException != null ? ex.InnerException.Message : ex.Message)));
            }
            return (new questStatus(Severity.Success));
        }
        private questStatus read(MasterPricingEntities dbContext, FilterId filterId, out List<Quest.Services.Dbio.MasterPricing.FilterItems> filterItemList)
        {
            // Initialize
            filterItemList = null;


            try
            {
                filterItemList = dbContext.FilterItems.Where(r => r.FilterId == filterId.Id).ToList();
                if (filterItemList == null)
                {
                    return (new questStatus(Severity.Error, String.Format("ERROR: {0}.{1}: {2}",
                            this.GetType().Name, MethodBase.GetCurrentMethod().Name,
                            String.Format("FilterId {0} not found", filterId.Id))));
                }
            }
            catch (System.Exception ex)
            {
                return (new questStatus(Severity.Fatal, String.Format("EXCEPTION: {0}.{1}: {2}",
                        this.GetType().Name, MethodBase.GetCurrentMethod().Name,
                        ex.InnerException != null ? ex.InnerException.Message : ex.Message)));
            }
            return (new questStatus(Severity.Success));
        }
        private questStatus update(MasterPricingEntities dbContext, Quest.Functional.MasterPricing.FilterItem filterItem)
        {
            // Initialize 
            questStatus status = null;


            try
            {
                // Read the record.
                FilterItemId filterItemId = new FilterItemId(filterItem.Id);
                Quest.Services.Dbio.MasterPricing.FilterItems _filterItems = null;
                status = read(dbContext, filterItemId, out _filterItems);
                if (! questStatusDef.IsSuccess(status))
                {
                    return (status);
                }

                // Update the record.
                BufferMgr.TransferBuffer(filterItem, _filterItems);
                dbContext.SaveChanges();
            }
            catch (System.Exception ex)
            {
                return (new questStatus(Severity.Fatal, String.Format("EXCEPTION: {0}.{1}: {2}",
                        this.GetType().Name, MethodBase.GetCurrentMethod().Name,
                        ex.InnerException != null ? ex.InnerException.Message : ex.Message)));
            }
            return (new questStatus(Severity.Success));
        }
        private questStatus delete(MasterPricingEntities dbContext, FilterItemId filterItemId)
        {
            try
            {
                dbContext.FilterItems.RemoveRange(dbContext.FilterItems.Where(r => r.Id == filterItemId.Id));
                dbContext.SaveChanges();
            }
            catch (System.Exception ex)
            {
                return (new questStatus(Severity.Fatal, String.Format("EXCEPTION: {0}.{1}: {2}",
                        this.GetType().Name, MethodBase.GetCurrentMethod().Name,
                        ex.InnerException != null ? ex.InnerException.Message : ex.Message)));
            }
            return (new questStatus(Severity.Success));
        }
        private questStatus delete(MasterPricingEntities dbContext, FilterId filterId)
        {
            try
            {
                dbContext.FilterItems.RemoveRange(dbContext.FilterItems.Where(r => r.FilterId == filterId.Id));
                dbContext.SaveChanges();
            }
            catch (System.Exception ex)
            {
                return (new questStatus(Severity.Fatal, String.Format("EXCEPTION: {0}.{1}: {2}",
                        this.GetType().Name, MethodBase.GetCurrentMethod().Name,
                        ex.InnerException != null ? ex.InnerException.Message : ex.Message)));
            }
            return (new questStatus(Severity.Success));
        }
        #endregion

        #endregion
    }
}
