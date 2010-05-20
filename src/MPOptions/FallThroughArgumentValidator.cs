using System;

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
                if (value < 0)
                    throw new ArgumentException("maximumOccurrence can not be less then 0");
                _MaximumOccurrence = value;
            }
        }

        #endregion
    }
}