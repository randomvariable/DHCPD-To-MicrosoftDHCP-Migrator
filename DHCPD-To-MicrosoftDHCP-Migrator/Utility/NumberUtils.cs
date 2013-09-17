using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DhcpdToMicrosoft.Utility
{
    class NumberUtils
    {
        public static IEnumerable<uint> Range(uint start, uint count)
        {
            for (uint current = 0; current < count; ++current)
            {
                yield return start + current;
            }
        }
    }
}
