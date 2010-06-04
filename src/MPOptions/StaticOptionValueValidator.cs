using System.Linq;

namespace MPOptions
{
    internal sealed class StaticOptionValueValidator : IOptionValueValidator
    {
        public StaticOptionValueValidator(params string[] values)
        {
            this.values = values;
        }

        internal readonly string[] values;

        #region IOptionValidator Members

        public bool IsMatch(string value)
        {
            return values.Contains(value);
        }

        public int MaximumOccurrence
        {
            get { return 1; }
        }

        public bool ValueOptional
        {
            get;
            set;
        }

        #endregion
    }
}