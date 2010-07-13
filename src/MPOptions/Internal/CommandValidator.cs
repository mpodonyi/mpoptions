using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace MPOptions.Internal
{
    internal class CommandValidator:Validator<Command>
    {
        internal CommandValidator(Command obj):base(obj)
        {}

        private const string TokenRegex = @"^((\s*\w+\s*)|(\s*\w+\s*;\s*)|(\s*\w+(\s*;\s*\w+\s*(;)?\s*)*))$";
        
        public override void PostValidate()
        {
            ValidateAllOptionsOnLevel(obj);

            //List<Option> optionsWithSameToken = (from ii in (obj.IsGlobalOption ? obj.StateBag.Options.Values as IEnumerable<Option> : obj.ParentCommand.Options as IEnumerable<Option>)
            //                                     from iii in ii.Token.SplitInternal()
            //                                     from iiii in obj.Token.SplitInternal()
            //                                     where iii == iiii
            //                                     && !CompareHelper(ii, obj)
            //                                     select ii).Distinct().ToList();  //MP: should provide which token breaks the rules (for better exception handling)

            //optionsWithSameToken.Add(obj);

            ////test that only option without validator or with validator exist
            //if (optionsWithSameToken.Any(opt => opt.OptionValueValidator == null) && optionsWithSameToken.Any(opt => opt.OptionValueValidator != null))
            //    ThrowHelper.ThrowArgumentException(ExceptionResource.Generic);
        }

       

        private void ValidateAllOptionsOnLevel(Command command)
        {
            var optionsWithSameTokenw = from o1 in command.Options
                                        from o2 in command.Options
                                        where !new OptionEqualityComparer().Equals(o1, o2)
                                        from t1 in o1.Token.SplitInternal()
                                        from t2 in o2.Token.SplitInternal()
                                        where t1 == t2
                                        group new[] {o1, o2} by t1
                                        into g
                                            select new
                                                       {
                                                           token = g.Key,
                                                           vals = (from i in g
                                                                   from u in i
                                                                   select u).Distinct(new OptionEqualityComparer())
                                                       };

            ObjectDumper.Write(optionsWithSameTokenw);

            foreach (var options in optionsWithSameTokenw)
            {
                //test that only 1 option without validator and same token exist
                //should maybe also test if optionvalue starts with other optionvalue; not just equal; like in ValidateOptionArguments6 in Oldstyle project
                if (options.vals.Count(opt => opt.OptionValueValidator == null) > 1)
                    ThrowHelper.ThrowArgumentException(ExceptionResource.Generic);

                //test that only 1 option with Validator and optionvalidatoroptional exist
                if (options.vals.Any(opt => opt.OptionValueValidator == null) && options.vals.Any(opt => opt.OptionValueValidator != null && opt.OptionValueValidator.ValueOptional))
                    ThrowHelper.ThrowArgumentException(ExceptionResource.Generic);

                ////test that only one option with ValueOptional exist
                //if (options.vals.Count(opt => opt.OptionValueValidator != null && opt.OptionValueValidator.ValueOptional) > 1)
                //    ThrowHelper.ThrowArgumentException(ExceptionResource.Generic);


                //MP: why this validation???
                //test that only option without validator or with validator exist
                if (options.vals.Any(opt => opt.OptionValueValidator == null) && options.vals.Any(opt => opt.OptionValueValidator != null))
                    ThrowHelper.ThrowArgumentException(ExceptionResource.Generic);

                var ttt = from i in options.vals
                          where i.OptionValueValidator !=null
                          group i by i.OptionValueValidator.GetType();

                if (ttt.Count()==0)
                    continue;

                if (ttt.Count() > 1)
                    ThrowHelper.ThrowArgumentException(ExceptionResource.Generic); //only 1 OptionValueValidator by type allowed

                if (ttt.First().Key == typeof(StaticOptionValueValidator))
                {
                    var countOptionWithSameStaticValidationValue = (from i in ttt.First()
                                                                    from i2 in ((StaticOptionValueValidator)i.OptionValueValidator).values
                                                                    group i by i2
                                                                        into g
                                                                        where g.Count() > 1
                                                                        select g).Count();
                    if (countOptionWithSameStaticValidationValue > 0)
                        ThrowHelper.ThrowArgumentException(ExceptionResource.DoubleStaticValue);
                }

                if(ttt.First().Key != typeof(StaticOptionValueValidator) && ttt.First().Count()>1)
                    ThrowHelper.ThrowArgumentException(ExceptionResource.Generic); 
            }
            
            foreach (Command com in command.Commands)
            {
                ValidateAllOptionsOnLevel(com);
            }
        }

        public override void Validate()
        {
            //Test that CommandValues meet the RegexRequirements
            if (!Regex.IsMatch(obj.Token, TokenRegex))
            {
                ThrowHelper.ThrowArgumentException(ExceptionResource.Argument_InValidForm, ExceptionArgument.token);
            }

            //Test that no sibling has same Name
            var name = from ii in obj.ParentCommand.Commands
                       where ii.Name==obj.Name
                       select ii;
            if(name.Count()>0)
            {
                ThrowHelper.ThrowArgumentException(ExceptionResource.Argument_AlreadyInDictionary, ExceptionArgument.name);
            }

            //Test that no sibling has same Token
            var strings = from ii in obj.ParentCommand.Commands
                          from iii in ii.Token.SplitInternal()
                          from iiii in obj.Token.SplitInternal()
                          where iii == iiii
                          select iiii;
            if(strings.Count()>0)
            {
                ThrowHelper.ThrowArgumentException(ExceptionResource.Argument_TokenPartAlreadyInDictionary, ExceptionArgument.token);
            }
        }
    }
}