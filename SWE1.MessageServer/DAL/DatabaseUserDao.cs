using MonsterTradingCardsGame;
using MonsterTradingCardsGame.SWE1.MessageServer.Models.User;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWE1.MessageServer.DAL
{
    internal class DatabaseUserDao : IUserDao
    {
        private const string InsertUserCommand = "INSERT INTO user_account(username, password) VALUES(@username, @password)";
        private const string UpdateUserCommand = "UPDATE user_account SET username = @new_username, password = @new_password WHERE username = @old_username";
        private const string SelectUserByCredentialsCommand = "SELECT username, password FROM user_account WHERE username=@username AND password=@password";
        private NpgsqlConnection connection;

        public DatabaseUserDao(string connectionString)
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
                users.Add(new User(Convert.ToString(reader["username"]), Convert.ToString(reader["password"])));

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
                    return new User(Convert.ToString(reader["username"]), Convert.ToString(reader["password"]));
            }
            catch (Exception)
            {
            }
            return null;
        }
        
        public bool UpdateUser(User user, string oldUsername, string oldPassword)
        {
            var oldUser = GetUserByCredentials(oldUsername, oldPassword);
            if (oldUser == null)
                return false;
            
            using var cmd = new NpgsqlCommand(UpdateUserCommand, connection)
            {
                Parameters =
                {
                    new("new_username", user.Credentials.Username),
                    new("new_password", user.Credentials.Password),
                    new("old_username", oldUsername)
                }
            };
            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (Exception)
            {
                // this might happen, if the user already exists (constraint violation)
                return false;
            }
            return true;
        }
        
        public bool InsertUser(User user)
        {
            using var cmd = new NpgsqlCommand(InsertUserCommand, connection)
            {
                Parameters =
                {
                    new("username", user.Credentials.Username),
                    new("password", user.Credentials.Password)
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
            return true;
        }
    }
}
