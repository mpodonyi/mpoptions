using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace MPOptions
{
    public class ParserException: Exception, ISerializable
    {
        internal ParserException(ParserErrorContext parserErrorContext)
        {
            ErrorContext = parserErrorContext;
        }

        public ParserErrorContext ErrorContext
        {
            get; private set;
        }

        internal ParserException()
        {
            // Add implementation.
        }
        
        internal ParserException(string message)
        {
            // Add implementation.
        }
        
        internal ParserException(string message, Exception inner)
        {
            // Add implementation.
        }

        // This constructor is needed for serialization.
        internal ParserException(SerializationInfo info, StreamingContext context)
        {
            // Add implementation.
        }
    
        #region ISerializable Members

        void  ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}