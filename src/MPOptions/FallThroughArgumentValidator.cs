using System;
using MPOptions.Internal;

namespace MPOptions
{
    public class FallThroughArgumentValidator:IArgumentValidator
    {
        #region IArgumentValidator Members

        public bool IsMatch(string value)
        {
            return true;
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
                if (value < 1)
                    ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionResource.Maximumoccurenceminimum, value);
                _MaximumOccurrence = value;
            }
        }

        #endregion
    }
}