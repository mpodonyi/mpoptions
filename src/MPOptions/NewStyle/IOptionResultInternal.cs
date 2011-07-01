using System.Collections.Generic;

namespace MPOptions.NewStyle
{
    internal interface IOptionResultInternal : IOptionResult
    {
        new bool IsSet
        {
            get;
            set;
        }

        ICollection<string> _Values
        { get; }

        IOptionValueValidator OptionValueValidator
        { get; }
    }
}