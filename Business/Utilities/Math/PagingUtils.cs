using System;
using System.Collections.Generic;
using System.Text;

namespace Utilities
{
    public class PagingUtils
    {
        /// <summary>
        /// One-Based (the first page is 1)
        /// </summary>
        public static int CalculatePageCount(int itemCount, int pageSize)
        {
            if (itemCount == 0)
            {
                return 1;
            }

            float pages = (float)itemCount / pageSize;
            int pageCount = MathUtils.RoundUp(pages);
            return pageCount;
        }

        /// <summary>
        /// In the method below the FirstPage Is 1, and the first item is 0
        /// </summary>
        public static int GetFirstItemIndex(int pageIndex, int pageSize)
        {
            return (pageIndex - 1) * pageSize;

        }

        public static int GetLastItemIndex(int pageIndex, int pageSize)
        {
            return (pageIndex) * pageSize - 1;
        }
    }
}
