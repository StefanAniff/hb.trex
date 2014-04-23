using System;
using System.Windows.Threading;
using NUnit.Framework;
using Trex.SmartClient.Core.Extensions;
using Trex.SmartClient.Forecast.ForecastRegistration;
using Trex.SmartClient.Forecast.ForecastRegistration.Helpers;
using System.Linq;

namespace Trex.SmartClient.Test.Forecast.ForecastRegistration.Helpers
{    
    [TestFixture]
    public class CopyStatusCommandHandlerTest
    {
        [Test]
        public void ExecuteCopyForward_OnlyFuturePresenceRegistrationsAreAffected()
        {
            // Arrange
            new DispatcherFrame();
            var presenceTypes = ForecastTestData.ForecastTypesList;
            var dateHeaders = new DateTime(2013, 2, 1)
                                .CreateDatesForMonth()
                                .Select(x => new ForecastRegistrationDateColumn(x))
                                .ToList();

            var hostCollection = dateHeaders
                                    .Select(x =>
                                        {
                                            var newItem = new ForecastTypeRegistration(ForecastTestData.ProjectHoursOnlyForecastType, presenceTypes);
                                            x.ForecastTypeRegistration = newItem;
                                            return newItem;
                                        })
                                    .ToList();

            var source = hostCollection.Single(x => x.DateColumn.Date.Equals(new DateTime(2013, 2, 15)));
            source.SelectedForecastType = ForecastTestData.SimpleForecastType;

            var sut = new CopyStatusCommandHandler();

            // Act
            sut.ExecuteCopyForward(source, hostCollection);

            // Assert
            var preFifteenth = hostCollection.Where(x => x.DateColumn.Date < new DateTime(2013, 2, 15) && x.SelectedForecastType.Equals(ForecastTestData.ProjectHoursOnlyForecastType)).ToList();
            Assert.That(preFifteenth.Count, Is.EqualTo(14));

            var postFifteenth = hostCollection.Where(x => x.DateColumn.Date >= new DateTime(2013, 2, 15) && x.SelectedForecastType.Equals(ForecastTestData.SimpleForecastType)).ToList();
            Assert.That(postFifteenth.Count, Is.EqualTo(10)); // 10 since weekends are excluded
        }
    }
}