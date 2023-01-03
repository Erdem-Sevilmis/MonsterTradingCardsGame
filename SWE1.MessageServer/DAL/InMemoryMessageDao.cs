using SWE1.MessageServer.Models;

namespace SWE1.MessageServer.DAL
{
    internal class InMemoryMessageDao : IMessageDao
    {
        private int _nextId = 1;
        private readonly Dictionary<string, List<Message>> _userMessages = new();

        public void DeleteMessage(string username, int messageId)
        {
            var foundMessage = GetMessageById(username, messageId);
            if (foundMessage != null)
            {
                if (_userMessages.TryGetValue(username, out var messages))
                {
                    messages.Remove(foundMessage);
                    if (messages.Count == 0)
                    {
                        _userMessages.Remove(username);
                    }
                }
            }
        }

        public Message? GetMessageById(string username, int messageId)
        {
            return GetMessages(username).SingleOrDefault(m => m.Id == messageId);
        }

        public IEnumerable<Message> GetMessages(string username)
        {
            return _userMessages.TryGetValue(username, out var messages) ? messages : Enumerable.Empty<Message>();
        }

        public void InsertMessage(string username, Message message)
        {
            if (GetMessageById(username, message.Id) == null)
            {
                if (!_userMessages.TryGetValue(username, out var messages))
                {
                    messages = new List<Message>();
                    _userMessages.Add(username, messages);
                }
                message.Id = _nextId++;
                messages.Add(message);
            }
        }

        public bool UpdateMessage(string username, Message message)
        {
            var foundMessage = GetMessageById(username, message.Id);
            if (foundMessage != null)
            {
                foundMessage.Content = message.Content;
                return true;
            }
            return false;
        }
    }
}
