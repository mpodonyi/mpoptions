using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using MPOptions.Parser;
using MPOptions.Validators;

namespace MPOptions
{
    public class Option : Element
    {
        public Option(string name, string token,bool globalOption):base(name)
        {
            _GlobalOption = globalOption;
            this.Token = token;
        }


        //internal Option(Command parentCommand, string name, string token,bool globalOption)
        //    : base(parentCommand.StateBag,parentCommand, name)
        //{
        //    _GlobalOption = globalOption;
        //    this.Token = token;
        //}


        public string Token
        {
            get;
            private set;
        }

        internal virtual IOptionValueValidator OptionValueValidator
        {
            private set; get;
        }

        private readonly bool _GlobalOption = false;
        public bool IsGlobalOption
        {
            get
            {
                return _GlobalOption;
            }
        }



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
