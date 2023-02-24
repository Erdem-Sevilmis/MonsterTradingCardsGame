using MonsterTradingCardsGame.SWE1.MessageServer.Models.Card;
using MonsterTradingCardsGame.SWE1.MessageServer.Models.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWE1.MessageServer.BLL.package
{
    internal interface IPackageManager
    {
        void NewPackage(User identity, Card[] cardIds);
        List<Card> AcquireNewPackage(User identity);
    }
}
