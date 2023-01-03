using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities;
using Moq;
using SWE1.MessageServer.BLL;
using SWE1.MessageServer.DAL;
using SWE1.MessageServer.Models;
using Message = SWE1.MessageServer.Models.Message;

namespace SWE1.MessageServer.Test
{
    internal class MessageManagerTest
    {
        [Test]
        public void AddMessageStoresMessageInRepository()
        {
            // arrange
            var messageRepo = new Mock<IMessageDao>();
            var messageManager = new MessageManager(messageRepo.Object);
            var user = new User("testusr", "testpwd");
            var content = "testcontent";

            // act
            messageManager.AddMessage(user, content);

            // assert
            messageRepo.Verify(m => m.InsertMessage(user.Username, It.Is<Message>(m => m.Content == content)));
        }

        [Test]
        public void ShowMessageWithInvalidIdThrowsMessageNotFoundException()
        {
            // arrange
            var messageRepo = new Mock<IMessageDao>();
            var messageManager = new MessageManager(messageRepo.Object);
            var user = new User("testusr", "testpwd");
            var invalidId = 1;

            messageRepo.Setup(m => m.GetMessageById(user.Username, invalidId)).Returns((Message?)null);

            // act and assert
            Assert.That(() => messageManager.ShowMessage(user, invalidId), Throws.TypeOf<MessageNotFoundException>());
        }

        [Test]
        public void ShowMessageWithValidIdReturnsMessage()
        {
            // arrange
            var messageRepo = new Mock<IMessageDao>();
            var messageManager = new MessageManager(messageRepo.Object);
            var user = new User("testusr", "testpwd");
            var expectedMessage = new Message(1, "test"); ;

            messageRepo.Setup(m => m.GetMessageById(user.Username, expectedMessage.Id)).Returns(expectedMessage);

            // act
            var returnedMessage = messageManager.ShowMessage(user, expectedMessage.Id);

            // assert
            Assert.That(returnedMessage, Is.EqualTo(expectedMessage));
        }

        [Test]
        public void ListMessagesWithValidUserReturnsUserMessages()
        {
            // arrange
            var messageRepo = new Mock<IMessageDao>();
            var messageManager = new MessageManager(messageRepo.Object);
            var user = new User("test", "pwd");
            var expectedMessages = new List<Message>()
            {
                new Message(1, "test1"),
                new Message(2, "test2"),
                new Message(3, "test3")
            };
            messageRepo.Setup(m => m.GetMessages(It.IsAny<string>())).Returns(expectedMessages);

            // act
            var actualMessages = messageManager.ListMessages(user);

            // assert
            Assert.That(actualMessages, Is.EquivalentTo(expectedMessages));
        }
    }
}
