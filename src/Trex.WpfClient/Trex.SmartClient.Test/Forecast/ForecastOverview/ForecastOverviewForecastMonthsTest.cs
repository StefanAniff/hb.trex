using System.Collections.Generic;
using Moq;
using NUnit.Framework;
using Ploeh.AutoFixture;
using Trex.SmartClient.Forecast.ForecastOverview;
using Trex.SmartClient.Forecast.Shared;
using Trex.SmartClient.Test.TestUtil;
using System.Linq;

namespace Trex.SmartClient.Test.Forecast.ForecastOverview
{
    [TestFixture]
    public class ForecastOverviewForecastMonthsTest : AutoFixtureTestBase
    {
        [Test]
        public void UpdateForecastTypeForceHide_AllAreOfSameForecastType_AllForceHideIsTrue()
        {
            // Arrange
            var forecastType = Fixture.Create<ForecastType>();
            forecastType.Name = "Sometype";
            Fixture.Inject(forecastType);

            var forecast1 = CreateMock<ForecastOverviewForecast>();
            var forecast2 = CreateMock<ForecastOverviewForecast>();
            var forecast3 = CreateMock<ForecastOverviewForecast>();

            var sut = new ForecastOverviewForecastMonths
                {
                    new ForecastOverviewForecastMonth { Forecasts = new List<ForecastOverviewForecast> { forecast1.Object }},
                    new ForecastOverviewForecastMonth { Forecasts = new List<ForecastOverviewForecast> { forecast2.Object }},
                    new ForecastOverviewForecastMonth { Forecasts = new List<ForecastOverviewForecast> { forecast3.Object }},
                };

            var option = new OverviewForecastTypeOption(forecastType)
                {
                    IsSelected = false
                };

            // Act
            sut.UpdateForecastTypeForceHide(option);

            // Assert
            forecast1.Verify(x => x.UpdateVisuals(option));
            forecast2.Verify(x => x.UpdateVisuals(option));
            forecast3.Verify(x => x.UpdateVisuals(option));
        }
    }
}