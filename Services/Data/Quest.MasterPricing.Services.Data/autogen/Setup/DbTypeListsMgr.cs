using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
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
using Quest.Functional.ASM;
using Quest.Functional.MasterPricing;
using Quest.MPDW.Services.Data;
using Quest.Services.Dbio.MasterPricing;


namespace Quest.MasterPricing.Services.Data.Database
{
    public class DbTypeListsMgr : DbMgrSessionBased
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
        public DbTypeListsMgr(UserSession userSession)
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
        public questStatus Create(Quest.Functional.MasterPricing.TypeList typeList, out TypeListId typeListId)
        {
            // Initialize
            questStatus status = null;
            typeListId = null;


            // Data rules.


            // Create the typeList
            using (MasterPricingEntities dbContext = new MasterPricingEntities())
            {
                status = create(dbContext, typeList, out typeListId);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Create(DbMgrTransaction trans, Quest.Functional.MasterPricing.TypeList typeList, out TypeListId typeListId)
        {
            // Initialize
            questStatus status = null;
            typeListId = null;


            // Data rules.


            // Create the typeList in this transaction.
            status = create((MasterPricingEntities)trans.DbContext, typeList, out typeListId);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Read(TypeListId typeListId, out Quest.Functional.MasterPricing.TypeList typeList)
        {
            // Initialize
            questStatus status = null;
            typeList = null;


            // Perform read
            using (MasterPricingEntities dbContext = new MasterPricingEntities())
            {
                Quest.Services.Dbio.MasterPricing.TypeLists _typeList = null;
                status = read(dbContext, typeListId, out _typeList);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }
                typeList = new Quest.Functional.MasterPricing.TypeList();
                BufferMgr.TransferBuffer(_typeList, typeList);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Read(string name, out Quest.Functional.MasterPricing.TypeList typeList)
        {
            // Initialize
            questStatus status = null;
            typeList = null;


            // Perform read
            using (MasterPricingEntities dbContext = new MasterPricingEntities())
            {
                Quest.Services.Dbio.MasterPricing.TypeLists _typeList = null;
                status = read(dbContext, name, out _typeList);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }
                typeList = new Quest.Functional.MasterPricing.TypeList();
                BufferMgr.TransferBuffer(_typeList, typeList);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Read(DbMgrTransaction trans, TypeListId typeListId, out Quest.Functional.MasterPricing.TypeList typeList)
        {
            // Initialize
            questStatus status = null;
            typeList = null;


            // Perform read.
            using (MasterPricingEntities dbContext = new MasterPricingEntities())
            {
                Quest.Services.Dbio.MasterPricing.TypeLists _typeList = null;
                status = read((MasterPricingEntities)trans.DbContext, typeListId, out _typeList);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }
                typeList = new Quest.Functional.MasterPricing.TypeList();
                BufferMgr.TransferBuffer(_typeList, typeList);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Update(Quest.Functional.MasterPricing.TypeList typeList)
        {
            // Initialize
            questStatus status = null;


            // Perform update.
            using (MasterPricingEntities dbContext = new MasterPricingEntities())
            {
                status = update(dbContext, typeList);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Update(DbMgrTransaction trans, Quest.Functional.MasterPricing.TypeList typeList)
        {
            // Initialize
            questStatus status = null;
            bool bCreateTransaction = trans == null;


            // Perform update in this transaction.
            status = update((MasterPricingEntities)trans.DbContext, typeList);
            if (!questStatusDef.IsSuccess(status))
            {
                RollbackTransaction(trans);
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Delete(TypeListId typeListId)
        {
            // Initialize
            questStatus status = null;


            // Perform delete.
            using (MasterPricingEntities dbContext = new MasterPricingEntities())
            {
                status = delete(dbContext, typeListId);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Delete(DbMgrTransaction trans, TypeListId typeListId)
        {
            // Initialize
            questStatus status = null;


            // Perform delete in this transaction.
            status = delete((MasterPricingEntities)trans.DbContext, typeListId);
            if (!questStatusDef.IsSuccess(status))
            {
                RollbackTransaction(trans);
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus List(QueryOptions queryOptions, out List<Quest.Functional.MasterPricing.TypeList> typeListList, out QueryResponse queryResponse)
        {
            // Initialize
            questStatus status = null;
            typeListList = null;
            queryResponse = null;


            using (MasterPricingEntities dbContext = new MasterPricingEntities())
            {
                using (DbContextTransaction tx = dbContext.Database.BeginTransaction(IsolationLevel.ReadUncommitted))
                {
                    try
                    {
                        PropertyInfo[] dbProperties = typeof(Quest.Services.Dbio.MasterPricing.TypeLists).GetProperties().ToArray();
                        int totalRecords = dbContext.vwTypeListsList.Where(BuildWhereClause(queryOptions, dbProperties)).Count();
                        List<Quest.Services.Dbio.MasterPricing.vwTypeListsList> _typeListsList = dbContext.vwTypeListsList.Where(BuildWhereClause(queryOptions, dbProperties))
                                .OrderBy(BuildSortString(queryOptions.SortColumns))
                                .Skip(queryOptions.Paging.PageSize * (queryOptions.Paging.PageNumber - 1))
                                .Take(queryOptions.Paging.PageSize).ToList();
                        if (_typeListsList == null)
                        {
                            return (new questStatus(Severity.Warning));
                        }
                        typeListList = new List<Quest.Functional.MasterPricing.TypeList>();
                        foreach (Quest.Services.Dbio.MasterPricing.vwTypeListsList _typeList in _typeListsList)
                        {
                            Quest.Functional.MasterPricing.TypeList typeList = new Quest.Functional.MasterPricing.TypeList();
                            BufferMgr.TransferBuffer(_typeList, typeList);
                            typeListList.Add(typeList);
                        }
                        status = BuildQueryResponse(totalRecords, queryOptions, out queryResponse);
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
            }
            catch (System.Exception ex)
            {
                status = new questStatus(Severity.Fatal, String.Format("EXCEPTION: {0}.{1}: {2}",
                        this.GetType().ToString(), MethodInfo.GetCurrentMethod().Name, ex.Message));
                throw new System.Exception(status.Message, ex);
            }
            return (new questStatus(Severity.Success));
        }


        #region TypeLists
        /*----------------------------------------------------------------------------------------------------------------------------------
         * TypeLists
         *---------------------------------------------------------------------------------------------------------------------------------*/
        private questStatus create(MasterPricingEntities dbContext, Quest.Functional.MasterPricing.TypeList typeList, out TypeListId typeListId)
        {
            // Initialize
            typeListId = null;


            // Perform create
            try
            {
                Quest.Services.Dbio.MasterPricing.TypeLists _typeList = new Quest.Services.Dbio.MasterPricing.TypeLists();
                BufferMgr.TransferBuffer(typeList, _typeList);
                dbContext.TypeLists.Add(_typeList);
                dbContext.SaveChanges();
                if (_typeList.Id == 0)
                {
                    return (new questStatus(Severity.Error, "Quest.Functional.MasterPricing.TypeList not created"));
                }
                typeListId = new TypeListId(_typeList.Id);
            }
            catch (System.Exception ex)
            {
                return (new questStatus(Severity.Fatal, String.Format("EXCEPTION: {0}.{1}: {2}",
                        this.GetType().Name, MethodBase.GetCurrentMethod().Name,
                        ex.InnerException != null ? ex.InnerException.Message : ex.Message)));
            }
            return (new questStatus(Severity.Success));
        }
        private questStatus read(MasterPricingEntities dbContext, TypeListId typeListId, out Quest.Services.Dbio.MasterPricing.TypeLists typeList)
        {
            // Initialize
            typeList = null;


            try
            {
                typeList = dbContext.TypeLists.Where(r => r.Id == typeListId.Id).SingleOrDefault();
                if (typeList == null)
                {
                    return (new questStatus(Severity.Error, String.Format("ERROR: {0}.{1}: {2}",
                            this.GetType().Name, MethodBase.GetCurrentMethod().Name,
                            String.Format("Id {0} not found", typeListId.Id))));
                }
            }
            catch (System.Exception ex)
            {
                return (new questStatus(Severity.Fatal, String.Format("EXCEPTION: {0}.{1}: {2}",
                        this.GetType().Name, MethodBase.GetCurrentMethod().Name,
                        ex.InnerException != null ? ex.InnerException.Message : ex.Message)));
            }
            return (new questStatus(Severity.Success));
        }
        private questStatus read(MasterPricingEntities dbContext, string name, out Quest.Services.Dbio.MasterPricing.TypeLists typeList)
        {
            // Initialize
            typeList = null;


            try
            {
                typeList = dbContext.TypeLists.Where(r => r.Name == name).SingleOrDefault();
                if (typeList == null)
                {
                    return (new questStatus(Severity.Error, String.Format("ERROR: {0}.{1}: {2}",
                            this.GetType().Name, MethodBase.GetCurrentMethod().Name,
                            String.Format("Name {0} not found", name))));
                }
            }
            catch (System.Exception ex)
            {
                return (new questStatus(Severity.Fatal, String.Format("EXCEPTION: {0}.{1}: {2}",
                        this.GetType().Name, MethodBase.GetCurrentMethod().Name,
                        ex.InnerException != null ? ex.InnerException.Message : ex.Message)));
            }
            return (new questStatus(Severity.Success));
        }
        private questStatus update(MasterPricingEntities dbContext, Quest.Functional.MasterPricing.TypeList typeList)
        {
            // Initialize 
            questStatus status = null;


            try
            {
                // Read the record.
                TypeListId typeListId = new TypeListId(typeList.Id);
                Quest.Services.Dbio.MasterPricing.TypeLists _typeList = null;
                status = read(dbContext, typeListId, out _typeList);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }

                // Update the record.
                BufferMgr.TransferBuffer(typeList, _typeList);
                dbContext.SaveChanges();
            }
            catch (System.Exception ex)
            {
                return (new questStatus(Severity.Fatal, String.Format("EXCEPTION: {0}.{1}: {2}",
                        this.GetType().Name, MethodBase.GetCurrentMethod().Name,
                        ex.InnerException != null ? ex.InnerException.Message : ex.Message)));
            }
            return (new questStatus(Severity.Success));
        }
        private questStatus delete(MasterPricingEntities dbContext, TypeListId typeListId)
        {
            // Initialize 
            questStatus status = null;


            try
            {
                // Read the record.
                Quest.Services.Dbio.MasterPricing.TypeLists _typeList = null;
                status = read(dbContext, typeListId, out _typeList);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }

                // Delete the record.
                dbContext.TypeLists.Remove(_typeList);
                dbContext.SaveChanges();
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
