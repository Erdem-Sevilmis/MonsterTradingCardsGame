﻿using MonsterTradingCardsGame;
using MonsterTradingCardsGame.SWE1.MessageServer.Models.Card;
using MonsterTradingCardsGame.SWE1.MessageServer.Models.User;
using Npgsql;
using System.Diagnostics.CodeAnalysis;
using System.Net.Http.Headers;
using System.Net.NetworkInformation;

namespace SWE1.MessageServer.DAL
{
    public class DataBaseGameDao
    {
        private NpgsqlConnection connection;

        public DataBaseGameDao()
        {
            DataBase db = new DataBase();
            connection = db.connection;
        }
        public UserStats GetStats(User identity)
        {
            using var cmd = new NpgsqlCommand("SELECT * FROM stats WHERE name=@name", connection);
            cmd.Parameters.AddWithValue("@name", identity.Credentials.Username);
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

        internal void UpdateStats(User user, bool won)
        {
            string query = String.Empty;
            if (won)
                query = "UPDATE stats SET elo = elo+5, wins = wins+1 WHERE name = @name";
            else
                query = "UPDATE stats SET elo = elo-5, losses = losses+1 WHERE name = @name";

            using var cmd = new NpgsqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@name", user.Credentials.Username);
            cmd.ExecuteNonQuery();
        }
    }
}