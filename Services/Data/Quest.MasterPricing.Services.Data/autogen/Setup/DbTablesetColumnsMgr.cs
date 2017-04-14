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
    public class DbTablesetColumnsMgr : DbLogsMgr
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
        public DbTablesetColumnsMgr(UserSession userSession)
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
        public questStatus Create(Quest.Functional.MasterPricing.TablesetColumn tablesetColumn, out TablesetColumnId tablesetColumnId)
        {
            // Initialize
            questStatus status = null;
            tablesetColumnId = null;


            // Data rules.


            // Create the tablesetColumn
            using (MasterPricingEntities dbContext = new MasterPricingEntities())
            {
                status = create(dbContext, tablesetColumn, out tablesetColumnId);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Create(DbMgrTransaction trans, Quest.Functional.MasterPricing.TablesetColumn tablesetColumn, out TablesetColumnId tablesetColumnId)
        {
            // Initialize
            questStatus status = null;
            tablesetColumnId = null;


            // Data rules.


            // Create the tablesetColumn in this transaction.
            status = create((MasterPricingEntities)trans.DbContext, tablesetColumn, out tablesetColumnId);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Read(EntityType entityType, TableSetEntityId tableSetEntityId, out List<Quest.Functional.MasterPricing.TablesetColumn> tablesetColumnList)
        {
            // Initialize
            questStatus status = null;
            tablesetColumnList = null;


            using (MasterPricingEntities dbContext = new MasterPricingEntities())
            {
                List<Quest.Services.Dbio.MasterPricing.TablesetColumns> _tablesetColumnList = null;
                status = read(dbContext, entityType, tableSetEntityId, out _tablesetColumnList);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }
                tablesetColumnList = new List<TablesetColumn>();
                foreach (Quest.Services.Dbio.MasterPricing.TablesetColumns _tablesetColumn in _tablesetColumnList)
                {
                    Quest.Functional.MasterPricing.TablesetColumn tablesetColumn = new TablesetColumn();
                    BufferMgr.TransferBuffer(_tablesetColumn, tablesetColumn);
                    tablesetColumnList.Add(tablesetColumn);
                }
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Read(TablesetColumnId tablesetColumnId, out Quest.Functional.MasterPricing.TablesetColumn tablesetColumn)
        {
            // Initialize
            questStatus status = null;
            tablesetColumn = null;


            // Perform read
            using (MasterPricingEntities dbContext = new MasterPricingEntities())
            {
                Quest.Services.Dbio.MasterPricing.TablesetColumns _tablesetColumn = null;
                status = read(dbContext, tablesetColumnId, out _tablesetColumn);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }
                tablesetColumn = new Quest.Functional.MasterPricing.TablesetColumn();
                BufferMgr.TransferBuffer(_tablesetColumn, tablesetColumn);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Read(FilterColumnTablesetColumnId filterColumnTablesetColumnId, out Quest.Functional.MasterPricing.TablesetColumn tablesetColumn)
        {
            // Initialize
            questStatus status = null;
            tablesetColumn = null;


            // Perform read
            using (MasterPricingEntities dbContext = new MasterPricingEntities())
            {
                Quest.Services.Dbio.MasterPricing.TablesetColumns _tablesetColumn = null;
                status = read(dbContext, filterColumnTablesetColumnId, out _tablesetColumn);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }
                tablesetColumn = new Quest.Functional.MasterPricing.TablesetColumn();
                BufferMgr.TransferBuffer(_tablesetColumn, tablesetColumn);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Read(DbMgrTransaction trans, FilterColumnTablesetColumnId filterColumnTablesetColumnId, out Quest.Functional.MasterPricing.TablesetColumn tablesetColumn)
        {
            // Initialize
            questStatus status = null;
            tablesetColumn = null;


            // Perform read
            Quest.Services.Dbio.MasterPricing.TablesetColumns _tablesetColumn = null;
            status = read((MasterPricingEntities)trans.DbContext, filterColumnTablesetColumnId, out _tablesetColumn);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            tablesetColumn = new Quest.Functional.MasterPricing.TablesetColumn();
            BufferMgr.TransferBuffer(_tablesetColumn, tablesetColumn);

            return (new questStatus(Severity.Success));
        }
        public questStatus Read(DbMgrTransaction trans, TablesetColumnId tablesetColumnId, out Quest.Functional.MasterPricing.TablesetColumn tablesetColumn)
        {
            // Initialize
            questStatus status = null;
            tablesetColumn = null;


            // Perform read.
            Quest.Services.Dbio.MasterPricing.TablesetColumns _tablesetColumn = null;
            status = read((MasterPricingEntities)trans.DbContext, tablesetColumnId, out _tablesetColumn);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            tablesetColumn = new Quest.Functional.MasterPricing.TablesetColumn();
            BufferMgr.TransferBuffer(_tablesetColumn, tablesetColumn);

            return (new questStatus(Severity.Success));
        }
        public questStatus Update(Quest.Functional.MasterPricing.TablesetColumn tablesetColumn)
        {
            // Initialize
            questStatus status = null;


            // Perform update.
            using (MasterPricingEntities dbContext = new MasterPricingEntities())
            {
                status = update(dbContext, tablesetColumn);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Update(DbMgrTransaction trans, Quest.Functional.MasterPricing.TablesetColumn tablesetColumn)
        {
            // Initialize
            questStatus status = null;
            bool bCreateTransaction = trans == null;


            // Perform update in this transaction.
            status = update((MasterPricingEntities)trans.DbContext, tablesetColumn);
            if (!questStatusDef.IsSuccess(status))
            {
                RollbackTransaction(trans);
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Delete(TablesetColumnId tablesetColumnId)
        {
            // Initialize
            questStatus status = null;


            // Perform delete.
            using (MasterPricingEntities dbContext = new MasterPricingEntities())
            {
                status = delete(dbContext, tablesetColumnId);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Delete(DbMgrTransaction trans, TablesetColumnId tablesetColumnId)
        {
            // Initialize
            questStatus status = null;


            // Perform delete in this transaction.
            status = delete((MasterPricingEntities)trans.DbContext, tablesetColumnId);
            if (!questStatusDef.IsSuccess(status))
            {
                RollbackTransaction(trans);
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Delete(EntityType entityType, TableSetEntityId tableSetEntityId)
        {
            // Initialize
            questStatus status = null;


            // Perform delete.
            using (MasterPricingEntities dbContext = new MasterPricingEntities())
            {
                status = delete(dbContext, entityType, tableSetEntityId);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Delete(DbMgrTransaction trans, EntityType entityType, TableSetEntityId tableSetEntityId)
        {
            // Initialize
            questStatus status = null;


            // Perform delete in this transaction.
            status = delete((MasterPricingEntities)trans.DbContext, entityType, tableSetEntityId);
            if (!questStatusDef.IsSuccess(status))
            {
                RollbackTransaction(trans);
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


        #region TablesetColumns
        /*----------------------------------------------------------------------------------------------------------------------------------
         * TablesetColumns
         *---------------------------------------------------------------------------------------------------------------------------------*/
        private questStatus create(MasterPricingEntities dbContext, Quest.Functional.MasterPricing.TablesetColumn tablesetColumn, out TablesetColumnId tablesetColumnId)
        {
            // Initialize
            questStatus status = null;
            tablesetColumnId = null;


            // Perform create
            try
            {
                Quest.Services.Dbio.MasterPricing.TablesetColumns _tablesetColumn = new Quest.Services.Dbio.MasterPricing.TablesetColumns();
                BufferMgr.TransferBuffer(tablesetColumn, _tablesetColumn);
                dbContext.TablesetColumns.Add(_tablesetColumn);
                dbContext.SaveChanges();
                if (_tablesetColumn.Id == 0)
                {
                    return (new questStatus(Severity.Error, "Quest.Functional.MasterPricing.TablesetColumn not created"));
                }
                tablesetColumnId = new TablesetColumnId(_tablesetColumn.Id);
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
        private questStatus read(MasterPricingEntities dbContext, TablesetColumnId tablesetColumnId, out Quest.Services.Dbio.MasterPricing.TablesetColumns tablesetColumn)
        {
            // Initialize
            questStatus status = null;
            tablesetColumn = null;


            try
            {
                tablesetColumn = dbContext.TablesetColumns.Where(r => r.Id == tablesetColumnId.Id).SingleOrDefault();
                if (tablesetColumn == null)
                {
                    return (new questStatus(Severity.Error, String.Format("ERROR: {0}.{1}: {2}",
                            this.GetType().Name, MethodBase.GetCurrentMethod().Name,
                            String.Format("Id {0} not found", tablesetColumnId.Id))));
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
        private questStatus read(MasterPricingEntities dbContext, FilterColumnTablesetColumnId filterColumnTablesetColumnId, out Quest.Services.Dbio.MasterPricing.TablesetColumns tablesetColumn)
        {
            // Initialize
            questStatus status = null;
            tablesetColumn = null;


            try
            {
                tablesetColumn = dbContext.TablesetColumns.Where(r => r.EntityTypeId == filterColumnTablesetColumnId.EntityTypeId.Id
                        && r.TableSetEntityId == filterColumnTablesetColumnId.EntityId.Id
                        && r.Name == filterColumnTablesetColumnId.Name).SingleOrDefault();
                if (tablesetColumn == null)
                {
                    return (new questStatus(Severity.Error, String.Format("ERROR: {0}.{1}: {2}",
                            this.GetType().Name, MethodBase.GetCurrentMethod().Name,
                            String.Format("FilterColumnTablesetColumnId EntityTypeId: {0}  EntityId: {1}  Name: {2} not found",
                                    filterColumnTablesetColumnId.EntityTypeId.Id, filterColumnTablesetColumnId.EntityId.Id, filterColumnTablesetColumnId.Name))));
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
        private questStatus read(MasterPricingEntities dbContext, EntityType entityType, TableSetEntityId tableSetEntityId, out List<Quest.Services.Dbio.MasterPricing.TablesetColumns> tablesetColumnList)
        {
            // Initialize
            questStatus status = null;
            tablesetColumnList = null;


            try
            {
                tablesetColumnList = dbContext.TablesetColumns.Where(r => r.EntityTypeId == entityType.Id && r.TableSetEntityId == tableSetEntityId.Id).ToList();
                if (tablesetColumnList == null)
                {
                    return (new questStatus(Severity.Error, String.Format("ERROR: {0}.{1}: {2}",
                            this.GetType().Name, MethodBase.GetCurrentMethod().Name,
                            String.Format("TablesetColumn for EntityType {0} TableSetEntityId {1} not found", entityType.Id, tableSetEntityId.Id))));
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
        private questStatus update(MasterPricingEntities dbContext, Quest.Functional.MasterPricing.TablesetColumn tablesetColumn)
        {
            // Initialize 
            questStatus status = null;


            try
            {
                // Read the record.
                TablesetColumnId tablesetColumnId = new TablesetColumnId(tablesetColumn.Id);
                Quest.Services.Dbio.MasterPricing.TablesetColumns _tablesetColumn = null;
                status = read(dbContext, tablesetColumnId, out _tablesetColumn);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }

                // Update the record.
                BufferMgr.TransferBuffer(tablesetColumn, _tablesetColumn);
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
        private questStatus delete(MasterPricingEntities dbContext, TablesetColumnId tablesetColumnId)
        {
            // Initialize 
            questStatus status = null;


            try
            {
                // Read the record.
                Quest.Services.Dbio.MasterPricing.TablesetColumns _tablesetColumn = null;
                status = read(dbContext, tablesetColumnId, out _tablesetColumn);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }

                // Delete the record.
                dbContext.TablesetColumns.Remove(_tablesetColumn);
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
        private questStatus delete(MasterPricingEntities dbContext, EntityType entityType, TableSetEntityId tableSetEntityId)
        {
            // Initialize 
            questStatus status = null;


            try
            {
                // Read the records.
                List<Quest.Services.Dbio.MasterPricing.TablesetColumns> tablesetColumnList = null;
                status = read(dbContext, entityType, tableSetEntityId, out tablesetColumnList);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }

                // Delete the records.
                dbContext.TablesetColumns.RemoveRange(tablesetColumnList);
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
