using System;

namespace MPOptions.Parser
{
    internal enum ExceptionArgument
    {
        token,
        name,
        staticoptionvalue,
        argumentvalidator,
        optionvaluevalidator,
    }

    internal enum ExceptionResource
    {
        Argument_InValidForm,
        Argument_AlreadyInDictionary,
        MoreThenOneRegularExpression,
        DoubleStaticValue,
        Argument_TokenPartAlreadyInDictionary,
        Maximumoccurenceminimum,

        Generic
    }

    internal static class ThrowHelper
    {
        //internal static void ThrowArgumentException(ExceptionResource resource, ExceptionArgument argument, string value)
        //{
        //    throw new ArgumentException(string.Format(GetResource(resource), GetArgumentName(argument), value), GetArgumentName(argument));
        //}

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

        internal static void ThrowParserException(ParserErrorContext parserErrorContext)
        {
            throw new ParserException(parserErrorContext);
        }

        internal static void ThrowArgumentOutOfRangeException(ExceptionResource resource,int value)
        {
            throw new ArgumentOutOfRangeException("value",value,GetResource(resource));
        }

        private static string GetArgumentName(ExceptionArgument argument)
        {
            switch (argument)
            {
                case     ExceptionArgument.token:
                    return "token";

                case     ExceptionArgument.name:
                    return "name";
                case ExceptionArgument.argumentvalidator:
                    return "argumentValidator";



                case     ExceptionArgument.staticoptionvalue:
                    return "staticoptionvalidator";
               
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
                    return "A Part or the whole of the argument \"{0}\" is already in dictionary.";
                case ExceptionResource.MoreThenOneRegularExpression:
                    return "Option with RegularExpressionValidator and Same Token Exist already.";
                case ExceptionResource.DoubleStaticValue:
                    return "Option with same token and same StaticValue exist already.";
                case ExceptionResource.Maximumoccurenceminimum:
                    return "Value can not be less then 1.";
                    
            }
            return string.Empty;
        }
    }
}