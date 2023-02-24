using MonsterTradingCardsGame.SWE1.MessageServer.Models.User;
using SWE1.MessageServer.DAL;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWE1.MessageServer.BLL.game
{
    internal class GameManager : IGameManager
    {
        DataBaseGameDao dataBaseGameDao = new DataBaseGameDao();
        ConcurrentQueue<User> users = new ConcurrentQueue<User>();
        ConcurrentDictionary<string, List<string>> log = new ConcurrentDictionary<string, List<string>>();
        public List<string> GetInToBattle(User user)
        {
            User otherUser = null;
            var result = users.TryDequeue(out otherUser);
            if (result)
            {
                List<string> battlelogs = new List<string>();
                //Fetch Deck from both players 
                //Get a randome card form each deck 
                //more info from angabe

                //Battlelogic
                //bumbum stuff happens
                battlelogs.Add("bumm kaputt");
                battlelogs.Add("sasaa");
                battlelogs.Add("dara");
                log.TryAdd(otherUser.Credentials.Username, battlelogs);
                return battlelogs;
            }
            else
            {
                users.Enqueue(user);
                //wait for result 
                List<string> value;
                while (!log.TryGetValue(user.Credentials.Username, out value))
                {
                    // If the key is not yet present, wait for a short period of time before checking again
                    Thread.Sleep(100);
                }
                return value;
            }
        }

        public List<UserStats> GetScoreboard(User identity)
        {
            return dataBaseGameDao.GetScoreBoard(identity);
        }
        public UserStats GetStats(User identity)
        {
            return dataBaseGameDao.GetStats(identity);
        }

    }
}
