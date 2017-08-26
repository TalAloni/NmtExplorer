using System;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Web;

namespace Utilities
{
	public class Conversion
	{
        public static int ToInt32(object obj)
        {
            return ToInt32(obj, 0);
        }

		public static int ToInt32(object obj, int defaultValue)
		{
            int result = defaultValue;
			if (obj != null)
			{
				try
				{
					result = Convert.ToInt32(obj);
				}
				catch
				{}
			}
			return result;
		}

        public static long ToInt64(object obj)
        {
            return ToInt64(obj, 0);
        }

        public static long ToInt64(object obj, long defaultValue)
        {
            long result = defaultValue;
            if (obj != null)
            {
                try
                {
                    result = Convert.ToInt64(obj);
                }
                catch
                { }
            }
            return result;
        }

        public static float ToFloat(object obj)
        {
            return ToFloat(obj, 0);
        }

		public static float ToFloat(object obj, float defaultValue)
		{
            float result = defaultValue;
			if (obj != null)
			{
				try
				{
					result = Convert.ToSingle(obj);
				}
				catch
				{}
			}
			return result;
		}

        public static double ToDouble(object obj)
        {
            return ToDouble(obj, 0);
        }

        public static double ToDouble(object obj, double defaultValue)
        {
            double result = defaultValue;
            if (obj != null)
            {
                try
                {
                    result = Convert.ToDouble(obj);
                }
                catch
                { }
            }
            return result;
        }

        public static decimal ToDecimal(object obj)
        {
            return ToDecimal(obj, 0);
        }

        public static decimal ToDecimal(object obj, decimal defaultValue)
        {
            decimal result = defaultValue;
            if (obj != null)
            {
                try
                {
                    result = Convert.ToDecimal(obj);
                }
                catch
                { }
            }
            return result;
        }

        public static bool ToBoolean(object obj)
        {
            return ToBoolean(obj, false);
        }

        public static bool ToBoolean(object obj, bool defaultValue)
        {
            bool result = defaultValue;
            if (obj != null)
            {
                try
                {
                    result = Convert.ToBoolean(obj);
                }
                catch
                { }
            }
            return result;
        }

        public static string ToString(object obj)
        {
            string result = String.Empty;
            if (obj != null)
            {
                try
                {
                    result = Convert.ToString(obj);
                }
                catch
                {}
            }
            return result;
        }

        public static char ToChar(object obj)
        {
            return ToChar(obj, new char());
        }

        public static char ToChar(object obj, char defaultValue)
        {
            char result = defaultValue;
            if (obj != null)
            {
                try
                {
                    result = Convert.ToChar(obj);
                }
                catch
                { }
            }
            return result;
        }

        public static List<int> ToInt32(List<string> list)
        { 
            return ToInt32(list, true);
        }

        public static List<int> ToInt32(List<string> list, bool removeZeros)
        {
            List<int> result = new List<int>();
            foreach (string entity in list)
            {
                int value = ToInt32(entity);
                if (!(value == 0 && removeZeros))
                {
                    result.Add(value);
                }
            }
            return result;
        }

        /// <summary>
        /// dd?MM?yyyy Format
        /// </summary>
        public static DateTime ToDateTime(string value)
        {
            return ToDateTime(value, "dd?MM?yyyy");
        }

        public static DateTime ToDateTime(string value, string format)
        {
            return ToDateTime(value, format, DateTime.Now);
        }

        public static DateTime ToDateTime(string value, string format, DateTime defaultValue)
        {
            if (value.Length == format.Length)
            {
                try
                {
                    int dayIndex = format.IndexOf("dd");
                    int monthIndex = format.IndexOf("MM");
                    int yearIndex = format.IndexOf("yyyy");
                    int day = Convert.ToInt32(value.Substring(dayIndex, 2));
                    int month = Convert.ToInt32(value.Substring(monthIndex, 2));
                    int year = Convert.ToInt32(value.Substring(yearIndex, 4));
                    return new DateTime(year, month, day);
                }
                catch { return defaultValue; }
            }
            return defaultValue;
        }

        public static DateTime ToDateTime(object obj)
        {
            DateTime result = new DateTime();
            if (obj != null)
            {
                try
                {
                    result = Convert.ToDateTime(obj);
                }
                catch
                { }
            }
            return result;
        }

		public static BitArray ToBitArray(UInt64 number)
		{
			int bitCount = 64;
			BitArray bits = new BitArray(bitCount);
			
			UInt64 temp = number;

			for (int position = bitCount - 1; position > -1 ;position--)
			{
				UInt64 bitNumericValue  = Convert.ToUInt64(Math.Pow( 2 , position));
				bool isBitOn = temp / (bitNumericValue) > 0;
				bits[position] = isBitOn;
				if (isBitOn)
				{
					temp -= bitNumericValue;
				}
			}
			return bits;
		}

		public static UInt64 ToUInt64(BitArray bits)
		{
            if (bits.Count != 64)
            {
                bits.Length = 64;
            }

			int bitCount = 64;
			UInt64 retVal = 0;
			for (int position = 0; position < bitCount ;position++)
			{
				UInt64 bitNumericValue  = Convert.ToUInt64(Math.Pow( 2, position)) ;
				if (bits[position]) 
				{
					retVal += bitNumericValue;
				}
			}
			return retVal;
		}

        public static byte[] ToByteArray(BitArray bits)
        {
            // Who knows, might change
            const int BITSPERBYTE = 8;
            // Get the size of bytes needed to store all bytes
            int bytesize = bits.Length / BITSPERBYTE;
            // Any bit left over another byte is necessary
            if (bits.Length % BITSPERBYTE > 0)
            {
                bytesize++;
            }
            // For the result
            byte[] bytes = new byte[bytesize];
            // Must init to good value, all zero bit byte has value zero
            // Lowest significant bit has a place value of 1, each position to
            // to the left doubles the value
            byte value = 0;
            byte significance = 1;
            // Remember where in the input/output arrays
            int bytepos = 0;
            int bitpos = 0;
            while (bitpos < bits.Length)
            {
                // If the bit is set add its value to the byte
                if (true == bits[bitpos])
                {
                    value += significance;
                }
                bitpos++;
                if (0 == bitpos % BITSPERBYTE)
                {
                    // A full byte has been processed, store it
                    // increase output buffer index and reset work values
                    bytes[bytepos] = value;
                    bytepos++;
                    value = 0;
                    significance = 1;
                }
                else
                {
                    // Another bit processed, next has doubled value
                    significance *= 2;
                }
            }
            return bytes;
        }
	}
}
