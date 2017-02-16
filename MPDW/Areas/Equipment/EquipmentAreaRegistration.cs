using System.Web.Mvc;

namespace MPDW.Areas.Equipment
{
    public class EquipmentAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Equipment";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Equipment_default",
                "Equipment/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "Quest.MPDW.Equipment" }
            );
        }
    }
}
