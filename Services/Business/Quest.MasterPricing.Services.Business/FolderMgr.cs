using System;
using System.IO;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Reflection;
using System.Threading.Tasks;
using Quest.Util.Status;
using Quest.Util.Buffer;
using Quest.Util.Data;
using Quest.Functional.ASM;
using Quest.Functional.FMS;
using Quest.Functional.Logging;
using Quest.Functional.MasterPricing;
using Quest.MPDW.Services.Data;
using Quest.MPDW.Services.Business;
using Quest.MasterPricing.Services.Business.Database;
using Quest.MasterPricing.Services.Data.Filters;
using Quest.MasterPricing.Services.Data.Database;
using Quest.Services.Data.Logging;


namespace Quest.MasterPricing.Services.Business.Filters
{
    public class FolderMgr : Mgr
    {
        #region Declarations
        /*==================================================================================================================================
         * Declarations
         *=================================================================================================================================*/
        private FilterFoldersMgr _filterFoldersMgr = null;
        private FilterMgr _filterMgr = null;
        private DbFolderMgr _dbFolderMgr = null;
        private UserSession _userSession = null;

        #endregion


        #region Constructors
        /*==================================================================================================================================
         * Constructors
         *=================================================================================================================================*/
        public FolderMgr()
            : base()
        {
            initialize();
        }
        public FolderMgr(UserSession userSession)
            : base(userSession)
        {
            _userSession = userSession;
            initialize();
        }
        #endregion


        #region Properties
        /*==================================================================================================================================
         * Properties
         *=================================================================================================================================*/
        public UserSession UserSession
        {
            get
            {
                return (this._userSession);
            }
        }
        #endregion


        #region Public Methods
        /*==================================================================================================================================
         * Public Methods
         *=================================================================================================================================*/
        public questStatus Delete(FilterFolderId filterFolderId)
        {
            // Initialize
            questStatus status = null;
            DbMgrTransaction trans = null;
            Mgr mgr = new Mgr(this.UserSession);

            try
            {
                // BEGIN TRANSACTION
                status = mgr.BeginTransaction("DeleteFolder_" + filterFolderId.Id.ToString() + "_" + Guid.NewGuid().ToString(), out trans);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }

                // Load folder contents
                FolderId folderId = new FolderId(filterFolderId.Id);
                List<FilterFolder> filterFolderList = null;
                FilterFoldersMgr filterFoldersMgr = new FilterFoldersMgr(this.UserSession);
                status = filterFoldersMgr.Load(folderId, out filterFolderList);
                if (!questStatusDef.IsSuccess(status))
                {
                    mgr.RollbackTransaction(trans);
                    return (status);
                }

                // Clear out the folder
                status = clearFolder(trans, filterFolderList);
                if (!questStatusDef.IsSuccess(status))
                {
                    mgr.RollbackTransaction(trans);
                    return (status);
                }

                // Get fitlers in this folder.
                List<Quest.Functional.MasterPricing.Filter> filterList = null;
                FiltersMgr filtersMgr = new FiltersMgr(this.UserSession);
                status = filtersMgr.Read(folderId, out filterList);
                if (!questStatusDef.IsSuccess(status))
                {
                    mgr.RollbackTransaction(trans);
                    return (status);
                }

                // Delete filters in this folder
                foreach (Filter filter in filterList)
                {
                    FilterId filterId = new FilterId(filter.Id);
                    status = _dbFolderMgr.DeleteFilter(filterId);
                    if (!questStatusDef.IsSuccess(status))
                    {
                        return (status);
                    }
                }

                // Delete the folder.
                status = _filterFoldersMgr.Delete(trans, filterFolderId);
                if (!questStatusDef.IsSuccess(status))
                {
                    mgr.RollbackTransaction(trans);
                    return (status);
                }

                // COMMIT TRANSACTION
                status = mgr.CommitTransaction(trans);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }
            }
            catch (System.Exception ex)
            {
                if (trans != null)
                {
                    mgr.RollbackTransaction(trans);
                }
                return (new questStatus(Severity.Fatal, String.Format("EXCEPTION: {0}.{1}: {2}",
                        this.GetType().Name, MethodBase.GetCurrentMethod().Name,
                        ex.InnerException != null ? ex.InnerException.Message : ex.Message)));
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
                _filterFoldersMgr = new FilterFoldersMgr(this.UserSession);
                _filterMgr = new FilterMgr(this.UserSession);
                _dbFolderMgr = new DbFolderMgr(this.UserSession);
            }
            catch (System.Exception ex)
            {
                status = new questStatus(Severity.Fatal, String.Format("EXCEPTION: {0}.{1}: {2}",
                        this.GetType().ToString(), MethodInfo.GetCurrentMethod().Name, ex.Message));
                throw new System.Exception(status.Message, ex);
            }
            return (new questStatus(Severity.Success));
        }
        
        private questStatus clearFolder(DbMgrTransaction trans, List<FilterFolder> filterFolderList)
        {
            // Initialize
            questStatus status = null;


            // Navigate to bottom of folder hierarchy...
            foreach (FilterFolder filterFolder in filterFolderList)
            {
                status = clearFolder(trans, filterFolder.Folders);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }

                // Delete filters in the folder.
                foreach (Filter filter in filterFolder.Filters)
                {
                    FilterId filterId = new FilterId(filter.Id);
                    status = _dbFolderMgr.DeleteFilter(filterId);
                    if (!questStatusDef.IsSuccess(status))
                    {
                        return (status);
                    }
                }

                // Delete the folder.
                FilterFolderId filterFolderId = new FilterFolderId(filterFolder.Id);
                status = _filterFoldersMgr.Delete(trans, filterFolderId);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }
            }
            return (new questStatus(Severity.Success));
        }
        #endregion
    }
}

