using MPOptions.ElementTree;

namespace MPOptions.Result
{

    internal class CommandResult :  ICommandResultInternal
    {
        private Command _Command;
        private ResultStateBag _ResultStateBag;

        internal CommandResult(Command command, ResultStateBag resultStateBag)
        {
            _ResultStateBag = resultStateBag;
            _Command = command;
        }


        private CommandResultCollection _Commands;

        public ICommandResultCollection Commands
        {
            get 
            {
                if (_Commands == null)
                    _Commands = new CommandResultCollection(_Command.Commands as CommandCollection,_ResultStateBag);
                return _Commands;
            }
        }

        ICommandResultCollectionInternal ICommandResultInternal.Commands
        {
            get
            {
                if (_Commands == null)
                    _Commands = new CommandResultCollection(_Command.Commands as CommandCollection, _ResultStateBag);
                return _Commands;
            }
        }

        private ArgumentResultCollection _Arguments;

        public IArgumentResultCollection Arguments
        {
            get
            {
                if (_Arguments == null)
                    _Arguments = new ArgumentResultCollection(_Command.Arguments as ArgumentCollection, _ResultStateBag);
                return _Arguments;
            }
        }

        IArgumentResultCollectionInternal ICommandResultInternal.Arguments
        {
            get
            {
                if (_Arguments == null)
                    _Arguments = new ArgumentResultCollection(_Command.Arguments as ArgumentCollection, _ResultStateBag);
                return _Arguments;
            }
        }

        private OptionResultCollection _Options;

        public IOptionResultCollection Options
        {
            get
            {
                if (_Options == null)
                    _Options = new OptionResultCollection(_Command.Options as OptionCollection, _ResultStateBag);
                return _Options;
            }
        }

        IOptionResultCollectionInternal ICommandResultInternal.Options
        {
            get
            {
                if (_Options == null)
                    _Options = new OptionResultCollection(_Command.Options as OptionCollection, _ResultStateBag);
                return _Options;
            }
        }



        private bool _IsSet;
        public bool IsSet
        {
            get
            {
                return _ResultStateBag.HasError ? false : _IsSet;
            }
            set
            {
                _IsSet = value;
            }
        }


        public string Token
        {
            get 
            {
                return _Command.Token;
            }
        }

        public string Name
        {
            get
            {
                return _Command.Name;
            }
        }

        ResultStateBag ICommandResultInternal.ResultStateBag
        {
            get
            {
                return this._ResultStateBag;
            }
        }

    }
}
