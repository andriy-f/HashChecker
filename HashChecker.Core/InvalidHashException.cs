namespace HashChecker.Core
{
    using System;
    using System.Runtime.Serialization;

    public class InvalidHashException : Exception, ISerializable
    {
        public InvalidHashException()
        {
        }

        public InvalidHashException(string message)
            : base(message)
        {
        }

        public InvalidHashException(string message, Exception inner)
            : base(message, inner)
        {
        }

        // This constructor is needed for serialization.
        protected InvalidHashException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
