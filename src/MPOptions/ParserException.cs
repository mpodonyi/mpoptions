using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using MPOptions.Parser;

namespace MPOptions
{
    public class ParserException: Exception, ISerializable
    {
        internal ParserException(ParserErrorContext parserErrorContext):base(parserErrorContext.ToString())
        {
            ErrorContext = parserErrorContext;
        }

        public ParserErrorContext ErrorContext
        {
            get; private set;
        }


        // This constructor is needed for serialization.
        internal ParserException(SerializationInfo info, StreamingContext context)
        {
            // Add implementation.
        }
    
        #region ISerializable Members

        void  ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
        {
            ThrowHelper.ThrowNotImplementedException();
        }

        #endregion
    }
}