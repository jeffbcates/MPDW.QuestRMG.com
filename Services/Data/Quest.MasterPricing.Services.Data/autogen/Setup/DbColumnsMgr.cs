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
    public class DbColumnsMgr : DbLogsMgr
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
        public DbColumnsMgr(UserSession userSession)
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
        public questStatus Create(Quest.Functional.MasterPricing.Column column, out ColumnId columnId)
        {
            // Initialize
            questStatus status = null;
            columnId = null;


            // Data rules.


            // Create the column
            using (MasterPricingEntities dbContext = new MasterPricingEntities())
            {
                status = create(dbContext, column, out columnId);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Create(DbMgrTransaction trans, Quest.Functional.MasterPricing.Column column, out ColumnId columnId)
        {
            // Initialize
            questStatus status = null;
            columnId = null;


            // Data rules.


            // Create the column in this transaction.
            status = create((MasterPricingEntities)trans.DbContext, column, out columnId);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Create(DbMgrTransaction trans, List<Quest.Functional.MasterPricing.Column> columnList, out List<Quest.Functional.MasterPricing.Column> columnIdList)
        {
            // Initialize
            questStatus status = null;
            columnIdList = null;


            // Data rules.


            // Create the columns in this transaction.
            status = create((MasterPricingEntities)trans.DbContext, columnList, out columnIdList);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Read(ColumnId columnId, out Quest.Functional.MasterPricing.Column column)
        {
            // Initialize
            questStatus status = null;
            column = null;


            // Perform read
            using (MasterPricingEntities dbContext = new MasterPricingEntities())
            {
                Quest.Services.Dbio.MasterPricing.Columns _columns = null;
                status = read(dbContext, columnId, out _columns);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }
                column = new Quest.Functional.MasterPricing.Column();
                BufferMgr.TransferBuffer(_columns, column);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Read(DbMgrTransaction trans, ColumnId columnId, out Quest.Functional.MasterPricing.Column column)
        {
            // Initialize
            questStatus status = null;
            column = null;


            // Perform read
            Quest.Services.Dbio.MasterPricing.Columns _columns = null;
            status = read((MasterPricingEntities)trans.DbContext, columnId, out _columns);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            column = new Quest.Functional.MasterPricing.Column();
            BufferMgr.TransferBuffer(_columns, column);

            return (new questStatus(Severity.Success));
        }
        public questStatus Read(TableId tableId, out List<Quest.Functional.MasterPricing.Column> columnList)
        {
            // Initialize
            questStatus status = null;
            columnList = null;


            // Perform read
            using (MasterPricingEntities dbContext = new MasterPricingEntities())
            {
                List<Quest.Services.Dbio.MasterPricing.Columns> _columnsList = null;
                status = read(dbContext, tableId, out _columnsList);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }
                columnList = new List<Column>();
                foreach (Quest.Services.Dbio.MasterPricing.Columns _column in _columnsList)
                {
                    Quest.Functional.MasterPricing.Column column = new Quest.Functional.MasterPricing.Column();
                    BufferMgr.TransferBuffer(_column, column);
                    columnList.Add(column);
                }
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Read(DbMgrTransaction trans, TableId tableId, out List<Quest.Functional.MasterPricing.Column> columnList)
        {
            // Initialize
            questStatus status = null;
            columnList = null;


            // Perform read
            List<Quest.Services.Dbio.MasterPricing.Columns> _columnsList = null;
            status = read((MasterPricingEntities)trans.DbContext, tableId, out _columnsList);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            columnList = new List<Column>();
            foreach (Quest.Services.Dbio.MasterPricing.Columns _column in _columnsList)
            {
                Quest.Functional.MasterPricing.Column column = new Quest.Functional.MasterPricing.Column();
                BufferMgr.TransferBuffer(_column, column);
                columnList.Add(column);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Read(ViewId viewId, out List<Quest.Functional.MasterPricing.Column> columnList)
        {
            // Initialize
            questStatus status = null;
            columnList = null;


            // Perform read
            using (MasterPricingEntities dbContext = new MasterPricingEntities())
            {
                List<Quest.Services.Dbio.MasterPricing.Columns> _columnsList = null;
                status = read(dbContext, viewId, out _columnsList);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }
                columnList = new List<Column>();
                foreach (Quest.Services.Dbio.MasterPricing.Columns _column in _columnsList)
                {
                    Quest.Functional.MasterPricing.Column column = new Quest.Functional.MasterPricing.Column();
                    BufferMgr.TransferBuffer(_column, column);
                    columnList.Add(column);
                }
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Read(DbMgrTransaction trans, ViewId viewId, out List<Quest.Functional.MasterPricing.Column> columnList)
        {
            // Initialize
            questStatus status = null;
            columnList = null;


            // Perform read
            List<Quest.Services.Dbio.MasterPricing.Columns> _columnsList = null;
            status = read((MasterPricingEntities)trans.DbContext, viewId, out _columnsList);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            columnList = new List<Column>();
            foreach (Quest.Services.Dbio.MasterPricing.Columns _column in _columnsList)
            {
                Quest.Functional.MasterPricing.Column column = new Quest.Functional.MasterPricing.Column();
                BufferMgr.TransferBuffer(_column, column);
                columnList.Add(column);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Read(EntityTypeId entityTypeId, EntityId entityId, string name, out Quest.Functional.MasterPricing.Column column)
        {
            // Initialize
            questStatus status = null;
            column = null;


            // Perform read
            using (MasterPricingEntities dbContext = new MasterPricingEntities())
            {
                Quest.Services.Dbio.MasterPricing.Columns _columns = null;
                status = read(dbContext, entityTypeId, entityId, name, out _columns);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }
                column = new Quest.Functional.MasterPricing.Column();
                BufferMgr.TransferBuffer(_columns, column);
            }
            return (new questStatus(Severity.Success));
        }



        public questStatus Update(Quest.Functional.MasterPricing.Column column)
        {
            // Initialize
            questStatus status = null;


            // Perform update.
            using (MasterPricingEntities dbContext = new MasterPricingEntities())
            {
                status = update(dbContext, column);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Update(DbMgrTransaction trans, Quest.Functional.MasterPricing.Column column)
        {
            // Initialize
            questStatus status = null;
            bool bCreateTransaction = trans == null;


            // Perform update in this transaction.
            status = update((MasterPricingEntities)trans.DbContext, column);
            if (!questStatusDef.IsSuccess(status))
            {
                RollbackTransaction(trans);
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Delete(ColumnId columnId)
        {
            // Initialize
            questStatus status = null;


            // Perform delete.
            using (MasterPricingEntities dbContext = new MasterPricingEntities())
            {
                status = delete(dbContext, columnId);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Delete(DbMgrTransaction trans, ColumnId columnId)
        {
            // Initialize
            questStatus status = null;


            // Perform delete in this transaction.
            status = delete((MasterPricingEntities)trans.DbContext, columnId);
            if (!questStatusDef.IsSuccess(status))
            {
                RollbackTransaction(trans);
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Delete(TableId tableId)
        {
            // Initialize
            questStatus status = null;


            // Perform delete.
            using (MasterPricingEntities dbContext = new MasterPricingEntities())
            {
                status = delete(dbContext, tableId);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Delete(DbMgrTransaction trans, TableId tableId)
        {
            // Initialize
            questStatus status = null;


            // Perform delete in this transaction.
            status = delete((MasterPricingEntities)trans.DbContext, tableId);
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
        public questStatus List(QueryOptions queryOptions, out List<Quest.Functional.MasterPricing.Column> columnList, out QueryResponse queryResponse)
        {
            // Initialize
            questStatus status = null;
            columnList = null;
            queryResponse = null;


            using (MasterPricingEntities dbContext = new MasterPricingEntities())
            {
                using (DbContextTransaction tx = dbContext.Database.BeginTransaction(IsolationLevel.ReadUncommitted))
                {
                    try
                    {
                        PropertyInfo[] dbProperties = typeof(Quest.Services.Dbio.MasterPricing.Columns).GetProperties().ToArray();
                        int totalRecords = dbContext.Columns.Where(BuildWhereClause(queryOptions, dbProperties)).Count();
                        List<Quest.Services.Dbio.MasterPricing.Columns> _countriesList = dbContext.Columns.Where(BuildWhereClause(queryOptions, dbProperties))
                                .OrderBy(BuildSortString(queryOptions.SortColumns))
                                .Skip(queryOptions.Paging.PageSize * (queryOptions.Paging.PageNumber - 1))
                                .Take(queryOptions.Paging.PageSize).ToList();
                        if (_countriesList == null)
                        {
                            return (new questStatus(Severity.Warning));
                        }
                        columnList = new List<Quest.Functional.MasterPricing.Column>();
                        foreach (Quest.Services.Dbio.MasterPricing.Columns _columns in _countriesList)
                        {
                            Quest.Functional.MasterPricing.Column column = new Quest.Functional.MasterPricing.Column();
                            BufferMgr.TransferBuffer(_columns, column);
                            columnList.Add(column);
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


        #region Columns
        /*----------------------------------------------------------------------------------------------------------------------------------
         * Columns
         *---------------------------------------------------------------------------------------------------------------------------------*/
        private questStatus create(MasterPricingEntities dbContext, Quest.Functional.MasterPricing.Column column, out ColumnId columnId)
        {
            // Initialize
            questStatus status = null;
            columnId = null;


            // Perform create
            try
            {
                Quest.Services.Dbio.MasterPricing.Columns _columns = new Quest.Services.Dbio.MasterPricing.Columns();
                BufferMgr.TransferBuffer(column, _columns);
                dbContext.Columns.Add(_columns);
                dbContext.SaveChanges();
                if (_columns.Id == 0)
                {
                    return (new questStatus(Severity.Error, "Quest.Functional.MasterPricing.Column not created"));
                }
                columnId = new ColumnId(_columns.Id);
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
        private questStatus create(MasterPricingEntities dbContext, List<Quest.Functional.MasterPricing.Column> columnList, out List<Quest.Functional.MasterPricing.Column> columnIdList)
        {
            // Initialize
            questStatus status = null;
            columnIdList = null;


            // Perform create
            try
            {
                List<Quest.Services.Dbio.MasterPricing.Columns> _columnList = new List<Columns>();
                foreach (Quest.Functional.MasterPricing.Column column in columnList)
                {
                    Quest.Services.Dbio.MasterPricing.Columns _column = new Quest.Services.Dbio.MasterPricing.Columns();
                    BufferMgr.TransferBuffer(column, _column);
                    _columnList.Add(_column);
                }
                dbContext.Columns.AddRange(_columnList);
                dbContext.SaveChanges();

                columnIdList = new List<Column>();
                foreach (Quest.Services.Dbio.MasterPricing.Columns _column in _columnList)
                {
                    Quest.Functional.MasterPricing.Column column = new Column();
                    column.Id = _column.Id;
                    columnIdList.Add(column);
                }
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
        private questStatus read(MasterPricingEntities dbContext, ColumnId columnId, out Quest.Services.Dbio.MasterPricing.Columns column)
        {
            // Initialize
            questStatus status = null;
            column = null;


            try
            {
                column = dbContext.Columns.Where(r => r.Id == columnId.Id).SingleOrDefault();
                if (column == null)
                {
                    return (new questStatus(Severity.Error, String.Format("ERROR: {0}.{1}: {2}",
                            this.GetType().Name, MethodBase.GetCurrentMethod().Name,
                            String.Format("Id {0} not found", columnId.Id))));
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
        private questStatus read(MasterPricingEntities dbContext, TableId tableId, out List<Quest.Services.Dbio.MasterPricing.Columns> columnList)
        {
            // Initialize
            questStatus status = null;
            columnList = null;


            try
            {
                columnList = dbContext.Columns.Where(r => r.EntityTypeId == 1 && r.EntityId == tableId.Id).ToList();
                if (columnList == null)
                {
                    return (new questStatus(Severity.Error, String.Format("ERROR: {0}.{1}: {2}",
                            this.GetType().Name, MethodBase.GetCurrentMethod().Name,
                            String.Format("TableId {0} not found", tableId.Id))));
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
        private questStatus read(MasterPricingEntities dbContext, ViewId viewId, out List<Quest.Services.Dbio.MasterPricing.Columns> columnList)
        {
            // Initialize
            questStatus status = null;
            columnList = null;


            try
            {
                columnList = dbContext.Columns.Where(r => r.EntityTypeId == 2 && r.EntityId == viewId.Id).ToList();
                if (columnList == null)
                {
                    return (new questStatus(Severity.Error, String.Format("ERROR: {0}.{1}: {2}",
                            this.GetType().Name, MethodBase.GetCurrentMethod().Name,
                            String.Format("ViewId {0} not found", viewId.Id))));
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
        private questStatus read(MasterPricingEntities dbContext, EntityTypeId entityTypeId, EntityId entityId, string name, out Quest.Services.Dbio.MasterPricing.Columns column)
        {
            // Initialize
            questStatus status = null;
            column = null;


            try
            {
                column = dbContext.Columns.Where(r => r.EntityTypeId == entityTypeId.Id && r.EntityId == entityId.Id && r.Name == name).SingleOrDefault();
                if (column == null)
                {
                    return (new questStatus(Severity.Error, String.Format("ERROR: {0}.{1}: {2}",
                            this.GetType().Name, MethodBase.GetCurrentMethod().Name,
                            String.Format("EntityId {0} Name {1} not found", entityId.Id, name))));
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
        private questStatus update(MasterPricingEntities dbContext, Quest.Functional.MasterPricing.Column column)
        {
            // Initialize 
            questStatus status = null;


            try
            {
                // Read the record.
                ColumnId columnId = new ColumnId(column.Id);
                Quest.Services.Dbio.MasterPricing.Columns _columns = null;
                status = read(dbContext, columnId, out _columns);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }

                // Update the record.
                BufferMgr.TransferBuffer(column, _columns);
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
        private questStatus delete(MasterPricingEntities dbContext, ColumnId columnId)
        {
            // Initialize 
            questStatus status = null;


            try
            {
                // Read the record.
                Quest.Services.Dbio.MasterPricing.Columns _columns = null;
                status = read(dbContext, columnId, out _columns);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }

                // Delete the record.
                dbContext.Columns.Remove(_columns);
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
        private questStatus delete(MasterPricingEntities dbContext, TableId tableId)
        {
            // Initialize 
            questStatus status = null;


            try
            {
                // Read all columns for this table.
                List<Quest.Services.Dbio.MasterPricing.Columns> _columnsList = null;
                status = read(dbContext, tableId, out _columnsList);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }

                // Delete the records.
                dbContext.Columns.RemoveRange(_columnsList);
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
                // Read all columns for this view.
                List<Quest.Services.Dbio.MasterPricing.Columns> _columnsList = null;
                status = read(dbContext, viewId, out _columnsList);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }

                // Delete the records.
                dbContext.Columns.RemoveRange(_columnsList);
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
