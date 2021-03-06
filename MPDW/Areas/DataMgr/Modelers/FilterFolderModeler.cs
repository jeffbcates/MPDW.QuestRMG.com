﻿using System;
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
            if (filterFolderViewModel.FolderId < BaseId.VALID_ID)
            {
                filterFolder.FolderId = null;
            }


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
            status = filterFoldersMgr.Load(folderId, out filterFolderList);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }

            // Transfer model
            filterFolderViewModel = new FilterFolderViewModel();
            BufferMgr.TransferBuffer(viewModel, filterFolderViewModel);
            foreach (FilterFolder filterFolder in filterFolderList)
            {
                BootstrapTreenodeViewModel bootstrapTreenodeViewModel = null;
                status = FormatBootstrapTreeviewNode(filterFolder, out bootstrapTreenodeViewModel);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }
                filterFolderViewModel.Items.Add(bootstrapTreenodeViewModel);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Load(FolderListViewModel folderListViewModel)
        {
            // Initialize
            questStatus status = null;


            // Load all folders
            FolderId folderId = null;
            List<FilterFolder> filterFolderList = null;
            FilterFoldersMgr filterFoldersMgr = new FilterFoldersMgr(this.UserSession);
            status = filterFoldersMgr.Load(folderId, out filterFolderList);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }

            // Transfer model
            foreach (FilterFolder filterFolder in filterFolderList)
            {
                BootstrapTreenodeViewModel bootstrapTreenodeViewModel = null;
                status = FormatBootstrapTreeviewNode(filterFolder, out bootstrapTreenodeViewModel);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }
                folderListViewModel.Items.Add(bootstrapTreenodeViewModel);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Delete(FilterFolderViewModel filterFolderViewModel)
        {
            // Initialize
            questStatus status = null;



            // Delete the folder
            ////FilterFolderId filterFolderId = new FilterFolderId(filterFolderViewModel.Id);
            ////FilterFoldersMgr filterFoldersMgr = new FilterFoldersMgr(this.UserSession);
            ////status = filterFoldersMgr.Delete(filterFolderId);
            ////if (!questStatusDef.IsSuccess(status))
            ////{
            ////    FormatErrorMessage(status, filterFolderViewModel);
            ////    return (status);
            ////}


            FilterFolderId filterFolderId = new FilterFolderId(filterFolderViewModel.Id);
            FolderMgr folderMgr = new FolderMgr(this.UserSession);
            status = folderMgr.Delete(filterFolderId);
            if (!questStatusDef.IsSuccess(status))
            {
                FormatErrorMessage(status, filterFolderViewModel);
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
            return (new questStatus(Severity.Success));
        }
        #endregion
    }
}