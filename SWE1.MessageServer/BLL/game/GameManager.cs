using MonsterTradingCardsGame.SWE1.MessageServer.Models.User;
using SWE1.MessageServer.DAL;
using System;
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
