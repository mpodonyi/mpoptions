using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MPOptions.NewStyle
{
    public interface ICommandResult
    {
        ICommandResultCollection Commands
        { get; }

        IArgumentResultCollection Arguments
        { get; }

        IOptionResultCollection Options
        { get; }




        bool IsSet
        { get; }

        string Token
        {
            get;
        }


        string Name
        {
            get;
        }
    }

    internal interface ICommandResultInternal : ICommandResult
    {
        new ICommandResultCollectionInternal Commands
        { get; }

        new IArgumentResultCollectionInternal Arguments
        { get; }

        new IOptionResultCollectionInternal Options
        { get; }

        new bool IsSet
        {
            get;
            set;
        }

       
    }
   

    internal class CommandResult :  ICommandResultInternal
    {
        private Command _Command;

        internal CommandResult(Command command, ResultStateBag resultStateBag)
        {
            _Command = command;
        }


        private CommandResultCollection _Commands;

        public ICommandResultCollection Commands
        {
            get 
            {
                if (_Commands == null)
                    _Commands = new CommandResultCollection(_Command.Commands as CommandCollection);
                return _Commands;
            }
        }

        ICommandResultCollectionInternal ICommandResultInternal.Commands
        {
            get
            {
                if (_Commands == null)
                    _Commands = new CommandResultCollection(_Command.Commands as CommandCollection);
                return _Commands;
            }
        }

        private ArgumentResultCollection _Arguments;

        public IArgumentResultCollection Arguments
        {
            get
            {
                if (_Arguments == null)
                    _Arguments = new ArgumentResultCollection(_Command.Arguments as ArgumentCollection);
                return _Arguments;
            }
        }

        IArgumentResultCollectionInternal ICommandResultInternal.Arguments
        {
            get
            {
                if (_Arguments == null)
                    _Arguments = new ArgumentResultCollection(_Command.Arguments as ArgumentCollection);
                return _Arguments;
            }
        }

        private OptionResultCollection _Options;

        public IOptionResultCollection Options
        {
            get
            {
                if (_Options == null)
                    _Options = new OptionResultCollection(_Command.Options as OptionCollection);
                return _Options;
            }
        }

        IOptionResultCollectionInternal ICommandResultInternal.Options
        {
            get
            {
                if (_Options == null)
                    _Options = new OptionResultCollection(_Command.Options as OptionCollection);
                return _Options;
            }
        }



        public bool IsSet
        {
            get;
            set;
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
    }
}
