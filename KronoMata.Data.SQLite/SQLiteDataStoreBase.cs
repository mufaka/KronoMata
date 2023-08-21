using Dapper;
using System.Data;
using System.Data.SQLite;

namespace KronoMata.Data.SQLite
{
    public abstract class SQLiteDataStoreBase : DbConnectionDataStoreBase
    {
        public override IDbConnection DbConnection
        {
            get
            {
                return new SQLiteConnection(ConnectionString);
            }
        }

        public void TruncateTable(string tableName)
        {
            Execute((connection) =>
            {
                var sql = $"delete from {tableName};";
                connection.Execute(sql);

                sql = "delete from sqlite_sequence where name = @tableName;";
                connection.Execute(sql, new
                {
                    tableName = tableName
                });
            });
        }
    }
}
