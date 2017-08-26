using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;
using Utilities;

namespace NmtExplorer.Business
{
    public class SettingManager
    {
        public static string GetValue(string key)
        {
            return Conversion.ToString(ConfigurationManager.AppSettings[key]);
        }

        public static int GetIntValue(string key, int defaultValue)
        {
            return Conversion.ToInt32(GetValue(key), defaultValue);
        }

        public static bool GetBooleanValue(string key, bool defaultValue)
        {
            return Conversion.ToBoolean(GetValue(key), defaultValue);
        }
    }
}
