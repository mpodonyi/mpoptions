using System;

namespace MPOptions
{
    public class CustomArgumentValidator : IArgumentValidator
    {
        public CustomArgumentValidator(Func<string,bool> validator)
        {
            this.validator = validator;
        }

        private readonly Func<string, bool> validator;

        #region IArgumentValidator Members

        public bool IsMatch(string value)
        {
            return validator(value);
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
                    throw new ArgumentOutOfRangeException("maximumOccurrence can not be less then 0");
                _MaximumOccurrence = value;
            }
        }

        #endregion
    }
}