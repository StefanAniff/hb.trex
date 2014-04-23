using System;
using Moq;
using NUnit.Framework;
using Ploeh.AutoFixture;
using TRex.Services.Test.Util;
using Trex.Server.Core.Model;
using Trex.Server.Core.Model.Forecast;
using System.Linq;

namespace TRex.Services.Test.Trex.Server.Core.Model
{
    [TestFixture]
    public class ForecastMonthTest : AutoFixtureTestBase
    {
        [TestCase(0, 2103, "Month value 0 is invalid")]
        [TestCase(-1, 2103, "Month value -1 is invalid")]
        [TestCase(13, 2103, "Month value 13 is invalid")]
        [TestCase(1, 0, "Year value 0 is invalid")]
        [TestCase(12, -1, "Year value -1 is invalid")]
        public void Constructor_MonthYearIntIsOutOfScope_ThrowsException(int month, int year, string expectedError)
        {
            // Arrange
            Fixture.Behaviors.Add(new OmitOnRecursionBehavior());
            var usr = Fixture
                        .Build<User>()
                        .Without(x => x.ForecastMonths)
                        .Create();

            // Act
            var exp = Assert.Throws<Exception>(() => new ForecastMonth(month, year, 3, usr, usr));

            // Assert
            Assert.That(exp.Message, Is.EqualTo(expectedError));
        }

        [Test]
        public void AddForecast_AddsNewForecastToCollection()
        {
            // Arrange
            Fixture.Behaviors.Add(new OmitOnRecursionBehavior());
            var usr = Fixture
                        .Build<User>()
                        .Without(x => x.ForecastMonths)
                        .Create();
            var forecastType = Fixture.Create<ForecastType>();

            var month = new ForecastMonth(1, 2013, 3, usr, usr);

            // Act
            month.AddForecast(new DateTime(2013, 1, 1), forecastType, null);

            // Assert
            var forecast = month.Forecasts.Single();
            Assert.That(forecast.Date, Is.EqualTo(new DateTime(2013, 1, 1)));
            Assert.That(forecast.ForecastType, Is.EqualTo(forecastType));
            Assert.That(forecast.DedicatedForecastTypeHours, Is.EqualTo(null));
        }

        [TestCase("01-02-2013", "Date 01-02-2013 is not a part of ForecastMonth month: 1 year: 2013")]
        [TestCase("31-12-2012", "Date 31-12-2012 is not a part of ForecastMonth month: 1 year: 2013")]
        public void AddForecast_ForecastDateIsNotPartOfForecastMonth_ThrowsException(string dateString, string message)
        {
            // Arrange
            Fixture.Behaviors.Add(new OmitOnRecursionBehavior());

            var usr = Fixture
                        .Build<User>()
                        .Without(x => x.ForecastMonths)
                        .Create();
            var forecastType = Fixture.Create<ForecastType>();

            var month = new ForecastMonth(1, 2013, 3, usr, usr);

            // Act
            var exp = Assert.Throws<Exception>(() => month.AddForecast(ParseDkDateString(dateString), forecastType, null));

            // Assert
            Assert.That(exp.Message, Is.EqualTo(message));
        }

        [TestCase(1, 2013, "3-1-2013", 3, false)]
        [TestCase(2, 2013, "1-2-2013", 3, false)]
        [TestCase(5, 2011, "3-1-2013", 3, true)]
        [TestCase(12, 2012, "3-1-2013", 4, false)]
        [TestCase(12, 2012, "3-1-2013", 3, false)]
        [TestCase(12, 2012, "3-1-2013", 2, true)]
        public void IsLocked_TestCase(int month, int year, string nowDateString, int lockDay, bool expectedResult)
        {
            // Arrange
            var user = CreateMock<User>();

            var sut = new ForecastMonth(month, year, lockDay, user.Object, user.Object)
                {
                    Now = ParseDkDateString(nowDateString)
                };

            // Act
            var result = sut.IsLocked;

            // Assert
            Assert.That(result, Is.EqualTo(expectedResult));
        }
    }
}