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
    public class DbFilterProceduresMgr : DbMgrSessionBased
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
        public DbFilterProceduresMgr(UserSession userSession)
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
        public questStatus Create(Quest.Functional.MasterPricing.FilterProcedure filterProcedure, out FilterProcedureId filterProcedureId)
        {
            // Initialize
            questStatus status = null;
            filterProcedureId = null;


            // Data rules.


            // Create the filterProcedure
            using (MasterPricingEntities dbContext = new MasterPricingEntities())
            {
                status = create(dbContext, filterProcedure, out filterProcedureId);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Create(DbMgrTransaction trans, Quest.Functional.MasterPricing.FilterProcedure filterProcedure, out FilterProcedureId filterProcedureId)
        {
            // Initialize
            questStatus status = null;
            filterProcedureId = null;


            // Data rules.


            // Create the filterProcedure in this transaction.
            status = create((MasterPricingEntities)trans.DbContext, filterProcedure, out filterProcedureId);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Create(DbMgrTransaction trans, List<Quest.Functional.MasterPricing.FilterProcedure> filterProcedureList, out List<Quest.Functional.MasterPricing.FilterProcedure> filterProcedureIdList)
        {
            // Initialize
            questStatus status = null;
            filterProcedureIdList = null;


            // Data rules.


            // Create the filterProcedures in this transaction.
            status = create((MasterPricingEntities)trans.DbContext, filterProcedureList, out filterProcedureIdList);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Read(FilterProcedureId filterProcedureId, out Quest.Functional.MasterPricing.FilterProcedure filterProcedure)
        {
            // Initialize
            questStatus status = null;
            filterProcedure = null;


            // Perform read
            using (MasterPricingEntities dbContext = new MasterPricingEntities())
            {
                Quest.Services.Dbio.MasterPricing.FilterProcedures _filterProcedures = null;
                status = read(dbContext, filterProcedureId, out _filterProcedures);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }
                filterProcedure = new Quest.Functional.MasterPricing.FilterProcedure();
                BufferMgr.TransferBuffer(_filterProcedures, filterProcedure);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Read(DbMgrTransaction trans, FilterProcedureId filterProcedureId, out Quest.Functional.MasterPricing.FilterProcedure filterProcedure)
        {
            // Initialize
            questStatus status = null;
            filterProcedure = null;


            // Perform read
            Quest.Services.Dbio.MasterPricing.FilterProcedures _filterProcedures = null;
            status = read((MasterPricingEntities)trans.DbContext, filterProcedureId, out _filterProcedures);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            filterProcedure = new Quest.Functional.MasterPricing.FilterProcedure();
            BufferMgr.TransferBuffer(_filterProcedures, filterProcedure);

            return (new questStatus(Severity.Success));
        }
        public questStatus Read(FilterId filterId, out List<Quest.Functional.MasterPricing.FilterProcedure> filterProcedureList)
        {
            // Initialize
            questStatus status = null;
            filterProcedureList = null;


            // Perform read
            using (MasterPricingEntities dbContext = new MasterPricingEntities())
            {
                List<Quest.Services.Dbio.MasterPricing.FilterProcedures> _filterProceduresList = null;
                status = read(dbContext, filterId, out _filterProceduresList);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }
                filterProcedureList = new List<FilterProcedure>();
                foreach (Quest.Services.Dbio.MasterPricing.FilterProcedures _filterProcedure in _filterProceduresList)
                {
                    Quest.Functional.MasterPricing.FilterProcedure filterProcedure = new Quest.Functional.MasterPricing.FilterProcedure();
                    BufferMgr.TransferBuffer(_filterProcedure, filterProcedure);
                    filterProcedureList.Add(filterProcedure);
                }
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Read(DbMgrTransaction trans, FilterId filterId, out List<Quest.Functional.MasterPricing.FilterProcedure> filterProcedureList)
        {
            // Initialize
            questStatus status = null;
            filterProcedureList = null;


            // Perform read
            List<Quest.Services.Dbio.MasterPricing.FilterProcedures> _filterProceduresList = null;
            status = read((MasterPricingEntities)trans.DbContext, filterId, out _filterProceduresList);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            filterProcedureList = new List<FilterProcedure>();
            foreach (Quest.Services.Dbio.MasterPricing.FilterProcedures _filterProcedure in _filterProceduresList)
            {
                Quest.Functional.MasterPricing.FilterProcedure filterProcedure = new Quest.Functional.MasterPricing.FilterProcedure();
                BufferMgr.TransferBuffer(_filterProcedure, filterProcedure);
                filterProcedureList.Add(filterProcedure);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Update(Quest.Functional.MasterPricing.FilterProcedure filterProcedure)
        {
            // Initialize
            questStatus status = null;


            // Perform update.
            using (MasterPricingEntities dbContext = new MasterPricingEntities())
            {
                status = update(dbContext, filterProcedure);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Update(DbMgrTransaction trans, Quest.Functional.MasterPricing.FilterProcedure filterProcedure)
        {
            // Initialize
            questStatus status = null;
            bool bCreateTransaction = trans == null;


            // Perform update in this transaction.
            status = update((MasterPricingEntities)trans.DbContext, filterProcedure);
            if (!questStatusDef.IsSuccess(status))
            {
                RollbackTransaction(trans);
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Delete(FilterProcedureId filterProcedureId)
        {
            // Initialize
            questStatus status = null;


            // Perform delete.
            using (MasterPricingEntities dbContext = new MasterPricingEntities())
            {
                status = delete(dbContext, filterProcedureId);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Delete(DbMgrTransaction trans, FilterProcedureId filterProcedureId)
        {
            // Initialize
            questStatus status = null;


            // Perform delete in this transaction.
            status = delete((MasterPricingEntities)trans.DbContext, filterProcedureId);
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
            if (!questStatusDef.IsSuccess(status))
            {
                RollbackTransaction(trans);
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus List(QueryOptions queryOptions, out List<Quest.Functional.MasterPricing.FilterProcedure> filterProcedureList, out QueryResponse queryResponse)
        {
            // Initialize
            questStatus status = null;
            filterProcedureList = null;
            queryResponse = null;


            using (MasterPricingEntities dbContext = new MasterPricingEntities())
            {
                using (DbContextTransaction tx = dbContext.Database.BeginTransaction(IsolationLevel.ReadUncommitted))
                {
                    try
                    {
                        PropertyInfo[] dbProperties = typeof(Quest.Services.Dbio.MasterPricing.FilterProcedures).GetProperties().ToArray();
                        int totalRecords = dbContext.FilterProcedures.Where(BuildWhereClause(queryOptions, dbProperties)).Count();
                        List<Quest.Services.Dbio.MasterPricing.FilterProcedures> _countriesList = dbContext.FilterProcedures.Where(BuildWhereClause(queryOptions, dbProperties))
                                .OrderBy(BuildSortString(queryOptions.SortColumns))
                                .Skip(queryOptions.Paging.PageSize * (queryOptions.Paging.PageNumber - 1))
                                .Take(queryOptions.Paging.PageSize).ToList();
                        if (_countriesList == null)
                        {
                            return (new questStatus(Severity.Warning));
                        }
                        filterProcedureList = new List<Quest.Functional.MasterPricing.FilterProcedure>();
                        foreach (Quest.Services.Dbio.MasterPricing.FilterProcedures _filterProcedures in _countriesList)
                        {
                            Quest.Functional.MasterPricing.FilterProcedure filterProcedure = new Quest.Functional.MasterPricing.FilterProcedure();
                            BufferMgr.TransferBuffer(_filterProcedures, filterProcedure);
                            filterProcedureList.Add(filterProcedure);
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


        #region FilterProcedures
        /*----------------------------------------------------------------------------------------------------------------------------------
         * FilterProcedures
         *---------------------------------------------------------------------------------------------------------------------------------*/
        private questStatus create(MasterPricingEntities dbContext, Quest.Functional.MasterPricing.FilterProcedure filterProcedure, out FilterProcedureId filterProcedureId)
        {
            // Initialize
            filterProcedureId = null;


            // Perform create
            try
            {
                Quest.Services.Dbio.MasterPricing.FilterProcedures _filterProcedures = new Quest.Services.Dbio.MasterPricing.FilterProcedures();
                BufferMgr.TransferBuffer(filterProcedure, _filterProcedures);
                dbContext.FilterProcedures.Add(_filterProcedures);
                dbContext.SaveChanges();
                if (_filterProcedures.Id == 0)
                {
                    return (new questStatus(Severity.Error, "Quest.Functional.MasterPricing.FilterProcedure not created"));
                }
                filterProcedureId = new FilterProcedureId(_filterProcedures.Id);
            }
            catch (System.Exception ex)
            {
                return (new questStatus(Severity.Fatal, String.Format("EXCEPTION: {0}.{1}: {2}",
                        this.GetType().Name, MethodBase.GetCurrentMethod().Name,
                        ex.InnerException != null ? ex.InnerException.Message : ex.Message)));
            }
            return (new questStatus(Severity.Success));
        }
        private questStatus create(MasterPricingEntities dbContext, List<Quest.Functional.MasterPricing.FilterProcedure> filterProcedureList, out List<Quest.Functional.MasterPricing.FilterProcedure> filterProcedureIdList)
        {
            // Initialize
            filterProcedureIdList = null;


            // Perform create
            try
            {
                List<Quest.Services.Dbio.MasterPricing.FilterProcedures> _filterProcedureList = new List<Quest.Services.Dbio.MasterPricing.FilterProcedures>();
                foreach (Quest.Functional.MasterPricing.FilterProcedure filterProcedure in filterProcedureList)
                {
                    Quest.Services.Dbio.MasterPricing.FilterProcedures _filterProcedure = new Quest.Services.Dbio.MasterPricing.FilterProcedures();
                    BufferMgr.TransferBuffer(filterProcedure, _filterProcedure);
                    _filterProcedureList.Add(_filterProcedure);
                }
                dbContext.FilterProcedures.AddRange(_filterProcedureList);
                dbContext.SaveChanges();

                filterProcedureIdList = new List<FilterProcedure>();
                foreach (Quest.Services.Dbio.MasterPricing.FilterProcedures _filterProcedure in _filterProcedureList)
                {
                    Quest.Functional.MasterPricing.FilterProcedure filterProcedure = new FilterProcedure();
                    filterProcedure.Id = _filterProcedure.Id;
                    filterProcedureIdList.Add(filterProcedure);
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
        private questStatus read(MasterPricingEntities dbContext, FilterProcedureId filterProcedureId, out Quest.Services.Dbio.MasterPricing.FilterProcedures filterProcedure)
        {
            // Initialize
            filterProcedure = null;


            try
            {
                filterProcedure = dbContext.FilterProcedures.Where(r => r.Id == filterProcedureId.Id).SingleOrDefault();
                if (filterProcedure == null)
                {
                    return (new questStatus(Severity.Error, String.Format("ERROR: {0}.{1}: {2}",
                            this.GetType().Name, MethodBase.GetCurrentMethod().Name,
                            String.Format("Id {0} not found", filterProcedureId.Id))));
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
        private questStatus read(MasterPricingEntities dbContext, FilterId filterId, out List<Quest.Services.Dbio.MasterPricing.FilterProcedures> filterProcedureList)
        {
            // Initialize
            filterProcedureList = null;


            try
            {
                filterProcedureList = dbContext.FilterProcedures.Where(r => r.FilterId == filterId.Id).ToList();
                if (filterProcedureList == null)
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
        private questStatus update(MasterPricingEntities dbContext, Quest.Functional.MasterPricing.FilterProcedure filterProcedure)
        {
            // Initialize 
            questStatus status = null;


            try
            {
                // Read the record.
                FilterProcedureId filterProcedureId = new FilterProcedureId(filterProcedure.Id);
                Quest.Services.Dbio.MasterPricing.FilterProcedures _filterProcedures = null;
                status = read(dbContext, filterProcedureId, out _filterProcedures);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }

                // Update the record.
                BufferMgr.TransferBuffer(filterProcedure, _filterProcedures);
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
        private questStatus delete(MasterPricingEntities dbContext, FilterProcedureId filterProcedureId)
        {
            try
            {
                dbContext.FilterProcedures.RemoveRange(dbContext.FilterProcedures.Where(r => r.Id == filterProcedureId.Id));
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
                dbContext.FilterProcedures.RemoveRange(dbContext.FilterProcedures.Where(r => r.FilterId == filterId.Id));
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
