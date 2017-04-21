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
using Quest.MasterPricing.Services.Business.Filters;


namespace Quest.MasterPricing.Services.Business.Database
{
    public class StoredProceduresMgr : MgrSessionBased
    {
        #region Declarations
        /*==================================================================================================================================
         * Declarations
         *=================================================================================================================================*/
        private DbStoredProceduresMgr _dbStoredProceduresMgr = null;

        #endregion


        #region Constructors
        /*==================================================================================================================================
         * Constructors
         *=================================================================================================================================*/
        public StoredProceduresMgr(UserSession userSession)
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
        public questStatus Create(DbMgrTransaction trans, Quest.Functional.MasterPricing.Database database, Quest.Functional.MasterPricing.StoredProcedure storedProcedure, out Quest.Functional.MasterPricing.StoredProcedureId storedProcedureId)
        {
            // Initialize
            questStatus status = null;
            storedProcedureId = null;


            // Create storedProcedure
            status = _dbStoredProceduresMgr.Create(trans, storedProcedure, out storedProcedureId);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }

            // Store parameters for procedure
            List<StoredProcedureParameter> storedProcedureParameterList = null;
            status = GetStoredProdecureParameters(database, storedProcedure.Name, out storedProcedureParameterList);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            StoredProcedureParametersMgr storedProcedureParametersMgr = new StoredProcedureParametersMgr(this.UserSession);
            foreach (StoredProcedureParameter storedProcedureParameter in storedProcedureParameterList)
            {
                storedProcedureParameter.StoredProcedureId = storedProcedureId.Id;
                StoredProcedureParameterId storedProcedureParameterId = null;
                status = storedProcedureParametersMgr.Create(trans, database, storedProcedureParameter, out storedProcedureParameterId);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Create(DbMgrTransaction trans, List<Quest.Functional.MasterPricing.StoredProcedure> storedProcedureList, out List<Quest.Functional.MasterPricing.StoredProcedure> storedProcedureIdList)
        {
            // Initialize
            questStatus status = null;
            storedProcedureIdList = null;


            // Create storedProcedure
            status = _dbStoredProceduresMgr.Create(trans, storedProcedureList, out storedProcedureIdList);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Read(Quest.Functional.MasterPricing.StoredProcedureId storedProcedureId, out Quest.Functional.MasterPricing.StoredProcedure storedProcedure)
        {
            // Initialize
            questStatus status = null;
            storedProcedure = null;


            // Read storedProcedure
            status = _dbStoredProceduresMgr.Read(storedProcedureId, out storedProcedure);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Read(DbMgrTransaction trans, StoredProcedureId storedProcedureId, out StoredProcedure storedProcedure)
        {
            // Initialize
            questStatus status = null;
            storedProcedure = null;


            // Read storedProcedure
            status = _dbStoredProceduresMgr.Read(trans, storedProcedureId, out storedProcedure);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Read(DatabaseId databaseId, out List<StoredProcedure> storedProcedureList)
        {
            // Initialize
            questStatus status = null;
            storedProcedureList = null;


            // Read storedProcedure
            status = _dbStoredProceduresMgr.Read(databaseId, out storedProcedureList);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Read(DbMgrTransaction trans, DatabaseId databaseId, out List<StoredProcedure> storedProcedureList)
        {
            // Initialize
            questStatus status = null;
            storedProcedureList = null;


            // Read storedProcedure
            status = _dbStoredProceduresMgr.Read(trans, databaseId, out storedProcedureList);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Read(DatabaseId databaseId, string storedProcedureName, out StoredProcedure storedProcedure)
        {
            // Initialize
            questStatus status = null;
            storedProcedure = null;


            // Read storedProcedure
            status = _dbStoredProceduresMgr.Read(databaseId, storedProcedureName, out storedProcedure);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Update(StoredProcedure storedProcedure)
        {
            // Initialize
            questStatus status = null;


            // Update storedProcedure
            status = _dbStoredProceduresMgr.Update(storedProcedure);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Update(DbMgrTransaction trans, StoredProcedure storedProcedure)
        {
            // Initialize
            questStatus status = null;


            // Update storedProcedure
            status = _dbStoredProceduresMgr.Update(trans, storedProcedure);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Delete(StoredProcedureId storedProcedureId)
        {
            // Initialize
            questStatus status = null;


            // Delete storedProcedure
            status = _dbStoredProceduresMgr.Delete(storedProcedureId);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Delete(DbMgrTransaction trans, StoredProcedureId storedProcedureId)
        {
            // Initialize
            questStatus status = null;


            // Delete storedProcedure
            status = _dbStoredProceduresMgr.Delete(trans, storedProcedureId);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Delete(DatabaseId databaseId)
        {
            // Initialize
            questStatus status = null;


            // Delete all procedureProcedures in this stored.
            status = _dbStoredProceduresMgr.Delete(databaseId);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Delete(DbMgrTransaction trans, DatabaseId databaseId)
        {
            // Initialize
            questStatus status = null;


            // Delete all procedureProcedures in this procedure.
            status = _dbStoredProceduresMgr.Delete(trans, databaseId);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus List(QueryOptions queryOptions, out List<StoredProcedure> storedProcedureList, out QueryResponse queryResponse)
        {
            // Initialize
            questStatus status = null;
            storedProcedureList = null;


            // List
            status = _dbStoredProceduresMgr.List(queryOptions, out storedProcedureList, out queryResponse);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }

        public questStatus RefreshFilters(StoredProcedureId storedProcedureId, out List<FilterId> updatedFilterIds)
        {
            // Every filter that uses this stored procedure, refresh it to update the 'Make Required' changes.

            // Initialize
            questStatus status = null;
            updatedFilterIds = null;


            // Get the database Id of this stored procedure
            StoredProcedure storedProcedure = null;
            status = _dbStoredProceduresMgr.Read(storedProcedureId, out storedProcedure);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }

            // Get the tablesets using this database
            DatabaseId databaseId = new DatabaseId(storedProcedure.DatabaseId);
            List<Tableset> tablesetList = null;
            TablesetsMgr tablesetsMgr = new TablesetsMgr(this.UserSession);
            status = tablesetsMgr.Read(databaseId, out tablesetList);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }

            // Get the filters using these tablesets.  Return the filters we updated.
            // TODO: OPTIMIZE THIS FOR THE FILTERS RECORD ONLY.  THIS IS A QUICK-FIX IN NEED OF BEING PUBLISHED ASAP.
            updatedFilterIds = new List<FilterId>();
            FilterMgr filterMgr = new FilterMgr(this.UserSession);
            foreach (Tableset tableset in tablesetList)
            {
                TablesetId tablesetId = new TablesetId(tableset.Id);
                List<Filter> filterList = null;
                status = filterMgr.Read(tablesetId, out filterList);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }

                // Refresh these filters if not refreshed.
                foreach (Filter filter in filterList)
                {
                    // See if previously refreshed.
                    FilterId filterId = updatedFilterIds.Find(delegate (FilterId fid) { return (fid.Id == filter.Id); });
                    if (filterId != null)
                    {
                        continue;
                    }

                    // Update the filter
                    filterId = new FilterId(filter.Id);
                    status = filterMgr.Refresh(filterId);
                    if (!questStatusDef.IsSuccess(status))
                    {
                        return (status);
                    }

                    // Record the Id we updated
                    filterId = new FilterId(filter.Id);
                    updatedFilterIds.Add(filterId);
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
                _dbStoredProceduresMgr = new DbStoredProceduresMgr(this.UserSession);
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

