using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MPOptions.Internal
{
    internal static class StringExtension
    {
        internal static IEnumerable<string> SplitInternal(this string source)
        {
            return source.Split(';').Select(obj => obj.Trim());
            //return source.Split(';');
        }
    }
}