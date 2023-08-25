using Dapper;
using KronoMata.Model;
using System.Xml.Linq;

namespace KronoMata.Data.SQLite
{
    public class SQLiteGlobalConfigurationDataStore : SQLiteDataStoreBase, IGlobalConfigurationDataStore
    {
        public GlobalConfiguration Create(GlobalConfiguration globalConfiguration)
        {
            Execute((connection) =>
            {
                var sql = @"insert into GlobalConfiguration 
(
	Category,
	Name,
	Value,
	IsAccessibleToPlugins,
	IsMasked,
	IsSystemConfiguration,
	InsertDate,
	UpdateDate
)
values 
(
	@Category,
	@Name,
	@Value,
	@IsAccessibleToPlugins,
	@IsMasked,
	@IsSystemConfiguration,
	@InsertDate,
	@UpdateDate
);
select last_insert_rowid();";
                var id = connection.ExecuteScalar<int>(sql, new
                {
                    globalConfiguration.Category,
                    globalConfiguration.Name,
                    globalConfiguration.Value,
                    globalConfiguration.IsAccessibleToPlugins,
                    globalConfiguration.IsMasked,
                    globalConfiguration.IsSystemConfiguration,
                    globalConfiguration.InsertDate,
                    globalConfiguration.UpdateDate
                });

                globalConfiguration.Id = id;
            });

            return globalConfiguration;
        }

        public void Delete(int id)
        {
            var existing = GetById(id);

            if (existing.IsSystemConfiguration)
            {
                throw new InvalidOperationException("Deletion of system configuration is not allowed.");
            }

            Execute((connection) =>
            {
                var sql = "delete from GlobalConfiguration where Id = @Id";
                connection.Execute(sql, new
                {
                    Id = id
                });
            });
        }

        public List<GlobalConfiguration> GetAll()
        {
            return Query<GlobalConfiguration>((connection) =>
            {
                var sql = @"SELECT Id,
       Category,
       Name,
       Value,
       IsAccessibleToPlugins,
       IsMasked,
       IsSystemConfiguration,
       InsertDate,
       UpdateDate
  FROM GlobalConfiguration;";
                return connection.Query<GlobalConfiguration>(sql).ToList();
            });
        }

        public List<GlobalConfiguration> GetByCategory(string categoryName)
        {
            return Query<GlobalConfiguration>((connection) =>
            {
                var sql = @"SELECT Id,
       Category,
       Name,
       Value,
       IsAccessibleToPlugins,
       IsMasked,
       IsSystemConfiguration,
       InsertDate,
       UpdateDate
  FROM GlobalConfiguration
  WHERE Category = @Category;";
                return connection.Query<GlobalConfiguration>(sql, new { Category = categoryName }).ToList();
            });
        }

        public GlobalConfiguration GetByCategoryAndName(string category, string name)
        {
            return QueryOne<GlobalConfiguration>((connection) =>
            {
                var sql = @"SELECT Id,
       Category,
       Name,
       Value,
       IsAccessibleToPlugins,
       IsMasked,
       IsSystemConfiguration,
       InsertDate,
       UpdateDate
  FROM GlobalConfiguration
  WHERE Category = @Category
  AND Name = @Name;";

#pragma warning disable CS8603 // Possible null reference return.
                return connection.Query<GlobalConfiguration>(sql, new
                {
                    Category = category,
                    Name = name
                }).FirstOrDefault();
#pragma warning restore CS8603 // Possible null reference return.
            });
        }

        public GlobalConfiguration GetById(int id)
        {
            return QueryOne<GlobalConfiguration>((connection) =>
            {
                var sql = @"SELECT Id,
       Category,
       Name,
       Value,
       IsAccessibleToPlugins,
       IsMasked,
       IsSystemConfiguration,
       InsertDate,
       UpdateDate
  FROM GlobalConfiguration
  WHERE Id = @Id;";

#pragma warning disable CS8603 // Possible null reference return.
                return connection.Query<GlobalConfiguration>(sql, new
                {
                    Id = id
                }).FirstOrDefault();
#pragma warning restore CS8603 // Possible null reference return.
            });
        }

        public void Update(GlobalConfiguration globalConfiguration)
        {
            Execute((connection) =>
            {
                var sql = @"UPDATE GlobalConfiguration
   SET 
       Category = @Category,
       Name = @Name,
       Value = @Value,
       IsAccessibleToPlugins = @IsAccessibleToPlugins,
       IsMasked = @IsMasked,
       IsSystemConfiguration = @IsSystemConfiguration,
       InsertDate = @InsertDate,
       UpdateDate = @UpdateDate
 WHERE Id = @Id;";

                connection.Execute(sql, new
                {
                    globalConfiguration.Category,
                    globalConfiguration.Name,
                    globalConfiguration.Value,
                    globalConfiguration.IsAccessibleToPlugins,
                    globalConfiguration.IsMasked,
                    globalConfiguration.IsSystemConfiguration,
                    globalConfiguration.InsertDate,
                    globalConfiguration.UpdateDate,
                    globalConfiguration.Id
                });
            });
        }
    }
}
