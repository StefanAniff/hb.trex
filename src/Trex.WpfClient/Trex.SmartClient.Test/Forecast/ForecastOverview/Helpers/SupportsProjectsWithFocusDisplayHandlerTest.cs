using System.Collections.Generic;
using Moq;
using NUnit.Framework;
using Ploeh.AutoFixture;
using Trex.SmartClient.Forecast.ForecastOverview;
using Trex.SmartClient.Forecast.ForecastOverview.Helpers;
using Trex.SmartClient.Test.Forecast.ForecastRegistration;
using Trex.SmartClient.Test.TestUtil;

namespace Trex.SmartClient.Test.Forecast.ForecastOverview.Helpers
{    
    [TestFixture]
    public class SupportsProjectsWithFocusDisplayHandlerTest : AutoFixtureTestBase
    {
        [Test]
        public void IsRelatedToCurrentProjectOrCompany_HasProjectsReleatedToCurrentProjectId_ReturnsTrue()
        {
            // Arrange
            var forecast = new ForecastOverviewForecast
                {
                    Projects = new List<ForecastOverviewProjectHours>
                        {
                            new ForecastOverviewProjectHours { ProjectId = 13 }
                        }
                };

            // Act
            var sut = new SupportsProjectsWithFocusDisplayHandler(forecast, 13, null, 1);

            // Assert
            Assert.That(sut.IsRelatedToCurrentProjectOrCompany, Is.True);
        }

        [Test]
        public void IsRelatedToCurrentProjectOrCompany_HasProjectsReleatedToCurrentCompanyId_ReturnsTrue()
        {
            // Arrange
            var forecast = new ForecastOverviewForecast
            {
                Projects = new List<ForecastOverviewProjectHours>
                        {
                            new ForecastOverviewProjectHours { CompanyId = 22 }
                        }
            };

            // Act
            var sut = new SupportsProjectsWithFocusDisplayHandler(forecast, null, 22, 1);

            // Assert
            Assert.That(sut.IsRelatedToCurrentProjectOrCompany, Is.True);
        }

        [Test]
        public void DoUpdateVisuals_ForceHideIsTrueWhenBothOwnAndProjectTypeAreDisabled()
        {
            // Arrange
            var forecast = new ForecastOverviewForecast
                {
                    ForecastType = ForecastTestData.DedicatedAndProjectHoursForcastType,
                    Projects = new List<ForecastOverviewProjectHours>
                        {
                            new ForecastOverviewProjectHours { ProjectId = 22 }
                        }
                };

            var sut = new SupportsProjectsWithFocusDisplayHandler(forecast, 22, 0, ForecastTestData.ProjectHoursOnlyForecastType.Id);

            // Act
            Assert.That(sut.ForceHide, Is.False);
            sut.UpdateVisuals(new OverviewForecastTypeOption(ForecastTestData.ProjectHoursOnlyForecastType) {IsSelected = false});
            Assert.That(sut.ForceHide, Is.False);
            sut.UpdateVisuals(new OverviewForecastTypeOption(ForecastTestData.DedicatedAndProjectHoursForcastType) { IsSelected = false });

            // Assert
            Assert.That(sut.ForceHide, Is.True);
        }
    }
}