using System;
using System.Configuration;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace Utilities
{
    public class SettingUtils
    {
        private static XmlDocument GetConfigXml(string configXmlPath)
        {
            if (!FileSystemUtils.IsFileExist(configXmlPath))
            {
                WriteConfigXml(configXmlPath);
            }
            return XmlUtils.GetXmlDocument(configXmlPath);
        }

        private static void WriteConfigXml(string configXmlPath)
        {
            XmlDocument doc = new XmlDocument();
            XmlDeclaration declaration = doc.CreateXmlDeclaration("1.0", "utf-8", String.Empty);
            
            XmlElement configuration = doc.CreateElement("configuration");
            XmlElement appSettings = doc.CreateElement("appSettings");
            doc.AppendChild(declaration);
            doc.AppendChild(configuration);
            configuration.AppendChild(appSettings);
            doc.Save(configXmlPath);
        }

        private static XmlNode CreateChildNode(XmlDocument doc, XmlNode parentNode, string key, string value)
        {
            XmlElement element = doc.CreateElement("add");
            element.SetAttribute("key", key);
            element.SetAttribute("value", value);
            parentNode.AppendChild(element);
            return element;
        }

        private static XmlNode GetChildByKey(XmlNode node, string key)
        {
            foreach (XmlNode childNode in node.ChildNodes)
            {
                if (childNode.Attributes["key"].Value == key)
                {
                    return childNode;
                }
            }
            return null;
        }

        public static string GetValue(string configXmlPath, string key)
        { 
            return GetValue(configXmlPath, "configuration/appSettings", key);
        }

        public static string GetValue(string configXmlPath, string xpath, string key)
        {
            XmlDocument doc = GetConfigXml(configXmlPath);
            XmlNode node = doc.SelectSingleNode(xpath);

            node = GetChildByKey(node, key);
            if (node != null)
            {
                return node.Attributes["value"].Value;
            }
            return String.Empty;
        }

        public static int GetIntValue(string configXmlPath, string key)
        {
            return Conversion.ToInt32(GetValue(configXmlPath, key));
        }

        public static int GetIntValue(string configXmlPath, string xpath, string key)
        {
            return Conversion.ToInt32(GetValue(configXmlPath, xpath, key));
        }

        public static bool GetBooleanValue(string configXmlPath, string key)
        {
            return Conversion.ToBoolean(GetValue(configXmlPath, key));
        }

        public static bool GetBooleanValue(string configXmlPath, string xpath, string key)
        {
            return Conversion.ToBoolean(GetValue(configXmlPath, xpath, key));
        }

        public static void SetValue(string configXmlPath, string key, object value)
        {
            SetValue(configXmlPath, "configuration/appSettings", key, value);
        }

        public static void SetValue(string configXmlPath, string xpath, string key, object value)
        {
            XmlDocument doc = GetConfigXml(configXmlPath);
            XmlNode node = doc.SelectSingleNode(xpath);
            XmlNode childNode = GetChildByKey(node, key);
            if (childNode == null)
            {
                childNode = CreateChildNode(doc, node, key, Conversion.ToString(value));
            }
            else
            {
                childNode.Attributes["value"].Value = Conversion.ToString(value);
            }

            doc.Save(configXmlPath);
        }
    }
}
