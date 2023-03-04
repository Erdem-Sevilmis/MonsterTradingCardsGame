using MonsterTradingCardsGame.SWE1.MessageServer.Models.Card;
using MonsterTradingCardsGame.SWE1.MessageServer.Models.User;
using Moq;
using Newtonsoft.Json;
using SWE1.MessageServer.API.RouteCommands.cards;
using SWE1.MessageServer.API.RouteCommands.game;
using SWE1.MessageServer.BLL.cards;
using SWE1.MessageServer.BLL.game;
using SWE1.MessageServer.BLL.trading;
using SWE1.MessageServer.Core.Response;
using SWE1.MessageServer.DAL;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWE1.MessageServer.Test
{
    internal class GameTest
    {
        GameManager gameManager;
        DataBaseGameDao gameDao;

        [SetUp]
        public void SetUp()
        {
            gameDao = new DataBaseGameDao();
            gameManager = new GameManager();
        }
        
        [Test]
        public void GetScoreboardSuccess()
        {
            User user = new User("testusr", "testpwd", 0);
            List<UserStats> userStats = new List<UserStats>();
            Mock<IGameManager> gameManagerMock = new Mock<IGameManager>();
            GetScoreBoardCommand rc = new GetScoreBoardCommand(user, gameManagerMock.Object);

            gameManagerMock.Setup(x => x.GetScoreboard(It.IsAny<User>())).Returns(userStats);
            var response = rc.Execute();

            gameManagerMock.Verify(x => x.GetScoreboard(user));
            Assert.That(response.StatusCode, Is.EqualTo(StatusCode.Ok));
            Assert.That(response.Payload, Is.EqualTo(JsonConvert.SerializeObject(userStats.Select(stat => new { Name = stat.Name, Elo = stat.Elo, Winns = stat.Wins, Losses = stat.Losses }).ToArray())));
        }

        [Test]
        public void GetStatsSuccess()
        {
            User user = new User("testusr", "testpwd", 0);
            UserStats userStats = new UserStats(user.Credentials.Username, 0, 0, 0);
            Mock <IGameManager> gameManagerMock = new Mock<IGameManager>();
            GetStatsCommand rc = new GetStatsCommand(user, gameManagerMock.Object);

            gameManagerMock.Setup(x => x.GetStats(It.IsAny<User>())).Returns(userStats);
            var response = rc.Execute();

            gameManagerMock.Verify(x => x.GetStats(user));
            Assert.That(response.StatusCode, Is.EqualTo(StatusCode.Ok));
            Assert.That(response.Payload, Is.EqualTo(JsonConvert.SerializeObject(userStats)));
        }
    }
}
