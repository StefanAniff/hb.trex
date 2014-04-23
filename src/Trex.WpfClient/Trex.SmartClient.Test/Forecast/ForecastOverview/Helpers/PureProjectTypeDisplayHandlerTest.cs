using System.Collections.Generic;
using NUnit.Framework;
using Trex.SmartClient.Forecast.ForecastOverview;
using Trex.SmartClient.Forecast.ForecastOverview.Helpers;
using Trex.SmartClient.Test.TestUtil;

namespace Trex.SmartClient.Test.Forecast.ForecastOverview.Helpers
{
    [TestFixture]
    public class PureProjectTypeDisplayHandlerTest : AutoFixtureTestBase
    {
        [Test]
        public void ForceHide_ProjectFocusIdHasValueAndForecastHasNoRelatingProjects_ReturnsTrue()
        {
            // Arrange
            var forecast = new ForecastOverviewForecast { Projects = new List<ForecastOverviewProjectHours>() };
            var sut = new PureProjectTypeDisplayHandler(forecast, 10, null);

            // Act
            var result = sut.ForceHide;

            // Assert
            Assert.That(result, Is.True);
            Assert.That(sut.DisplayValue, Is.EqualTo(string.Empty));
        }

        [Test]
        public void ForceHide_CompanyFocusIdHasValueAndForecastHasNoRelatingProjects_ReturnsTrue()
        {
            // Arrange
            var forecast = new ForecastOverviewForecast { Projects = new List<ForecastOverviewProjectHours>() };
            var sut = new PureProjectTypeDisplayHandler(forecast, null, 10);

            // Act
            var result = sut.ForceHide;

            // Assert
            Assert.That(result, Is.True);
            Assert.That(sut.DisplayValue, Is.EqualTo(string.Empty));
        }

        [Test]
        public void ForceHide_ProjectFocusIdHasValueAndForecastHasRelatingProjects_ReturnsFalse()
        {
            // Arrange
            var forecast = new ForecastOverviewForecast
                {
                    Projects = new List<ForecastOverviewProjectHours>
                        {
                            new ForecastOverviewProjectHours { ProjectId = 10, Hours = 2},
                            new ForecastOverviewProjectHours { ProjectId = 10, Hours = 4}
                        }
                };

            var sut = new PureProjectTypeDisplayHandler(forecast, 10, null);

            // Act
            var result = sut.ForceHide;

            // Assert
            Assert.That(result, Is.False);
            Assert.That(sut.DisplayValue, Is.EqualTo("6"));
        }

        [Test]
        public void ForceHide_CompanyFocusIdHasValueAndForecastHasRelatingProjects_ReturnsFalse()
        {
            // Arrange
            var forecast = new ForecastOverviewForecast
            {
                Projects = new List<ForecastOverviewProjectHours>
                        {
                            new ForecastOverviewProjectHours { CompanyId = 30, Hours = 1 },
                            new ForecastOverviewProjectHours { CompanyId = 30, Hours = 3}
                        }
            };

            var sut = new PureProjectTypeDisplayHandler(forecast, null, 30);

            // Act
            var result = sut.ForceHide;

            // Assert
            Assert.That(result, Is.False);
            Assert.That(sut.DisplayValue, Is.EqualTo("4"));
        }        
    }
}