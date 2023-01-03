using SWE1.MessageServer.API.RouteCommands;
using SWE1.MessageServer.BLL;
using SWE1.MessageServer.Core.Server;
using SWE1.MessageServer.DAL;
using System.Net;

namespace MonsterTradingCardsGame
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var userDao = new InMemoryUserDao();
            var userManager = new UserManager(userDao);

            var router = new Router(userManager);
            var server = new HttpServer(IPAddress.Any, 10001, router);
            server.Start();
        }
    }
}

/*
 * Checklist:
 *  -User class ✔️
 *  -User credentials ✔️
 *  -Card ✔️
 *  -SpellCard ✔️
 *  -MonsterCard ✔️
 */