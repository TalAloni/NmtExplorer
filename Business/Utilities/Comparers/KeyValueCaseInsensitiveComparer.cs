using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Utilities
{
    public class KeyValueCaseInsensitiveComparer : IComparer
    {
        private bool m_isReversed = false;
        public KeyValueCaseInsensitiveComparer()
        {
        }

        public KeyValueCaseInsensitiveComparer(bool isReversed)
        {
            m_isReversed = isReversed;
        }

        int IComparer.Compare(Object x, Object y)
        {
            KeyValue kvx = (KeyValue)x;
            KeyValue kvy = (KeyValue)y;
            int result = CaseInsensitiveComparer.Default.Compare(kvx.Value, kvy.Value);
            if (m_isReversed)
            {
                result = -result;
            }
            if (result == 0)
            {
                result = CaseInsensitiveComparer.Default.Compare(Convert.ToInt32(kvx.Key), Convert.ToInt32(kvy.Key));
            }
            return result;
        }
    }
}
