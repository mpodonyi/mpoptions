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

        bool IsSet
        { get; }
    }

    internal interface ICommandResultInternal
    {
        ICommandResultCollectionInternal Commands
        { get; }


        bool IsSet
        {
            get;
            set;
        }
    }
   

    internal class CommandResult : ICommandResult, ICommandResultInternal
    {
        private Command _Command;

        internal CommandResult(Command command)
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

        public bool IsSet
        {
            get;
            set;
        }
    }
}
