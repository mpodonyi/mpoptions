using System.Collections;
using System.Collections.Generic;

namespace MPOptions.NewStyle
{
    public interface IMPOptionCollection<T>: ICollection<T>, ICollection where T:Element
    {
        bool Contains(string key);

        bool Remove(string key);

        T this[string key]
        { get; }

        new int Count
        {
            get;
        }

    }
}