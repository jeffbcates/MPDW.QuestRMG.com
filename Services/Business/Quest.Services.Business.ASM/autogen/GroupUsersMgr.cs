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
    public class GroupUsersMgr : MgrSessionBased
    {
        #region Declarations
        /*==================================================================================================================================
         * Declarations
         *=================================================================================================================================*/
        private DbGroupUsersMgr _dbGroupUsersMgr = null;

        #endregion


        #region Constructors
        /*==================================================================================================================================
         * Constructors
         *=================================================================================================================================*/
        public GroupUsersMgr(UserSession userSession)
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
        public questStatus Create(GroupUser groupUser, out GroupUserId groupUserId)
        {
            // Initialize
            questStatus status = null;
            groupUserId = null;



            // Create groupUser
            status = _dbGroupUsersMgr.Create(groupUser, out groupUserId);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Create(DbMgrTransaction trans, GroupUser groupUser, out GroupUserId groupUserId)
        {
            // Initialize
            questStatus status = null;
            groupUserId = null;



            // Create groupUser
            status = _dbGroupUsersMgr.Create(trans, groupUser, out groupUserId);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Read(GroupUserId groupUserId, out GroupUser groupUser)
        {
            // Initialize
            groupUser = null;
            questStatus status = null;


            // Read groupUser
            status = _dbGroupUsersMgr.Read(groupUserId, out groupUser);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Read(DbMgrTransaction trans, GroupUserId groupUserId, out GroupUser groupUser)
        {
            // Initialize
            questStatus status = null;
            groupUser = null;


            // Read groupUser
            status = _dbGroupUsersMgr.Read(trans, groupUserId, out groupUser);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Read(GroupId groupId, out GroupUserList groupUserList)
        {
            // Initialize
            questStatus status = null;
            groupUserList = null;


            // Read groupUser
            status = _dbGroupUsersMgr.Read(groupId, out groupUserList);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Read(DbMgrTransaction trans, GroupId groupId, out GroupUserList groupUserList)
        {
            // Initialize
            questStatus status = null;
            groupUserList = null;


            // Read groupUser
            status = _dbGroupUsersMgr.Read(trans, groupId, out groupUserList);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Read(UserId userId, out UserGroupList userGroupList)
        {
            // Initialize
            questStatus status = null;
            userGroupList = null;


            // Read groupUser
            status = _dbGroupUsersMgr.Read(userId, out userGroupList);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Read(DbMgrTransaction trans, UserId userId, out UserGroupList userGroupList)
        {
            // Initialize
            questStatus status = null;
            userGroupList = null;


            // Read groupUser
            status = _dbGroupUsersMgr.Read(trans, userId, out userGroupList);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Update(GroupUser groupUser)
        {
            // Initialize
            questStatus status = null;


            // Update groupUser
            status = _dbGroupUsersMgr.Update(groupUser);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Update(DbMgrTransaction trans, GroupUser groupUser)
        {
            // Initialize
            questStatus status = null;


            // Update groupUser
            status = _dbGroupUsersMgr.Update(trans, groupUser);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Delete(GroupUserId groupUserId)
        {
            // Initialize
            questStatus status = null;


            // Delete groupUser
            status = _dbGroupUsersMgr.Delete(groupUserId);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Delete(DbMgrTransaction trans, GroupUserId groupUserId)
        {
            // Initialize
            questStatus status = null;


            // Delete groupUser
            status = _dbGroupUsersMgr.Delete(trans, groupUserId);
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


            // Delete groupUser
            status = _dbGroupUsersMgr.Delete(groupId);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Delete(DbMgrTransaction trans, GroupId groupId)
        {
            // Initialize
            questStatus status = null;


            // Delete groupUser
            status = _dbGroupUsersMgr.Delete(trans, groupId);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Delete(UserId userId)
        {
            // Initialize
            questStatus status = null;


            // Delete groupUser
            status = _dbGroupUsersMgr.Delete(userId);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Delete(DbMgrTransaction trans, UserId userId)
        {
            // Initialize
            questStatus status = null;


            // Delete groupUser
            status = _dbGroupUsersMgr.Delete(trans, userId);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus List(QueryOptions queryOptions, out List<GroupUser> groupUserList, out QueryResponse queryResponse)
        {
            // Initialize
            questStatus status = null;
            groupUserList = null;


            // List groupUsers
            status = _dbGroupUsersMgr.List(queryOptions, out groupUserList, out queryResponse);
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
                _dbGroupUsersMgr = new DbGroupUsersMgr(this.UserSession); 
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
