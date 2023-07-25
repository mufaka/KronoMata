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
            var now = DateTime.Now;

            var package = new Package()
            {
                FileName = "FileName"
            };

            _provider.PackageDataStore.Create(package);

            Assert.IsTrue(1 == package.Id);
        }

        [Test()]
        public void Can_Delete()
        {
            var now = DateTime.Now;

            var package = new Package()
            {
                FileName = "FileName"
            };

            _provider.PackageDataStore.Create(package);

            Assert.IsTrue(1 == package.Id);

            _provider.PackageDataStore.Delete(package.Id);

            var existing = _provider.PackageDataStore.GetById(package.Id);

            Assert.IsNull(existing);
        }

        [Test()]
        public void Can_GetAll()
        {
            Assert.Fail();
        }

        [Test()]
        public void Can_GetById()
        {
            Assert.Fail();
        }

        [Test()]
        public void Can_Update()
        {
            var package = new Package()
            {
                FileName = "FileName"
            };

            _provider.PackageDataStore.Create(package);

            Assert.IsTrue(1 == package.Id);

            package.FileName = "UpdatedFileName";

            _provider.PackageDataStore.Update(package);

            var updated = _provider.PackageDataStore.GetById(package.Id);

            Assert.That(updated.FileName, Is.EqualTo("UpdatedFileName"));
        }
    }
}