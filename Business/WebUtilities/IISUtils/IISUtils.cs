using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.Runtime.InteropServices;
using System.Web.Hosting;
using System.Text;

namespace Utilities
{
    /// <summary>
    /// Some parts adopted from:
    /// http://dataerror.blogspot.com/2006/10/iis-getting-physical-path-for-virtual.html
    /// </summary>
    public class IISUtils
    {
        public static string GetIISSiteID(string siteName)
        {
            return GetIISSiteID("IIS://localhost", siteName);
        }

        public static string GetIISSiteID(string iisHost, string siteName)
        {
            string adsiPath = iisHost + "/W3SVC";

            try
            {
                DirectoryEntry entry = new DirectoryEntry(adsiPath);
                foreach (DirectoryEntry site in entry.Children)
                {
                    if (site.SchemaClassName == "IIsWebServer" && site.Properties["ServerComment"].Value.ToString().Equals(siteName))
                    {
                        return site.Name;
                    }
                }
            }
            catch (COMException)
            {
                return String.Empty;
            }

            return String.Empty;
        }

        /// <param name="virtualDirectoryName">Explicit virtual directory name, excluding physical subdirectories</param>
        public static string GetVirtualDirectoryPhysicalPath(string siteID, string virtualDirectoryName)
        {
            return GetVirtualDirectoryPhysicalPath("IIS://localhost", siteID, virtualDirectoryName);
        }

        public static string GetVirtualDirectoryPhysicalPath(string iisHost, string siteID, string virtualDirectoryName)
        {
            string adsiPath = String.Format("{0}/W3SVC/{1}/Root/{2}", iisHost, siteID, virtualDirectoryName);
            try
            {
                DirectoryEntry entry = new DirectoryEntry(adsiPath);
                if (entry.Properties.Contains("Path"))
                {
                    return entry.Properties["Path"].Value.ToString();
                }
                else
                {
                    return String.Empty;
                }
            }
            catch (COMException)
            {
                // If Virtual Directory is not found, exception will be thrown.
                return String.Empty;
            }
        }

        /// <summary>
        /// Map virtual path to physical path
        /// </summary>
        /// <param name="virtualPath">directory name must end with a slash</param>
        public static string MapPath(string virtualPath)
        {
            string relativeUrl = UrlUtils.ToRelativeUrl(virtualPath);
            string siteID = GetIISSiteID(HostingEnvironment.SiteName);
            if (!String.IsNullOrEmpty(siteID))
            {
                for (int index = relativeUrl.Length - 1; index > 0; index--)
                {
                    if (relativeUrl[index] == '/')
                    {
                        string virtualDirectoryName = relativeUrl.Substring(1, index - 1);
                        string virtualDirectoryPhysicalPath = GetVirtualDirectoryPhysicalPath(siteID, virtualDirectoryName);

                        if (!String.IsNullOrEmpty(virtualDirectoryPhysicalPath))
                        {
                            if (!virtualDirectoryPhysicalPath.EndsWith("\\"))
                            {
                                virtualDirectoryPhysicalPath += "\\";
                            }

                            return virtualDirectoryPhysicalPath + relativeUrl.Substring(index + 1).Replace("/", "\\");
                        }
                    }
                }
            }
            return HostingEnvironment.MapPath(relativeUrl);
        }
    }
}
