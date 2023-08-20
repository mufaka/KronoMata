using Dapper;
using KronoMata.Model;

namespace KronoMata.Data.SQLite
{
    public class SQLiteGlobalConfigurationDataStore : SQLiteDataStoreBase, IGlobalConfigurationDataStore
    {
        public GlobalConfiguration Create(GlobalConfiguration globalConfiguration)
        {
            Execute((connection) =>
            {
                var sql = @"insert into GlobalConfiguration () values ()";
                var id = connection.ExecuteScalar<int>(sql);

                globalConfiguration.Id = id;
            });

            return globalConfiguration;
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public List<GlobalConfiguration> GetAll()
        {
            throw new NotImplementedException();
        }

        public List<GlobalConfiguration> GetByCategory(string categoryName)
        {
            throw new NotImplementedException();
        }

        public GlobalConfiguration GetByCategoryAndName(string category, string name)
        {
            throw new NotImplementedException();
        }

        public GlobalConfiguration GetById(int id)
        {
            throw new NotImplementedException();
        }

        public void Update(GlobalConfiguration globalConfiguration)
        {
            throw new NotImplementedException();
        }
    }
}
