using MPOptions.Parser;

namespace MPOptions.ElementTree
{
    internal class ArgumentCollection : CollectionAdapter<Argument>, IArgumentCollection
    {
        //private readonly StateBag _StateBag;

        internal ArgumentCollection(StateBag stateBag, string preKey)
            : base(stateBag.Arguments, preKey)
        {
            //_StateBag = stateBag;
        }

        protected override void InsertItem(Argument item)
        {
            Validate(item);
            base.InsertItem(item);
        }

        private void Validate(Argument obj)
        {
            if (Count > 0)
            {
                //if (obj.ParentCommand.Arguments.Count() > 1 || obj.ParentCommand.Arguments.First() != obj)
                    ThrowHelper.ThrowArgumentException(ExceptionResource.Generic);
            }

        }





      




    }
}