
using System;
using System.Web;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using System.Data;
using System.IO;
using Neodynamic.SDK.Web;
using System.Configuration;
using System.Web.SessionState;

namespace WebReportHightJump.WebPrint
{
    /// <summary>
    /// Summary description for PrintRPTHandler1
    /// </summary>
    public class PrintRPTHandler : IHttpHandler, IRequiresSessionState
    {
        /*############### IMPORTANT!!! ############
           If your website requires AUTHENTICATION, then you MUST configure THIS Handler file
           to be ANONYMOUS access allowed!!!
          ######################################### */
        //private rptReport rpt;
        public void ProcessRequest(HttpContext context)
        {
            if (WebClientPrint.ProcessPrintJob(context.Request.Url.Query))
            {
                string query = context.Server.UrlDecode(context.Request.QueryString.ToString());
                string strRptName = context.Request.QueryString["_app_reportpath"].ToString(); // context.Request["reportName"]; // ConfigurationManager.AppSettings["rptFilePath"] + rpt.rptName;

                bool useDefaultPrinter = (context.Request["useDefaultPrinter"] == "checked");
                string printerName = context.Server.UrlDecode(context.Request["printerName"]);
                string trayName = context.Server.UrlDecode(context.Request["trayName"]);
                string paperName = context.Server.UrlDecode(context.Request["paperName"]);

                string printRotation = context.Server.UrlDecode(context.Request["printRotation"]);
                string pagesRange = context.Server.UrlDecode(context.Request["pagesRange"]);

                bool printAnnotations = (context.Request["printAnnotations"] == "true");
                bool printAsGrayscale = (context.Request["printAsGrayscale"] == "true");
                bool printInReverseOrder = (context.Request["printInReverseOrder"] == "true");
                bool printAutoRotate = (context.Request["printAutoRotate"] == "true");
                int printCopies = 1;
                try
                {
                    printCopies = Convert.ToInt32(context.Server.UrlDecode(context.Request["printCopies"]));
                }
                catch
                {
                    printCopies = 1;
                }


                //load and set report's data source
                //DataSet ds = new DataSet();
                //ds.ReadXml(context.Server.MapPath("~/NorthwindProducts.xml"));

                //create and load rpt in memory
                ReportDocument crReportDocument = new ReportDocument();

                //PageMargins margins;
                //// Get the PageMargins structure and set the 
                //// margins for the report.
                //margins = crReportDocument.PrintOptions.PageMargins;
                //margins.bottomMargin = 0;
                //margins.leftMargin = 0;
                //margins.rightMargin = 0;
                //margins.topMargin = 0;
                //// Apply the page margins.
                //crReportDocument.PrintOptions.ApplyPageMargins(margins);

                //crReportDocument.Load(context.Server.MapPath("~/rptFilePath/bill_receive.rpt"));
                //crReportDocument.Load(context.Server.MapPath(strRptName));
                crReportDocument.Load(context.Server.MapPath("../CrystalReports/" + strRptName));

                //crReportLocal.Load(Server.MapPath(strRptName));
                //crReportLocal.SetDataSource(ds.Tables[0]);

                TableLogOnInfo crtableLogoninfo = new TableLogOnInfo();
                ConnectionInfo crConnectionInfo = new ConnectionInfo();
                Tables CrTables;


                string pUser = ClassConfig.Instance.getUser();
                string pPasss = ClassConfig.Instance.getPass();
                string pServer = ClassConfig.Instance.getServer();
                string pDatabase = ClassConfig.Instance.getDatabase();

                crConnectionInfo.ServerName = pServer; //Database server or ODBC
                crConnectionInfo.DatabaseName = pDatabase; // Database name
                crConnectionInfo.UserID = pUser; // username
                crConnectionInfo.Password = pPasss; // password
                CrTables = crReportDocument.Database.Tables;

                foreach (Table CrTable in CrTables)
                {
                    crtableLogoninfo = CrTable.LogOnInfo;
                    crtableLogoninfo.ConnectionInfo = crConnectionInfo;
                    CrTable.ApplyLogOnInfo(crtableLogoninfo);
                }

                string[] arr = query.Split('?');
                string[] a = arr[0].Split('&');
                for (int i = 0; i < a.Length; i++)
                {
                    if (i >= 16)
                    {
                        crReportDocument.SetParameterValue(a[i].Split('=')[0].ToString(), a[i].Split('=')[1].ToString());
                    }
                }

                //if (context.Request["reportParam"].ToString() != "undefined")
                //{
                //    PropertyCollection paramObjectJson = new System.Web.Script.Serialization.JavaScriptSerializer().Deserialize<PropertyCollection>(context.Request["reportParam"].ToString() ?? context.Request["reportParam"].ToString());
                //    foreach (var param in paramObjectJson.Keys)
                //    {
                //        crReportDocument.SetParameterValue(param.ToString(), paramObjectJson[param]);
                //    }
                //}

                ////Export rpt to a temp PDF and get binary content
                //byte[] pdfContent = null;
                //try
                //{
                //    using (Stream rptStream = crReportDocument.ExportToStream(ExportFormatType.PortableDocFormat))
                //    {
                //        using (MemoryStream ms = new MemoryStream())
                //        {
                //            byte[] buffer = new byte[10000];
                //            int bytesRead = 0;
                //            do
                //            {
                //                bytesRead = rptStream.Read(buffer, 0, buffer.Length);
                //                ms.Write(buffer, 0, bytesRead);
                //            } while (bytesRead > 0);
                //            ms.Position = 0;
                //            pdfContent = ms.ToArray();
                //        }
                //    }
                //}
                //catch (Exception ex)
                //{
                //    throw ex;
                //}

                //create a temp file name for our PDF file...
                string fileName = "temp_" + Guid.NewGuid().ToString("N");
                //string filePath = "~/CrystalReports/mixed-page-orientation.pdf";
                string pdfFilePath = "~/CrystalReports/" + fileName + ".pdf";
                crReportDocument.ExportToDisk(ExportFormatType.PortableDocFormat, context.Server.MapPath(pdfFilePath));

                PrintFilePDF file = new PrintFilePDF(context.Server.MapPath(pdfFilePath), fileName);
                //Create a PrintFilePDF object with the PDF file
                //PrintFilePDF file = new PrintFilePDF(pdfContent, fileName);
                file.PrintRotation = (PrintRotation)Enum.Parse(typeof(PrintRotation), printRotation); ;
                file.PagesRange = pagesRange;
                file.PrintAnnotations = printAnnotations;
                file.PrintAsGrayscale = printAsGrayscale;
                file.PrintInReverseOrder = printInReverseOrder;
                file.AutoRotate = printAutoRotate;
                file.Sizing = Sizing.Fit;
                file.Copies = printCopies;
                //file.AutoCenter = false;

                //file.PrintOrientation = PrintOrientation.Portrait;

                //Set license info...
                WebClientPrint.LicenseOwner = "OGA International Co Ltd - 1 WebApp Lic - 1 WebServer Lic";
                WebClientPrint.LicenseKey = "DE19B2D8963F5B1825CFD643AADF6932B1A4CA74";

                //Create a ClientPrintJob and send it back to the client!
                ClientPrintJob cpj = new ClientPrintJob();

                //set file to print...
                cpj.PrintFile = file;


                //set client printer...
                if (printerName == "null")
                    cpj.ClientPrinter = new DefaultPrinter();
                else
                {
                    if (trayName == "null") trayName = "";
                    if (paperName == "null") paperName = "";

                    cpj.ClientPrinter = new InstalledPrinter(printerName, true, trayName, paperName);
                }

                //cpj.DefaultPageSettings.Landscape = true;

                //send it...            
                context.Response.ContentType = "application/octet-stream";
                context.Response.BinaryWrite(cpj.GetContent());
                if (System.IO.File.Exists(context.Server.MapPath(pdfFilePath)))
                {
                    System.IO.File.Delete(context.Server.MapPath(pdfFilePath));
                }
                context.Response.End();
            }
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