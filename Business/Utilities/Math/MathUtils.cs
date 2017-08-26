using System;
using System.Collections.Generic;
using System.Text;

namespace Utilities
{
    public class MathUtils
    {
        public static int RoundUp(float number)
        {
            int result = Convert.ToInt32(number);

            if (result < number)
            {
                result++;
            }

            return result;
        }
    }
}
