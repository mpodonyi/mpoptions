using System.Collections.Generic;

namespace MPOptions.Result
{
    internal interface IArgumentResultCollectionInternal : IEnumerable<IArgumentResultInternal>
    {
        IArgumentResultInternal this[string key]
        {
            get;
        }
    }
}