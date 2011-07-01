namespace MPOptions.NewStyle
{
    public interface ICommandResult
    {
        ICommandResultCollection Commands
        { get; }

        IArgumentResultCollection Arguments
        { get; }

        IOptionResultCollection Options
        { get; }




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