using System.Collections.Generic;

namespace MPOptions.NewStyle
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