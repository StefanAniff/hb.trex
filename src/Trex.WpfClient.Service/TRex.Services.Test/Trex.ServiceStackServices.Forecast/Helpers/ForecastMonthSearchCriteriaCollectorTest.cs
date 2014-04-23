using System;
using NUnit.Framework;
using TRex.Services.Test.Util;
using Trex.Common.ServiceStack;
using TrexSL.Web.ServiceStackServices.Forecast.ForecastMonthCriterias;
using TrexSL.Web.ServiceStackServices.Forecast.Helpers;
using System.Linq;

namespace TRex.Services.Test.Trex.ServiceStackServices.Forecast.Helpers
{
    [TestFixture]
    public class ForecastMonthSearchCriteriaCollectorTest : AutoFixtureTestBase
    {
        [Test]
        public void Collect_BothProjectIdAndCompanyIdHasValue_ThrowsException()
        {
            // Arrange
            var sut = new ForecastMonthSearchCriteriaCollector();

            // Act
            var exception = Assert.Throws<Exception>(() => sut.Collect(new ForecastSearchByRegistrationRequest {CompanyId = 1, ProjectId = 2}));

            // Assert
            Assert.That(exception.Message, Is.EqualTo("CompanyId and ProjectId search combination does not make sence"));
        }

        [Test]
        public void Collect_ProjectIdHasValue_ResultContainsForecastProjectQueryCriteria()
        {
            // Arrange
            var sut = new ForecastMonthSearchCriteriaCollector();

            // Act
            var result = sut.Collect(new ForecastSearchByRegistrationRequest {ProjectId = 1});

            // Assert
            Assert.That(result.Single(), Is.InstanceOf<ForecastProjectQueryCriteria>());
        }

        [Test]
        public void Collect_CompanyIdHasValue_ResultContainsForecastCompanyQueryCriteria()
        {
            // Arrange
            var sut = new ForecastMonthSearchCriteriaCollector();

            // Act
            var result = sut.Collect(new ForecastSearchByRegistrationRequest { CompanyId = 1 });

            // Assert
            Assert.That(result.Single(), Is.InstanceOf<ForecastCompanyQueryCriteria>());
        }

        [Test]
        public void Collect_ForecastIdHasValue_ResultContainsForcastTypeQueryCriteria()
        {
            // Arrange
            var sut = new ForecastMonthSearchCriteriaCollector();

            // Act
            var result = sut.Collect(new ForecastSearchByRegistrationRequest { ForecastTypeId = 1 });

            // Assert
            Assert.That(result.Single(), Is.InstanceOf<ForcastTypeQueryCriteria>());
        }
    }
}