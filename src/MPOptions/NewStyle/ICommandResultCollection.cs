using System.Collections.Generic;

namespace MPOptions.NewStyle
{
    public interface ICommandResultCollection: IEnumerable<ICommandResult>
    {
        ICommandResult this[string key]
        {
            get;
        }
      
    }
}