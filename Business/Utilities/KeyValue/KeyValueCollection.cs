using System;
using System.Text;
using System.Collections;

namespace Utilities
{
    public class KeyValueCollection : CollectionBase
    {
        private ConditionalOperator m_conditionalOperator = ConditionalOperator.AND;

        public int Add(KeyValue entity)
        {
            return List.Add(entity);
        }

        public int Add(string key, object value)
        {
            KeyValue entity = new KeyValue(key, value);
            return List.Add(entity);
        }

        public void Remove(KeyValue entity)
        {
            List.Remove(entity);
        }

        public bool Contains(KeyValue value)
        {
            return this.List.Contains(value);
        }

        public int IndexOf(KeyValue value)
        {
            return this.List.IndexOf(value);
        }

        public KeyValue this[int index]
        {
            get
            {
                return ((KeyValue)List[index]);
            }
            set
            {
                List[index] = (KeyValue)value;
            }
        }
        
        public string ListKeys()
        {
            return ListKeys(String.Empty, true);
        }

        public string ListKeys(bool seperateByComma)
        {
            return ListKeys(String.Empty, seperateByComma);
        }

        public string ListKeys(string prefix)
        {
            return ListKeys(prefix, true);
        }

        public string ListKeys(string prefix, bool seperateByComma)
        {
            int index = 0;
            StringBuilder sBuilder = new StringBuilder();
            foreach (KeyValue keyValue in this.List)
            {
                if (index != 0)
                {
                    if (seperateByComma)
                    {
                        sBuilder.Append(",");
                    }
                }

                sBuilder.Append(prefix);
                sBuilder.Append(keyValue.Key);

                index++;
            }
            return sBuilder.ToString();
        }

        public string KeyList
        {
            get
            {
                return ListKeys();
            }
        }

        public ConditionalOperator Operator
        {
            get { return m_conditionalOperator; }
            set { m_conditionalOperator = value; }
        }
    }
}