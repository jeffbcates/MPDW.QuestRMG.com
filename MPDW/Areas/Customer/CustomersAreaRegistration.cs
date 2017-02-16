using System.Web.Mvc;

namespace MPDW.Areas.Customers
{
    public class CustomersAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Customer";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Customers_default",
                "Customer/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "Quest.MPDW.Customer" }
            );
        }
    }
}
