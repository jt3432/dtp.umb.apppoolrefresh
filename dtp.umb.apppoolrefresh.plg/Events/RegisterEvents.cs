using dtp.umb.apppoolrefresh.plg.Install;
using System.Web.Configuration;
using umbraco.cms.businesslogic.packager;
using Umbraco.Core;

namespace dtp.umb.apppoolrefresh.plg.Events
{
    public class RegisterEvents : ApplicationEventHandler
    {
        protected override void ApplicationStarted(UmbracoApplicationBase umbracoApplication, ApplicationContext applicationContext)
        {
            if (WebConfigurationManager.AppSettings["AppPoolRefresh:TouchedOn"] == null)
            {
                InstallHelper installHelper = new InstallHelper();
                installHelper.AddSectionDashboard();
                installHelper.AddAppSetting();
            }
            InstalledPackage.BeforeDelete += InstalledPackage_BeforeDelete;
        }

        private void InstalledPackage_BeforeDelete(InstalledPackage sender, System.EventArgs e)
        {
            if (sender.Data.Name == "DTP App Pool Refresh")
            {
                InstallHelper installHelper = new InstallHelper();
                installHelper.RemoveSectionDashboard();
                installHelper.RemoveAppSetting();
            }
        }
    }
}
