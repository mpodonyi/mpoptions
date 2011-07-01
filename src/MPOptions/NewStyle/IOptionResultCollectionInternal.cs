using System.Collections.Generic;

namespace MPOptions.NewStyle
{
    internal interface IOptionResultCollectionInternal : IEnumerable<IOptionResultInternal>
    {
        IOptionResultInternal this[string key]
        {
            get;
        }
    }
}