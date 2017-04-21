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
    public class DbFilterColumnsMgr : DbMgrSessionBased
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
        public DbFilterColumnsMgr(UserSession userSession)
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
        public questStatus Create(Quest.Functional.MasterPricing.FilterColumn filterColumn, out FilterColumnId filterColumnId)
        {
            // Initialize
            questStatus status = null;
            filterColumnId = null;


            // Data rules.


            // Create the filterColumn
            using (MasterPricingEntities dbContext = new MasterPricingEntities())
            {
                status = create(dbContext, filterColumn, out filterColumnId);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Create(DbMgrTransaction trans, Quest.Functional.MasterPricing.FilterColumn filterColumn, out FilterColumnId filterColumnId)
        {
            // Initialize
            questStatus status = null;
            filterColumnId = null;


            // Data rules.


            // Create the filterColumn in this transaction.
            status = create((MasterPricingEntities)trans.DbContext, filterColumn, out filterColumnId);
            if (! questStatusDef.IsSuccess(status))
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


            // Data rules.


            // Create the filterColumns in this transaction.
            status = create((MasterPricingEntities)trans.DbContext, filterColumnList, out filterColumnIdList);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Read(FilterColumnId filterColumnId, out Quest.Functional.MasterPricing.FilterColumn filterColumn)
        {
            // Initialize
            questStatus status = null;
            filterColumn = null;


            // Perform read
            using (MasterPricingEntities dbContext = new MasterPricingEntities())
            {
                Quest.Services.Dbio.MasterPricing.FilterColumns _filterColumns = null;
                status = read(dbContext, filterColumnId, out _filterColumns);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }
                filterColumn = new Quest.Functional.MasterPricing.FilterColumn();
                BufferMgr.TransferBuffer(_filterColumns, filterColumn);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Read(DbMgrTransaction trans, FilterColumnId filterColumnId, out Quest.Functional.MasterPricing.FilterColumn filterColumn)
        {
            // Initialize
            questStatus status = null;
            filterColumn = null;


            // Perform read
            Quest.Services.Dbio.MasterPricing.FilterColumns _filterColumns = null;
            status = read((MasterPricingEntities)trans.DbContext, filterColumnId, out _filterColumns);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            filterColumn = new Quest.Functional.MasterPricing.FilterColumn();
            BufferMgr.TransferBuffer(_filterColumns, filterColumn);

            return (new questStatus(Severity.Success));
        }
        public questStatus Read(FilterId filterId, FilterTableId filterTableId, out List<Quest.Functional.MasterPricing.FilterColumn> filterColumnList)
        {
            // Initialize
            questStatus status = null;
            filterColumnList = null;


            // Perform read
            using (MasterPricingEntities dbContext = new MasterPricingEntities())
            {
                List<Quest.Services.Dbio.MasterPricing.FilterColumns> _filterColumnsList = null;
                status = read(dbContext, filterId, filterTableId, out _filterColumnsList);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }
                filterColumnList = new List<FilterColumn>();
                foreach (Quest.Services.Dbio.MasterPricing.FilterColumns _filterColumn in _filterColumnsList)
                {
                    Quest.Functional.MasterPricing.FilterColumn filterColumn = new Quest.Functional.MasterPricing.FilterColumn();
                    BufferMgr.TransferBuffer(_filterColumn, filterColumn);
                    filterColumnList.Add(filterColumn);
                }
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Read(DbMgrTransaction trans, FilterId filterId, FilterTableId filterTableId, out List<Quest.Functional.MasterPricing.FilterColumn> filterColumnList)
        {
            // Initialize
            questStatus status = null;
            filterColumnList = null;


            // Perform read
            List<Quest.Services.Dbio.MasterPricing.FilterColumns> _filterColumnsList = null;
            status = read((MasterPricingEntities)trans.DbContext, filterId, filterTableId, out _filterColumnsList);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            filterColumnList = new List<FilterColumn>();
            foreach (Quest.Services.Dbio.MasterPricing.FilterColumns _filterColumn in _filterColumnsList)
            {
                Quest.Functional.MasterPricing.FilterColumn filterColumn = new Quest.Functional.MasterPricing.FilterColumn();
                BufferMgr.TransferBuffer(_filterColumn, filterColumn);
                filterColumnList.Add(filterColumn);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Read(FilterId filterId, FilterViewId filterViewId, out List<Quest.Functional.MasterPricing.FilterColumn> filterColumnList)
        {
            // Initialize
            questStatus status = null;
            filterColumnList = null;


            // Perform read
            using (MasterPricingEntities dbContext = new MasterPricingEntities())
            {
                List<Quest.Services.Dbio.MasterPricing.FilterColumns> _filterColumnsList = null;
                status = read(dbContext, filterId, filterViewId, out _filterColumnsList);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }
                filterColumnList = new List<FilterColumn>();
                foreach (Quest.Services.Dbio.MasterPricing.FilterColumns _filterColumn in _filterColumnsList)
                {
                    Quest.Functional.MasterPricing.FilterColumn filterColumn = new Quest.Functional.MasterPricing.FilterColumn();
                    BufferMgr.TransferBuffer(_filterColumn, filterColumn);
                    filterColumnList.Add(filterColumn);
                }
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Read(DbMgrTransaction trans, FilterId filterId, FilterViewId filterViewId, out List<Quest.Functional.MasterPricing.FilterColumn> filterColumnList)
        {
            // Initialize
            questStatus status = null;
            filterColumnList = null;


            // Perform read
            List<Quest.Services.Dbio.MasterPricing.FilterColumns> _filterColumnsList = null;
            status = read((MasterPricingEntities)trans.DbContext, filterId, filterViewId, out _filterColumnsList);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            filterColumnList = new List<FilterColumn>();
            foreach (Quest.Services.Dbio.MasterPricing.FilterColumns _filterColumn in _filterColumnsList)
            {
                Quest.Functional.MasterPricing.FilterColumn filterColumn = new Quest.Functional.MasterPricing.FilterColumn();
                BufferMgr.TransferBuffer(_filterColumn, filterColumn);
                filterColumnList.Add(filterColumn);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Read(FilterId filterId, out List<Quest.Functional.MasterPricing.FilterColumn> filterColumnList)
        {
            // Initialize
            questStatus status = null;
            filterColumnList = null;


            // Perform read
            using (MasterPricingEntities dbContext = new MasterPricingEntities())
            {
                List<Quest.Services.Dbio.MasterPricing.FilterColumns> _filterColumnsList = null;
                status = read(dbContext, filterId, out _filterColumnsList);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }
                filterColumnList = new List<FilterColumn>();
                foreach (Quest.Services.Dbio.MasterPricing.FilterColumns _filterColumn in _filterColumnsList)
                {
                    Quest.Functional.MasterPricing.FilterColumn filterColumn = new Quest.Functional.MasterPricing.FilterColumn();
                    BufferMgr.TransferBuffer(_filterColumn, filterColumn);
                    filterColumnList.Add(filterColumn);
                }
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Read(DbMgrTransaction trans, FilterId filterId, out List<Quest.Functional.MasterPricing.FilterColumn> filterColumnList)
        {
            // Initialize
            questStatus status = null;
            filterColumnList = null;


            // Perform read
            List<Quest.Services.Dbio.MasterPricing.FilterColumns> _filterColumnsList = null;
            status = read((MasterPricingEntities)trans.DbContext, filterId, out _filterColumnsList);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            filterColumnList = new List<FilterColumn>();
            foreach (Quest.Services.Dbio.MasterPricing.FilterColumns _filterColumn in _filterColumnsList)
            {
                Quest.Functional.MasterPricing.FilterColumn filterColumn = new Quest.Functional.MasterPricing.FilterColumn();
                BufferMgr.TransferBuffer(_filterColumn, filterColumn);
                filterColumnList.Add(filterColumn);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Update(Quest.Functional.MasterPricing.FilterColumn filterColumn)
        {
            // Initialize
            questStatus status = null;


            // Perform update.
            using (MasterPricingEntities dbContext = new MasterPricingEntities())
            {
                status = update(dbContext, filterColumn);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Update(DbMgrTransaction trans, Quest.Functional.MasterPricing.FilterColumn filterColumn)
        {
            // Initialize
            questStatus status = null;
            bool bCreateTransaction = trans == null;


            // Perform update in this transaction.
            status = update((MasterPricingEntities)trans.DbContext, filterColumn);
            if (!questStatusDef.IsSuccess(status))
            {
                RollbackTransaction(trans);
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Delete(FilterColumnId filterColumnId)
        {
            // Initialize
            questStatus status = null;


            // Perform delete.
            using (MasterPricingEntities dbContext = new MasterPricingEntities())
            {
                status = delete(dbContext, filterColumnId);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Delete(DbMgrTransaction trans, FilterColumnId filterColumnId)
        {
            // Initialize
            questStatus status = null;


            // Perform delete in this transaction.
            status = delete((MasterPricingEntities)trans.DbContext, filterColumnId);
            if (! questStatusDef.IsSuccess(status))
            {
                RollbackTransaction(trans);
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Delete(FilterId filterId, FilterTableId filterTableId)
        {
            // Initialize
            questStatus status = null;


            // Perform delete.
            using (MasterPricingEntities dbContext = new MasterPricingEntities())
            {
                status = delete(dbContext, filterId, filterTableId);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Delete(DbMgrTransaction trans, FilterId filterId, FilterTableId filterTableId)
        {
            // Initialize
            questStatus status = null;


            // Perform delete in this transaction.
            status = delete((MasterPricingEntities)trans.DbContext, filterId, filterTableId);
            if (!questStatusDef.IsSuccess(status))
            {
                RollbackTransaction(trans);
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Delete(FilterId filterId, FilterViewId filterViewId)
        {
            // Initialize
            questStatus status = null;


            // Perform delete.
            using (MasterPricingEntities dbContext = new MasterPricingEntities())
            {
                status = delete(dbContext, filterId, filterViewId);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Delete(DbMgrTransaction trans, FilterId filterId, FilterViewId filterViewId)
        {
            // Initialize
            questStatus status = null;


            // Perform delete in this transaction.
            status = delete((MasterPricingEntities)trans.DbContext, filterId, filterViewId);
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
        public questStatus List(QueryOptions queryOptions, out List<Quest.Functional.MasterPricing.FilterColumn> filterColumnList, out QueryResponse queryResponse)
        {
            // Initialize
            questStatus status = null;
            filterColumnList = null;
            queryResponse = null;


            using (MasterPricingEntities dbContext = new MasterPricingEntities())
            {
                using (DbContextTransaction tx = dbContext.Database.BeginTransaction(IsolationLevel.ReadUncommitted))
                {
                    try
                    {
                        PropertyInfo[] dbProperties = typeof(Quest.Services.Dbio.MasterPricing.FilterColumns).GetProperties().ToArray();
                        int totalRecords = dbContext.FilterColumns.Where(BuildWhereClause(queryOptions, dbProperties)).Count();
                        List<Quest.Services.Dbio.MasterPricing.FilterColumns> _countriesList = dbContext.FilterColumns.Where(BuildWhereClause(queryOptions, dbProperties))
                                .OrderBy(BuildSortString(queryOptions.SortColumns))
                                .Skip(queryOptions.Paging.PageSize * (queryOptions.Paging.PageNumber - 1))
                                .Take(queryOptions.Paging.PageSize).ToList();
                        if (_countriesList == null)
                        {
                            return (new questStatus(Severity.Warning));
                        }
                        filterColumnList = new List<Quest.Functional.MasterPricing.FilterColumn>();
                        foreach (Quest.Services.Dbio.MasterPricing.FilterColumns _filterColumns in _countriesList)
                        {
                            Quest.Functional.MasterPricing.FilterColumn filterColumn = new Quest.Functional.MasterPricing.FilterColumn();
                            BufferMgr.TransferBuffer(_filterColumns, filterColumn);
                            filterColumnList.Add(filterColumn);
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
        private questStatus create(MasterPricingEntities dbContext, Quest.Functional.MasterPricing.FilterColumn filterColumn, out FilterColumnId filterColumnId)
        {
            // Initialize
            filterColumnId = null;


            // Perform create
            try
            {
                Quest.Services.Dbio.MasterPricing.FilterColumns _filterColumns = new Quest.Services.Dbio.MasterPricing.FilterColumns();
                BufferMgr.TransferBuffer(filterColumn, _filterColumns);
                dbContext.FilterColumns.Add(_filterColumns);
                dbContext.SaveChanges();
                if (_filterColumns.Id == 0)
                {
                    return (new questStatus(Severity.Error, "Quest.Functional.MasterPricing.FilterColumn not created"));
                }
                filterColumnId = new FilterColumnId(_filterColumns.Id);
            }
            catch (System.Exception ex)
            {
                return (new questStatus(Severity.Fatal, String.Format("EXCEPTION: {0}.{1}: {2}",
                        this.GetType().Name, MethodBase.GetCurrentMethod().Name,
                        ex.InnerException != null ? ex.InnerException.Message : ex.Message)));
            }
            return (new questStatus(Severity.Success));
        }
        private questStatus create(MasterPricingEntities dbContext, List<Quest.Functional.MasterPricing.FilterColumn> filterColumnList, out List<Quest.Functional.MasterPricing.FilterColumn> filterColumnIdList)
        {
            // Initialize
            filterColumnIdList = null;


            // Perform create
            try
            {
                List<Quest.Services.Dbio.MasterPricing.FilterColumns> _filterColumnList = new List<Quest.Services.Dbio.MasterPricing.FilterColumns>();
                foreach (Quest.Functional.MasterPricing.FilterColumn filterColumn in filterColumnList)
                {
                    Quest.Services.Dbio.MasterPricing.FilterColumns _filterColumn = new Quest.Services.Dbio.MasterPricing.FilterColumns();
                    BufferMgr.TransferBuffer(filterColumn, _filterColumn);
                    _filterColumnList.Add(_filterColumn);
                }
                dbContext.FilterColumns.AddRange(_filterColumnList);
                dbContext.SaveChanges();

                filterColumnIdList = new List<FilterColumn>();
                foreach (Quest.Services.Dbio.MasterPricing.FilterColumns _filterColumn in _filterColumnList)
                {
                    Quest.Functional.MasterPricing.FilterColumn filterColumn = new FilterColumn();
                    filterColumn.Id = _filterColumn.Id;
                    filterColumnIdList.Add(filterColumn);
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
        private questStatus read(MasterPricingEntities dbContext, FilterColumnId filterColumnId, out Quest.Services.Dbio.MasterPricing.FilterColumns filterColumn)
        {
            // Initialize
            filterColumn = null;


            try
            {
                filterColumn = dbContext.FilterColumns.Where(r => r.Id == filterColumnId.Id).SingleOrDefault();
                if (filterColumn == null)
                {
                    return (new questStatus(Severity.Error, String.Format("ERROR: {0}.{1}: {2}",
                            this.GetType().Name, MethodBase.GetCurrentMethod().Name,
                            String.Format("Id {0} not found", filterColumnId.Id))));
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
        private questStatus read(MasterPricingEntities dbContext, FilterId filterId, FilterTableId filterTableId, out List<Quest.Services.Dbio.MasterPricing.FilterColumns> filterColumnList)
        {
            // Initialize
            filterColumnList = null;


            try
            {
                filterColumnList = dbContext.FilterColumns.Where(r => r.FilterId == filterId.Id && r.FilterEntityTypeId == FilterEntityType.Table && r.FilterEntityId == filterTableId.Id).ToList();
                if (filterColumnList == null)
                {
                    return (new questStatus(Severity.Error, String.Format("ERROR: {0}.{1}: {2}",
                            this.GetType().Name, MethodBase.GetCurrentMethod().Name,
                            String.Format("FilterColumn for FilterTableId {0} not found", filterTableId.Id))));
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
        private questStatus read(MasterPricingEntities dbContext, FilterId filterId, FilterViewId filterViewId, out List<Quest.Services.Dbio.MasterPricing.FilterColumns> filterColumnList)
        {
            // Initialize
            filterColumnList = null;


            try
            {
                filterColumnList = dbContext.FilterColumns.Where(r => r.FilterId == filterId.Id && r.FilterEntityTypeId == FilterEntityType.View && r.FilterEntityId == filterViewId.Id).ToList();
                if (filterColumnList == null)
                {
                    return (new questStatus(Severity.Error, String.Format("ERROR: {0}.{1}: {2}",
                            this.GetType().Name, MethodBase.GetCurrentMethod().Name,
                            String.Format("FilterColumn for FilterViewId {0} not found", filterViewId.Id))));
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
        private questStatus read(MasterPricingEntities dbContext, FilterId filterId, out List<Quest.Services.Dbio.MasterPricing.FilterColumns> filterColumnList)
        {
            // Initialize
            filterColumnList = null;


            try
            {
                filterColumnList = dbContext.FilterColumns.Where(r => r.FilterId == filterId.Id).ToList();
                if (filterColumnList == null)
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
        private questStatus update(MasterPricingEntities dbContext, Quest.Functional.MasterPricing.FilterColumn filterColumn)
        {
            // Initialize 
            questStatus status = null;


            try
            {
                // Read the record.
                FilterColumnId filterColumnId = new FilterColumnId(filterColumn.Id);
                Quest.Services.Dbio.MasterPricing.FilterColumns _filterColumns = null;
                status = read(dbContext, filterColumnId, out _filterColumns);
                if (! questStatusDef.IsSuccess(status))
                {
                    return (status);
                }

                // Update the record.
                BufferMgr.TransferBuffer(filterColumn, _filterColumns);
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
        private questStatus delete(MasterPricingEntities dbContext, FilterColumnId filterColumnId)
        {
            try
            {
                dbContext.FilterColumns.RemoveRange(dbContext.FilterColumns.Where(r => r.Id == filterColumnId.Id));
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
        private questStatus delete(MasterPricingEntities dbContext, FilterId filterId, FilterTableId filterTableId)
        {
            try
            {
                dbContext.FilterColumns.RemoveRange(dbContext.FilterColumns.Where(
                        r => r.FilterId == filterId.Id && r.FilterEntityTypeId == FilterEntityType.Table && r.FilterEntityId == filterTableId.Id));
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
        private questStatus delete(MasterPricingEntities dbContext, FilterId filterId, FilterViewId filterViewId)
        {
            try
            {
                dbContext.FilterColumns.RemoveRange(dbContext.FilterColumns.Where(
                        r => r.FilterId == filterId.Id && r.FilterEntityTypeId == FilterEntityType.View && r.FilterEntityId == filterViewId.Id));
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
                dbContext.FilterColumns.RemoveRange(dbContext.FilterColumns.Where(r => r.FilterId == filterId.Id));
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
