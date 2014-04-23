using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Threading;
using Moq;
using NUnit.Framework;
using Trex.Common.DataTransferObjects;
using Trex.Common.ServiceStack;
using Trex.SmartClient.Core.Interfaces;
using Trex.SmartClient.Core.Model;
using Trex.SmartClient.Core.Services;
using Trex.SmartClient.Forecast.ForecastRegistration;
using Trex.SmartClient.Forecast.ForecastRegistration.Helpers;
using Trex.SmartClient.Test.TestUtil;
using Ploeh.AutoFixture;

namespace Trex.SmartClient.Test.Forecast.ForecastRegistration.Helpers
{    
    public class CopyPreviousMonthCommandHandlerTest
    {
        [TestFixture]
        public class ShouldContinueTest : AutoFixtureTestBase
        {
            [Test] 
            public void ClientRegistrationsIsEmptyAndPresenceTypeIsDefault_ReturnsTrue()
            {
                // Arrange
                var fixture = InitializeFixture();
                var presenceTypeProvider = ForecastTestData.MockForecastTypeProvider(fixture);
                fixture.Inject(presenceTypeProvider.Object);

                FreezeMock<ForecastRegistrationSelectedUserHandler>();                

                var vmMock = new Mock<IForecastRegistrationViewModel>();
                vmMock.SetupGet(x => x.ProjectRegistrations).Returns(new ProjectRegistrations());
                vmMock.SetupGet(x => x.PresenceRegistrations).Returns(new ObservableCollection<ForecastTypeRegistration>());

                var sut = fixture.Create<CopyPreviousMonthCommandHandler>();

                // Act
                var result = sut.ShouldContinue(vmMock.Object);

                // Assert
                vmMock.VerifyAll();
                Assert.That(result, Is.True);
            }

            [Test]
            public void ClientRegistrationsExists_ContinueWarningIsCalled()
            {
                // Arrange
                var commonDlgMock = Fixture.Create<Mock<ICommonDialogs>>();
                commonDlgMock
                    .Setup(x => x.ContinueWarning("Data already exists for this month!\n\nContinue with overwrite?", "Overwrite"))
                    .Returns(false);
                Fixture.Inject(commonDlgMock.Object);

                var presenceTypeProvider = ForecastTestData.MockForecastTypeProvider(Fixture);
                Fixture.Inject(presenceTypeProvider);

                FreezeMock<ForecastRegistrationSelectedUserHandler>();                

                var vmMock = Fixture.Create<Mock<IForecastRegistrationViewModel>>();
                vmMock.SetupGet(x => x.ProjectRegistrations).Returns(new ProjectRegistrations { new ProjectRegistration { ProjectId = 1 } });

                var sut = Fixture.Create<CopyPreviousMonthCommandHandler>();

                // Act
                var result = sut.ShouldContinue(vmMock.Object);

                // Assert
                Assert.That(result, Is.False);
                vmMock.VerifyAll();
                commonDlgMock.VerifyAll();
            }

            [Test]
            public void PresenceTypeIsNotDefault_ContinueWarningIsCalled()
            {
                // Arrange
                new DispatcherFrame(); // Dispatcher is needed for gui tests
                var fixture = InitializeFixture();

                var commonDlgMock = fixture.Create<Mock<ICommonDialogs>>();
                commonDlgMock
                    .Setup(x => x.ContinueWarning("Data already exists for this month!\n\nContinue with overwrite?", "Overwrite"))
                    .Returns(false);
                fixture.Inject(commonDlgMock.Object);

                FreezeMock<ForecastRegistrationSelectedUserHandler>();                

                var presenceTypeProviderMock = ForecastTestData.MockForecastTypeProvider(fixture);
                fixture.Inject(presenceTypeProviderMock.Object);

                var vmMock = new Mock<IForecastRegistrationViewModel>();
                vmMock.SetupGet(x => x.ProjectRegistrations).Returns(new ProjectRegistrations());
                vmMock.SetupGet(x => x.PresenceRegistrations).Returns(new ObservableCollection<ForecastTypeRegistration>
                    {
                        new ForecastTypeRegistration(ForecastTestData.SimpleForecastType, ForecastTestData.ForecastTypesList)
                    });

                var sut = fixture.Create<CopyPreviousMonthCommandHandler>();


                // Act
                var result = sut.ShouldContinue(vmMock.Object);

                // Assert
                Assert.That(result, Is.False);
                vmMock.VerifyAll();
                commonDlgMock.VerifyAll();
            }
        }

        [TestFixture]
        public class ExecuteTest : AutoFixtureTestBase
        {
            [Test]
            public void TwoCompaniesInPreviousMonth_ViewModelIsInitializedAndCompaniesAreAdded()
            {
                // Arrange
                var fixture = InitializeFixture();
                var project1 = new ProjectDto
                    {
                        Id = 1, 
                        Name = "1",
                        CompanyDto = new CompanyDto {Name = "d60"}
                    };
                var project2 = new ProjectDto
                    {
                        Id = 2, 
                        Name = "2",
                        CompanyDto = new CompanyDto {Name = "PTS"}
                    };

                var selectedUserHanlderMock = FreezeMock<ForecastRegistrationSelectedUserHandler>();
                selectedUserHanlderMock.SetupGet(x => x.UserId).Returns(10);

                var holidaysDtos = new List<HolidayDto>
                    {
                        new HolidayDto {Date = new DateTime(2013, 1, 1), Description = "Hangover is a scumbag"}
                    };

                var forecastServiceMock = FreezeMock<IForecastService>();
                forecastServiceMock.Setup(x => x.GetByUserIdAndMonth(10, 12, 2012, 1, 2013)).ReturnsAsync(new ForecastsByUserAndMonthResponse
                    {
                        ForecastMonth = new ForecastMonthDto
                            {
                                Id = 11,
                                Month = 12,
                                Year = 2012,
                                IsLocked = false,
                                ForecastDtos =  new List<ForecastDto> 
                                                            {
                                                                new ForecastDto
                                                                    {
                                                                        ForecastType = new ForecastTypeDto { SupportsProjectHours = true },
                                                                        ForecastProjectHoursDtos = new Collection<ForecastProjectHoursDto>
                                                                            {
                                                                                new ForecastProjectHoursDto { Project = project1 },
                                                                                new ForecastProjectHoursDto { Project = project2}
                                                                            }
                                                                    }                            
                                                            }
                            },                        
                        Holidays = holidaysDtos
                    });

                var forecastDataGeneratorMock = FreezeMock<ForecastRegistrationDataGenerator>();
                var dayLayoutSelectorMock = FreezeMock<MostFrequentDayLayoutSelector>();

                var vmMock = FreezeMock<IForecastRegistrationViewModel>();
                vmMock.SetupGet(x => x.SelectedDate).Returns(new DateTime(2013, 1, 1));
                var sut = fixture.Create<CopyPreviousMonthCommandHandler>();

                // Act
                sut.Execute(vmMock.Object);

                // Assert
                forecastDataGeneratorMock.Verify(x => x.GenerateBaseDataByDate(new DateTime(2013, 1, 1), vmMock.Object));
                forecastDataGeneratorMock.Verify(x => x.MergeHolidays(vmMock.Object, holidaysDtos));
                vmMock.Verify(x => x.AddNewProjectRegistration(1, "1", "d60"));
                vmMock.Verify(x => x.AddNewProjectRegistration(2, "2", "PTS"));
                selectedUserHanlderMock.VerifyAll();
                forecastServiceMock.VerifyAll();
                forecastDataGeneratorMock.VerifyAll();
                dayLayoutSelectorMock.Verify(x => x.MostFrequentDayLayout(DayOfWeek.Monday, It.IsAny<IEnumerable<ForecastDto>>()));
                dayLayoutSelectorMock.Verify(x => x.MostFrequentDayLayout(DayOfWeek.Tuesday, It.IsAny<IEnumerable<ForecastDto>>()));
                dayLayoutSelectorMock.Verify(x => x.MostFrequentDayLayout(DayOfWeek.Wednesday, It.IsAny<IEnumerable<ForecastDto>>()));
                dayLayoutSelectorMock.Verify(x => x.MostFrequentDayLayout(DayOfWeek.Thursday, It.IsAny<IEnumerable<ForecastDto>>()));
                dayLayoutSelectorMock.Verify(x => x.MostFrequentDayLayout(DayOfWeek.Friday, It.IsAny<IEnumerable<ForecastDto>>()));
                vmMock.VerifyAll();
            }
        }
    }    
}