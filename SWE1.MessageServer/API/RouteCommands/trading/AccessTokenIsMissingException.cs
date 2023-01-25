using System.Runtime.Serialization;

namespace SWE1.MessageServer.API.RouteCommands.trading
{
    [Serializable]
    internal class AccessTokenIsMissingException : Exception
    {
        public AccessTokenIsMissingException()
        {
        }

        public AccessTokenIsMissingException(string? message) : base(message)
        {
        }

        public AccessTokenIsMissingException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected AccessTokenIsMissingException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}