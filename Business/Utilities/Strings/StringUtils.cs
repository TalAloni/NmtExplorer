using System;
using System.Collections;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Web;

namespace Utilities
{
	/// <summary>
	/// Summary description for StringUtils.
	/// </summary>
	public partial class StringUtils
	{
		private StringUtils()
		{}

		public static string CommaSeperation(int[] intArray)
		{
			int length = intArray.Length;

			StringBuilder sBuilder = new StringBuilder();

			for (int index=0;index<length;index++)
			{
				sBuilder.Append(intArray[index].ToString() + ",");
			}
			if (sBuilder.Length > 0)
			{
				sBuilder.Remove(sBuilder.Length-1,1);
			}

			return sBuilder.ToString();	
		}

        // adding zeros before a number
        public static string GetFixedLenghtNumber(object input, int outputLenght)
        {
            string strInput = Convert.ToString(input);
            if (strInput.Length > outputLenght)
            {
                return strInput.Substring(0, outputLenght);
            }

            int counter = outputLenght - strInput.Length;

            for (int index = 0; index < counter; index++)
            {
                strInput = "0" + strInput;
            }

            return strInput;
        }
        
        public static string GetFixedLenghtString(string input, int outputLenght)
        {
            string strInput = Convert.ToString(input);
            if (strInput.Length > outputLenght)
            {
                return strInput.Substring(0, outputLenght);
            }
            else
            {
                return strInput;
            }
        }

        public static int CountOccurrences(string str, char charToCount)
        {
            int result = 0;
            foreach (char currentChar in str)
            {
                if (charToCount == currentChar)
                {
                    result++;
                }
            }

            return result;
        }

        public static string RemoveMultipleSpace(string text)
        {
            while (text.Contains("  "))
            {
                text = text.Replace("  ", " ");
            }
            return text;
        }

        public static List<int> IndexesOf(string text, string value)
        {
            List<int> result = new List<int>();
            int position = 0;
            while (text.Contains(value))
            {
                int index = text.IndexOf(value);
                result.Add(position + index);
                position = position + index + value.Length;
                text = text.Substring(index + value.Length);
            }
            return result;
        }

        public static string FindByFormat(string format, string text)
        {
            int index = format.IndexOf("{0}");
            if (index > -1)
            {
                string prefix = format.Substring(0, index);
                string suffix = format.Substring(index + 3, format.Length - (index + 3));
                if (text.Contains(prefix))
                {
                    int startIndex = text.IndexOf(prefix) + prefix.Length;
                    int endIndex = text.IndexOf(suffix, startIndex);
                    return text.Substring(startIndex, endIndex - startIndex);
                }
            }
            return String.Empty;
        }

        public static List<string> Split(string text, char seperator)
        {
            List<string> result = new List<string>(text.Split(seperator));
            return result;
        }
         
        public static string Join(List<string> parts)
        {
            return Join(parts, String.Empty);
        }

        public static string Join(List<string> parts, string seperator)
        {
            StringBuilder sBuilder = new StringBuilder();
            for (int index = 0; index < parts.Count; index++)
            {
                if (index != 0)
                {
                    sBuilder.Append(seperator);
                }
                sBuilder.Append(parts[index]);
            }
            return sBuilder.ToString();
        }

        public static string Join(List<int> parts, string seperator)
        {
            StringBuilder sBuilder = new StringBuilder();
            for (int index = 0; index < parts.Count; index++)
            {
                if (index != 0)
                {
                    sBuilder.Append(seperator);
                }
                sBuilder.Append(parts[index]);
            }
            return sBuilder.ToString();
        }
        /*
        public static string GetAscii(string input)
        {
            return GetAscii(input, '_');
        }

        public static string GetAscii(string input, char replaceChar)
        {
            StringBuilder sBuilder = new StringBuilder();
            foreach (char c in input)
            {
                if (c < 256)
                {
                    sBuilder.Append(c);
                }
                else
                {
                    sBuilder.Append(replaceChar);
                }
            }
            return sBuilder.ToString();
        }
*/
        public static List<String> ToLower (List<string> list)
        {
            List<String> result = new List<string>();
            foreach (string enrty in list)
            {
                result.Add(enrty.ToLower());
            }
            return result;
        }
	}
}
