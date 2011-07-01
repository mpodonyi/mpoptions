using System.Collections.Generic;

namespace MPOptions.NewStyle
{
    public interface IOptionResultCollection : IEnumerable<IOptionResult>
    {
        IOptionResult this[string key]
        {
            get;
        }

    }
}