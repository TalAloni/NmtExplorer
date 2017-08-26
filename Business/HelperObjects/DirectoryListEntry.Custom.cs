using System;
using System.Collections.Generic;
using System.Text;
using Utilities;

namespace NmtExplorer.Business
{
    public partial class DirectoryListEntry
    {
        public bool IsVideo
        {
            get
            {
                return ExtensionUtils.IsVideo(this.Extension);
            }
        }

        public bool IsAudio
        {
            get
            {
                return ExtensionUtils.IsAudio(this.Extension);
            }
        }

        public bool IsImage
        {
            get
            {
                return ExtensionUtils.IsImage(this.Extension);
            }
        }

        public bool IsPlayable
        {
            get
            {
                return (this.IsVideo || this.IsAudio || this.IsImage);
            }
        }

        public string IconName
        {
            get
            {
                if (this.IsDirectory)
                {
                    return "Folder";
                }
                else if (this.IsVideo)
                {
                    return "Video";
                }
                else if (this.IsAudio)
                {
                    return "Song";
                }
                else if (this.IsImage)
                {
                    return "Photo";
                }
                else
                {
                    return String.Empty;
                }
            }
        }

        public string FileNameWithExtension
        {
            get
            { 
                if (m_extension == String.Empty)
                {
                    return m_name;
                }
                else
                {
                    return String.Format("{0}.{1}", m_name, m_extension);
                }
            }
        }
    }
}
