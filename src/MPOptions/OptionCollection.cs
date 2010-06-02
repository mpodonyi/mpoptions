using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MPOptions.Internal;

namespace MPOptions
{
    public class OptionCollection : IEnumerable<Option>, ICollection
    {
        private Command command;

        internal OptionCollection(Command command)
        {
            this.command = command;
        }

        public Option this[string name]
        {
            get
            {
                var option = (from obj in command.StateBag.Options.Values
                        where (obj.ParentCommand == command || obj.IsGlobalOption) && obj.Name == name
                        select new OptionImpl(obj,command)).SingleOrDefault();
                //if (option!=null)
                //    option.ContextParent = command;

                return option;
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

        #region IEnumerable<Option> Members

        public IEnumerator<Option> GetEnumerator()
        {
            var options= (from obj in command.StateBag.Options.Values
                    where (obj.ParentCommand == command) ||
                          (obj.IsGlobalOption)
                    select new OptionImpl(obj,command));
            //foreach (Option option in options)
            //{
            //    option.ContextParent = command;
            //    yield return option;
            //}

            return options.OfType<Option>().GetEnumerator();
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
                return (from obj in command.StateBag.Options.Values
                        where (obj.ParentCommand == command) ||
                              (obj.IsGlobalOption)
                        select obj).Count();
            }
        }

        public bool IsSynchronized
        {
            get { ThrowHelper.ThrowNotImplementedException();
                return true;}
        }

        public object SyncRoot
        {
            get { ThrowHelper.ThrowNotImplementedException();
                return true;}
        }

        #endregion
    }
}
