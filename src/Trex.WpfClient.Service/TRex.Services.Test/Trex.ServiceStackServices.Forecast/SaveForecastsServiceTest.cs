using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace TRex.Services.Test.Trex.ServiceStackServices.Forecast
{
    [TestFixture]
    public class SaveForecastsServiceTest : AutoFixtureTestBase
    {
        [Test]
        public void PostValue_SavesAsExpected()
        {
            // Arrange         
            var fixture = InitializeFixture();            

            var forecastType = fixture.Create<Mock<ForecastType>>();
            forecastType.SetupGet(x => x.Id).Returns(22);

            var forecastTypeRepoMock = FreezeMock<IForecastTypeRepository>();
            forecastTypeRepoMock.Setup(x => x.GetAll()).Returns(new List<ForecastType> {forecastType.Object});

            var userMock = fixture.Create<Mock<User>>();
            userMock.SetupGet(x => x.EntityId).Returns(10);
            var userRepoMock = FreezeMock<IUserRepository>();
            userRepoMock.Setup(x => x.GetByUserID(10)).Returns(userMock.Object);

            var forecastMonthMock = fixture.Create<Mock<ForecastMonth>>();
            forecastMonthMock.CallBase = false;
            forecastMonthMock.SetupGet(x => x.Id).Returns(66);

            ForecastMonth buildtMonth = null;
            var monthRepoMock = FreezeMock<IForecastMonthRepository>();
            monthRepoMock.Setup(x => x.GetById(0)).Returns((ForecastMonth) null);
            monthRepoMock.Setup(x => x.SaveOrUpdate(It.IsAny<ForecastMonth>())).Callback<ForecastMonth>(x => buildtMonth = x);

            var monthFactoryMock = FreezeMock<IForecastMonthFactory>();
            monthFactoryMock.Setup(x => x.CreateForecastMonth(1, 2013, It.IsAny<User>(), It.IsAny<User>())).Returns(forecastMonthMock.Object);

            var request = new SaveForecastsRequest
                {
                    ForecastMonthDto = new ForecastMonthDto
                        {
                                Id = 0,
                                UserId = 10,
                                Month = 1,
                                Year = 2013,
                                CreatedById = 10,
                                ForecastDtos = new List<ForecastDto>
                                    {
                                        new ForecastDto
                                            {
                                                Date = new DateTime(2013, 1, 1),
                                                ForecastType = new ForecastTypeDto {Id = 22},
                                                ForecastProjectHoursDtos = new Collection<ForecastProjectHoursDto>()
                                            }
                                    }
                   
                        }
                };

            var sut = fixture.Create<SaveForecastsService>();

            // Act
            var result = sut.Post(request);

            // Assert
            monthRepoMock.VerifyAll();
            monthFactoryMock.VerifyAll();
            forecastType.VerifyAll();
            userRepoMock.VerifyAll();
            forecastMonthMock.Verify(x => x.AddForecast(new DateTime(2013, 1, 1), forecastType.Object, null));

            Assert.That(result, Is.TypeOf<SaveForecastsResponse>());
            Assert.That(((SaveForecastsResponse)result).ForecastMonthId, Is.EqualTo(buildtMonth.Id));
            Assert.That(buildtMonth.Id, Is.EqualTo(66));
        }

        [Test]
        public void PostValue_ForecastIsLocked_ThrowsException()
        {
            // Arrange
            var dto = Fixture.Create<ForecastMonthDto>();
            dto.Id = 123;

            var forecastMonthMock = CreateMock<ForecastMonth>();
            forecastMonthMock.SetupGet(x => x.IsLocked).Returns(true);
            forecastMonthMock.SetupGet(x => x.Month).Returns(6);
            forecastMonthMock.SetupGet(x => x.Year).Returns(2012);
            forecastMonthMock.SetupGet(x => x.IsLocked).Returns(true);

            var monthRepoMock = FreezeMock<IForecastMonthRepository>();
            monthRepoMock.Setup(x => x.GetById(123)).Returns(forecastMonthMock.Object);

            FreezeMock<IUserRepository>();

            var sut = Fixture.Create<SaveForecastsService>();

            // Act
            var exp = Assert.Throws<Exception>(() => sut.Post(new SaveForecastsRequest { ForecastMonthDto = new ForecastMonthDto {Id = 123}}));

            // Assert
            forecastMonthMock.VerifyAll();
            monthRepoMock.VerifyAll();
            Assert.That(exp.InnerException.Message, Is.StringContaining("Cannot update/create workplan for month: 6 year: 2012, since it is locked"));
        }
    }
}