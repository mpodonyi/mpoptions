using MPOptions.Parser;

namespace MPOptions.Validators
{
    internal class NullArgumentValidator:IArgumentValidator
    {
        #region IArgumentValidator Members

        public bool IsMatch(string value, int position)
        {
            return true;
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