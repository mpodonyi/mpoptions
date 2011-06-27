﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MPOptions.NewStyle
{
    //internal class ElementResult
    //{

    //    //private ResultStateBag _ResultStateBag;

    //    protected ElementResult(ResultStateBag resultStateBag)
    //    { 
    //        this.ResultStateBag=resultStateBag;
    //    }

    //    internal ResultStateBag ResultStateBag
    //    {
    //        get;
    //        private set;
    //    }
    
    //}


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

        ResultStateBag ResultStateBag
        { get; }
        

       
    }
   

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
