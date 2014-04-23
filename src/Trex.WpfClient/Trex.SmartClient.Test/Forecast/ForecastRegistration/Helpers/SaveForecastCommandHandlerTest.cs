using System;
using System.Collections.Generic;
using System.Windows.Threading;
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
    public class SaveForecastCommandHandlerTest : AutoFixtureTestBase
    {
        [Test]
        public void Execute_OneOfEachPresenceTypeToSave_CallSaveForecastsWithMappedDtos()
        {
            // Arrange
            var frame = new DispatcherFrame();
            var fixture = InitializeFixture();
            fixture.Behaviors.Add(new OmitOnRecursionBehavior());
            fixture.Inject(new DateTime(2013,1,1));
            fixture.Inject(ForecastTestData.ProjectHoursOnlyForecastType);
            fixture.Inject(ForecastTestData.ForecastTypesList);

            fixture.Register(() => User.Create(string.Empty, string.Empty, 10, null, null));
            var user = fixture.Create<User>();
            var userSessionMock = fixture.Freeze<Mock<IUserSession>>();
            userSessionMock.SetupGet(x => x.CurrentUser).Returns(user);

            var selectedUserHandlerMock = FreezeMock<ForecastRegistrationSelectedUserHandler>();
            selectedUserHandlerMock.SetupGet(x => x.UserId).Returns(10);


            ForecastMonthDto passedForecastMonth = null;

            // Hookup callback for value assertion
            var forecastServiceMock = fixture.Freeze<Mock<IForecastService>>();
            forecastServiceMock
                .Setup(x => x.SaveForecasts(It.IsAny<ForecastMonthDto>()))
                .ReturnsAsync(new SaveForecastsResponse { ForecastMonthId = 123 })
                .Callback<ForecastMonthDto>((fmonth) =>
                    {
                        passedForecastMonth = fmonth;                        
                    });

            fixture.Register(() => new ForecastRegistrationDateColumn(new DateTime(2013,1,1)));
            fixture.Register(() => new ProjectHourRegistration(fixture.Create<ProjectRegistration>()));
            var hourReg = fixture.Create<ProjectHourRegistration>();
            hourReg.Hours = 7.5m;

            fixture.Register(() => new ForecastTypeRegistration(ForecastTestData.ProjectHoursOnlyForecastType, ForecastTestData.ForecastTypesList));
            var dateColumn = fixture.Create<ForecastRegistrationDateColumn>();
            dateColumn.AddProjectHours(hourReg);

            var dateColumns = fixture.Create<ForecastDateColumns>();
            dateColumns.Add(dateColumn);

            var vm = fixture.Create<Mock<IForecastRegistrationViewModel>>();
            vm.SetupGet(x => x.ForecastMonthId).Returns(1);
            vm.Setup(x => x.IsValid()).Returns(true);
            vm.SetupGet(x => x.SelectedDate).Returns(new DateTime(2013, 1, 1));
            vm.SetupGet(x => x.DateColumns).Returns(dateColumns);

            var sut = fixture.Create<SaveForecastCommandHandler>();

            // Act
            sut.Execute(vm.Object);

            // Assert
            vm.VerifySet(x => x.ForecastMonthId = 123);
            vm.VerifyAll();
            //userSessionMock.VerifyAll();
            selectedUserHandlerMock.VerifyAll();
            forecastServiceMock.VerifyAll();                       
            Assert.That(passedForecastMonth.Id, Is.EqualTo(1));
            Assert.That(passedForecastMonth.UserId, Is.EqualTo(10));
            Assert.That(passedForecastMonth.Month, Is.EqualTo(1));
            Assert.That(passedForecastMonth.Year, Is.EqualTo(2013));
        }

        [Test]
        public void CanExecute_ForecstMonthIsLocked_ReturnsFalse()
        {
            // Arrange
            var selectedUserHandler = FreezeMock<ForecastRegistrationSelectedUserHandler>();
            selectedUserHandler.SetupGet(x => x.SelectedUser).Returns(Fixture.Create<ForecastUserDto>());

            var vmMock = CreateMock<IForecastRegistrationViewModel>();
            vmMock.SetupGet(x => x.ForecastMonthIsLocked).Returns(true);
            vmMock.SetupGet(x => x.SelectedUserHandler).Returns(selectedUserHandler.Object);

            var sut = Fixture.Create<SaveForecastCommandHandler>();

            // Act
            var result = sut.CanExecute(vmMock.Object);

            // Assert
            vmMock.VerifyAll();
            Assert.That(result, Is.False);
        }

    }
}