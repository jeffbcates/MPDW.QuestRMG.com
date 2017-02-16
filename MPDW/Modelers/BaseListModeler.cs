using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Quest.Util.Status;
using Quest.Util.Buffer;
using Quest.Util.Data;
using Quest.Functional.ASM;
using Quest.Functional.FMS;
using Quest.MPDW.Models;
using Quest.MPDW.Models.List;
using Quest.MPDW.Services.Data;
using Quest.MPDW.Services.Business.Accounts;


namespace Quest.MPDW.Modelers
{
    public class BaseListModeler : BaseModeler
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
        public BaseListModeler(HttpRequestBase httpRequestBase)
            : base(httpRequestBase)
        {
            initialize();
        }
        public BaseListModeler(HttpRequestBase httpRequestBase, UserSession userSession)
            : base(httpRequestBase, userSession)
        {
            initialize();
        }
        #endregion


        #region Public Methods
        /*==================================================================================================================================
        * Public Methods
        *=================================================================================================================================*/


        #region Options
        //----------------------------------------------------------------------------------------------------------------------------------
        // Options
        //----------------------------------------------------------------------------------------------------------------------------------
        #endregion


        #region Validations
        //----------------------------------------------------------------------------------------------------------------------------------
        // Validations
        //----------------------------------------------------------------------------------------------------------------------------------
        #endregion


        #region Paging
        //----------------------------------------------------------------------------------------------------------------------------------
        // Paging
        //----------------------------------------------------------------------------------------------------------------------------------
        public questStatus ParsePagingOptions(HttpRequestBase httpRequestBase, out QueryOptions queryOptions)
        {
            // Initialize
            queryOptions = null;
            char[] delimiters = { '[', ']' };

            // Parse
            try
            {
                QueryOptions _queryOptions = new QueryOptions();
                for (int index = 0; index < httpRequestBase.QueryString.AllKeys.Length; index++)
                {
                    string key = httpRequestBase.QueryString.AllKeys[index];

                    string[] parts = key.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);
                    if (parts.Length == 0) { continue; }
                    if (parts[0] == "SortColumns")
                    {
                        if (parts[1] == "Columns")
                        {
                            int _sortColumnIndex = BaseId.INVALID_ID;
                            if (!int.TryParse(parts[2], out _sortColumnIndex))
                            {
                                throw new System.Exception("Invalid List SortColumns index: " + parts[2] == null ? "null" : parts[2]);
                            }
                            SortColumn sortColumn = null;
                            if (_queryOptions.SortColumns.Columns.Count < (_sortColumnIndex + 1))
                            {
                                sortColumn = new SortColumn();
                                _queryOptions.SortColumns.Columns.Add(sortColumn);
                            }
                            else
                            {
                                sortColumn = _queryOptions.SortColumns.Columns[_sortColumnIndex];
                            }
                            if (parts[3] == "Name")
                            {
                                sortColumn.Name = httpRequestBase.QueryString[index];
                            }
                            else if (parts[3] == "Direction")
                            {
                                sortColumn.Direction = httpRequestBase.QueryString[index] == "2" ? SortDirection.DESC : SortDirection.ASC;
                            }
                        }
                    }
                    else if (parts[0] == "Paging")
                    {
                        if (parts[1] == "PageNumber")
                        {
                            _queryOptions.Paging.PageNumber = Convert.ToInt32(httpRequestBase.QueryString[index]);
                        }
                        else if (parts[1] == "PageSize")
                        {
                            _queryOptions.Paging.PageSize = Convert.ToInt32(httpRequestBase.QueryString[index]);
                        }
                    }
                    else if (parts[0] == "SearchOptions")
                    {
                        if (parts[1] == "SearchString")
                        {
                            _queryOptions.SearchOptions.SearchString = httpRequestBase.QueryString[index];
                        }
                    }
                }
                queryOptions = _queryOptions;
            }
            catch (System.Exception ex)
            {
                throw new System.Exception("Error parsing list QueryOptions: " + ex.Message);
            }
            return (new questStatus(Severity.Success));
        }
        #endregion


        #region Query Options
        //----------------------------------------------------------------------------------------------------------------------------------
        // Query Options
        //----------------------------------------------------------------------------------------------------------------------------------
		public questStatus TransferQueryOptions(QueryOptionsViewModel queryOptionsViewModel, out QueryOptions queryOptions)
		{
            // Initialize
            queryOptions = null;
            QueryOptions qo = new QueryOptions();

            // Transfer model
            foreach (SortColumnViewModel sortColumnViewModel in queryOptionsViewModel.SortColumns.Columns)
            {
                SortColumn sortColumn = new SortColumn();
                sortColumn.Name = sortColumnViewModel.Name;
                sortColumn.Direction = sortColumnViewModel.Direction == SortDirectionViewModel.DESC ? SortDirection.DESC : SortDirection.ASC;
                qo.SortColumns.Columns.Add(sortColumn);
            }
            BufferMgr.TransferBuffer(queryOptionsViewModel.Paging, qo.Paging);
            qo.SearchOptions.SearchString = queryOptionsViewModel.SearchOptions.SearchString;
            foreach (SearchFieldViewModel searchFieldViewModel in queryOptionsViewModel.SearchOptions.SearchFieldList)
            {
                SearchField searchField = new SearchField();
                searchField.Name = searchFieldViewModel.Name;
                searchField.Value = searchFieldViewModel.Value;
                searchField.SearchOperation = convertSearchOperationTypes(searchFieldViewModel.SearchOperation);
                searchField.Type = convertSearchFieldDataTypes(searchFieldViewModel.Type);
                qo.SearchOptions.SearchFieldList.Add(searchField);
            }

            // Return model
            queryOptions = qo;

            return (new questStatus(Severity.Success));
		}		   
		public questStatus TransferQueryOptions(QueryOptions queryOptions, out QueryOptionsViewModel queryOptionsViewModel)
		{
            // Initialize
            queryOptionsViewModel = null;

            // Initialize
            QueryOptionsViewModel qo = new QueryOptionsViewModel();

            // Transfer model
            foreach (SortColumn sortColumn in queryOptions.SortColumns.Columns)
            {
                SortColumnViewModel sortColumnViewModel = new SortColumnViewModel();
                sortColumnViewModel.Name = sortColumn.Name;
                sortColumnViewModel.Direction = sortColumn.Direction == SortDirection.DESC ? "DESC" : "ASC";
                qo.SortColumns.Columns.Add(sortColumnViewModel);
            }
            BufferMgr.TransferBuffer(queryOptions.Paging, qo.Paging);
            qo.SearchOptions.SearchString = queryOptions.SearchOptions.SearchString;
            foreach (SearchField searchField in queryOptions.SearchOptions.SearchFieldList)
            {
                SearchFieldViewModel searchFieldViewModel = new SearchFieldViewModel();
                searchFieldViewModel.Name = searchField.Name;
                searchFieldViewModel.Value = searchField.Value;
                searchFieldViewModel.SearchOperation = convertSearchOperationTypes(searchField.SearchOperation);
                searchFieldViewModel.Type = convertSearchFieldDataTypes(searchField.Type);
                qo.SearchOptions.SearchFieldList.Add(searchFieldViewModel);
            }

            // Return model
            queryOptionsViewModel = qo;


            return (new questStatus(Severity.Success));
        }
        public questStatus TransferQueryResponse(QueryResponse queryResponse, out QueryResponseViewModel queryResponseViewModel)
        {
            // Initialize
            queryResponseViewModel = new QueryResponseViewModel();


            // Transfer model.
            BufferMgr.TransferBuffer(queryResponse, queryResponseViewModel);

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
        private SearchOperation convertSearchOperationTypes(SearchOperationViewModel searchOperationViewModel)
        {
            SearchOperation so = SearchOperation.Equal;
            switch (searchOperationViewModel.Value)
            {
                case SearchOperationViewModel.None: so = SearchOperation.None; break;
                case SearchOperationViewModel.Equal: so = SearchOperation.Equal; break;
                case SearchOperationViewModel.NotEquals: so = SearchOperation.NotEquals; break;
                case SearchOperationViewModel.LessThan: so = SearchOperation.LessThan; break;
                case SearchOperationViewModel.LessThanOrEqual: so = SearchOperation.LessThanOrEqual; break;
                case SearchOperationViewModel.GreaterThanOrEqual: so = SearchOperation.GreaterThanOrEqual; break;
                case SearchOperationViewModel.BeginsWith: so = SearchOperation.BeginsWith; break;
                case SearchOperationViewModel.DoesNotBeginWith: so = SearchOperation.DoesNotBeginWith; break;
                case SearchOperationViewModel.Contains: so = SearchOperation.Contains; break;
                case SearchOperationViewModel.DoesNotContain: so = SearchOperation.DoesNotContain; break;
                case SearchOperationViewModel.EndsWith: so = SearchOperation.EndsWith; break;
                case SearchOperationViewModel.DoesNotEndWith: so = SearchOperation.DoesNotEndWith; break;
                case SearchOperationViewModel.IsNull: so = SearchOperation.IsNull; break;
                case SearchOperationViewModel.IsNotNull: so = SearchOperation.IsNotNull; break;
                case SearchOperationViewModel.DateOnly: so = SearchOperation.DateOnly; break;
                case SearchOperationViewModel.DateAndTime: so = SearchOperation.DateAndTime; break;
                default: so = SearchOperation.Contains;
                    break;
            }
            return so;
        }
        private SearchOperationViewModel convertSearchOperationTypes(SearchOperation searchOperation)
        {
            SearchOperationViewModel soVM = new SearchOperationViewModel(SearchOperationViewModel.Contains);
            switch (searchOperation)
            {
                case SearchOperation.None: soVM = new SearchOperationViewModel(SearchOperationViewModel.None); break;
                case SearchOperation.Equal: soVM = new SearchOperationViewModel(SearchOperationViewModel.Equal); break;
                case SearchOperation.NotEquals: soVM = new SearchOperationViewModel(SearchOperationViewModel.NotEquals); break;
                case SearchOperation.LessThan: soVM = new SearchOperationViewModel(SearchOperationViewModel.LessThan); break;
                case SearchOperation.LessThanOrEqual: soVM = new SearchOperationViewModel(SearchOperationViewModel.LessThanOrEqual); break;
                case SearchOperation.GreaterThanOrEqual: soVM = new SearchOperationViewModel(SearchOperationViewModel.GreaterThanOrEqual); break;
                case SearchOperation.BeginsWith: soVM = new SearchOperationViewModel(SearchOperationViewModel.BeginsWith); break;
                case SearchOperation.DoesNotBeginWith: soVM = new SearchOperationViewModel(SearchOperationViewModel.DoesNotBeginWith); break;
                case SearchOperation.Contains: soVM = new SearchOperationViewModel(SearchOperationViewModel.Contains); break;
                case SearchOperation.DoesNotContain: soVM = new SearchOperationViewModel(SearchOperationViewModel.DoesNotContain); break;
                case SearchOperation.EndsWith: soVM = new SearchOperationViewModel(SearchOperationViewModel.EndsWith); break;
                case SearchOperation.DoesNotEndWith: soVM = new SearchOperationViewModel(SearchOperationViewModel.DoesNotEndWith); break;
                case SearchOperation.IsNull: soVM = new SearchOperationViewModel(SearchOperationViewModel.IsNull); break;
                case SearchOperation.IsNotNull: soVM = new SearchOperationViewModel(SearchOperationViewModel.IsNotNull); break;
                case SearchOperation.DateOnly: soVM = new SearchOperationViewModel(SearchOperationViewModel.DateOnly); break;
                case SearchOperation.DateAndTime: soVM = new SearchOperationViewModel(SearchOperationViewModel.DateAndTime); break;
                default: soVM = new SearchOperationViewModel(SearchOperationViewModel.Contains);
                    break;            
            }
            return soVM;
        }
        private string convertSearchFieldDataTypes(Type type)
        {
            string _t = null;
            switch (type.ToString())
            {
                case "System.String": _t = "string"; break;
                case "System.DateTime": _t = "date"; break;
                case "System.Boolean": _t = "bool"; break;
                default: _t = "String"; break;
            }
            return _t;
        }
        private Type convertSearchFieldDataTypes(string type)
        {
            Type _t = null;
            switch (type.ToString().ToLower())
            {
                case "string": _t = typeof(System.String); 
                    break;
                case "date":
                case "datetime":
                    _t = typeof(System.DateTime); 
                    break;
                case "bool":
                case "boolean":
                    _t = typeof(System.Boolean); 
                    break;
                default: _t = typeof(System.String); 
                    break;
            }
            return _t;
        }
        #endregion
    }

}