namespace MPOptions.Result
{
    internal class ResultStateBag
    {
        internal ParserErrorContext ErrorContext
        { get; set; }

        internal bool HasError
        {
            get
            {
                return ErrorContext != null;
            } 
        }


    }
}
