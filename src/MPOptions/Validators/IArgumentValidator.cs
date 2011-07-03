namespace MPOptions.Validators
{
    internal interface IArgumentValidator
    {
        bool IsMatch(string value);

        int MaximumOccurrence
        { get; }
    }
}