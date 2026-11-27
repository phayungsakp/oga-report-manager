
using System;
using System.Collections.Specialized;
using System.Text;
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
                NameValueCollection printOptions = context.Request.QueryString;
                NameValueCollection reportParameters = context.Request.QueryString;
                string payload = printOptions["printPayload"];
                bool legacyRequest = payload == null;
                if (!legacyRequest)
                {
                    try
                    {
                        printOptions = DecodePrintPayload(payload);
                    }
                    catch (FormatException)
                    {
                        BadPrintRequest(context.Response, "Invalid print payload.");
                        return;
                    }

                    reportParameters = HttpUtility.ParseQueryString(printOptions["reportQuery"] ?? string.Empty);
                }

                string strRptName = reportParameters["_app_reportpath"];
                if (string.IsNullOrWhiteSpace(strRptName))
                {
                    BadPrintRequest(context.Response, "Missing report path.");
                    return;
                }

                string printerName = ReadPrintOption(printOptions, "printerName", legacyRequest);
                string trayName = ReadPrintOption(printOptions, "trayName", legacyRequest);
                string paperName = ReadPrintOption(printOptions, "paperName", legacyRequest);
                string pagesRange = ReadPrintOption(printOptions, "pagesRange", legacyRequest);

                PrintRotation printRotation;
                if (!Enum.TryParse(ReadPrintOption(printOptions, "printRotation", legacyRequest), out printRotation))
                {
                    BadPrintRequest(context.Response, "Invalid print rotation.");
                    return;
                }

                bool printAnnotations = (printOptions["printAnnotations"] == "true");
                bool printAsGrayscale = (printOptions["printAsGrayscale"] == "true");
                bool printInReverseOrder = (printOptions["printInReverseOrder"] == "true");
                bool printAutoRotate = (printOptions["printAutoRotate"] == "true");
                int printCopies;
                if (!int.TryParse(ReadPrintOption(printOptions, "printCopies", legacyRequest), out printCopies))
                    printCopies = 1;


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
                crConnectionInfo.LogonProperties.Set("Provider", "MSOLEDBSQL");
                crConnectionInfo.LogonProperties.Set("Encrypt", "False");
                crConnectionInfo.LogonProperties.Set("Trust Server Certificate", "True");
                CrTables = crReportDocument.Database.Tables;

                foreach (Table CrTable in CrTables)
                {
                    crtableLogoninfo = CrTable.LogOnInfo;
                    crtableLogoninfo.ConnectionInfo = crConnectionInfo;
                    CrTable.ApplyLogOnInfo(crtableLogoninfo);
                }

                CrystalReport.SetReportParameters(crReportDocument, reportParameters);

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
                file.PrintRotation = printRotation;
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



        private static NameValueCollection DecodePrintPayload(string payload)
        {
            string base64 = payload.Replace('-', '+').Replace('_', '/');
            base64 = base64.PadRight(((base64.Length + 3) / 4) * 4, '=');
            string printQuery = Encoding.ASCII.GetString(Convert.FromBase64String(base64));
            return HttpUtility.ParseQueryString(printQuery);
        }

        private static string ReadPrintOption(NameValueCollection options, string key, bool legacyRequest)
        {
            string value = options[key];
            return legacyRequest ? HttpUtility.UrlDecode(value) : value;
        }

        private static void BadPrintRequest(HttpResponse response, string message)
        {
            response.StatusCode = 400;
            response.TrySkipIisCustomErrors = true;
            response.ContentType = "text/plain";
            response.Write(message);
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
