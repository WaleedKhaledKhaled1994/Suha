using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AD.Server.Helpers
{
    public class TotalPagesCount
    {
        public static long Value(long total, long size)
        {
            double result = (double)total / size;
            return (long)Math.Ceiling(result);
        }
    }
}
