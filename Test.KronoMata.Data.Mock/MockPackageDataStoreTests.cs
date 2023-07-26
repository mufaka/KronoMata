using KronoMata.Data;
using KronoMata.Data.Mock;
using KronoMata.Model;

namespace Test.KronoMata.Data.Mock
{
    [TestFixture()]
    public class MockPackageDataStoreTests
    {
        private IDataStoreProvider _provider;

        [SetUp]
        public void Setup()
        {
            _provider = new MockDataStoreProvider();
        }

        [Test()]
        public void Can_Create()
        {
            var package = new Package()
            {
                FileName = "FileName"
            };

            _provider.PackageDataStore.Create(package);

            Assert.That(package.Id, Is.EqualTo(1));
        }

        [Test()]
        public void Can_Delete()
        {
            var package = new Package()
            {
                FileName = "FileName"
            };

            _provider.PackageDataStore.Create(package);

            Assert.That(package.Id, Is.EqualTo(1));

            _provider.PackageDataStore.Delete(package.Id);

            var existing = _provider.PackageDataStore.GetById(package.Id);

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

                _provider.PackageDataStore.Create(package);
            }

            var all = _provider.PackageDataStore.GetAll();
            Assert.That(all, Has.Count.EqualTo(10));
        }

        [Test()]
        public void Can_GetById()
        {
            var package = new Package()
            {
                FileName = "FileName"
            };

            _provider.PackageDataStore.Create(package);

            Assert.That(package.Id, Is.EqualTo(1));

            var existing = _provider.PackageDataStore.GetById(1);

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

            _provider.PackageDataStore.Create(package);

            Assert.That(package.Id, Is.EqualTo(1));

            package.FileName = "UpdatedFileName";

            _provider.PackageDataStore.Update(package);

            var updated = _provider.PackageDataStore.GetById(package.Id);

            Assert.That(updated.FileName, Is.EqualTo("UpdatedFileName"));
        }
    }
}