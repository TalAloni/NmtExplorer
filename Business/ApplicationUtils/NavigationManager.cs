using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Web.Hosting;
using Utilities;

namespace NmtExplorer.Business
{
    public class NavigationManager
    {
        public static string GetPlaylistPageUrl(string directoryRelativeUrl, string fileName, string extension)
        {
            return UrlUtils.ToRelativeUrl(String.Format("~/Playlist.aspx?DirectoryRelativeUrl={0}&FileName={1}&Extension={2}", directoryRelativeUrl, fileName, extension));
        }

        public static string GetBrowsePageUrl(string browseUrl)
        {
            return GetBrowsePageUrl(browseUrl, 1);
        }

        public static string GetBrowsePageUrl(string browseUrl, int pageIndex)
        {
            return UrlUtils.ToRelativeUrl(String.Format("~/Browse.aspx?BrowseUrl={0}&PageIndex={1}", browseUrl, pageIndex));
        }
    }
}
