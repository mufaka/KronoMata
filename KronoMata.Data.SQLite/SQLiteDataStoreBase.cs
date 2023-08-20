using Microsoft.Data.Sqlite;
using System.Data;

namespace KronoMata.Data.SQLite
{
    public abstract class SQLiteDataStoreBase : DbConnectionDataStoreBase
    {
        public override IDbConnection DbConnection
        {
            get
            {
                return new SqliteConnection(ConnectionString);
            }
        }
    }
}
