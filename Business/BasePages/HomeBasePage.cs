using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Web.Hosting;
using Utilities;

namespace NmtExplorer.Business
{
    public class HomeBasePage : BasePage
    {
        public HomeBasePage()
        { 

        }

        protected void Page_Load(object sender, EventArgs e)
        {
            BindData();
        }

        private void BindData()
        {
            string homePageHtml = GetTemplate("Home\\Home.htm");
            string movieJukeboxLinkHtml = String.Empty;
            if (Directory.Exists(IISUtils.MapPath(UrlUtils.ToRelativeUrl("~/MovieJukebox/"))))
            {
                movieJukeboxLinkHtml = GetTemplate("Home\\MovieJukeboxLink.htm");
            }
            homePageHtml = homePageHtml.Replace("<MovieJukeboxLink>", movieJukeboxLinkHtml);
            this.MainContentHtml = homePageHtml;
        }
    }
}
