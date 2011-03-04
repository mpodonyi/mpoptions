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


        new bool IsSet
        {
            get;
            set;
        }

       
    }
   

    internal class CommandResult :  ICommandResultInternal
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
