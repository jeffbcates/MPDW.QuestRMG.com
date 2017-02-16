using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Quest.Util.Status;
using Quest.Util.Buffer;
using Quest.Services.Dbio.MasterPricing;


namespace Quest.MPDW.Services.Data
{
    public class DbMgrTransaction
    {
        #region Declarations
        /*==================================================================================================================================
         * Declarations
         *=================================================================================================================================*/
        private MasterPricingEntities _dbContext = null;
        private DbContextTransaction _transaction = null;
        private string _name = null;

        #endregion


        #region Constructors
        /*==================================================================================================================================
         * Constructors
         *=================================================================================================================================*/
        public DbMgrTransaction(string name)
        {
            _name = name;
            initialize();
        }
        #endregion


        #region Properties
        /*==================================================================================================================================
         * Properties
         *=================================================================================================================================*/
        public DbContext DbContext
        {
            get
            {
                return (this._dbContext);
            }
        }
        public string TransactionName
        {
            get
            {
                return (this._name);
            }
        }
        #endregion


        #region Public Methods
        /*==================================================================================================================================
         * Public Methods
         *=================================================================================================================================*/
        public questStatus BeginTransaction()
        {
            try
            {
                _transaction = _dbContext.Database.BeginTransaction();
            }
            catch (System.Exception ex)
            {
                return (new questStatus(Severity.Error, String.Format("EXCEPTION: starting transaction {0}: {1}", _name, ex.Message)));
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus RollbackTransaction()
        {
            if (_transaction == null)
            {
                return (new questStatus(Severity.Error, String.Format("Error: there is no current transaction to rollback {0}", _name)));
            }
            _transaction.Rollback();
            _transaction.Dispose();
            _transaction = null;

            return (new questStatus(Severity.Success));
        }
        public questStatus CommitTransaction()
        {
            if (_transaction == null)
            {
                return (new questStatus(Severity.Error, String.Format("Error: there is no current transaction to commit {0}", _name)));
            }
            _transaction.Commit();
            _transaction.Dispose();
            _transaction = null;

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
                _dbContext = new MasterPricingEntities();
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
