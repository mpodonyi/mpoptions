using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace MPOptions.NewStyle
{

    public interface ICommandResultCollection: IEnumerable<ICommandResult>
    {
        ICommandResult this[string key]
        {
            get;
        }
      
    }

    internal interface ICommandResultCollectionInternal :  IEnumerable<ICommandResultInternal>
    {
        ICommandResultInternal this[string key]
        {
            get;
        }
    }



    internal class CommandResultCollection : ICommandResultCollection, ICommandResultCollectionInternal
    {



        internal CommandResultCollection(CommandCollection commandCollection, ResultStateBag resultStateBag)
        {
            _CommandCollection = commandCollection;
        }

        private CommandCollection _CommandCollection;
        private IDictionary<string, ICommandResultInternal> _CommandResults;
        private IDictionary<string, ICommandResultInternal> CommandResults
        {
            get
            {
                if (_CommandResults == null)
                {
                    _CommandResults = new Dictionary<string, ICommandResultInternal>(_CommandCollection.Count);

                    foreach (Command cmd in _CommandCollection)
                    {
                        _CommandResults.Add(cmd.Name, new CommandResult(cmd));
                    }
                }

                return _CommandResults;
            }
        }




        public ICommandResult this[string key]
        {
            get 
            {
                return CommandResults[key];
            }
        }

        ICommandResultInternal ICommandResultCollectionInternal.this[string key]
        {
            get
            {
                return CommandResults[key];
            }
        }



        public IEnumerator<ICommandResultInternal> GetEnumerator()
        {
            return CommandResults.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return CommandResults.Values.GetEnumerator();
        }

        IEnumerator<ICommandResult> IEnumerable<ICommandResult>.GetEnumerator()
        {
#if NET40
            return CommandResults.Values.GetEnumerator();
#else
            return CommandResults.Values.OfType<ICommandResult>().GetEnumerator();
#endif
        }
    }

   
}
