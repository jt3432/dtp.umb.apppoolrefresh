using System;
using System.Collections.Generic;
using System.Text;
using umbraco.interfaces;
using System.Xml;
using umbraco.cms.businesslogic.packager.standardPackageActions;
using System.Configuration;
using System.Web.Configuration;

namespace dtp.umb.apppoolrefresh.plg.Install
{

    /// <summary>
    /// Adds a key to the web.config app settings
    /// </summary>
    /// <remarks>Contribution from Paul Sterling</remarks>
    public class AddAppConfigKey : IPackageAction
    {
        #region IPackageAction Members

        public string Alias()
        {
            return "AddAppConfigKey";
        }

        public XmlNode SampleXml()
        {
            string sample = "<Action runat=\"install\" undo=\"true/false\" alias=\"AddAppConfigKey\" key=\"your key\" value=\"your value\"></Action>";
            return helper.parseStringToXmlNode(sample);
        }

        public bool Execute(string packageName, XmlNode xmlData)
        {
            string addKey = string.Empty;
            string addValue = string.Empty;

            try
            {
                addKey = GetAttributeValueFromNode<string>(xmlData, "key", String.Empty);
                addValue = GetAttributeValueFromNode<string>(xmlData, "value", String.Empty);

                if (addKey != String.Empty)
                {
                    AddUpdateAppSettings(addKey, addValue, false);
                }

                return true;
            }
            catch
            {
                return false;
            }

        }

        public bool Undo(string packageName, XmlNode xmlData)
        {
            string addKey = string.Empty;

            try
            {
                addKey = GetAttributeValueFromNode<string>(xmlData, "key", String.Empty);

                if (addKey != string.Empty)
                {
                    RemoveAppSettings(addKey);
                }

                return true;
            }
            catch
            {
                return false;
            }
        }

        #endregion

        #region helpers

        public T GetAttributeValueFromNode<T>(XmlNode node, string attributeName, T defaultValue)
        {
            if (node.Attributes[attributeName] != null)
            {
                string result = node.Attributes[attributeName].InnerText;
                if (string.IsNullOrEmpty(result))
                    return defaultValue;

                return (T)Convert.ChangeType(result, typeof(T));
            }
            return defaultValue;
        }

        private bool RemoveAppSettings(string key)
        {
            bool success = false;
            try
            {
                var configFile = WebConfigurationManager.OpenWebConfiguration("/");
                var settings = configFile.AppSettings.Settings;
                if (settings[key] != null)
                {
                    settings.Remove(key);
                }
                configFile.Save(ConfigurationSaveMode.Modified);
                ConfigurationManager.RefreshSection(configFile.AppSettings.SectionInformation.Name);
                success = true;
            }
            catch (Exception)
            {
                // Opps
            }
            return success;
        }

        private bool AddUpdateAppSettings(string key, string value, bool forceUpdate = true)
        {
            bool success = false;
            try
            {
                var configFile = WebConfigurationManager.OpenWebConfiguration("/");
                var settings = configFile.AppSettings.Settings;
                if (settings[key] == null)
                {
                    settings.Add(key, value);
                }
                else if (forceUpdate)
                {
                    settings[key].Value = value;
                }
                configFile.Save(ConfigurationSaveMode.Modified);
                ConfigurationManager.RefreshSection(configFile.AppSettings.SectionInformation.Name);
                success = true;
            }
            catch (Exception)
            {
                // Opps
            }
            return success;
        }

        #endregion
    }
}