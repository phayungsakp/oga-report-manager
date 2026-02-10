using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;

namespace WebReportHightJump.Addon.WebClientPrint
{
    public class DownloadFile : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            string fileName = "wcpp-5.0.20.127.msi";
            System.Web.HttpResponse response = System.Web.HttpContext.Current.Response;
            response.ClearContent();
            response.Clear();
            response.ContentType = "text/plain";
            response.AddHeader("Content-Disposition",
                               "attachment; filename=" + fileName + ";");
            response.TransmitFile(context.Server.MapPath("~/Addon/WebClientPrint/" + fileName));
            response.Flush();
            response.End();
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}
