using Dapper;
using KronoMata.Model;

namespace KronoMata.Data.SQLite
{
    public class SQLitePackageDataStore : SQLiteDataStoreBase, IPackageDataStore
    {
        public Package Create(Package package)
        {
            Execute((connection) =>
            {
                var sql = @"INSERT INTO Package 
(
	Name,
	FileName,
	UploadDate
)
VALUES (
	@Name,
	@FileName,
	@UploadDate
);
select last_insert_rowid();";
                var id = connection.ExecuteScalar<int>(sql, new
                {
                    package.Name,
                    package.FileName,
                    package.UploadDate
                });

                package.Id = id;
            });

            return package;
        }

        public void Delete(int id)
        {
            /*
                Package
                    Plugin
                        PluginConfiguration
                        ScheduledJob
                            ConfigurationValue
                            JobHistory

            */

            // JobHistory
            Execute((connection) =>
            {
                var sql = @"delete from JobHistory where ScheduledJobId in
(
    select sj.Id
    from ScheduledJob sj
    join PluginMetaData p on p.Id = sj.PluginMetaDataId
    where p.PackageId = @PackageId 
)";
                connection.Execute(sql, new
                {
                    PackageId = id
                });
            });

            // ConfigurationValue
            Execute((connection) =>
            {
                var sql = @"delete from ConfigurationValue where ScheduledJobId in
(
    select sj.Id
    from ScheduledJob sj
    join PluginMetaData p on p.Id = sj.PluginMetaDataId
    where p.PackageId = @PackageId 
)";
                connection.Execute(sql, new
                {
                    PackageId = id
                });
            });

            // ScheduledJob
            Execute((connection) =>
            {
                var sql = @"delete from ScheduledJob where Id in
(
    select sj.Id
    from ScheduledJob sj
    join PluginMetaData p on p.Id = sj.PluginMetaDataId
    where p.PackageId = @PackageId 
)";
                connection.Execute(sql, new
                {
                    PackageId = id
                });
            });

            // PluginConfiguration
            Execute((connection) =>
            {
                var sql = @"delete from PluginConfiguration where PluginMetaDataId in
(
    select p.Id
    from PluginMetaData p
    where p.PackageId = @PackageId
)";
                connection.Execute(sql, new
                {
                    PackageId = id
                });
            });

            // Plugin
            Execute((connection) =>
            {
                var sql = @"delete from PluginMetaData where PackageId = @PackageId";

                connection.Execute(sql, new
                {
                    PackageId = id
                });
            });

            // Package
            Execute((connection) =>
            {
                var sql = "delete from Package where Id = @Id;";
                connection.Execute(sql, new
                {
                    Id = id
                });
            });
        }

        public List<Package> GetAll()
        {
            return Query<Package>((connection) =>
            {
                var sql = @"SELECT 
	Id,
	Name,
	FileName,
	UploadDate
FROM Package
ORDER BY Name asc;";
                return connection.Query<Package>(sql).ToList();
            });
        }

        public Package GetById(int id)
        {
            return QueryOne<Package>((connection) =>
            {
                var sql = @"SELECT 
	Id,
	Name,
	FileName,
	UploadDate
FROM Package
  WHERE Id = @Id;";

#pragma warning disable CS8603 // Possible null reference return.
                return connection.Query<Package>(sql, new
                {
                    Id = id
                }).FirstOrDefault();
#pragma warning restore CS8603 // Possible null reference return.
            });
        }

        public void Update(Package package)
        {
            Execute((connection) =>
            {
                var sql = @"UPDATE Package
SET
   Name = @Name,
   FileName = @FileName,
   UploadDate = @UploadDate
WHERE Id = @Id;";

                connection.Execute(sql, new
                {
                    package.Name,
                    package.FileName,
                    package.UploadDate,
                    package.Id
                });
            });
        }
    }
}
