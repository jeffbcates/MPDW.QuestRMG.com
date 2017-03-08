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
    public class GroupsMgr : Mgr
    {
        #region Declarations
        /*==================================================================================================================================
         * Declarations
         *=================================================================================================================================*/
        private DbGroupsMgr _dbGroupsMgr = null;

        #endregion


        #region Constructors
        /*==================================================================================================================================
         * Constructors
         *=================================================================================================================================*/
        public GroupsMgr()
            : base()
        {
            initialize();
        }
        public GroupsMgr(UserSession userSession)
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
        public questStatus Create(Group group, out GroupId groupId)
        {
            // Initialize
            groupId = null;
            questStatus status = null;


            // Create group
            status = _dbGroupsMgr.Create(group, out groupId);
            if (! questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Read(GroupId groupId, out Group group)
        {
            // Initialize
            group = null;
            questStatus status = null;


            // Read group
            status = _dbGroupsMgr.Read(groupId, out group);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Update(Group group)
        {
            // Initialize
            questStatus status = null;


            // Update group
            status = _dbGroupsMgr.Update(group);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Delete(GroupId groupId)
        {
            // Initialize
            questStatus status = null;


            // Delete group
            status = _dbGroupsMgr.Delete(groupId);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus List(QueryOptions queryOptions, out List<Group> groupList, out QueryResponse queryResponse)
        {
            // Initialize
            questStatus status = null;
            groupList = null;


            // List groups
            status = _dbGroupsMgr.List(queryOptions, out groupList, out queryResponse);
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
                _dbGroupsMgr = new DbGroupsMgr(new UserSession()); // TODO: UNSTUB THIS LATER.
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
