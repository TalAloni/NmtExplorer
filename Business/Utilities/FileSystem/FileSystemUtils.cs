using System;
using System.Collections;
using System.IO;
using System.Text;
using System.Collections.Generic;

namespace Utilities
{
	public class FileSystemUtils
	{
		private FileSystemUtils()
		{}

        public static string ToAbsolutePath(string path, string fileName)
        {
            return ToAbsolutePath(path, false) + fileName;
        }

        public static string ToAbsoluteDirectoryPath(string path)
        {
            return ToAbsolutePath(path, false);
        }

        public static string ToAbsoluteFilePath(string path)
        {
            return ToAbsolutePath(path, true);
        }

        public static string ToAbsolutePath(string path, bool isFilePath)
		{
            if (path.Length > 0)
			{
                if (path.Substring(0, 1) == "~")
				{
					string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
                    baseDirectory = baseDirectory.Remove(baseDirectory.Length - 1, 1);

                    path = path.Replace("~", baseDirectory);
				}

                if (!path.Contains(":\\"))
                { 
                    path = path.Replace(":", ":\\");
                }

                if (!path.EndsWith("\\") && !isFilePath)
                {
                    path = String.Format("{0}\\", path);
                }
			}
			
			return path;
		}

		public static void CreateDirectory(string path)
		{
			Directory.CreateDirectory(ToAbsoluteDirectoryPath(path));
		}

		public static bool IsDirectoryExist(string path)
		{
            path = ToAbsolutePath(path, false);
            DirectoryInfo dir = new DirectoryInfo(path);
			return dir.Exists;
		}

		public static int CountFiles(string path)
		{
            DirectoryInfo dir = new DirectoryInfo(ToAbsoluteDirectoryPath(path) + "\\");
			return dir.GetFiles().Length;
		}

		public static bool IsFileExist(string path)
		{
            FileInfo file = new FileInfo(ToAbsolutePath(path, true));
			return file.Exists;	
		}

        /// <summary>
        /// This Method does not support files with length over 4GB
        /// </summary>
		public static byte[] ReadFile(string path)
		{
            path = ToAbsoluteFilePath(path);
			FileStream fileStream = new FileStream(path,FileMode.Open, FileAccess.Read);

			int fileLength = Convert.ToInt32(fileStream.Length);
			byte[] fileBytes = new byte[fileLength];

			fileStream.Read(fileBytes,0,fileLength);

			fileStream.Close();

			return fileBytes;

		}

		public static void RenameFile(string path, string oldName, string newName)
		{
            File.Delete(ToAbsolutePath(path,newName));
            File.Move(ToAbsolutePath(path, oldName),
                ToAbsolutePath(path, newName));
		}

        public static void WriteFile(string path, byte[] bytes)
        {
            WriteFile(path, bytes, FileMode.Create);
        }

		public static void WriteFile(string path, byte[] bytes, FileMode fileMode)
		{
            path = ToAbsoluteFilePath(path);
			
			FileInfo file = new FileInfo(path);

            FileStream stream = file.Open(fileMode, FileAccess.Write);
			stream.Write(bytes,0,bytes.Length);
			stream.Close();
		}

        public static void WriteFile(string path, string value)
        {
            WriteFile(path, value, FileMode.Create);
        }

        public static void WriteFile(string path, string value, FileMode fileMode)
        {
            byte[] bytes = UTF8Encoding.UTF8.GetBytes(value);
            WriteFile(path, bytes, fileMode);
        }

        public static void DeleteDirectory(string path)
        {
            path = ToAbsoluteDirectoryPath(path);
            Directory.Delete(path, true);
        }

		public static void DeleteFile(string path)
		{
            path = ToAbsolutePath(path, true);
			File.Delete(path);
		}

        public static string FindDirectory(string initialPath, string dirNameToFind)
        {
            dirNameToFind = dirNameToFind.ToLower();
            Queue queue = new Queue();
            queue.Enqueue(initialPath);
            while (queue.Count > 0)
            {
                string path = (string)queue.Dequeue();
                string[] dirs = Directory.GetDirectories(path);

                foreach (string subDirPath in dirs)
                {
                    string[] subDirParts = subDirPath.Split('\\');
                    if (subDirParts[subDirParts.Length - 1].ToLower() == dirNameToFind)
                    {
                        return subDirPath;
                    }
                    else
                    {
                        queue.Enqueue(subDirPath);
                    }
                }
            }
            return String.Empty;
        }

        //Extracts file name from path
        public static string GetEntryName(string path)
        {
            path = ToAbsoluteFilePath(path);
            string[] parts = path.Split('\\');
            if (parts.Length > 0)
            {
                if (parts[parts.Length - 1] == String.Empty)
                {
                    return parts[parts.Length - 2];
                }
                else
                {
                    return parts[parts.Length - 1];
                }
            }
            else
            {
                return String.Empty;
            }
        }

        public static string GetPathToParent(string path)
        {
            path = ToAbsoluteFilePath(path);
            List<string> parts = new List<string>();
            parts.AddRange(path.Split('\\'));
            if (parts[parts.Count - 1] == String.Empty)
            {
                parts.RemoveAt(parts.Count - 1);
            }
            parts.RemoveAt(parts.Count - 1);
            return ToAbsoluteDirectoryPath(StringUtils.Join(parts, "\\"));
        }

        public static bool IsRootDirectory(string path)
        {
            path = ToAbsoluteDirectoryPath(path);
            List<string> parts = new List<string>();
            parts.AddRange(path.Split('\\'));
            return (parts.Count == 1);
        }

        public static string CleanPath(string path)
        {
            List<string> parts = new List<string>(path.Split('\\'));
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
                    sBuilder.Append("\\");
                }
            }
            return sBuilder.ToString();
        }

        public static void GetFileNameAndExtension(string fileName, out string name, out string extension)
        {
            name = String.Empty;
            extension = String.Empty;
            int lastIndex = fileName.LastIndexOf(".");
            if (lastIndex > -1)
            {
                name = fileName.Remove(lastIndex);
                extension = fileName.Substring(lastIndex + 1);
            }
            else
            {
                name = fileName;
            }
        }

        public static string GetFileName(string name, string extension)
        {
            if (extension == String.Empty)
            {
                return name;
            }
            else
            {
                return String.Format("{0}.{1}", name, extension);
            }
        }

        public static void UpdateLastWriteTime(string path, DateTime lastWriteTime)
        {
            FileInfo file = new FileInfo(ToAbsolutePath(path, true));
            file.LastWriteTime = lastWriteTime;
        }
	}
}
