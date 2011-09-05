using System.Text.RegularExpressions;
using MPOptions.Parser;

namespace MPOptions.Validators
{
    internal class RegularExpressionArgumentValidator : IArgumentValidator
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

        //private int _MaximumOccurrence = 1;
        //public int MaximumOccurrence
        //{
        //    get
        //    {
        //        return _MaximumOccurrence;
        //    }
        //    set
        //    {
        //        if (value < 1)
        //            ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionResource.Maximumoccurenceminimum, value);
        //        _MaximumOccurrence = value;
        //    }
        //}

        #endregion
    }
}