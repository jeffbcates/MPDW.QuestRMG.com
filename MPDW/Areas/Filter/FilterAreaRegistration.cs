using System.Web.Mvc;

namespace MPDW.Areas.Filter
{
    public class FilterAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Filter";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Filter_default",
                "Filter/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "Quest.MasterPricing.Filter" }
            );
        }
    }
}
