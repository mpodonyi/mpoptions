﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MPOptions.Internal;

namespace MPOptions
{
    public class CommandCollection : IEnumerable<Command>, ICollection
    {
        private Command command;

        internal CommandCollection(Command command)
        {
            this.command = command;
        }

        public Command this[string name]
        {
            get
            {
                return (from obj in command.StateBag.Commands.Values
                       where obj.ParentCommand == command && obj.Name == name
                       select obj).SingleOrDefault();
            }
        }

        #region IEnumerable<Command> Members

        public IEnumerator<Command> GetEnumerator()
        {
            return (from obj in command.StateBag.Commands.Values
                    where obj.ParentCommand == command 
                    select obj).GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        #endregion

        #region ICollection Members

        public void CopyTo(Array array, int index)
        {
            ThrowHelper.ThrowNotImplementedException();
        }

        public int Count
        {
            get
            {
                return (from obj in command.StateBag.Commands.Values
                 where obj.ParentCommand == command
                 select obj).Count();
            }
        }

        public bool IsSynchronized
        {
            get { ThrowHelper.ThrowNotImplementedException();
                return true; }
        }

        public object SyncRoot
        {
            get { ThrowHelper.ThrowNotImplementedException(); return true; }
        }

        #endregion
    }
}
