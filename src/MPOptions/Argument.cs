using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MPOptions.Internal;

namespace MPOptions
{
    public class Argument : Element
    {
        public Argument(string name) : base(name)
        {
        }

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
                ThrowHelper.ThrowParserException(errorContext);
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
            var parser = new Parser(this, commandLine);
            parserErrorContext = parser.Parse();
            return this;

            //var parser = new Parser(this.RootCommand, commandLine);
            //error = parser.Parse();
            //return this;
        }

        public Argument WithCustomValidator(Func<string, bool> validator)
        {
            return WithCustomValidator(validator, 1);
        }

        public Argument WithCustomValidator(Func<string,bool> validator, int maximumOccurrence)
        {
            IArgumentValidator argumentValueValidator = this.ArgumentValidator;
            try
            {
                this.ArgumentValidator = new CustomArgumentValidator(validator) { MaximumOccurrence = maximumOccurrence };
                ValidationFactory.Validate(this);
            }
            catch
            {
                this.ArgumentValidator = argumentValueValidator;
                throw;
            }

            return this;

        }


        public Argument WithRegexValidator(string pattern)
        {
            return WithRegexValidator(pattern,1);
        }

        public Argument WithRegexValidator(string pattern,int maximumOccurrence)
        {
            IArgumentValidator argumentValueValidator = new RegularExpressionArgumentValidator(pattern) { MaximumOccurrence = maximumOccurrence }; 

            //IArgumentValidator argumentValueValidator = this.ArgumentValidator;
            //try
            //{
            //    this.ArgumentValidator = new RegularExpressionArgumentValidator(pattern){ MaximumOccurrence = maximumOccurrence };
            //    ValidationFactory.Validate(this);
            //}
            //catch
            //{
            //    this.ArgumentValidator = argumentValueValidator;
            //    throw;
            //}

            return this;

        }

        //public Argument WithNoValidator()
        //{
        //    return WithNoValidator(1);
        //}

        //public Argument WithNoValidator(int maximumOccurrence)
        //{
        //    IArgumentValidator argumentValueValidator = this.ArgumentValidator;
        //    try
        //    {
        //        this.ArgumentValidator= new FallThroughArgumentValidator(){ MaximumOccurrence = maximumOccurrence};
        //        ValidationFactory.Validate(this);
        //    }
        //    catch
        //    {
        //        this.ArgumentValidator= argumentValueValidator;
        //        throw;
        //    }

        //    return this;

        //}
    }
}
