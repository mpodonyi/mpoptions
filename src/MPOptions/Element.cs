﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MPOptions.ElementTree;
using MPOptions.Parser;

namespace MPOptions
{
    public abstract class Element
    {
        internal Element(string name)
        {
            this.Name = name;
            ReadOnly = false;
        }

        private bool ReadOnly = false;

        internal void SetReadOnly()
        {
            ReadOnly = true;
        }

        protected void ThrowErrorWhenReadOnly()
        {
            if (ReadOnly)
                throw new InvalidOperationException("Can't change object after adding to Command tree.");
        }

        //internal Element(StateBag stateBag, Command parentCommand, string name)
        //{
        //  //  ParentCommand = parentCommand;
        //    ContextParent = parentCommand;
        //    StateBag = stateBag;
        //    this.Name = name;
        //}

        //protected Command ContextParent
        //{
        //    private get;
        //    set;
        //}

        //internal StateBag StateBag
        //{
        //    get; set;
        //}

        public string Name
        {
            get;
            private set;
        }
    }
}
