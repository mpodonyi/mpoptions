using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MPOptions.Validators;

namespace MPOptions.Extensions
{
    public static class ArgumentExtensions
    {
        public static Argument WithCustomValidator(this Argument element, Func<string, bool> validator)
        {
            return element.WithValidation(new CustomArgumentValidator(validator)); 
        }

        public static Argument WithRegexValidator(this Argument element, string pattern)
        {
            return element.WithValidation(new RegularExpressionArgumentValidator(pattern));
        }

        public static Argument WithNoValidator(this Argument element) //MP: rename; sounds not good
        {
            return element.WithValidation(new NullArgumentValidator() );
        }

        public static Argument SetNoMaximumOccurrence(this Argument element) 
        {
            return element.SetMaximumOccurrence(Int32.MaxValue);
        }

    }
}
