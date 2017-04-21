﻿using System.Web.Mvc;

namespace MPDW.Areas.Database
{
    public class DatabaseAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Database";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Database_default",
                "Database/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "Quest.MasterPricing.Database" }
            );
        }
    }
}
