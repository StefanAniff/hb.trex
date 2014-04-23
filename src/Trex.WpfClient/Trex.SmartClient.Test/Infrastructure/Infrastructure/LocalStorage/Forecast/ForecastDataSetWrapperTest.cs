using System.Collections.Generic;
using System.IO.IsolatedStorage;
using System.Threading;
using NUnit.Framework;
using Ploeh.AutoFixture;
using Trex.SmartClient.Core.Interfaces;
using Trex.SmartClient.Data;
using Trex.SmartClient.Infrastructure.Implemented.LocalStorage.Forecast;
using Trex.SmartClient.Test.TestUtil;
using System.Linq;

namespace Trex.SmartClient.Test.Infrastructure.Infrastructure.LocalStorage.Forecast
{
    [TestFixture]
    public class ForecastDataSetWrapperTest : AutoFixtureTestBase
    {
        [Test]
        public void CanSaveAndLoadDataSet()
        {
            // Arrange
            var isoProviderMock = FreezeMock<IIsolatedStorageFileProvider>();
            isoProviderMock.Setup(x => x.GetIsolatedStorage()).Returns(IsolatedStorageFile.GetUserStoreForAssembly());

            var sut = Fixture.Create<ForecastDataSetWrapper>();

            // Act
            sut.ForecastDataSet.ForecastUserSearchPresets.Add(new ForecastUserSearchPresetData
                {
                    Name = "Some Preset",
                    UserIds = new List<int> { 1,2 }
                });

            sut.Save();

            Thread.Sleep(5000);

            // Assert
            sut.Load();
            var result = sut.ForecastDataSet.ForecastUserSearchPresets.Single();
            Assert.That(result.Name, Is.EqualTo("Some Preset"));
            Assert.That(result.UserIds.Count, Is.EqualTo(2));
            Assert.That(result.UserIds[0], Is.EqualTo(1));
            Assert.That(result.UserIds[1], Is.EqualTo(2));

            sut.DeleteFile();
        }
    }
}