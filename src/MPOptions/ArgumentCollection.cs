using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace MPOptions
{
    public class ArgumentCollection : IEnumerable<Argument>, ICollection
    {
        private Command command;

        internal ArgumentCollection(Command command)
        {
            this.command = command;
        }

        public Argument this[string name]
        {
            get
            {
                return (from obj in command.StateBag.Arguments.Values
                        where obj.ParentCommand == command  && obj.Name == name
                        select obj).SingleOrDefault();
            }
        }

        //public Option this[string name]
        //{
        //    get
        //    {
        //        return (from obj in command.StateBag.Options.Values
        //               where obj.ParentCommand == command && obj.Name == name
        //               select obj).SingleOrDefault();
        //    }
        //}

        //public Option this[string name,bool withGlobal]
        //{
        //    get
        //    {
        //        Option opt = this[name];

        //        if (withGlobal && opt == null)
        //            opt = (from obj in command.StateBag.GlobalOptions.Values
        //                   where obj.Name == name
        //                   select obj).SingleOrDefault();

        //        return opt;
        //    }
        //}

        #region IEnumerable<Argument> Members

        public IEnumerator<Argument> GetEnumerator()
        {
            return (from obj in command.StateBag.Arguments.Values
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
            throw new NotImplementedException();
        }

        public int Count
        {
            get
            {
                return (from obj in command.StateBag.Arguments.Values
                        where obj.ParentCommand == command
                        select obj).Count();
            }
        }

        public bool IsSynchronized
        {
            get { throw new NotImplementedException(); }
        }

        public object SyncRoot
        {
            get { throw new NotImplementedException(); }
        }

        #endregion
    }
}