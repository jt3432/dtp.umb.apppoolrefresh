using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
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
            var webConfigPath = HttpContext.Current.Server.MapPath(@"~/web.config");

            XmlDocument webConfig = new XmlDocument();
            webConfig.Load(webConfigPath);

            // Updating an AppSetting is actually not necessary, simply loading then saving the config file will cause 
            // an App Pool restart. This is just to track that the plug is working and educational purposes.
            XmlNode appPoolRefreshNode = webConfig.SelectSingleNode(String.Format("/configuration/appSettings/add[@key='{0}']", APPKEY_TOUCHEDON));
            if (appPoolRefreshNode == null)
            {
                XmlAttribute keyAttr = webConfig.CreateAttribute("key");
                keyAttr.Value = "AppPoolRefresh:TouchedOn";

                XmlAttribute valueAttr = webConfig.CreateAttribute("value");
                valueAttr.Value = String.Empty;

                appPoolRefreshNode = webConfig.CreateNode("element", "add", "");
                appPoolRefreshNode.Attributes.Append(keyAttr);
                appPoolRefreshNode.Attributes.Append(valueAttr);

                XmlNode appSettingsNode = webConfig.SelectSingleNode("/configuration/appSettings");
                appSettingsNode.AppendChild(appPoolRefreshNode);

                webConfig.Save(webConfigPath);
            }

            // The code below works great other than it will remove any comments that are under the AppSettings node 
            // when the config file is saved.
            //var configFile = WebConfigurationManager.OpenWebConfiguration("/");
            //var settings = configFile.AppSettings.Settings;
            //if (settings[APPKEY_TOUCHEDON] == null)
            //{
            //    settings.Add(APPKEY_TOUCHEDON, String.Empty);
            //    configFile.Save(ConfigurationSaveMode.Modified);
            //    ConfigurationManager.RefreshSection(configFile.AppSettings.SectionInformation.Name);
            //}

        }

        public void RemoveAppSetting()
        {
            var webConfigPath = HttpContext.Current.Server.MapPath(@"~/web.config");

            XmlDocument webConfig = new XmlDocument();
            webConfig.Load(webConfigPath);

            XmlNode appPoolRefreshNode = webConfig.SelectSingleNode(String.Format("/configuration/appSettings/add[@key='{0}']", APPKEY_TOUCHEDON));
            if (appPoolRefreshNode != null)
            {
                XmlNode appSettingsNode = webConfig.SelectSingleNode("/configuration/appSettings");
                appSettingsNode.RemoveChild(appPoolRefreshNode);

                webConfig.Save(webConfigPath);
            }

            // The code below works great other than it will remove any comments that are under the AppSettings node 
            // when the config file is saved.
            //var configFile = WebConfigurationManager.OpenWebConfiguration("/");
            //var settings = configFile.AppSettings.Settings;
            //if (settings[APPKEY_TOUCHEDON] != null)
            //{
            //    settings.Remove(APPKEY_TOUCHEDON);
            //    configFile.Save(ConfigurationSaveMode.Modified);
            //    ConfigurationManager.RefreshSection(configFile.AppSettings.SectionInformation.Name);
            //}
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

