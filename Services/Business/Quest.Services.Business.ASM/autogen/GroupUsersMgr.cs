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
            groupUserId = null;
            questStatus status = null;



            // Create groupUser
            status = _dbGroupUsersMgr.Create(groupUser, out groupUserId);
            if (! questStatusDef.IsSuccess(status))
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
