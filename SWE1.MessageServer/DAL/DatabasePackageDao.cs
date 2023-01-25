using MonsterTradingCardsGame;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWE1.MessageServer.DAL
{
    internal class DatabasePackageDao
    {
        private const string CreateNewCardPackage = "INSERT INTO card_package (package_id, card_id) VALUES (@package_id, @card_id_1),(@package_id, @card_id_2),(@package_id, @card_id_3),(@package_id, @card_id_4)";
        private const string CreateNewPackage = "INSERT INTO package default values returning id";
        private const string AcquireCardPackageForUser = "UPDATE user_account SET username = @new_username, password = @new_password WHERE username = @old_username";
        private NpgsqlConnection connection;
        public DatabasePackageDao()
        {
            DataBase db = new DataBase();
            connection = db.connection;
        }
        public void CreatePackage(Guid[] card_ids)
        {
            //TODO: Admin Check throw new UserNotAdminException();
            int package_id;
            using (var cmd = new NpgsqlCommand(CreateNewPackage, connection)) { package_id = (int)cmd.ExecuteScalar(); }
            //TODO: throw new AtLeastOneCardInThePackageAlreadyExistsException();
            using (var cmd = new NpgsqlCommand(CreateNewCardPackage, connection)
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
                try
                {
                    cmd.ExecuteNonQuery();
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }
        public void AddPackageToPlayer(string username)
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
            using (var cmd = new NpgsqlCommand("UPDATE user_account SET stack = array_cat(stack, @newCards) WHERE username = @username", connection))
            {
                // Add the parameter and its value
                cmd.Parameters.AddWithValue("@newCards", cardIds);
                cmd.Parameters.AddWithValue("@username", username);

                // Execute the update statement
                cmd.ExecuteNonQuery();
            }
        }

    }
}
