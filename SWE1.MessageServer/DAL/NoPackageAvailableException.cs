using System.Runtime.Serialization;

namespace SWE1.MessageServer.DAL
{
    [Serializable]
    internal class NoPackageAvailableException : Exception
    {
        public NoPackageAvailableException()
        {
        }

        public NoPackageAvailableException(string? message) : base(message)
        {
        }

        public NoPackageAvailableException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected NoPackageAvailableException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}