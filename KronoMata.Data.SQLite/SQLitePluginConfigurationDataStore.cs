using Dapper;
using KronoMata.Model;

namespace KronoMata.Data.SQLite
{
    public class SQLitePluginConfigurationDataStore : SQLiteDataStoreBase, IPluginConfigurationDataStore
    {
        public PluginConfiguration Create(PluginConfiguration pluginConfiguration)
        {
            Execute((connection) =>
            {
                var sql = @"INSERT INTO PluginConfiguration 
(
	PluginMetaDataId,
	DataType,
	Name,
	Description,
	IsRequired,
	SelectValues,
	InsertDate,
	UpdateDate
)
VALUES 
(
	@PluginMetaDataId,
	@DataType,
	@Name,
	@Description,
	@IsRequired,
	@SelectValues,
	@InsertDate,
	@UpdateDate
);
select last_insert_rowid();";
                var id = connection.ExecuteScalar<int>(sql, new
                {
                    pluginConfiguration.PluginMetaDataId,
                    pluginConfiguration.DataType,
                    pluginConfiguration.Name,
                    pluginConfiguration.Description,
                    pluginConfiguration.IsRequired,
                    pluginConfiguration.SelectValues,
                    pluginConfiguration.InsertDate,
                    pluginConfiguration.UpdateDate
                });

                pluginConfiguration.Id = id;
            });

            return pluginConfiguration;
        }

        public void Delete(int id)
        {
            Execute((connection) =>
            {
                var sql = "delete from PluginConfiguration where Id = @Id;";
                connection.Execute(sql, new
                {
                    Id = id
                });
            });
        }

        public PluginConfiguration GetById(int id)
        {
            return QueryOne<PluginConfiguration>((connection) =>
            {
                var sql = @"SELECT 
	Id,
	PluginMetaDataId,
	DataType,
	Name,
	Description,
	IsRequired,
	SelectValues,
	InsertDate,
	UpdateDate
FROM PluginConfiguration
WHERE Id = @Id;";

#pragma warning disable CS8603 // Possible null reference return.
                return connection.Query<PluginConfiguration>(sql, new
                {
                    Id = id
                }).FirstOrDefault();
#pragma warning restore CS8603 // Possible null reference return.
            });
        }

        public List<PluginConfiguration> GetByPluginMetaData(int pluginMetaDataId)
        {
            return Query<PluginConfiguration>((connection) =>
            {
                var sql = @"SELECT 
	Id,
	PluginMetaDataId,
	DataType,
	Name,
	Description,
	IsRequired,
	SelectValues,
	InsertDate,
	UpdateDate
FROM PluginConfiguration
WHERE PluginMetaDataId = @PluginMetaDataId;";
                return connection.Query<PluginConfiguration>(sql, new { PluginMetaDataId = pluginMetaDataId }).ToList();
            });
        }

        public void Update(PluginConfiguration pluginConfiguration)
        {
            Execute((connection) =>
            {
                var sql = @"UPDATE PluginConfiguration
SET
	PluginMetaDataId = @PluginMetaDataId,
	DataType = @DataType,
	Name = @Name,
	Description = @Description,
	IsRequired = @IsRequired,
	SelectValues = @SelectValues,
	InsertDate = @InsertDate,
	UpdateDate = @UpdateDate
WHERE 
	Id = @Id";

                connection.Execute(sql, new
                {
                    pluginConfiguration.PluginMetaDataId,
                    pluginConfiguration.DataType,
                    pluginConfiguration.Name,
                    pluginConfiguration.Description,
                    pluginConfiguration.IsRequired,
                    pluginConfiguration.SelectValues,
                    pluginConfiguration.InsertDate,
                    pluginConfiguration.UpdateDate,
                    pluginConfiguration.Id

                });
            });
        }
    }
}
