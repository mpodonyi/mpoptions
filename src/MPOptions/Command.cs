using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MPOptions.Internal;

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


        /// <summary>
        /// Parses this instance.
        /// </summary>
        /// <exception cref="ParserException">Thrown when the Commandline can not be parsed successful.</exception>
        /// <returns>Return the current command.</returns>
        public Command Parse()
        {
            var parser = new Parser(this);
            ParserErrorContext errorContext = parser.Parse();
            if (errorContext != null)
                ThrowHelper.ThrowParserError(errorContext);
            return this;
        }

        ///// <summary>
        ///// Parses this instance.
        ///// </summary>
        ///// <param name="parserErrorContext">The Error Reported by the Parser. Otherwise Null.</param>
        ///// <returns>Return the current command.</returns>
        //public Command Parse(out ParserErrorContext parserErrorContext)
        //{
        //    var parser = new Parser(this);
        //    parserErrorContext = parser.Parse();
        //    return this;
        //}


        internal Command Parse(string commandLine, out ParserErrorContext parserErrorContext)
        {
            var parser = new Parser(this, commandLine);
            parserErrorContext = parser.Parse();
            return this;
        }
    }
}
