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
    public class DbFiltersMgr : DbMgrSessionBased
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
        public DbFiltersMgr(UserSession userSession)
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
        public questStatus Create(Quest.Functional.MasterPricing.Filter filter, out FilterId filterId)
        {
            // Initialize
            questStatus status = null;
            filterId = null;


            // Data rules.


            // Create the filter
            using (MasterPricingEntities dbContext = new MasterPricingEntities())
            {
                status = create(dbContext, filter, out filterId);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Create(DbMgrTransaction trans, Quest.Functional.MasterPricing.Filter filter, out FilterId filterId)
        {
            // Initialize
            questStatus status = null;
            filterId = null;


            // Data rules.


            // Create the filter in this transaction.
            status = create((MasterPricingEntities)trans.DbContext, filter, out filterId);
            if (! questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Create(DbMgrTransaction trans, List<Quest.Functional.MasterPricing.Filter> filterList, out List<Quest.Functional.MasterPricing.Filter> filterIdList)
        {
            // Initialize
            questStatus status = null;
            filterIdList = null;


            // Data rules.


            // Create the filters in this transaction.
            status = create((MasterPricingEntities)trans.DbContext, filterList, out filterIdList);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Read(FilterId filterId, out Quest.Functional.MasterPricing.Filter filter)
        {
            // Initialize
            questStatus status = null;
            filter = null;


            // Perform read
            using (MasterPricingEntities dbContext = new MasterPricingEntities())
            {
                Quest.Services.Dbio.MasterPricing.Filters _filters = null;
                status = read(dbContext, filterId, out _filters);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }
                filter = new Quest.Functional.MasterPricing.Filter();
                BufferMgr.TransferBuffer(_filters, filter);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Read(DbMgrTransaction trans, FilterId filterId, out Quest.Functional.MasterPricing.Filter filter)
        {
            // Initialize
            questStatus status = null;
            filter = null;


            // Perform read
            Quest.Services.Dbio.MasterPricing.Filters _filters = null;
            status = read((MasterPricingEntities)trans.DbContext, filterId, out _filters);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            filter = new Quest.Functional.MasterPricing.Filter();
            BufferMgr.TransferBuffer(_filters, filter);

            return (new questStatus(Severity.Success));
        }
        public questStatus Read(TablesetId tablesetId, out List<Quest.Functional.MasterPricing.Filter> filterList)
        {
            // Initialize
            questStatus status = null;
            filterList = null;


            // Perform read
            using (MasterPricingEntities dbContext = new MasterPricingEntities())
            {
                List<Quest.Services.Dbio.MasterPricing.Filters> _filtersList = null;
                status = read(dbContext, tablesetId, out _filtersList);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }
                filterList = new List<Filter>();
                foreach (Quest.Services.Dbio.MasterPricing.Filters _filter in _filtersList)
                {
                    Quest.Functional.MasterPricing.Filter filter = new Quest.Functional.MasterPricing.Filter();
                    BufferMgr.TransferBuffer(_filter, filter);
                    filterList.Add(filter);
                }
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Read(DbMgrTransaction trans, TablesetId tablesetId, out List<Quest.Functional.MasterPricing.Filter> filterList)
        {
            // Initialize
            questStatus status = null;
            filterList = null;


            // Perform read
            List<Quest.Services.Dbio.MasterPricing.Filters> _filtersList = null;
            status = read((MasterPricingEntities)trans.DbContext, tablesetId, out _filtersList);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            filterList = new List<Filter>();
            foreach (Quest.Services.Dbio.MasterPricing.Filters _filter in _filtersList)
            {
                Quest.Functional.MasterPricing.Filter filter = new Quest.Functional.MasterPricing.Filter();
                BufferMgr.TransferBuffer(_filter, filter);
                filterList.Add(filter);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Read(string name, out Quest.Functional.MasterPricing.Filter filter)
        {
            // Initialize
            questStatus status = null;
            filter = null;


            // Perform read
            using (MasterPricingEntities dbContext = new MasterPricingEntities())
            {
                Quest.Services.Dbio.MasterPricing.Filters _filters = null;
                status = read(dbContext, name, out _filters);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }
                filter = new Quest.Functional.MasterPricing.Filter();
                BufferMgr.TransferBuffer(_filters, filter);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Read(DbMgrTransaction trans, string name, out Quest.Functional.MasterPricing.Filter filter)
        {
            // Initialize
            questStatus status = null;
            filter = null;


            // Perform read
            Quest.Services.Dbio.MasterPricing.Filters _filters = null;
            status = read((MasterPricingEntities)trans.DbContext, name, out _filters);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            filter = new Quest.Functional.MasterPricing.Filter();
            BufferMgr.TransferBuffer(_filters, filter);
            
            return (new questStatus(Severity.Success));
        }
        public questStatus Update(Quest.Functional.MasterPricing.Filter filter)
        {
            // Initialize
            questStatus status = null;


            // Perform update.
            using (MasterPricingEntities dbContext = new MasterPricingEntities())
            {
                status = update(dbContext, filter);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Update(DbMgrTransaction trans, Quest.Functional.MasterPricing.Filter filter)
        {
            // Initialize
            questStatus status = null;
            bool bCreateTransaction = trans == null;


            // Perform update in this transaction.
            status = update((MasterPricingEntities)trans.DbContext, filter);
            if (!questStatusDef.IsSuccess(status))
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
            if (! questStatusDef.IsSuccess(status))
            {
                RollbackTransaction(trans);
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Delete(TablesetId tablesetId)
        {
            // Initialize
            questStatus status = null;


            // Perform delete.
            using (MasterPricingEntities dbContext = new MasterPricingEntities())
            {
                status = delete(dbContext, tablesetId);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Delete(DbMgrTransaction trans, TablesetId tablesetId)
        {
            // Initialize
            questStatus status = null;


            // Perform delete in this transaction.
            status = delete((MasterPricingEntities)trans.DbContext, tablesetId);
            if (!questStatusDef.IsSuccess(status))
            {
                RollbackTransaction(trans);
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus List(QueryOptions queryOptions, out List<Quest.Functional.MasterPricing.Filter> filterList, out QueryResponse queryResponse)
        {
            // Initialize
            questStatus status = null;
            filterList = null;
            queryResponse = null;


            using (MasterPricingEntities dbContext = new MasterPricingEntities())
            {
                using (DbContextTransaction tx = dbContext.Database.BeginTransaction(IsolationLevel.ReadUncommitted))
                {
                    try
                    {
                        PropertyInfo[] dbProperties = typeof(Quest.Services.Dbio.MasterPricing.Filters).GetProperties().ToArray();
                        int totalRecords = dbContext.Filters.Where(BuildWhereClause(queryOptions, dbProperties)).Count();
                        List<Quest.Services.Dbio.MasterPricing.Filters> _filtersList = dbContext.Filters.Where(BuildWhereClause(queryOptions, dbProperties))
                                .OrderBy(BuildSortString(queryOptions.SortColumns))
                                .Skip(queryOptions.Paging.PageSize * (queryOptions.Paging.PageNumber - 1))
                                .Take(queryOptions.Paging.PageSize).ToList();
                        if (_filtersList == null)
                        {
                            return (new questStatus(Severity.Warning));
                        }
                        filterList = new List<Quest.Functional.MasterPricing.Filter>();
                        foreach (Quest.Services.Dbio.MasterPricing.Filters _filters in _filtersList)
                        {
                            Quest.Functional.MasterPricing.Filter filter = new Quest.Functional.MasterPricing.Filter();
                            BufferMgr.TransferBuffer(_filters, filter);
                            filterList.Add(filter);
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


        #region Filters
        /*----------------------------------------------------------------------------------------------------------------------------------
         * Filters
         *---------------------------------------------------------------------------------------------------------------------------------*/
        private questStatus create(MasterPricingEntities dbContext, Quest.Functional.MasterPricing.Filter filter, out FilterId filterId)
        {
            // Initialize
            filterId = null;


            // Perform create
            try
            {
                Quest.Services.Dbio.MasterPricing.Filters _filters = new Quest.Services.Dbio.MasterPricing.Filters();
                BufferMgr.TransferBuffer(filter, _filters);
                dbContext.Filters.Add(_filters);
                dbContext.SaveChanges();
                if (_filters.Id == 0)
                {
                    return (new questStatus(Severity.Error, "Quest.Functional.MasterPricing.Filter not created"));
                }
                filterId = new FilterId(_filters.Id);
            }
            catch (System.Exception ex)
            {
                return (new questStatus(Severity.Fatal, String.Format("EXCEPTION: {0}.{1}: {2}",
                        this.GetType().Name, MethodBase.GetCurrentMethod().Name,
                        ex.InnerException != null ? ex.InnerException.Message : ex.Message)));
            }
            return (new questStatus(Severity.Success));
        }
        private questStatus create(MasterPricingEntities dbContext, List<Quest.Functional.MasterPricing.Filter> filterList, out List<Quest.Functional.MasterPricing.Filter> filterIdList)
        {
            // Initialize
            filterIdList = null;


            // Perform create
            try
            {
                List<Quest.Services.Dbio.MasterPricing.Filters> _filterList = new List<Quest.Services.Dbio.MasterPricing.Filters>();
                foreach (Quest.Functional.MasterPricing.Filter filter in filterList)
                {
                    Quest.Services.Dbio.MasterPricing.Filters _filter = new Quest.Services.Dbio.MasterPricing.Filters();
                    BufferMgr.TransferBuffer(filter, _filter);
                    _filterList.Add(_filter);
                }
                dbContext.Filters.AddRange(_filterList);
                dbContext.SaveChanges();

                filterIdList = new List<Filter>();
                foreach (Quest.Services.Dbio.MasterPricing.Filters _filter in _filterList)
                {
                    Quest.Functional.MasterPricing.Filter filter = new Filter();
                    filter.Id = _filter.Id;
                    filterIdList.Add(filter);
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
        private questStatus read(MasterPricingEntities dbContext, FilterId filterId, out Quest.Services.Dbio.MasterPricing.Filters filter)
        {
            // Initialize
            filter = null;


            try
            {
                filter = dbContext.Filters.Where(r => r.Id == filterId.Id).SingleOrDefault();
                if (filter == null)
                {
                    return (new questStatus(Severity.Error, String.Format("ERROR: {0}.{1}: {2}",
                            this.GetType().Name, MethodBase.GetCurrentMethod().Name,
                            String.Format("Id {0} not found", filterId.Id))));
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
        private questStatus read(MasterPricingEntities dbContext, string name, out Quest.Services.Dbio.MasterPricing.Filters filter)
        {
            // Initialize
            filter = null;


            try
            {
                filter = dbContext.Filters.Where(r => r.Name == name).SingleOrDefault();
                if (filter == null)
                {
                    return (new questStatus(Severity.Error, String.Format("ERROR: {0}.{1}: {2}",
                            this.GetType().Name, MethodBase.GetCurrentMethod().Name,
                            String.Format("Name {0} not found", name))));
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
        private questStatus read(MasterPricingEntities dbContext, TablesetId tablesetId, out List<Quest.Services.Dbio.MasterPricing.Filters> filterList)
        {
            // Initialize
            filterList = null;


            try
            {
                filterList = dbContext.Filters.Where(r => r.TablesetId == tablesetId.Id).ToList();
                if (filterList == null)
                {
                    return (new questStatus(Severity.Error, String.Format("ERROR: {0}.{1}: {2}",
                            this.GetType().Name, MethodBase.GetCurrentMethod().Name,
                            String.Format("TablesetId {0} not found", tablesetId.Id))));
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
        private questStatus update(MasterPricingEntities dbContext, Quest.Functional.MasterPricing.Filter filter)
        {
            // Initialize 
            questStatus status = null;


            try
            {
                // Read the record.
                FilterId filterId = new FilterId(filter.Id);
                Quest.Services.Dbio.MasterPricing.Filters _filters = null;
                status = read(dbContext, filterId, out _filters);
                if (! questStatusDef.IsSuccess(status))
                {
                    return (status);
                }

                // Update the record.
                BufferMgr.TransferBuffer(filter, _filters);
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
                dbContext.Filters.RemoveRange(dbContext.Filters.Where(r => r.Id == filterId.Id));
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
        private questStatus delete(MasterPricingEntities dbContext, TablesetId tablesetId)
        {
            try
            {
                dbContext.Filters.RemoveRange(dbContext.Filters.Where(r => r.TablesetId == tablesetId.Id));
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
