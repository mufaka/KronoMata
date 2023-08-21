using Dapper;
using KronoMata.Model;

namespace KronoMata.Data.SQLite
{
    public class SQLiteHostDataStore : SQLiteDataStoreBase, IHostDataStore
    {
        public Host Create(Host host)
        {
            Execute((connection) =>
            {
                var sql = @"INSERT INTO Host 
(
	MachineName,
	IsEnabled,
	InsertDate,
	UpdateDate
)
VALUES 
(
	@MachineName,
	@IsEnabled,
	@InsertDate,
	@UpdateDate
);
select last_insert_rowid();";
                var id = connection.ExecuteScalar<int>(sql, new
                {
                    host.MachineName,
                    host.IsEnabled,
                    host.InsertDate,
                    host.UpdateDate
                });

                host.Id = id;
            });

            return host;
        }

        public void Delete(int id)
        {
            Execute(async (connection) =>
            {
                var sql = "delete from Host where Id = @Id;";
                await connection.ExecuteAsync(sql, new
                {
                    Id = id
                });
            });
        }

        public List<Host> GetAll()
        {
            return Query<Host>((connection) =>
            {
                var sql = @"SELECT 
	Id,
	MachineName,
	IsEnabled,
	InsertDate,
	UpdateDate
FROM Host
ORDER BY IsEnabled asc, MachineName asc;";
                return connection.Query<Host>(sql).ToList();
            });
        }

        public Host GetById(int id)
        {
            return QueryOne<Host>((connection) =>
            {
                var sql = @"SELECT 
	Id,
	MachineName,
	IsEnabled,
	InsertDate,
	UpdateDate
FROM Host
  WHERE Id = @Id;";

#pragma warning disable CS8603 // Possible null reference return.
                return connection.Query<Host>(sql, new
                {
                    Id = id
                }).FirstOrDefault();
#pragma warning restore CS8603 // Possible null reference return.
            });
        }

        public Host GetByMachineName(string machineName)
        {
            return QueryOne<Host>((connection) =>
            {
                var sql = @"SELECT 
	Id,
	MachineName,
	IsEnabled,
	InsertDate,
	UpdateDate
FROM Host
  WHERE MachineName = @MachineName;";

#pragma warning disable CS8603 // Possible null reference return.
                return connection.Query<Host>(sql, new
                {
                    MachineName = machineName
                }).FirstOrDefault();
#pragma warning restore CS8603 // Possible null reference return.
            });
        }

        public void Update(Host host)
        {
            Execute(async (connection) =>
            {
                var sql = @"UPDATE Host
SET
	MachineName = @MachineName,
	IsEnabled = @IsEnabled,
	InsertDate = @InsertDate,
	UpdateDate = @UpdateDate
WHERE Id = @Id;";

                await connection.ExecuteAsync(sql, new
                {
                    host.MachineName,
                    host.IsEnabled,
                    host.InsertDate,
                    host.UpdateDate,
                    host.Id
                });
            });
        }
    }
}
