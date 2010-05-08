namespace MPOptions.Validator
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

            validator.Validate();
        }

        internal class NullValidator : Validator<string>
        {
            internal NullValidator()
                : base(null)
            { }
        }

    }
}