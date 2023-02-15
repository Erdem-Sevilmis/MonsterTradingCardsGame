﻿using MonsterTradingCardsGame;
using Npgsql;
using SWE1.MessageServer.API.RouteCommands.trading;
using SWE1.MessageServer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using static SWE1.MessageServer.Models.TradingDeal;

namespace SWE1.MessageServer.DAL
{
    internal class DataBaseTradingDao
    {
        private NpgsqlConnection connection;
        public DataBaseTradingDao()
        {
            DataBase db = new DataBase();
            connection = db.connection;
        }
        public bool CreateNewTradingDeal(string username, TradingDeal tradindeal)
        {
            using (var cmd = new NpgsqlCommand("SELECT * FROM tradings where id=@id", connection))
            {
                // Add the parameter and its value
                cmd.Parameters.AddWithValue("@id", tradindeal.Id);
                // Execute the update statement
                using var reader = cmd.ExecuteReader();
                if (reader.HasRows)
                    throw new CardDealAlredyExistsException();
            }

            DatabaseCardDao cardDao = new DatabaseCardDao();
            var cards = cardDao.GetUserCards(username);

            if (cards.Any(e => e.Id == tradindeal.CardToTrade))
            {
                var userDeck = cardDao.GetUserDeck(username);
                if (userDeck.All((card) => card.Id.Equals(card)))
                    throw new CardNotOwnedOrInDeckException();
            }
            else
                return false;

            using (var cmd = new NpgsqlCommand("INSERT INTO tradings (id, cardid, minimum_damage, type, username ) VALUES (@id, @cardid, @minimum_damage,@type,@username )", connection))
            {
                // Add the parameter and its value
                cmd.Parameters.AddWithValue("@id", tradindeal.Id);
                cmd.Parameters.AddWithValue("@cardid", tradindeal.CardToTrade);
                cmd.Parameters.AddWithValue("@minimum_damage", tradindeal.MinimumDamage);
                cmd.Parameters.AddWithValue("@type", tradindeal.Type);
                cmd.Parameters.AddWithValue("@username", username);
                try
                {
                    cmd.ExecuteNonQuery();

                }
                catch (Exception)
                {
                    Console.WriteLine("wadehell");
                }
                return true;
            }
        }

        public List<TradingDeal> GetTradingDeals(string username)
        {
            List<TradingDeal> availableTrades = new List<TradingDeal>();
            using (var cmd = new NpgsqlCommand("SELECT * FROM tradings", connection))
            {
                using var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    SpellMonster name;
                    Enum.TryParse(reader["type"].ToString(), out name);
                    availableTrades.Add(new TradingDeal((Guid)reader["id"], (Guid)reader["cardid"], name, int.Parse(reader["minimum_damage"].ToString())));
                }
            }
            if (availableTrades.Count <= 0)
                throw new NoTradsAvailbleException();

            return availableTrades;
        }

        public bool DeleteTradingDeal(string username, Guid tradeID)
        {
            Guid cardID = new Guid();
            using (var cmd = new NpgsqlCommand("SELECT * FROM tradings where id=@id", connection))
            {
                // Add the parameter and its value
                cmd.Parameters.AddWithValue("@id", tradeID);
                // Execute the update statement
                using var reader = cmd.ExecuteReader();
                if (!reader.HasRows)
                    throw new CardNotFoundException();
                if (reader.Read())
                {
                    cardID = (Guid)reader["cardid"];
                }
            }
            
            DatabaseCardDao cardDao = new DatabaseCardDao();
            var cards = cardDao.GetUserCards(username);

            if (!cards.Any(e => e.Id == cardID))
                throw new CardNotOwnedOrInDeckException();

            using (var cmd = new NpgsqlCommand("DELETE FROM tradings where id=@id", connection))
            {
                cmd.Parameters.AddWithValue("@id", tradeID);
                cmd.ExecuteNonQuery();
                return true;
            }
        }

        public bool Trade(string username, Guid cardId, Guid otherCardId)
        {
            string otherUsername = String.Empty;
            using (var cmd = new NpgsqlCommand("SELECT * FROM tradings where id=@id", connection))
            {
                // Add the parameter and its value
                cmd.Parameters.AddWithValue("@id", cardId);
                // Execute the update statement
                using var reader = cmd.ExecuteReader();
                if (!reader.HasRows)
                    throw new CardNotFoundException();
            }
            DatabaseCardDao cardDao = new DatabaseCardDao();
            var cards = cardDao.GetUserCards(username);
            if (cards.Any(e => e.Id == cardId))
            {
                var userDeck = cardDao.GetUserDeck(username);
                if (userDeck.All((card) => card.Id.Equals(cardId)))
                    throw new CardNotOwnedOrRequirementsNotMetException();
            }
            else
                throw new CardNotOwnedOrRequirementsNotMetException();

            using (var cmd = new NpgsqlCommand("SELECT * FROM tradings where id=@id", connection))
            {
                cmd.Parameters.AddWithValue("id", otherCardId);
                // Execute the update statement
                using var reader = cmd.ExecuteReader();
                if (reader.Read())
                    otherUsername = reader["username"].ToString();
            }

            if (otherUsername == null)
                return false;

            //TODO: Check if Trade meets requirements 
            //if not throw new CardNotOwnedOrRequirementsNotMetException();

            using (var cmd = new NpgsqlCommand("UPDATE user_account SET stack = array_remove(stack, '@otherCardId') WHERE username = @otherusername;", connection))
            {
                cmd.Parameters.AddWithValue("otherCardId", otherCardId);
                cmd.Parameters.AddWithValue("otherusername", otherUsername);
                cmd.ExecuteNonQuery();
            }
            using (var cmd = new NpgsqlCommand("UPDATE user_account SET stack = array_append(stack, @otherCardId) WHERE username = @otherusername", connection))
            {
                cmd.Parameters.AddWithValue("otherCardId", otherCardId);
                cmd.Parameters.AddWithValue("otherusername", otherUsername);
                cmd.ExecuteNonQuery();
            }

            using (var cmd = new NpgsqlCommand("UPDATE user_account SET stack = array_remove(stack, '@cardId') WHERE username = @username;", connection))
            {
                cmd.Parameters.AddWithValue("cardId", cardId);
                cmd.Parameters.AddWithValue("username", username);
                cmd.ExecuteNonQuery();
            }
            using (var cmd = new NpgsqlCommand("UPDATE user_account SET stack = array_append(stack, @cardId) WHERE username = @username", connection))
            {
                cmd.Parameters.AddWithValue("cardId", cardId);
                cmd.Parameters.AddWithValue("username", username);
                cmd.ExecuteNonQuery();
            }

            using (var cmd = new NpgsqlCommand("DELETE FROM tradings WHERE username = '@username' AND card_id = '@card_id'", connection))
            {
                cmd.Parameters.AddWithValue("username", username);
                cmd.Parameters.AddWithValue("cardId", cardId);
                cmd.ExecuteNonQuery();
            }
            using (var cmd = new NpgsqlCommand("DELETE FROM tradings WHERE username = '@otherusername' AND card_id = '@othercard_id'", connection))
            {
                cmd.Parameters.AddWithValue("otherusername", otherUsername);
                cmd.Parameters.AddWithValue("othercard_id", otherCardId);
                cmd.ExecuteNonQuery();
            }
            return true;
        }
    }
}
