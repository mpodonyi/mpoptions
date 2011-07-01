namespace MPOptions.Result
{
    internal interface ICommandResultInternal : ICommandResult
    {
        new ICommandResultCollectionInternal Commands
        { get; }

        new IArgumentResultCollectionInternal Arguments
        { get; }

        new IOptionResultCollectionInternal Options
        { get; }

        new bool IsSet
        {
            get;
            set;
        }

        ResultStateBag ResultStateBag
        { get; }
        

       
    }
}