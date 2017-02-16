using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Quest.Util.Data;


namespace Quest.Functional.FMS
{
    public class Requisition
    {
        public int Id { get; set; }
        public int SectorId { get; set; }
        public int AuthorId { get; set; }
        public int RequisitionTypeId { get; set; }
        public int RequisitionStateId { get; set; }
        public string RequisitionStateName { get; set; }
        public string Name { get; set; }
        public string Tag { get; set; }
        public string Description { get; set; }
        public string State { get; set; }
        public string City { get; set; }
        public string ZipCode { get; set; }
        public int? ZipDistance { get; set; }
        public DateTime? StartDate { get; set; }
        public bool bStartImmediately { get; set; }
        public DateTime? EndDate { get; set; }
        public bool bPreferFramework { get; set; }
        public bool bPreferSolutionShop { get; set; }
        public decimal? HourlyRateMin { get; set; }
        public decimal? HourlyRateMax { get; set; }
        public int? BudgetId { get; set; }
        public decimal? TargetBudget { get; set; }
        public decimal? ProfileRating { get; set; }
        public int VisibilityScopeId { get; set; }
        public int Revision { get; set; }
        public bool bAcceptApplications { get; set; }
        public bool bAcceptBids { get; set; }
        public DateTime Created { get; set; }
        public string CreatedTimezone { get; set; }
    }
}
