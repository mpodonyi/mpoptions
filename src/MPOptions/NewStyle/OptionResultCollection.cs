using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace MPOptions.NewStyle
{
    internal class OptionResultCollection : IOptionResultCollection, IOptionResultCollectionInternal
    {
        private ResultStateBag _ResultStateBag;

        internal OptionResultCollection(OptionCollection optionCollection, ResultStateBag resultStateBag)
        {
            _ResultStateBag = resultStateBag;
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
                        _OptionResults.Add(cmd.Name, new OptionResult(cmd, _ResultStateBag));
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
