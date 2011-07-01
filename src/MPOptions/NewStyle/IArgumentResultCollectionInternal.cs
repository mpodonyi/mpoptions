using System.Collections.Generic;

namespace MPOptions.NewStyle
{
    internal interface IArgumentResultCollectionInternal : IEnumerable<IArgumentResultInternal>
    {
        IArgumentResultInternal this[string key]
        {
            get;
        }
    }
}