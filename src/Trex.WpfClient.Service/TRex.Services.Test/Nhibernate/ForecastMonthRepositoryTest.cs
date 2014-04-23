using System.Collections.Generic;
using NUnit.Framework;
using TRex.Services.Test.Util;
using Trex.Server.Core.Services;
using Trex.Server.Infrastructure.Implemented;
using TrexSL.Web.ServiceStackServices.Forecast.ForecastMonthCriterias;

namespace TRex.Services.Test.Nhibernate
{
    [TestFixture]
    public class ForecastMonthRepositoryTest : DbTest
    {
        [Test]
        public void GetBySearchCriterias_ForecastTypeCompanyCombo_CanExecuteSql()
        {
            // Arrange

            var criterias = new List<IForecastMonthQueryCriteria>
                {
                    new ForcastTypeQueryCriteria(1),                    
                    new ForecastCompanyQueryCriteria(1)
                };

            var sut = new ForecastMonthRepository(Session);

            // Act
            var result = sut.GetBySearchCriterias(criterias, 1, 2013);

            // Assert
        }

        [Test]
        public void GetBySearchCriterias_ForecastTypeProjectCombo_CanExecuteSql()
        {
            // Arrange

            var criterias = new List<IForecastMonthQueryCriteria>
                {
                    new ForcastTypeQueryCriteria(1),                    
                    new ForecastProjectQueryCriteria(1)
                };

            var sut = new ForecastMonthRepository(Session);

            // Act
            var result = sut.GetBySearchCriterias(criterias, 1, 2013);

            // Assert

        }

        [Test]
        public void GetByUsersAndMonth_CanExecuteQuery()
        {
            // Arrange

            var sut = new ForecastMonthRepository(Session);

            // Act
            var result = sut.GetByUsersAndMonth(new[] {1, 2}, 1, 2013);

            // Assert

        }
    }
}