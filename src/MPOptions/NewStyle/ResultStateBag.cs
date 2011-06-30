using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MPOptions.NewStyle
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
