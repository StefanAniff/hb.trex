using System;
using System.Collections.Generic;
using Moq;
using NUnit.Framework;
using Ploeh.AutoFixture;
using TRex.Services.Test.Util;
using Trex.Common.DataTransferObjects;
using Trex.Common.ServiceStack;
using Trex.Server.Core.Model;
using Trex.Server.Core.Model.Forecast;
using Trex.Server.Core.Services;
using TrexSL.Web.ServiceStackServices.Forecast;
using ForecastDomain = Trex.Server.Core.Model.Forecast.Forecast;
using System.Linq;

namespace TRex.Services.Test.Trex.ServiceStackServices.Forecast
{
    [TestFixture]
    public class ForecastsByUserAndMonthServiceTest : AutoFixtureTestBase
    {
        [Test]
        public void Get_ExtractsFromForecastAndHolidayRepository()
        {
            // Arrange
            var fixture = InitializeFixture();
            fixture.Behaviors.Add(new OmitOnRecursionBehavior());

            var forecastMonthEntity = fixture.Create<Mock<ForecastMonth>>();
            forecastMonthEntity.SetupGet(x => x.Id).Returns(1234);
            forecastMonthEntity.CallBase = false;
            var forecastMonthRepoMock = FreezeMock<IForecastMonthRepository>();
            forecastMonthRepoMock.Setup(x => x.GetByUserAndMonth(10, 12, 2012)).Returns(forecastMonthEntity.Object);

            var holidayEntity = fixture.Create<Holiday>();
            var holidayRepoMock = FreezeMock<IHolidayRepository>();
            holidayRepoMock.Setup(x => x.GetByMonth(1, 2013)).Returns(new List<Holiday> { holidayEntity });

            var sut = fixture.Create<ForecastsByUserAndMonthService>();

            // Act
            var request = new ForecastsByUserAndMonthRequest
                {
                    UserId = 10,
                    ForecastMonth = 12,
                    ForecastYear = 2012,
                    HolidayMonth = 1,
                    HolidayYear = 2013
                };

            var result = (ForecastsByUserAndMonthResponse)sut.Post(request);

            // Assert
            forecastMonthRepoMock.VerifyAll();
            holidayRepoMock.VerifyAll();
            Assert.That(result.ForecastMonth.Id, Is.EqualTo(1234));
            Assert.That(result.Holidays.Single().Date, Is.EqualTo(holidayEntity.Date));
            Assert.That(result.Holidays.Single().Description, Is.EqualTo(holidayEntity.Description));
        }

        [Test]
        public void SetIsLocked_IsPastAllowUpdateIsTrue_IsLockedIsFalse()
        {
            // Arrange
            var monthDomMock = Fixture.Create<Mock<ForecastMonth>>();
            monthDomMock.SetupGet(x => x.UnLocked).Returns(true);

            var dto = Fixture.Create<ForecastMonthDto>();
            var sut = Fixture.Create<ForecastsByUserAndMonthService>();

            // Act
            sut.SetDtoIsLocked(dto, monthDomMock.Object);

            // Assert
            monthDomMock.VerifyAll();
            Assert.That(dto.IsLocked, Is.False);
        }

        [TestCase(1, 2013, "3-2-2013", 3, false)]
        [TestCase(2, 2013, "31-1-2013", 1, false)]
        [TestCase(12, 2012, "4-1-2013", 3, true)]
        [TestCase(5, 2011, "4-1-2013", 3, true)]
        public void SetIsLocked_MonthIsNullTestCase(int dtoMonth, int dtoYear, string nowString, int dayLock, bool expectedResult)
        {
            // Arrange
            var settingsMock = FreezeMock<IDomainSettings>();
            settingsMock.SetupGet(x => x.PastMonthsDayLock).Returns(dayLock);

            var dto = Fixture.Create<ForecastMonthDto>();
            dto.IsLocked = false;
            dto.Month = dtoMonth;
            dto.Year = dtoYear;

            var sut = Fixture.Create<ForecastsByUserAndMonthService>();
            sut.Now = ParseDkDateString(nowString);

            // Act
            sut.SetDtoIsLocked(dto, null);

            // Assert
            settingsMock.VerifyAll();
            Assert.That(dto.IsLocked, Is.EqualTo(expectedResult));
        }
    }
}