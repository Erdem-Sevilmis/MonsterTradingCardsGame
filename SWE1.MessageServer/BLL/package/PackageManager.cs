using MonsterTradingCardsGame.SWE1.MessageServer.Models.Card;
using MonsterTradingCardsGame.SWE1.MessageServer.Models.User;
using SWE1.MessageServer.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWE1.MessageServer.BLL.package
{
    internal class PackageManager : IPackageManager
    {
        private DatabasePackageDao DatabasePackageDao = new DatabasePackageDao();
        public List<Card> AcquireNewPackage(User identity)
        {
           return DatabasePackageDao.AddPackageToPlayer(identity);
        }
        
        public void NewPackage(User identity, Card[] cardIds)
        {
            DatabasePackageDao.CreatePackage(cardIds);
        }
    }
}
