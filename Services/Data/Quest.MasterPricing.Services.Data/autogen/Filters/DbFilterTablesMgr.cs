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
    public class DbFilterTablesMgr : DbMgrSessionBased
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
        public DbFilterTablesMgr(UserSession userSession)
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
        public questStatus Create(Quest.Functional.MasterPricing.FilterTable filterTable, out FilterTableId filterTableId)
        {
            // Initialize
            questStatus status = null;
            filterTableId = null;


            // Data rules.


            // Create the filterTable
            using (MasterPricingEntities dbContext = new MasterPricingEntities())
            {
                status = create(dbContext, filterTable, out filterTableId);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Create(DbMgrTransaction trans, Quest.Functional.MasterPricing.FilterTable filterTable, out FilterTableId filterTableId)
        {
            // Initialize
            questStatus status = null;
            filterTableId = null;


            // Data rules.


            // Create the filterTable in this transaction.
            status = create((MasterPricingEntities)trans.DbContext, filterTable, out filterTableId);
            if (! questStatusDef.IsSuccess(status))
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


            // Data rules.


            // Create the filterTables in this transaction.
            status = create((MasterPricingEntities)trans.DbContext, filterTableList, out filterTableIdList);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Read(FilterTableId filterTableId, out Quest.Functional.MasterPricing.FilterTable filterTable)
        {
            // Initialize
            questStatus status = null;
            filterTable = null;


            // Perform read
            using (MasterPricingEntities dbContext = new MasterPricingEntities())
            {
                Quest.Services.Dbio.MasterPricing.FilterTables _filterTables = null;
                status = read(dbContext, filterTableId, out _filterTables);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }
                filterTable = new Quest.Functional.MasterPricing.FilterTable();
                BufferMgr.TransferBuffer(_filterTables, filterTable);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Read(FilterTableNameIdentifier filterTableNameIdentifier, out Quest.Functional.MasterPricing.FilterTable filterTable)
        {
            // Initialize
            questStatus status = null;
            filterTable = null;


            // Perform read
            using (MasterPricingEntities dbContext = new MasterPricingEntities())
            {
                Quest.Services.Dbio.MasterPricing.FilterTables _filterTables = null;
                status = read(dbContext, filterTableNameIdentifier, out _filterTables);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }
                filterTable = new Quest.Functional.MasterPricing.FilterTable();
                BufferMgr.TransferBuffer(_filterTables, filterTable);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Read(DbMgrTransaction trans, FilterTableId filterTableId, out Quest.Functional.MasterPricing.FilterTable filterTable)
        {
            // Initialize
            questStatus status = null;
            filterTable = null;


            // Perform read
            Quest.Services.Dbio.MasterPricing.FilterTables _filterTables = null;
            status = read((MasterPricingEntities)trans.DbContext, filterTableId, out _filterTables);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            filterTable = new Quest.Functional.MasterPricing.FilterTable();
            BufferMgr.TransferBuffer(_filterTables, filterTable);

            return (new questStatus(Severity.Success));
        }
        public questStatus Read(FilterId filterId, out List<Quest.Functional.MasterPricing.FilterTable> filterTableList)
        {
            // Initialize
            questStatus status = null;
            filterTableList = null;


            // Perform read
            using (MasterPricingEntities dbContext = new MasterPricingEntities())
            {
                List<Quest.Services.Dbio.MasterPricing.FilterTables> _filterTablesList = null;
                status = read(dbContext, filterId, out _filterTablesList);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }
                filterTableList = new List<FilterTable>();
                foreach (Quest.Services.Dbio.MasterPricing.FilterTables _filterTable in _filterTablesList)
                {
                    Quest.Functional.MasterPricing.FilterTable filterTable = new Quest.Functional.MasterPricing.FilterTable();
                    BufferMgr.TransferBuffer(_filterTable, filterTable);
                    filterTableList.Add(filterTable);
                }
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Read(DbMgrTransaction trans, FilterId filterId, out List<Quest.Functional.MasterPricing.FilterTable> filterTableList)
        {
            // Initialize
            questStatus status = null;
            filterTableList = null;


            // Perform read
            List<Quest.Services.Dbio.MasterPricing.FilterTables> _filterTablesList = null;
            status = read((MasterPricingEntities)trans.DbContext, filterId, out _filterTablesList);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            filterTableList = new List<FilterTable>();
            foreach (Quest.Services.Dbio.MasterPricing.FilterTables _filterTable in _filterTablesList)
            {
                Quest.Functional.MasterPricing.FilterTable filterTable = new Quest.Functional.MasterPricing.FilterTable();
                BufferMgr.TransferBuffer(_filterTable, filterTable);
                filterTableList.Add(filterTable);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Update(Quest.Functional.MasterPricing.FilterTable filterTable)
        {
            // Initialize
            questStatus status = null;


            // Perform update.
            using (MasterPricingEntities dbContext = new MasterPricingEntities())
            {
                status = update(dbContext, filterTable);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Update(DbMgrTransaction trans, Quest.Functional.MasterPricing.FilterTable filterTable)
        {
            // Initialize
            questStatus status = null;
            bool bCreateTransaction = trans == null;


            // Perform update in this transaction.
            status = update((MasterPricingEntities)trans.DbContext, filterTable);
            if (!questStatusDef.IsSuccess(status))
            {
                RollbackTransaction(trans);
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Delete(FilterTableId filterTableId)
        {
            // Initialize
            questStatus status = null;


            // Perform delete.
            using (MasterPricingEntities dbContext = new MasterPricingEntities())
            {
                status = delete(dbContext, filterTableId);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Delete(DbMgrTransaction trans, FilterTableId filterTableId)
        {
            // Initialize
            questStatus status = null;


            // Perform delete in this transaction.
            status = delete((MasterPricingEntities)trans.DbContext, filterTableId);
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
        public questStatus List(QueryOptions queryOptions, out List<Quest.Functional.MasterPricing.FilterTable> filterTableList, out QueryResponse queryResponse)
        {
            // Initialize
            questStatus status = null;
            filterTableList = null;
            queryResponse = null;


            using (MasterPricingEntities dbContext = new MasterPricingEntities())
            {
                using (DbContextTransaction tx = dbContext.Database.BeginTransaction(IsolationLevel.ReadUncommitted))
                {
                    try
                    {
                        PropertyInfo[] dbProperties = typeof(Quest.Services.Dbio.MasterPricing.FilterTables).GetProperties().ToArray();
                        int totalRecords = dbContext.FilterTables.Where(BuildWhereClause(queryOptions, dbProperties)).Count();
                        List<Quest.Services.Dbio.MasterPricing.FilterTables> _countriesList = dbContext.FilterTables.Where(BuildWhereClause(queryOptions, dbProperties))
                                .OrderBy(BuildSortString(queryOptions.SortColumns))
                                .Skip(queryOptions.Paging.PageSize * (queryOptions.Paging.PageNumber - 1))
                                .Take(queryOptions.Paging.PageSize).ToList();
                        if (_countriesList == null)
                        {
                            return (new questStatus(Severity.Warning));
                        }
                        filterTableList = new List<Quest.Functional.MasterPricing.FilterTable>();
                        foreach (Quest.Services.Dbio.MasterPricing.FilterTables _filterTables in _countriesList)
                        {
                            Quest.Functional.MasterPricing.FilterTable filterTable = new Quest.Functional.MasterPricing.FilterTable();
                            BufferMgr.TransferBuffer(_filterTables, filterTable);
                            filterTableList.Add(filterTable);
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


        #region FilterTables
        /*----------------------------------------------------------------------------------------------------------------------------------
         * FilterTables
         *---------------------------------------------------------------------------------------------------------------------------------*/
        private questStatus create(MasterPricingEntities dbContext, Quest.Functional.MasterPricing.FilterTable filterTable, out FilterTableId filterTableId)
        {
            // Initialize
            filterTableId = null;


            // Perform create
            try
            {
                Quest.Services.Dbio.MasterPricing.FilterTables _filterTables = new Quest.Services.Dbio.MasterPricing.FilterTables();
                BufferMgr.TransferBuffer(filterTable, _filterTables);
                dbContext.FilterTables.Add(_filterTables);
                dbContext.SaveChanges();
                if (_filterTables.Id == 0)
                {
                    return (new questStatus(Severity.Error, "Quest.Functional.MasterPricing.FilterTable not created"));
                }
                filterTableId = new FilterTableId(_filterTables.Id);
            }
            catch (System.Exception ex)
            {
                return (new questStatus(Severity.Fatal, String.Format("EXCEPTION: {0}.{1}: {2}",
                        this.GetType().Name, MethodBase.GetCurrentMethod().Name,
                        ex.InnerException != null ? ex.InnerException.Message : ex.Message)));
            }
            return (new questStatus(Severity.Success));
        }
        private questStatus create(MasterPricingEntities dbContext, List<Quest.Functional.MasterPricing.FilterTable> filterTableList, out List<Quest.Functional.MasterPricing.FilterTable> filterTableIdList)
        {
            // Initialize
            filterTableIdList = null;


            // Perform create
            try
            {
                List<Quest.Services.Dbio.MasterPricing.FilterTables> _filterTableList = new List<Quest.Services.Dbio.MasterPricing.FilterTables>();
                foreach (Quest.Functional.MasterPricing.FilterTable filterTable in filterTableList)
                {
                    Quest.Services.Dbio.MasterPricing.FilterTables _filterTable = new Quest.Services.Dbio.MasterPricing.FilterTables();
                    BufferMgr.TransferBuffer(filterTable, _filterTable);
                    _filterTableList.Add(_filterTable);
                }
                dbContext.FilterTables.AddRange(_filterTableList);
                dbContext.SaveChanges();

                filterTableIdList = new List<FilterTable>();
                foreach (Quest.Services.Dbio.MasterPricing.FilterTables _filterTable in _filterTableList)
                {
                    Quest.Functional.MasterPricing.FilterTable filterTable = new FilterTable();
                    filterTable.Id = _filterTable.Id;
                    filterTableIdList.Add(filterTable);
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
        private questStatus read(MasterPricingEntities dbContext, FilterTableId filterTableId, out Quest.Services.Dbio.MasterPricing.FilterTables filterTable)
        {
            // Initialize
            filterTable = null;


            try
            {
                filterTable = dbContext.FilterTables.Where(r => r.Id == filterTableId.Id).SingleOrDefault();
                if (filterTable == null)
                {
                    return (new questStatus(Severity.Error, String.Format("ERROR: {0}.{1}: {2}",
                            this.GetType().Name, MethodBase.GetCurrentMethod().Name,
                            String.Format("Id {0} not found", filterTableId.Id))));
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
        private questStatus read(MasterPricingEntities dbContext, FilterTableNameIdentifier filterTableNameIdentifier, out Quest.Services.Dbio.MasterPricing.FilterTables filterTable)
        {
            // Initialize
            filterTable = null;


            try
            {
                filterTable = dbContext.FilterTables.Where(r => r.FilterId == filterTableNameIdentifier.FilterId.Id && r.Schema == filterTableNameIdentifier.Schema && r.Name == filterTableNameIdentifier.Name).SingleOrDefault();
                if (filterTable == null)
                {
                    return (new questStatus(Severity.Error, String.Format("ERROR: {0}.{1}: {2}",
                            this.GetType().Name, MethodBase.GetCurrentMethod().Name,
                            String.Format("FilterTableNameIdentifier FilterId:{0}  Schema:{1}  Name:{2}  not found", 
                                filterTableNameIdentifier.FilterId.Id, filterTableNameIdentifier.Schema, filterTableNameIdentifier.Name))));
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
        private questStatus read(MasterPricingEntities dbContext, FilterId filterId, out List<Quest.Services.Dbio.MasterPricing.FilterTables> filterTableList)
        {
            // Initialize
            filterTableList = null;


            try
            {
                filterTableList = dbContext.FilterTables.Where(r => r.FilterId == filterId.Id).ToList();
                if (filterTableList == null)
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
        private questStatus update(MasterPricingEntities dbContext, Quest.Functional.MasterPricing.FilterTable filterTable)
        {
            // Initialize 
            questStatus status = null;


            try
            {
                // Read the record.
                FilterTableId filterTableId = new FilterTableId(filterTable.Id);
                Quest.Services.Dbio.MasterPricing.FilterTables _filterTables = null;
                status = read(dbContext, filterTableId, out _filterTables);
                if (! questStatusDef.IsSuccess(status))
                {
                    return (status);
                }

                // Update the record.
                BufferMgr.TransferBuffer(filterTable, _filterTables);
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
        private questStatus delete(MasterPricingEntities dbContext, FilterTableId filterTableId)
        {
            try
            {
                dbContext.FilterTables.RemoveRange(dbContext.FilterTables.Where(r => r.Id == filterTableId.Id));
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
                dbContext.FilterTables.RemoveRange(dbContext.FilterTables.Where(r => r.FilterId == filterId.Id));
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
