using Npgsql;
using System.Globalization;
using MonsterTradingCardsGame.SWE1.MessageServer.Models.Card;
using static System.Net.Mime.MediaTypeNames;
using System.Xml.Linq;
using System;
using static SWE1.MessageServer.Models.TradingDeal;

namespace MonsterTradingCardsGame
{
    public partial class DataBase
    {
        public NpgsqlConnection connection;
        private const string CONNECTIONSTRING = "Host=localhost;Username=admin;Password=admin;Database=dbmtcg";
        public DataBase()
        {
            this.connection = new NpgsqlConnection(CONNECTIONSTRING);
            NpgsqlConnection.GlobalTypeMapper.MapEnum<Card_Type>();
            NpgsqlConnection.GlobalTypeMapper.MapEnum<ElementType>();
            NpgsqlConnection.GlobalTypeMapper.MapEnum<SpellMonster>();
            connection.Open();
        }
        public void Start()
        {
            InsertIntoTableCard(connection, Card_Type.Kraken, 4.0f);
            InsertIntoTableCard(connection, Card_Type.WaterElf, 3.8f);
            InsertIntoTableCard(connection, Card_Type.Knight, 1.5f);
            InsertIntoTableCard(connection, Card_Type.Dragon, 2.2f);
            /*
            CreateTradingDeal(connection, "testuser01", Guid.Parse("bd2bb180-471d-4f2d-93b6-ae0ce8ca020a"));
            var card_ids = ReadFromTableCard(connection);
            CreatePackage(connection, card_ids.ToArray());
            AddPackageToPlayer(connection, "testuser01");
            //InsertIntoTableUser_Account(connection, "testuser01", "testpassword01");
            foreach (var card_id in card_ids)
                UpdateStackInTableUser_Account(connection, "testuser01", card_id);

            ReadFromTableUser_Account(connection);


            //Create package with id of cards
            CreatePackage(connection, new Guid[4] { card_ids[0], card_ids[1], card_ids[2], card_ids[3] });

            DeleteFromTableUser_Account(connection, "testuser01");
            foreach (var card_id in card_ids)
                DeleteFromTableCard(connection, card_id);
            */
        }
        private bool CreateTradingDeal(NpgsqlConnection connection, string username, Guid cardId)
        {
            //check if user owns card
            //check if card is not in user deck
            //make entry

            //check if card is already in system
            using (var cmd = new NpgsqlCommand("SELECT * FROM tradings where card_id=@cardId", connection))
            {
                // Add the parameter and its value
                cmd.Parameters.AddWithValue("@cardId", cardId);
                // Execute the update statement
                using var reader = cmd.ExecuteReader();
                if (reader.HasRows)
                    return false;
            }
            //check if user owns card
            //check if card is not in user deck
            //DatabaseCardDao cardDao = new DatabaseCardDao();

            using (var cmd = new NpgsqlCommand("INSERT INTO tradings (card_id , username ) VALUES (@card_id, @username )", connection))
            {
                // Add the parameter and its value
                cmd.Parameters.AddWithValue("@card_id", cardId);
                cmd.Parameters.AddWithValue("@username", username);
                cmd.ExecuteNonQuery();
                return true;
            }
        }
        private string UserLogin(NpgsqlConnection connection)
        {
            string finalusername = string.Empty;
            bool userCredentialsNotFound = true;
            while (userCredentialsNotFound)
            {
                // Prompt the user for their credentials
                Console.Write("Enter your username: ");
                string username = Console.ReadLine();
                Console.Write("Enter your password: ");
                string password = Console.ReadLine();
                // Validate the input
                if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
                {
                    Console.WriteLine("Invalid input. Please enter a valid username and password.");
                    continue;
                }

                if (username.Length < 5 || username.Length > 20)
                {
                    Console.WriteLine("Invalid input. Please enter a username between 5 and 20 characters.");
                    continue;
                }

                if (password.Length < 8 || password.Length > 20)
                {
                    Console.WriteLine("Invalid input. Please enter a password between 8 and 20 characters.");
                    continue;
                }

                // Prepare the insert statement
                using var cmd = new NpgsqlCommand("INSERT INTO user_account (username, password) VALUES (@username, @password)", connection)
                {
                    Parameters =
                    {
                        new("username", username),
                        new("password", password)
                    }
                };
                try
                {
                    if (cmd.ExecuteNonQuery() != -1)
                    {
                        userCredentialsNotFound = false;
                        finalusername = username;
                        break;
                    }
                    Console.WriteLine("Invalid User credentials. Please try again.");
                }
                catch (Exception e)
                {
                    Console.WriteLine("Something went wrong:" + e.Message);
                    continue;
                }
            }
            return finalusername;
        }
        private void AddPackageToPlayer(NpgsqlConnection connection, string username)
        {
            Random rnd = new Random();
            int package_id;
            List<Guid> cardIds;
            using (var cmd = new NpgsqlCommand("SELECT MAX(package_id) FROM card_package;", connection))
            {
                using (var reader = cmd.ExecuteReader())
                {
                    int max = 0;
                    if (reader.Read())
                        max = (int)reader["max"];
                    package_id = rnd.Next(0, max + 1);
                }
            }

            using (var cmd = new NpgsqlCommand("SELECT * FROM card_package WHERE package_id=@package_id;", connection)
            {
                Parameters =
                {
                    new("package_id",package_id)
                }
            })
            {
                cardIds = new List<Guid>();
                using var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    //reader["card_id"]
                    cardIds.Add((Guid)reader["card_id"]);
                }
            }
            InsertCardToUserStack(username, cardIds.ToArray());
        }
        private void InsertCardToUserStack(string username, Guid[] cardIds)
        {
            // Create a new NpgsqlCommand
            using (var cmd = new NpgsqlCommand("UPDATE user_account SET stack = array_cat(stack, @newCards) WHERE username = @username", connection))
            {
                // Add the parameter and its value
                cmd.Parameters.AddWithValue("@newCards", cardIds);
                cmd.Parameters.AddWithValue("@username", username);

                // Execute the update statement
                cmd.ExecuteNonQuery();
            }
        }
        private void CreatePackage(NpgsqlConnection connection, Guid[] card_ids)
        {
            //INSERT INTO package default values returning id;(cmd.executescala())
            Console.WriteLine("INSERT INTO TABLE USER_ACCOUNT");
            int package_id;
            using (var cmd = new NpgsqlCommand("INSERT INTO package default values returning id", connection)) { package_id = (int)cmd.ExecuteScalar(); }

            using (var cmd = new NpgsqlCommand("INSERT INTO card_package (package_id, card_id) VALUES (@package_id, @card_id_1),(@package_id, @card_id_2),(@package_id, @card_id_3),(@package_id, @card_id_4)", connection)
            {
                Parameters =
                {
                    new("package_id",package_id),
                    new("card_id_1",card_ids[0]),
                    new("card_id_2",card_ids[1]),
                    new("card_id_3",card_ids[2]),
                    new("card_id_4",card_ids[3]),
                }
            })
            {
                cmd.ExecuteNonQuery();
            }


            //var package_id = int.Parse((string)cmd.ExecuteScalar());
        }

        private void UpdateStackInTableUser_Account(NpgsqlConnection connection, string username, Guid card_id)
        {
            Console.WriteLine("UPDATE TABLE USER_ACCOUNT");
            /*
            cmd.CommandText = $"UPDATE user_account SET stack = array_append(stack, '{card_id}') WHERE username = '{username}'";
            cmd.ExecuteNonQuery();
            */
            using var cmd = new NpgsqlCommand("UPDATE user_account SET stack = array_append(stack, '{@card_id}') WHERE username = '{@username}'", connection)
            {
                Parameters =
                {
                    new("card_id", card_id),
                    new("username", username)
                }
            };
            cmd.ExecuteNonQuery();
        }

        private void InsertIntoTableUser_Account(NpgsqlConnection connection, string username, string password)
        {
            Console.WriteLine("INSERT INTO TABLE USER_ACCOUNT");
            using var cmd = new NpgsqlCommand("INSERT INTO user_account VALUES (@username, @password)", connection)
            {
                Parameters =
                {
                    new("username", username),
                    new("password", password)
                }
            };
            cmd.ExecuteNonQuery();
        }

        private void ReadFromTableUser_Account(NpgsqlConnection connection)
        {
            Console.WriteLine("SELECT * FROM TABLE USER_ACCOUNT");
            /*
            cmd.CommandText = "SELECT * FROM user_account";
            NpgsqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                Console.WriteLine($"USERNAME: {reader["username"]} PASSWORD: {reader["password"]}");
                string[] stack = (string[])reader["stack"];
                string[] deck = (string[])reader["deck"];
                foreach (var item in stack)
                    Console.Write(item + " ");

                Console.WriteLine();
                foreach (var item in deck)
                    Console.Write(item + " ");

                Console.WriteLine();
            }
            reader.Close();
            */
            using var cmd = new NpgsqlCommand("SELECT * FROM user_account", connection);
            cmd.ExecuteNonQuery();
        }

        private void DeleteFromTableUser_Account(NpgsqlConnection connection, string username)
        {
            /*
            Console.WriteLine("DELETE FROM TABLE USER_ACCOUNT");
            cmd.CommandText = $"DELETE FROM user_account WHERE username = '{username}'";
            cmd.ExecuteNonQuery();

            */
            using var cmd = new NpgsqlCommand("DELETE FROM user_account WHERE username = '{@username}'", connection)
            {
                Parameters =
                {
                    new("username", username),
                }
            };
            cmd.ExecuteNonQuery();
        }

        private void InsertIntoTableCard(NpgsqlConnection connection, Card_Type name, float damage)
        {
            NumberFormatInfo nfi = new NumberFormatInfo();
            nfi.NumberDecimalSeparator = ".";
            var card_id = Guid.NewGuid();

            Console.WriteLine("INSERT INTO TABLE CARD");
            /*
            using var cmd = new NpgsqlCommand($"INSERT INTO card(card_id,name, damage) VALUES('{card_id}', '{name}',{damage})", connection);
            cmd.ExecuteNonQuery();
            */
            using var cmd = new NpgsqlCommand("INSERT INTO card VALUES (@card_id, @name, @damage)", connection)
            {
                Parameters =
                {
                    new("card_id", card_id),
                    new("name", name),
                    new("damage", damage)
                }
            };
            cmd.ExecuteNonQuery();
        }

        private List<Guid> ReadFromTableCard(NpgsqlConnection connection)
        {
            Console.WriteLine("SELECT * FROM TABLE CARD");
            /*
            cmd.CommandText = "SELECT * FROM card";
            NpgsqlDataReader reader = cmd.ExecuteReader();
            */
            using var cmd = new NpgsqlCommand("SELECT * FROM card", connection);
            NpgsqlDataReader reader = cmd.ExecuteReader();


            List<Guid> card_ids = new List<Guid>();
            Guid card_id;
            while (reader.Read())
            {
                card_id = (Guid)reader["card_id"];
                Console.WriteLine($"NAME: {reader["name"]} DAMAGE: {reader["damage"]} CARD_ID: {card_id}");
                card_ids.Add(card_id);
            }
            reader.Close();
            return card_ids;
        }

        private void DeleteFromTableCard(NpgsqlConnection connection, Guid card_id)
        {
            Console.WriteLine("DELETE FROM TABLE CARD");
            /*
            cmd.CommandText = $"DELETE FROM card WHERE card_id = '{card_id}'";
            cmd.ExecuteNonQuery();
            */

            using var cmd = new NpgsqlCommand("DELETE FROM card WHERE card_id = '{@card_id}'\"", connection)
            {
                Parameters =
                {
                    new("card_id", card_id),
                }
            };
            cmd.ExecuteNonQuery();
        }
    }
}