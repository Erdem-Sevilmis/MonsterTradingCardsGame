using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SWE1.MessageServer.Core.Routing
{
    public class RouteNotAuthenticatedException : Exception
    {
        public RouteNotAuthenticatedException()
        {
        }

        public RouteNotAuthenticatedException(string? message) : base(message)
        {
        }

        public RouteNotAuthenticatedException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected RouteNotAuthenticatedException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
