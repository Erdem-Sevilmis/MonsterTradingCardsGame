using MonsterTradingCardsGame;
using MonsterTradingCardsGame.SWE1.MessageServer.Models.User;
using Npgsql;
using SWE1.MessageServer.API.RouteCommands.Users;
using SWE1.MessageServer.BLL.user;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using static System.Net.Mime.MediaTypeNames;

namespace SWE1.MessageServer.DAL
{
    internal class DatabaseUserDao : IUserDao
    {
        private const string InsertUserCommand = "INSERT INTO user_account(username, password, coins) VALUES(@username, @password, @coins)";
        private const string UpdateUserCommand = "UPDATE user_account SET username = @new_username, password = @new_password WHERE username = @old_username";
        private const string SelectUserByCredentialsCommand = "SELECT username, password, coins FROM user_account WHERE username=@username AND password=@password";
        private NpgsqlConnection connection;

        public DatabaseUserDao()
        {
            DataBase db = new DataBase();
            connection = db.connection;
        }

        private List<User> GetAllUsers()
        {
            var users = new List<User>();
            using var cmd = new NpgsqlCommand("SELECT * FROM user_account", connection);
            using NpgsqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                users.Add(new User(Convert.ToString(reader["username"]), Convert.ToString(reader["password"]), (int)reader["coins"]));
            }

            return users;
        }

        public User? GetUserByAuthToken(string authToken)
        {
            return GetAllUsers().SingleOrDefault(u => u.Token == authToken);
        }

        public User? GetUserByCredentials(string username, string password)
        {
            using var cmd = new NpgsqlCommand(SelectUserByCredentialsCommand, connection)
            {
                Parameters =
                {
                    new("username", username),
                    new("password", password)
                }
            };
            try
            {
                using NpgsqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                    return new User(Convert.ToString(reader["username"]), Convert.ToString(reader["password"]), (int)reader["coins"]);
            }
            catch (Exception)
            {
            }
            return null;
        }


        public bool InsertUser(User user)
        {
            using var cmd = new NpgsqlCommand(InsertUserCommand, connection)
            {
                Parameters =
                {
                    new("username", user.Credentials.Username),
                    new("password", user.Credentials.Password),
                    new("coins", user.Coins)
                }
            };
            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (PostgresException)
            {
                // this might happen, if the user already exists (constraint violation)
                return false;
            }
            InsertStats(user);
            return true;
        }

        private void InsertStats(User user)
        {
            using var cmd = new NpgsqlCommand("INSERT INTO stats(name, elo, wins,losses) VALUES(@name, @elo, @wins, @losses)", connection)
            {
                Parameters =
                {
                    new("name", user.Credentials.Username),
                    new("elo", 100),
                    new("wins",DbType.Int32){Value=0},
                    new("losses",DbType.Int32){Value=0}
                }
            };
            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (PostgresException)
            {
                // this might happen, if the user already exists (constraint violation)
            }
        }
        public UserData GetUserData(User identity)
        {
            UserData userData;
            using (var cmd = new NpgsqlCommand("SELECT name,bio,image FROM user_account WHERE username=@username", connection))
            {
                cmd.Parameters.AddWithValue("username", identity.Credentials.Username);
                using NpgsqlDataReader reader = cmd.ExecuteReader();
                if (!reader.HasRows)
                {
                    throw new UserNotFoundException();
                }
                if (!reader.Read())
                    return null;
                
                userData = new UserData(reader["name"].ToString(), reader["bio"].ToString(), reader["image"].ToString());
                return userData;
            }
        }
        public void UpdateUserData(User identity, UserData userdata)
        {
            using (var cmd = new NpgsqlCommand("SELECT * FROM user_account", connection))
            {
                using NpgsqlDataReader reader = cmd.ExecuteReader();
                if (!reader.HasRows)
                {
                    throw new UserNotFoundException();
                }
            }


            using (var cmd = new NpgsqlCommand("UPDATE user_account SET name=@name,bio=@bio,image=@image WHERE username=@username", connection))
            {
                cmd.Parameters.AddWithValue("name", userdata.Name);
                cmd.Parameters.AddWithValue("bio", userdata.Bio);
                cmd.Parameters.AddWithValue("image", userdata.Image);
                cmd.Parameters.AddWithValue("username", identity.Credentials.Username);

                try
                {
                    cmd.ExecuteNonQuery();

                }
                catch (Exception)
                {
                }
            }
        }

    }
}
