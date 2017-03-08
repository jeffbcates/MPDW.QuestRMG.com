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
    public class TypeListsMgr : MgrSessionBased
    {
        #region Declarations
        /*==================================================================================================================================
         * Declarations
         *=================================================================================================================================*/
        private DbTypeListsMgr _dbTypeListsMgr = null;

        #endregion


        #region Constructors
        /*==================================================================================================================================
         * Constructors
         *=================================================================================================================================*/
        public TypeListsMgr(UserSession userSession)
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
        public questStatus Create(TypeList typeList, out TypeListId typeListId)
        {
            // Initialize
            typeListId = null;
            questStatus status = null;


            // Create typeList
            status = _dbTypeListsMgr.Create(typeList, out typeListId);
            if (! questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Create(DbMgrTransaction trans, TypeList typeList, out TypeListId typeListId)
        {
            // Initialize
            typeListId = null;
            questStatus status = null;


            // Create typeList
            status = _dbTypeListsMgr.Create(trans, typeList, out typeListId);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Read(TypeListId typeListId, out TypeList typeList)
        {
            // Initialize
            typeList = null;
            questStatus status = null;


            // Read typeList
            status = _dbTypeListsMgr.Read(typeListId, out typeList);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Read(DbMgrTransaction trans, TypeListId typeListId, out TypeList typeList)
        {
            // Initialize
            typeList = null;
            questStatus status = null;


            // Read typeList
            status = _dbTypeListsMgr.Read(trans, typeListId, out typeList);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Update(TypeList typeList)
        {
            // Initialize
            questStatus status = null;


            // Update typeList
            status = _dbTypeListsMgr.Update(typeList);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Update(DbMgrTransaction trans, TypeList typeList)
        {
            // Initialize
            questStatus status = null;


            // Update typeList
            status = _dbTypeListsMgr.Update(trans, typeList);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Delete(TypeListId typeListId)
        {
            // Initialize
            questStatus status = null;


            // Delete typeList
            status = _dbTypeListsMgr.Delete(typeListId);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Delete(DbMgrTransaction trans, TypeListId typeListId)
        {
            // Initialize
            questStatus status = null;


            // Delete typeList
            status = _dbTypeListsMgr.Delete(trans, typeListId);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus List(QueryOptions queryOptions, out List<TypeList> typeListList, out QueryResponse queryResponse)
        {
            // Initialize
            questStatus status = null;
            typeListList = null;


            // List usStates
            status = _dbTypeListsMgr.List(queryOptions, out typeListList, out queryResponse);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
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
                _dbTypeListsMgr = new DbTypeListsMgr(this.UserSession);
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

