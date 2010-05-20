using System;

namespace MPOptions.Internal
{
    internal enum ExceptionArgument
    {
        token,
        name
    }

    internal enum ExceptionResource
    {
        Argument_InValidForm,
        Argument_AlreadyInDictionary,
    }

    internal static class ThrowHelper
    {
        internal static void ThrowArgumentException(ExceptionResource resource, ExceptionArgument argument)
        {
            throw new ArgumentException();
        }

        internal static void ThrowArgumentNullException(ExceptionArgument argument)
        {
            throw new ArgumentNullException();
        }

        internal static void ThrowArgumentOutOfRangeException(ExceptionArgument argument)
        {
            throw new ArgumentOutOfRangeException();
        }


        //private static string GetArgumentName(ExceptionArgument argument)
        //{
        //    switch (argument)
        //    {
        //        case ExceptionArgument.token:
        //            return "token";
        //        case ExceptionArgument.name:
        //            return "name";
        //    }
        //    return string.Empty;
        //}

        //private static string GetResourceName(ExceptionResource resource)
        //{
        //    switch (resource)
        //    {
        //        case ExceptionResource.Argument_ImplementIComparable:
        //            return "Argument_ImplementIComparable";
        //    }
        //    return string.Empty;
        //}

        internal static void ThrowParserError(ParserErrorContext parserErrorContext)
        {
            throw new ParserException(parserErrorContext);
        }
    }
}