﻿using System.Runtime.Serialization;

namespace SWE1.MessageServer.API.RouteCommands.packages
{
    [Serializable]
    public class AtLeastOneCardInThePackageAlreadyExistsException : Exception
    {
        public AtLeastOneCardInThePackageAlreadyExistsException()
        {
        }

        public AtLeastOneCardInThePackageAlreadyExistsException(string? message) : base(message)
        {
        }

        public AtLeastOneCardInThePackageAlreadyExistsException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected AtLeastOneCardInThePackageAlreadyExistsException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}