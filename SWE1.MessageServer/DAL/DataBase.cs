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
    }
}