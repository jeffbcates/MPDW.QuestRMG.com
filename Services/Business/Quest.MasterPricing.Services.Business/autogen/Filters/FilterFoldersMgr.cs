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
using Quest.MasterPricing.Services.Data.FilterFolders;


namespace Quest.MasterPricing.Services.Business.Filters
{
    public class FilterFoldersMgr : MgrSessionBased
    {
        #region Declarations
        /*==================================================================================================================================
         * Declarations
         *=================================================================================================================================*/
        private DbFilterFoldersMgr _dbFilterFoldersMgr = null;

        #endregion


        #region Constructors
        /*==================================================================================================================================
         * Constructors
         *=================================================================================================================================*/
        public FilterFoldersMgr(UserSession userSession)
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
        public questStatus Create(FilterFolder filterFolder, out FilterFolderId filterFolderId)
        {
            // Initialize
            questStatus status = null;
            filterFolderId = null;


            // Create filterFolder
            status = _dbFilterFoldersMgr.Create(filterFolder, out filterFolderId);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Create(DbMgrTransaction trans, FilterFolder filterFolder, out FilterFolderId filterFolderId)
        {
            // Initialize
            questStatus status = null;
            filterFolderId = null;


            // Create filterFolder
            status = _dbFilterFoldersMgr.Create(trans, filterFolder, out filterFolderId);
            if (!questStatusDef.IsSuccess(status))
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


            // Create filterFolder
            status = _dbFilterFoldersMgr.Create(trans, filterFolderList, out filterFolderIdList);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Read(FilterFolderId filterFolderId, out FilterFolder filterFolder)
        {
            // Initialize
            questStatus status = null;
            filterFolder = null;


            // Read filterFolder
            status = _dbFilterFoldersMgr.Read(filterFolderId, out filterFolder);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Read(DbMgrTransaction trans, FilterFolderId filterFolderId, out FilterFolder filterFolder)
        {
            // Initialize
            questStatus status = null;
            filterFolder = null;


            // Read filterFolder
            status = _dbFilterFoldersMgr.Read(trans, filterFolderId, out filterFolder);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Read(FolderId folderId, out List<Quest.Functional.MasterPricing.FilterFolder> filterFolderList)
        {
            // Initialize
            questStatus status = null;
            filterFolderList = null;


            // Read filterFolder
            status = _dbFilterFoldersMgr.Read(folderId, out filterFolderList);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Read(DbMgrTransaction trans, FolderId folderId, out List<Quest.Functional.MasterPricing.FilterFolder> filterFolderList)
        {
            // Initialize
            questStatus status = null;
            filterFolderList = null;


            // Read filterFolder
            status = _dbFilterFoldersMgr.Read(trans, folderId, out filterFolderList);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Update(FilterFolder filterFolder)
        {
            // Initialize
            questStatus status = null;


            // Update filterFolder
            status = _dbFilterFoldersMgr.Update(filterFolder);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Update(DbMgrTransaction trans, FilterFolder filterFolder)
        {
            // Initialize
            questStatus status = null;


            // Update filterFolder
            status = _dbFilterFoldersMgr.Update(trans, filterFolder);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Delete(FilterFolderId filterFolderId)
        {
            // Initialize
            questStatus status = null;

            // Delete filterFolder
            DbFilterFoldersMgr dbFilterFoldesrMgr = new DbFilterFoldersMgr(this.UserSession);
            status = dbFilterFoldesrMgr.Delete(filterFolderId);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Delete(DbMgrTransaction trans, FilterFolderId filterFolderId)
        {
            // Initialize
            questStatus status = null;


            // Delete filterFolder
            status = _dbFilterFoldersMgr.Delete(trans, filterFolderId);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Delete(FolderId folderId)
        {
            // Initialize
            questStatus status = null;


            // Delete all filterFolders in this table.
            status = _dbFilterFoldersMgr.Delete(folderId);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Delete(DbMgrTransaction trans, FolderId folderId)
        {
            // Initialize
            questStatus status = null;


            // Delete all filterFolders in this table.
            status = _dbFilterFoldersMgr.Delete(trans, folderId);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus List(QueryOptions queryOptions, out List<FilterFolder> filterFolderList, out QueryResponse queryResponse)
        {
            // Initialize
            questStatus status = null;
            filterFolderList = null;


            // List
            status = _dbFilterFoldersMgr.List(queryOptions, out filterFolderList, out queryResponse);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Load(FolderId folderId, out List<FilterFolder> filterFolderList)
        {
            // Initialize
            questStatus status = null;
            filterFolderList = null;
            FiltersMgr filtersMgr = new FiltersMgr(this.UserSession);


            // Read filterFolder
            status = _dbFilterFoldersMgr.Read(folderId, out filterFolderList);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            filterFolderList.Sort(delegate (FilterFolder i1, FilterFolder i2) { return i1.Name.CompareTo(i2.Name); });


            // Read filters in this folder, if we have a folderId
            if (folderId != null && folderId.Id >= BaseId.VALID_ID)
            {
                List<Filter> filterList = null;
                status = filtersMgr.Read(folderId, out filterList);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }
                filterList.Sort(delegate (Filter i1, Filter i2) { return i1.Name.CompareTo(i2.Name); });
            }


            // Recursively load all child folders
            foreach (FilterFolder filterFolder in filterFolderList)
            {
                FolderId subFolderId = new FolderId(filterFolder.Id);
                List<FilterFolder> subFolderList = null;
                status = Load(subFolderId, out subFolderList);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }
                subFolderList.Sort(delegate (FilterFolder i1, FilterFolder i2) { return i1.Name.CompareTo(i2.Name); });
                filterFolder.Folders.AddRange(subFolderList);


                List<Filter> filterList = null;
                status = filtersMgr.Read(subFolderId, out filterList);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }
                filterList.Sort(delegate (Filter i1, Filter i2) { return i1.Name.CompareTo(i2.Name); });
                filterFolder.Filters = filterList;
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
                _dbFilterFoldersMgr = new DbFilterFoldersMgr(this.UserSession);
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

