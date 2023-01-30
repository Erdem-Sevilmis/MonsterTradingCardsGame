using System.Runtime.Serialization;

namespace SWE1.MessageServer.API.RouteCommands.packages
{
    [Serializable]
    internal class NotEnoughMoneyException : Exception
    {
        public NotEnoughMoneyException()
        {
        }

        public NotEnoughMoneyException(string? message) : base(message)
        {
        }

        public NotEnoughMoneyException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected NotEnoughMoneyException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}