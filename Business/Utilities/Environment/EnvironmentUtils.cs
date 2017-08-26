using System;
using System.Web;
using System.Collections.Generic;
using System.Text;

namespace Utilities
{
    public class EnvironmentUtils
    {
        public static string MachineFullyQualifiedHostName
        {
            get
            {
                return System.Net.Dns.GetHostEntry(System.Net.Dns.GetHostName()).HostName;
            }
        }

        public static string MachineFullyQualifiedDomainName
        {
            get
            {
                List<string> parts = StringUtils.Split(MachineFullyQualifiedHostName, '.');
                parts.RemoveAt(0);
                string result = StringUtils.Join(parts, ".");
                return result;
            }
        }

        public static string ComputerName
        {
            get
            {
                if (HttpContext.Current.User != null)
                {
                    if (HttpContext.Current.User.Identity.IsAuthenticated)
                    {
                        return HttpContext.Current.User.Identity.Name;
                    }
                }
                return String.Empty;
            }
        }

        public static string Domain
        {
            get
            {
                if (ComputerName.Contains("\\"))
                {
                    return ComputerName.Split('\\')[0];
                }
                else
                {
                    return String.Empty;
                }
                
            }
        }

        public static string Account
        {
            get
            {
                if (ComputerName.Contains("\\"))
                {
                    return ComputerName.Split('\\')[1];
                }
                else
                {
                    return String.Empty;
                }
            }
        }

    }
}
