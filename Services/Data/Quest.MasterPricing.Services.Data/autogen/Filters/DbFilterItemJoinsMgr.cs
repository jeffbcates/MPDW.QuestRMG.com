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
    public class DbFilterItemJoinsMgr : DbMgrSessionBased
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
        public DbFilterItemJoinsMgr(UserSession userSession)
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
        public questStatus Create(Quest.Functional.MasterPricing.FilterItemJoin filterItemJoin, out FilterItemJoinId filterItemJoinId)
        {
            // Initialize
            questStatus status = null;
            filterItemJoinId = null;


            // Data rules.


            // Create the filterItemJoin
            using (MasterPricingEntities dbContext = new MasterPricingEntities())
            {
                status = create(dbContext, filterItemJoin, out filterItemJoinId);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Create(DbMgrTransaction trans, Quest.Functional.MasterPricing.FilterItemJoin filterItemJoin, out FilterItemJoinId filterItemJoinId)
        {
            // Initialize
            questStatus status = null;
            filterItemJoinId = null;


            // Data rules.


            // Create the filterItemJoin in this transaction.
            status = create((MasterPricingEntities)trans.DbContext, filterItemJoin, out filterItemJoinId);
            if (! questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Create(DbMgrTransaction trans, List<Quest.Functional.MasterPricing.FilterItemJoin> filterItemJoinList, out List<Quest.Functional.MasterPricing.FilterItemJoin> filterItemJoinIdList)
        {
            // Initialize
            questStatus status = null;
            filterItemJoinIdList = null;


            // Data rules.


            // Create the filterItemJoins in this transaction.
            status = create((MasterPricingEntities)trans.DbContext, filterItemJoinList, out filterItemJoinIdList);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Read(FilterItemJoinId filterItemJoinId, out Quest.Functional.MasterPricing.FilterItemJoin filterItemJoin)
        {
            // Initialize
            questStatus status = null;
            filterItemJoin = null;


            // Perform read
            using (MasterPricingEntities dbContext = new MasterPricingEntities())
            {
                Quest.Services.Dbio.MasterPricing.FilterItemJoins _filterItemJoins = null;
                status = read(dbContext, filterItemJoinId, out _filterItemJoins);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }
                filterItemJoin = new Quest.Functional.MasterPricing.FilterItemJoin();
                BufferMgr.TransferBuffer(_filterItemJoins, filterItemJoin);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Read(DbMgrTransaction trans, FilterItemJoinId filterItemJoinId, out Quest.Functional.MasterPricing.FilterItemJoin filterItemJoin)
        {
            // Initialize
            questStatus status = null;
            filterItemJoin = null;


            // Perform read
            Quest.Services.Dbio.MasterPricing.FilterItemJoins _filterItemJoins = null;
            status = read((MasterPricingEntities)trans.DbContext, filterItemJoinId, out _filterItemJoins);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            filterItemJoin = new Quest.Functional.MasterPricing.FilterItemJoin();
            BufferMgr.TransferBuffer(_filterItemJoins, filterItemJoin);

            return (new questStatus(Severity.Success));
        }
        public questStatus Read(FilterItemId filterItemId, out List<Quest.Functional.MasterPricing.FilterItemJoin> filterItemJoinList)
        {
            // Initialize
            questStatus status = null;
            filterItemJoinList = null;


            // Perform read
            using (MasterPricingEntities dbContext = new MasterPricingEntities())
            {
                List<Quest.Services.Dbio.MasterPricing.FilterItemJoins> _filterItemJoinsList = null;
                status = read(dbContext, filterItemId, out _filterItemJoinsList);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }
                filterItemJoinList = new List<FilterItemJoin>();
                foreach (Quest.Services.Dbio.MasterPricing.FilterItemJoins _filterItemJoin in _filterItemJoinsList)
                {
                    Quest.Functional.MasterPricing.FilterItemJoin filterItemJoin = new Quest.Functional.MasterPricing.FilterItemJoin();
                    BufferMgr.TransferBuffer(_filterItemJoin, filterItemJoin);
                    filterItemJoinList.Add(filterItemJoin);
                }
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Read(DbMgrTransaction trans, FilterItemId filterItemId, out List<Quest.Functional.MasterPricing.FilterItemJoin> filterItemJoinList)
        {
            // Initialize
            questStatus status = null;
            filterItemJoinList = null;


            // Perform read
            List<Quest.Services.Dbio.MasterPricing.FilterItemJoins> _filterItemJoinsList = null;
            status = read((MasterPricingEntities)trans.DbContext, filterItemId, out _filterItemJoinsList);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            filterItemJoinList = new List<FilterItemJoin>();
            foreach (Quest.Services.Dbio.MasterPricing.FilterItemJoins _filterItemJoin in _filterItemJoinsList)
            {
                Quest.Functional.MasterPricing.FilterItemJoin filterItemJoin = new Quest.Functional.MasterPricing.FilterItemJoin();
                BufferMgr.TransferBuffer(_filterItemJoin, filterItemJoin);
                filterItemJoinList.Add(filterItemJoin);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Update(Quest.Functional.MasterPricing.FilterItemJoin filterItemJoin)
        {
            // Initialize
            questStatus status = null;


            // Perform update.
            using (MasterPricingEntities dbContext = new MasterPricingEntities())
            {
                status = update(dbContext, filterItemJoin);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Update(DbMgrTransaction trans, Quest.Functional.MasterPricing.FilterItemJoin filterItemJoin)
        {
            // Initialize
            questStatus status = null;
            bool bCreateTransaction = trans == null;


            // Perform update in this transaction.
            status = update((MasterPricingEntities)trans.DbContext, filterItemJoin);
            if (!questStatusDef.IsSuccess(status))
            {
                RollbackTransaction(trans);
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Delete(FilterItemJoinId filterItemJoinId)
        {
            // Initialize
            questStatus status = null;


            // Perform delete.
            using (MasterPricingEntities dbContext = new MasterPricingEntities())
            {
                status = delete(dbContext, filterItemJoinId);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Delete(DbMgrTransaction trans, FilterItemJoinId filterItemJoinId)
        {
            // Initialize
            questStatus status = null;


            // Perform delete in this transaction.
            status = delete((MasterPricingEntities)trans.DbContext, filterItemJoinId);
            if (! questStatusDef.IsSuccess(status))
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
            if (!questStatusDef.IsSuccess(status))
            {
                RollbackTransaction(trans);
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus List(QueryOptions queryOptions, out List<Quest.Functional.MasterPricing.FilterItemJoin> filterItemJoinList, out QueryResponse queryResponse)
        {
            // Initialize
            questStatus status = null;
            filterItemJoinList = null;
            queryResponse = null;


            using (MasterPricingEntities dbContext = new MasterPricingEntities())
            {
                using (DbContextTransaction tx = dbContext.Database.BeginTransaction(IsolationLevel.ReadUncommitted))
                {
                    try
                    {
                        PropertyInfo[] dbProperties = typeof(Quest.Services.Dbio.MasterPricing.FilterItemJoins).GetProperties().ToArray();
                        int totalRecords = dbContext.FilterItemJoins.Where(BuildWhereClause(queryOptions, dbProperties)).Count();
                        List<Quest.Services.Dbio.MasterPricing.FilterItemJoins> _countriesList = dbContext.FilterItemJoins.Where(BuildWhereClause(queryOptions, dbProperties))
                                .OrderBy(BuildSortString(queryOptions.SortColumns))
                                .Skip(queryOptions.Paging.PageSize * (queryOptions.Paging.PageNumber - 1))
                                .Take(queryOptions.Paging.PageSize).ToList();
                        if (_countriesList == null)
                        {
                            return (new questStatus(Severity.Warning));
                        }
                        filterItemJoinList = new List<Quest.Functional.MasterPricing.FilterItemJoin>();
                        foreach (Quest.Services.Dbio.MasterPricing.FilterItemJoins _filterItemJoins in _countriesList)
                        {
                            Quest.Functional.MasterPricing.FilterItemJoin filterItemJoin = new Quest.Functional.MasterPricing.FilterItemJoin();
                            BufferMgr.TransferBuffer(_filterItemJoins, filterItemJoin);
                            filterItemJoinList.Add(filterItemJoin);
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
        private questStatus create(MasterPricingEntities dbContext, Quest.Functional.MasterPricing.FilterItemJoin filterItemJoin, out FilterItemJoinId filterItemJoinId)
        {
            // Initialize
            filterItemJoinId = null;


            // Perform create
            try
            {
                Quest.Services.Dbio.MasterPricing.FilterItemJoins _filterItemJoins = new Quest.Services.Dbio.MasterPricing.FilterItemJoins();
                BufferMgr.TransferBuffer(filterItemJoin, _filterItemJoins);
                dbContext.FilterItemJoins.Add(_filterItemJoins);
                dbContext.SaveChanges();
                if (_filterItemJoins.Id == 0)
                {
                    return (new questStatus(Severity.Error, "Quest.Functional.MasterPricing.FilterItemJoin not created"));
                }
                filterItemJoinId = new FilterItemJoinId(_filterItemJoins.Id);
            }
            catch (System.Exception ex)
            {
                return (new questStatus(Severity.Fatal, String.Format("EXCEPTION: {0}.{1}: {2}",
                        this.GetType().Name, MethodBase.GetCurrentMethod().Name,
                        ex.InnerException != null ? ex.InnerException.Message : ex.Message)));
            }
            return (new questStatus(Severity.Success));
        }
        private questStatus create(MasterPricingEntities dbContext, List<Quest.Functional.MasterPricing.FilterItemJoin> filterItemJoinList, out List<Quest.Functional.MasterPricing.FilterItemJoin> filterItemJoinIdList)
        {
            // Initialize
            filterItemJoinIdList = null;


            // Perform create
            try
            {
                List<Quest.Services.Dbio.MasterPricing.FilterItemJoins> _filterItemJoinList = new List<Quest.Services.Dbio.MasterPricing.FilterItemJoins>();
                foreach (Quest.Functional.MasterPricing.FilterItemJoin filterItemJoin in filterItemJoinList)
                {
                    Quest.Services.Dbio.MasterPricing.FilterItemJoins _filterItemJoin = new Quest.Services.Dbio.MasterPricing.FilterItemJoins();
                    BufferMgr.TransferBuffer(filterItemJoin, _filterItemJoin);
                    _filterItemJoinList.Add(_filterItemJoin);
                }
                dbContext.FilterItemJoins.AddRange(_filterItemJoinList);
                dbContext.SaveChanges();

                filterItemJoinIdList = new List<FilterItemJoin>();
                foreach (Quest.Services.Dbio.MasterPricing.FilterItemJoins _filterItemJoin in _filterItemJoinList)
                {
                    Quest.Functional.MasterPricing.FilterItemJoin filterItemJoin = new FilterItemJoin();
                    filterItemJoin.Id = _filterItemJoin.Id;
                    filterItemJoinIdList.Add(filterItemJoin);
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
        private questStatus read(MasterPricingEntities dbContext, FilterItemJoinId filterItemJoinId, out Quest.Services.Dbio.MasterPricing.FilterItemJoins filterItemJoin)
        {
            // Initialize
            filterItemJoin = null;


            try
            {
                filterItemJoin = dbContext.FilterItemJoins.Where(r => r.Id == filterItemJoinId.Id).SingleOrDefault();
                if (filterItemJoin == null)
                {
                    return (new questStatus(Severity.Error, String.Format("ERROR: {0}.{1}: {2}",
                            this.GetType().Name, MethodBase.GetCurrentMethod().Name,
                            String.Format("Id {0} not found", filterItemJoinId.Id))));
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
        private questStatus read(MasterPricingEntities dbContext, FilterItemId filterItemId, out List<Quest.Services.Dbio.MasterPricing.FilterItemJoins> filterItemJoinList)
        {
            // Initialize
            filterItemJoinList = null;


            try
            {
                filterItemJoinList = dbContext.FilterItemJoins.Where(r => r.FilterItemId == filterItemId.Id).ToList();
                if (filterItemJoinList == null)
                {
                    return (new questStatus(Severity.Error, String.Format("ERROR: {0}.{1}: {2}",
                            this.GetType().Name, MethodBase.GetCurrentMethod().Name,
                            String.Format("FilterItemId {0} not found", filterItemId.Id))));
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
        private questStatus update(MasterPricingEntities dbContext, Quest.Functional.MasterPricing.FilterItemJoin filterItemJoin)
        {
            // Initialize 
            questStatus status = null;


            try
            {
                // Read the record.
                FilterItemJoinId filterItemJoinId = new FilterItemJoinId(filterItemJoin.Id);
                Quest.Services.Dbio.MasterPricing.FilterItemJoins _filterItemJoins = null;
                status = read(dbContext, filterItemJoinId, out _filterItemJoins);
                if (! questStatusDef.IsSuccess(status))
                {
                    return (status);
                }

                // Update the record.
                BufferMgr.TransferBuffer(filterItemJoin, _filterItemJoins);
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
        private questStatus delete(MasterPricingEntities dbContext, FilterItemJoinId filterItemJoinId)
        {
            try
            {
                dbContext.FilterItemJoins.RemoveRange(dbContext.FilterItemJoins.Where(r => r.Id == filterItemJoinId.Id));
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
                dbContext.FilterItemJoins.RemoveRange(dbContext.FilterItemJoins.Where(r => r.FilterItemId == filterItemId.Id));
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
