using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Dynamic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;


namespace Quest.MPDW.Modelers
{
    public class ExpandoJsonResult : JsonResult
    {
        public override void ExecuteResult(ControllerContext context)
        {
            HttpResponseBase response = context.HttpContext.Response;
            response.ContentType = !string.IsNullOrEmpty(ContentType) ? ContentType : "application/json";
            response.ContentEncoding = ContentEncoding ?? response.ContentEncoding;

            if (Data != null)
            {
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                serializer.RegisterConverters(new JavaScriptConverter[] { new ExpandoConverter() });
                response.Write(serializer.Serialize(Data));
            }
        }
    }
}