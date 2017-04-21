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
using Quest.Functional.ASM;
using Quest.Functional.FMS;
using Quest.Functional.MasterPricing;
using Quest.MPDW.Services.Data;
using Quest.Services.Dbio.MasterPricing;
using Quest.Functional.Logging;
using Quest.Services.Data.Logging;


namespace Quest.MasterPricing.Services.Data.Database
{
    public class DbTablesetViewsMgr : DbLogsMgr
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
        public DbTablesetViewsMgr(UserSession userSession)
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
        public questStatus Create(Quest.Functional.MasterPricing.TablesetView tablesetView, out TablesetViewId tablesetViewId)
        {
            // Initialize
            questStatus status = null;
            tablesetViewId = null;


            // Data rules.


            // Create the tablesetView
            using (MasterPricingEntities dbContext = new MasterPricingEntities())
            {
                status = create(dbContext, tablesetView, out tablesetViewId);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Create(DbMgrTransaction trans, Quest.Functional.MasterPricing.TablesetView tablesetView, out TablesetViewId tablesetViewId)
        {
            // Initialize
            questStatus status = null;
            tablesetViewId = null;


            // Data rules.


            // Create the tablesetView in this transaction.
            status = create((MasterPricingEntities)trans.DbContext, tablesetView, out tablesetViewId);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Read(TablesetId tablesetId, out List<Quest.Functional.MasterPricing.TablesetView> tablesetViewList)
        {
            // Initialize
            questStatus status = null;
            tablesetViewList = null;


            // Perform read
            using (MasterPricingEntities dbContext = new MasterPricingEntities())
            {
                List<Quest.Services.Dbio.MasterPricing.TablesetViews> _tablesetViewList = null;
                status = read(dbContext, tablesetId, out _tablesetViewList);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }
                tablesetViewList = new List<TablesetView>();
                foreach (Quest.Services.Dbio.MasterPricing.TablesetViews _tablesetView in _tablesetViewList)
                {
                    Quest.Functional.MasterPricing.TablesetView tablesetView = new Quest.Functional.MasterPricing.TablesetView();
                    BufferMgr.TransferBuffer(_tablesetView, tablesetView);
                    tablesetViewList.Add(tablesetView);
                }
            }
            return (new questStatus(Severity.Success));
        }





        public questStatus Read(TablesetViewId tablesetViewId, out Quest.Functional.MasterPricing.TablesetView tablesetView)
        {
            // Initialize
            questStatus status = null;
            tablesetView = null;


            // Perform read
            using (MasterPricingEntities dbContext = new MasterPricingEntities())
            {
                Quest.Services.Dbio.MasterPricing.TablesetViews _tablesetView = null;
                status = read(dbContext, tablesetViewId, out _tablesetView);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }
                tablesetView = new Quest.Functional.MasterPricing.TablesetView();
                BufferMgr.TransferBuffer(_tablesetView, tablesetView);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Read(DbMgrTransaction trans, TablesetViewId tablesetViewId, out Quest.Functional.MasterPricing.TablesetView tablesetView)
        {
            // Initialize
            questStatus status = null;
            tablesetView = null;


            // Perform read.
            using (MasterPricingEntities dbContext = new MasterPricingEntities())
            {
                Quest.Services.Dbio.MasterPricing.TablesetViews _tablesetView = null;
                status = read((MasterPricingEntities)trans.DbContext, tablesetViewId, out _tablesetView);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }
                tablesetView = new Quest.Functional.MasterPricing.TablesetView();
                BufferMgr.TransferBuffer(_tablesetView, tablesetView);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Read(FilterViewTablesetViewId filterViewTablesetViewId, out Quest.Functional.MasterPricing.TablesetView tablesetView)
        {
            // Initialize
            questStatus status = null;
            tablesetView = null;


            // Perform read
            using (MasterPricingEntities dbContext = new MasterPricingEntities())
            {
                Quest.Services.Dbio.MasterPricing.TablesetViews _tablesetView = null;
                status = read(dbContext, filterViewTablesetViewId, out _tablesetView);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }
                tablesetView = new Quest.Functional.MasterPricing.TablesetView();
                BufferMgr.TransferBuffer(_tablesetView, tablesetView);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Read(DbMgrTransaction trans, FilterViewTablesetViewId filterViewTablesetViewId, out Quest.Functional.MasterPricing.TablesetView tablesetView)
        {
            // Initialize
            questStatus status = null;
            tablesetView = null;


            // Perform read.
            using (MasterPricingEntities dbContext = new MasterPricingEntities())
            {
                Quest.Services.Dbio.MasterPricing.TablesetViews _tablesetView = null;
                status = read((MasterPricingEntities)trans.DbContext, filterViewTablesetViewId, out _tablesetView);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }
                tablesetView = new Quest.Functional.MasterPricing.TablesetView();
                BufferMgr.TransferBuffer(_tablesetView, tablesetView);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Update(Quest.Functional.MasterPricing.TablesetView tablesetView)
        {
            // Initialize
            questStatus status = null;


            // Perform update.
            using (MasterPricingEntities dbContext = new MasterPricingEntities())
            {
                status = update(dbContext, tablesetView);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Update(DbMgrTransaction trans, Quest.Functional.MasterPricing.TablesetView tablesetView)
        {
            // Initialize
            questStatus status = null;
            bool bCreateTransaction = trans == null;


            // Perform update in this transaction.
            status = update((MasterPricingEntities)trans.DbContext, tablesetView);
            if (!questStatusDef.IsSuccess(status))
            {
                RollbackTransaction(trans);
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Delete(TablesetId tablesetId)
        {
            // Initialize
            questStatus status = null;


            // Perform delete.
            using (MasterPricingEntities dbContext = new MasterPricingEntities())
            {
                status = delete(dbContext, tablesetId);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Delete(DbMgrTransaction trans, TablesetId tablesetId)
        {
            // Initialize
            questStatus status = null;


            // Perform delete in this transaction.
            status = delete((MasterPricingEntities)trans.DbContext, tablesetId);
            if (!questStatusDef.IsSuccess(status))
            {
                RollbackTransaction(trans);
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus List(QueryOptions queryOptions, out List<Quest.Functional.MasterPricing.TablesetView> tablesetViewList, out QueryResponse queryResponse)
        {
            // Initialize
            questStatus status = null;
            tablesetViewList = null;
            queryResponse = null;


            using (MasterPricingEntities dbContext = new MasterPricingEntities())
            {
                using (DbContextTransaction tx = dbContext.Database.BeginTransaction(IsolationLevel.ReadUncommitted))
                {
                    try
                    {
                        PropertyInfo[] dbProperties = typeof(Quest.Services.Dbio.MasterPricing.TablesetViews).GetProperties().ToArray();
                        int totalRecords = dbContext.vwTablesetViewsList2.Where(BuildWhereClause(queryOptions, dbProperties)).Count();
                        List<Quest.Services.Dbio.MasterPricing.vwTablesetViewsList2> _tablesetViewsList = dbContext.vwTablesetViewsList2.Where(BuildWhereClause(queryOptions, dbProperties))
                                .OrderBy(BuildSortString(queryOptions.SortColumns))
                                .Skip(queryOptions.Paging.PageSize * (queryOptions.Paging.PageNumber - 1))
                                .Take(queryOptions.Paging.PageSize).ToList();
                        if (_tablesetViewsList == null)
                        {
                            return (new questStatus(Severity.Warning));
                        }
                        tablesetViewList = new List<Quest.Functional.MasterPricing.TablesetView>();
                        foreach (Quest.Services.Dbio.MasterPricing.vwTablesetViewsList2 _tablesetView in _tablesetViewsList)
                        {
                            Quest.Functional.MasterPricing.TablesetView tablesetView = new Quest.Functional.MasterPricing.TablesetView();
                            BufferMgr.TransferBuffer(_tablesetView, tablesetView);
                            tablesetViewList.Add(tablesetView);
                        }
                        status = BuildQueryResponse(totalRecords, queryOptions, out queryResponse);
                        if (!questStatusDef.IsSuccess(status))
                        {
                            return (status);
                        }
                    }
                    catch (System.Exception ex)
                    {
                        status = new questStatus(Severity.Fatal, String.Format("EXCEPTION: {0}.{1}: {2}",
                                this.GetType().Name, MethodBase.GetCurrentMethod().Name,
                                ex.InnerException != null ? ex.InnerException.Message : ex.Message));
                        LogException(ex, status);
                        return (status);
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
                LogException(ex, status);
                throw new System.Exception(status.Message, ex);
            }
            return (new questStatus(Severity.Success));
        }


        #region TablesetViews
        /*----------------------------------------------------------------------------------------------------------------------------------
         * TablesetViews
         *---------------------------------------------------------------------------------------------------------------------------------*/
        private questStatus create(MasterPricingEntities dbContext, Quest.Functional.MasterPricing.TablesetView tablesetView, out TablesetViewId tablesetViewId)
        {
            // Initialize
            questStatus status = null;
            tablesetViewId = null;


            // Perform create
            try
            {
                Quest.Services.Dbio.MasterPricing.TablesetViews _tablesetView = new Quest.Services.Dbio.MasterPricing.TablesetViews();
                BufferMgr.TransferBuffer(tablesetView, _tablesetView);
                dbContext.TablesetViews.Add(_tablesetView);
                dbContext.SaveChanges();
                if (_tablesetView.Id == 0)
                {
                    return (new questStatus(Severity.Error, "Quest.Functional.MasterPricing.TablesetView not created"));
                }
                tablesetViewId = new TablesetViewId(_tablesetView.Id);
            }
            catch (DbEntityValidationException ex)
            {
                var errorMessages = ex.EntityValidationErrors.SelectMany(x => x.ValidationErrors).Select(x => x.ErrorMessage);
                String fullErrorMessage = string.Join("; ", errorMessages);
                String exceptionMessage = string.Concat(ex.Message, fullErrorMessage);

                status = new questStatus(Severity.Fatal, String.Format("EXCEPTION: {0}.{1}: {2}",
                                        this.GetType().Name, MethodBase.GetCurrentMethod().Name,
                                        exceptionMessage));
                LogException(ex, status);
                return (status);
            }
            catch (System.Exception ex)
            {
                status = new questStatus(Severity.Fatal, String.Format("EXCEPTION: {0}.{1}: {2}",
                        this.GetType().Name, MethodBase.GetCurrentMethod().Name,
                        ex.InnerException != null ? ex.InnerException.Message : ex.Message));
                LogException(ex, status);
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        private questStatus read(MasterPricingEntities dbContext, TablesetId tablesetId, out List<Quest.Services.Dbio.MasterPricing.TablesetViews> tablesetViewList)
        {
            // Initialize
            questStatus status = null;
            tablesetViewList = null;


            try
            {
                tablesetViewList = dbContext.TablesetViews.Where(r => r.TablesetId == tablesetId.Id).ToList();
                if (tablesetViewList == null)
                {
                    return (new questStatus(Severity.Error, String.Format("ERROR: {0}.{1}: {2}",
                            this.GetType().Name, MethodBase.GetCurrentMethod().Name,
                            String.Format("TablesetViews for TablesetId {0} not found", tablesetId.Id))));
                }
            }
            catch (System.Exception ex)
            {
                status = new questStatus(Severity.Fatal, String.Format("EXCEPTION: {0}.{1}: {2}",
                        this.GetType().Name, MethodBase.GetCurrentMethod().Name,
                        ex.InnerException != null ? ex.InnerException.Message : ex.Message));
                LogException(ex, status);
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        private questStatus read(MasterPricingEntities dbContext, TablesetViewId tablesetViewId, out Quest.Services.Dbio.MasterPricing.TablesetViews tablesetView)
        {
            // Initialize
            questStatus status = null;
            tablesetView = null;


            try
            {
                tablesetView = dbContext.TablesetViews.Where(r => r.Id == tablesetViewId.Id).SingleOrDefault();
                if (tablesetView == null)
                {
                    return (new questStatus(Severity.Error, String.Format("ERROR: {0}.{1}: {2}",
                            this.GetType().Name, MethodBase.GetCurrentMethod().Name,
                            String.Format("TablesetId.Id {0} not found", tablesetViewId.Id))));
                }
            }
            catch (System.Exception ex)
            {
                status = new questStatus(Severity.Fatal, String.Format("EXCEPTION: {0}.{1}: {2}",
                        this.GetType().Name, MethodBase.GetCurrentMethod().Name,
                        ex.InnerException != null ? ex.InnerException.Message : ex.Message));
                LogException(ex, status);
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        private questStatus read(MasterPricingEntities dbContext, FilterViewTablesetViewId filterViewTablesetViewId, out Quest.Services.Dbio.MasterPricing.TablesetViews tablesetView)
        {
            // Initialize
            questStatus status = null;
            tablesetView = null;


            try
            {
                tablesetView = dbContext.TablesetViews.Where(r => r.TablesetId == filterViewTablesetViewId.TablesetId.Id && r.Schema == filterViewTablesetViewId.Schema && r.Name == filterViewTablesetViewId.Name).SingleOrDefault();
                if (tablesetView == null)
                {
                    return (new questStatus(Severity.Error, String.Format("ERROR: {0}.{1}: {2}",
                            this.GetType().Name, MethodBase.GetCurrentMethod().Name,
                            String.Format("FilterViewTablesetViewId.Id {0} Schema {1} Name {2} not found", filterViewTablesetViewId.TablesetId.Id, filterViewTablesetViewId.Schema, filterViewTablesetViewId.Name))));
                }
            }
            catch (System.Exception ex)
            {
                status = new questStatus(Severity.Fatal, String.Format("EXCEPTION: {0}.{1}: {2}",
                        this.GetType().Name, MethodBase.GetCurrentMethod().Name,
                        ex.InnerException != null ? ex.InnerException.Message : ex.Message));
                LogException(ex, status);
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        private questStatus update(MasterPricingEntities dbContext, Quest.Functional.MasterPricing.TablesetView tablesetView)
        {
            // Initialize 
            questStatus status = null;


            try
            {
                // Read the record.
                TablesetViewId tablesetViewId = new TablesetViewId(tablesetView.Id);
                Quest.Services.Dbio.MasterPricing.TablesetViews _tablesetView = null;
                status = read(dbContext, tablesetViewId, out _tablesetView);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }

                // Update the record.
                BufferMgr.TransferBuffer(tablesetView, _tablesetView);
                dbContext.SaveChanges();
            }
            catch (DbEntityValidationException ex)
            {
                var errorMessages = ex.EntityValidationErrors.SelectMany(x => x.ValidationErrors).Select(x => x.ErrorMessage);
                String fullErrorMessage = string.Join("; ", errorMessages);
                String exceptionMessage = string.Concat(ex.Message, fullErrorMessage);
                status = new questStatus(Severity.Fatal, String.Format("EXCEPTION: {0}.{1}: {2}",
                                        this.GetType().Name, MethodBase.GetCurrentMethod().Name,
                                        exceptionMessage));
                LogException(ex, status);
                return (status);
            }
            catch (System.Exception ex)
            {
                status = new questStatus(Severity.Fatal, String.Format("EXCEPTION: {0}.{1}: {2}",
                        this.GetType().Name, MethodBase.GetCurrentMethod().Name,
                        ex.InnerException != null ? ex.InnerException.Message : ex.Message));
                LogException(ex, status);
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        private questStatus delete(MasterPricingEntities dbContext, TablesetId tablesetId)
        {
            // Initialize 
            questStatus status = null;


            try
            {
                // Read the records.
                List<Quest.Services.Dbio.MasterPricing.TablesetViews> _tablesetViewList = null;
                status = read(dbContext, tablesetId, out _tablesetViewList);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }

                // Delete the records.
                dbContext.TablesetViews.RemoveRange(_tablesetViewList);
                dbContext.SaveChanges();
            }
            catch (DbEntityValidationException ex)
            {
                var errorMessages = ex.EntityValidationErrors.SelectMany(x => x.ValidationErrors).Select(x => x.ErrorMessage);
                String fullErrorMessage = string.Join("; ", errorMessages);
                String exceptionMessage = string.Concat(ex.Message, fullErrorMessage);
                status = new questStatus(Severity.Fatal, String.Format("EXCEPTION: {0}.{1}: {2}",
                                        this.GetType().Name, MethodBase.GetCurrentMethod().Name,
                                        exceptionMessage));
                LogException(ex, status);
                return (status);
            }
            catch (System.Exception ex)
            {
                status = new questStatus(Severity.Fatal, String.Format("EXCEPTION: {0}.{1}: {2}",
                        this.GetType().Name, MethodBase.GetCurrentMethod().Name,
                        ex.InnerException != null ? ex.InnerException.Message : ex.Message));
                LogException(ex, status);
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        #endregion

        #endregion
    }
}
