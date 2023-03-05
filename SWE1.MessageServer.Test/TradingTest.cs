using MonsterTradingCardsGame.SWE1.MessageServer.Models.Card;
using MonsterTradingCardsGame.SWE1.MessageServer.Models.User;
using Moq;
using Newtonsoft.Json;
using SWE1.MessageServer.API.RouteCommands.cards;
using SWE1.MessageServer.API.RouteCommands.trading;
using SWE1.MessageServer.BLL.cards;
using SWE1.MessageServer.BLL.package;
using SWE1.MessageServer.BLL.trading;
using SWE1.MessageServer.Core.Response;
using SWE1.MessageServer.DAL;
using SWE1.MessageServer.Models;
using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWE1.MessageServer.Test
{
    internal class TradingTest
    {
        TradingManager tradingManager;
        DataBaseTradingDao tradingDao;

        [SetUp]
        public void SetUp()
        {
            tradingDao = new DataBaseTradingDao();
            tradingManager = new TradingManager();
        }

        [Test]
        public void CreateTradingDealSuccess()
        {
            User user = new User("testusr", "testpwd", 0);
            TradingDeal tradingDeal = new TradingDeal(new Guid(), new Guid(), TradingDeal.SpellMonster.spell, 0);
            Mock<ITradingManager> tradingManagerMock = new Mock<ITradingManager>();
            CreateTradingDealCommand rc = new CreateTradingDealCommand(user, tradingManagerMock.Object, tradingDeal);

            tradingManagerMock.Setup(x => x.CreateNewTradingDeal(It.IsAny<Credentials>(), It.IsAny<TradingDeal>())).Returns(true);
            var response = rc.Execute();


            tradingManagerMock.Verify(x => x.CreateNewTradingDeal(user.Credentials, tradingDeal));
            Assert.That(response.StatusCode, Is.EqualTo(StatusCode.Created));
            Assert.That(response.Payload, Is.EqualTo(null));
        }

        [Test]
        public void CreateTradingDealFailCardNotOwned()
        {
            User user = new User("testusr", "testpwd", 0);
            TradingDeal tradingDeal = new TradingDeal(new Guid(), new Guid(), TradingDeal.SpellMonster.spell, 0);
            Mock<ITradingManager> tradingManagerMock = new Mock<ITradingManager>();
            CreateTradingDealCommand rc = new CreateTradingDealCommand(user, tradingManagerMock.Object, tradingDeal);

            tradingManagerMock.Setup(x => x.CreateNewTradingDeal(It.IsAny<Credentials>(), It.IsAny<TradingDeal>())).Throws<CardNotOwnedOrInDeckException>();
            var response = rc.Execute();


            tradingManagerMock.Verify(x => x.CreateNewTradingDeal(user.Credentials, tradingDeal));
            Assert.That(response.StatusCode, Is.EqualTo(StatusCode.Forbidden));
            Assert.That(response.Payload, Is.EqualTo(null));
        }
        
        [Test]
        public void CreateTradingDealFailCardDealAlredyExist()
        {
            User user = new User("testusr", "testpwd", 0);
            TradingDeal tradingDeal = new TradingDeal(new Guid(), new Guid(), TradingDeal.SpellMonster.spell, 0);
            Mock<ITradingManager> tradingManagerMock = new Mock<ITradingManager>();
            CreateTradingDealCommand rc = new CreateTradingDealCommand(user, tradingManagerMock.Object, tradingDeal);

            tradingManagerMock.Setup(x => x.CreateNewTradingDeal(It.IsAny<Credentials>(), It.IsAny<TradingDeal>())).Throws<CardDealAlredyExistsException>();
            var response = rc.Execute();


            tradingManagerMock.Verify(x => x.CreateNewTradingDeal(user.Credentials, tradingDeal));
            Assert.That(response.StatusCode, Is.EqualTo(StatusCode.Conflict));
            Assert.That(response.Payload, Is.EqualTo(null));
        }

        [Test]
        public void DeleteTradingdealSuccess()
        {
            User user = new User("testusr", "testpwd", 0);
            Guid cardID = new Guid();
            Mock<ITradingManager> tradingManagerMock = new Mock<ITradingManager>();
            DeleteTradingdealCommand rc = new DeleteTradingdealCommand(user, tradingManagerMock.Object, cardID);

            tradingManagerMock.Setup(x => x.DeleteTradingDeal(It.IsAny<Credentials>(), It.IsAny<Guid>())).Returns(true);
            var response = rc.Execute();

            tradingManagerMock.Verify(x => x.DeleteTradingDeal(user.Credentials, cardID));
            Assert.That(response.StatusCode, Is.EqualTo(StatusCode.Ok));
            Assert.That(response.Payload, Is.EqualTo(null));
        }

        [Test]
        public void DeleteTradingdealFailCardNotOwned()
        {
            User user = new User("testusr", "testpwd", 0);
            Guid cardID = new Guid();
            Mock<ITradingManager> tradingManagerMock = new Mock<ITradingManager>();
            DeleteTradingdealCommand rc = new DeleteTradingdealCommand(user, tradingManagerMock.Object, cardID);

            tradingManagerMock.Setup(x => x.DeleteTradingDeal(It.IsAny<Credentials>(), It.IsAny<Guid>())).Throws<CardNotOwnedOrInDeckException>();
            var response = rc.Execute();

            tradingManagerMock.Verify(x => x.DeleteTradingDeal(user.Credentials, cardID));
            Assert.That(response.StatusCode, Is.EqualTo(StatusCode.Forbidden));
            Assert.That(response.Payload, Is.EqualTo(null));
        }
        
        [Test]
        public void DeleteTradingdealFailCardNotFound()
        {
            User user = new User("testusr", "testpwd", 0);
            Guid cardID = new Guid();
            Mock<ITradingManager> tradingManagerMock = new Mock<ITradingManager>();
            DeleteTradingdealCommand rc = new DeleteTradingdealCommand(user, tradingManagerMock.Object, cardID);

            tradingManagerMock.Setup(x => x.DeleteTradingDeal(It.IsAny<Credentials>(), It.IsAny<Guid>())).Throws<CardNotFoundException>();
            var response = rc.Execute();

            tradingManagerMock.Verify(x => x.DeleteTradingDeal(user.Credentials, cardID));
            Assert.That(response.StatusCode, Is.EqualTo(StatusCode.NotFound));
            Assert.That(response.Payload, Is.EqualTo(null));
        }
        
        [Test]
        public void GetAllTradingdealSuccess()
        {
            User user = new User("testusr", "testpwd", 0);
            List<TradingDeal> tradingDeals = new List<TradingDeal>();
            Mock<ITradingManager> tradingManagerMock = new Mock<ITradingManager>();
            GetAllTradingDealsCommand rc = new GetAllTradingDealsCommand(user, tradingManagerMock.Object);

            tradingManagerMock.Setup(x => x.GetTradingDeals(It.IsAny<Credentials>())).Returns(tradingDeals);
            var response = rc.Execute();

            tradingManagerMock.Verify(x => x.GetTradingDeals(user.Credentials));
            Assert.That(response.StatusCode, Is.EqualTo(StatusCode.Ok));
            Assert.That(response.Payload, Is.EqualTo(JsonConvert.SerializeObject(tradingDeals.Select(deal => new { Id = deal.Id, CardToTrade = deal.CardToTrade, Type = deal.Type, MinimumDamage = deal.MinimumDamage }).ToArray())));
        }

        [Test]
        public void GetAllTradingdealFailCardNotOwned()
        {
            User user = new User("testusr", "testpwd", 0);
            Mock<ITradingManager> tradingManagerMock = new Mock<ITradingManager>();
            GetAllTradingDealsCommand rc = new GetAllTradingDealsCommand(user, tradingManagerMock.Object);

            tradingManagerMock.Setup(x => x.GetTradingDeals(It.IsAny<Credentials>())).Throws<NoTradsAvailbleException>();
            var response = rc.Execute();

            tradingManagerMock.Verify(x => x.GetTradingDeals(user.Credentials));
            Assert.That(response.StatusCode, Is.EqualTo(StatusCode.NoContent));
            Assert.That(response.Payload, Is.EqualTo(null));
        }
        
        [Test]
        public void CarryOutTradeSuccess()
        {
            User user = new User("testusr", "testpwd", 0);
            Guid cardID = new Guid();
            Guid tradeId = new Guid();
            //TradingDeal tradingDeal = new TradingDeal(new Guid(), new Guid(), TradingDeal.SpellMonster.spell, 0);
            Mock<ITradingManager> tradingManagerMock = new Mock<ITradingManager>();
            CarryOutTradeCommand rc = new CarryOutTradeCommand(user, tradingManagerMock.Object, cardID, tradeId);

            tradingManagerMock.Setup(x => x.AcceptTradingDeal(It.IsAny<Credentials>(),It.IsAny<Guid>(), It.IsAny<Guid>())).Returns(true);
            var response = rc.Execute();

            tradingManagerMock.Verify(x => x.AcceptTradingDeal(user.Credentials, cardID, tradeId));
            Assert.That(response.StatusCode, Is.EqualTo(StatusCode.Ok));
            Assert.That(response.Payload, Is.EqualTo(null));
        }

        [Test]
        public void CarryOutTradeFailCardNotOwned()
        {
            User user = new User("testusr", "testpwd", 0);
            Guid cardID = new Guid();
            Guid tradeID = new Guid();
            Mock<ITradingManager> tradingManagerMock = new Mock<ITradingManager>();
            CarryOutTradeCommand rc = new CarryOutTradeCommand(user, tradingManagerMock.Object, cardID, tradeID);

            tradingManagerMock.Setup(x => x.AcceptTradingDeal(It.IsAny<Credentials>(), It.IsAny<Guid>(), It.IsAny<Guid>())).Throws<CardNotOwnedOrRequirementsNotMetException>();
            var response = rc.Execute();

            tradingManagerMock.Verify(x => x.AcceptTradingDeal(user.Credentials, cardID, tradeID));
            Assert.That(response.StatusCode, Is.EqualTo(StatusCode.Forbidden));
            Assert.That(response.Payload, Is.EqualTo(null));
        } 
        
        [Test]
        public void CarryOutTradeFailCardNotFound()
        {
            User user = new User("testusr", "testpwd", 0);
            Guid cardID = new Guid();
            Guid tradeID = new Guid();
            Mock<ITradingManager> tradingManagerMock = new Mock<ITradingManager>();
            CarryOutTradeCommand rc = new CarryOutTradeCommand(user, tradingManagerMock.Object, cardID, tradeID);

            tradingManagerMock.Setup(x => x.AcceptTradingDeal(It.IsAny<Credentials>(), It.IsAny<Guid>(), It.IsAny<Guid>())).Throws<CardNotFoundException>();
            var response = rc.Execute();

            tradingManagerMock.Verify(x => x.AcceptTradingDeal(user.Credentials, cardID, tradeID));
            Assert.That(response.StatusCode, Is.EqualTo(StatusCode.NotFound));
            Assert.That(response.Payload, Is.EqualTo(null));
        }
    }
}
