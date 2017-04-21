using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Quest.MPDW.Controllers;


namespace Quest.MPDW.Controllers
{
    public class ErrorController : BaseController
    {
        public ActionResult Index()
        {

            return View("~/Views/Shared/Error.cshtml");
        }

    }
}
