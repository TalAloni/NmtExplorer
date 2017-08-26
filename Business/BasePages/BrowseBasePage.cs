using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Web;
using Utilities;

namespace NmtExplorer.Business
{
    public class BrowseBasePage : BasePage
    {
        private readonly int m_pageSize = 10;
        string m_browseUrl = Conversion.ToString(HttpContext.Current.Request.Params["BrowseUrl"]);
        int m_pageIndex = Conversion.ToInt32(HttpContext.Current.Request.Params["pageIndex"], 1);

        private DirectoryListEntryCollection m_directoryList;
        private DirectoryListEntryCollection m_directoryListPage;

        public BrowseBasePage()
        {
            if (m_browseUrl == String.Empty)
            {
                m_browseUrl = "/";
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            BindData();
        }

        private void BindData()
        {
            this.MainContentHtml = GetTemplate("Browse\\Browse.htm");

            BindList();
            BindStatusBar();
            BindNavigationLinks();
        }

        private void BindList()
        {
            string listEntryHtml = GetTemplate("Browse\\DirectoryListEntry.htm");
            StringBuilder builder = new StringBuilder();

            DirectoryListEntryCollection collection = this.DirectoryList;
            DirectoryListEntryCollection page = this.DirectoryListPage;

            int pageCount = PagingUtils.CalculatePageCount(this.DirectoryList.Count, m_pageSize);

            int index = 1;
            foreach (DirectoryListEntry entry in page)
            {
                string entryHtml = listEntryHtml;
                entryHtml = entryHtml.Replace("[Name]", entry.Name);
                entryHtml = entryHtml.Replace("[NavigateUrl]", entry.NavigateUrl);
                int displayOrder = (this.PageIndex - 1) * 10 + index;
                entryHtml = entryHtml.Replace("[DisplayOrder]", displayOrder.ToString());
                entryHtml = entryHtml.Replace("[NextDisplayOrder]", (displayOrder + 1).ToString());
                entryHtml = entryHtml.Replace("[IconName]", entry.IconName);
                if (m_pageIndex > 1 && index == 1)
                {
                    entry.BrowserTags = entry.BrowserTags + " onkeyupset=\"prev\"";
                }
                if (m_pageIndex < pageCount && index == m_pageSize)
                {
                    entry.BrowserTags = entry.BrowserTags + " onkeydownset=\"next\"";
                }
                entryHtml = entryHtml.Replace("[BrowserTags]", entry.BrowserTags);

                builder.Append(entryHtml);
                index++;
            }

            this.MainContentHtml = this.MainContentHtml.Replace("<List>", builder.ToString());
        }

        private void BindStatusBar()
        {
            int pageLastIndex = (this.PageIndex - 1) * 10 + this.DirectoryListPage.Count;
            int pageFirstIndex = Math.Min((this.PageIndex - 1) * 10 + 1, pageLastIndex);
            this.MainContentHtml = this.MainContentHtml.Replace("[PageFirstIndex]", pageFirstIndex.ToString());
            this.MainContentHtml = this.MainContentHtml.Replace("[PageLastIndex]", pageLastIndex.ToString());
            this.MainContentHtml = this.MainContentHtml.Replace("[ItemCount]", this.DirectoryList.Count.ToString());
        }
        private void BindNavigationLinks()
        {
            int pageCount = PagingUtils.CalculatePageCount(this.DirectoryList.Count, m_pageSize);
            this.MainContentHtml = this.MainContentHtml.Replace("[UpStatus]", m_pageIndex > 1 ? "On" : "Off");
            this.MainContentHtml = this.MainContentHtml.Replace("[DownStatus]", m_pageIndex < pageCount ? "On" : "Off");

            string previousPageLink = String.Empty;
            if (m_pageIndex > 1)
            {
                previousPageLink = this.GetTemplate("Browse\\PreviousPageLink.htm");
                previousPageLink = previousPageLink.Replace("[PreviousPageUrl]", NavigationManager.GetBrowsePageUrl(this.BrowseUrl, m_pageIndex - 1));
            }
            this.MainContentHtml = this.MainContentHtml.Replace("<PreviousPageLink>", previousPageLink);
            
            string nextPageLink = String.Empty;
            if (m_pageIndex < pageCount)
            { 
                nextPageLink = this.GetTemplate("Browse\\NextPageLink.htm");
                nextPageLink = nextPageLink.Replace("[NextPageUrl]", NavigationManager.GetBrowsePageUrl(this.BrowseUrl, m_pageIndex + 1));
            }
            this.MainContentHtml = this.MainContentHtml.Replace("<NextPageLink>", nextPageLink);
        }

        public string BrowseUrl
        {
            get
            {
                return m_browseUrl;
            }
        }

        public int PageIndex
        {
            get
            {
                return m_pageIndex;
            }
        }

        public string PhysicalPath
        {
            get
            {
                if (!m_browseUrl.EndsWith("/"))
                {
                    m_browseUrl += "/";
                }
                return IISUtils.MapPath(m_browseUrl);
            }
        }

        public DirectoryListEntryCollection DirectoryList
        {
            get
            {
                if (m_directoryList == null)
                {
                    m_directoryList = DirectoryListUtils.GetDirectoryList(this.PhysicalPath);
                }
                return m_directoryList;
            }
        }

        public DirectoryListEntryCollection DirectoryListPage
        {
            get
            {
                if (m_directoryListPage == null)
                {
                    m_directoryListPage = DirectoryList.GetPage(m_pageIndex, m_pageSize);
                    //Second pass - Update NavigateUrl, including playlist
                    foreach (DirectoryListEntry entry in m_directoryListPage)
                    {
                        if (entry.IsDirectory)
                        {
                            entry.NavigateUrl = NavigationManager.GetBrowsePageUrl(m_browseUrl + entry.Name);
                        }
                        else if (entry.IsImage)
                        {
                            //string playlist = DirectoryListUtils.GetImagePlaylist(m_directoryList, entry, UrlUtils.ToAbsoluteUrl("~") + m_browseUrl);
                            string playlistUrl = NavigationManager.GetPlaylistPageUrl(m_browseUrl, entry.Name, entry.Extension);
                            entry.NavigateUrl = "MUTE";
                            entry.BrowserTags = String.Format("pod=\"1,1,{0}\"", playlistUrl);
                        }
                        else if (entry.IsAudio)
                        {
                            //string playlist = DirectoryListUtils.GetAudioPlaylist(m_directoryList, entry, UrlUtils.ToAbsoluteUrl("~") + m_browseUrl);
                            string playlistUrl = NavigationManager.GetPlaylistPageUrl(m_browseUrl, entry.Name, entry.Extension);
                            entry.NavigateUrl = playlistUrl;
                            entry.BrowserTags = String.Format("pod=\"2,1,{0}\"", String.Empty);
                        }
                        else
                        {
                            entry.NavigateUrl = String.Format("{0}{1}.{2}", m_browseUrl, entry.Name, entry.Extension);
                            entry.BrowserTags = String.Format("vod=\"{0}\"", String.Empty);
                        }
                    }
                }
                return m_directoryListPage;
            }
        }

        
        /*
        public string GetBrowsePageUrl(string browsePageUrl, string browseUrl)
        {
            return GetBrowsePageUrl(browsePageUrl, browseUrl, 1);
        }

        public string GetBrowsePageUrl(string browsePageUrl, string browseUrl, int pageIndex)
        {
            return String.Format("{0}?BrowseUrl={1}&PageIndex={2}", browsePageUrl, browseUrl, pageIndex);
        }*/
    }
}
