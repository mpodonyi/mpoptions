using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MPOptions.Internal
{
    internal class OptionImpl: Option
    {
        internal OptionImpl(Option option, Command contextParent)
            : base(option.ParentCommand, option.Name, option.Token, option.IsGlobalOption)
        {
            base.OptionValueValidator = option.OptionValueValidator;
            ContextParent = contextParent;
            savedInstance = option;

            this.Set = option.Set;
            this._Values = option._Values;

        }

        internal Option savedInstance;

        internal override IOptionValueValidator OptionValueValidator
        {
            get
            {
                return savedInstance.OptionValueValidator;
            }
            set
            {
                savedInstance.OptionValueValidator = value;
            }
        }

        internal override bool Set
        {
            set
            {
                savedInstance.Set = value;
                //base.Set = value;
            }
            get
            {
                return savedInstance.Set;
            }
        }
    }

    public static class CommandExtensions
    {
        public static Command Add(this Command com, Action<Command> action)
        {
            action(com);
            return com;
        }
    }
}