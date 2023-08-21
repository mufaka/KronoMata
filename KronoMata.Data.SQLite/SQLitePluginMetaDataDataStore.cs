using Dapper;
using KronoMata.Model;

namespace KronoMata.Data.SQLite
{
    public class SQLitePluginMetaDataDataStore : SQLiteDataStoreBase, IPluginMetaDataDataStore
    {
        public PluginMetaData Create(PluginMetaData pluginMetaData)
        {
            Execute((connection) =>
            {
                var sql = @"INSERT INTO PluginMetaData 
(
	PackageId,
	Name,
	Description,
	Version,
	AssemblyName,
	ClassName,
	InsertDate,
	UpdateDate
)
VALUES 
(
	@PackageId,
	@Name,
	@Description,
	@Version,
	@AssemblyName,
	@ClassName,
	@InsertDate,
	@UpdateDate
);
select last_insert_rowid();";

                var id = connection.ExecuteScalar<int>(sql, new
                {
                    pluginMetaData.PackageId,
                    pluginMetaData.Name,
                    pluginMetaData.Description,
                    pluginMetaData.Version,
                    pluginMetaData.AssemblyName,
                    pluginMetaData.ClassName,
                    pluginMetaData.InsertDate,
                    pluginMetaData.UpdateDate
                });

                pluginMetaData.Id = id;
            });

            return pluginMetaData;
        }

        public void Delete(int id)
        {
            Execute(async (connection) =>
            {
                var sql = "delete from PluginMetaData where Id = @Id;";
                await connection.ExecuteAsync(sql, new
                {
                    Id = id
                });
            });
        }

        public List<PluginMetaData> GetAll()
        {
            return Query<PluginMetaData>((connection) =>
            {
                var sql = @"SELECT 
	Id,
	PackageId,
	Name,
	Description,
	Version,
	AssemblyName,
	ClassName,
	InsertDate,
	UpdateDate
FROM PluginMetaData;";
                return connection.Query<PluginMetaData>(sql).ToList();
            });
        }

        public PluginMetaData GetById(int id)
        {
            return QueryOne<PluginMetaData>((connection) =>
            {
                var sql = @"SELECT 
	Id,
	PackageId,
	Name,
	Description,
	Version,
	AssemblyName,
	ClassName,
	InsertDate,
	UpdateDate
FROM PluginMetaData
WHERE Id = @Id;";

#pragma warning disable CS8603 // Possible null reference return.
                return connection.Query<PluginMetaData>(sql, new
                {
                    Id = id
                }).FirstOrDefault();
#pragma warning restore CS8603 // Possible null reference return.
            });
        }

        public List<PluginMetaData> GetByPackageId(int packageId)
        {
            return Query<PluginMetaData>((connection) =>
            {
                var sql = @"SELECT 
	Id,
	PackageId,
	Name,
	Description,
	Version,
	AssemblyName,
	ClassName,
	InsertDate,
	UpdateDate
FROM PluginMetaData
WHERE PackageId = @PackageId;";
                return connection.Query<PluginMetaData>(sql, new { PackageId = packageId }).ToList();
            });
        }

        public void Update(PluginMetaData pluginMetaData)
        {
            Execute(async (connection) =>
            {
                var sql = @"UPDATE PluginMetaData
SET
	PackageId = @PackageId,
	Name = @Name,
	Description = @Description,
	Version = @Version,
	AssemblyName = @AssemblyName,
	ClassName = @ClassName,
	InsertDate = @InsertDate,
	UpdateDate = @UpdateDate
WHERE 
	Id = @Id;";

                await connection.ExecuteAsync(sql, new
                {
                    pluginMetaData.PackageId,
                    pluginMetaData.Name,
                    pluginMetaData.Description,
                    pluginMetaData.Version,
                    pluginMetaData.AssemblyName,
                    pluginMetaData.ClassName,
                    pluginMetaData.InsertDate,
                    pluginMetaData.UpdateDate,
                    pluginMetaData.Id
                });
            });
        }
    }
}
