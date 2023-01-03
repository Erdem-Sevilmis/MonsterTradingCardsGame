using MonsterTradingCardsGame.SWE1.MessageServer.Models.User;
using Moq;
using SWE1.MessageServer.BLL;
using SWE1.MessageServer.DAL;
using SWE1.MessageServer.Models;

namespace SWE1.MessageServer.Test
{
    internal class UserManagerTest
    {
        [Test]
        public void LoginWithValidCredentialsReturnsUser()
        {
            // arrange
            var userDao = new Mock<IUserDao>();
            var userManager = new UserManager(userDao.Object);

            var expectedUser = new User("test", "pwd");
            userDao.Setup(m => m.GetUserByCredentials(It.IsAny<string>(), It.IsAny<string>())).Returns(expectedUser);

            // act
            var actualUser = userManager.LoginUser(new Credentials(expectedUser.Username, expectedUser.Password));

            // assert
            Assert.That(actualUser, Is.EqualTo(expectedUser));
        }

        [Test]
        public void LoginWithInvalidCredentialsThrowsUserNotFoundException()
        {
            // arrange
            var userDao = new Mock<IUserDao>();
            var userManager = new UserManager(userDao.Object);

            var invalidUser = new User("test", "pwd");
            userDao.Setup(m => m.GetUserByCredentials(It.IsAny<string>(), It.IsAny<string>())).Returns<User?>(null);

            // act and assert
            Assert.That(() => userManager.LoginUser(new Credentials(invalidUser.Username, invalidUser.Password)), Throws.TypeOf<UserNotFoundException>());
        }

        [Test]
        public void RegisterWithValidCredentialsDoesNotThrow()
        {
            // arrange
            var userRepo = new Mock<IUserDao>();
            var userManager = new UserManager(userRepo.Object);

            var newUser = new User("testusr", "testpwd");
            userRepo.Setup(m => m.InsertUser(It.IsAny<User>())).Returns(true);

            // act and assert
            Assert.That(() => userManager.RegisterUser(new Credentials(newUser.Username, newUser.Password)), Throws.Nothing);
        }

        [Test]
        public void RegisterWithExistingUserThrowsDuplicateUserException()
        {
            // arrange
            var userRepo = new Mock<IUserDao>();
            var userManager = new UserManager(userRepo.Object);

            var existingUser = new User("testusr", "testpwd");
            userRepo.Setup(m => m.InsertUser(It.IsAny<User>())).Returns(false);

            // act and assert
            Assert.That(() => userManager.RegisterUser(new Credentials(existingUser.Username, existingUser.Password)), Throws.TypeOf<DuplicateUserException>());
        }
    }
}
