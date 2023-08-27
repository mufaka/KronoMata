using KronoMata.Data;
using KronoMata.Model;
using NUnit.Framework;

namespace Test.KronoMata.Data.Base
{
    [TestFixture]
    public abstract class PackageDataStoreTestsBase
    {
        protected abstract IDataStoreProvider DataStoreProvider { get; }

        [Test()]
        public void Can_Create()
        {
            var package = new Package()
            {
                FileName = "FileName"
            };

            DataStoreProvider.PackageDataStore.Create(package);

            Assert.That(package.Id, Is.EqualTo(1));
        }

        [Test()]
        public void Can_Delete()
        {
            var package = new Package()
            {
                FileName = "FileName"
            };

            DataStoreProvider.PackageDataStore.Create(package);

            Assert.That(package.Id, Is.EqualTo(1));

            DataStoreProvider.PackageDataStore.Delete(package.Id);

            var existing = DataStoreProvider.PackageDataStore.GetById(package.Id);

            Assert.That(existing, Is.Null);
        }

        [Test()]
        public void Can_GetAll()
        {
            for (int x = 0; x < 10; x++)
            {
                var package = new Package()
                {
                    FileName = $"FileName{x + 1}"
                };

                DataStoreProvider.PackageDataStore.Create(package);
            }

            var all = DataStoreProvider.PackageDataStore.GetAll();
            Assert.That(all, Has.Count.EqualTo(10));
        }

        [Test()]
        public void Can_GetById()
        {
            var package = new Package()
            {
                FileName = "FileName"
            };

            DataStoreProvider.PackageDataStore.Create(package);

            Assert.That(package.Id, Is.EqualTo(1));

            var existing = DataStoreProvider.PackageDataStore.GetById(1);

            Assert.That(existing, Is.Not.Null);
            Assert.That(existing.Id, Is.EqualTo(1));
        }

        [Test()]
        public void Can_Update()
        {
            var package = new Package()
            {
                FileName = "FileName"
            };

            DataStoreProvider.PackageDataStore.Create(package);

            Assert.That(package.Id, Is.EqualTo(1));

            package.FileName = "UpdatedFileName";

            DataStoreProvider.PackageDataStore.Update(package);

            var updated = DataStoreProvider.PackageDataStore.GetById(package.Id);

            Assert.That(updated.FileName, Is.EqualTo("UpdatedFileName"));
        }
    }
}