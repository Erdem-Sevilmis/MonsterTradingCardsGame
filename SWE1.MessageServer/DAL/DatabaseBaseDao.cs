using Npgsql;

namespace SWE1.MessageServer.DAL
{
    internal abstract class DatabaseBaseDao
    {
        private readonly string _connectionString;

        public DatabaseBaseDao(string connectionString)
        {
            _connectionString = connectionString;
        }

        protected T ExecuteWithDbConnection<T>(Func<NpgsqlConnection, T> command)
        {
            try
            {
                using var connection = new NpgsqlConnection(_connectionString);
                connection.Open();

                return command(connection);
            }
            catch (NpgsqlException e)
            {
                throw new DataAccessFailedException("Could not connect to database", e);
            }
        }
    }
}
