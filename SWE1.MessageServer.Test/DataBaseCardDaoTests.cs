using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities;
using MonsterTradingCardsGame.SWE1.MessageServer.Models.Card;
using MonsterTradingCardsGame;
using MonsterTradingCardsGame.SWE1.MessageServer.Models.User;
using Moq;
using NUnit.Framework;
using SWE1.MessageServer.BLL;
using SWE1.MessageServer.DAL;
using SWE1.MessageServer.Models;

namespace SWE1.MessageServer.Test
{
    [TestClass]
    public class DataBaseCardDaoTests
    {
        [Test]
        public void TestGetUserCards()
        {
            var dbCardDao = new DatabaseCardDao();
            var expectedCards = new List<Guid> { Guid.NewGuid(), Guid.NewGuid() };
            // setup mock data to return expected cards
            var actualCards = dbCardDao.GetUserCards("test_user");
            Assert.AreEqual(expectedCards, actualCards);
        }
        [Test]
        public void TestGetUserDeck()
        {
            var dbCardDao = new DatabaseCardDao();
            var expectedCards = new List<Card> { new Card(DataBase.Card_Type.Fire, 10, Guid.NewGuid(), ElementType.Fire) };
            // setup mock data to return expected cards
            var actualCards = dbCardDao.GetUserDeck("test_user");
            Assert.AreEqual(expectedCards, actualCards);
        }
        [Test]
        public void TestGetCard()
        {
            var dbCardDao = new DatabaseCardDao();
            var expectedCard = new Card(DataBase.Card_Type.Fire, 10, Guid.NewGuid(), ElementType.Fire);
            // setup mock data to return expected card
            var actualCard = dbCardDao.GetCard(expectedCard.Id);
            Assert.AreEqual(expectedCard, actualCard);
        }
        [Test]
        public void TestGetUserStack()
        {
            var dbCardDao = new DatabaseCardDao();
            var expectedStack = new List<Guid> { Guid.NewGuid(), Guid.NewGuid() };
            // setup mock data to return expected stack
            var actualStack = dbCardDao.GetUserStack("test_user");
            Assert.AreEqual(expectedStack, actualStack);
        }
        [Test]
public void TestConfigureNewDeckNotEnoughCards() {
    var dbCardDao = new DatabaseCardDao();
    var user = new User { Credentials = new Credentials { Username = "test_user" } };
    var cardIds = new Guid[] { Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid() };
    Assert.Throws<NotEnoughCardsinDeckException>(() => dbCardDao.ConfigureNewDeck(user, cardIds));
}
    }
}
