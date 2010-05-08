namespace MPOptions
{
    public interface IArgumentValidator
    {
        bool IsMatch(string value);

        int MaximumOccurrence
        { get; }
    }
}