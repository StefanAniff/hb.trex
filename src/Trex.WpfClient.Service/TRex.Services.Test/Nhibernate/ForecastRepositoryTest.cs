using System;
using NUnit.Framework;
using TRex.Services.Test.Util;
using Trex.Server.Core.Model;
using Trex.Server.Core.Model.Forecast;
using Trex.Server.Infrastructure.Implemented;

namespace TRex.Services.Test.Nhibernate
{    
    public class ForecastRepositoryTest : DbTest
    {
        [TestFixture]
        public class GetRestOfYearTest : ForecastRepositoryTest
        {
            [Test]
            public void GetRestOfYear_CanExtract()
            {
                // Arrange
                var user = new GenericRepository<User>(Session).SaveOrUpdate(DataGenerator.GetUser());

                var client = DataGenerator.GetCustomer(user);
                client.Internal = true;
                client = new GenericRepository<Company>(Session).SaveOrUpdate(client);

                var project = new GenericRepository<Project>(Session).SaveOrUpdate(DataGenerator.GetProject(user, client));
                var forecastType = new GenericRepository<ForecastType>(Session).SaveOrUpdate(new ForecastType("Client", "", true, true));
                var illnessForecastType = new GenericRepository<ForecastType>(Session).SaveOrUpdate(new ForecastType("Illness", "", false, true) {StatisticsInclusion = false});
                var forecastTypeRepo = new ForecastTypeRepository(Session);
                forecastTypeRepo.SaveOrUpdate(forecastType);

                var repo = new ForecastRepository(Session);
                var month = new ForecastMonth(1, 2013, 3, user, user);

                CreateForecastWithSingleProjectRegistration(month, new DateTime(2013, 1, 1), forecastType, project, 7.5m, 1);
                CreateForecastWithSingleProjectRegistration(month, new DateTime(2013, 1, 2), forecastType, project, 6, 1);
                CreateForecastWithSingleProjectRegistration(month, new DateTime(2013, 1, 3), forecastType, project, 5, 1);
                CreateForecastWithSingleProjectRegistration(month, new DateTime(2013, 1, 4), illnessForecastType, null, 0, 7.5m);

                var monthRepo = new ForecastMonthRepository(Session);
                monthRepo.SaveOrUpdate(month);

                // Act
                var all = repo.GetHourSumByCriteria(1, true, DateSpan.YearDateSpan(2013));
                var allExcluded = repo.GetHourSumByCriteria(1, false, DateSpan.YearDateSpan(2013));
                var twoByDateSpan = repo.GetHourSumByCriteria(1, true, new DateSpan { From = new DateTime(2013, 1, 1), To = new DateTime(2013, 1, 2)});

                // Assert
                Assert.That(all, Is.EqualTo(21.5m)); // 7.5 + 6 + 5 + 1 + 1 + 1 (projecthours and dedicated hours (1's))
                Assert.That(allExcluded, Is.EqualTo(0));
                Assert.That(twoByDateSpan, Is.EqualTo(15.5m));
            }
        }

        [TestFixture]
        public class GetDateCountByForecastType : ForecastRepositoryTest
        {
            [Test]
            public void GetDateCountByForecastType_CanExtract()
            {
                // Arrange
                var user = new GenericRepository<User>(Session).SaveOrUpdate(DataGenerator.GetUser());
                var client = new GenericRepository<Company>(Session).SaveOrUpdate(DataGenerator.GetCustomer(user));
                var project = new GenericRepository<Project>(Session).SaveOrUpdate(DataGenerator.GetProject(user, client));
                var forecastType = new GenericRepository<ForecastType>(Session).SaveOrUpdate(new ForecastType("Client", "", true, false));
                var forecastTypeRepo = new ForecastTypeRepository(Session);
                forecastTypeRepo.SaveOrUpdate(forecastType);
                var clientForecastTypeId = forecastType.Id;

                var month = new ForecastMonth(1, 2013, 3, user, user);
                var repo = new ForecastRepository(Session);
                CreateForecastWithSingleProjectRegistration(month, new DateTime(2013, 1, 1), forecastType, project, 3);
                forecastType = new GenericRepository<ForecastType>(Session).SaveOrUpdate(new ForecastType("Vacation", "", false, false));
                forecastTypeRepo.SaveOrUpdate(forecastType);
                var vacationForecastTypeId = forecastType.Id;
                CreateForecastWithSingleProjectRegistration(month, new DateTime(2013, 1, 2), forecastType, null, 0);
                CreateForecastWithSingleProjectRegistration(month, new DateTime(2013, 1, 3), forecastType, null, 0);

                var monthRepo = new ForecastMonthRepository(Session);
                monthRepo.SaveOrUpdate(month);

                // Act
                var clientForecastCount = repo.GetForecastCountByForecastType(1, clientForecastTypeId, DateSpan.YearDateSpan(2013));
                var vacationForecastcount = repo.GetForecastCountByForecastType(1, vacationForecastTypeId, DateSpan.YearDateSpan(2013));

                // Assert
                Assert.That(clientForecastCount, Is.EqualTo(1));
                Assert.That(vacationForecastcount, Is.EqualTo(2));
            }
        }

        private void CreateForecastWithSingleProjectRegistration(ForecastMonth month, DateTime date, ForecastType forecastType, Project project, decimal hours, decimal? dedicatedHours = null)
        {
            var forecast = month.AddForecast(date, forecastType, dedicatedHours);
            if (forecastType.SupportsProjectHours)
                forecast.AddProjectRegistration(project, hours);
        }


    }
}