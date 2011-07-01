using System.Collections.Generic;
using MPOptions.NewStyle;

namespace MPOptions.Result
{
    internal interface IArgumentResultInternal : IArgumentResult
    {
        new bool IsSet
        {
            get;
            set;
        }

        ICollection<string> _Values
        { get; }

        IArgumentValidator ArgumentValidator
        { get; }
    }
}