using System.Collections.Generic;
using System.Linq;

namespace MPOptions.Parser
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