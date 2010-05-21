using System;
using System.Linq;
using System.Text;

namespace MPOptions
{
    public class ParserErrorContext
    {
        internal ParserErrorContext(char[] charArray, int pos)
        {
            this.charArray = charArray;
            this.pos = pos;
        }

        private readonly char[] charArray;
        private readonly int pos;

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(new string(charArray));
            sb.AppendFormat("{0,"+(pos+1)+"}", '^');
            return sb.ToString();
        }
    }
}