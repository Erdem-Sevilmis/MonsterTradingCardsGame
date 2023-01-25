using System.Runtime.Serialization;

namespace SWE1.MessageServer.API.RouteCommands.trading
{
    [Serializable]
    internal class NoTradsAvailbleException : Exception
    {
        public NoTradsAvailbleException()
        {
        }

        public NoTradsAvailbleException(string? message) : base(message)
        {
        }

        public NoTradsAvailbleException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected NoTradsAvailbleException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}