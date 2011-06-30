using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using MPOptions.Internal;

namespace MPOptions
{
    public class Option : Element
    {
        public Option(string name, string token,bool globalOption):base(name)
        {
            _GlobalOption = globalOption;
            this.Token = token;
        }


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

        internal virtual IOptionValueValidator OptionValueValidator
        {
            set; get;
        }


        //internal override string Path
        //{
        //    get
        //    {
        //        return IsGlobalOption ? "[" + Name + "]" : ParentCommand.Path+"[" + Name + "]";
        //    }
        //}

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

        //MP: should global option give back the rootcommand or null
        //public override Command ParentCommand
        //{
        //    get
        //    {
        //        return IsGlobalOption ? RootCommand: base.ParentCommand;
        //    }
        //}

        public Option WithStaticValidator(params string[] values)
        {
            return WithStaticValidator(false, values);
        }

        public Option WithStaticValidator(bool valueOptional,params string[] values)
        {
            //IOptionValueValidator optionValueValidator = this.OptionValueValidator;
            //try
            //{
            this.OptionValueValidator = new StaticOptionValueValidator(values) { ValueOptional = valueOptional };
            //    ValidationFactory.Validate(this);
            //}
            //catch
            //{
            //    this.OptionValueValidator = optionValueValidator;
            //    throw;
            //}

            return this;
        }

        public Option WithRegexValidator(string pattern)
        {
            return WithRegexValidator(pattern, false, 1);
        }

        public Option WithRegexValidator(string pattern,int maximumOccurrence)
        {
            return WithRegexValidator(pattern, false, maximumOccurrence);
        }

        public Option WithRegexValidator(string pattern, bool valueOptional)
        {
            return WithRegexValidator(pattern, valueOptional, 1);
        }

        public Option WithRegexValidator(string pattern, bool valueOptional, int maximumOccurrence)
        {
            //IOptionValueValidator optionValueValidator = this.OptionValueValidator;
            //try
            //{
            this.OptionValueValidator = new RegularExpressionOptionValueValidator(pattern) { MaximumOccurrence = maximumOccurrence, ValueOptional = valueOptional };
            //    ValidationFactory.Validate(this);
            //}
            //catch
            //{
            //    this.OptionValueValidator = optionValueValidator;
            //    throw;
            //}

            return this;
        }

        public Option WithNoValidator()
        {
            return WithNoValidator(false,1);
        }

        public Option WithNoValidator(int maximumOccurrence)
        {
            return WithNoValidator(false, maximumOccurrence);
        }

        public Option WithNoValidator(bool valueOptional)
        {
            return WithNoValidator(valueOptional, 1);
        }

        public Option WithNoValidator(bool valueOptional, int maximumOccurrence)
        {
            //IOptionValueValidator optionValueValidator = this.OptionValueValidator;
            //try
            //{
            this.OptionValueValidator = new FallThroughOptionValueValidator() { MaximumOccurrence = maximumOccurrence, ValueOptional = valueOptional };
            //    ValidationFactory.Validate(this);
            //}
            //catch
            //{
            //    this.OptionValueValidator = optionValueValidator;
            //    throw;
            //}

            return this;
        }
    }
}
