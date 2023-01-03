using MonsterTradingCardsGame.SWE1.MessageServer.Models.User;
using SWE1.MessageServer.DAL;

namespace SWE1.MessageServer.Test
{
    internal class InMemoryUserDaoTest
    {
        [Test]
        public void TestInsertNotExistingUserReturnsTrue()
        {
            // arrange
            var repo = new InMemoryUserDao();
            var user = new User("test", "pwd");

            // act
            var result = repo.InsertUser(user);

            // assert
            Assert.That(result, Is.True);
        }

        [Test]
        public void TestInsertExistingUsernameReturnsFalse()
        {
            // arrange
            var repo = new InMemoryUserDao();
            repo.InsertUser(new User("test", "pwd"));
            var user = new User("test", "otherpwd");

            // act
            var result = repo.InsertUser(user);

            // assert
            Assert.That(result, Is.False);
        }

        [Test]
        public void TestInsertExistingUsernameAndPasswordReturnsFalse()
        {
            // arrange
            var repo = new InMemoryUserDao();
            repo.InsertUser(new User("test", "pwd"));
            var user = new User("test", "pwd");

            // act
            var result = repo.InsertUser(user);

            // assert
            Assert.That(result, Is.False);
        }

        [Test]
        public void TestGetUserByAuthTokenReturnsUserForCorrectToken()
        {
            // arrange
            var repo = new InMemoryUserDao();
            var expectedUser = new User("test", "pwd");
            var token = expectedUser.Token;
            repo.InsertUser(expectedUser);

            // act
            var actualUser = repo.GetUserByAuthToken(token);

            // assert
            Assert.That(actualUser, Is.EqualTo(expectedUser).Using<User>((a, b) => a.Username == b.Username && a.Password == b.Password));
        }

        [Test]
        public void TestGetUserByAuthTokenReturnsNullForWrongToken()
        {
            // arrange
            var repo = new InMemoryUserDao();
            var user = new User("test", "pwd");
            var token = "invalid";
            repo.InsertUser(user);

            // act
            var actualUser = repo.GetUserByAuthToken(token);

            // assert
            Assert.That(actualUser, Is.Null);
        }

        [Test]
        public void TestGetUserByCredentialsReturnsUserForCorrectCredentials()
        {
            // arrange
            var repo = new InMemoryUserDao();
            var expectedUser = new User("test", "pwd");
            repo.InsertUser(expectedUser);

            // act
            var actualUser = repo.GetUserByCredentials(expectedUser.Username, expectedUser.Password);

            // assert
            Assert.That(actualUser, Is.EqualTo(expectedUser).Using<User>((a, b) => a.Username == b.Username && a.Password == b.Password));
        }

        [Test]
        public void TestGetUserByCredentialsReturnsNullForWrongPassword()
        {
            // arrange
            var repo = new InMemoryUserDao();
            var user = new User("test", "pwd");
            repo.InsertUser(user);

            // act
            var actualUser = repo.GetUserByCredentials(user.Username, "wrong");

            // assert
            Assert.That(actualUser, Is.Null);
        }

        [Test]
        public void TestGetUserByCredentialsReturnsNullForWrongUsername()
        {
            // arrange
            var repo = new InMemoryUserDao();
            var user = new User("test", "pwd");
            repo.InsertUser(user);

            // act
            var actualUser = repo.GetUserByCredentials("wrong", user.Password);

            // assert
            Assert.That(actualUser, Is.Null);
        }

        [Test]
        public void TestGetUserByCredentialsReturnsNullForWrongCredentials()
        {
            // arrange
            var repo = new InMemoryUserDao();
            var user = new User("test", "pwd");

            // act
            var actualUser = repo.GetUserByCredentials("wrong", "wrong");

            // assert
            Assert.That(actualUser, Is.Null);
        }
    }
}
