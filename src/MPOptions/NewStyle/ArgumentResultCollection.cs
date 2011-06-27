using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

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
        private ResultStateBag _ResultStateBag;

        internal ArgumentResultCollection(ArgumentCollection argumentCollection, ResultStateBag resultStateBag)
        {
            _ResultStateBag = resultStateBag;
            _ArgumentCollection = argumentCollection;
        }

        private ArgumentCollection _ArgumentCollection;
        private IDictionary<string, IArgumentResultInternal> _ArgumentResults;
        private IDictionary<string, IArgumentResultInternal> ArgumentResults
        {
            get
            {
                if (_ArgumentResults == null)
                {
                    _ArgumentResults = new Dictionary<string, IArgumentResultInternal>(_ArgumentCollection.Count);

                    foreach (Argument cmd in _ArgumentCollection)
                    {
                        _ArgumentResults.Add(cmd.Name, new ArgumentResult(cmd, _ResultStateBag));
                    }
                }

                return _ArgumentResults;
            }
        }

        public IArgumentResult this[string key]
        {
            get { return ArgumentResults[key]; }
        }

        IArgumentResultInternal IArgumentResultCollectionInternal.this[string key]
        {
            get { return ArgumentResults[key]; }
        }

        IEnumerator<IArgumentResult> IEnumerable<IArgumentResult>.GetEnumerator()
        {
#if NET40
            return ArgumentResults.Values.GetEnumerator();
#else
            return ArgumentResults.Values.OfType<IArgumentResult>().GetEnumerator();
#endif
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ArgumentResults.Values.GetEnumerator();
        }

        public IEnumerator<IArgumentResultInternal> GetEnumerator()
        {
            return ArgumentResults.Values.GetEnumerator();
        }

    }
}
