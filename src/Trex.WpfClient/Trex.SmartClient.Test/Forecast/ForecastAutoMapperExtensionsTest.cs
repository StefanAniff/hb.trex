using NUnit.Framework;
using Ploeh.AutoFixture;
using Trex.Common.DataTransferObjects;
using Trex.SmartClient.Test.TestUtil;
using Trex.SmartClient.Forecast;

namespace Trex.SmartClient.Test.Forecast
{
    [TestFixture]
    public class ForecastAutoMapperExtensionsTest : AutoFixtureTestBase
    {
        [Test]
        public void ForecastProjectHoursDtoMapping()
        {
            // Arrange
            var source = Fixture.Create<ForecastProjectHoursDto>();

            // Act
            var result = source.ToClient();

            // Assert
        }
    }
}