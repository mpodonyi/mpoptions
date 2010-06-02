using System.Linq;
using System.Text.RegularExpressions;

namespace MPOptions.Internal
{
    internal class CommandValidator:Validator<Command>
    {
        internal CommandValidator(Command obj):base(obj)
        {}

        private const string TokenRegex = @"^((\s*\w+\s*)|(\s*\w+\s*;\s*)|(\s*\w+(\s*;\s*\w+\s*(;)?\s*)*))$";


        public override void Validate()
        {
            //Test that CommandValues meet the RegexRequirements
            if (!Regex.IsMatch(obj.Token, TokenRegex))
            {
                ThrowHelper.ThrowArgumentException(ExceptionResource.Argument_InValidForm, ExceptionArgument.token);
            }

            //Test that no sibling has same Name
            var name = from ii in obj.ParentCommand.Commands
                       where ii.Name==obj.Name
                       select ii;
            if(name.Count()>0)
            {
                ThrowHelper.ThrowArgumentException(ExceptionResource.Argument_AlreadyInDictionary, ExceptionArgument.name);
            }

            //Test that no sibling has same Token
            var strings = from ii in obj.ParentCommand.Commands
                          from iii in ii.Token.SplitInternal()
                          from iiii in obj.Token.SplitInternal()
                          where iii == iiii
                          select iiii;
            if(strings.Count()>0)
            {
                ThrowHelper.ThrowArgumentException(ExceptionResource.Argument_TokenPartAlreadyInDictionary, ExceptionArgument.token);
            }
        }
    }
}