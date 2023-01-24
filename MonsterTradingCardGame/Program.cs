using MonsterTradingCardsGame;
using System.Net;

namespace MyApp // Note: actual namespace depends on the project name.
{
    internal class Program
    {
        static void Main(string[] args)
        {
           DataBase db = new DataBase();
           db.Start();
            /*
            * Connect to Database
            * 
            var userDao = new InMemoryUserDao();
            var userManager = new UserManager(userDao);

            var router = new Router(userManager);
            var server = new HttpServer(IPAddress.Any, 10001, router);
            server.Start();
            */
        }
    }
}