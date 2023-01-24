using MonsterTradingCardsGame.SWE1.MessageServer.Models.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SWE1.MessageServer.BLL.user
{
    internal interface IUserManager
    {
        User LoginUser(Credentials credentials);
        void RegisterUser(Credentials credentials);
        void UpdateUser(Credentials olduser, Credentials newUser);
        User GetUserByAuthToken(string authToken);
    }
}
