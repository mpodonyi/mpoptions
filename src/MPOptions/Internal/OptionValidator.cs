using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace MPOptions.Internal
{
    internal class OptionValidator : Validator<Option>
    {
        internal OptionValidator(Option obj)
            : base(obj)
        {}

        public override void Validate()
        {
            //if (!Regex.IsMatch(obj.Token, TokenRegex))
            if ((from ob in obj.Token.SplitInternal()
                 where ob.StartsWith("-") || ob.EndsWith("=") || ob.EndsWith(":") || ob.Any(o => char.IsWhiteSpace(o))
                 select ob).Any())
            {
                ThrowHelper.ThrowArgumentException(ExceptionResource.Argument_InValidForm,ExceptionArgument.token);
            }

            //Test that no sibling has same Name
            var name = from ii in (obj.IsGlobalOption ? obj.StateBag.Options.Values as IEnumerable<Option> : obj.ParentCommand.Options as IEnumerable<Option>) 
                       where ii.Name == obj.Name
                       select ii;
            if (name.Count() > 0)
            {
                ThrowHelper.ThrowArgumentException(ExceptionResource.Argument_AlreadyInDictionary,ExceptionArgument.name);
            }


            //Test that no sibling has same Token
            List<Option> optionsWithSameToken = (from ii in (obj.IsGlobalOption ? obj.StateBag.Options.Values as IEnumerable<Option> : obj.ParentCommand.Options as IEnumerable<Option>) 
                                                 from iii in ii.Token.SplitInternal()
                                                 from iiii in obj.Token.SplitInternal()
                                                 where iii == iiii
                                                 select ii).ToList();  //MP: should provide which token breaks the rules (for better exception handling)
            optionsWithSameToken.Add(obj);


            if (optionsWithSameToken.Count() > 1)
            {
                //test that only one exist with regex validator
                var countOptionWithRegex = (from i in optionsWithSameToken
                                            where i.OptionValueValidator is RegularExpressionOptionValueValidator
                                            select i).Count();
                if (countOptionWithRegex > 1)
                    ThrowHelper.ThrowArgumentException(ExceptionResource.MoreThenOneRegularExpression);


                //test that staticvalidator values are unique over every option with same token value
                var countOptionWithSameStaticValidationValue = (from i in optionsWithSameToken
                                                                where i.OptionValueValidator is StaticOptionValueValidator
                                                                from i2 in ((StaticOptionValueValidator) i.OptionValueValidator).values
                                                                group i by i2
                                                                into g
                                                                    where g.Count() > 1
                                                                    select g).Count();
                if (countOptionWithSameStaticValidationValue>0)
                    ThrowHelper.ThrowArgumentException(ExceptionResource.DoubleStaticValue,ExceptionArgument.staticoptionvalue);
            }

            //test that only option without validator or with validator exist
            if (optionsWithSameToken.Any(opt => opt.OptionValueValidator == null) && optionsWithSameToken.Any(opt => opt.OptionValueValidator != null))
                ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.token);

            //test that only 1 option without validator and same token exist
            //should maybe also test if optionvalue starts with other optionvalue; not just equal; like in ValidateOptionArguments6 in Oldstyle project
            if (optionsWithSameToken.Count(opt => opt.OptionValueValidator == null) > 1)  
                ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.token);

            //test that only 1 option with Validator and optionvalidatoroptional exist
            if(optionsWithSameToken.Count(opt => opt.OptionValueValidator == null || opt.OptionValueValidator.ValueOptional) > 1)
                ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.token);

            //test that when Option with OptionValidator FallThrough Exist that no other options exist with same token value
            if (optionsWithSameToken.Any(opt => opt.OptionValueValidator is FallThroughOptionValueValidator) && optionsWithSameToken.Count > 1)
                ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.token);
        }
    }
}