using System;
using System.Text.RegularExpressions;

namespace MPOptions
{
    public class RegularExpressionOptionValueValidator : IOptionValueValidator
    {
        public RegularExpressionOptionValueValidator(string pattern)
        {
            this.pattern = pattern;
            ValueOptional = false;
        }

        private readonly string pattern;

        #region IOptionValidator Members

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

        public bool ValueOptional
        {
            get; set;
        }

        #endregion
    }
}