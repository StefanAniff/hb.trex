using System.Collections.Generic;
using System.Collections.ObjectModel;
using Moq;
using NUnit.Framework;
using Ploeh.AutoFixture;
using Trex.Common.DataTransferObjects;
using Trex.Common.ServiceStack;
using Trex.SmartClient.Core.Model;
using Trex.SmartClient.Core.Services;
using Trex.SmartClient.Forecast.ForecastOverview;
using Trex.SmartClient.Forecast.ForecastOverview.Helpers;
using Trex.SmartClient.Forecast.Shared;
using Trex.SmartClient.Test.TestUtil;
using System.Linq;

namespace Trex.SmartClient.Test.Forecast.ForecastOverview
{
    [TestFixture]
    public class ForecastOverviewSearchOptionsTest : AutoFixtureTestBase
    {
        [Test]
        public void SelectedForecastTypeId_SelectionIsEmptyForecastType_ReturnsNull()
        {
            // Arrange

            var sut = Fixture.Create<ForecastOverviewSearchOptions>();
            sut.SelectedForecastType = new ForecastOverviewSearchOptions.EmptyForecastType();

            // Act
            var result = sut.SelectedForecastTypeId;

            // Assert
            Assert.That(result, Is.Null);
        }

        [Test]
        public void SelectedProjectSetter_SelectedCompanyHasValue_SelectedCompanyIsSetToNull()
        {
            // Arrange
            var sut = Fixture.Create<ForecastOverviewSearchOptions>();
            sut.SelectedCompany = Fixture.Create<Company>();

            // Act
            sut.SelectedProject = Fixture.Create<Project>();

            // Assert
            Assert.That(sut.SelectedCompany, Is.Null);
        }

        [Test]
        public void SelectedCompanySetter_SelectedProjectHasValue_SelectedProjectIsSetToNull()
        {
            // Arrange
            var sut = Fixture.Create<ForecastOverviewSearchOptions>();
            sut.SelectedProject = Fixture.Create<Project>();

            // Act
            sut.SelectedCompany = Fixture.Create<Company>();

            // Assert
            Assert.That(sut.SelectedProject, Is.Null);
        }

        [Test]
        public void SetupVisualsBySearchOption_OneForecastForEachDisplayHandlerType_AppliesTheExpectedDisplayHandlers()
        {
            // Arrange
            const int projectForecastTypeId = 1;
            var projectForecastType = new ForecastType { Id = projectForecastTypeId };
            var projectSupportingForecastType = new ForecastType {Id = 2, SupportsProjectHours = true};
            var nonProjectSupportingForecastType = new ForecastType {Id = 3, SupportsProjectHours = false};


            var forecastMonth = new ForecastOverviewForecastMonth
                {
                    Forecasts = new List<ForecastOverviewForecast>()
                };

            // Non workday forecast
            var nonWorkDayForecastMock = CreateMock<ForecastOverviewForecast>();
            nonWorkDayForecastMock.SetupGet(x => x.IsWorkDay).Returns(false);
            forecastMonth.Forecasts.Add(nonWorkDayForecastMock.Object);

            // Project forecast
            var projectForecastMock = CreateWorkdayForecastMock(projectForecastType);
            forecastMonth.Forecasts.Add(projectForecastMock.Object);

            // Projectsupporting forecast
            var projectSupportingForecastMock = CreateWorkdayForecastMock(projectSupportingForecastType);
            forecastMonth.Forecasts.Add(projectSupportingForecastMock.Object);

            // Non projectsupporting forecast
            var nonProjectSupportingForecastMock = CreateWorkdayForecastMock(nonProjectSupportingForecastType);
            forecastMonth.Forecasts.Add(nonProjectSupportingForecastMock.Object);

            var sut = Fixture.Create<ForecastOverviewSearchOptions>();
            sut.SelectedTabIndex = 0; // Indicates that searching by registration

            // Act
            sut.SetupVisualsBySearchOption(forecastMonth, projectForecastTypeId);

            // Assert
            nonWorkDayForecastMock.VerifySet(x => x.DisplayHandler = It.IsAny<EmptyDisplayHandler>());
            projectForecastMock.VerifySet(x => x.DisplayHandler = It.IsAny<PureProjectTypeDisplayHandler>());
            projectSupportingForecastMock.VerifySet(x => x.DisplayHandler = It.IsAny<SupportsProjectsWithFocusDisplayHandler>());
            nonProjectSupportingForecastMock.VerifySet(x => x.DisplayHandler = It.IsAny<NonProjectSupportingDisplayHandler>());
        }

        [Test]
        public void SetupVisualsBySearchOption_OneProjectSupportingForecastWithNoProjectCompanyFocus_AppliesSupportsProjectsNoFocusDisplayHandler()
        {
            // Arrange
            var forecastMonth = new ForecastOverviewForecastMonth
            {
                Forecasts = new List<ForecastOverviewForecast>()
            };

            // Projectsupporting forecast
            var projectSupportingForecastMock = CreateWorkdayForecastMock(new ForecastType { Id = 2, SupportsProjectHours = true });
            forecastMonth.Forecasts.Add(projectSupportingForecastMock.Object);

            var sut = Fixture.Create<ForecastOverviewSearchOptions>();
            sut.SelectedCompany = null;
            sut.SelectedProject = null;

            // Act
            sut.SetupVisualsBySearchOption(forecastMonth, 1);

            // Assert
            projectSupportingForecastMock.VerifySet(x => x.DisplayHandler = It.IsAny<SupportsProjectsNoFocusDisplayHandler>());            

        }

        private Mock<ForecastOverviewForecast> CreateWorkdayForecastMock(ForecastType forecastType)
        {
            var forecastMock = CreateMock<ForecastOverviewForecast>();
            forecastMock.SetupGet(x => x.IsWorkDay).Returns(true);
            forecastMock.SetupGet(x => x.ForecastType).Returns(forecastType);
            return forecastMock;
        }

        [Test]
        public void DoSearch_SelectedTabIndexIsSearchByRegistration_InvokesGetBySearchWithRegistrationSpecificParams()
        {
            // Arrange
            var forecastServiceMock = CreateMock<IForecastService>();
            var sut = new ForecastOverviewSearchOptions(forecastServiceMock.Object, CreateMock<ForecastOverviewUserSearchOptions>().Object)
                {
                    SelectedTabIndex = ForecastOverviewViewSetup.SearchByRegistrationTabIndex
                };

            // Act
            var result = sut.DoSearch(1, 2013);

            // Assert
            forecastServiceMock.Verify(x => x.GetBySearch(1, 2013, It.IsAny<int?>(), It.IsAny<int?>(), It.IsAny<int?>()));
        }

        [Test]
        public void DoSearch_SelectedTabIndexIsSearchByUser_InvokesGetBySearchWithUserIdsParams()
        {
            // Arrange
            var forecastServiceMock = CreateMock<IForecastService>();
            var sut = new ForecastOverviewSearchOptions(forecastServiceMock.Object, CreateMock<ForecastOverviewUserSearchOptions>().Object)
                {
                    SelectedTabIndex = ForecastOverviewViewSetup.SearchByUserTabIndex
                };

            // Act
            var result = sut.DoSearch(1, 2013);

            // Assert
            forecastServiceMock.Verify(x => x.GetBySearch(1, 2013, It.IsAny<IEnumerable<int>>()));
        }

        [Test]
        public void DoSearch_AllUsersMarkerSelected_InvokesGetBySearchWithAllUserIds()
        {
            // Arrange
            var forecastServiceMock = CreateMock<IForecastService>();

            var users = new[]
                {
                    ForecastUserDto.AllUsersDto(),
                    new ForecastUserDto { UserId = 1},
                    new ForecastUserDto { UserId = 2}, 
                };

            var userSearchOptionsMock = CreateMock<ForecastOverviewUserSearchOptions>();
            userSearchOptionsMock.SetupGet(x => x.Users).Returns(new ObservableCollection<ForecastUserDto>(users));
            userSearchOptionsMock.SetupGet(x => x.SelectedUsers).Returns(new ObservableCollection<ForecastUserDto>(users.Where(x => x.IsAllUsers)));            

            var sut = new ForecastOverviewSearchOptions(forecastServiceMock.Object, userSearchOptionsMock.Object)
            {
                SelectedTabIndex = ForecastOverviewViewSetup.SearchByUserTabIndex
            };

            // Act            
            IEnumerable<int> invokedWithUserIds = new List<int>();
            forecastServiceMock
                .Setup(x => x.GetBySearch(1, 2013, It.IsAny<IEnumerable<int>>()))
                .Callback<int, int, IEnumerable<int>>((x, y, z) => invokedWithUserIds = z);

            sut.DoSearch(1, 2013);

            // Assert
            forecastServiceMock.Verify(x => x.GetBySearch(1, 2013, It.IsAny<IEnumerable<int>>()));
            Assert.That(invokedWithUserIds.Count(), Is.EqualTo(2));
            Assert.That(invokedWithUserIds.ElementAt(0), Is.EqualTo(1));
            Assert.That(invokedWithUserIds.ElementAt(1), Is.EqualTo(2));

        }

        [Test]
        public void TryAddMissingUsers_ServerReturnsNoResultsForAUser_MissingResponseUsersAreAdded()
        {
            // Arrange
            var forecastServiceMock = CreateMock<IForecastService>();
            var userSearchOptionsMock = CreateMock<ForecastOverviewUserSearchOptions>();
            userSearchOptionsMock.SetupGet(x => x.SelectedUsers).Returns(new ObservableCollection<ForecastUserDto>
                {
                    new ForecastUserDto {UserId = 2},
                    new ForecastUserDto {UserId = 3},
                    new ForecastUserDto {UserId = 4},
                });

            var response = new ForecastSearchResponse
                {
                    ForecastMonths = new List<ForecastMonthDto>
                        {
                            new ForecastMonthDto { UserId = 2 }, 
                            new ForecastMonthDto { UserId = 4 }, // Month with userid 3 is intentionally missing
                        }
                };

            var sut = new ForecastOverviewSearchOptions(forecastServiceMock.Object, userSearchOptionsMock.Object);

            // Act
            sut.TryAddMissingUsers(1, 2013, response);

            // Assert
            Assert.That(response.ForecastMonths.FirstOrDefault(x => x.UserId == 3), Is.Not.Null);
        }

        [Test]
        public void TryAddMissingUsers_AllUsersIsSelected_AllUsersAreAdded()
        {
            // Arrange
            var allUserDto = ForecastUserDto.AllUsersDto();

            var forecastServiceMock = CreateMock<IForecastService>();
            var userSearchOptionsMock = CreateMock<ForecastOverviewUserSearchOptions>();
            userSearchOptionsMock.SetupGet(x => x.Users).Returns(new ObservableCollection<ForecastUserDto>
                {
                    allUserDto,
                    new ForecastUserDto {UserId = 2},
                    new ForecastUserDto {UserId = 3},
                    new ForecastUserDto {UserId = 4},
                });
            userSearchOptionsMock.SetupGet(x => x.SelectedUsers).Returns(new ObservableCollection<ForecastUserDto> { allUserDto });

            var response = new ForecastSearchResponse
            {
                ForecastMonths = new List<ForecastMonthDto>
                        {
                            new ForecastMonthDto { UserId = 4 }, // Month with userid 2 and 3 is intentionally missing
                        }
            };

            var sut = new ForecastOverviewSearchOptions(forecastServiceMock.Object, userSearchOptionsMock.Object);

            // Act
            sut.TryAddMissingUsers(1, 2013, response);

            // Assert
            Assert.That(response.ForecastMonths.FirstOrDefault(x => x.UserId == 2), Is.Not.Null);
            Assert.That(response.ForecastMonths.FirstOrDefault(x => x.UserId == 3), Is.Not.Null);
            Assert.That(response.ForecastMonths.FirstOrDefault(x => x.UserId == 4), Is.Not.Null);
        }
    }
}