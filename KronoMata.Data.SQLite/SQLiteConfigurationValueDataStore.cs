using Dapper;
using KronoMata.Model;

namespace KronoMata.Data.SQLite
{
    public class SQLiteConfigurationValueDataStore : SQLiteDataStoreBase, IConfigurationValueDataStore
    {
        public ConfigurationValue Create(ConfigurationValue configurationValue)
        {
            Execute((connection) =>
            {
                var sql = @"insert into ConfigurationValue 
(
   ScheduledJobId,
   PluginConfigurationId,
   Value,
   InsertDate,
   UpdateDate
)
values 
(
   @ScheduledJobId,
   @PluginConfigurationId,
   @Value,
   @InsertDate,
   @UpdateDate
);
select last_insert_rowid();
";
                var id = connection.ExecuteScalar<int>(sql, new 
                {
                    configurationValue.ScheduledJobId,
                    configurationValue.PluginConfigurationId,
                    configurationValue.Value,
                    configurationValue.InsertDate,
                    configurationValue.UpdateDate
                });

                configurationValue.Id = id;
            });

            return configurationValue;
        }

        public void Delete(int id)
        {
            Execute((connection) =>
            {
                var sql = "delete from ConfigurationValue where Id = @Id;";
                connection.Execute(sql, new
                {
                    Id = id
                });
            });
        }

        public List<ConfigurationValue> GetByScheduledJob(int scheduledJobId)
        {
            return Query<ConfigurationValue>((connection) =>
            {
                var sql = "select Id, ScheduledJobId, PluginConfigurationId, Value, InsertDate, UpdateDate from ConfigurationValue where ScheduledJobId = @ScheduledJobId;";
                return connection.Query<ConfigurationValue>(sql, new
                {
                    ScheduledJobId = scheduledJobId
                }).ToList();
            });
        }

        public void Update(ConfigurationValue configurationValue)
        {
            Execute((connection) =>
            {
                var sql = @"update ConfigurationValue set ScheduledJobId = @ScheduledJobId, PluginConfigurationId = @PluginConfigurationId, Value = @Value, InsertDate = @InsertDate, UpdateDate = @UpdateDate
where Id = @Id;";
                connection.Execute(sql, new
                {
                    configurationValue.ScheduledJobId,
                    configurationValue.PluginConfigurationId,
                    configurationValue.Value,
                    configurationValue.InsertDate,
                    configurationValue.UpdateDate,
                    configurationValue.Id
                });
            });
        }
    }
}
