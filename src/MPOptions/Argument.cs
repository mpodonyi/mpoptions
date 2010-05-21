using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MPOptions.Internal;

namespace MPOptions
{
    public class Argument : Element
    {
        internal Argument(Command parentCommand, string name, IArgumentValidator argumentValidator)
            : base(parentCommand.StateBag,parentCommand, name)
        {
            ArgumentValidator = argumentValidator;
        }

        internal IArgumentValidator ArgumentValidator
        {
            private set;
            get;
        }
        

        internal override string Path
        {
            get 
            {
                return ParentCommand.Path + "<" + Name + ">";
            }
        }

        public string Value
        {
            get
            {
                return _Values.FirstOrDefault();
            }
        }

        internal ICollection<string> _Values = new List<string>();

        public string[] Values
        {
            get
            {
                return _Values.ToArray();
            }
        }

        /// <summary>
        /// Parses this instance.
        /// </summary>
        /// <exception cref="ParserException">Thrown when the Commandline can not be parsed successful.</exception>
        /// <returns>Return the current argument.</returns>
        public Argument Parse()
        {
            var parser = new Parser(this);
            ParserErrorContext errorContext = parser.Parse();
            if (errorContext != null)
                ThrowHelper.ThrowParserError(errorContext);
            return this;
        }

        //internal Argument Parse(string commandLine, out bool error)
        //{
        //    var parser = new Parser(this.RootCommand, commandLine);
        //    error = parser.Parse();
        //    return this;
        //}

        internal Argument Parse(string commandLine, out ParserErrorContext parserErrorContext)
        {
            var parser = new Parser(this.RootCommand, commandLine);
            parserErrorContext = parser.Parse();
            return this;

            //var parser = new Parser(this.RootCommand, commandLine);
            //error = parser.Parse();
            //return this;
        }
    }
}
