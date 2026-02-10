using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebReportHightJump
{
    public partial class SiteMaster : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                
            }
        }

        public string MasterPageLabelHeader 
        {
            get { return lbReportTitle.Text; }
            set { 
                lbReportTitle.Text = value; 
                
            }
        }
    }
}
