using System;
using System.Windows.Threading;
using Moq;
using NUnit.Framework;
using Trex.SmartClient.Forecast.ForecastRegistration;
using Trex.SmartClient.Forecast.ForecastRegistration.ValidationRules;
using Trex.SmartClient.Forecast.Shared;
using Trex.SmartClient.Test.TestUtil;
using Ploeh.AutoFixture;

namespace Trex.SmartClient.Test.Forecast.ForecastRegistration.ValidationRules
{
    [TestFixture]
    public class TotalDateHoursValidationRuleTest : AutoFixtureTestBase
    {
        [TestFixtureSetUp]
        public void Fixture()
        {
            new DispatcherFrame();            
        }

        private static HourRegistration CreateHourRegistration(decimal hours, bool hasClientHourRegistrations)
        {
            return CreateHourRegistration(hours, hasClientHourRegistrations, ForecastTestData.ProjectHoursOnlyForecastType);
        }

        private static HourRegistration CreateHourRegistration(decimal hours, bool hasClientHourRegistrations, ForecastType forecastType)
        {
            var dateHeaderMock = new Mock<ForecastRegistrationDateColumn>(new DateTime(2013, 1, 1));
            dateHeaderMock.SetupGet(x => x.ForecastType).Returns(forecastType);
            dateHeaderMock.SetupGet(x => x.IsWorkDay).Returns(true);
            dateHeaderMock.SetupGet(x => x.HasProjectHours).Returns(hasClientHourRegistrations);
            dateHeaderMock.SetupGet(x => x.ForecastTypeRegistration).Returns(new ForecastTypeRegistration(forecastType, ForecastTestData.ForecastTypesList));
            var hourReg = new HourRegistration{ Hours = hours, DateColumn = dateHeaderMock.Object };
            return hourReg;
        }

        [TestCase(23, null)]
        [TestCase(24, null)]
        [TestCase(25, "Max 24 hours in total for a given day is allowed")]
        public void ValidateViewModel_Max24HoursTestCase(decimal hours, string expectedValidationText)
        {
            // Arrange
            var hourReg = CreateHourRegistration(hours, true);
            var sut = new TotalDateHoursValidationRule();

            // Act
            var result = sut.ValidateViewModel(hourReg, string.Empty, null);

            // Assert           
            Assert.That((result == null) == (expectedValidationText == null), Is.True);
            if (expectedValidationText != null)
                Assert.That(result.ErrorMessage, Is.EqualTo(expectedValidationText));
        }

        [TestCase(0, "When \"Client\" is selected, total can't be zero")]
        [TestCase(1, null)]
        public void ValidateViewModel_ClientZeroHoursTestCase(decimal hours, string expectedValidationText)
        {
            // Arrange            
            var hourReg = CreateHourRegistration(hours, true);
            var sut = new TotalDateHoursValidationRule();

            // Act
            var result = sut.ValidateViewModel(hourReg, string.Empty, null);

            // Assert           
            Assert.That((result == null) == (expectedValidationText == null), Is.True);
            if (expectedValidationText != null)
                Assert.That(result.ErrorMessage, Is.EqualTo(expectedValidationText));

        }

        [Test]
        public void ValidateViewModel_NonProjectZeroHoursTestCase()
        {
            // Arrange
            var hourReg = CreateHourRegistration(1m, true, ForecastTestData.SimpleForecastType);
            var sut = new TotalDateHoursValidationRule();

            // Act
            var result = sut.ValidateViewModel(hourReg, string.Empty, null);

            // Assert           
            Assert.That(result, Is.Null);

        }

        [Test]
        public void ValidateViewModel_NoProjectRowsValidatesDedicatedHours()
        {
            // Arrange
            var fix = InitializeFixture();            

            var hourReg = CreateHourRegistration(25m, false, ForecastTestData.DedicatedAndProjectHoursForcastType);
            var sut = fix.Create<TotalDateHoursValidationRule>();

            // Act
            var result = sut.ValidateViewModel(hourReg, string.Empty, null);

            // Assert
            Assert.That(result, Is.Not.Null);
        }
    }
}