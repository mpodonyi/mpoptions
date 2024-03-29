using System.Collections.Generic;
using System.Linq;
using MPOptions.Parser;
using MPOptions.Validators;

namespace MPOptions.ElementTree
{
    internal class OptionCollection : CollectionAdapter<Option>, IOptionCollection
    {

        private readonly StateBag _StateBag;
        //private readonly CollectionAdapter<Option> Globaloptions;


        internal OptionCollection(StateBag stateBag, string preKey)
            : base(stateBag.Options, preKey)
        {
           // Globaloptions = new CollectionAdapter<Option>(stateBag.Options, "::");
            _StateBag = stateBag;
        }

        protected override void InsertItem(Option item)
        {
            Validate(item);
            if (item.IsGlobalOption)
                _StateBag.GlobalOptions.Add(item);
            else
                base.InsertItem(item);
        }

        private void Validate(Option obj)
        {
            if ((from ob in obj.Token.SplitInternal()
                 where ob.StartsWith("-") || ob.EndsWith("=") || ob.EndsWith(":") || ob.Any(o => char.IsWhiteSpace(o))
                 select ob).Any())
            {
                ThrowHelper.ThrowArgumentException(ExceptionResource.Argument_InValidForm, ExceptionArgument.token);
            }

            if (obj.IsGlobalOption)
            {
                if (_StateBag.Options.Values.Any(o=>o.Name==obj.Name))
                    ThrowHelper.ThrowArgumentException(ExceptionResource.Argument_AlreadyInDictionary, ExceptionArgument.name);
            }
            else
            {
                if (this.Contains(obj.Name))
                    ThrowHelper.ThrowArgumentException(ExceptionResource.Argument_AlreadyInDictionary, ExceptionArgument.name);
                if (_StateBag.GlobalOptions.Contains(obj.Name))
                    ThrowHelper.ThrowArgumentException(ExceptionResource.Argument_AlreadyInDictionary, ExceptionArgument.name);
            }

            //--------------------------------------------------------------

            IEnumerable<Option> colls;
            if (obj.IsGlobalOption)
                colls = _StateBag.Options.Values;
            else
                colls = _StateBag.GlobalOptions.Concat(this);

            var optionsWithSameTokenw = from o1 in colls
                                        //from o2 in command.Options
                                        //where !new OptionEqualityComparer().Equals(o1, o2)
                                        from t1 in o1.Token.SplitInternal()
                                        from t2 in obj.Token.SplitInternal()
                                        where t1 == t2
                                        group new[] { o1, obj } by t1
                                            into g
                                            select new
                                            {
                                                token = g.Key,
                                                vals = (from i in g
                                                        from u in i
                                                        select u).Distinct() //  .Distinct(new OptionEqualityComparer())
                                            };

            //ObjectDumper.Write(optionsWithSameTokenw);

            foreach (var options in optionsWithSameTokenw)
            {
                //test that only 1 option without validator and same token exist
                //should maybe also test if optionvalue starts with other optionvalue; not just equal; like in ValidateOptionArguments6 in Oldstyle project
                if (options.vals.Count(opt => opt.OptionValueValidator == null) > 1)
                    ThrowHelper.ThrowArgumentException(ExceptionResource.Generic);

                //test that only 1 option with Validator and optionvalidatoroptional exist
                if (options.vals.Any(opt => opt.OptionValueValidator == null) && options.vals.Any(opt => opt.OptionValueValidator != null && opt.OptionValueValidator.ValueOptional))
                    ThrowHelper.ThrowArgumentException(ExceptionResource.Generic);

                //MP: simulate this if i can really retire this
                ////test that only one option with ValueOptional exist
                //if (options.vals.Count(opt => opt.OptionValueValidator != null && opt.OptionValueValidator.ValueOptional) > 1)  
                //    ThrowHelper.ThrowArgumentException(ExceptionResource.Generic);


                //test that only option without validator or with validator exist
                if (options.vals.Any(opt => opt.OptionValueValidator == null) && options.vals.Any(opt => opt.OptionValueValidator != null))
                    ThrowHelper.ThrowArgumentException(ExceptionResource.Generic);

                var ttt = from i in options.vals
                          where i.OptionValueValidator != null
                          group i by i.OptionValueValidator.GetType();

                if (ttt.Count() == 0)
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

                if (ttt.First().Key != typeof(StaticOptionValueValidator) && ttt.First().Count() > 1)
                    ThrowHelper.ThrowArgumentException(ExceptionResource.Generic);
            }

            //foreach (Command com in command.Commands)
            //{
            //    ValidateAllOptionsOnLevel(com);
            //}



        }

        public void Clear(bool includingGlobalOptions)
        {
            this.Clear();
            if (includingGlobalOptions)
                _StateBag.GlobalOptions.Clear();


        }

        public new Option this[string key]
        {
            get
            {
                if (this.Contains(key))
                    return base[key];
                if (_StateBag.GlobalOptions.Contains(key))
                    return _StateBag.GlobalOptions[key];

                throw new KeyNotFoundException();
            }
        }

        public new int Count
        {
            get
            {
                return base.Count + _StateBag.GlobalOptions.Count;
            }
        }



       
    }

}
