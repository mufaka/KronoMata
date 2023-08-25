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
