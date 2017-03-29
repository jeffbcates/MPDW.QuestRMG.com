using System;
using System.Collections.Generic;
using System.Web;
using Quest.Util.Status;
using Quest.Util.Buffer;
using Quest.Util.Data;
using Quest.Functional.ASM;
using Quest.Functional.FMS;
using Quest.Functional.MasterPricing;
using Quest.MPDW.Models;
using Quest.MPDW.Modelers;
using Quest.MasterPricing.DataMgr.Models;
using Quest.MasterPricing.Services.Business.Filters;


namespace Quest.MasterPricing.DataMgr.Modelers
{
    public class FilterFolderModeler : DataMgrBaseModeler
    {
        #region Declarations
        /*==================================================================================================================================
         * Declarations
         *=================================================================================================================================*/
        private DataMgrBaseViewModel _dataMgrBaseViewModel = null;

        #endregion


        #region Constructors
        /*==================================================================================================================================
        * ConstructorsRequisitionListModeler
        *=================================================================================================================================*/
        public FilterFolderModeler(HttpRequestBase httpRequestBase, UserSession userSession)
            : base(httpRequestBase, userSession)
        {
            initialize();
        }
        public FilterFolderModeler(HttpRequestBase httpRequestBase, UserSession userSession, DataMgrBaseViewModel dataMgrBaseViewModel)
            : base(httpRequestBase, userSession)
        {
            this._dataMgrBaseViewModel = dataMgrBaseViewModel;
            initialize();
        }
        #endregion


        #region Public Methods
        /*==================================================================================================================================
         * Public Methods
         *=================================================================================================================================*/
        public questStatus Save(FilterFolderViewModel filterFolderViewModel)
        {
            // Initialize
            questStatus status = null;


            // Transfer model
            Quest.Functional.MasterPricing.FilterFolder filterFolder = new Functional.MasterPricing.FilterFolder();
            BufferMgr.TransferBuffer(filterFolderViewModel, filterFolder, true);


            // Determine if this is a create or update
            FilterFoldersMgr filterFoldersMgr = new FilterFoldersMgr(this.UserSession);
            if (filterFolderViewModel.Id < BaseId.VALID_ID)
            {
                // Create
                FilterFolderId filterFolderId = null;
                status = filterFoldersMgr.Create(filterFolder, out filterFolderId);
                if (!questStatusDef.IsSuccess(status))
                {
                    FormatErrorMessage(status, filterFolderViewModel);
                    return (status);
                }
                filterFolderViewModel.Id = filterFolderId.Id;
            }
            else
            {
                // Update
                status = filterFoldersMgr.Update(filterFolder);
                if (!questStatusDef.IsSuccess(status))
                {
                    FormatErrorMessage(status, filterFolderViewModel);
                    return (status);
                }
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Read(FilterFolderViewModel viewModel, out FilterFolderViewModel filterFolderViewModel)
        {
            // Initialize
            questStatus status = null;
            filterFolderViewModel = null;


            // Read filter
            FolderId folderId = new FolderId(viewModel.Id);
            List<FilterFolder> filterFolderList = null;
            FilterFoldersMgr filterFoldersMgr = new FilterFoldersMgr(this.UserSession);
            status = filterFoldersMgr.Read(folderId, out filterFolderList);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }

            // Transfer model
            filterFolderViewModel = new FilterFolderViewModel();
            BufferMgr.TransferBuffer(viewModel, filterFolderViewModel);
            foreach (FilterFolder filterFolder in filterFolderList)
            {
                DynatreeNode dynatreeNode = null;
                status = FormatDynatreeNode(filterFolder, out dynatreeNode);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }
                filterFolderViewModel.Items.Add(dynatreeNode);
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
            return (new questStatus(Severity.Success));
        }
        #endregion
    }
}