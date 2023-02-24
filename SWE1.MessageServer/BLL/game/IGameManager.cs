using MonsterTradingCardsGame.SWE1.MessageServer.Models.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWE1.MessageServer.BLL.game
{
    internal interface IGameManager
    {
        List<string> GetInToBattle(User user);
        List<UserStats> GetScoreboard(User identity);
        UserStats GetStats(User identity);
    }
}
