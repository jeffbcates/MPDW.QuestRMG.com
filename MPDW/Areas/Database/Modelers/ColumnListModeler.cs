using System;
using System.Collections.Generic;
using System.Web;
using Quest.Util.Status;
using Quest.Util.Buffer;
using Quest.Util.Data;
using Quest.Functional.ASM;
using Quest.Functional.FMS;
using Quest.MPDW.Models.List;
using Quest.MPDW.Modelers;
using Quest.MasterPricing.Database.Models;
using Quest.Functional.MasterPricing;
using Quest.MasterPricing.Services.Business.Database;


namespace Quest.MasterPricing.Database.Modelers
{
    public class ColumnListModeler : BaseListModeler
    {
        #region Declarations
        /*==================================================================================================================================
         * Declarations
         *=================================================================================================================================*/
        private ColumnListViewModel _columnListViewModel = null;

        #endregion


        #region Constructors
        /*==================================================================================================================================
        * ConstructorsRequisitionListModeler
        *=================================================================================================================================*/
        public ColumnListModeler(HttpRequestBase httpRequestBase, UserSession userSession)
            : base(httpRequestBase, userSession)
        {
            initialize();
        }
        public ColumnListModeler(HttpRequestBase httpRequestBase, UserSession userSession, ColumnListViewModel columnListViewModel)
            : base(httpRequestBase, userSession)
        {
            this._columnListViewModel = columnListViewModel;
            initialize();
        }
        #endregion


        #region Public Methods
        /*==================================================================================================================================
         * Public Methods
         *=================================================================================================================================*/

        #region LIST 
        //----------------------------------------------------------------------------------------------------------------------------------
        // LIST
        //----------------------------------------------------------------------------------------------------------------------------------
        public questStatus List(ColumnListViewModel viewModel, out ColumnListViewModel columnListViewModel)
        {
            // Initialize
            questStatus status = null;
            columnListViewModel = null;


            // Read the database
            DatabaseId databaseId = new DatabaseId(viewModel.DatabaseId);
            DatabaseBaseViewModel databaseBaseViewModel = null;
            DatabaseBaseModeler databaseBaseModeler = new DatabaseBaseModeler(this.HttpRequestBase, this.UserSession);
            status = databaseBaseModeler.GetDatabase(databaseId, out databaseBaseViewModel);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }

            // Get columns according to parent entity type
            ColumnsMgr columnsMgr = new ColumnsMgr(this.UserSession);
            List<Column> columnList = null;
            Table table = null;
            View view = null;
            if (String.Equals(viewModel.ParentEntityType, "table", StringComparison.InvariantCultureIgnoreCase))
            {

                // Get table columns
                TableId tableId = new TableId(viewModel.Id);
                status = columnsMgr.Read(tableId, out columnList);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }

                // Read the table info
                TablesMgr tablesMgr = new TablesMgr(this.UserSession);
                status = tablesMgr.Read(tableId, out table);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }
            }
            else if (String.Equals(viewModel.ParentEntityType, "view", StringComparison.InvariantCultureIgnoreCase))
            {
                // Get view columns
                ViewId viewId = new ViewId(viewModel.Id);
                status = columnsMgr.Read(viewId, out columnList);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }

                // Read the view info
                ViewsMgr viewsMgr = new ViewsMgr(this.UserSession);
                status = viewsMgr.Read(viewId, out view);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }
            }
            else
            {
                return (new questStatus(Severity.Error, String.Format("Invalid ParentEntityType: {0}", viewModel.ParentEntityType)));
            }


            // Sort by display order
            columnList.Sort(delegate (Column i1, Column i2) { return i1.DisplayOrder.CompareTo(i2.DisplayOrder); });


            // Transfer model.
            columnListViewModel = new ColumnListViewModel(this.UserSession, viewModel);
            columnListViewModel.DatabaseId = databaseId.Id;
            columnListViewModel.Database = databaseBaseViewModel;
            columnListViewModel.Id = viewModel.Id;
            columnListViewModel.ParentEntityType = viewModel.ParentEntityType;
            if (table != null)
            {
                TableViewModel tableViewModel = new TableViewModel();
                BufferMgr.TransferBuffer(table, tableViewModel);
                columnListViewModel.Table = tableViewModel;
            }
            if (view != null)
            {
                ViewViewModel viewViewModel = new ViewViewModel();
                BufferMgr.TransferBuffer(view, viewViewModel);
                columnListViewModel.View = viewViewModel;
            }
            foreach (Column column in columnList)
            {
                ColumnLineItemViewModel columnLineItemViewModel = new ColumnLineItemViewModel();
                BufferMgr.TransferBuffer(column, columnLineItemViewModel);
                columnListViewModel.Items.Add(columnLineItemViewModel);
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
            return (new questStatus(Severity.Success));
        }
        #endregion
    }
}