using MonsterTradingCardsGame;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public bool CreateNewTradingDeal(NpgsqlConnection connection, string username, Guid cardId)
        {
            using (var cmd = new NpgsqlCommand("SELECT * FROM tradings where card_id=@cardId", connection))
            {
                // Add the parameter and its value
                cmd.Parameters.AddWithValue("@cardId", cardId);
                // Execute the update statement
                using var reader = cmd.ExecuteReader();
                if (reader.HasRows)
                    return false;
            }

            DatabaseCardDao cardDao = new DatabaseCardDao();
            var cards = cardDao.GetUserCards(username);
            if (cards.Contains(cardId))
            {
                var userDeck = cardDao.GetUserDeck(username);
                if (userDeck.Contains(cardId))
                    return false;
            }
            else
                return false;

            using (var cmd = new NpgsqlCommand("INSERT INTO tradings (card_id , username ) VALUES (@card_id, @username )", connection))
            {
                // Add the parameter and its value
                cmd.Parameters.AddWithValue("@card_id", cardId);
                cmd.Parameters.AddWithValue("@username", username);
                cmd.ExecuteNonQuery();
                return true;
            }
        }

        public List<Guid> GetTradingDeals(NpgsqlConnection connection, string username)
        {
            List<Guid> availableTrades = new List<Guid>();
            using (var cmd = new NpgsqlCommand("SELECT card_id FROM tradings where username != @username", connection))
            {
                // Add the parameter and its value
                cmd.Parameters.AddWithValue("username", username);
                using var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    availableTrades.Add((Guid)reader["card_id "]);
                }
            }
            return availableTrades;
        }

        public bool DeleteTradingDeal(NpgsqlConnection connection, string username, Guid cardId)
        {
            using (var cmd = new NpgsqlCommand("SELECT * FROM tradings where card_id=@cardId", connection))
            {
                // Add the parameter and its value
                cmd.Parameters.AddWithValue("@cardId", cardId);
                // Execute the update statement
                using var reader = cmd.ExecuteReader();
                if (!reader.HasRows)
                    return false;
            }

            DatabaseCardDao cardDao = new DatabaseCardDao();
            var cards = cardDao.GetUserCards(username);
            if (!cards.Contains(cardId))
                return false;

            using (var cmd = new NpgsqlCommand("DELETE FROM tradings where card_id=@cardId", connection))
            {
                cmd.Parameters.AddWithValue("@cardId", cardId);
                cmd.ExecuteNonQuery();
                return true;
            }
        }

        public bool Trade(NpgsqlConnection connection, string username, Guid cardId, Guid otherCardId)
        {
            string otherUsername = String.Empty;
            using (var cmd = new NpgsqlCommand("SELECT * FROM tradings where card_id=@cardId", connection))
            {
                // Add the parameter and its value
                cmd.Parameters.AddWithValue("@cardId", cardId);
                // Execute the update statement
                using var reader = cmd.ExecuteReader();
                if (!reader.HasRows)
                    return false;
            }
            DatabaseCardDao cardDao = new DatabaseCardDao();
            var cards = cardDao.GetUserCards(username);
            if (cards.Contains(cardId))
            {
                var userDeck = cardDao.GetUserDeck(username);
                if (userDeck.Contains(cardId))
                    return false;
            }
            else
                return false;

            using (var cmd = new NpgsqlCommand("SELECT * FROM tradings where card_id=@cardId", connection))
            {
                cmd.Parameters.AddWithValue("cardId", otherCardId);
                // Execute the update statement
                using var reader = cmd.ExecuteReader();
                if (reader.Read())
                    otherUsername = reader["username"].ToString();
            }

            if (otherUsername==null)
                return false;
            
            using (var cmd = new NpgsqlCommand("UPDATE user_account SET stack = array_remove(stack, '@otherCardId') WHERE username = @otherusername;", connection))
            {
                cmd.Parameters.AddWithValue("otherCardId", otherCardId);
                cmd.Parameters.AddWithValue("otherusername", otherUsername);
                cmd.ExecuteNonQuery();
            }
            using (var cmd = new NpgsqlCommand("UPDATE user_account SET stack = array_append(stack, @otherCardId) WHERE username = @otherusername", connection))
            {
                cmd.Parameters.AddWithValue("otherCardId",otherCardId);
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
