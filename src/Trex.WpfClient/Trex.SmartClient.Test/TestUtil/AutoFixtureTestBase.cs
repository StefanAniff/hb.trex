using System;
using System.Reflection;
using Moq;
using NUnit.Framework;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.AutoMoq;
using Ploeh.AutoFixture.Kernel;

namespace Trex.SmartClient.Test.TestUtil
{
    public class AutoFixtureTestBase
    {
        private IFixture _fixture;

        public IFixture Fixture
        {
            get { return _fixture ?? (_fixture = InitializeFixture()); }
        }

        public IFixture InitializeFixture()
        {
            _fixture = new Fixture().Customize(new AutoMoqCustomization());
            return Fixture;
        }

        /// <summary>
        /// If fixture.Freeze<Mock<T>> is used, Mock.Callbase is set to true,
        /// which will give you a bad time working on mocks of implementation.
        /// Thus this manual CallBase setting
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public Mock<T> FreezeMock<T>() where T : class
        {
            var mock = CreateMock<T>();
            _fixture.Inject(mock.Object);
            return mock;
        }

        public Mock<T> CreateMock<T>() where T : class
        {
            var mock = _fixture.Create<Mock<T>>();
            mock.CallBase = false;
            return mock;
        }

        [SetUp]
        public void Setup()
        {
            InitializeFixture();
        }
    }
}