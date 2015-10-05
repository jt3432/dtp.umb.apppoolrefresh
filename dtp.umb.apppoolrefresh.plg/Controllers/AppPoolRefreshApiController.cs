using System;
using System.Configuration;
using System.Web;
using System.Web.Configuration;
using System.Web.Http;
using System.Xml;
using Umbraco.Core.Logging;
using Umbraco.Web.Editors;
using Umbraco.Web.Mvc;

namespace dtp.umb.apppoolrefresh.plg.Controllers
{
    [PluginController("AppPoolRefresh")]
    public class AppPoolRefreshApiController : UmbracoAuthorizedJsonController, IAppPoolRefreshApiController
    {
        #region Constants

        private const string APPKEY_TOUCHEDON = "AppPoolRefresh:TouchedOn";

        #endregion

        [HttpGet]
        public bool Refresh()
        {
            bool success = false;
            try
            {
                var currentDateTime = DateTime.Now.ToString();
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
                    valueAttr.Value = currentDateTime;

                    appPoolRefreshNode = webConfig.CreateNode("element", "add", "");
                    appPoolRefreshNode.Attributes.Append(keyAttr);
                    appPoolRefreshNode.Attributes.Append(valueAttr);

                    XmlNode appSettingsNode = webConfig.SelectSingleNode("/configuration/appSettings");
                    appSettingsNode.AppendChild(appPoolRefreshNode);
                }
                else
                {
                    appPoolRefreshNode.Attributes["value"].Value = currentDateTime;
                }

                webConfig.Save(webConfigPath);

                // The code below works great other than it will remove any comments that are under the AppSettings node 
                // when the config file is saved.
                //var configFile = WebConfigurationManager.OpenWebConfiguration("/");
                //var settings = configFile.AppSettings.Settings;
                //var currentDateTime = DateTime.Now.ToString();

                //if (settings[APP_SETTING_KEY] == null)
                //{
                //    settings.Add(APP_SETTING_KEY, currentDateTime);
                //}
                //else
                //{
                //    settings[APP_SETTING_KEY].Value = currentDateTime;
                //}
                //configFile.Save(ConfigurationSaveMode.Modified);
                //ConfigurationManager.RefreshSection(configFile.AppSettings.SectionInformation.Name);

                success = true;
            }
            catch (Exception ex)
            {
                // Opps
                LogHelper.Error(typeof(AppPoolRefreshApiController), ex.Message, ex);
            }
            return success;
        }
    }
}
