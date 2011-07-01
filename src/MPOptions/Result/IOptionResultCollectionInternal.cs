using System.Collections.Generic;

namespace MPOptions.Result
{
    internal interface IOptionResultCollectionInternal : IEnumerable<IOptionResultInternal>
    {
        IOptionResultInternal this[string key]
        {
            get;
        }
    }
}