using System.Linq;
using System.Text.RegularExpressions;
using MPOptions.Parser;

namespace MPOptions.ElementTree
{
    internal class CommandCollection : CollectionAdapter<Command>, ICommandCollection
    {
        //private readonly StateBag _StateBag;

        internal CommandCollection(StateBag stateBag, string preKey)
            : base(stateBag.Commands, preKey)
        {
            //_StateBag = stateBag;
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

  
}
