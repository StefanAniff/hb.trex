using System;
using System.Collections.ObjectModel;
using NUnit.Framework;
using Ploeh.AutoFixture;
using Trex.Common.DataTransferObjects;
using Trex.SmartClient.Forecast.ForecastRegistration.Helpers;
using Trex.SmartClient.Test.TestUtil;
using System.Linq;

namespace Trex.SmartClient.Test.Forecast.ForecastRegistration.Helpers
{
    [TestFixture]
    public class MostFrequentDayLayoutSelectorTest : AutoFixtureTestBase
    {
        [Test]
        public void MethodName_SetupDescription_ExpectedResult()
        {
            // Arrange
            var forecastType = new ForecastTypeDto
                {
                    Id = 22,                    
                };

            var project1 = new ProjectDto {Id = 1, Name = "project1"};
            var project2 = new ProjectDto {Id = 2, Name = "project2"};

            var forecastTwin1 = new ForecastDto
                {
                    Id = 1,
                    Date = new DateTime(2013, 11, 11),                    
                    ForecastType = forecastType,
                    DedicatedForecastTypeHours = 3,
                    ForecastProjectHoursDtos = new Collection<ForecastProjectHoursDto>
                        {
                            new ForecastProjectHoursDto { Project = project1, Hours = 4},
                            new ForecastProjectHoursDto { Project = project2, Hours = 4.5m}
                        }
                };

            var forecastTwin2 = new ForecastDto
            {
                Id = 2,
                Date = new DateTime(2013, 11, 18),
                ForecastType = forecastType,
                DedicatedForecastTypeHours = 3,
                ForecastProjectHoursDtos = new Collection<ForecastProjectHoursDto>
                        {
                            new ForecastProjectHoursDto { Project = project1, Hours = 4},
                            new ForecastProjectHoursDto { Project = project2, Hours = 4.5m}
                        }
            };

            var forecastBastard1 = new ForecastDto
            {
                Id = 1,
                Date = new DateTime(2013, 11, 4),
                ForecastType = forecastType,
                DedicatedForecastTypeHours = 2, // Difference here!
                ForecastProjectHoursDtos = new Collection<ForecastProjectHoursDto>
                        {
                            new ForecastProjectHoursDto { Project = project1, Hours = 4},
                            new ForecastProjectHoursDto { Project = project2, Hours = 4.5m}
                        }
            };

            var forecastBastard2 = new ForecastDto
            {
                Id = 2,
                Date = new DateTime(2013, 11, 25),
                ForecastType = forecastType,
                DedicatedForecastTypeHours = 3,
                ForecastProjectHoursDtos = new Collection<ForecastProjectHoursDto>
                        {
                            new ForecastProjectHoursDto { Project = project1, Hours = 4},
                            new ForecastProjectHoursDto { Project = project2, Hours = 1} // Difference here!
                        }
            };

            var forecastBastard3 = new ForecastDto
            {
                Id = 2,
                Date = new DateTime(2013, 12, 12),
                ForecastType = null, // Difference here!
                DedicatedForecastTypeHours = 3,
                ForecastProjectHoursDtos = new Collection<ForecastProjectHoursDto>
                        {
                            new ForecastProjectHoursDto { Project = project1, Hours = 4},
                            new ForecastProjectHoursDto { Project = project2, Hours = 4.5m} 
                        }
            };

            var notMondayBastard = new ForecastDto
            {
                Id = 2,
                Date = new DateTime(2013, 12, 12),
                ForecastType = null, // Difference here!
                DedicatedForecastTypeHours = 3,
                ForecastProjectHoursDtos = new Collection<ForecastProjectHoursDto>
                        {
                            new ForecastProjectHoursDto { Project = project1, Hours = 4},
                            new ForecastProjectHoursDto { Project = project2, Hours = 4.5m} 
                        }
            };

            var sut = Fixture.Create<MostFrequentDayLayoutSelector>();

            // Act
            var result = sut.MostFrequentDayLayout(DayOfWeek.Monday, new[]
                {
                    forecastBastard1, 
                    forecastTwin1, 
                    forecastTwin2, 
                    forecastBastard2, 
                    forecastBastard3,
                    notMondayBastard
                });

            // Assert
            Assert.That(result.ForecastType.Id, Is.EqualTo(22));
            Assert.That(result.DedicatedForecastTypeHours, Is.EqualTo(3));
            Assert.That(result.ForecastProjectHoursDtos.ElementAt(0).Project.Name, Is.EqualTo("project1"));
            Assert.That(result.ForecastProjectHoursDtos.ElementAt(0).Hours, Is.EqualTo(4));
            Assert.That(result.ForecastProjectHoursDtos.ElementAt(1).Project.Name, Is.EqualTo("project2"));
            Assert.That(result.ForecastProjectHoursDtos.ElementAt(1).Hours, Is.EqualTo(4.5m));
        }
    }
}