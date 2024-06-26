﻿using MonsterTradingCardsGame.SWE1.MessageServer.Models.Card;
using MonsterTradingCardsGame.SWE1.MessageServer.Models.User;
using SWE1.MessageServer.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWE1.MessageServer.BLL.cards
{
    public class CardsManager : ICardsManager
    {
        private DatabaseCardDao DatabaseCardDao = new DatabaseCardDao();
        public bool ConfigureNewDeck(User identity, Guid[] cardIds)
        {
            return DatabaseCardDao.ConfigureNewDeck(identity,cardIds);
        }
        
        public List<Card> GetUserCards(User identity)
        {
           return DatabaseCardDao.GetUserCards(identity.Credentials.Username);
        }

        public List<Card> GetUserDeck(User identity)
        {
           return DatabaseCardDao.GetUserDeck(identity.Credentials.Username);
        }
    }
}
