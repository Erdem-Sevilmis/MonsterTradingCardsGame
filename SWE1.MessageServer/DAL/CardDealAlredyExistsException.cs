using System.Runtime.Serialization;

namespace SWE1.MessageServer.DAL
{
    [Serializable]
    public class CardDealAlredyExistsException : Exception
    {
        public CardDealAlredyExistsException()
        {
        }

        public CardDealAlredyExistsException(string? message) : base(message)
        {
        }

        public CardDealAlredyExistsException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected CardDealAlredyExistsException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}