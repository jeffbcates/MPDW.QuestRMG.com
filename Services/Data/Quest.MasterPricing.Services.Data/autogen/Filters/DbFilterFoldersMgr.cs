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


namespace Quest.MasterPricing.Services.Data.FilterFolders
{
    public class DbFilterFoldersMgr : DbMgrSessionBased
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
        public DbFilterFoldersMgr(UserSession userSession)
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
        public questStatus Create(Quest.Functional.MasterPricing.FilterFolder filterFolder, out FilterFolderId filterFolderId)
        {
            // Initialize
            questStatus status = null;
            filterFolderId = null;


            // Data rules.


            // Create the filterFolder
            using (MasterPricingEntities dbContext = new MasterPricingEntities())
            {
                status = create(dbContext, filterFolder, out filterFolderId);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Create(DbMgrTransaction trans, Quest.Functional.MasterPricing.FilterFolder filterFolder, out FilterFolderId filterFolderId)
        {
            // Initialize
            questStatus status = null;
            filterFolderId = null;


            // Data rules.


            // Create the filterFolder in this transaction.
            status = create((MasterPricingEntities)trans.DbContext, filterFolder, out filterFolderId);
            if (! questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Create(DbMgrTransaction trans, List<Quest.Functional.MasterPricing.FilterFolder> filterFolderList, out List<Quest.Functional.MasterPricing.FilterFolder> filterFolderIdList)
        {
            // Initialize
            questStatus status = null;
            filterFolderIdList = null;


            // Data rules.


            // Create the filterFolders in this transaction.
            status = create((MasterPricingEntities)trans.DbContext, filterFolderList, out filterFolderIdList);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Read(FilterFolderId filterFolderId, out Quest.Functional.MasterPricing.FilterFolder filterFolder)
        {
            // Initialize
            questStatus status = null;
            filterFolder = null;


            // Perform read
            using (MasterPricingEntities dbContext = new MasterPricingEntities())
            {
                Quest.Services.Dbio.MasterPricing.FilterFolders _filterFolders = null;
                status = read(dbContext, filterFolderId, out _filterFolders);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }
                filterFolder = new Quest.Functional.MasterPricing.FilterFolder();
                BufferMgr.TransferBuffer(_filterFolders, filterFolder);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Read(DbMgrTransaction trans, FilterFolderId filterFolderId, out Quest.Functional.MasterPricing.FilterFolder filterFolder)
        {
            // Initialize
            questStatus status = null;
            filterFolder = null;


            // Perform read
            Quest.Services.Dbio.MasterPricing.FilterFolders _filterFolders = null;
            status = read((MasterPricingEntities)trans.DbContext, filterFolderId, out _filterFolders);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            filterFolder = new Quest.Functional.MasterPricing.FilterFolder();
            BufferMgr.TransferBuffer(_filterFolders, filterFolder);

            return (new questStatus(Severity.Success));
        }
        public questStatus Read(FolderId folderId, out List<Quest.Functional.MasterPricing.FilterFolder> filterFolderList)
        {
            // Initialize
            questStatus status = null;
            filterFolderList = null;


            // Perform read
            using (MasterPricingEntities dbContext = new MasterPricingEntities())
            {
                List<Quest.Services.Dbio.MasterPricing.FilterFolders> _filterFoldersList = null;
                status = read(dbContext, folderId, out _filterFoldersList);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }
                filterFolderList = new List<FilterFolder>();
                foreach (Quest.Services.Dbio.MasterPricing.FilterFolders _filterFolder in _filterFoldersList)
                {
                    Quest.Functional.MasterPricing.FilterFolder filterFolder = new Quest.Functional.MasterPricing.FilterFolder();
                    BufferMgr.TransferBuffer(_filterFolder, filterFolder);
                    filterFolderList.Add(filterFolder);
                }
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Read(DbMgrTransaction trans, FolderId folderId, out List<Quest.Functional.MasterPricing.FilterFolder> filterFolderList)
        {
            // Initialize
            questStatus status = null;
            filterFolderList = null;


            // Perform read
            List<Quest.Services.Dbio.MasterPricing.FilterFolders> _filterFoldersList = null;
            status = read((MasterPricingEntities)trans.DbContext, folderId, out _filterFoldersList);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            filterFolderList = new List<FilterFolder>();
            foreach (Quest.Services.Dbio.MasterPricing.FilterFolders _filterFolder in _filterFoldersList)
            {
                Quest.Functional.MasterPricing.FilterFolder filterFolder = new Quest.Functional.MasterPricing.FilterFolder();
                BufferMgr.TransferBuffer(_filterFolder, filterFolder);
                filterFolderList.Add(filterFolder);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Update(Quest.Functional.MasterPricing.FilterFolder filterFolder)
        {
            // Initialize
            questStatus status = null;


            // Perform update.
            using (MasterPricingEntities dbContext = new MasterPricingEntities())
            {
                status = update(dbContext, filterFolder);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Update(DbMgrTransaction trans, Quest.Functional.MasterPricing.FilterFolder filterFolder)
        {
            // Initialize
            questStatus status = null;
            bool bCreateTransaction = trans == null;


            // Perform update in this transaction.
            status = update((MasterPricingEntities)trans.DbContext, filterFolder);
            if (!questStatusDef.IsSuccess(status))
            {
                RollbackTransaction(trans);
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Delete(FilterFolderId filterFolderId)
        {
            // Initialize
            questStatus status = null;


            // Perform delete.
            using (MasterPricingEntities dbContext = new MasterPricingEntities())
            {
                status = delete(dbContext, filterFolderId);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Delete(DbMgrTransaction trans, FilterFolderId filterFolderId)
        {
            // Initialize
            questStatus status = null;


            // Perform delete in this transaction.
            status = delete((MasterPricingEntities)trans.DbContext, filterFolderId);
            if (! questStatusDef.IsSuccess(status))
            {
                RollbackTransaction(trans);
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Delete(FolderId folderId)
        {
            // Initialize
            questStatus status = null;


            // Perform delete.
            using (MasterPricingEntities dbContext = new MasterPricingEntities())
            {
                status = delete(dbContext, folderId);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Delete(DbMgrTransaction trans, FolderId folderId)
        {
            // Initialize
            questStatus status = null;


            // Perform delete in this transaction.
            status = delete((MasterPricingEntities)trans.DbContext, folderId);
            if (!questStatusDef.IsSuccess(status))
            {
                RollbackTransaction(trans);
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus List(QueryOptions queryOptions, out List<Quest.Functional.MasterPricing.FilterFolder> filterFolderList, out QueryResponse queryResponse)
        {
            // Initialize
            questStatus status = null;
            filterFolderList = null;
            queryResponse = null;


            using (MasterPricingEntities dbContext = new MasterPricingEntities())
            {
                using (DbContextTransaction tx = dbContext.Database.BeginTransaction(IsolationLevel.ReadUncommitted))
                {
                    try
                    {
                        PropertyInfo[] dbProperties = typeof(Quest.Services.Dbio.MasterPricing.FilterFolders).GetProperties().ToArray();
                        int totalRecords = dbContext.FilterFolders.Where(BuildWhereClause(queryOptions, dbProperties)).Count();
                        List<Quest.Services.Dbio.MasterPricing.FilterFolders> _filterFoldersList = dbContext.FilterFolders.Where(BuildWhereClause(queryOptions, dbProperties))
                                .OrderBy(BuildSortString(queryOptions.SortColumns))
                                .Skip(queryOptions.Paging.PageSize * (queryOptions.Paging.PageNumber - 1))
                                .Take(queryOptions.Paging.PageSize).ToList();
                        if (_filterFoldersList == null)
                        {
                            return (new questStatus(Severity.Warning));
                        }
                        filterFolderList = new List<Quest.Functional.MasterPricing.FilterFolder>();
                        foreach (Quest.Services.Dbio.MasterPricing.FilterFolders _filterFolders in _filterFoldersList)
                        {
                            Quest.Functional.MasterPricing.FilterFolder filterFolder = new Quest.Functional.MasterPricing.FilterFolder();
                            BufferMgr.TransferBuffer(_filterFolders, filterFolder);
                            filterFolderList.Add(filterFolder);
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


        #region FilterFolders
        /*----------------------------------------------------------------------------------------------------------------------------------
         * FilterFolders
         *---------------------------------------------------------------------------------------------------------------------------------*/
        private questStatus create(MasterPricingEntities dbContext, Quest.Functional.MasterPricing.FilterFolder filterFolder, out FilterFolderId filterFolderId)
        {
            // Initialize
            filterFolderId = null;


            // Perform create
            try
            {
                Quest.Services.Dbio.MasterPricing.FilterFolders _filterFolders = new Quest.Services.Dbio.MasterPricing.FilterFolders();
                BufferMgr.TransferBuffer(filterFolder, _filterFolders);
                dbContext.FilterFolders.Add(_filterFolders);
                dbContext.SaveChanges();
                if (_filterFolders.Id == 0)
                {
                    return (new questStatus(Severity.Error, "Quest.Functional.MasterPricing.FilterFolder not created"));
                }
                filterFolderId = new FilterFolderId(_filterFolders.Id);
            }
            catch (System.Exception ex)
            {
                return (new questStatus(Severity.Fatal, String.Format("EXCEPTION: {0}.{1}: {2}",
                        this.GetType().Name, MethodBase.GetCurrentMethod().Name,
                        ex.InnerException != null ? ex.InnerException.Message : ex.Message)));
            }
            return (new questStatus(Severity.Success));
        }
        private questStatus create(MasterPricingEntities dbContext, List<Quest.Functional.MasterPricing.FilterFolder> filterFolderList, out List<Quest.Functional.MasterPricing.FilterFolder> filterFolderIdList)
        {
            // Initialize
            filterFolderIdList = null;


            // Perform create
            try
            {
                List<Quest.Services.Dbio.MasterPricing.FilterFolders> _filterFolderList = new List<Quest.Services.Dbio.MasterPricing.FilterFolders>();
                foreach (Quest.Functional.MasterPricing.FilterFolder filterFolder in filterFolderList)
                {
                    Quest.Services.Dbio.MasterPricing.FilterFolders _filterFolder = new Quest.Services.Dbio.MasterPricing.FilterFolders();
                    BufferMgr.TransferBuffer(filterFolder, _filterFolder);
                    _filterFolderList.Add(_filterFolder);
                }
                dbContext.FilterFolders.AddRange(_filterFolderList);
                dbContext.SaveChanges();

                filterFolderIdList = new List<FilterFolder>();
                foreach (Quest.Services.Dbio.MasterPricing.FilterFolders _filterFolder in _filterFolderList)
                {
                    Quest.Functional.MasterPricing.FilterFolder filterFolder = new FilterFolder();
                    filterFolder.Id = _filterFolder.Id;
                    filterFolderIdList.Add(filterFolder);
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
        private questStatus read(MasterPricingEntities dbContext, FilterFolderId filterFolderId, out Quest.Services.Dbio.MasterPricing.FilterFolders filterFolder)
        {
            // Initialize
            filterFolder = null;


            try
            {
                filterFolder = dbContext.FilterFolders.Where(r => r.Id == filterFolderId.Id).SingleOrDefault();
                if (filterFolder == null)
                {
                    return (new questStatus(Severity.Error, String.Format("ERROR: {0}.{1}: {2}",
                            this.GetType().Name, MethodBase.GetCurrentMethod().Name,
                            String.Format("FilterFolderId {0} not found", filterFolderId.Id))));
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
        private questStatus read(MasterPricingEntities dbContext, FolderId folderId, out List<Quest.Services.Dbio.MasterPricing.FilterFolders> filterFolderList)
        {
            // Initialize
            filterFolderList = null;


            try
            {
                if (folderId == null || folderId.Id < BaseId.VALID_ID)
                {
                    filterFolderList = dbContext.FilterFolders.Where(r => r.FolderId == null).ToList();
                }
                else
                {
                    filterFolderList = dbContext.FilterFolders.Where(r => r.FolderId == folderId.Id).ToList();
                }

                if (filterFolderList == null)
                {
                    return (new questStatus(Severity.Error, String.Format("ERROR: {0}.{1}: {2}",
                            this.GetType().Name, MethodBase.GetCurrentMethod().Name,
                            String.Format("FolderId {0} not found", folderId.Id))));
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
        private questStatus update(MasterPricingEntities dbContext, Quest.Functional.MasterPricing.FilterFolder filterFolder)
        {
            // Initialize 
            questStatus status = null;


            try
            {
                // Read the record.
                FilterFolderId filterFolderId = new FilterFolderId(filterFolder.Id);
                Quest.Services.Dbio.MasterPricing.FilterFolders _filterFolders = null;
                status = read(dbContext, filterFolderId, out _filterFolders);
                if (! questStatusDef.IsSuccess(status))
                {
                    return (status);
                }

                // Update the record.
                BufferMgr.TransferBuffer(filterFolder, _filterFolders);
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
        private questStatus delete(MasterPricingEntities dbContext, FilterFolderId filterFolderId)
        {
            try
            {
                dbContext.FilterFolders.RemoveRange(dbContext.FilterFolders.Where(r => r.Id == filterFolderId.Id));
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
        private questStatus delete(MasterPricingEntities dbContext, FolderId parentFolderId)
        {
            try
            {
                dbContext.FilterFolders.RemoveRange(dbContext.FilterFolders.Where(r => r.FolderId == parentFolderId.Id));
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
