using KronoMata.Data.Mock;
using KronoMata.Model;

namespace KronoMata.Data.InMemory
{
    public class InMemoryPackageDataStore : InMemoryDataStoreBase, IPackageDataStore
    {
        public InMemoryPackageDataStore(MockDataStoreProvider inMemoryDataStoreProvider, IDataStoreProvider backingDataStoreProvider)
            : base(inMemoryDataStoreProvider, backingDataStoreProvider) 
        {
            ((MockPackageDataStore)inMemoryDataStoreProvider.PackageDataStore)
                .Initialize(backingDataStoreProvider.PackageDataStore.GetAll());
        }

        public Package Create(Package package)
        {
            var createdPackage = BackingDataStoreProvider.PackageDataStore.Create(package);
            InMemoryDataStoreProvider.PackageDataStore.Create(createdPackage);
            return createdPackage;
        }

        public void Delete(int id)
        {
            BackingDataStoreProvider.PackageDataStore.Delete(id);
            InMemoryDataStoreProvider.PackageDataStore.Delete(id);
        }

        public List<Package> GetAll()
        {
            return InMemoryDataStoreProvider.PackageDataStore.GetAll();
        }

        public Package GetById(int id)
        {
            return InMemoryDataStoreProvider.PackageDataStore.GetById(id);
        }

        public void Update(Package package)
        {
            BackingDataStoreProvider.PackageDataStore.Update(package);
            InMemoryDataStoreProvider.PackageDataStore.Update(package);
        }
    }
}
