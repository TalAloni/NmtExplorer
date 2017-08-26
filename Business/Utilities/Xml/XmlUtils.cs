using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace Utilities
{
    public class XmlUtils
    {
        public static XmlDocument GetXmlDocument(string path)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(path);
            return doc;
        }

        public static XmlDocument GetXmlDocument(byte[] bytes)
        {
            string xml = Encoding.UTF8.GetString(bytes);
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml);
            return doc;
        }
    }
}
