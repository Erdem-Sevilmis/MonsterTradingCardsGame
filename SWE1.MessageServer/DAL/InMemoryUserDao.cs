using MonsterTradingCardsGame.SWE1.MessageServer.Models.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWE1.MessageServer.DAL
{
    public class InMemoryUserDao : IUserDao
    {
        private readonly List<User> _users = new();

        public User? GetUserByAuthToken(string authToken)
        {
            return _users.SingleOrDefault(u => u.Token == authToken);
        }

        public User? GetUserByCredentials(string username, string password)
        {
            return _users.SingleOrDefault(u => u.Username == username && u.Password == password);
        }

        public bool InsertUser(User user)
        {
            var inserted = false;

            if (GetUserByUsername(user.Username) == null)
            {
                _users.Add(user);
                inserted = true;
            }

            return inserted;
        }

        private User? GetUserByUsername(string username)
        {
            return _users.SingleOrDefault(u => u.Username == username);
        }
    }
}
