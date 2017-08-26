using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Web.Hosting;
using Utilities;

namespace NmtExplorer.Business
{
    public class BasePage : System.Web.UI.Page
    {
        private string m_themeName;
        private string m_screenType;

        private string m_pageHtml;
        private string m_mainContentHtml = String.Empty;

        public BasePage()
        {
            m_themeName = SettingManager.GetValue("ThemeName");
            m_screenType = HttpContext.Current.Request.ServerVariables["HTTP_USER_AGENT"].Contains("1280x720") ? "HD" : "SD";
            m_pageHtml = GetTemplate("Layout.htm");
        }

        public string ThemeName
        {
            get
            {
                return m_themeName;
            }
        }

        public string ThemeDirectory
        {
            get
            {
                return ThemeManager.GetThemeDirectoryRelativeUrl(m_themeName, m_screenType);
            }
        }

        public string ScreenType
        {
            get
            {
                return m_screenType;
            }
        }
        
        public string PageHtml
        {
            get
            {
                return m_pageHtml;
            }
            set
            {
                m_pageHtml = value;
            }
        }

        public string MainContentHtml
        {
            get
            {
                return m_mainContentHtml;
            }
            set
            {
                m_mainContentHtml = value;
            }
        }

        private void BindData()
        {
            m_pageHtml = m_pageHtml.Replace("<MainContent>", this.MainContentHtml);
            m_pageHtml = m_pageHtml.Replace("[server]", EnvironmentUtils.MachineFullyQualifiedHostName);
            m_pageHtml = m_pageHtml.Replace("[ThemeDirectory]", this.ThemeDirectory);
            m_pageHtml = m_pageHtml.Replace("[application]", "video");
            m_pageHtml = m_pageHtml.Replace("[ApplicationRelativeUrl]", UrlUtils.ToRelativeUrl("~"));
            
            BindTheme();
        }

        public void BindTheme()
        {
            List<string> themeSettings = ThemeManager.ListThemeSettings(m_themeName);
            foreach (string setting in themeSettings)
            { 
                string value = ThemeManager.GetThemeValue(m_themeName, setting, m_screenType);
                m_pageHtml = m_pageHtml.Replace(String.Format("[${0}]", setting), value);
            }

            List<string> themeLanguageSettings = ThemeManager.ListLanguageSettings(m_themeName);

            foreach (string setting in themeLanguageSettings)
            {
                string languageID = SettingManager.GetValue("LanguageID");
                string value = ThemeManager.GetLanguageValue(m_themeName, setting, languageID);
                m_pageHtml = m_pageHtml.Replace(String.Format("[${0}]", setting), value);
            }
        }

        protected override void Render(System.Web.UI.HtmlTextWriter writer)
        {
            BindData();
            writer.Write(this.PageHtml);
        }

        public string GetTemplate(string fileName)
        {
            return ThemeManager.GetTemplate(m_themeName, fileName);
        }
    }
}
