using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Web;
using System.Configuration;
using System.Drawing.Printing;

namespace WebReportHightJump
{
    public partial class CrystalReport : System.Web.UI.Page
    {
        ReportDocument rpt = new ReportDocument();
        protected void Page_Load(object sender, EventArgs e)
        {           
                try
                {
                    SiteMaster master = (SiteMaster)this.Master;
                    //CrystalReportSource crs = new CrystalDecisions.Web.CrystalReportSource();
                    //crs.Report.FileName = Server.MapPath("CrystalReports/Test.rpt");
                    lbError.Text = string.Empty;


                    //System.Drawing.Printing.PrintDocument printDocument = new System.Drawing.Printing.PrintDocument();
                    //if (printDocument.PrinterSettings.PrinterName != "") {
                    //    this.LabelPrinterName.Text = printDocument.PrinterSettings.PrinterName;
                    //}

                    //PrinterSettings settings = new PrinterSettings();
                    //foreach (string printer in PrinterSettings.InstalledPrinters) {
                    //    settings.PrinterName = printer;
                    //    if (settings.IsDefaultPrinter)
                    //        this.LabelPrinterName.Text = printer;
                    //}


                    //ReportDocument rpt = new ReportDocument();
                    string user = string.Empty;
                    string pass = string.Empty;
                   
                    string query =  Server.UrlDecode(Request.QueryString.ToString()); //Request.QueryString.ToString();
                    //string query = Request.QueryString.ToString();
                              
                    string[] arr = query.Split('?');

                    // set Report Title
                    if (Request.QueryString["_app_Reporttitle"] != null)
                    {
                        master.MasterPageLabelHeader = Server.UrlDecode(Request.QueryString["_app_Reporttitle"].ToString());
                        //lbError.Text = Server.UrlDecode(Request.QueryString["_app_Reporttitle"].ToString());                        
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
                    this.CrystalReportViewer1.ReportSource = rpt;
                    this.CrystalReportViewer1.Zoom(Convert.ToInt32(ClassConfig.Instance.getZoomDefault()));
                    //this.CrystalReportViewer1.ReportSource = crs;

                    try {
                        //rpt.PrintOptions.PrinterName = "Microsoft XPS Document Writer";//"RICOH MP C2503 PCL 6";
                        rpt.PrintOptions.PrinterName = "RICOH MP C2503 PCL 6";
                        rpt.PrintToPrinter(2, false, 1, 1);
                        lbError.ForeColor = System.Drawing.Color.Green;
                        lbError.Text = "Print complete at s " + rpt.PrintOptions.PrinterName;
                    } catch (Exception ex) {
                        lbError.ForeColor = System.Drawing.Color.Red;
                        lbError.Text = ex.Message.ToString() + (ex.InnerException != null ? ex.InnerException.ToString() : "");
                        this.CrystalReportViewer1.ReportSource = null;
                    }   
                }
                catch (Exception ex)
                {
                    lbError.ForeColor = System.Drawing.Color.Red;
                    lbError.Text = ex.Message.ToString();
                    this.CrystalReportViewer1.ReportSource = null;
                }
       
        }

        protected void ButtonPrint_Click(object sender, EventArgs e) {
            //try {
            //    //rpt.PrintOptions.PrinterName = "Microsoft XPS Document Writer";//"RICOH MP C2503 PCL 6";
            //    rpt.PrintOptions.PrinterName = "RICOH MP C2503 PCL 6";
            //    rpt.PrintToPrinter(2, false, 1, 1);
            //    lbError.ForeColor = System.Drawing.Color.Green;
            //    lbError.Text = "Print complete at " + rpt.PrintOptions.PrinterName;
            //} catch (Exception ex) {                
            //    lbError.ForeColor = System.Drawing.Color.Red;
            //    lbError.Text = ex.Message.ToString() + (ex.InnerException != null ? ex.InnerException.ToString() : "");
            //    this.CrystalReportViewer1.ReportSource = null;
            //}            
        }

    
    }
}