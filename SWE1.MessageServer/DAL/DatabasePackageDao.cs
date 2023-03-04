using MonsterTradingCardsGame;
using MonsterTradingCardsGame.SWE1.MessageServer.Models.Card;
using MonsterTradingCardsGame.SWE1.MessageServer.Models.User;
using Npgsql;
using SWE1.MessageServer.API.RouteCommands.packages;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static MonsterTradingCardsGame.DataBase;

namespace SWE1.MessageServer.DAL
{
    public class DatabasePackageDao
    {
        private const string CreateNewCardPackage = "INSERT INTO card_package (package_id, card_id) VALUES (@package_id, @card_id_1),(@package_id, @card_id_2),(@package_id, @card_id_3),(@package_id, @card_id_4),(@package_id, @card_id_5)";
        private const string CreateNewPackage = "INSERT INTO package default values returning id";
        private const string AcquireCardPackageForUser = "UPDATE user_account SET username = @new_username, password = @new_password WHERE username = @old_username";
        private NpgsqlConnection connection;
        public DatabasePackageDao()
        {
            DataBase db = new DataBase();
            connection = db.connection;
        }

        public void CreatePackage(Card[] cards)
        {
            int package_id;
            using (var cmd = new NpgsqlCommand(CreateNewPackage, connection)) { package_id = (int)cmd.ExecuteScalar(); }
            using (var cmd = new NpgsqlCommand(CreateNewCardPackage, connection)
            {
                Parameters =
                {
                    new("package_id",package_id),
                    new("card_id_1",cards[0].Id),
                    new("card_id_2",cards[1].Id),
                    new("card_id_3",cards[2].Id),
                    new("card_id_4",cards[3].Id),
                    new("card_id_5",cards[4].Id),
                }
            })
            {
                try
                {
                    foreach (var card in cards)
                    {
                        InsertIntoTableCard(card);
                    }
                    cmd.ExecuteNonQuery();
                }
                catch (Npgsql.PostgresException)
                {
                    throw new AtLeastOneCardInThePackageAlreadyExistsException();
                }
            }
        }
        public List<Card> AddPackageToPlayer(User user)
        {
            int package_id;
            List<Guid> cardIds;
            List<int> package_ids = new List<int>();
            using (var cmd = new NpgsqlCommand("Select distinct package_id from card_package order by package_id asc;", connection))
            {
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                        package_ids.Add((int)reader["package_id"]);
                }
            }
            if (package_ids.Count < 1)
                throw new NoPackageAvailableException();


            package_id = package_ids[0];
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
                    cardIds.Add((Guid)reader["card_id"]);
                }
            }
            InsertCardToUserStack(user.Credentials.Username, cardIds.ToArray());
            DeletePackageId(package_id);
            BoughtPackage(user);
            DatabaseCardDao cardDao = new DatabaseCardDao();
            List<Card> cards = new List<Card>();
            foreach (var cardId in cardIds)
            {
                cards.Add(cardDao.GetCard(cardId));
            }
            return cards;
        }
        private void DeletePackageId(int package_id)
        {
            using (var cmd = new NpgsqlCommand("DELETE FROM card_package WHERE package_id=@package_id;", connection)
            {
                Parameters =
                {
                    new("package_id",package_id)
                }
            })
            {
                cmd.ExecuteNonQuery();
            }
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
        private void InsertIntoTableCard(Card card)
        {
            using var cmd = new NpgsqlCommand("INSERT INTO card VALUES (@card_id, @name, @element,@damage)", connection)
            {
                Parameters =
                {
                    new("card_id", card.Id),
                    new("name", card.Name),
                    new("element", card.ElementType),
                    new("damage", card.Damage)
                }
            };
            cmd.ExecuteNonQuery();
        }
        private void BoughtPackage(User user)
        {
            using (var cmd = new NpgsqlCommand("UPDATE user_account SET coins = coins - 5 WHERE username = @username", connection))
            {
                cmd.Parameters.AddWithValue("@username", user.Credentials.Username);
                cmd.ExecuteNonQuery();
            }
        }

    }
}
