namespace MPOptions
{
    public interface IOptionValueValidator
    {
        bool IsMatch(string value);

        int MaximumOccurrence
        { get; }

        bool ValueOptional
        { get; }
    }
}