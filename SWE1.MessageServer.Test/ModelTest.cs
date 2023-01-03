using SWE1.MessageServer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWE1.MessageServer.Test
{
    internal class ModelTest
    {
        [Test]
        public void UserHasValidToken()
        {
            // arrange
            var expectedToken = "testusr-msgToken";

            // act
            var user = new User("testusr", "testpwd");

            // assert
            Assert.That(user.Token, Is.EqualTo(expectedToken));
        }
    }
}
