using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MPOptions.NewStyle
{
    public interface IArgumentResultCollection : IEnumerable<IArgumentResult>
    {
        IArgumentResult this[string key]
        {
            get;
        }

    }

    internal interface IArgumentResultCollectionInternal : IEnumerable<IArgumentResultInternal>
    {
        IArgumentResultInternal this[string key]
        {
            get;
        }
    }

    internal class ArgumentResultCollection : IArgumentResultCollection, IArgumentResultCollectionInternal
    {
        internal ArgumentResultCollection(ArgumentCollection argumentCollection)
        {
            _ArgumentCollection = argumentCollection;
        }

        private ArgumentCollection _ArgumentCollection;

        public IArgumentResult this[string key]
        {
            get { throw new NotImplementedException(); }
        }

        public IEnumerator<IArgumentResult> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }

        IArgumentResultInternal IArgumentResultCollectionInternal.this[string key]
        {
            get { throw new NotImplementedException(); }
        }

        IEnumerator<IArgumentResultInternal> IEnumerable<IArgumentResultInternal>.GetEnumerator()
        {
            throw new NotImplementedException();
        }
    }
}
