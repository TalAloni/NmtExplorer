using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Utilities
{
    public class WebControlUtils
    {
        public static Control FindParentControl(Control control, string id)
        {
            while (control != null)
            {
                if (control.ID == id)
                {
                    return control;
                }
                control = control.Parent;
            }
            return null;
        }

        public static Control FindParentControl(Control control, Type parentType)
        {
            while (control != null)
            {
                if (IsBaseType(control.GetType(), parentType))
                {
                    return control;
                }
                control = control.Parent;
            }
            return null;
        }

        public static Control FindChildControl(Control control, string id)
        {
            return FindChildControl(control.Controls, id);
        }

        public static Control FindChildControl(ControlCollection controls, string id)
        {
            foreach(Control control in controls)
            {
                if (control.ID == id)
                {
                    return control;
                }
                else
                {
                    Control child = FindChildControl(control.Controls, id);
                    if (child != null)
                    {
                        return child;
                    }
                }
            }
            return null;
        }

        public static bool IsBaseType(Type baseType, Type parentType)
        {
            while (baseType != null)
            {
                if (baseType == parentType)
                { return true; }
                baseType = baseType.BaseType;
            }
            return false;
        }

        public static string RenderControl(string path)
        {
            Page page = new Page();
            return RenderControl(page.LoadControl(path));
        }

        public static string RenderControl(Control control)
        {
            StringWriter sWriter = new StringWriter();
            HtmlTextWriter writer = new HtmlTextWriter(sWriter);
            control.RenderControl(writer);
            return sWriter.ToString();
        }
    }
}

