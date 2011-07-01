using System.Collections.Generic;

namespace MPOptions
{
    public interface ICommandResultCollection: IEnumerable<ICommandResult>
    {
        ICommandResult this[string key]
        {
            get;
        }
      
    }
}