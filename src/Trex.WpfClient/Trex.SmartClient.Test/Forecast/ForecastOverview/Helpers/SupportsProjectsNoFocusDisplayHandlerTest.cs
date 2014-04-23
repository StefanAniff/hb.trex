using NUnit.Framework;
using Trex.SmartClient.Forecast.ForecastOverview;
using Trex.SmartClient.Forecast.ForecastOverview.Helpers;
using Trex.SmartClient.Test.Forecast.ForecastRegistration;
using Trex.SmartClient.Test.TestUtil;

namespace Trex.SmartClient.Test.Forecast.ForecastOverview.Helpers
{
    [TestFixture]
    public class SupportsProjectsNoFocusDisplayHandlerTest : AutoFixtureTestBase
    {
        [Test]
        public void DoUpdateVisuals_ForceHideIsTrueWhenBothOwnAndProjectTypeAreDisabled()
        {
            // Arrange
            var forecastMock = CreateMock<ForecastOverviewForecast>();
            forecastMock.SetupGet(x => x.HasNoProjects).Returns(false);
            forecastMock.SetupGet(x => x.ForecastType).Returns(ForecastTestData.DedicatedAndProjectHoursForcastType);

            var sut = new SupportsProjectsNoFocusDisplayHandler(forecastMock.Object, ForecastTestData.ProjectHoursOnlyForecastType.Id);
            
            // Act
            Assert.That(sut.ForceHide, Is.False);
            sut.UpdateVisuals(new OverviewForecastTypeOption(ForecastTestData.DedicatedAndProjectHoursForcastType) {IsSelected = false});
            Assert.That(sut.ForceHide, Is.False);
            sut.UpdateVisuals(new OverviewForecastTypeOption(ForecastTestData.ProjectHoursOnlyForecastType) { IsSelected = false });

            // Assert
            Assert.That(sut.ForceHide, Is.True);
            forecastMock.VerifyAll();
        }

        [Test]
        public void DoUpdateVisuals_ForceHideIsTrueWhenBothOwnTypeIsDisabledAndForecastHasNoProjects()
        {
            // Arrange
            var forecastMock = CreateMock<ForecastOverviewForecast>();
            forecastMock.SetupGet(x => x.HasNoProjects).Returns(true);
            forecastMock.SetupGet(x => x.ForecastType).Returns(ForecastTestData.DedicatedAndProjectHoursForcastType);

            var sut = new SupportsProjectsNoFocusDisplayHandler(forecastMock.Object, ForecastTestData.ProjectHoursOnlyForecastType.Id);

            // Act
            Assert.That(sut.ForceHide, Is.False);
            sut.UpdateVisuals(new OverviewForecastTypeOption(ForecastTestData.DedicatedAndProjectHoursForcastType) { IsSelected = false });

            // Assert
            Assert.That(sut.ForceHide, Is.True);
            forecastMock.VerifyAll();
        }
    }
}