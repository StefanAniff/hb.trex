using System;
using System.Windows.Threading;
using NUnit.Framework;
using Trex.SmartClient.Forecast.ForecastRegistration;
using Trex.SmartClient.Test.TestUtil;

namespace Trex.SmartClient.Test.Forecast.ForecastRegistration
{
    [TestFixture]
    public class ForecastRegistrationDateColumnTest : AutoFixtureTestBase
    {
        [Test]
        public void IsEmptyProjectRegistration_IsTypeProjectAndHasNoProjectHours_ReturnsTrue()
        {
            // Arrange

            var frame = new DispatcherFrame();

            var sut = new ForecastRegistrationDateColumn(new DateTime(2013,1,1))
                {
                    ForecastTypeRegistration =
                        new ForecastTypeRegistration(ForecastTestData.ProjectHoursOnlyForecastType,
                                                     ForecastTestData.ForecastTypesList)
                };

            // Act
            var resultIsTrue = sut.IsEmptyProjectRegistration(ForecastTestData.ProjectHoursOnlyForecastType.Id);
            var resultIsFalse = sut.IsEmptyProjectRegistration(666);

            // Assert
            Assert.That(resultIsTrue, Is.True);
            Assert.That(resultIsFalse, Is.False);
        }

        [Test]
        public void IsEmptyProjectRegistration_IsTypeProjectAndHasProjectHours_ReturnsFalse()
        {
            // Arrange
            var frame = new DispatcherFrame();
            var sut = new ForecastRegistrationDateColumn(new DateTime(2013, 1, 1))
            {
                ForecastTypeRegistration =
                    new ForecastTypeRegistration(ForecastTestData.ProjectHoursOnlyForecastType,
                                                 ForecastTestData.ForecastTypesList)
            };
            sut.AddProjectHours(new ProjectHourRegistration(new ProjectRegistration()));

            // Act
            var result = sut.IsEmptyProjectRegistration(ForecastTestData.ProjectHoursOnlyForecastType.Id);

            // Assert
            Assert.That(result, Is.False);
        }
    }
}