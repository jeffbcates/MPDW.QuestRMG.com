using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Linq;
using System.Linq.Dynamic;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Quest.Util.Status;
using Quest.Util.Buffer;
using Quest.Util.Data;
using Quest.Util.Encryption;
using Quest.Functional.ASM;
using Quest.MPDW.Services.Data;
using Quest.Services.Dbio.FMS;


namespace Quest.MPDW.Services.Data.Accounts
{
    public class DbUsersMgr : DbMgrSessionBased
    {
        #region Declarations
        /*==================================================================================================================================
         * Declarations
         *=================================================================================================================================*/
        AESEncryption _aesEncryption = null;
        #endregion


        #region Constructors
        /*==================================================================================================================================
         * Constructors
         *=================================================================================================================================*/
        public DbUsersMgr(UserSession userSession)
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
        public questStatus Create(User user, out UserId userId)
        {
            // Initialize
            questStatus status = null;
            userId = null;


            // Data rules.
            user.Created = DateTime.Now;
            user.Password = _aesEncryption.Encrypt(user.Password);


            // Create the user
            using (FMSEntities dbContext = new FMSEntities())
            {
                status = create(dbContext, user, out userId);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Create(DbMgrTransaction trans, User user, out UserId userId)
        {
            // Initialize
            questStatus status = null;
            userId = null;


            // Data rules.
            user.Created = DateTime.Now;
            user.Password = _aesEncryption.Encrypt(user.Password);


            // Create the user in this transaction.
            status = create((FMSEntities)trans.DbContext, user, out userId);
            if (! questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Read(UserId userId, out User user)
        {
            // Initialize
            questStatus status = null;
            user = null;


            // Perform read
            using (FMSEntities dbContext = new FMSEntities())
            {
                Quest.Services.Dbio.FMS.Users _user = null;
                status = read(dbContext, userId, out _user);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }
                user = new User();
                BufferMgr.TransferBuffer(_user, user);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Read(string username, out User user)
        {
            // Initialize
            questStatus status = null;
            user = null;


            // Perform read
            using (FMSEntities dbContext = new FMSEntities())
            {
                Quest.Services.Dbio.FMS.Users _user = null;
                status = read(dbContext, username, out _user);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }
                user = new User();
                BufferMgr.TransferBuffer(_user, user);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Read(DbMgrTransaction trans, UserId userId, out User user)
        {
            // Initialize
            questStatus status = null;
            user = null;


            // Perform read.
            using (FMSEntities dbContext = new FMSEntities())
            {
                Quest.Services.Dbio.FMS.Users _user = null;
                status = read((FMSEntities)trans.DbContext, userId, out _user);
                if (! questStatusDef.IsSuccess(status))
                {
                    return (status);
                }
                user = new User();
                BufferMgr.TransferBuffer(_user, user);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Read(DbMgrTransaction trans, string username, out User user)
        {
            // Initialize
            questStatus status = null;
            user = null;


            // Perform transaction read.
            Quest.Services.Dbio.FMS.Users _user = null;
            status = read((FMSEntities)trans.DbContext, username, out _user);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            user = new User();
            BufferMgr.TransferBuffer(_user, user);
            
            return (new questStatus(Severity.Success));
        }
        public questStatus Update(User user)
        {
            // Initialize
            questStatus status = null;


            // Perform update.
            using (FMSEntities dbContext = new FMSEntities())
            {
                status = update(dbContext, user);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Update(DbMgrTransaction trans, User user)
        {
            // Initialize
            questStatus status = null;
            bool bCreateTransaction = trans == null;


            // Perform update in this transaction.
            status = update((FMSEntities)trans.DbContext, user);
            if (!questStatusDef.IsSuccess(status))
            {
                RollbackTransaction(trans);
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Delete(UserId userId)
        {
            // Initialize
            questStatus status = null;


            // Perform delete.
            using (FMSEntities dbContext = new FMSEntities())
            {
                status = delete(dbContext, userId);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Delete(DbMgrTransaction trans, UserId userId)
        {
            // Initialize
            questStatus status = null;


            // Perform delete in this transaction.
            status = delete((FMSEntities)trans.DbContext, userId);
            if (! questStatusDef.IsSuccess(status))
            {
                RollbackTransaction(trans);
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus List(QueryOptions queryOptions, out List<User> userList, out QueryResponse queryResponse)
        {
            // Initialize
            questStatus status = null;
            userList = null;
            queryResponse = null;


            using (FMSEntities dbContext = new FMSEntities())
            {
                using (DbContextTransaction tx = dbContext.Database.BeginTransaction(IsolationLevel.ReadUncommitted))
                {
                    try
                    {
                        // TEST
                        ////int _totalRecords2 = dbContext.Users.Where("CONVERT(date, Created) = '2016-07-08'").Count();
                        ////int _totalRecords2 = dbContext.Users.Where(String.Format("Created = ")).Count();

                        ////int _totalRecords2 = dbContext.Users.Where(string.Format("{0} = @0", "Created"),
                        ////                     DateTime.Parse("2016-07-08")).Count();

                        ////int _totalRecords2 = dbContext.Users.Where(string.Format("{0} == @0", "Created"),
                        ////                     DateTime.Parse("2016-07-08")).Count();

                        ////int _totalRecords2 = dbContext.Users.Where(String.Format("{0} >= @0 AND {0} < @1", "Created"),
                        ////                     DateTime.Parse("2016-07-08"), DateTime.Parse("2016-07-09")).Count();


                        ////"DateAdded >= DateTime(2013, 06, 18)"
                        int _totalRecords2 = dbContext.Users.Where("Created >= DateTime(2016, 07, 08)").Count();
                        _totalRecords2 = dbContext.Users.Where("( Created >= DateTime(2016, 07, 08) AND Created < DateTime(2016, 07, 09) )").Count();


                        PropertyInfo[] dbProperties = typeof(Quest.Services.Dbio.FMS.Users).GetProperties().ToArray();
                        List<Quest.Services.Dbio.FMS.Users> _usersList = dbContext.Users.Where(BuildWhereClause(queryOptions, dbProperties))
                                .OrderBy(BuildSortString(queryOptions.SortColumns))
                                .Skip(queryOptions.Paging.PageSize * (queryOptions.Paging.PageNumber - 1))
                                .Take(queryOptions.Paging.PageSize).ToList();
                        if (_usersList == null)
                        {
                            return (new questStatus(Severity.Warning));
                        }
                        userList = new List<User>();
                        foreach (Quest.Services.Dbio.FMS.Users _user in _usersList)
                        {
                            User user = new User();
                            BufferMgr.TransferBuffer(_user, user);
                            userList.Add(user);
                        }
                        status = BuildQueryResponse(_usersList.Count, queryOptions, out queryResponse);
                        if (!questStatusDef.IsSuccess(status))
                        {
                            return (status);
                        }
                    }
                    catch (System.Exception ex)
                    {
                        return (new questStatus(Severity.Fatal, String.Format("EXCEPTION: {0}.{1}: {2}",
                                this.GetType().Name, MethodBase.GetCurrentMethod().Name,
                                ex.InnerException != null ? ex.InnerException.Message : ex.Message)));
                    }
                }
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
                _aesEncryption = new AESEncryption();
            }
            catch (System.Exception ex)
            {
                status = new questStatus(Severity.Fatal, String.Format("EXCEPTION: {0}.{1}: {2}",
                        this.GetType().ToString(), MethodInfo.GetCurrentMethod().Name, ex.Message));
                throw new System.Exception(status.Message, ex);
            }
            return (new questStatus(Severity.Success));
        }


        #region Users
        /*----------------------------------------------------------------------------------------------------------------------------------
         * Users
         *---------------------------------------------------------------------------------------------------------------------------------*/
        private questStatus create(FMSEntities dbContext, User user, out UserId userId)
        {
            // Initialize
            userId = null;


            // Perform create
            try
            {
                Quest.Services.Dbio.FMS.Users _user = new Quest.Services.Dbio.FMS.Users();
                BufferMgr.TransferBuffer(user, _user);
                dbContext.Users.Add(_user);
                dbContext.SaveChanges();
                if (_user.Id == 0)
                {
                    return (new questStatus(Severity.Error, "User not created"));
                }
                userId = new UserId(_user.Id);
            }
            catch (DbEntityValidationException ex)
            {
                // Retrieve the error messages as a list of strings.
                var errorMessages = ex.EntityValidationErrors.SelectMany(x => x.ValidationErrors).Select(x => x.ErrorMessage);

                String fullErrorMessage = string.Join("; ", errorMessages);
                String exceptionMessage = string.Concat(ex.Message, " The DbEntityValidationException errors are: ", fullErrorMessage);
                return (new questStatus(Severity.Fatal, String.Format("EXCEPTION: {0}.{1}: {2}",
                                        this.GetType().Name, MethodBase.GetCurrentMethod().Name,
                                        exceptionMessage)));
            }
            catch (System.Exception ex)
            {
                return (new questStatus(Severity.Fatal, String.Format("EXCEPTION: {0}.{1}: {2}",
                        this.GetType().Name, MethodBase.GetCurrentMethod().Name,
                        ex.InnerException != null ? ex.InnerException.Message : ex.Message)));
            }
            return (new questStatus(Severity.Success));
        }
        private questStatus read(FMSEntities dbContext, UserId userId, out Quest.Services.Dbio.FMS.Users user)
        {
            // Initialize
            user = null;


            try
            {
                user = dbContext.Users.Where(r => r.Id == userId.Id).SingleOrDefault();
                if (user == null)
                {
                    return (new questStatus(Severity.Error, String.Format("ERROR: {0}.{1}: {2}",
                            this.GetType().Name, MethodBase.GetCurrentMethod().Name,
                            String.Format("Id {0} not found", userId.Id))));
                }
            }
            catch (DbEntityValidationException ex)
            {
                // Retrieve the error messages as a list of strings.
                var errorMessages = ex.EntityValidationErrors.SelectMany(x => x.ValidationErrors).Select(x => x.ErrorMessage);

                String fullErrorMessage = string.Join("; ", errorMessages);
                String exceptionMessage = string.Concat(ex.Message, " The DbEntityValidationException errors are: ", fullErrorMessage);
                return (new questStatus(Severity.Fatal, String.Format("EXCEPTION: {0}.{1}: {2}",
                                        this.GetType().Name, MethodBase.GetCurrentMethod().Name,
                                        exceptionMessage)));
            }
            catch (System.Exception ex)
            {
                return (new questStatus(Severity.Fatal, String.Format("EXCEPTION: {0}.{1}: {2}",
                        this.GetType().Name, MethodBase.GetCurrentMethod().Name,
                        ex.InnerException != null ? ex.InnerException.Message : ex.Message)));
            }
            return (new questStatus(Severity.Success));
        }
        private questStatus read(FMSEntities dbContext, string username, out Quest.Services.Dbio.FMS.Users user)
        {
            // Initialize
            user = null;


            try
            {
                user = dbContext.Users.Where(r => r.Username == username).SingleOrDefault();
                if (user == null)
                {
                    return (new questStatus(Severity.Error, String.Format("ERROR: {0}.{1}: {2}",
                            this.GetType().Name, MethodBase.GetCurrentMethod().Name,
                            String.Format("Id {0} not found", username))));
                }
            }
            catch (DbEntityValidationException ex)
            {
                // Retrieve the error messages as a list of strings.
                var errorMessages = ex.EntityValidationErrors.SelectMany(x => x.ValidationErrors).Select(x => x.ErrorMessage);

                String fullErrorMessage = string.Join("; ", errorMessages);
                String exceptionMessage = string.Concat(ex.Message, " The DbEntityValidationException errors are: ", fullErrorMessage);
                return (new questStatus(Severity.Fatal, String.Format("EXCEPTION: {0}.{1}: {2}",
                                        this.GetType().Name, MethodBase.GetCurrentMethod().Name,
                                        exceptionMessage)));
            }
            catch (System.Exception ex)
            {
                return (new questStatus(Severity.Fatal, String.Format("EXCEPTION: {0}.{1}: {2}",
                        this.GetType().Name, MethodBase.GetCurrentMethod().Name,
                        ex.InnerException != null ? ex.InnerException.Message : ex.Message)));
            }
            return (new questStatus(Severity.Success));
        }
        private questStatus update(FMSEntities dbContext, User user)
        {
            // Initialize 
            questStatus status = null;


            try
            {
                // Read the record.
                UserId userId = new UserId(user.Id);
                Quest.Services.Dbio.FMS.Users _user = null;
                status = read(dbContext, userId, out _user);
                if (! questStatusDef.IsSuccess(status))
                {
                    return (status);
                }

                // Update the record.
                BufferMgr.TransferBuffer(user, _user);
                dbContext.SaveChanges();
            }
            catch (DbEntityValidationException ex)
            {
                // Retrieve the error messages as a list of strings.
                var errorMessages = ex.EntityValidationErrors.SelectMany(x => x.ValidationErrors).Select(x => x.ErrorMessage);

                String fullErrorMessage = string.Join("; ", errorMessages);
                String exceptionMessage = string.Concat(ex.Message, " The DbEntityValidationException errors are: ", fullErrorMessage);
                return (new questStatus(Severity.Fatal, String.Format("EXCEPTION: {0}.{1}: {2}",
                                        this.GetType().Name, MethodBase.GetCurrentMethod().Name,
                                        exceptionMessage)));
            }
            catch (System.Exception ex)
            {
                return (new questStatus(Severity.Fatal, String.Format("EXCEPTION: {0}.{1}: {2}",
                        this.GetType().Name, MethodBase.GetCurrentMethod().Name,
                        ex.InnerException != null ? ex.InnerException.Message : ex.Message)));
            }
            return (new questStatus(Severity.Success));
        }
        private questStatus delete(FMSEntities dbContext, UserId userId)
        {
            // Initialize 
            questStatus status = null;


            try
            {
                // Read the record.
                Quest.Services.Dbio.FMS.Users _user = null;
                status = read(dbContext, userId, out _user);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }

                // Delete the record.
                dbContext.Users.Remove(_user);
                dbContext.SaveChanges();
            }
            catch (DbEntityValidationException ex)
            {
                // Retrieve the error messages as a list of strings.
                var errorMessages = ex.EntityValidationErrors.SelectMany(x => x.ValidationErrors).Select(x => x.ErrorMessage);

                String fullErrorMessage = string.Join("; ", errorMessages);
                String exceptionMessage = string.Concat(ex.Message, " The DbEntityValidationException errors are: ", fullErrorMessage);
                return (new questStatus(Severity.Fatal, String.Format("EXCEPTION: {0}.{1}: {2}",
                                        this.GetType().Name, MethodBase.GetCurrentMethod().Name,
                                        exceptionMessage)));
            }
            catch (System.Exception ex)
            {
                return (new questStatus(Severity.Fatal, String.Format("EXCEPTION: {0}.{1}: {2}",
                        this.GetType().Name, MethodBase.GetCurrentMethod().Name,
                        ex.InnerException != null ? ex.InnerException.Message : ex.Message)));
            }
            return (new questStatus(Severity.Success));
        }
        #endregion

        #endregion
    }
}
