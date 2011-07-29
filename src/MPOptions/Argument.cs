using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MPOptions.Parser;
using MPOptions.Validators;

namespace MPOptions
{
    public class Argument : Element
    {
        public Argument(string name) : base(name)
        {
            WithNoValidator();
        }

        //internal Argument(Command parentCommand, string name, IArgumentValidator argumentValidator)
        //    : base(parentCommand.StateBag,parentCommand, name)
        //{
        //    ArgumentValidator = argumentValidator;
        //}

        internal IArgumentValidator ArgumentValidator
        {
            private set;
            get;
        }
        



        public Argument WithCustomValidator(Func<string, bool> validator)
        {
            return WithCustomValidator(validator, 1);
        }

        public Argument WithCustomValidator(Func<string,bool> validator, int maximumOccurrence)
        {
            ThrowErrorWhenReadOnly();
            //IArgumentValidator argumentValueValidator = this.ArgumentValidator;
            //try
            //{
            this.ArgumentValidator = new CustomArgumentValidator(validator) { MaximumOccurrence = maximumOccurrence };
            //    ValidationFactory.Validate(this);
            //}
            //catch
            //{
            //    this.ArgumentValidator = argumentValueValidator;
            //    throw;
            //}

            return this;

        }


        public Argument WithRegexValidator(string pattern)
        {
            return WithRegexValidator(pattern,1);
        }

        public Argument WithRegexValidator(string pattern,int maximumOccurrence)
        {
            ThrowErrorWhenReadOnly();
            //IArgumentValidator argumentValueValidator = this.ArgumentValidator;
            //try
            //{
            this.ArgumentValidator = new RegularExpressionArgumentValidator(pattern) { MaximumOccurrence = maximumOccurrence };
            //    ValidationFactory.Validate(this);
            //}
            //catch
            //{
            //    this.ArgumentValidator = argumentValueValidator;
            //    throw;
            //}

            return this;

        }

        public Argument WithNoValidator() //MP: rename; sounds not good
        {
            return WithNoValidator(1);
        }

        public Argument WithNoValidator(int maximumOccurrence) //MP: rename; sounds not good
        {
            ThrowErrorWhenReadOnly();

            //IArgumentValidator argumentValueValidator = this.ArgumentValidator;
            //try
            //{
                this.ArgumentValidator = new FallThroughArgumentValidator() { MaximumOccurrence = maximumOccurrence };
            //    ValidationFactory.Validate(this);
            //}
            //catch
            //{
            //    this.ArgumentValidator = argumentValueValidator;
            //    throw;
            //}

            return this;

        }
    }
}
