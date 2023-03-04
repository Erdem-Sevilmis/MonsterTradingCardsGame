using MonsterTradingCardsGame.SWE1.MessageServer.Models.Card;
using MonsterTradingCardsGame.SWE1.MessageServer.Models.User;
using Moq;
using Newtonsoft.Json;
using SWE1.MessageServer.API.RouteCommands.cards;
using SWE1.MessageServer.API.RouteCommands.Users;
using SWE1.MessageServer.BLL.cards;
using SWE1.MessageServer.BLL.user;
using SWE1.MessageServer.Core.Response;
using SWE1.MessageServer.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWE1.MessageServer.Test
{
    internal class CardTest
    {
        CardsManager cardManager;
        DatabaseCardDao cardDao;

        [SetUp]
        public void SetUp()
        {
            cardDao = new DatabaseCardDao();
            cardManager = new CardsManager();
        }

        [Test]
        public void GetCardsSuccess()
        {
            User user = new User("testusr", "testpwd", 0);
            List<Card> userCards = new List<Card>();
            Mock<ICardsManager> cardManagerMock = new Mock<ICardsManager>();
            GetCardsCommand rc = new GetCardsCommand(user, cardManagerMock.Object);

            cardManagerMock.Setup(x => x.GetUserCards(It.IsAny<User>())).Returns(userCards);
            var response = rc.Execute();

            cardManagerMock.Verify(x => x.GetUserCards(user));
            Assert.That(response.StatusCode, Is.EqualTo(StatusCode.Ok));
            Assert.That(response.Payload, Is.EqualTo(JsonConvert.SerializeObject(userCards.Select(card => new { id = card.Id, name = card.Name, damage = card.Damage, element = card.ElementType }).ToArray())));
        }

        [Test]
        public void GetCardsFail()
        {
            User user = new User("testusr", "testpwd", 0);
            Mock<ICardsManager> cardManagerMock = new Mock<ICardsManager>();
            GetCardsCommand rc = new GetCardsCommand(user, cardManagerMock.Object);

            cardManagerMock.Setup(x => x.GetUserCards(It.IsAny<User>())).Throws<NoCardsException>();
            var response = rc.Execute();

            cardManagerMock.Verify(x => x.GetUserCards(user));
            Assert.That(response.StatusCode, Is.EqualTo(StatusCode.NoContent));
            Assert.That(response.Payload, Is.EqualTo(null));
        }

        [Test]
        public void GetDeckJsonSuccess()
        {
            User user = new User("testusr", "testpwd", 0);
            List<Card> userCards = new List<Card> { new Card(MonsterTradingCardsGame.DataBase.Card_Type.WaterGoblin, 0.0, new Guid()) };
            Mock<ICardsManager> cardManagerMock = new Mock<ICardsManager>();
            GetDeckCommand rc = new GetDeckCommand(user, cardManagerMock.Object, "json");

            cardManagerMock.Setup(x => x.GetUserDeck(It.IsAny<User>())).Returns(userCards);
            var response = rc.Execute();

            cardManagerMock.Verify(x => x.GetUserDeck(user));
            Assert.That(response.StatusCode, Is.EqualTo(StatusCode.Ok));
            Assert.That(response.Payload, Is.EqualTo(JsonConvert.SerializeObject(userCards.Select(card => new { id = card.Id, name = card.Name, damage = card.Damage, element = card.ElementType }).ToArray())));
        }

        [Test]
        public void GetDeckJsonFailNotEnoughCards()
        {
            User user = new User("testusr", "testpwd", 0);
            Mock<ICardsManager> cardManagerMock = new Mock<ICardsManager>();
            GetDeckCommand rc = new GetDeckCommand(user, cardManagerMock.Object, "json");

            cardManagerMock.Setup(x => x.GetUserDeck(It.IsAny<User>())).Throws<NotEnoughCardsinDeckException>();
            var response = rc.Execute();

            cardManagerMock.Verify(x => x.GetUserDeck(user));
            Assert.That(response.StatusCode, Is.EqualTo(StatusCode.BadRequest));
            Assert.That(response.Payload, Is.EqualTo(null));
        }

        [Test]
        public void GetDeckJsonFailDeckEmpty()
        {
            User user = new User("testusr", "testpwd", 0);
            Mock<ICardsManager> cardManagerMock = new Mock<ICardsManager>();
            GetDeckCommand rc = new GetDeckCommand(user, cardManagerMock.Object, "json");

            cardManagerMock.Setup(x => x.GetUserDeck(It.IsAny<User>())).Throws<DeckEmptyException>();
            var response = rc.Execute();

            cardManagerMock.Verify(x => x.GetUserDeck(user));
            Assert.That(response.StatusCode, Is.EqualTo(StatusCode.NoContent));
            Assert.That(response.Payload, Is.EqualTo(null));
        }

        [Test]
        public void GetDeckPlainSuccess()
        {
            User user = new User("testusr", "testpwd", 0);
            List<Card> userCards = new List<Card> { new Card(MonsterTradingCardsGame.DataBase.Card_Type.WaterGoblin, 0.0, new Guid()) };
            Mock<ICardsManager> cardManagerMock = new Mock<ICardsManager>();
            GetDeckCommand rc = new GetDeckCommand(user, cardManagerMock.Object, "plain");
            string message = "Your deck includes:\n";
            foreach (var card in userCards)
            {
                message += "\t" + card.ToString() + "\n";
            }

            cardManagerMock.Setup(x => x.GetUserDeck(It.IsAny<User>())).Returns(userCards);
            var response = rc.Execute();

            cardManagerMock.Verify(x => x.GetUserDeck(user));
            Assert.That(response.StatusCode, Is.EqualTo(StatusCode.Ok));
            Assert.That(response.Payload, Is.EqualTo(message));
        }

        [Test]
        public void GetDeckPlainFailNotEnoughCards()
        {
            User user = new User("testusr", "testpwd", 0);
            Mock<ICardsManager> cardManagerMock = new Mock<ICardsManager>();
            GetDeckCommand rc = new GetDeckCommand(user, cardManagerMock.Object, "plain");

            cardManagerMock.Setup(x => x.GetUserDeck(It.IsAny<User>())).Throws<NotEnoughCardsinDeckException>();
            var response = rc.Execute();

            cardManagerMock.Verify(x => x.GetUserDeck(user));
            Assert.That(response.StatusCode, Is.EqualTo(StatusCode.BadRequest));
            Assert.That(response.Payload, Is.EqualTo(null));
        }

        [Test]
        public void GetDeckPlainFailDeckEmpty()
        {
            User user = new User("testusr", "testpwd", 0);
            Mock<ICardsManager> cardManagerMock = new Mock<ICardsManager>();
            GetDeckCommand rc = new GetDeckCommand(user, cardManagerMock.Object, "plain");

            cardManagerMock.Setup(x => x.GetUserDeck(It.IsAny<User>())).Throws<DeckEmptyException>();
            var response = rc.Execute();

            cardManagerMock.Verify(x => x.GetUserDeck(user));
            Assert.That(response.StatusCode, Is.EqualTo(StatusCode.NoContent));
            Assert.That(response.Payload, Is.EqualTo(null));
        }

        [Test]
        public void ConfigureDeckSuccess()
        {
            User user = new User("testusr", "testpwd", 0);
            Guid[] cardIds = new Guid[] {  new Guid() };
            Mock<ICardsManager> cardManagerMock = new Mock<ICardsManager>();
            ConfigureDeckCommand rc = new ConfigureDeckCommand(user, cardManagerMock.Object, cardIds);

            cardManagerMock.Setup(x => x.ConfigureNewDeck(It.IsAny<User>(), It.IsAny<Guid[]>())).Returns(true);
            var response = rc.Execute();

            cardManagerMock.Verify(x => x.ConfigureNewDeck(user, cardIds));
            Assert.That(response.StatusCode, Is.EqualTo(StatusCode.Ok));
            Assert.That(response.Payload, Is.EqualTo(null));
        }

        [Test]
        public void ConfigureDeckFailNotEnoughCards()
        {
            User user = new User("testusr", "testpwd", 0);
            Guid[] cardIds = new Guid[] { new Guid() };
            Mock<ICardsManager> cardManagerMock = new Mock<ICardsManager>();
            ConfigureDeckCommand rc = new ConfigureDeckCommand(user, cardManagerMock.Object, cardIds);

            cardManagerMock.Setup(x => x.ConfigureNewDeck(It.IsAny<User>(), It.IsAny<Guid[]>())).Throws<NotEnoughCardsinDeckException>();
            var response = rc.Execute();

            cardManagerMock.Verify(x => x.ConfigureNewDeck(user, cardIds));
            Assert.That(response.StatusCode, Is.EqualTo(StatusCode.BadRequest));
            Assert.That(response.Payload, Is.EqualTo(null));
        }
       
        [Test]
        public void ConfigureDeckFailCardNotOwnedOrUnavailable()
        {
            User user = new User("testusr", "testpwd", 0);
            Guid[] cardIds = new Guid[] { new Guid() };
            Mock<ICardsManager> cardManagerMock = new Mock<ICardsManager>();
            ConfigureDeckCommand rc = new ConfigureDeckCommand(user, cardManagerMock.Object, cardIds);

            cardManagerMock.Setup(x => x.ConfigureNewDeck(It.IsAny<User>(), It.IsAny<Guid[]>())).Throws<CardNotOwnedOrUnavailableException>();
            var response = rc.Execute();

            cardManagerMock.Verify(x => x.ConfigureNewDeck(user, cardIds));
            Assert.That(response.StatusCode, Is.EqualTo(StatusCode.Forbidden));
            Assert.That(response.Payload, Is.EqualTo(null));
        }
    }
}
