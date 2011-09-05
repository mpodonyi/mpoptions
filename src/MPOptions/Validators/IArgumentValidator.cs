namespace MPOptions.Validators
{
    public interface IArgumentValidator
    {
        bool IsMatch(string value);

        //int MaximumOccurrence
        //{ get; }
    }
}