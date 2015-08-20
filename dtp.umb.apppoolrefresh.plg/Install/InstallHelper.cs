using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Configuration;
using System.Web.Hosting;
using System.Xml;

namespace dtp.umb.apppoolrefresh.plg.Install
{
    public class InstallHelper
    {
        private const string APPKEY_TOUCHEDON = "AppPoolRefresh:TouchedOn";

        public void AddAppSetting()
        {
            var configFile = WebConfigurationManager.OpenWebConfiguration("/");
            var settings = configFile.AppSettings.Settings;
            if (settings[APPKEY_TOUCHEDON] == null)
            {
                settings.Add(APPKEY_TOUCHEDON, String.Empty);
                configFile.Save(ConfigurationSaveMode.Modified);
                ConfigurationManager.RefreshSection(configFile.AppSettings.SectionInformation.Name);
            }

        }

        public void RemoveAppSetting()
        {
            var configFile = WebConfigurationManager.OpenWebConfiguration("/");
            var settings = configFile.AppSettings.Settings;
            if (settings[APPKEY_TOUCHEDON] != null)
            {
                settings.Remove(APPKEY_TOUCHEDON);
                configFile.Save(ConfigurationSaveMode.Modified);
                ConfigurationManager.RefreshSection(configFile.AppSettings.SectionInformation.Name);
            }
        }

        public void AddSectionDashboard()
        {
            string virtualPath = "~/config/dashboard.config";
            string filename = HostingEnvironment.MapPath(virtualPath);
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.Load(filename);
            if (xmlDocument.SelectSingleNode("//section [@alias='AppPoolRefreshDeveloperDashboardSection']") == null)
            {
                // XML for adding Dashboard
                //<section alias="AppPoolRefreshDeveloperDashboardSection">
                //  <areas>
                //    <area>developer</area>
                //  </areas>
                //  <tab caption="App Pool Refresh">
                //    <control showOnce="false" addPanel="true" panelCaption="">
                //        /app_plugins/apppoolrefresh/views/dashboard.html
                //    </control>
                //  </tab>
                //</section>  

                string xml = "<section alias=\"AppPoolRefreshDeveloperDashboardSection\"><areas><area>developer</area></areas><tab caption=\"App Pool Refresh\"><control showOnce=\"false\" addPanel=\"true\" panelCaption=\"\">/app_plugins/apppoolrefresh/views/dashboard.html</control></tab></section>";
                XmlNode xmlNode = xmlDocument.SelectSingleNode("//dashBoard");
                if (xmlNode != null)
                {
                    XmlDocument xmlDocument2 = new XmlDocument();
                    xmlDocument2.LoadXml(xml);
                    XmlNode node = xmlDocument2.SelectSingleNode("*");
                    xmlNode.AppendChild(xmlNode.OwnerDocument.ImportNode(node, true));
                    xmlDocument.Save(filename);
                }
            }
        }

        public void RemoveSectionDashboard()
        {
            string virtualPath = "~/config/dashboard.config";
            string filename = HostingEnvironment.MapPath(virtualPath);
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.Load(filename);
            XmlNode xmlNode = xmlDocument.SelectSingleNode("//dashboard");
            if (xmlNode != null)
            {
                XmlNode xmlNode2 = xmlNode.SelectSingleNode("./section [@alias='AppPoolRefreshDeveloperDashboardSection']");
                if (xmlNode2 != null)
                {
                    xmlNode.RemoveChild(xmlNode2);
                    xmlDocument.Save(filename);
                }
            }
        }
    }
}

