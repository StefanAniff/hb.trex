using System;
using System.Collections.Generic;
using System.Windows.Threading;
using NUnit.Framework;
using Ploeh.AutoFixture;
using Trex.Common.DataTransferObjects;
using Trex.SmartClient.Forecast.ForecastRegistration;
using Trex.SmartClient.Forecast.Shared;
using Trex.SmartClient.Test.TestUtil;

namespace Trex.SmartClient.Test.Forecast.ForecastRegistration
{
    [TestFixture]
    public class ForecastDateColumnTest : AutoFixtureTestBase
    {
        private void AddClientHourRegistration(ProjectRegistration projectRegistration, ForecastRegistrationDateColumn dateColumn, decimal hours)
        {
            var clientHourReg = new ProjectHourRegistration(projectRegistration) { Hours = hours};
            dateColumn.AddProjectHours(clientHourReg);
        }

        [Test]
        public void CalculateTotal_SumsClinetHourRegistrationsAndForecastTypeDedidcatedHours()
        {
            // Arrange
            new DispatcherFrame();

            var fixture = InitializeFixture();
            var clientReg = fixture.Create<ProjectRegistration>();

            var sut = new ForecastRegistrationDateColumn(new DateTime(2013, 1, 1));

            AddClientHourRegistration(clientReg, sut, 3);
            AddClientHourRegistration(clientReg, sut, 4);
            sut.ForecastTypeRegistration = new ForecastTypeRegistration(ForecastTestData.DedicatedAndProjectHoursForcastType, ForecastTestData.ForecastTypesList) { DedicatedHours = 1};

            // Act
            sut.CalculateTotal();

            // Assert
            Assert.That(sut.DateTotal.Hours, Is.EqualTo(8));
        }

        [Test]
        public void AddPresenceRegistration_PresenceRegistrationIsAddedToCollection()
        {
            // Arrange            
            new DispatcherFrame();

            var sut = new ForecastRegistrationDateColumn(new DateTime(2013, 1, 1));

            // Act
            var presenceReg = new ForecastTypeRegistration(ForecastTestData.ProjectHoursOnlyForecastType, new List<ForecastType>());
            sut.ForecastTypeRegistration = presenceReg;

            // Assert
            Assert.That(sut.ForecastTypeRegistration, Is.EqualTo(presenceReg));
        }        

        [Test]
        public void SetToHoliday_HolidayToSetFromHasDifferentDate_ThrowsException()
        {
            // Arrange
            var holiday = new HolidayDto { Date = new DateTime(2013, 1, 1), Description = "buuh" };
            var dateItem = new ForecastRegistrationDateColumn(new DateTime(2013, 1, 2));

            // Act
            var error = Assert.Throws<Exception>(() => dateItem.SetToHoliday(holiday));

            // Assert
            Assert.That(error.Message, Is.EqualTo("Holiday buuh does not have same date as 02-01-2013"));
        }

        [Test]
        public void SetToHoliday_HolidayHasSameDateAsDateItem_HolidayDescriptionGetValue()
        {
            // Arrange
            var holiday = new HolidayDto { Date = new DateTime(2013, 1, 1), Description = "buuh" };
            var dateItem = new ForecastRegistrationDateColumn(new DateTime(2013, 1, 1));

            // Act
            dateItem.SetToHoliday(holiday);

            // Assert
            Assert.That(dateItem.IsHoliday, Is.True);
            Assert.That(dateItem.HolidayDesciption, Is.EqualTo("buuh"));
        }

        [Test]
        public void DateTotalSetter_WhenSetThenDateColumOnValueIsSetToSelf()
        {
            // Arrange
            var fixture = InitializeFixture();
            fixture.Register(() => new ForecastRegistrationDateColumn(fixture.Create<DateTime>()));
            
            var hourRegistration = fixture.Create<HourRegistration>();
            var sut = fixture.Create<ForecastRegistrationDateColumn>();

            // Act
            sut.DateTotal = hourRegistration;

            // Assert
            Assert.That(hourRegistration.DateColumn, Is.EqualTo(sut));
        }

        [Test]
        public void AddClientHourRegistration_SetsDateColumnOnClientHourRegistrationToSelf()
        {
            // Arrange
            var fixture = InitializeFixture();
            fixture.Register(() => new ForecastRegistrationDateColumn(fixture.Create<DateTime>()));

            fixture.Register(() => new ProjectHourRegistration(fixture.Create<ProjectRegistration>()));
            var clientHourReg = fixture.Create<ProjectHourRegistration>();

            var sut = fixture.Create<ForecastRegistrationDateColumn>();

            // Act
            sut.AddProjectHours(clientHourReg);

            // Assert
            Assert.That(clientHourReg.DateColumn, Is.EqualTo(sut));
        }
    }
}