using Microsoft.VisualBasic;
using MonsterTradingCardsGame.SWE1.MessageServer.Models.User;
using Moq;
using Npgsql;
using SWE1.MessageServer.API.RouteCommands.Users;
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
    internal class UserTest
    {
        UserManager userManager;
        DatabaseUserDao userDao;

        [SetUp]
        public void SetUp()
        {
            userDao = new DatabaseUserDao();
            userManager = new UserManager(userDao);
        }

        [Test]
        public void UserRegisterSuccess()
        {
            User user = new User("testusr", "testpwd", 0);
            Mock<IUserManager> userManagerMock = new Mock<IUserManager>();
            RegisterCommand rc = new RegisterCommand(userManagerMock.Object, user.Credentials);

            var response = rc.Execute();

            userManagerMock.Verify(x => x.RegisterUser(user.Credentials));
            Assert.That(response.StatusCode, Is.EqualTo(StatusCode.Created));
            Assert.That(response.Payload, Is.EqualTo(null));
        }

        [Test]
        public void UserRegisterFail()
        {
            User user = new User("testusr", "testpwd", 0);
            Mock<IUserManager> userManagerMock = new Mock<IUserManager>();
            RegisterCommand rc = new RegisterCommand(userManagerMock.Object, user.Credentials);

            userManagerMock.Setup(x => x.RegisterUser(It.IsAny<Credentials>())).Throws<DuplicateUserException>();
            var response = rc.Execute();

            userManagerMock.Verify(x => x.RegisterUser(user.Credentials));
            Assert.That(response.StatusCode, Is.EqualTo(StatusCode.Conflict));

            Assert.That(response.Payload, Is.EqualTo(null));
        }

        [Test]
        public void UserLoginCommandSuccess()
        {
            User user = new User("testusr", "testpwd", 0);
            Mock<IUserManager> userManagerMock = new Mock<IUserManager>();
            LoginCommand rc = new LoginCommand(userManagerMock.Object, user.Credentials);

            userManagerMock.Setup(x => x.LoginUser(It.IsAny<Credentials>())).Returns(user);
            var response = rc.Execute();

            userManagerMock.Verify(x => x.LoginUser(user.Credentials));
            Assert.That(response.StatusCode, Is.EqualTo(StatusCode.Ok));
            Assert.That(response.Payload, Is.EqualTo(user.Token));
        }

        [Test]
        public void UserLoginCommandFail()
        {
            User user = new User("testusr", "testpwd", 0);
            Mock<IUserManager> userManagerMock = new Mock<IUserManager>();
            LoginCommand rc = new LoginCommand(userManagerMock.Object, user.Credentials);

            userManagerMock.Setup(x => x.LoginUser(It.IsAny<Credentials>())).Throws<UserNotFoundException>();
            var response = rc.Execute();

            userManagerMock.Verify(x => x.LoginUser(user.Credentials));
            Assert.That(response.StatusCode, Is.EqualTo(StatusCode.Unauthorized));
            Assert.That(response.Payload, Is.EqualTo(null));
        }

        [Test]
        public void UserUpdateCommandSuccess()
        {
            User user = new User("testusr", "testpwd", 0);
            Mock<IUserManager> userManagerMock = new Mock<IUserManager>();
            UpdateCommand rc = new UpdateCommand(user, userManagerMock.Object, user.Credentials.Username, user.UserData);

            var response = rc.Execute();

            userManagerMock.Verify(x => x.UpdateUser(user, user.UserData));
            Assert.That(response.StatusCode, Is.EqualTo(StatusCode.Ok));
            Assert.That(response.Payload, Is.EqualTo(null));
        }

        [Test]
        public void UserUpdateCommandFail()
        {
            User user = new User("testusr", "testpwd", 0);
            Mock<IUserManager> userManagerMock = new Mock<IUserManager>();
            UpdateCommand rc = new UpdateCommand(user, userManagerMock.Object, user.Credentials.Username, user.UserData);

            userManagerMock.Setup(x => x.UpdateUser(It.IsAny<User>(), It.IsAny<UserData>())).Throws<UserNotFoundException>();
            var response = rc.Execute();

            userManagerMock.Verify(x => x.UpdateUser(user, user.UserData));
            Assert.That(response.StatusCode, Is.EqualTo(StatusCode.NotFound));
            Assert.That(response.Payload, Is.EqualTo(null));
        }

        [Test]
        public void UserHasValidToken()
        {
            // arrange
            var expectedToken = "testusr-mtcgToken";

            // act
            var user = new User("testusr", "testpwd", 0);

            // assert
            Assert.That(user.Token, Is.EqualTo(expectedToken));
        }
    }
}
