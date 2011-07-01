using System.Collections.Generic;

namespace MPOptions.Result
{
    internal interface ICommandResultCollectionInternal :  IEnumerable<ICommandResultInternal>
    {
        ICommandResultInternal this[string key]
        {
            get;
        }
    }
}