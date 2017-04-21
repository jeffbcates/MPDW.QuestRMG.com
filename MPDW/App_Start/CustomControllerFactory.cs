using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web;
using System.Web.Routing;


namespace Quest.MPDW.Controllers
{
    public class CustomControllerFactory : DefaultControllerFactory
    {
        public override IController CreateController(RequestContext requestContext, string controllerName)
        {
            IController controller = null;
            try
            {
                controller = base.CreateController(requestContext, controllerName);
            }
            catch (Exception ex)
            {
                string Message = ex.ToString();
            }
            if (controller == null)
            {
                requestContext.RouteData.Values["controller"] = "Error";
                requestContext.RouteData.Values["action"] = "Index";

                controller = base.CreateController(requestContext, "Error");
            }
            return (controller);
        }
    }
}