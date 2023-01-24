using MonsterTradingCardsGame;
using Npgsql;
using System;
using System.Collections.Generic;
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
        public List<Guid> GetUserCards(string username)=> Enumerable.Concat(GetUserStack(username), GetUserDeck(username)).ToList();

        public List<Guid> GetUserDeck(string username)
        {
            using var cmd = new NpgsqlCommand("SELECT deck FROM user_account WHERE username = @username", connection);
            cmd.Parameters.AddWithValue("@username", username);
            using var reader = cmd.ExecuteReader();
            List<Guid> deck = new List<Guid>();
            while (reader.Read())
                deck.Add((Guid)reader["deck"]);

            return deck;
        }

        public List<Guid> GetUserStack(string username)
        {
            using var cmd = new NpgsqlCommand("SELECT stack FROM user_account WHERE username = @username", connection);
            cmd.Parameters.AddWithValue("@username", username);
            using var reader = cmd.ExecuteReader();
            List<Guid> stack = new List<Guid>();
            while (reader.Read())
                stack.Add((Guid)reader["stack"]);
            return stack;
        }

        public bool ConfigureNewDeck(string username, Guid[] cardIds)
        {
            using var cmd = new NpgsqlCommand("UPDATE user_account SET deck = @deck WHERE username = @username", connection);
            cmd.Parameters.AddWithValue("@username", username);
            cmd.Parameters.AddWithValue("@deck", cardIds);
            return cmd.ExecuteNonQuery() == 1;
        }
    }
}
