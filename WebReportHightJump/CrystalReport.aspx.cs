using System;
using System.Web.UI;
using CrystalDecisions.CrystalReports.Engine;

namespace WebReportHightJump
{
    public partial class CrystalReport : Page
    {
        ReportDocument rpt = new ReportDocument();
        protected void Page_Init(object sender, EventArgs e)
        {
            CrystalReportViewer1.Navigate += CrystalReportViewer1_Navigate;
            if (!IsPostBack)
            {
                try
                {
                    SiteMaster master = (SiteMaster)this.Master;
                    lbError.Text = string.Empty;
                    string user = string.Empty;
                    string pass = string.Empty;
                    string query = Server.UrlDecode(Request.QueryString.ToString()); //Request.QueryString.ToString();
                                                                                     //string query = Request.QueryString.ToString();

                    string[] arr = query.Split('?');

                    // set Report Title
                    if (Request.QueryString["_app_Reporttitle"] != null)
                    {
                        master.MasterPageLabelHeader = Server.UrlDecode(Request.QueryString["_app_Reporttitle"].ToString());
                    }

                    //set Report Path
                    string pathReport = ClassConfig.Instance.GetMapDriveReportPath();
                    if (Request.QueryString["_app_reportpath"] != null)
                    {
                        rpt.Load(Server.MapPath("CrystalReports/" + Request.QueryString["_app_reportpath"].ToString()));
                    }
                    else
                    {
                        lbError.Text = "No Report Path";
                        return;
                    }

                    // set Parameter to Report
                    string[] a = arr[0].Split('&');
                    for (int i = 0; i < a.Length; i++)
                    {
                        if (i >= 4)
                        {
                            rpt.SetParameterValue(a[i].Split('=')[0].ToString(), a[i].Split('=')[1].ToString());
                        }
                    }

                    //rpt.PrintOptions.NoPrinter = false;

                    //set Database User Login
                    //if (Request.QueryString["_app_config"] != null && !string.IsNullOrEmpty(Request.QueryString["_app_config"])) {
                    //    string scon = Request.QueryString["_app_config"].ToString().Trim();
                    //    scon = scon.Replace("[*CHAP*]", "#");
                    //    //rpt.SetDatabaseLogon(scon.Split('|')[2], scon.Split('|')[3], scon.Split('|')[0], scon.Split('|')[1]);
                    //    string pPass = scon.Split('|')[3].ToString().Trim().Replace("[*CHAP*]", "#");

                    //    //อ่านค่า connection string จาก url
                    //    rpt.DataSourceConnections[0].SetConnection(scon.Split('|')[0], scon.Split('|')[1], scon.Split('|')[2], scon.Split('|')[3]);
                    //} else {
                    //    //อ่านค่า connection string จาก file config ของ web                   
                    //    string pUser = ClassConfig.Instance.getUser();
                    //    string pPasss = ClassConfig.Instance.getPass();
                    //    string pServer = ClassConfig.Instance.getServer();
                    //    string pDatabase = ClassConfig.Instance.getDatabase();
                    //    rpt.DataSourceConnections[0].SetConnection(pServer, pDatabase, pUser, pPasss);
                    //}
                    string pUser = ClassConfig.Instance.getUser();
                    string pPasss = ClassConfig.Instance.getPass();
                    string pServer = ClassConfig.Instance.getServer();
                    string pDatabase = ClassConfig.Instance.getDatabase();
                    rpt.DataSourceConnections[0].SetConnection(pServer, pDatabase, pUser, pPasss);


                    //rpt.PrintOptions.PaperSize = CrystalDecisions.Shared.PaperSize.PaperA4;
                    

                    this.CrystalReportViewer1.ReportSource = rpt;
                    this.CrystalReportViewer1.Zoom(Convert.ToInt32(ClassConfig.Instance.getZoomDefault()));
                    //this.CrystalReportViewer1.ReportSource = crs;

                    //try {
                    //    //rpt.PrintOptions.PrinterName = "Microsoft XPS Document Writer";//"RICOH MP C2503 PCL 6";
                    //    rpt.PrintOptions.PrinterName = "RICOH MP C2503 PCL 6";
                    //    rpt.PrintToPrinter(2, false, 1, 1);
                    //    lbError.ForeColor = System.Drawing.Color.Green;
                    //    lbError.Text = "Print complete at s " + rpt.PrintOptions.PrinterName;
                    //} catch (Exception ex) {
                    //    lbError.ForeColor = System.Drawing.Color.Red;
                    //    lbError.Text = ex.Message.ToString() + (ex.InnerException != null ? ex.InnerException.ToString() : "");
                    //    this.CrystalReportViewer1.ReportSource = null;
                    //}   
                }
                catch (Exception ex)
                {
                    lbError.ForeColor = System.Drawing.Color.Red;
                    lbError.Text = ex.Message.ToString();
                    this.CrystalReportViewer1.ReportSource = null;
                }
                finally
                {
                    if (lbError.Text == "")
                        alert_box.Visible = false;
                }
                Session["ReportDocument"] = rpt;
            }
            else
            {
                ReportDocument doc = (ReportDocument)Session["ReportDocument"];
                this.CrystalReportViewer1.ReportSource = doc;
            }
        }

        private void CrystalReportViewer1_Navigate(object source, CrystalDecisions.Web.NavigateEventArgs e)
        {
            try
            {
                string query = Server.UrlDecode(Request.QueryString.ToString()); //Request.QueryString.ToString();
                                                                                 //string query = Request.QueryString.ToString();

                string[] arr = query.Split('?');

                // set Parameter to Report
                string[] a = arr[0].Split('&');
                for (int i = 0; i < a.Length; i++)
                {
                    if (i >= 4)
                    {
                        rpt.SetParameterValue(a[i].Split('=')[0].ToString(), a[i].Split('=')[1].ToString());
                    }
                }

                string pUser = ClassConfig.Instance.getUser();
                string pPass = ClassConfig.Instance.getPass();
                string pServer = ClassConfig.Instance.getServer();
                string pDatabase = ClassConfig.Instance.getDatabase();
                rpt.DataSourceConnections[0].SetConnection(pServer, pDatabase, pUser, pPass);
                 
                //rpt.PrintOptions.PaperSize = CrystalDecisions.Shared.PaperSize.PaperA4;
                 
                this.CrystalReportViewer1.ReportSource = rpt;
                this.CrystalReportViewer1.Zoom(Convert.ToInt32(ClassConfig.Instance.getZoomDefault()));
            }
            catch //(Exception)
            {

                //throw;
            }
        }

        protected void lbtnPrint_Click(object sender, EventArgs e)
        {
            //Export PDF
            ReportDocument cryRpt = new ReportDocument();
            cryRpt.Load(""); // PUT CRYSTAL REPORT PATH HERE\\CrystalReport1.rpt");
            CrystalReportViewer1.ReportSource = cryRpt;
            CrystalReportViewer1.RefreshReport();

            CrystalDecisions.Shared.ExportOptions CrExportOptions;
            CrystalDecisions.Shared.DiskFileDestinationOptions CrDiskFileDestinationOptions = new CrystalDecisions.Shared.DiskFileDestinationOptions();
            CrystalDecisions.Shared.PdfRtfWordFormatOptions CrFormatTypeOptions = new CrystalDecisions.Shared.PdfRtfWordFormatOptions();
            CrDiskFileDestinationOptions.DiskFileName = "c:\\csharp.net-informations.pdf";
            CrExportOptions = cryRpt.ExportOptions;
            {
                CrExportOptions.ExportDestinationType = CrystalDecisions.Shared.ExportDestinationType.DiskFile;
                CrExportOptions.ExportFormatType = CrystalDecisions.Shared.ExportFormatType.PortableDocFormat;
                CrExportOptions.DestinationOptions = CrDiskFileDestinationOptions;
                CrExportOptions.FormatOptions = CrFormatTypeOptions;
            }
            cryRpt.Export();
        }

        protected void Export(ReportDocument rptDoc, string Format)
        {
            CrystalDecisions.Shared.ExportFormatType formatType = CrystalDecisions.Shared.ExportFormatType.NoFormat;
            switch (Format)
            {
                case "Word":
                    formatType = CrystalDecisions.Shared.ExportFormatType.WordForWindows;
                    break;
                case "PDF":
                    formatType = CrystalDecisions.Shared.ExportFormatType.PortableDocFormat;
                    break;
                case "Excel":
                    formatType = CrystalDecisions.Shared.ExportFormatType.Excel;
                    break;
                case "CSV":
                    formatType = CrystalDecisions.Shared.ExportFormatType.CharacterSeparatedValues;
                    break;
            }
            rptDoc.ExportToHttpResponse(formatType, Response, true, "Crystal");
            Response.End();
        }


        protected void btInstall_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Addon/WebClientPrint/DownloadFile.ashx");
        }
    }
}