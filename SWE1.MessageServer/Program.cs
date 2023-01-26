using Npgsql;
using SWE1.MessageServer.API.RouteCommands;
using SWE1.MessageServer.BLL.cards;
using SWE1.MessageServer.BLL.game;
using SWE1.MessageServer.BLL.package;
using SWE1.MessageServer.BLL.trading;
using SWE1.MessageServer.BLL.user;
using SWE1.MessageServer.Core.Server;
using SWE1.MessageServer.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SWE1.MessageServer
{
    internal class Program
    {
        static void Main(string[] args)
        {
            ClearDataBase();
            var userDao = new DatabaseUserDao();
            var userManager = new UserManager(userDao);
            var cardManager = new CardsManager();
            var gameManager = new GameManager();
            var packageManager = new PackageManager();
            var tradingManager = new TradingManager();

            var router = new Router(userManager,cardManager,packageManager,gameManager,tradingManager);
            var server = new HttpServer(IPAddress.Any, 10001, router);
            server.Start();
        }

        private static void ClearDataBase()
        {
            using (var conn = new NpgsqlConnection("Host=localhost;Username=admin;Password=admin;Database=dbmtcg"))
            {
                conn.Open();

                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = conn;

                    // Read the contents of the script file
                    string script = File.ReadAllText("script.sql");

                    // Execute the script
                    cmd.CommandText = script;
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
