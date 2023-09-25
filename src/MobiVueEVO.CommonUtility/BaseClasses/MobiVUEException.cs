using System;

namespace MobiVUE
{
    public class MobiVUEException : Exception
    {
        public MobiVUEException()
            : base()
        {
        }

        public MobiVUEException(string message)
            : base(message)
        {
        }

        public MobiVUEException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}