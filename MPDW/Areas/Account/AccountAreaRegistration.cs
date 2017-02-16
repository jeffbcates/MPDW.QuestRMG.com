﻿using System.Web.Mvc;

namespace MPDW.Areas.Account
{
    public class AccountAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Account";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Account_default",
                "Account/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "Quest.MPDW.Account" }
            );
        }
    }
}
