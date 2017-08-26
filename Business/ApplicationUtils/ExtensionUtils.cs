using System;
using System.Collections.Generic;
using System.Text;

namespace NmtExplorer.Business
{
    public class ExtensionUtils
    {
        public static bool HasMatchingExtension(string extensions, string extension)
        {
            extension = extension.ToLower();
            extensions = extensions.ToLower();

            string[] extensionList = extensions.Split(';');

            foreach (string supportedExtension in extensionList)
            {
                if (supportedExtension.Equals(extension))
                {
                    return true;
                }
            }
            return false;
        }

        public static bool IsVideo(string extension)
        {
            string videoExtensions = SettingManager.GetValue("VideoExtensions");
            return HasMatchingExtension(videoExtensions, extension);
        }

        public static bool IsAudio(string extension)
        {
            string audioExtensions = SettingManager.GetValue("AudioExtensions");
            return HasMatchingExtension(audioExtensions, extension);
        }

        public static bool IsImage(string extension)
        {
            string imageExtensions = SettingManager.GetValue("ImageExtensions");
            return HasMatchingExtension(imageExtensions, extension);
        }
    }
}
