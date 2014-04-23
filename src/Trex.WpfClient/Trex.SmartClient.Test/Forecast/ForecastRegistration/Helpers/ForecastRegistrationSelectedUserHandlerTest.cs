using System.Collections.Generic;
using Moq;
using NUnit.Framework;
using Ploeh.AutoFixture;
using Trex.Common.DataTransferObjects;
using Trex.Common.ServiceStack;
using Trex.SmartClient.Core.Model;
using Trex.SmartClient.Core.Services;
using Trex.SmartClient.Forecast.ForecastRegistration;
using Trex.SmartClient.Forecast.ForecastRegistration.Helpers;
using Trex.SmartClient.Test.TestUtil;

namespace Trex.SmartClient.Test.Forecast.ForecastRegistration.Helpers
{
    [TestFixture]
    public class ForecastRegistrationSelectedUserHandlerTest : AutoFixtureTestBase
    {
        [TestCase(true, true)]
        [TestCase(false, false)]
        public void MayEditOthersWorkplan_ReturnsDependingOnUserSession(bool mayEditWokrplan, bool expectedResult)
        {
            // Arrange
            var userSessionMock = FreezeMock<IUserSession>();
            userSessionMock.SetupGet(x => x.MayEditOthersWorksplan).Returns(mayEditWokrplan);

            FreezeMock<ForecastRegistrationSelectedUserHandler>();
            var sut = Fixture.Create<ForecastRegistrationSelectedUserHandler>();

            // Act
            var result = sut.MayEditOthersWorkplan;


            // Assert
            userSessionMock.VerifyAll();
            Assert.That(result, Is.EqualTo(expectedResult));
        }

        [TestCase(1,1,false)]
        [TestCase(1,2,true)]
        [TestCase(2,1,true)]
        public void IsEditingOthersWorkplan_TestCase(int userSessionId, int selectedUserId, bool expectedResult)
        {
            // Arrange
            var userSessionMock = FreezeMock<IUserSession>();
            userSessionMock.SetupGet(x => x.CurrentUser).Returns(User.Create("", "", userSessionId, null, null));

            FreezeMock<ForecastRegistrationSelectedUserHandler>();
            var sut = Fixture.Create<ForecastRegistrationSelectedUserHandler>();
            sut.SelectedUser.UserId = selectedUserId;

            // Act
            var result = sut.IsEditingOthersWorkplan;


            // Assert
            userSessionMock.VerifyAll();
            Assert.That(result, Is.EqualTo(expectedResult));
        }

        [Test]
        public void Initialize_MayNotEditOthersWorksplan_ForecastServiceIsNotUnitilizedSelectedUserIsSetToUserSession()
        {
            // Arrange
            var forecastServiceMock = FreezeMock<IForecastService>();

            var userSessionMock = FreezeMock<IUserSession>();
            userSessionMock.SetupGet(x => x.MayEditOthersWorksplan).Returns(false);
            userSessionMock.SetupGet(x => x.CurrentUser).Returns(User.Create("abc", "abc cba", 10, null, null));

            var sut = Fixture.Create<ForecastRegistrationSelectedUserHandler>();

            // Act
            sut.Initialize(null);

            // Assert
            forecastServiceMock.Verify(x => x.GetOverivewSearchOptions(), Times.Never());
            userSessionMock.VerifyAll();
            Assert.That(sut.SelectedUser.UserId, Is.EqualTo(10));
            Assert.That(sut.SelectedUser.UserName, Is.EqualTo("abc"));
            Assert.That(sut.SelectedUser.Name, Is.EqualTo("abc cba"));
        }

        [Test]
        public void Initialize_MayEditOthersWorkplan_ForecastServiceUtilized()
        {
            // Arrange
            var forecastServiceMock = FreezeMock<IForecastService>();
            forecastServiceMock.Setup(x => x.GetOverivewSearchOptions()).ReturnsAsync(new ForecastSearchOptionsResponse
                {
                    Users = new List<ForecastUserDto> { new ForecastUserDto { UserId = 10} }
                });

            var userSessionMock = FreezeMock<IUserSession>();
            userSessionMock.SetupGet(x => x.MayEditOthersWorksplan).Returns(true);
            userSessionMock.SetupGet(x => x.CurrentUser).Returns(User.Create("abc", "abc cba", 10, null, null));

            var sut = Fixture.Create<ForecastRegistrationSelectedUserHandler>();

            // Act
            sut.Initialize(null);

            // Assert
            forecastServiceMock.Verify(x => x.GetOverivewSearchOptions());
            userSessionMock.VerifyAll();
            Assert.That(sut.SelectedUser.UserId, Is.EqualTo(10));
        }


    }
}