namespace MPOptions
{
    public interface ICommandResult
    {
        ICommandResultCollection Commands
        { get; }

        IArgumentResultCollection Arguments
        { get; }

        IOptionResultCollection Options
        { get; }


        ICommandResult ChoosenCommand
        {
            get;
        }


        bool IsSet
        { get; }

        string Token
        {
            get;
        }


        string Name
        {
            get;
        }
    }
}