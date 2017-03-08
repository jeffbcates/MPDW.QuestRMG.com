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
using Quest.MPDW.Services.Data;
using Quest.MPDW.Services.Business;
using Quest.MPDW.Services.Data.Accounts;


namespace Quest.MPDW.Services.Business.Accounts
{
    public class PrivilegesMgr : Mgr
    {
        #region Declarations
        /*==================================================================================================================================
         * Declarations
         *=================================================================================================================================*/
        private DbPrivilegesMgr _dbPrivilegesMgr = null;

        #endregion


        #region Constructors
        /*==================================================================================================================================
         * Constructors
         *=================================================================================================================================*/
        public PrivilegesMgr()
            : base()
        {
            initialize();
        }
        public PrivilegesMgr(UserSession userSession)
            : base()
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
        public questStatus Create(Privilege privilege, out PrivilegeId privilegeId)
        {
            // Initialize
            privilegeId = null;
            questStatus status = null;


            // Create privilege
            status = _dbPrivilegesMgr.Create(privilege, out privilegeId);
            if (! questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Read(PrivilegeId privilegeId, out Privilege privilege)
        {
            // Initialize
            privilege = null;
            questStatus status = null;


            // Read privilege
            status = _dbPrivilegesMgr.Read(privilegeId, out privilege);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Update(Privilege privilege)
        {
            // Initialize
            questStatus status = null;


            // Update privilege
            status = _dbPrivilegesMgr.Update(privilege);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Delete(PrivilegeId privilegeId)
        {
            // Initialize
            questStatus status = null;


            // Delete privilege
            status = _dbPrivilegesMgr.Delete(privilegeId);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus List(QueryOptions queryOptions, out List<Privilege> privilegeList, out QueryResponse queryResponse)
        {
            // Initialize
            questStatus status = null;
            privilegeList = null;


            // List privileges
            status = _dbPrivilegesMgr.List(queryOptions, out privilegeList, out queryResponse);
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
                _dbPrivilegesMgr = new DbPrivilegesMgr(new UserSession()); // TODO: UNSTUB THIS LATER.
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
