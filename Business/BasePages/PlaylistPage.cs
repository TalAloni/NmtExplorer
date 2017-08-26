using System;
using System.Web;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Utilities;

namespace NmtExplorer.Business
{
    public class PlaylistPage : System.Web.UI.Page
    {
        string m_playlist;

        public PlaylistPage()
        {
            string directoryRelativeUrl = Conversion.ToString(HttpContext.Current.Request.Params["DirectoryRelativeUrl"]);
            string fileName = Conversion.ToString(HttpContext.Current.Request.Params["FileName"]);
            string extension = Conversion.ToString(HttpContext.Current.Request.Params["Extension"]);

            DirectoryListEntryCollection collection = DirectoryListUtils.GetDirectoryList(IISUtils.MapPath(directoryRelativeUrl));

            DirectoryListEntry firstEntry = new DirectoryListEntry();
            firstEntry.Name = fileName;
            firstEntry.Extension = extension;
            string directoryAbsoluteUrl = UrlUtils.HostUrl + directoryRelativeUrl;
            if (ExtensionUtils.IsImage(extension))
            {
                m_playlist = DirectoryListUtils.GetImagePlaylist(collection, firstEntry, directoryAbsoluteUrl);
            }
            else
            {
                m_playlist = DirectoryListUtils.GetAudioPlaylist(collection, firstEntry, directoryAbsoluteUrl);
            }   
        }

        protected override void Render(System.Web.UI.HtmlTextWriter writer)
        {
            writer.Write(m_playlist);
        }
    }
}
