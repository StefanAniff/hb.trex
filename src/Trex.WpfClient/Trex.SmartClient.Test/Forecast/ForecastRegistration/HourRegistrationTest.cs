using System;
using Moq;
using NUnit.Framework;
using Trex.SmartClient.Forecast.ForecastRegistration;
using Trex.SmartClient.Test.TestUtil;
using Ploeh.AutoFixture;

namespace Trex.SmartClient.Test.Forecast.ForecastRegistration
{
    [TestFixture]
    public class HourRegistrationTest : AutoFixtureTestBase
    {
        [TestCase(true, true, true)]
        [TestCase(true, false, false)]
        [TestCase(false, true, false)]
        [TestCase(false, false, false)]
        public void IsEditableWorkDay_ReturnsAccordingToIsEditEnabledAndIsWorkDay(bool isEditEnabled, bool isWorkDay, bool expectedResult)
        {
            // Arrange
            var fixture = InitializeFixture();
            fixture.Register(() => new ForecastRegistrationDateColumn(fixture.Create<DateTime>()));
            var headerMock = fixture.Create<Mock<ForecastRegistrationDateColumn>>();
            headerMock.SetupGet(x => x.IsWorkDay).Returns(isWorkDay);

            var sut = fixture
                .Build<HourRegistration>()
                .With(x => x.IsEditEnabled, isEditEnabled)
                .With(x => x.DateColumn, headerMock.Object)
                .Create();

            // Act
            var result = sut.IsEditableWorkDay;

            // Assert
            Assert.That(result, Is.EqualTo(expectedResult));
        }
    }
}