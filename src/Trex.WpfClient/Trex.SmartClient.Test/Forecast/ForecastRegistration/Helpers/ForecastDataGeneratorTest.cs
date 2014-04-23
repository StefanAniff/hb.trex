using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Threading;
using Moq;
using NUnit.Framework;
using Trex.SmartClient.Core.Services;
using Trex.SmartClient.Forecast.ForecastRegistration;
using Trex.SmartClient.Forecast.ForecastRegistration.Helpers;
using Trex.SmartClient.Test.TestUtil;

namespace Trex.SmartClient.Test.Forecast.ForecastRegistration.Helpers
{
    [TestFixture]
    public class ForecastDataGeneratorTest : AutoFixtureTestBase
    {               
        [Test]
        public void CreateRegistrationsFromHeaders_DateHeadersHasTwoItems_ReturnsTwoHourRegistrations()
        {
            // Arrange
            var frame = new DispatcherFrame();
            var dateHeaders = new List<ForecastRegistrationDateColumn>()
                    {
                        new ForecastRegistrationDateColumn(new DateTime(2013, 1, 1)),
                        new ForecastRegistrationDateColumn(new DateTime(2013, 1, 2))
                    };

            var viewModel = new Mock<IForecastRegistrationViewModel>();
            viewModel.SetupGet(x => x.PresenceRegistrations).Returns(new ObservableCollection<ForecastTypeRegistration>(
                    dateHeaders.Select(x =>
                        {
                            var newItem = new ForecastTypeRegistration(ForecastTestData.ProjectHoursOnlyForecastType, null);
                            x.ForecastTypeRegistration = newItem;
                            return newItem;
                        }).ToList()
                ));

            var clientReg = new ProjectRegistration();

            var generator = new ForecastRegistrationDataGenerator(null, null, null);

            // Act
            var result = generator.CreateProjectHoursFromHeaders(dateHeaders, viewModel.Object, clientReg);

            // Assert
            Assert.That(result.Count, Is.EqualTo(2));
            Assert.That(result.Any(x => x.Hours != 0), Is.False);
            Assert.That(result.Any(x => !x.Parent.Equals(clientReg)), Is.False);
        }

        [Test]
        public void CreateForecastTypeRegistrations_InputTwoColumns_ReturnsTwoForecastTypeRegistrationsWithExpectedValues()
        {
            // Arrange
            var frame = new DispatcherFrame();
            var fixture = InitializeFixture();
            var columns = new List<ForecastRegistrationDateColumn>
                {
                    new ForecastRegistrationDateColumn(new DateTime(2013, 1, 1)),
                    new ForecastRegistrationDateColumn(new DateTime(2013, 1, 2))
                };

            var forecastTypeProviderMock = ForecastTestData.MockForecastTypeProvider(fixture);
            var defaultForecastType = forecastTypeProviderMock.Object.Default;
            var forecastTypes = forecastTypeProviderMock.Object.ForecastTypes;

            var vmMock = new Mock<IForecastRegistrationViewModel>();
            vmMock.SetupGet(x => x.DateColumns).Returns(new ForecastDateColumns(columns));

            var generator = new ForecastRegistrationDataGenerator(forecastTypeProviderMock.Object, null, new CopyStatusCommandHandler());

            // Act
            var vm = vmMock.Object;
            generator.InitializeForecastTypeRegistrations(vm);

            // Assert
            Assert.That(vm.DateColumns.Any(x => !x.ForecastType.Equals(defaultForecastType)), Is.False);
            Assert.That(vm.DateColumns.Select(y => y.ForecastTypeRegistration).Any(x => !x.ForecastTypes.Equals(forecastTypes)), Is.False);
            vmMock.VerifyAll();
            forecastTypeProviderMock.VerifyAll();
        }
        

        [Test]
        public void GenerateBaseDataByDate_DateIsInFebuary2013_DateDependentCollectionsAreInitialized()
        {
            // Arrange
            var frame = new DispatcherFrame();
            var fixture = InitializeFixture();
            var generator = new ForecastRegistrationDataGenerator(ForecastTestData.MockForecastTypeProvider(fixture).Object, null, new CopyStatusCommandHandler());
            var timeEntryService = new Mock<ITimeEntryService>();
            var appSettings = new Mock<IAppSettings>();
            var viewModel = new ForecastRegistrationViewModel(generator, new ProjectSearchViewModel(null), null, null, null, null, null, 
                ForecastTestData.MockForecastTypeProvider(fixture).Object, timeEntryService.Object, appSettings.Object, null);
            var clientRegistration = new ProjectRegistration { Registrations = new ObservableCollection<ProjectHourRegistration>() };
            viewModel.ProjectRegistrations = new ProjectRegistrations { clientRegistration };            

            var date = new DateTime(2013, 2, 1);

            // Act            
            generator.GenerateBaseDataByDate(date, viewModel);

            // Assert            
            Assert.That(viewModel.DateColumns, Is.Not.Empty);
            Assert.That(viewModel.PresenceRegistrations, Is.Not.Empty);
        }
    }
}
