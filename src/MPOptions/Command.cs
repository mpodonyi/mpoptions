using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MPOptions.Internal;
using MPOptions.Validator;

namespace MPOptions
{
    public class Command: Element
    {
        public static Command GetRoot()
        {
            return new Command();
        }

        internal override string Path
        {
            get
            {
                return IsRoot ? string.Empty : ParentCommand.Path + Name + " ";
            }
        }

        internal Command():base(new StateBag(), null,null)
        {
            StateBag.RootCommand = this;
        }

        internal Command(Command parentCommand, string name, string token)
            : base(parentCommand.StateBag,parentCommand, name)
        {
            this.Token = token;
        }

        public string Token
        {
            get;
            private set;
        }

        public bool IsRoot
        {
            get { return this==StateBag.RootCommand; }
        }

        public CommandCollection Commands
        {
            get
            {
                return new CommandCollection(this);
            }
        }

        public OptionCollection Options
        {
            get
            {
                return new OptionCollection(this);
            }
        }

        public ArgumentCollection Arguments
        {
            get
            {
                return new ArgumentCollection(this);
            }
        }
        

        public Command Parse(string commandLine, out bool error)
        {
            var parser = new Parser.Parser(this.RootCommand, commandLine);
            error = parser.Parse();
            return this;
        }
    }
}
