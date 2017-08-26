using System;
using System.Collections.Generic;
using System.Text;
using System.Web.Hosting;
using System.Xml;
using Utilities;

namespace NmtExplorer.Business
{
    public class ThemeManager
    {
        public static string GetThemeRootDirectory(string themeName)
        {
             return String.Format("{0}theme\\{1}\\", HostingEnvironment.ApplicationPhysicalPath, themeName);
        }

        public static string GetThemeConfigurationFilePath(string themeName)
        {
            string path = GetThemeRootDirectory(themeName) + "theme.config";
            return path;
        }

        public static string GetThemeValue(string themeName, string itemName, string screenType)
        {
            string path = GetThemeConfigurationFilePath(themeName);
            string xpath = String.Format("configuration/themeSettings/{0}", itemName);
            return SettingUtils.GetValue(path ,xpath, screenType);
        }

        public static string GetLanguageValue(string themeName, string itemName, string langaugeID)
        {
            string path = GetThemeConfigurationFilePath(themeName);
            string xpath = String.Format("configuration/languageSettings/{0}", itemName);
            return SettingUtils.GetValue(path, xpath, langaugeID);
        }

        public static List<string> ListThemeSettings(string themeName)
        {
            string xpath = "configuration/themeSettings";
            return ListChildNodes(themeName, xpath);
        }

        public static List<string> ListLanguageSettings(string themeName)
        {
            string xpath = "configuration/languageSettings";
            return ListChildNodes(themeName, xpath);
        }

        private static List<string> ListChildNodes(string themeName, string xpath)
        {
            string path = String.Format("{0}theme\\{1}\\theme.config", HostingEnvironment.ApplicationPhysicalPath, themeName);

            XmlDocument doc = XmlUtils.GetXmlDocument(path);
            XmlNode node = doc.SelectSingleNode(xpath);

            List<string> result = new List<string>();

            foreach (XmlNode childNode in node.ChildNodes)
            {
                result.Add(childNode.Name);
            }
            return result;
        }

        public static string GetTemplate(string themeName, string fileName)
        {
            string path = String.Format("{0}Template\\{1}", GetThemeRootDirectory(themeName), fileName);
            string result = UTF8Encoding.UTF8.GetString(FileSystemUtils.ReadFile(path));
            return result;
        }

        public static string GetThemeDirectoryRelativeUrl(string themeName, string screenType)
        {
            return UrlUtils.ToRelativeUrl(String.Format("~/theme/{0}/{1}", themeName, screenType));
 
        }
    }
}
