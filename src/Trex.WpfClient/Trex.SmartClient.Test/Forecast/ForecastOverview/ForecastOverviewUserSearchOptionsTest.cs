using NUnit.Framework;
using Trex.Common.DataTransferObjects;
using Trex.SmartClient.Forecast.ForecastOverview;
using Trex.SmartClient.Test.TestUtil;
using Ploeh.AutoFixture;
using System.Linq;

namespace Trex.SmartClient.Test.Forecast.ForecastOverview
{
    [TestFixture]
    public class ForecastOverviewUserSearchOptionsTest : AutoFixtureTestBase
    {
        public class MyClassInitializeUsers : ForecastOverviewSearchOptionsTest
        {
            [Test]
            public void AllUsersMarkerIsAdded()
            {
                // Arrange
                var users  = new ForecastUserDto[]
                    {
                        new ForecastUserDto { UserId = 1, UserName = "abc"}, 
                        new ForecastUserDto { UserId = 2, UserName = "def"}
                    };

                var sut = Fixture.Create<ForecastOverviewUserSearchOptions>();

                // Act
                sut.InitializeUsers(users);

                // Assert
                Assert.That(sut.Users.SingleOrDefault(x => x.IsAllUsers), Is.Not.Null);
                Assert.That(sut.Users.SingleOrDefault(x => x.UserId.Equals(1)), Is.Not.Null);
                Assert.That(sut.Users.SingleOrDefault(x => x.UserId.Equals(2)), Is.Not.Null);
            }
        }
    }
}