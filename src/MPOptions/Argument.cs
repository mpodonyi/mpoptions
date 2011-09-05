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
            ArgumentValidator = new NullArgumentValidator();
            MaximumOccurrence = 1;
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

        public Argument WithValidation(IArgumentValidator validator)
        {
            ThrowErrorWhenReadOnly();
            this.ArgumentValidator = validator;
            return this;
        }

        internal int MaximumOccurrence
        {
            private set;
            get;
        }

        public Argument SetMaximumOccurrence(int value)
        {
            if (value< 1)
                ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionResource.Maximumoccurenceminimum, value);

            MaximumOccurrence = value;
            return this;
        }
       
    }
}
