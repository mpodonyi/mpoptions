using System;
using System.Text.RegularExpressions;

namespace MPOptions
{
    public class RegularExpressionArgumentValidator : IArgumentValidator
    {
        public RegularExpressionArgumentValidator(string pattern)
        {
            this.pattern = pattern;
        }

        private readonly string pattern;

        #region IArgumentValidator Members

        public bool IsMatch(string value)
        {
            return Regex.IsMatch(value, pattern, RegexOptions.Multiline | RegexOptions.Compiled);
        }

        private int _MaximumOccurrence = 1;
        public int MaximumOccurrence
        {
            get
            {
                return _MaximumOccurrence;
            }
            set
            {
                if (value < 0)
                    throw new ArgumentException("maximumOccurrence can not be less then 0");
                _MaximumOccurrence = value;
            }
        }

        #endregion
    }
}