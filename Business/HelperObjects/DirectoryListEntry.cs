using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace NmtExplorer.Business
{
    public partial class DirectoryListEntry
    {
        private string m_name;
        private string m_extension;
        private bool m_isDirectory;
        private string m_navigateUrl;
        private string m_browserTags;

        private SortedList<string, object> m_metaFields;

        public DirectoryListEntry()
        {
            m_name = String.Empty;
            m_extension = String.Empty;
            m_navigateUrl = String.Empty;

            m_metaFields = new SortedList<string, object>();
        }

        public string Name
        {
            get
            {
                return m_name;
            }
            set
            {
                m_name = value;
            }
        }

        public string Extension
        {
            get
            {
                return m_extension;
            }
            set
            {
                m_extension = value;
            }
        }

        public bool IsDirectory
        {
            get
            {
                return m_isDirectory;
            }
            set
            {
                m_isDirectory = value;
            }
        }

        public string NavigateUrl
        {
            get
            {
                return m_navigateUrl;
            }
            set
            {
                m_navigateUrl = value;
            }
        }

        public string BrowserTags
        {
            get
            {
                return m_browserTags;
            }
            set
            {
                m_browserTags = value;
            }
        }

        public SortedList<string, object> MetaFields
        {
            get { return m_metaFields; }
            set { m_metaFields = value; }
        }

        public object this[string columnName]
        {
            get
            {
                switch (columnName)
                {
                    case "Name":
                        return m_name;
                    case "Extension":
                        return m_extension;
                    case "IsDirectory":
                        return m_isDirectory;
                    case "NavigateUrl":
                        return m_navigateUrl;
                    case "BrowserTags":
                        return m_browserTags;
                    default:
                        return m_metaFields[columnName];
                }
            }
            set
            {
                switch (columnName)
                {
                    case "Name":
                        m_name = Convert.ToString(value);
                        break;
                    case "Extension":
                        m_extension = Convert.ToString(value);
                        break;
                    case "IsDirectory":
                        m_isDirectory = Convert.ToBoolean(value);
                        break;
                    case "NavigateUrl":
                        m_navigateUrl = Convert.ToString(value);
                        break;
                    case "BrowserTags":
                        m_browserTags = Convert.ToString(value);
                        break;
                    default:
                        m_metaFields[columnName] = value;
                        break;
                }
            }
        }


        public int CompareField(string columnName, object value)
        {
            switch (columnName)
            {
                case "Name":
                    return m_name.CompareTo(value);
                case "Extension":
                    return m_extension.CompareTo(value);
                case "IsDirectory":
                    return m_isDirectory.CompareTo(value);
                case "NavigateUrl":
                    return m_navigateUrl.CompareTo(value);
                case "BrowserTags":
                    return m_browserTags.CompareTo(value);
                default:
                    return CaseInsensitiveComparer.Default.Compare(m_metaFields[columnName], value);
            }
        }

        public DirectoryListEntry Clone()
        {
            DirectoryListEntry entity = new DirectoryListEntry();
            entity.Name = this.Name;
            entity.Extension = this.Extension;
            entity.IsDirectory = this.IsDirectory;
            entity.NavigateUrl = this.NavigateUrl;
            entity.BrowserTags = this.BrowserTags;
            //Cloning
            entity.MetaFields = new SortedList<string, object>(this.MetaFields);
            return entity;
        }

        public override bool Equals(object obj)
        {
            if (obj is DirectoryListEntry)
            {
                DirectoryListEntry entry = (DirectoryListEntry)obj;
                return (m_name == entry.Name) && (m_extension == entry.Extension);
            }
            return false;
        }

        public override int GetHashCode()
        {
            return String.Format("{0}.{1}", m_name, m_extension).GetHashCode();
        }
    }
}
