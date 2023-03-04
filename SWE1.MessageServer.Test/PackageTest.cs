using MonsterTradingCardsGame.SWE1.MessageServer.Models.Card;
using MonsterTradingCardsGame.SWE1.MessageServer.Models.User;
using Moq;
using Newtonsoft.Json;
using SWE1.MessageServer.API.RouteCommands.game;
using SWE1.MessageServer.API.RouteCommands.packages;
using SWE1.MessageServer.BLL.cards;
using SWE1.MessageServer.BLL.game;
using SWE1.MessageServer.BLL.package;
using SWE1.MessageServer.Core.Response;
using SWE1.MessageServer.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWE1.MessageServer.Test
{
    internal class PackageTest
    {
        PackageManager packageManager;
        DatabasePackageDao packageDao;

        [SetUp]
        public void SetUp()
        {
            packageDao = new DatabasePackageDao();
            packageManager = new PackageManager();
        }

        [Test]
        public void CreateNewPackageSuccess()
        {
            User user = new User("admin", "admin", 0);
            Card[] cards = new Card[] { };
            Mock<IPackageManager> packageManagerMock = new Mock<IPackageManager>();
            NewPackageCommand rc = new NewPackageCommand(user, packageManagerMock.Object, cards);

            packageManagerMock.Setup(x => x.NewPackage(It.IsAny<User>(), It.IsAny<Card[]>()));
            var response = rc.Execute();

            packageManagerMock.Verify(x => x.NewPackage(user, cards));
            Assert.That(response.StatusCode, Is.EqualTo(StatusCode.Created));
            Assert.That(response.Payload, Is.EqualTo(null));
        }
        
        /*
        [Test]
        public void CreateNewPackageFailCardExist()
        {
            User user = new User("testusr", "testpwd", 0);
            Card[] cards = new Card[] { };
            Mock<IPackageManager> packageManagerMock = new Mock<IPackageManager>();
            NewPackageCommand rc = new NewPackageCommand(user, packageManagerMock.Object, cards);

            packageManagerMock.Setup(x => x.NewPackage(It.IsAny<User>(), It.IsAny<Card[]>())).Throws<AtLeastOneCardInThePackageAlreadyExistsException>();
            var response = rc.Execute();

            packageManagerMock.Verify(x => x.NewPackage(user, cards));
            Assert.That(response.StatusCode, Is.EqualTo(StatusCode.Conflict));
            Assert.That(response.Payload, Is.EqualTo(null));
        }

        [Test]
        public void CreateNewPackageFailNotAdmin()
        {
            User user = new User("testusr", "testpwd", 0);
            Card[] cards = new Card[] { };
            Mock<IPackageManager> packageManagerMock = new Mock<IPackageManager>();
            NewPackageCommand rc = new NewPackageCommand(user, packageManagerMock.Object, cards);

            packageManagerMock.Setup(x => x.NewPackage(It.IsAny<User>(), It.IsAny<Card[]>()));
            var response = rc.Execute();

            packageManagerMock.Verify(x => x.NewPackage(user, cards));
            Assert.That(response.StatusCode, Is.EqualTo(StatusCode.Forbidden));
            Assert.That(response.Payload, Is.EqualTo(null));
        }
        */
    }
}
