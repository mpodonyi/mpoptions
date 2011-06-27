using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace MPOptions.NewStyle
{
    public interface IOptionResultCollection : IEnumerable<IOptionResult>
    {
        IOptionResult this[string key]
        {
            get;
        }

    }

    internal interface IOptionResultCollectionInternal : IEnumerable<IOptionResultInternal>
    {
        IOptionResultInternal this[string key]
        {
            get;
        }
    }

    internal class OptionResultCollection : IOptionResultCollection, IOptionResultCollectionInternal
    {
        internal OptionResultCollection(OptionCollection optionCollection, ResultStateBag resultStateBag)
        {
            _OptionCollection = optionCollection;
        }

        private OptionCollection _OptionCollection;
        private IDictionary<string, IOptionResultInternal> _OptionResults;
        private IDictionary<string, IOptionResultInternal> OptionResults
        {
            get
            {
                if (_OptionResults == null)
                {
                    _OptionResults = new Dictionary<string, IOptionResultInternal>(_OptionCollection.Count);

                    foreach (Option cmd in _OptionCollection)
                    {
                        _OptionResults.Add(cmd.Name, new OptionResult(cmd));
                    }
                }

                return _OptionResults;
            }
        }

        public IOptionResult this[string key]
        {
            get { return OptionResults[key]; }
        }

        IOptionResultInternal IOptionResultCollectionInternal.this[string key]
        {
            get { return OptionResults[key]; }
        }

        IEnumerator<IOptionResult> IEnumerable<IOptionResult>.GetEnumerator()
        {
#if NET40
            return OptionResults.Values.GetEnumerator();
#else
            return OptionResults.Values.OfType<IOptionResult>().GetEnumerator();
#endif
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return OptionResults.Values.GetEnumerator();
        }

        public IEnumerator<IOptionResultInternal> GetEnumerator()
        {
            return OptionResults.Values.GetEnumerator();
        }

    }
}
