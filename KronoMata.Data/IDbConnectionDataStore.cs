using System.Data;

namespace KronoMata.Data
{
    /// <summary>
    /// Represents a DataStore that utilizes IDbConnection
    /// as it's provider.
    /// </summary>
    public interface IDbConnectionDataStore
    {
        /// <summary>
        /// The IDbConnection to use when connecting to the
        /// data store.
        /// </summary>
        IDbConnection DbConnection { get; }
    }
}
