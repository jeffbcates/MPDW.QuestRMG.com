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
using Quest.MasterPricing.Services.Data.Database;


namespace Quest.MasterPricing.Services.Business.Database
{
    public class ViewsMgr : MgrSessionBased
    {
        #region Declarations
        /*==================================================================================================================================
         * Declarations
         *=================================================================================================================================*/
        private DbViewsMgr _dbViewsMgr = null;

        #endregion


        #region Constructors
        /*==================================================================================================================================
         * Constructors
         *=================================================================================================================================*/
        public ViewsMgr(UserSession userSession)
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

        #region CRUD Operations
        //----------------------------------------------------------------------------------------------------------------------------------
        // CRUD Operations
        //----------------------------------------------------------------------------------------------------------------------------------
        public questStatus Create(View view, out ViewId viewId)
        {
            // Initialize
            viewId = null;
            questStatus status = null;


            // Create view
            status = _dbViewsMgr.Create(view, out viewId);
            if (! questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Create(DbMgrTransaction trans, View view, out ViewId viewId)
        {
            // Initialize
            questStatus status = null;
            viewId = null;


            // Create view
            status = _dbViewsMgr.Create(trans, view, out viewId);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Create(DbMgrTransaction trans, List<Quest.Functional.MasterPricing.View> viewList, out List<Quest.Functional.MasterPricing.View> viewIdList)
        {
            // Initialize
            questStatus status = null;
            viewIdList = null;


            // Create view
            status = _dbViewsMgr.Create(trans, viewList, out viewIdList);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Read(ViewId viewId, out View view)
        {
            // Initialize
            questStatus status = null;
            view = null;


            // Read view
            status = _dbViewsMgr.Read(viewId, out view);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Read(DbMgrTransaction trans, ViewId viewId, out View view)
        {
            // Initialize
            questStatus status = null;
            view = null;


            // Read view
            status = _dbViewsMgr.Read(trans, viewId, out view);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Update(View view)
        {
            // Initialize
            questStatus status = null;


            // Update view
            status = _dbViewsMgr.Update(view);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Update(DbMgrTransaction trans, View view)
        {
            // Initialize
            questStatus status = null;


            // Update view
            status = _dbViewsMgr.Update(trans, view);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Delete(ViewId viewId)
        {
            // Initialize
            questStatus status = null;


            // Delete view
            status = _dbViewsMgr.Delete(viewId);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Delete(DbMgrTransaction trans, ViewId viewId)
        {
            // Initialize
            questStatus status = null;


            // Delete view
            status = _dbViewsMgr.Delete(trans, viewId);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Delete(DatabaseId databaseId)
        {
            // Initialize
            questStatus status = null;


            // Delete view
            status = _dbViewsMgr.Delete(databaseId);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Delete(DbMgrTransaction trans, DatabaseId databaseId)
        {
            // Initialize
            questStatus status = null;


            // Delete view
            status = _dbViewsMgr.Delete(trans, databaseId);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus List(QueryOptions queryOptions, out List<View> viewList, out QueryResponse queryResponse)
        {
            // Initialize
            questStatus status = null;
            viewList = null;


            // List views
            status = _dbViewsMgr.List(queryOptions, out viewList, out queryResponse);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        #endregion

        #region Business Operations
        //----------------------------------------------------------------------------------------------------------------------------------
        // Business Operations
        //----------------------------------------------------------------------------------------------------------------------------------
        public questStatus GetDatabaseViews()
        {
            return (new questStatus(Severity.Warning, "Not implemented"));
        }
        #endregion

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
                _dbViewsMgr = new DbViewsMgr(this.UserSession);
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

