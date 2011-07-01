using System.Collections.Generic;

namespace MPOptions.NewStyle
{
    internal interface ICommandResultCollectionInternal :  IEnumerable<ICommandResultInternal>
    {
        ICommandResultInternal this[string key]
        {
            get;
        }
    }
}