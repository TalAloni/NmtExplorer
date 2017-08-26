using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Security;
using System.Text;

namespace Utilities
{
    public class UrlUtils
    {
        public static string HostUrl
        {
            get
            {
                string result = String.Format("{0}://{1}", HttpContext.Current.Request.Url.Scheme, HttpContext.Current.Request.Url.Host);
                if (HttpContext.Current.Request.Url.Port != 80)
                {
                    result += String.Format(":{0}", HttpContext.Current.Request.Url.Port);
                }
                return result;
            }
        }

        public static string ToRelativeUrl(string url)
        {
            if (url.Length > 0)
            {
                string currentPath = System.Web.HttpContext.Current.Request.ApplicationPath;
                if (url.Substring(0, 1) == "~")
                {
                    if (currentPath.Length == 1)
                    {
                        currentPath = String.Empty;
                    }
                    url = url.Replace("~", currentPath);
                }
            }

            return url;
        }

        public static string ToAbsoluteUrl(string url)
        {
            if (url.Length > 0)
            {
                if (url.Substring(0, 1) == "~")
                {
                    string curPath = HostUrl + ToRelativeUrl("~");
                    url = url.Replace("~", curPath);
                }
            }
            return url;
        }

        public static string ToVirtualUrl(string url)
        {
            if (url.Length > 0)
            {
                string currentPath = HttpContext.Current.Request.ApplicationPath;

                if (url.StartsWith(currentPath))
                {
                    url = url.Remove(0, currentPath.Length);
                    url = "~" + url;
                }

            }
            return url;
        }

        public static string GetPageVirtualUrl(int pageID, string rawUrl, bool addPageID, params object[] parameters)
        {
            string url = rawUrl;
            int paramCount = StringUtils.CountOccurrences(url, '{');
            object[] urlParameters = new object[paramCount];

            for (int index = 0; index < paramCount; index++)
            {
                if (index < parameters.Length)
                {
                    urlParameters[index] = parameters[index];
                }
                else
                {
                    urlParameters[index] = 0;
                }
            }
            url = String.Format(url, urlParameters);
            if (addPageID && url.StartsWith("~"))
            {
                if (url.Contains("?"))
                {
                    url += "&";
                }
                else
                {
                    url += "?";
                }
                url += "P=" + pageID;
            }
            return url;
        }

        public static string RootDomain
        {
            get
            {
                string domain = HttpContext.Current.Request.Url.Host;
                string[] parts = domain.Split('.');
                if (parts.Length == 3)
                {
                    if (parts[2].Length != 2)
                    {
                        return domain.Substring(parts[0].Length + 1);
                    }
                }
                if (parts.Length == 4)
                {
                    return domain.Substring(parts[0].Length + 1);
                }
                return domain;
            }
        }

        public static string CleanUrl(string url)
        {
            List<string> parts = new List<string>(url.Split('/'));
            int index = parts.IndexOf("..");
            while (index > -1 && parts[index - 1] != "~")
            {
                parts.RemoveAt(index - 1);
                parts.RemoveAt(index - 1);
                index = parts.IndexOf("..");
            }
            StringBuilder sBuilder = new StringBuilder();
            for (int j = 0; j < parts.Count; j++)
            {
                sBuilder.Append(parts[j]);
                if (j != parts.Count - 1)
                {
                    sBuilder.Append("/");
                }
            }
            return sBuilder.ToString();
        }
    }
}
