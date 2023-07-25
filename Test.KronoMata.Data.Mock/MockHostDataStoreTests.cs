﻿using KronoMata.Data;
using KronoMata.Data.Mock;
using KronoMata.Model;

namespace Test.KronoMata.Data.Mock
{
    [TestFixture()]
    public class MockHostDataStoreTests
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
            Assert.Fail();
        }

        [Test()]
        public void Can_Delete()
        {
            Assert.Fail();
        }

        [Test()]
        public void Can_GetAll()
        {
            Assert.Fail();
        }

        [Test()]
        public void Can_GetByMachineName()
        {
            Assert.Fail();
        }

        [Test()]
        public void Can_Update()
        {
            Assert.Fail();
        }
    }
}