using System.Collections.Generic;

namespace MPOptions
{
    public interface IArgumentResultCollection : IEnumerable<IArgumentResult>
    {
        IArgumentResult this[string key]
        {
            get;
        }

    }
}