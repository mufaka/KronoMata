using Dapper;
using KronoMata.Model;

namespace KronoMata.Data.SQLite
{
    public class SQLiteJobHistoryDataStore : SQLiteDataStoreBase, IJobHistoryDataStore
    {
        public JobHistory Create(JobHistory jobHistory)
        {
            Execute((connection) =>
            {
                var sql = @"INSERT INTO JobHistory 
(
	ScheduledJobId,
	HostId,
	Status,
	Message,
	Detail,
	RunTime,
	CompletionTime
)
VALUES (
	@ScheduledJobId,
	@HostId,
	@Status,
	@Message,
	@Detail,
	@RunTime,
	@CompletionTime
);
select last_insert_rowid();";
                var id = connection.ExecuteScalar<int>(sql, new
                {
                    jobHistory.ScheduledJobId,
                    jobHistory.HostId,
                    jobHistory.Status,
                    jobHistory.Message,
                    jobHistory.Detail,
                    jobHistory.RunTime,
                    jobHistory.CompletionTime 
                });

                jobHistory.Id = id;
            });

            return jobHistory;
        }

        public void Delete(int id)
        {
            Execute((connection) =>
            {
                var sql = "delete from JobHistory where Id = @Id;";
                connection.Execute(sql, new
                {
                    Id = id
                });
            });
        }

        public List<JobHistory> GetAll()
        {
            return Query<JobHistory>((connection) =>
            {
                var sql = @"SELECT 
    Id,
	ScheduledJobId,
	HostId,
	Status,
	Message,
	Detail,
	RunTime,
	CompletionTime
FROM JobHistory
ORDER BY RunTime desc;";

                return connection.Query<JobHistory>(sql).ToList();
            });
        }

        public PagedList<JobHistory> GetAllPaged(int pageIndex, int pageSize)
        {
            var pagedList = new PagedList<JobHistory>();

            var skip = pageIndex * pageSize;
            var sql = $@"SELECT 
    Id,
	ScheduledJobId,
	HostId,
	Status,
	Message,
	Detail,
	RunTime,
	CompletionTime
FROM JobHistory
WHERE Id NOT IN (SELECT Id FROM JobHistory ORDER BY RunTime desc LIMIT {skip})
ORDER BY RunTime desc LIMIT {pageSize};";

            Execute((connection) =>
            {
                pagedList.TotalRecords = connection.ExecuteScalar<int>("select count(*) from JobHistory;");
            });

            pagedList.List = Query<JobHistory>((connection) =>
            {
                return connection.Query<JobHistory>(sql).ToList();
            });

            return pagedList;
        }

        public PagedList<JobHistory> GetByHost(int hostId, int pageIndex, int pageSize)
        {
            return GetFilteredPaged(pageIndex, pageSize, -1, -1, hostId);
        }

        public PagedList<JobHistory> GetByScheduledJob(int scheduledJobId, int pageIndex, int pageSize)
        {
            return GetFilteredPaged(pageIndex, pageSize, -1, scheduledJobId, -1);
        }

        public PagedList<JobHistory> GetFilteredPaged(int pageIndex, int pageSize, int status, int scheduledJobId, int hostId)
        {
            var pagedList = new PagedList<JobHistory>();

            var skip = pageIndex * pageSize;
            var sql = $@"SELECT 
    Id,
	ScheduledJobId,
	HostId,
	Status,
	Message,
	Detail,
	RunTime,
	CompletionTime
FROM JobHistory
WHERE Id NOT IN (SELECT Id FROM JobHistory 
WHERE (@HostId = -1 or HostId = @HostId)
AND (@Status = -1 or Status = @Status)
AND (@ScheduledJobId = -1 or ScheduledJobId = @ScheduledJobId)
ORDER BY RunTime desc LIMIT {skip})
AND (@HostId = -1 or HostId = @HostId)
AND (@Status = -1 or Status = @Status)
AND (@ScheduledJobId = -1 or ScheduledJobId = @ScheduledJobId)
ORDER BY RunTime desc LIMIT {pageSize};";

            var totalRecordsSql = @"SELECT count(*)
FROM JobHistory
WHERE (@HostId = -1 or HostId = @HostId)
AND (@Status = -1 or Status = @Status)
AND (@ScheduledJobId = -1 or ScheduledJobId = @ScheduledJobId);";

            Execute((connection) =>
            {
                pagedList.TotalRecords = connection.ExecuteScalar<int>(totalRecordsSql, new
                {
                    HostId = hostId,
                    Status = status,
                    ScheduledJobId = scheduledJobId
                });
            });

            pagedList.List = Query<JobHistory>((connection) =>
            {
                return connection.Query<JobHistory>(sql, new
                {
                    HostId = hostId,
                    Status = status,
                    ScheduledJobId = scheduledJobId
                }).ToList();
            });

            return pagedList;
        }

        public List<JobHistory> GetLastByDate(DateTime startDate)
        {
            return Query<JobHistory>((connection) =>
            {
                var sql = @"SELECT 
    Id,
	ScheduledJobId,
	HostId,
	Status,
	Message,
	Detail,
	RunTime,
	CompletionTime
FROM JobHistory
WHERE RunTime > @StartDate
ORDER BY RunTime desc;";

                return connection.Query<JobHistory>(sql, new { StartDate = startDate }).ToList();
            });
        }

        public PagedList<JobHistory> GetLastByDatePaged(DateTime startDate, int pageIndex, int pageSize)
        {
            var pagedList = new PagedList<JobHistory>();

            var skip = pageIndex * pageSize;
            var sql = $@"SELECT 
    Id,
	ScheduledJobId,
	HostId,
	Status,
	Message,
	Detail,
	RunTime,
	CompletionTime
FROM JobHistory
WHERE Id NOT IN (SELECT Id FROM JobHistory where RunTime > @StartDate ORDER BY RunTime desc LIMIT {skip})
AND RunTime > @StartDate
ORDER BY RunTime desc LIMIT {pageSize};";

            Execute((connection) =>
            {
                pagedList.TotalRecords = connection.ExecuteScalar<int>("select count(*) from JobHistory WHERE RunTime > @StartDate;", new { StartDate = startDate });
            });

            pagedList.List = Query<JobHistory>((connection) =>
            {
                return connection.Query<JobHistory>(sql, new { StartDate = startDate }).ToList();
            });

            return pagedList;
        }

        public List<JobHistory> GetTop(int howMany)
        {
            return Query<JobHistory>((connection) =>
            {
                var sql = $@"SELECT 
    Id,
	ScheduledJobId,
	HostId,
	Status,
	Message,
	Detail,
	RunTime,
	CompletionTime
FROM JobHistory
ORDER BY RunTime desc
LIMIT {howMany};";

                return connection.Query<JobHistory>(sql).ToList();
            });
        }
    }
}
