﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MPOptions
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

        private Option savedInstance;

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