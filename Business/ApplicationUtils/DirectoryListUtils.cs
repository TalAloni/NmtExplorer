using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace NmtExplorer.Business
{
    public class DirectoryListUtils
    {
        public static DirectoryListEntryCollection GetDirectoryList(string physicalPath)
        {
            string[] directories;
            string[] files;
            DirectoryListEntryCollection collection = new DirectoryListEntryCollection();

            if (Directory.Exists(physicalPath))
            {
                directories = Directory.GetDirectories(physicalPath);
                files = Directory.GetFiles(physicalPath);

                foreach (string directoryPath in directories)
                {
                    //virtualPath = VirtualPathUtility.Combine(context.Request.Path + "/", Path.GetFileName(str3));
                    DirectoryListEntry entry = new DirectoryListEntry();

                    string directoryName = Path.GetFileName(directoryPath);
                    entry.Name = directoryName;
                    entry.IsDirectory = true;
                    collection.Add(entry);
                }

                foreach (string filePath in files)
                {
                    DirectoryListEntry entry = new DirectoryListEntry();

                    string fileName = Path.GetFileNameWithoutExtension(filePath);
                    entry.Name = fileName;
                    
                    entry.IsDirectory = false;
                    string extension = Path.GetExtension(filePath);

                    if (extension.StartsWith("."))
                    {
                        extension = extension.Substring(1);
                    }
                    entry.Extension = extension;
                    if (entry.IsPlayable)
                    {
                        collection.Add(entry);
                    }
                }
            }
            collection = FilterCollection(collection);
            return collection;
        }

        public static DirectoryListEntryCollection GetImageCollection(DirectoryListEntryCollection collection)
        {
            DirectoryListEntryCollection result = new DirectoryListEntryCollection();
            {
                foreach (DirectoryListEntry entry in collection)
                {
                    if (entry.IsImage)
                    {
                        result.Add(entry);
                    }
                }
            }
            return result;
        }

        public static DirectoryListEntryCollection GetAudioCollection(DirectoryListEntryCollection collection)
        {
            DirectoryListEntryCollection result = new DirectoryListEntryCollection();
            {
                foreach (DirectoryListEntry entry in collection)
                {
                    if (entry.IsAudio)
                    {
                        result.Add(entry);
                    }
                }
            }
            return result;
        }

        public static string GetImagePlaylist(DirectoryListEntryCollection collection, DirectoryListEntry firstEntry, string directoryAbsoluteUrl)
        {
            StringBuilder builder = new StringBuilder();
            collection = GetImageCollection(collection);
            int startIndex = collection.IndexOf(firstEntry);
            for (int index = startIndex; index < collection.Count; index++)
            { 
                DirectoryListEntry entry = collection[index];
                AppendImageEntry(builder, entry, directoryAbsoluteUrl);
            }

            for (int index = 0; index < startIndex; index++)
            {
                DirectoryListEntry entry = collection[index];
                AppendImageEntry(builder, entry, directoryAbsoluteUrl);
            }
            return builder.ToString();
        }

        public static string GetAudioPlaylist(DirectoryListEntryCollection collection, DirectoryListEntry firstEntry, string directoryAbsoluteUrl)
        {
            StringBuilder builder = new StringBuilder();
            collection = GetAudioCollection(collection);
            int startIndex = collection.IndexOf(firstEntry);
            for (int index = startIndex; index < collection.Count; index++)
            {
                DirectoryListEntry entry = collection[index];
                AppendAudioEntry(builder, entry, directoryAbsoluteUrl);
            }

            for (int index = 0; index < startIndex; index++)
            {
                DirectoryListEntry entry = collection[index];
                AppendAudioEntry(builder, entry, directoryAbsoluteUrl);
            }
            return builder.ToString();
        }

        public static void AppendAudioEntry(StringBuilder builder, DirectoryListEntry entry, string directoryAbsoluteUrl)
        {
            string url = String.Format("{0}{1}.{2}", directoryAbsoluteUrl, entry.Name, entry.Extension);
            //Song List Format:
            //Song Title|Range Start|Range End|Song URL|
            bool encodeNonAsciiUrls = SettingManager.GetBooleanValue("EncodeNonAsciiUrlsInAudioPlaylist", true);
            if (encodeNonAsciiUrls)
            {
                url = UrlEncodeNonAscii(url);
            }
            builder.AppendFormat("{0}|0|0|{1}|", entry.Name, url);
        }

        public static void AppendImageEntry(StringBuilder builder, DirectoryListEntry entry, string directoryAbsoluteUrl)
        {
            int photoInterval = SettingManager.GetIntValue("ImageInterval", 5);
            string url = String.Format("{0}{1}.{2}", directoryAbsoluteUrl, entry.Name, entry.Extension);
            //Photo List Format:
            //Photo Interval (in seconds)|Reserved|Title|Photo URL|
            builder.AppendFormat("{0}|0|{1}|{2}|", photoInterval, entry.Name, url);
        }

        public static DirectoryListEntryCollection FilterCollection(DirectoryListEntryCollection collection)
        {
            DirectoryListEntryCollection result = new DirectoryListEntryCollection();
            string excludeOnExactMatch = SettingManager.GetValue("ExcludeOnExactMatch").ToLower();
            string[] excludeOnExactMatchList = excludeOnExactMatch.Split(';');
            string excludeOnPartialMatch = SettingManager.GetValue("ExcludeOnPartialMatch").ToLower();
            string[] excludeOnPartialMatchList = excludeOnPartialMatch.Split(';');
            
            foreach (DirectoryListEntry entry in collection)
            {
                bool exclude = false;
                foreach (string exact in excludeOnExactMatchList)
                {
                    if (entry.Name.ToLower().Equals(exact))
                    {
                        exclude = true;
                        break;
                    }
                }
                foreach (string partial in excludeOnPartialMatchList)
                {
                    if (entry.Name.ToLower().Contains(partial) && !partial.Equals(String.Empty))
                    {
                        exclude = true;
                        break;
                    }
                }
                if (!exclude)
                {
                    result.Add(entry);
                }
            }
            return result;
        }

        public static string UrlEncodeNonAscii(string str)
        {
            return UrlEncodeNonAscii(str, Encoding.UTF8);
        }

        public static string UrlEncodeNonAscii(string str, Encoding e)
        {
            MethodInfo methodInfo = typeof(System.Web.HttpUtility).GetMethod("UrlEncodeNonAscii", BindingFlags.Static | BindingFlags.NonPublic);
            object result = methodInfo.Invoke(null, new object[] { str, e });
            return (string)result;
        }
    }
}
