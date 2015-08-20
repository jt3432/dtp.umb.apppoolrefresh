using System;
using System.Configuration;
using System.Web.Configuration;
using System.Web.Http;
using Umbraco.Core.Logging;
using Umbraco.Web.Editors;
using Umbraco.Web.Mvc;

namespace dtp.umb.apppoolrefresh.plg.Controllers
{
    [PluginController("AppPoolRefresh")]
    public class AppPoolRefreshApiController : UmbracoAuthorizedJsonController, IAppPoolRefreshApiController
    {
        #region Constants

        private const string APP_SETTING_KEY = "AppPoolRefresh:TouchedOn";

        #endregion

        [HttpGet]
        public bool Refresh()
        {
            bool success = false;
            try
            {
                var configFile = WebConfigurationManager.OpenWebConfiguration("/");
                var settings = configFile.AppSettings.Settings;
                var currentDateTime = DateTime.Now.ToString();

                if (settings[APP_SETTING_KEY] == null)
                {
                    settings.Add(APP_SETTING_KEY, currentDateTime);
                }
                else
                {
                    settings[APP_SETTING_KEY].Value = currentDateTime;
                }
                configFile.Save(ConfigurationSaveMode.Modified);
                ConfigurationManager.RefreshSection(configFile.AppSettings.SectionInformation.Name);
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
