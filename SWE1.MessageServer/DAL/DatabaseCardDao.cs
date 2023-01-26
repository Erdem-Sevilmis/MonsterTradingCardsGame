using Microsoft.VisualBasic;
using MonsterTradingCardsGame;
using MonsterTradingCardsGame.SWE1.MessageServer.Models.Card;
using MonsterTradingCardsGame.SWE1.MessageServer.Models.User;
using Npgsql;
using SWE1.MessageServer.API.RouteCommands.cards;
using SWE1.MessageServer.API.RouteCommands.trading;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace SWE1.MessageServer.DAL
{
    internal class DatabaseCardDao
    {
        private NpgsqlConnection connection;

        public DatabaseCardDao()
        {
            DataBase db = new DataBase();
            connection = db.connection;
        }
        public List<Guid> GetUserCards(string username)
        {
            List<Guid> cards = new List<Guid>();

            var allcards = Enumerable.Concat(GetUserStack(username), GetUserDeck(username).Select((card) => card.Id));
            foreach (var card in allcards)
            {
                cards.Add(card);
            }
            return cards;
        }
        private Card GetCard(Guid card_id)
        {
            using var cmd = new NpgsqlCommand("SELECT * FROM card WHERE card_id=@card_id", connection);
            cmd.Parameters.AddWithValue("@card_id", card_id);
            using var reader = cmd.ExecuteReader();
            if (!reader.Read())
                return null;

            var id = (Guid)reader["card_id"];
            var damage = (float)reader["damage"];
            Enum.TryParse(reader["name"].ToString(), out DataBase.Card_Type name);
            Enum.TryParse(reader["element"].ToString(), out ElementType element);
            return new Card(name, damage, id, element);
        }
        public List<Card> GetUserDeck(string username)
        {
            using var cmd = new NpgsqlCommand("SELECT deck FROM user_account WHERE username = @username", connection);
            cmd.Parameters.AddWithValue("@username", username);
            using var reader = cmd.ExecuteReader();
            List<Guid> deck = new List<Guid>();

            while (reader.Read())
            {
                if (reader["deck"] == System.DBNull.Value)
                    break;

                deck = ((Guid[])reader["deck"]).ToList();
            }
            reader.Close();
            List<Card> cards = new List<Card>();
            foreach (var id in deck)
            {
                cards.Add(GetCard(id));
            }
            return cards;
        }
        public List<Guid> GetUserStack(string username)
        {
            using var cmd = new NpgsqlCommand("SELECT stack FROM user_account WHERE username = @username", connection);
            cmd.Parameters.AddWithValue("@username", username);
            using var reader = cmd.ExecuteReader();
            List<Guid> stack = new List<Guid>();
            while (reader.Read())
            {
                if (reader["stack"] == System.DBNull.Value)
                    break;

                stack = ((Guid[])reader["stack"]).ToList();
            }
            return stack;
        }

        public bool ConfigureNewDeck(User user, Guid[] cardIds)
        {
            if (cardIds.Length < 4)
                throw new NotEnoughCardsinDeckException();

            var cards = GetUserCards(user.Credentials.Username);
            if (!cardIds.All(elem => cards.Contains(elem)))
                throw new CardNotOwnedOrUnavailableException();

            using var cmd = new NpgsqlCommand("UPDATE user_account SET deck = @deck WHERE username = @username", connection);
            cmd.Parameters.AddWithValue("@username", user.Credentials.Username);
            cmd.Parameters.AddWithValue("@deck", cardIds);
            cmd.ExecuteNonQuery();
            user.Deck = cardIds.ToList();
            return true;
        }
    }
}

