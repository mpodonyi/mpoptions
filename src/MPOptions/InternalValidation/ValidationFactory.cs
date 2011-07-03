using MPOptions.Parser;

namespace MPOptions.InternalValidation
{
    internal static class ValidationFactory
    {
        internal static void Validate<T>(T obj)
        {
            IValidator validator = new NullValidator();

            if (typeof(T) == typeof(Option))
            {
                validator = new OptionValidator(obj as Option);
            }
            else if (typeof(T) == typeof(Command))
            {
                validator = new CommandValidator(obj as Command);
            }
            else if (typeof(T) == typeof(Argument))
            {
                validator = new ArgumentValidator(obj as Argument);
            }
            //else if(typeof(T) == typeof(IEnumerable<Option>))
            //{
            //    validator = new OptionsValidator(obj as IEnumerable<Option>);
            //}

            validator.Validate();
        }

        internal static void PostValidate<T>(T obj)
        {
            IValidator validator = new NullValidator();

            if (typeof(T) == typeof(Command))
            {
                validator = new CommandValidator(obj as Command);
            }
           
            validator.PostValidate();
        }

        internal class NullValidator : Validator<string>
        {
            internal NullValidator()
                : base(null)
            { }
        }

    }
}