using System.Collections.Generic;
using MPOptions.Validators;

namespace MPOptions.Result
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