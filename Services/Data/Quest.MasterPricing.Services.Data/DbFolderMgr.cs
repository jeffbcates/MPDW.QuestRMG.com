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
using Quest.MasterPricing.Services.Data.Database;
using Quest.Services.Dbio.MasterPricing;


namespace Quest.MasterPricing.Services.Data.Filters
{
    public class DbFolderMgr : DbMgr
    {
        #region Declarations
        /*==================================================================================================================================
         * Declarations
         *=================================================================================================================================*/
        private UserSession _userSession = null;

        #endregion


        #region Constructors
        /*==================================================================================================================================
         * Constructors
         *=================================================================================================================================*/
        public DbFolderMgr()
            : base()
        {
            initialize();
        }
        public DbFolderMgr(UserSession userSession)
            : base()
        {
            _userSession = userSession;
            initialize();
        }
        #endregion


        #region Properties
        /*==================================================================================================================================
         * Properties
         *=================================================================================================================================*/
        public UserSession UserSession
        {
            get
            {
                return (this._userSession);
            }
        }
        #endregion


        #region Public Methods
        /*==================================================================================================================================
         * Public Methods
         *=================================================================================================================================*/


        #region Filters
        //
        // Filters
        //
        public questStatus DeleteFilter(FilterId filterId)
        {
            using (MasterPricingEntities dbContext = new MasterPricingEntities())
            {
                int numRows = dbContext.DeleteFilterById(filterId.Id);
                if (numRows < 1)
                {
                    return (new questStatus(Severity.Error, String.Format("Filter {0} was not deleted", filterId.Id)));
                }
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
            // Initialize
            questStatus status = null;
            try
            {
            }
            catch (System.Exception ex)
            {
                status = new questStatus(Severity.Fatal, String.Format("EXCEPTION: {0}.{1}: {2}",
                        this.GetType().ToString(), MethodInfo.GetCurrentMethod().Name, ex.Message));
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        private string bracketIdentifier(string identifier)
        {
            if (identifier.StartsWith("[") && identifier.EndsWith("]"))
            {
                return (identifier);
            }
            string _identifier = "[" + identifier + "]";
            return (_identifier);
        }
        private string quoteValueList(List<FilterValue> filterValueList, bool preLike = false, bool postLike = false)
        {
            StringBuilder sbValueList = new StringBuilder();
            foreach (FilterValue filterValue in filterValueList)
            {
                sbValueList.Append("'" + (preLike ? "%" : "") + filterValue.Value + (postLike ? "%" : "") + "'");
            }
            return (sbValueList.ToString());
        }
        private questStatus getJoinTargetColumnId(Filter filter, FilterItemJoin filterItemJoin)
        {
            if (filterItemJoin.TargetEntityTypeId == FilterEntityType.Table)
            {
                // Get the TablesetColumnId for the given join.
                FilterTable filterTable = filter.FilterTableList.Find(delegate (FilterTable t)
                {
                    return (filterItemJoin.TargetSchema == t.Schema && filterItemJoin.TargetEntityName == t.Name);
                });
                if (filterTable == null)
                {
                    return (new questStatus(String.Format("ERROR: seeking FilterItemJoin {0} TablesetColumnId not found (FilterTable) [{1}].[{2}]",
                        filterItemJoin.Id, filterItemJoin.TargetSchema, filterItemJoin.TargetEntityName)));
                }

                // Now get the TablesetColumnId for the given column name
                FilterColumn filterColumn = filterTable.FilterColumnList.Find(delegate (FilterColumn fc)
                {
                    return (filterItemJoin.TargetColumnName == fc.TablesetColumn.Name);
                });
                if (filterColumn == null)
                {
                    return (new questStatus(String.Format("ERROR: seeking FilterItemJoin {0} FilterColumn not found (FilterTable) [{1}].[{2}]",
                        filterItemJoin.Id, filterItemJoin.TargetSchema, filterItemJoin.TargetEntityName)));
                }
                filterItemJoin.ColumnId = filterColumn.TablesetColumn.Id;
            }
            else if (filterItemJoin.TargetEntityTypeId == FilterEntityType.View)
            {
                // Get the TablesetColumnId for the given join.
                FilterView filterView = filter.FilterViewList.Find(delegate (FilterView v)
                {
                    return (filterItemJoin.TargetSchema == v.Schema && filterItemJoin.TargetEntityName == v.Name);
                });
                if (filterView == null)
                {
                    return (new questStatus(String.Format("ERROR: seeking FilterItemJoin {0} TablesetColumnId not found (FilterView) [{1}].[{2}]",
                        filterItemJoin.Id, filterItemJoin.TargetSchema, filterItemJoin.TargetEntityName)));
                }

                // Now get the TablesetColumnId for the given column name
                FilterColumn filterColumn = filterView.FilterColumnList.Find(delegate (FilterColumn fc)
                {
                    return (filterItemJoin.TargetColumnName == fc.TablesetColumn.Name);
                });
                if (filterColumn == null)
                {
                    return (new questStatus(String.Format("ERROR: seeking FilterItemJoin {0} FilterColumn not found (FilterView) [{1}].[{2}]",
                        filterItemJoin.Id, filterItemJoin.TargetSchema, filterItemJoin.TargetEntityName)));
                }
                filterItemJoin.ColumnId = filterColumn.TablesetColumn.Id;
            }
            return (new questStatus(Severity.Success));
        }


        #region Hide this, embarrassing
        //
        // Hide this, embarrassing
        //
        // Indemenity clause: database and tableset changes independent of filters hath wreaked havoc in places.
        private questStatus klugieGetFilterItemInfo(FilterItemJoin filterItemJoin, out DatabaseId databaseId, out TablesetId tablesetId)
        {
            // Initialize 
            questStatus status = null;
            databaseId = null;
            tablesetId = null;

            FilterItemId filterItemId = new FilterItemId(filterItemJoin.FilterItemId);
            FilterItem filterItem = null;
            DbFilterItemsMgr dbFilterItemsMgr = new DbFilterItemsMgr(this.UserSession);

            status = dbFilterItemsMgr.Read(filterItemId, out filterItem);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (klugieGetFilterItemInfo(filterItem, out databaseId, out tablesetId));
        }
        private questStatus klugieGetFilterItemInfo(FilterItem filterItem, out DatabaseId databaseId, out TablesetId tablesetId)
        {
            // Initialize 
            questStatus status = null;
            databaseId = null;
            tablesetId = null;


            // Klugie: temporary
            // Just back up and get stuff we need.  (All this due to refactoring, more to do).

            // Get the filter
            FilterId filterId = new FilterId(filterItem.FilterId);
            Filter filter = null;
            DbFiltersMgr dbFiltersMgr = new DbFiltersMgr(this.UserSession);
            status = dbFiltersMgr.Read(filterId, out filter);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }

            // Get the tableset
            DbTablesetsMgr dbTablesetsMgr = new DbTablesetsMgr(this.UserSession);
            tablesetId = new TablesetId(filter.TablesetId);
            Tableset tableset = null;
            status = dbTablesetsMgr.Read(tablesetId, out tableset);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }

            // Return info.
            databaseId = new DatabaseId(tableset.DatabaseId);
            tablesetId = new TablesetId(tableset.Id);
            
            return (new questStatus(Severity.Success));
        }
        #endregion

        #endregion
    }
}
