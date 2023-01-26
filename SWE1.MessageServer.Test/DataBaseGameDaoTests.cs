using MonsterTradingCardsGame.SWE1.MessageServer.Models.User;
using NUnit.Framework;
using SWE1.MessageServer.DAL;

namespace SWE1.MessageServer.Test
{
    [TestClass]
    public class DataBaseGameDaoTests
    {
        private DataBaseGameDao _dao;

        [TestInitialize]
        public void Setup()
        {
            // Arrange
            _dao = new DataBaseGameDao();
        }

        [TestMethod]
        public void TestGetStats()
        {
            // Arrange
            var identity = new User(new UserCredentials("testuser", "password"));

            // Act
            var result = _dao.GetStats(identity);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("testuser", result.Name);
            Assert.IsTrue(result.Elo >= 0);
            Assert.IsTrue(result.Wins >= 0);
            Assert.IsTrue(result.Losses >= 0);
        }

        [TestMethod]
        public void TestGetScoreBoard()
        {
            // Arrange
            var identity = new User(new UserCredentials("testuser", "password"));

            // Act
            var result = _dao.GetScoreBoard(identity);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Count > 0);
            Assert.IsTrue(result.All(u => u.Elo >= 0));
            Assert.IsTrue(result.All(u => u.Wins >= 0));
            Assert.IsTrue(result.All(u => u.Losses >= 0));
        }
    }
}
