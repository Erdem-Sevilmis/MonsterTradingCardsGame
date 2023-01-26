using MonsterTradingCardsGame;
using MonsterTradingCardsGame.SWE1.MessageServer.Models.Card;
using MonsterTradingCardsGame.SWE1.MessageServer.Models.User;
using Npgsql;
using System.Net.NetworkInformation;

namespace SWE1.MessageServer.DAL
{
    internal class DataBaseGameDao
    {
        private NpgsqlConnection connection;

        public DataBaseGameDao()
        {
            DataBase db = new DataBase();
            connection = db.connection;
        }
        public UserStats GetStats(User identity)
        {
            using var cmd = new NpgsqlCommand("SELECT * FROM stats WHERE username=@username", connection);
            cmd.Parameters.AddWithValue("@username", identity.Credentials.Username);
            using var reader = cmd.ExecuteReader();
            if (!reader.Read())
                return null;

            string name = reader["name"].ToString();
            int elo = (int)reader["elo"];
            int wins = (int)reader["wins"];
            int losses = (int)reader["losses"];
            return new UserStats(name, elo, wins, losses);

        }

        internal List<UserStats> GetScoreBoard(User identity)
        {
            List<UserStats> scoreboard = new List<UserStats>();
            using var cmd = new NpgsqlCommand("SELECT * FROM stats", connection);
            cmd.Parameters.AddWithValue("@username", identity.Credentials.Username);
            using var reader = cmd.ExecuteReader();
            if (!reader.Read())
                return null;
            while (reader.Read())
            {
                string name = reader["name"].ToString();
                int elo = (int)reader["elo"];
                int wins = (int)reader["wins"];
                int losses = (int)reader["losses"];
                var userstats = new UserStats(name, elo, wins, losses);
                scoreboard.Add(userstats);
            }
            scoreboard.Sort((x, y) => y.Elo.CompareTo(x.Elo));
            return scoreboard;
        }
    }
}