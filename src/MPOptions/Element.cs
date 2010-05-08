using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MPOptions.Internal;
using MPOptions.Validator;

namespace MPOptions
{
    public abstract class Element
    {
        internal Element(StateBag stateBag, Command parentCommand, string name)
        {
            ParentCommand = parentCommand;
            ContextParent = parentCommand;
            StateBag = stateBag;
            this.Name = name;
        }

        protected Command ContextParent
        {
            private get;
            set;
        }

        internal StateBag StateBag
        {
            get; private set;
        }

        public Command RootCommand
        {
            get { return StateBag.RootCommand; }
        }

        public string Name
        {
            get;
            private set;
        }

        internal abstract string Path
        { get; }

        internal virtual bool Set
        {
            get; 
            set;
        }

        public bool IsSet
        {
            get { return Set; }
        }

        public virtual Command ParentCommand
        {
            get; private set;
        }


        public Command AddCommand(string name, string token)
        {
            Command command = new Command(this as Command ?? ContextParent, name, token);
            //Command command = new Command(this as Command ?? ParentCommand, name, token);
            ValidationFactory.Validate(command);

            this.StateBag.Commands.Add(command.Path, command);

            return command;
        }


        public Option AddOption(string name, string token)
        {
            return AddOptionInternal(name, token, null, false);
        }

        public Option AddOption(string name, string token, IOptionValueValidator optionValueValidator)
        {
            return AddOptionInternal(name, token, optionValueValidator, false);
        }

        public Option AddGlobalOption(string name, string token)
        {
            return AddOptionInternal(name, token, null, true);
        }

        public Option AddGlobalOption(string name, string token, IOptionValueValidator optionValueValidator)
        {
            return AddOptionInternal(name,token,optionValueValidator,true);
        }

        private Option AddOptionInternal(string name, string token, IOptionValueValidator optionValueValidator,bool globalOption)
        {
            Option option = new Option(this as Command ?? ContextParent, name, token, globalOption);
            //Option option = new Option(this as Command ?? ParentCommand, name, token, globalOption);
            option.OptionValueValidator = optionValueValidator;
            ValidationFactory.Validate(option);

            this.StateBag.Options.Add(option.Path, option);

            return option;
        }

        public Argument AddArgument(string name, IArgumentValidator argumentValidator)
        {
            Argument argument = new Argument(this as Command ?? ContextParent, name, argumentValidator);
            //Argument argument = new Argument(this as Command ?? ParentCommand, name, argumentValidator);
            ValidationFactory.Validate(argument);

            this.StateBag.Arguments.Add(argument.Path, argument);

            return argument;
        }


    }
}
