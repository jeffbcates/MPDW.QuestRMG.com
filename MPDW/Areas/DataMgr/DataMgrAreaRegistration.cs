using System.Web.Mvc;

namespace MPDW.Areas.DataMgr
{
    public class DataMgrAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "DataMgr";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "DataMgr_default",
                "DataMgr/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "Quest.MasterPricing.DataMgr" }
            );
        }
    }
}
