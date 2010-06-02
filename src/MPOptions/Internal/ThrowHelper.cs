using System;

namespace MPOptions.Internal
{
    internal enum ExceptionArgument
    {
        token,
        name,
        staticoptionvalue,
        argumentvalidator,
        optionvaluevalidator,
        tokenpart
    }

    internal enum ExceptionResource
    {
        Argument_InValidForm,
        Argument_AlreadyInDictionary,
        MoreThenOneRegularExpression,
        DoubleStaticValue,
        Argument_NameAlreadyInDictionary,
        Argument_TokenPartAlreadyInDictionary,

        Generic
    }

    internal static class ThrowHelper
    {
        internal static void ThrowArgumentException(ExceptionResource resource, ExceptionArgument argument, string value)
        {
            throw new ArgumentException(string.Format(GetResource(resource), GetArgumentName(argument), value), GetArgumentName(argument));
        }

        //internal static void ThrowArgumentException(ExceptionResource resource, string value)
        //{
        //    throw new ArgumentException(string.Format(GetResource(resource), value));
        //}

        internal static void ThrowArgumentException(ExceptionResource resource, ExceptionArgument argument)
        {
            throw new ArgumentException(string.Format(GetResource(resource), GetArgumentName(argument)), GetArgumentName(argument));
        }

        internal static void ThrowArgumentException(ExceptionResource resource)
        {
            throw new ArgumentException(GetResource(resource));
        }



       

       

        internal static void ThrowArgumentNullException(ExceptionArgument argument)
        {
            throw new ArgumentNullException(GetArgumentName(argument));
        }

        internal static void ThrowNotImplementedException()
        {
            throw new NotImplementedException();
        }

        internal static void ThrowParserError(ParserErrorContext parserErrorContext)
        {
            throw new ParserException(parserErrorContext);
        }

        //internal static void ThrowArgumentException(ExceptionResource resource, ExceptionArgument argument,string value)
        //{
        //    throw new ArgumentException();
        //}

        

        //internal static void ThrowArgumentOutOfRangeException(ExceptionArgument argument)
        //{
        //    throw new ArgumentOutOfRangeException();
        //}


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

        private static string GetArgumentName(ExceptionArgument argument)
        {
            switch (argument)
            {
                case     ExceptionArgument.token:
                    return "token";

                case     ExceptionArgument.name:
                    return "name";



                case     ExceptionArgument.staticoptionvalue:
                    return "staticoptionvalidator";
                case     ExceptionArgument.argumentvalidator:
                    return "argumentvalidator";
                case     ExceptionArgument.optionvaluevalidator:
                    return "optionvaluevalidator";
            }
            return string.Empty;
        }

        private static string GetResource(ExceptionResource resource)
        {
            switch (resource)
            {
                case ExceptionResource.Argument_InValidForm:
                    return "The Argument \"{0}\" is in Invalid Form.";
                case ExceptionResource.Argument_AlreadyInDictionary:
                    return "The Argument \"{0}\" is already in dictionary.";
                case ExceptionResource.Argument_TokenPartAlreadyInDictionary:
                    return "A Part or the whole of the argument Token with value \"{1}\" is already in dictionary.";
            }
            return string.Empty;
        }


        //private static string GetString(ExceptionResource resource,ExceptionArgument token, string value)
        //{
        //    string retVal=string.Empty;

        //    switch (resource)
        //    {
        //        case ExceptionResource.Argument_InValidForm:
        //            retVal="The Argument {0} with the value {1} is in Invalid Form.";
        //            break;
        //        case ExceptionResource.Argument_AlreadyInDictionary:
        //            retVal = "Tha value or parts ";
        //            break;

        //    }

        //    return string.Format(retVal, GetArgumentName(token), value);
        //}

       
    }
}