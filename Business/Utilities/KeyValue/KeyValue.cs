using System;
using System.Text;
namespace Utilities
{
    public class KeyValue
    {
        private string m_key;
        private object m_value;

        public KeyValue()
        { }

        public KeyValue(string key, object value)
        {
            m_key = key;
            m_value = value;
        }

        public string Key
        {
            get { return m_key; }
            set { m_key = value; }
        }

        public object Value
        {
            get { return m_value; }
            set { m_value = value; }
        }
    }
}
