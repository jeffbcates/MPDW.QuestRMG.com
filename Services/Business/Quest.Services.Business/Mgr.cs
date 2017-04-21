using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Quest.Functional.ASM;
using Quest.Functional.FMS;
using Quest.Util.Status;
using Quest.Util.Buffer;
using Quest.MPDW.Services.Data;


namespace Quest.MPDW.Services.Business
{
    public class Mgr : DbMgr
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
        public Mgr()
            : base()
        {
            initialize();
        }
        public Mgr(UserSession userSession)
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

        #region Transactions
        /*----------------------------------------------------------------------------------------------------------------------------------
         * Transactions
         *---------------------------------------------------------------------------------------------------------------------------------*/
        public new questStatus BeginTransaction(string transactionName, out DbMgrTransaction trans)
        {
            // Initialize
            questStatus status = null;


            status = base.BeginTransaction(transactionName, out trans);
            if (! questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public new questStatus BeginTransaction(string database, string transactionName, out DbMgrTransaction trans)
        {
            // Initialize
            questStatus status = null;


            status = base.BeginTransaction(database, transactionName, out trans);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public new questStatus RollbackTransaction(DbMgrTransaction trans)
        {
            // Initialize
            questStatus status = null;


            status = base.RollbackTransaction(trans);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public new questStatus CommitTransaction(DbMgrTransaction trans)
        {
            // Initialize
            questStatus status = null;


            status = base.CommitTransaction(trans);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
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
