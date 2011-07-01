using System.Collections.Generic;

namespace MPOptions.NewStyle
{
    public interface IArgumentResultCollection : IEnumerable<IArgumentResult>
    {
        IArgumentResult this[string key]
        {
            get;
        }

    }
}