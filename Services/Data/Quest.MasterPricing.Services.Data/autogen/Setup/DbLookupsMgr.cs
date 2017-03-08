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
using Quest.Functional.FMS;
using Quest.Functional.MasterPricing;
using Quest.MPDW.Services.Data;
using Quest.Services.Dbio.MasterPricing;


namespace Quest.MasterPricing.Services.Data.Database
{
    public class DbLookupsMgr : DbMgrSessionBased
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
        public DbLookupsMgr(UserSession userSession)
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
        public questStatus Create(Quest.Functional.MasterPricing.Lookup lookup, out LookupId lookupId)
        {
            // Initialize
            questStatus status = null;
            lookupId = null;


            // Data rules.


            // Create the lookup
            using (MasterPricingEntities dbContext = new MasterPricingEntities())
            {
                status = create(dbContext, lookup, out lookupId);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Create(DbMgrTransaction trans, Quest.Functional.MasterPricing.Lookup lookup, out LookupId lookupId)
        {
            // Initialize
            questStatus status = null;
            lookupId = null;


            // Data rules.


            // Create the lookup in this transaction.
            status = create((MasterPricingEntities)trans.DbContext, lookup, out lookupId);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Read(LookupId lookupId, out Quest.Functional.MasterPricing.Lookup lookup)
        {
            // Initialize
            questStatus status = null;
            lookup = null;


            // Perform read
            using (MasterPricingEntities dbContext = new MasterPricingEntities())
            {
                Quest.Services.Dbio.MasterPricing.Lookups _lookup = null;
                status = read(dbContext, lookupId, out _lookup);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }
                lookup = new Quest.Functional.MasterPricing.Lookup();
                BufferMgr.TransferBuffer(_lookup, lookup);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Read(string name, out Quest.Functional.MasterPricing.Lookup lookup)
        {
            // Initialize
            questStatus status = null;
            lookup = null;


            // Perform read
            using (MasterPricingEntities dbContext = new MasterPricingEntities())
            {
                Quest.Services.Dbio.MasterPricing.Lookups _lookup = null;
                status = read(dbContext, name, out _lookup);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }
                lookup = new Quest.Functional.MasterPricing.Lookup();
                BufferMgr.TransferBuffer(_lookup, lookup);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Read(DbMgrTransaction trans, LookupId lookupId, out Quest.Functional.MasterPricing.Lookup lookup)
        {
            // Initialize
            questStatus status = null;
            lookup = null;


            // Perform read.
            using (MasterPricingEntities dbContext = new MasterPricingEntities())
            {
                Quest.Services.Dbio.MasterPricing.Lookups _lookup = null;
                status = read((MasterPricingEntities)trans.DbContext, lookupId, out _lookup);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }
                lookup = new Quest.Functional.MasterPricing.Lookup();
                BufferMgr.TransferBuffer(_lookup, lookup);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Update(Quest.Functional.MasterPricing.Lookup lookup)
        {
            // Initialize
            questStatus status = null;


            // Perform update.
            using (MasterPricingEntities dbContext = new MasterPricingEntities())
            {
                status = update(dbContext, lookup);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Update(DbMgrTransaction trans, Quest.Functional.MasterPricing.Lookup lookup)
        {
            // Initialize
            questStatus status = null;
            bool bCreateTransaction = trans == null;


            // Perform update in this transaction.
            status = update((MasterPricingEntities)trans.DbContext, lookup);
            if (!questStatusDef.IsSuccess(status))
            {
                RollbackTransaction(trans);
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Delete(LookupId lookupId)
        {
            // Initialize
            questStatus status = null;


            // Perform delete.
            using (MasterPricingEntities dbContext = new MasterPricingEntities())
            {
                status = delete(dbContext, lookupId);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Delete(DbMgrTransaction trans, LookupId lookupId)
        {
            // Initialize
            questStatus status = null;


            // Perform delete in this transaction.
            status = delete((MasterPricingEntities)trans.DbContext, lookupId);
            if (!questStatusDef.IsSuccess(status))
            {
                RollbackTransaction(trans);
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus List(QueryOptions queryOptions, out List<Quest.Functional.MasterPricing.Lookup> lookupList, out QueryResponse queryResponse)
        {
            // Initialize
            questStatus status = null;
            lookupList = null;
            queryResponse = null;


            using (MasterPricingEntities dbContext = new MasterPricingEntities())
            {
                using (DbContextTransaction tx = dbContext.Database.BeginTransaction(IsolationLevel.ReadUncommitted))
                {
                    try
                    {
                        PropertyInfo[] dbProperties = typeof(Quest.Services.Dbio.MasterPricing.Lookups).GetProperties().ToArray();
                        int totalRecords = dbContext.vwLookupsList4.Where(BuildWhereClause(queryOptions, dbProperties)).Count();
                        List<Quest.Services.Dbio.MasterPricing.vwLookupsList4> _lookupsList = dbContext.vwLookupsList4.Where(BuildWhereClause(queryOptions, dbProperties))
                                .OrderBy(BuildSortString(queryOptions.SortColumns))
                                .Skip(queryOptions.Paging.PageSize * (queryOptions.Paging.PageNumber - 1))
                                .Take(queryOptions.Paging.PageSize).ToList();
                        if (_lookupsList == null)
                        {
                            return (new questStatus(Severity.Warning));
                        }
                        lookupList = new List<Quest.Functional.MasterPricing.Lookup>();
                        foreach (Quest.Services.Dbio.MasterPricing.vwLookupsList4 _lookup in _lookupsList)
                        {
                            Quest.Functional.MasterPricing.Lookup lookup = new Quest.Functional.MasterPricing.Lookup();
                            BufferMgr.TransferBuffer(_lookup, lookup);
                            lookupList.Add(lookup);
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


        #region Lookups
        /*----------------------------------------------------------------------------------------------------------------------------------
         * Lookups
         *---------------------------------------------------------------------------------------------------------------------------------*/
        private questStatus create(MasterPricingEntities dbContext, Quest.Functional.MasterPricing.Lookup lookup, out LookupId lookupId)
        {
            // Initialize
            lookupId = null;


            // Perform create
            try
            {
                Quest.Services.Dbio.MasterPricing.Lookups _lookup = new Quest.Services.Dbio.MasterPricing.Lookups();
                BufferMgr.TransferBuffer(lookup, _lookup);
                dbContext.Lookups.Add(_lookup);
                dbContext.SaveChanges();
                if (_lookup.Id == 0)
                {
                    return (new questStatus(Severity.Error, "Quest.Functional.MasterPricing.Lookup not created"));
                }
                lookupId = new LookupId(_lookup.Id);
            }
            catch (System.Exception ex)
            {
                return (new questStatus(Severity.Fatal, String.Format("EXCEPTION: {0}.{1}: {2}",
                        this.GetType().Name, MethodBase.GetCurrentMethod().Name,
                        ex.InnerException != null ? ex.InnerException.Message : ex.Message)));
            }
            return (new questStatus(Severity.Success));
        }
        private questStatus read(MasterPricingEntities dbContext, LookupId lookupId, out Quest.Services.Dbio.MasterPricing.Lookups lookup)
        {
            // Initialize
            lookup = null;


            try
            {
                lookup = dbContext.Lookups.Where(r => r.Id == lookupId.Id).SingleOrDefault();
                if (lookup == null)
                {
                    return (new questStatus(Severity.Error, String.Format("ERROR: {0}.{1}: {2}",
                            this.GetType().Name, MethodBase.GetCurrentMethod().Name,
                            String.Format("Id {0} not found", lookupId.Id))));
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
        private questStatus read(MasterPricingEntities dbContext, string name, out Quest.Services.Dbio.MasterPricing.Lookups lookup)
        {
            // Initialize
            lookup = null;


            try
            {
                lookup = dbContext.Lookups.Where(r => r.Name == name).SingleOrDefault();
                if (lookup == null)
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
        private questStatus update(MasterPricingEntities dbContext, Quest.Functional.MasterPricing.Lookup lookup)
        {
            // Initialize 
            questStatus status = null;


            try
            {
                // Read the record.
                LookupId lookupId = new LookupId(lookup.Id);
                Quest.Services.Dbio.MasterPricing.Lookups _lookup = null;
                status = read(dbContext, lookupId, out _lookup);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }

                // Update the record.
                BufferMgr.TransferBuffer(lookup, _lookup);
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
        private questStatus delete(MasterPricingEntities dbContext, LookupId lookupId)
        {
            // Initialize 
            questStatus status = null;


            try
            {
                // Read the record.
                Quest.Services.Dbio.MasterPricing.Lookups _lookup = null;
                status = read(dbContext, lookupId, out _lookup);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }

                // Delete the record.
                dbContext.Lookups.Remove(_lookup);
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
