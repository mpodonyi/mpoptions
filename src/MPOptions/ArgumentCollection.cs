using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using MPOptions.Internal;
using MPOptions.NewStyle;

namespace MPOptions
{
    public interface IArgumentCollection : IMPOptionCollection<Argument>
    {
        //bool Contains(string key);

        //bool Remove(string key);

        //Command this[string key]
        //{ get; }

        //new int Count { get; }


    }


    class ArgumentCollection : CollectionAdapter<Argument>, IArgumentCollection
    {
        private readonly StateBag _StateBag;

        internal ArgumentCollection(StateBag stateBag, string preKey)
            : base(stateBag.Arguments, preKey)
        {
            _StateBag = stateBag;
        }
    }
}