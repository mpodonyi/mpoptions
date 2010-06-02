using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using MPOptions.Internal;

namespace MPOptions
{
    public class Option : Element
    {
        internal Option(Command parentCommand, string name, string token,bool globalOption)
            : base(parentCommand.StateBag,parentCommand, name)
        {
            _GlobalOption = globalOption;
            this.Token = token;
        }

        //internal Option(StateBag stateBag, string name, string token): base(stateBag,null, name)
        //{
        //    this.Token = token;
        //}

        public string Token
        {
            get;
            private set;
        }

        internal IOptionValueValidator OptionValueValidator
        {
            set; get;
        }


        internal override string Path
        {
            get
            {
                return IsGlobalOption ? "[" + Name + "]" : ParentCommand.Path+"[" + Name + "]";
            }
        }

        private readonly bool _GlobalOption = false;
        public bool IsGlobalOption
        {
            get
            {
                return _GlobalOption;
                //return ParentCommand == null; 
                    //StateBag.Options.ContainsKey(":: " + Name);
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
        /// <returns>Return the current option.</returns>
        public Option Parse()
        {
            var parser = new Parser(this);
            ParserErrorContext errorContext = parser.Parse();
            if (errorContext != null)
                ThrowHelper.ThrowParserException(errorContext);
            return this;
        }

        //internal Option Parse(string commandLine, out bool error)
        //{
        //    var parser = new Parser(this.RootCommand, commandLine);
        //    error = parser.Parse();
        //    return this;
        //}

        internal Option Parse(string commandLine, out ParserErrorContext parserErrorContext)
        {
            var parser = new Parser(this, commandLine);
            parserErrorContext = parser.Parse();
            return this;
        }

        //should global option give back the rootcommand or null
        public override Command ParentCommand
        {
            get
            {
                return IsGlobalOption ? RootCommand: base.ParentCommand;
            }
        }
    }
}
