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
        public static string GetIISSiteName(string iisHost, string serverComment)
        {
            string adsiPath = iisHost + "/W3SVC";
            
            try
            {
                DirectoryEntry entry = new DirectoryEntry(adsiPath);
                foreach (DirectoryEntry site in entry.Children)
                {
                    if (site.SchemaClassName == "IIsWebServer" && site.Properties["ServerComment"].Value.ToString().Equals(serverComment))
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

        public static string GetVirtualDirectoryPhysicalPath(string iisHost, string siteName, string virtualDirectoryName)
        {
            string adsiPath = String.Format("{0}/W3SVC/{1}/Root/{2}", iisHost, siteName, virtualDirectoryName);
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

        public static string GetVirtualDirectoryPhysicalPath(string iisHost, string virtualDirectoryName)
        {
            string serverComment = "Default Web Site";
            string siteName = GetIISSiteName(iisHost, serverComment);
            if (siteName != String.Empty)
            {
                return GetVirtualDirectoryPhysicalPath(iisHost, siteName, virtualDirectoryName);
            }
            else
            {
                return String.Empty;
            }
        }

        /// <param name="virtualDirectoryName">Explicit virtual directory name, excluding physical subdirectories</param>
        public static string GetVirtualDirectoryPhysicalPath(string virtualDirectoryName)
        {
            return GetVirtualDirectoryPhysicalPath("IIS://localhost", virtualDirectoryName);
        }


        /// <summary>
        /// Map virtual path to physical path
        /// </summary>
        /// <param name="virtualPath">directory name must end with a slash</param>
        public static string MapPath(string virtualPath)
        {
            string relativeUrl = UrlUtils.ToRelativeUrl(virtualPath);
            for (int index = relativeUrl.Length - 1; index > 0; index--)
            {
                if (relativeUrl[index] == '/')
                {
                    string virtualDirectoryName = relativeUrl.Substring(1, index - 1);
                    string virtualDirectoryPath = GetVirtualDirectoryPhysicalPath(virtualDirectoryName);
                    if (virtualDirectoryPath != String.Empty)
                    {
                        return String.Format("{0}\\{1}", virtualDirectoryPath, relativeUrl.Substring(index + 1).Replace("/", "\\"));
                    }
                }
            }
            return HostingEnvironment.MapPath(relativeUrl);
        }
    }
}
