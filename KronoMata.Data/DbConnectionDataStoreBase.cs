using System.Data;

namespace KronoMata.Data
{
    /// <summary>
    /// A base class for IDbConnectionDataStore implementations that provides
    /// a generic means to handle DbConnection connections in a safe way.
    /// </summary>
    public abstract class DbConnectionDataStoreBase : IDbConnectionDataStore
    {
        /// <summary>
        /// The IDbConnection to use when connecting to the
        /// data store.
        /// </summary>
        abstract public IDbConnection DbConnection { get; }

        /// <summary>
        /// The database connection string to use when connecting
        /// to the IDbConnection implementation.
        /// </summary>
        public static string ConnectionString { get; set; } = String.Empty;

        /// <summary>
        /// Executes the provided action while managing the connection.
        /// </summary>
        /// <param name="action">The action to execute</param>
        protected void Execute(Action<IDbConnection> action)
        {
            using (var connection = DbConnection)
            {
                try
                {
                    connection.Open();
                    action(connection);
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        /// <summary>
        /// Executes the provided function while managing the connection.
        /// </summary>
        /// <typeparam name="T">The object type to fill.</typeparam>
        /// <param name="query">The query function.</param>
        /// <returns>Returns a single T or null</returns>
        protected T QueryOne<T>(Func<IDbConnection, T> query)
        {
            using (var connection = DbConnection)
            {
                try
                {
                    connection.Open();
                    return query(connection);
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        /// <summary>
        /// Executes the provided function while managing the connection.
        /// </summary>
        /// <typeparam name="T">The object type to fill.</typeparam>
        /// <param name="query">The query function.</param>
        /// <returns>A list of T.</returns>
        protected List<T> Query<T>(Func<IDbConnection, List<T>> query)
        {
            using (var connection = DbConnection)
            {
                try
                {
                    connection.Open();
                    var retVal = query(connection);
                    return retVal;
                }
                finally
                {
                    connection.Close();
                }
            }
        }
    }
}
