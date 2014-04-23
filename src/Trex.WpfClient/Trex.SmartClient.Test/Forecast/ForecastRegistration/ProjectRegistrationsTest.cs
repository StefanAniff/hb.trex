using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Threading;
using NUnit.Framework;
using Ploeh.AutoFixture;
using Trex.SmartClient.Core.Extensions;
using Trex.SmartClient.Forecast.ForecastRegistration;
using System.Linq;
using Trex.SmartClient.Test.TestUtil;

namespace Trex.SmartClient.Test.Forecast.ForecastRegistration
{
    [TestFixture]
    public class ProjectRegistrationsTest : AutoFixtureTestBase
    {
        private void AddDateColumnWithClientRegistration(ProjectRegistration projectRegistration, decimal hours, bool isEditEnabled)
        {
            var dateColumn = new ForecastRegistrationDateColumn(new DateTime(2013, 1, 1));
            var clientHourRegistration = new ProjectHourRegistration(projectRegistration)
                {
                    Hours = hours,
                    IsEditEnabled = isEditEnabled
                };
            dateColumn.AddProjectHours(clientHourRegistration);
            projectRegistration.Registrations.Add(clientHourRegistration);         
        }

        [Test]
        public void ClientHourRegistraionsWithValue_ContainsThreeItemsWhereTwoMeetsTheCondition_ReturnsTwoElements()
        {
            // Arrange
            var fixture = InitializeFixture();
            var clientReg = fixture.Create<ProjectRegistration>();

            AddDateColumnWithClientRegistration(clientReg, 3, true);
            AddDateColumnWithClientRegistration(clientReg, 2, true);
            AddDateColumnWithClientRegistration(clientReg, 5, false);            

            var sut = new ProjectRegistrations
                {
                    clientReg
                };

            // Act
            var result = sut.ProjectHourRegistraionsWithValue;

            // Assert
            Assert.That(result.Count(), Is.EqualTo(2));
            Assert.That(result.SingleOrDefault(x => x.Hours == 3), Is.Not.Null);
            Assert.That(result.SingleOrDefault(x => x.Hours == 2), Is.Not.Null);
        }

        [Test]
        public void ApplyHoursForward_InputIsTenthOfMonth_DaysAfterTenthAreUpdated()
        {
            // Arrange
            var clientRegistration = new ProjectRegistration();
            clientRegistration.Registrations = new ObservableCollection<ProjectHourRegistration>(CreateHoursForMonth(1, 2013, 7.5m, clientRegistration));
            var source = clientRegistration
                            .Registrations
                            .Single(x => x.DateColumn.Date.Day == 10);
            source.Hours = 6;

            // Act
            clientRegistration.ApplyHoursForward(source);

            // Assert
            var preTenth = clientRegistration.Registrations.Where(x => x.DateColumn.Date.Day < 10);
            Assert.That(preTenth.Any(x => x.Hours != 7.5m), Is.False);
            var postTenth = clientRegistration.Registrations.Where(x => x.DateColumn.Date.Day > 10);
            Assert.That(postTenth.Any(x => x.Hours != 6), Is.False);
        }

        [Test]
        public void ApplyHoursBackwards_IntputIsTwentiethOfMonth_DaysBeforeTwentiethAreUpdated()
        {
            // Arrange
            var clientRegistration = new ProjectRegistration();
            clientRegistration.Registrations = new ObservableCollection<ProjectHourRegistration>(CreateHoursForMonth(2, 2013, 7.5m, clientRegistration));

            var source = clientRegistration
                            .Registrations
                            .Single(x => x.DateColumn.Date.Day == 20);
            source.Hours = 4.75m;

            // Act
            clientRegistration.ApplyHoursBackward(source);

            // Assert
            var preTwentieth = clientRegistration.Registrations.Where(x => x.DateColumn.Date.Day < 20);
            Assert.That(preTwentieth.Any(x => x.Hours != 4.75m), Is.False);
            var postTenth = clientRegistration.Registrations.Where(x => x.DateColumn.Date.Day > 20);
            Assert.That(postTenth.Any(x => x.Hours != 7.5m), Is.False);
        }

        [Test]
        public void ApplyHoursToAll_IntputIsFifteenthOfMonth_AllDaysAreupdated()
        {
            // Arrange
            var clientRegistration = new ProjectRegistration();
            clientRegistration.Registrations = new ObservableCollection<ProjectHourRegistration>(CreateHoursForMonth(2, 2013, 7.5m, clientRegistration));

            var source = clientRegistration
                            .Registrations
                            .Single(x => x.DateColumn.Date.Day == 15);
            source.Hours = 0;

            // Act
            clientRegistration.ApplyHoursToAll(source);

            // Assert
            Assert.That(clientRegistration.Registrations.Any(x => x.Hours != 0), Is.False);            
        }

        private IEnumerable<ProjectHourRegistration> CreateHoursForMonth(int month, int year, decimal defaultHours, ProjectRegistration parent)
        {
            var result = new List<ProjectHourRegistration>();
            var day = new DateTime(year, month, 1);
            while (day.Month == month)
            {
                if (!day.IsWeekend())
                {
                    var dateColumn = new ForecastRegistrationDateColumn(day);
                    var hourReg = new ProjectHourRegistration(parent) { Hours = defaultHours };
                    dateColumn.AddProjectHours(hourReg);
                    result.Add(hourReg);
                }

                day = day.AddDays(1);
            }
            return result;
        }

        [Test]
        public void TryDistributeTotalInput_InputStringIsNotInt_AbortsUpdate()
        {
            // Arrange
            var sut = new ProjectRegistration { Registrations = new ObservableCollection<ProjectHourRegistration>()};

            // Act
            sut.TryDistributeTotalInput("abc");

            // Assert
        }

        [Test]
        public void TryDistributeTotalInput_ClientHourRegistrationHoursAreUpdatedAsExpected()
        {
            // Arrange
            new DispatcherFrame();
            var headerClient = new ForecastRegistrationDateColumn(new DateTime(2013, 1, 1))
                {
                    ForecastTypeRegistration = new ForecastTypeRegistration(ForecastTestData.ProjectHoursOnlyForecastType, null)
                };

            var headerOpen = new ForecastRegistrationDateColumn(new DateTime(2013, 1, 2))
                {
                    ForecastTypeRegistration = new ForecastTypeRegistration(ForecastTestData.DedicatedAndProjectHoursForcastType, null)
                };

            var sut = new ProjectRegistration { Registrations = new ObservableCollection<ProjectHourRegistration>() };
            AddClientHourRegistration(sut, headerClient, 7.5m);
            AddClientHourRegistration(sut, headerOpen, 4);            

            // Act
            sut.TryDistributeTotalInput("14");

            // Assert
            Assert.That(sut.Registrations.Single(x => x.SelectedPresencetypeSupportsProjectHoursOnly()).Hours, Is.EqualTo(7));
            Assert.That(sut.Registrations.Single(x => x.SelectedPresencetypeSupportsDedicatedHours).Hours, Is.EqualTo(7));
        }

        private static void AddClientHourRegistration(ProjectRegistration projectRegistration, ForecastRegistrationDateColumn dateColumn, decimal hours)
        {
            var clientHourReg = new ProjectHourRegistration(projectRegistration) { Hours = hours };
            dateColumn.AddProjectHours(clientHourReg);
            projectRegistration.Registrations.Add(clientHourReg);
        }
    }
}