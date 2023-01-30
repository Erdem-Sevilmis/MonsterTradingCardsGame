using System.Runtime.Serialization;

namespace SWE1.MessageServer.API.RouteCommands.Users
{
    [Serializable]
    internal class UserNotAdminException : Exception
    {
        public UserNotAdminException()
        {
        }

        public UserNotAdminException(string? message) : base(message)
        {
        }

        public UserNotAdminException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected UserNotAdminException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}