namespace MPOptions
{
    public interface IOptionResult
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

        string Token
        {
            get;
        }
    }
}