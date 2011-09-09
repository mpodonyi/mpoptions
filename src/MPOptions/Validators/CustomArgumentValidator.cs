using System;
using MPOptions.Parser;

namespace MPOptions.Validators
{
    internal class CustomArgumentValidator : IArgumentValidator
    {
        public CustomArgumentValidator(Func<string,bool> validator)
        {
            this.validator = validator;
        }

        private readonly Func<string, bool> validator;

        #region IArgumentValidator Members

        public bool IsMatch(string value, int position)
        {
            return validator(value);
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