using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MPOptions.Validator
{
    internal static class ThrowHelper
    {
        internal static void ThrowArgumentOutOfRangeException(ExceptionArgument argument)
        {
            throw new ArgumentOutOfRangeException(GetArgumentName(argument));
        }

        internal static void ThrowArgumentOutOfRangeException(ExceptionArgument argument, ExceptionResource resource)
        {
           // throw new ArgumentOutOfRangeException(GetArgumentName(argument), Environment.GetResourceString(GetResourceName(resource)));
        }

        private static string GetArgumentName(ExceptionArgument argument)
        {
            switch (argument)
            {
                case ExceptionArgument.token:
                    return "token";
                case ExceptionArgument.name:
                    return "name";
            }
            return string.Empty;
        }

        private static string GetResourceName(ExceptionResource resource)
        {
            switch (resource)
            {
                case ExceptionResource.Argument_ImplementIComparable:
                    return "Argument_ImplementIComparable";
            }
            return string.Empty;
        }

 

 

    }

    internal enum ExceptionArgument
    {
       token,
        name
    }

    internal enum ExceptionResource
    {
        Argument_ImplementIComparable,
    }

}
