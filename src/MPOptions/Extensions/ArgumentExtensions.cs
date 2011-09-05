using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MPOptions.Validators;

namespace MPOptions.Extensions
{
    public static class ArgumentExtensions
    {
        //public static Argument WithCustomValidator(this Argument element, Func<string, bool> validator)
        //{
        //    return element.WithCustomValidator(validator, 1);
        //}

        public static Argument WithCustomValidator(this Argument element, Func<string, bool> validator, int maximumOccurrence=1)
        {
            //ThrowErrorWhenReadOnly();
            //IArgumentValidator argumentValueValidator = this.ArgumentValidator;
            //try
            //{
            
            //this.ArgumentValidator = ;
            //    ValidationFactory.Validate(this);
            //}
            //catch
            //{
            //    this.ArgumentValidator = argumentValueValidator;
            //    throw;
            //}

            return element.WithValidator(new CustomArgumentValidator(validator) { MaximumOccurrence = maximumOccurrence }); 

        }


        //public static Argument WithRegexValidator(this Argument element, string pattern)
        //{
        //    return element.WithRegexValidator(pattern, 1);
        //}

        public static Argument WithRegexValidator(this Argument element, string pattern, int maximumOccurrence=1)
        {
            //ThrowErrorWhenReadOnly();
            //IArgumentValidator argumentValueValidator = this.ArgumentValidator;
            //try
            //{
            //this.ArgumentValidator = ;
            //    ValidationFactory.Validate(this);
            //}
            //catch
            //{
            //    this.ArgumentValidator = argumentValueValidator;
            //    throw;
            //}

            return element.WithValidator(new RegularExpressionArgumentValidator(pattern) { MaximumOccurrence = maximumOccurrence });

        }

        //public static Argument WithNoValidator(this Argument element) //MP: rename; sounds not good
        //{
        //    return element.WithNoValidator(1);
        //}

        public static Argument WithNoValidator(this Argument element, int maximumOccurrence=1) //MP: rename; sounds not good
        {
            //ThrowErrorWhenReadOnly();

            //IArgumentValidator argumentValueValidator = this.ArgumentValidator;
            //try
            //{
            //this.ArgumentValidator = ;
            //    ValidationFactory.Validate(this);
            //}
            //catch
            //{
            //    this.ArgumentValidator = argumentValueValidator;
            //    throw;
            //}

            return element.WithValidator(new FallThroughArgumentValidator() { MaximumOccurrence = maximumOccurrence });

        }

    }
}
