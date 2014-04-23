using System.Collections.Generic;
using System.Collections.ObjectModel;
using Moq;
using NUnit.Framework;
using Ploeh.AutoFixture;
using Trex.Common.DataTransferObjects;
using Trex.SmartClient.Forecast.ForecastOverview;
using Trex.SmartClient.Forecast.ForecastOverview.Helpers;
using Trex.SmartClient.Forecast.Shared;
using Trex.SmartClient.Test.TestUtil;
using System.Linq;

namespace Trex.SmartClient.Test.Forecast.ForecastOverview.Helpers
{
    [TestFixture]
    public class ForecastOverviewDataGeneratorTest : AutoFixtureTestBase
    {
        [Test]
        public void CreateForecastMonths_TwoMonthsInSource_MapsAsExpected()
        {
            // Arrange
            var types = Fixture.Create<ObservableCollection<OverviewForecastTypeOption>>();
            var source = Fixture.Create<List<ForecastMonthDto>>();
            var updated = new ForecastOverviewForecastMonths();
            var dates = Fixture.Create<ForecastDates>();

            var searchOptionsMock = CreateMock<ForecastOverviewSearchOptions>();

            var vmMock = Fixture.Create <Mock<IForecastOverviewViewModel>>();
            vmMock.SetupGet(x => x.ListOptions).Returns(new OverviewListOptions {ForecastTypeOptions = types});
            vmMock.SetupGet(x => x.Dates).Returns(dates);
            vmMock.SetupGet(x => x.SearchOptions).Returns(searchOptionsMock.Object);
            vmMock.SetupGet(x => x.UserRegistrations).Returns(updated);

            var sut = Fixture.Create<ForecastOverviewDataGenerator>();

            // Act
            sut.CreateForecastMonths(vmMock.Object, source, 1);

            // Assert
            Assert.That(updated.Count, Is.EqualTo(source.Count));
            foreach (var forecastDate in dates)
            {
                foreach (var userForecastMonth in updated)
                {
                    // Test that non-work-days are added
                    Assert.That(userForecastMonth.Forecasts.Select(x => x.Date).Contains(forecastDate), Is.True);
                }
            }
        }
    }
}