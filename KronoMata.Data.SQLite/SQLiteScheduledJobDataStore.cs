using Dapper;
using KronoMata.Model;

namespace KronoMata.Data.SQLite
{
    public class SQLiteScheduledJobDataStore : SQLiteDataStoreBase, IScheduledJobDataStore
    {
        public ScheduledJob Create(ScheduledJob scheduledJob)
        {
            Execute((connection) =>
            {
                var sql = @"INSERT INTO ScheduledJob 
(
	PluginMetaDataId,
	HostIds,
	Name,
	Description,
	Frequency,
	Interval,
	Days,
	DaysOfWeek,
	Hours,
	Minutes,
	StartTime,
	EndTime,
	IsEnabled,
	InsertDate,
	UpdateDate
)
VALUES 
(
	@PluginMetaDataId,
	@HostIds,
	@Name,
	@Description,
	@Frequency,
	@Interval,
	@Days,
	@DaysOfWeek,
	@Hours,
	@Minutes,
	@StartTime,
	@EndTime,
	@IsEnabled,
	@InsertDate,
	@UpdateDate
);
select last_insert_rowid();";
                var id = connection.ExecuteScalar<int>(sql, new
                {
                    scheduledJob.PluginMetaDataId,
                    scheduledJob.HostIds,
                    scheduledJob.Name,
                    scheduledJob.Description,
                    scheduledJob.Frequency,
                    scheduledJob.Interval,
                    scheduledJob.Days,
                    scheduledJob.DaysOfWeek,
                    scheduledJob.Hours,
                    scheduledJob.Minutes,
                    scheduledJob.StartTime,
                    scheduledJob.EndTime,
                    scheduledJob.IsEnabled,
                    scheduledJob.InsertDate,
                    scheduledJob.UpdateDate
                });

                scheduledJob.Id = id;
            });

            return scheduledJob;
        }

        public void Delete(int id)
        {
            Execute((connection) =>
            {
                var sql = "delete from ScheduledJob where Id = @Id;";
                connection.Execute(sql, new
                {
                    Id = id
                });
            });
        }

        public List<ScheduledJob> GetAll()
        {
            return Query<ScheduledJob>((connection) =>
            {
                var sql = @"SELECT 
	Id,
	PluginMetaDataId,
	HostIds,
	Name,
	Description,
	Frequency,
	Interval,
	Days,
	DaysOfWeek,
	Hours,
	Minutes,
	StartTime,
	EndTime,
	IsEnabled,
	InsertDate,
	UpdateDate
FROM ScheduledJob
ORDER BY Name asc;";

                return connection.Query<ScheduledJob>(sql).ToList();
            });
        }

        public List<ScheduledJob> GetByHost(int hostId)
        {
            return Query<ScheduledJob>((connection) =>
            {
                var sql = @"SELECT 
	Id,
	PluginMetaDataId,
	HostIds,
	Name,
	Description,
	Frequency,
	Interval,
	Days,
	DaysOfWeek,
	Hours,
	Minutes,
	StartTime,
	EndTime,
	IsEnabled,
	InsertDate,
	UpdateDate
FROM ScheduledJob
WHERE ((',' || HostIds || ',' like @HostId) or HostIds = '-1')
ORDER BY Name asc;";

                return connection.Query<ScheduledJob>(sql, new { HostId = $"%,{hostId},%" }).ToList();
            });
        }

        public ScheduledJob GetById(int id)
        {
            return QueryOne<ScheduledJob>((connection) =>
            {
                var sql = @"SELECT 
	Id,
	PluginMetaDataId,
	HostIds,
	Name,
	Description,
	Frequency,
	Interval,
	Days,
	DaysOfWeek,
	Hours,
	Minutes,
	StartTime,
	EndTime,
	IsEnabled,
	InsertDate,
	UpdateDate
FROM ScheduledJob
WHERE Id = @Id;";

#pragma warning disable CS8603 // Possible null reference return.
                return connection.Query<ScheduledJob>(sql, new
                {
                    Id = id
                }).FirstOrDefault();
#pragma warning restore CS8603 // Possible null reference return.
            });
        }

        public List<ScheduledJob> GetByPluginMetaData(int pluginMetaDataId)
        {
            return Query<ScheduledJob>((connection) =>
            {
                var sql = @"SELECT 
	Id,
	PluginMetaDataId,
	HostIds,
	Name,
	Description,
	Frequency,
	Interval,
	Days,
	DaysOfWeek,
	Hours,
	Minutes,
	StartTime,
	EndTime,
	IsEnabled,
	InsertDate,
	UpdateDate
FROM ScheduledJob
WHERE PluginMetaDataId = @PluginMetaDataId
ORDER BY Name asc;";

                return connection.Query<ScheduledJob>(sql, new { PluginMetaDataId = pluginMetaDataId }).ToList();
            });
        }

        public void Update(ScheduledJob scheduledJob)
        {
            Execute((connection) =>
            {
                var sql = @"UPDATE ScheduledJob
SET
	PluginMetaDataId = @PluginMetaDataId,
	HostIds = @HostIds,
	Name = @Name,
	Description = @Description,
	Frequency = @Frequency,
	Interval = @Interval,
	Days = @Days,
	DaysOfWeek = @DaysOfWeek,
	Hours = @Hours,
	Minutes = @Minutes,
	StartTime = @StartTime,
	EndTime = @EndTime,
	IsEnabled = @IsEnabled,
	InsertDate = @InsertDate,
	UpdateDate = @UpdateDate
WHERE 
	Id = @Id";

                connection.Execute(sql, new
                {
                    scheduledJob.PluginMetaDataId,
                    scheduledJob.HostIds,
                    scheduledJob.Name,
                    scheduledJob.Description,
                    scheduledJob.Frequency,
                    scheduledJob.Interval,
                    scheduledJob.Days,
                    scheduledJob.DaysOfWeek,
                    scheduledJob.Hours,
                    scheduledJob.Minutes,
                    scheduledJob.StartTime,
                    scheduledJob.EndTime,
                    scheduledJob.IsEnabled,
                    scheduledJob.InsertDate,
                    scheduledJob.UpdateDate,
                    scheduledJob.Id
                });
            });
        }
    }
}
