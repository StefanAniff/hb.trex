using Moq;
using NUnit.Framework;
using Ploeh.AutoFixture;
using Trex.Common.DataTransferObjects;
using Trex.Common.ServiceStack;
using Trex.SmartClient.Core.Services;
using Trex.SmartClient.Service;
using System.Linq;
using Trex.SmartClient.Test.TestUtil;

namespace Trex.SmartClient.Test.Service
{    
    [TestFixture]
    public class ForecastServiceTest : AutoFixtureTestBase
    {
        [Test]
        public void SaveForecasts_OneOfEachTypeOfForecastToSave_MapsAndCallsSaveOnClient()
        {
            // Arrange
            var fixture = InitializeFixture();

            var client = fixture.Create<Mock<IServiceStackClient>>();
            SaveForecastsRequest request = null;
            client
                .Setup(x => x.PostAsync(It.IsAny<SaveForecastsRequest>()))
                .ReturnsAsync(new SaveForecastsResponse())
                .Callback<SaveForecastsRequest>(x => request = x);

            var appSettingsMock = new Mock<IAppSettings>();
            var sut = new ForecastService(appSettingsMock.Object)
                {
                    Client = () => client.Object
                };

            var forecastMonth = fixture.Create<ForecastMonthDto>();
            var forecastDtos = fixture
                .CreateMany<ForecastDto>()
                .ToList();
            forecastMonth.ForecastDtos = forecastDtos;

            // Act
            sut.SaveForecasts(forecastMonth);

            // Assert
            Assert.That(request, Is.Not.Null);
            Assert.That(request.ForecastMonthDto, Is.EqualTo(forecastMonth));
            Assert.That(request.ForecastMonthDto.ForecastDtos, Is.EqualTo(forecastDtos));
            client.VerifyAll();
        }
    }
}