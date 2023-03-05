using MonsterTradingCardsGame;
using MonsterTradingCardsGame.SWE1.MessageServer.Models.Card;
using MonsterTradingCardsGame.SWE1.MessageServer.Models.User;
using Npgsql;
using SWE1.MessageServer.DAL;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace SWE1.MessageServer.BLL.game
{
    public class GameManager : IGameManager
    {
        DataBaseGameDao dataBaseGameDao = new DataBaseGameDao();
        DatabaseCardDao databaseCardDao = new DatabaseCardDao();
        ConcurrentQueue<User> users = new ConcurrentQueue<User>();
        ConcurrentDictionary<string, List<string>> log = new ConcurrentDictionary<string, List<string>>();
        List<string> battlelogs = new List<string>();
        List<Card> user_deck;
        List<Card> otherUser_deck;
        static Random rnd = new Random();
        int roundCount = 1;

        public List<string> GetInToBattle(User user)
        {
            User otherUser = null;
            var result = users.TryDequeue(out otherUser);
            if (result)
            {
                Battle(user, otherUser);
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

        private void Battle(User user, User otherUser)
        {
            user_deck = databaseCardDao.GetUserDeck(user.Credentials.Username);
            otherUser_deck = databaseCardDao.GetUserDeck(otherUser.Credentials.Username);
            battlelogs.Add("\n");
            while (user_deck.Count > 0 && otherUser_deck.Count > 0 && roundCount < 101)
            {
                battlelogs.Add($"\nRound #{roundCount}: ");
                var user_card = user_deck.ElementAt(rnd.Next(0, user_deck.Count));
                var otherUser_card = otherUser_deck.ElementAt(rnd.Next(0, otherUser_deck.Count));

                if (user_card.Name.ToString().Contains("Spell") || otherUser_card.Name.ToString().Contains("Spell"))
                {
                    bool isUserWaterSpell = user_card.Name.Equals(DataBase.Card_Type.WaterSpell);
                    bool isOtherUserKnight = otherUser_card.Name.Equals(DataBase.Card_Type.Knight);
                    bool isOtherUserWaterSpell = otherUser_card.Name.Equals(DataBase.Card_Type.WaterSpell);
                    bool isUserKnight = user_card.Name.Equals(DataBase.Card_Type.Knight);
                    if (isUserWaterSpell && isOtherUserKnight)
                    {
                        HandleLog(user, otherUser, user_card, otherUser_card, false, $"{user_card.Damage} vs {otherUser_card.Damage}");
                        HandleCardSwap(otherUser_card);
                        continue;
                    }
                    if (isOtherUserWaterSpell && isUserKnight)
                    {
                        HandleLog(otherUser, user, otherUser_card, user_card, false, $"{user_card.Damage} vs {otherUser_card.Damage}");
                        HandleCardSwap(user_card);
                        continue;
                    }

                    bool isUserKraken = user_card.Name.Equals(DataBase.Card_Type.Kraken);
                    bool isOtherUserSpell = otherUser_card.Name.ToString().Contains("Spell");
                    bool isOtherUserKraken = otherUser_card.Name.Equals(DataBase.Card_Type.Kraken);
                    bool isUserSpell = user_card.Name.ToString().Contains("Spell");
                    if (isUserKraken && isOtherUserSpell)
                    {
                        HandleLog(user, otherUser, user_card, otherUser_card, false, $"{user_card.Damage} vs {otherUser_card.Damage}");
                        HandleCardSwap(otherUser_card);
                        continue;
                    }
                    if (isOtherUserKraken && isUserSpell)
                    {
                        HandleLog(otherUser, user, otherUser_card, user_card, false, $"{user_card.Damage} vs {otherUser_card.Damage}");
                        HandleCardSwap(user_card);
                        continue;
                    }

                    CardVsCard(user, otherUser, user_card, otherUser_card);
                }
                else
                {
                    //Checking for specialties
                    bool isUserDragon = user_card.Name.Equals(DataBase.Card_Type.Dragon);
                    bool isOtherUserGoblin = otherUser_card.Name.ToString().Contains("Goblin");
                    bool isOtherUserDragon = otherUser_card.Name.Equals(DataBase.Card_Type.Dragon);
                    bool isUserGoblin = user_card.Name.ToString().Contains("Goblin");
                    if (isUserDragon && isOtherUserGoblin)
                    {
                        HandleLog(user, otherUser, user_card, otherUser_card, false, $"{user_card.Damage} vs {otherUser_card.Damage}");
                        HandleCardSwap(otherUser_card);
                        continue;
                    }
                    if (isOtherUserDragon && isUserGoblin)
                    {
                        HandleLog(otherUser, user, otherUser_card, user_card, false, $"{user_card.Damage} vs {otherUser_card.Damage}");
                        HandleCardSwap(user_card);
                        continue;
                    }

                    bool isUserWizard = user_card.Name.Equals(DataBase.Card_Type.Wizzard);
                    bool isOtherUserOrk = otherUser_card.Name.Equals(DataBase.Card_Type.Ork);
                    bool isOtherUserWizard = otherUser_card.Name.Equals(DataBase.Card_Type.Wizzard);
                    bool isUserOrk = user_card.Name.Equals(DataBase.Card_Type.Ork);
                    if (isUserWizard && isOtherUserOrk)
                    {
                        HandleLog(user, otherUser, user_card, otherUser_card, false, $"{user_card.Damage} vs {otherUser_card.Damage}");
                        HandleCardSwap(otherUser_card);
                        continue;
                    }
                    if (isOtherUserWizard && isUserOrk)
                    {
                        HandleLog(otherUser, user, otherUser_card, user_card, false, $"{user_card.Damage} vs {otherUser_card.Damage}");
                        HandleCardSwap(user_card);
                        continue;
                    }

                    bool isUserFireElf = user_card.Name.Equals(DataBase.Card_Type.FireElf);
                    bool isOtherUserFireElf = otherUser_card.Name.Equals(DataBase.Card_Type.FireElf);
                    if (isUserFireElf && isOtherUserDragon)
                    {
                        HandleLog(user, otherUser, user_card, otherUser_card, false, $"{user_card.Damage} vs {otherUser_card.Damage}");
                        HandleCardSwap(otherUser_card);
                        continue;
                    }
                    if (isOtherUserFireElf && isUserDragon)
                    {
                        HandleLog(otherUser, user, otherUser_card, user_card, false, $"{user_card.Damage} vs {otherUser_card.Damage}");
                        HandleCardSwap(user_card);
                        continue;
                    }
                    CompareDamage(user, otherUser, user_card, otherUser_card);
                }
            }

            battlelogs.Add($"\n");
            if (roundCount >= 100)
            {
                battlelogs.Add($"{user.Credentials.Username} vs {otherUser.Credentials.Username} ended in a Draw.\n");
            }
            else if (otherUser_deck.Count == 0)
            {
                UpdateStats(user, true);
                UpdateStats(otherUser, false);
                battlelogs.Add($"{user.Credentials.Username} has Won.\n");
            }
            else
            {
                UpdateStats(user, false);
                UpdateStats(otherUser, true);
                battlelogs.Add($"{otherUser.Credentials.Username} has Won.\n");
            }
            log.TryAdd(otherUser.Credentials.Username, battlelogs);
            return;
        }

        private void UpdateStats(User user, bool won)
        {
            DataBaseGameDao userDao = new DataBaseGameDao();
            userDao.UpdateStats(user, won);
        }

        private void CardVsCard(User user, User otherUser, Card user_card, Card otherUser_card)
        {
            switch ((user_card.ElementType, otherUser_card.ElementType))
            {
                case (ElementType.Water, ElementType.Fire):
                    HandleLog(user, otherUser, user_card, otherUser_card, false, $"{user_card.Damage * 2}vs{otherUser_card.Damage / 2}");
                    HandleCardSwap(otherUser_card);
                    break;
                case (ElementType.Fire, ElementType.Normal):
                    HandleLog(user, otherUser, user_card, otherUser_card, false, $"{user_card.Damage * 2}vs{otherUser_card.Damage / 2}");
                    HandleCardSwap(otherUser_card);
                    break;
                case (ElementType.Normal, ElementType.Water):
                    HandleLog(user, otherUser, user_card, otherUser_card, false, $"{user_card.Damage * 2}vs{otherUser_card.Damage / 2}");
                    HandleCardSwap(otherUser_card);
                    break;
                case (ElementType.Water, ElementType.Water):
                    CompareDamage(user, otherUser, user_card, otherUser_card);
                    break;
                case (ElementType.Fire, ElementType.Fire):
                    CompareDamage(user, otherUser, user_card, otherUser_card);
                    break;
                case (ElementType.Normal, ElementType.Normal):
                    CompareDamage(user, otherUser, user_card, otherUser_card);
                    break;
                default:
                    CardVsCard(otherUser, user, otherUser_card, user_card);
                    break;
            }
        }

        private bool UniqueFeature()
        {
            //3% chance for event to trigger
            if (rnd.Next(0, 101) >= 2)
                return false;
            //50/50 coinfip 
            if (rnd.Next(0, 101) <= 50)
                return false;

            battlelogs.Add($"**SPECIAL EVENT TRIGGER** looser keeps card!");
            return true;
        }

        private void CompareDamage(User user, User otherUser, Card user_card, Card otherUser_card)
        {
            switch (user_card.Damage.CompareTo(otherUser_card.Damage))
            {
                case 1:
                    HandleLog(user, otherUser, user_card, otherUser_card, false, $"{user_card.Damage} vs {otherUser_card.Damage}");
                    HandleCardSwap(otherUser_card);
                    break;
                case -1:
                    HandleLog(otherUser, user, otherUser_card, user_card, false, $"{user_card.Damage} vs {otherUser_card.Damage}");
                    HandleCardSwap(user_card);
                    //other user won
                    break;
                default:
                    //draw
                    HandleLog(user, otherUser, user_card, otherUser_card, true, $"{user_card.Damage} vs {otherUser_card.Damage}");
                    break;
            }
        }

        private void HandleCardSwap(Card looser_card)
        {
            if (UniqueFeature())
                return;

            foreach (var card in user_deck)
            {
                if (card.Id.Equals(looser_card.Id))
                {
                    otherUser_deck.Add(card);
                    user_deck.Remove(card);
                    return;
                }
            }

            foreach (var card in otherUser_deck)
            {
                if (card.Id.Equals(looser_card.Id))
                {
                    user_deck.Add(card);
                    otherUser_deck.Remove(card);
                    return;
                }
            }
        }

        private void HandleLog(User winner, User looser, Card winner_Card, Card looser_Card, bool draw, string realDmg)
        {
            roundCount++;
            if (draw)
            {
                battlelogs.Add($"{LogBuilder(winner, winner_Card)} vs {LogBuilder(looser, looser_Card)} => {winner_Card.Damage}vs{looser_Card.Damage} -> {realDmg} => Draw");
                return;
            }
            battlelogs.Add($"{LogBuilder(winner, winner_Card)} vs {LogBuilder(looser, looser_Card)} => {winner_Card.Damage}vs{looser_Card.Damage} -> {realDmg} => {winner_Card.Name} wins");
        }

        private string LogBuilder(User user, Card user_Card)
        {
            return $"{user.Credentials.Username}: {user_Card.Name} ({user_Card.Damage} Damage)";
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
