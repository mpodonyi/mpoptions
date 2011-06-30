using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using MPOptions.Internal;
using MPOptions.NewStyle;

namespace MPOptions
{
    public interface ICommandCollection : IMPOptionCollection<Command>
    {
        //bool Contains(string key);

        //bool Remove(string key);

        //Command this[string key]
        //{ get; }

        //new int Count { get; }


    }


    class CommandCollection : CollectionAdapter<Command>, ICommandCollection
    {
        private readonly StateBag _StateBag;

        internal CommandCollection(StateBag stateBag, string preKey)
            : base(stateBag.Commands, preKey)
        {
            _StateBag = stateBag;
        }

        protected override void InsertItem(Command item)
        {
            Validate(item);
            base.InsertItem(item);
        }

        private const string TokenRegex = @"^((\s*\w+\s*)|(\s*\w+\s*;\s*)|(\s*\w+(\s*;\s*\w+\s*(;)?\s*)*))$";

        private void Validate(Command obj)
        {
            //Test that CommandValues meet the RegexRequirements
            if (!Regex.IsMatch(obj.Token, TokenRegex))
            {
                ThrowHelper.ThrowArgumentException(ExceptionResource.Argument_InValidForm, ExceptionArgument.token);
            }

            ////Test that no sibling has same Name
            //var name = from ii in obj.ParentCommand.Commands
            //           where ii.Name==obj.Name
            //           select ii;
            //if(name.Count()>0)
            //{
            //    ThrowHelper.ThrowArgumentException(ExceptionResource.Argument_AlreadyInDictionary, ExceptionArgument.name);
            //}

            //Test that no sibling has same Token
            var strings = from ii in this
                          from iii in ii.Token.SplitInternal()
                          from iiii in obj.Token.SplitInternal()
                          where iii == iiii
                          select iiii;
            if (strings.Count() > 0)
            {
                ThrowHelper.ThrowArgumentException(ExceptionResource.Argument_TokenPartAlreadyInDictionary, ExceptionArgument.token);
            }
        }
    }

    //public class CommandCollection : IEnumerable<Command>, ICollection
    //{
    //    private Command command;

    //    internal CommandCollection(Command command)
    //    {
    //        this.command = command;
    //    }

    //    public Command this[string name]
    //    {
    //        get
    //        {
    //            return (from obj in command.StateBag.Commands.Values
    //                   where obj.ParentCommand == command && obj.Name == name
    //                   select obj).SingleOrDefault();
    //        }
    //    }

    //    #region IEnumerable<Command> Members

    //    public IEnumerator<Command> GetEnumerator()
    //    {
    //        return (from obj in command.StateBag.Commands.Values
    //                where obj.ParentCommand == command 
    //                select obj).GetEnumerator();
    //    }

    //    #endregion

    //    #region IEnumerable Members

    //    System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
    //    {
    //        return this.GetEnumerator();
    //    }

    //    #endregion

    //    #region ICollection Members

    //    public void CopyTo(Array array, int index)
    //    {
    //        ThrowHelper.ThrowNotImplementedException();
    //    }

    //    public int Count
    //    {
    //        get
    //        {
    //            return (from obj in command.StateBag.Commands.Values
    //             where obj.ParentCommand == command
    //             select obj).Count();
    //        }
    //    }

    //    public bool IsSynchronized
    //    {
    //        get { ThrowHelper.ThrowNotImplementedException();
    //            return true; }
    //    }

    //    public object SyncRoot
    //    {
    //        get { ThrowHelper.ThrowNotImplementedException(); return true; }
    //    }

    //    #endregion
    //}
}
