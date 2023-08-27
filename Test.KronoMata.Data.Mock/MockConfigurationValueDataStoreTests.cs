using KronoMata.Data;
using KronoMata.Data.Mock;
using Test.KronoMata.Data.Base;

namespace Test.KronoMata.Data.Mock
{
    [TestFixture()]
    public class MockConfigurationValueDataStoreTests : ConfigurationValueDataStoreTestsBase
    {
        private IDataStoreProvider _provider;

        protected override IDataStoreProvider DataStoreProvider { get { return _provider; } }

        [SetUp]
        public void Setup()
        {
            _provider = new MockDataStoreProvider();
        }
    }
}