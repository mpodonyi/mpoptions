namespace MPOptions.NewStyle
{
    public interface IArgumentResult
    {
        bool IsSet
        { get; }

        string Name
        {
            get;
        }

        string[] Values
        {
            get;
        }

        string Value
        {
            get;
        }
    }
}