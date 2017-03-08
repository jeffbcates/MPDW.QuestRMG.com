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
    public class DbViewsMgr : DbMgrSessionBased
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
        public DbViewsMgr(UserSession userSession)
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
        public questStatus Create(Quest.Functional.MasterPricing.View view, out ViewId viewId)
        {
            // Initialize
            questStatus status = null;
            viewId = null;


            // Data rules.


            // Create the view
            using (MasterPricingEntities dbContext = new MasterPricingEntities())
            {
                status = create(dbContext, view, out viewId);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Create(DbMgrTransaction trans, Quest.Functional.MasterPricing.View view, out ViewId viewId)
        {
            // Initialize
            questStatus status = null;
            viewId = null;


            // Data rules.


            // Create the view in this transaction.
            status = create((MasterPricingEntities)trans.DbContext, view, out viewId);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Create(DbMgrTransaction trans, List<Quest.Functional.MasterPricing.View> viewList, out List<Quest.Functional.MasterPricing.View> viewIdList)
        {
            // Initialize
            questStatus status = null;
            viewIdList = null;


            // Data rules.


            // Create the views in this transaction.
            status = create((MasterPricingEntities)trans.DbContext, viewList, out viewIdList);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Read(ViewId viewId, out Quest.Functional.MasterPricing.View view)
        {
            // Initialize
            questStatus status = null;
            view = null;


            // Perform read
            using (MasterPricingEntities dbContext = new MasterPricingEntities())
            {
                Quest.Services.Dbio.MasterPricing.Views _view = null;
                status = read(dbContext, viewId, out _view);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }
                view = new Quest.Functional.MasterPricing.View();
                BufferMgr.TransferBuffer(_view, view);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Read(DatabaseId databaseId, string schema, string name, out Quest.Functional.MasterPricing.View view)
        {
            // Initialize
            questStatus status = null;
            view = null;


            // Perform read
            using (MasterPricingEntities dbContext = new MasterPricingEntities())
            {
                Quest.Services.Dbio.MasterPricing.Views _view = null;
                status = read(dbContext, databaseId, schema, name, out _view);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }
                view = new Quest.Functional.MasterPricing.View();
                BufferMgr.TransferBuffer(_view, view);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Read(DatabaseId databaseId, out List<Quest.Functional.MasterPricing.View> viewList)
        {
            // Initialize
            questStatus status = null;
            viewList = null;


            // Perform read
            using (MasterPricingEntities dbContext = new MasterPricingEntities())
            {
                List<Quest.Services.Dbio.MasterPricing.Views> _viewList = null;
                status = read(dbContext, databaseId, out _viewList);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }
                viewList = new List<View>();
                foreach (Quest.Services.Dbio.MasterPricing.Views _view in _viewList)
                {
                    Quest.Functional.MasterPricing.View view = new View();
                    BufferMgr.TransferBuffer(_view, view);
                    viewList.Add(view);
                }
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Read(DbMgrTransaction trans, DatabaseId databaseId, out List<Quest.Functional.MasterPricing.View> viewList)
        {
            // Initialize
            questStatus status = null;
            viewList = null;


            // Perform read
            List<Quest.Services.Dbio.MasterPricing.Views> _viewList = null;
            status = read((MasterPricingEntities)trans.DbContext, databaseId, out _viewList);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            viewList = new List<View>();
            foreach (Quest.Services.Dbio.MasterPricing.Views _view in _viewList)
            {
                Quest.Functional.MasterPricing.View view = new View();
                BufferMgr.TransferBuffer(_view, view);
                viewList.Add(view);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Read(DbMgrTransaction trans, ViewId viewId, out Quest.Functional.MasterPricing.View view)
        {
            // Initialize
            questStatus status = null;
            view = null;


            // Perform read.
            Quest.Services.Dbio.MasterPricing.Views _view = null;
            status = read((MasterPricingEntities)trans.DbContext, viewId, out _view);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            view = new Quest.Functional.MasterPricing.View();
            BufferMgr.TransferBuffer(_view, view);


            return (new questStatus(Severity.Success));
        }
        public questStatus Update(Quest.Functional.MasterPricing.View view)
        {
            // Initialize
            questStatus status = null;


            // Perform update.
            using (MasterPricingEntities dbContext = new MasterPricingEntities())
            {
                status = update(dbContext, view);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Update(DbMgrTransaction trans, Quest.Functional.MasterPricing.View view)
        {
            // Initialize
            questStatus status = null;
            bool bCreateTransaction = trans == null;


            // Perform update in this transaction.
            status = update((MasterPricingEntities)trans.DbContext, view);
            if (!questStatusDef.IsSuccess(status))
            {
                RollbackTransaction(trans);
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Delete(ViewId viewId)
        {
            // Initialize
            questStatus status = null;


            // Perform delete.
            using (MasterPricingEntities dbContext = new MasterPricingEntities())
            {
                status = delete(dbContext, viewId);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Delete(DbMgrTransaction trans, ViewId viewId)
        {
            // Initialize
            questStatus status = null;


            // Perform delete in this transaction.
            status = delete((MasterPricingEntities)trans.DbContext, viewId);
            if (!questStatusDef.IsSuccess(status))
            {
                RollbackTransaction(trans);
                return (status);
            }
            return (new questStatus(Severity.Success));
        }

        public questStatus Delete(DatabaseId databaseId)
        {
            // Initialize
            questStatus status = null;


            // Perform delete.
            using (MasterPricingEntities dbContext = new MasterPricingEntities())
            {
                status = delete(dbContext, databaseId);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Delete(DbMgrTransaction trans, DatabaseId databaseId)
        {
            // Initialize
            questStatus status = null;


            // Perform delete in this transaction.
            status = delete((MasterPricingEntities)trans.DbContext, databaseId);
            if (!questStatusDef.IsSuccess(status))
            {
                RollbackTransaction(trans);
                return (status);
            }
            return (new questStatus(Severity.Success));
        }



        public questStatus List(QueryOptions queryOptions, out List<Quest.Functional.MasterPricing.View> viewList, out QueryResponse queryResponse)
        {
            // Initialize
            questStatus status = null;
            viewList = null;
            queryResponse = null;


            using (MasterPricingEntities dbContext = new MasterPricingEntities())
            {
                using (DbContextTransaction tx = dbContext.Database.BeginTransaction(IsolationLevel.ReadUncommitted))
                {
                    try
                    {
                        PropertyInfo[] dbProperties = typeof(Quest.Services.Dbio.MasterPricing.Views).GetProperties().ToArray();
                        int totalRecords = dbContext.Views.Where(BuildWhereClause(queryOptions, dbProperties)).Count();
                        List<Quest.Services.Dbio.MasterPricing.Views> _countriesList = dbContext.Views.Where(BuildWhereClause(queryOptions, dbProperties))
                                .OrderBy(BuildSortString(queryOptions.SortColumns))
                                .Skip(queryOptions.Paging.PageSize * (queryOptions.Paging.PageNumber - 1))
                                .Take(queryOptions.Paging.PageSize).ToList();
                        if (_countriesList == null)
                        {
                            return (new questStatus(Severity.Warning));
                        }
                        viewList = new List<Quest.Functional.MasterPricing.View>();
                        foreach (Quest.Services.Dbio.MasterPricing.Views _view in _countriesList)
                        {
                            Quest.Functional.MasterPricing.View view = new Quest.Functional.MasterPricing.View();
                            BufferMgr.TransferBuffer(_view, view);
                            viewList.Add(view);
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


        #region Views
        /*----------------------------------------------------------------------------------------------------------------------------------
         * Views
         *---------------------------------------------------------------------------------------------------------------------------------*/
        private questStatus create(MasterPricingEntities dbContext, Quest.Functional.MasterPricing.View view, out ViewId viewId)
        {
            // Initialize
            viewId = null;


            // Perform create
            try
            {
                Quest.Services.Dbio.MasterPricing.Views _view = new Quest.Services.Dbio.MasterPricing.Views();
                BufferMgr.TransferBuffer(view, _view);
                dbContext.Views.Add(_view);
                dbContext.SaveChanges();
                if (_view.Id == 0)
                {
                    return (new questStatus(Severity.Error, "Quest.Functional.MasterPricing.View not created"));
                }
                viewId = new ViewId(_view.Id);
            }
            catch (System.Exception ex)
            {
                return (new questStatus(Severity.Fatal, String.Format("EXCEPTION: {0}.{1}: {2}",
                        this.GetType().Name, MethodBase.GetCurrentMethod().Name,
                        ex.InnerException != null ? ex.InnerException.Message : ex.Message)));
            }
            return (new questStatus(Severity.Success));
        }
        private questStatus create(MasterPricingEntities dbContext, List<Quest.Functional.MasterPricing.View> viewList, out List<Quest.Functional.MasterPricing.View> viewIdList)
        {
            // Initialize
            viewIdList = null;


            // Perform create
            try
            {
                List<Quest.Services.Dbio.MasterPricing.Views> _viewList = new List<Views>();
                foreach (Quest.Functional.MasterPricing.View view in viewList)
                {
                    Quest.Services.Dbio.MasterPricing.Views _view = new Quest.Services.Dbio.MasterPricing.Views();
                    BufferMgr.TransferBuffer(view, _view);
                    _viewList.Add(_view);
                }
                dbContext.Views.AddRange(_viewList);
                dbContext.SaveChanges();

                viewIdList = new List<View>();
                foreach (Quest.Services.Dbio.MasterPricing.Views _view in _viewList)
                {
                    Quest.Functional.MasterPricing.View view = new View();
                    BufferMgr.TransferBuffer(_view, view);
                    viewIdList.Add(view);
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
        private questStatus read(MasterPricingEntities dbContext, ViewId viewId, out Quest.Services.Dbio.MasterPricing.Views view)
        {
            // Initialize
            view = null;


            try
            {
                view = dbContext.Views.Where(r => r.Id == viewId.Id).SingleOrDefault();
                if (view == null)
                {
                    return (new questStatus(Severity.Error, String.Format("ERROR: {0}.{1}: {2}",
                            this.GetType().Name, MethodBase.GetCurrentMethod().Name,
                            String.Format("Id {0} not found", viewId.Id))));
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
        private questStatus read(MasterPricingEntities dbContext, DatabaseId databaseId, string schema, string name, out Quest.Services.Dbio.MasterPricing.Views view)
        {
            // Initialize
            view = null;


            try
            {
                view = dbContext.Views.Where(r => r.DatabaseId == databaseId.Id && r.Schema == schema && r.Name == name).SingleOrDefault();
                if (view == null)
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
        private questStatus read(MasterPricingEntities dbContext, DatabaseId databaseId, out List<Quest.Services.Dbio.MasterPricing.Views> viewsList)
        {
            // Initialize
            viewsList = null;


            try
            {
                viewsList = dbContext.Views.Where(r => r.DatabaseId == databaseId.Id).ToList();
                if (viewsList == null)
                {
                    return (new questStatus(Severity.Error, String.Format("ERROR: {0}.{1}: {2}",
                            this.GetType().Name, MethodBase.GetCurrentMethod().Name,
                            String.Format("DatabaseId {0} not found", databaseId.Id))));
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
        private questStatus update(MasterPricingEntities dbContext, Quest.Functional.MasterPricing.View view)
        {
            // Initialize 
            questStatus status = null;


            try
            {
                // Read the record.
                ViewId viewId = new ViewId(view.Id);
                Quest.Services.Dbio.MasterPricing.Views _view = null;
                status = read(dbContext, viewId, out _view);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }

                // Update the record.
                BufferMgr.TransferBuffer(view, _view);
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
        private questStatus delete(MasterPricingEntities dbContext, ViewId viewId)
        {
            // Initialize 
            questStatus status = null;


            try
            {
                // Read the record.
                Quest.Services.Dbio.MasterPricing.Views _view = null;
                status = read(dbContext, viewId, out _view);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }

                // Delete the record.
                dbContext.Views.Remove(_view);
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
        private questStatus delete(MasterPricingEntities dbContext, DatabaseId databaseId)
        {
            // Initialize 
            questStatus status = null;


            try
            {
                // Read the record.
                List<Quest.Services.Dbio.MasterPricing.Views> _viewsList = null;
                status = read(dbContext, databaseId, out _viewsList);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }

                // Delete the records.
                dbContext.Views.RemoveRange(_viewsList);
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
