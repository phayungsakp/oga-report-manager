
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebReportHightJump
{

    public class ClassConfig
    {
        #region +Instance+
        private static ClassConfig _Instance;
        public static ClassConfig Instance
        {
            get
            {
                if (_Instance == null)
                {
                    _Instance = new ClassConfig();
                }
                return _Instance;
            }
        }
        #endregion

        public string GetMapDriveReportPath()
        {
            return System.Configuration.ConfigurationSettings.AppSettings["MapDriveReport"].ToString();
        }
        public string getUser()
        {
            return System.Configuration.ConfigurationSettings.AppSettings["crtUser"].ToString();
        }
        public string getPass()
        {
            return System.Configuration.ConfigurationSettings.AppSettings["crtPass"].ToString();
        }
        public string getServer()
        {
            return System.Configuration.ConfigurationSettings.AppSettings["crtServer"].ToString();
        }
        public string getDatabase()
        {
            return System.Configuration.ConfigurationSettings.AppSettings["crtDatabase"].ToString();
        }
        public string getZoomDefault() {
            return System.Configuration.ConfigurationSettings.AppSettings["crtZoomDefault"].ToString();
        }
    }
}