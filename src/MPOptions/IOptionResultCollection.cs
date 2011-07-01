using System.Collections.Generic;

namespace MPOptions
{
    public interface IOptionResultCollection : IEnumerable<IOptionResult>
    {
        IOptionResult this[string key]
        {
            get;
        }

    }
}