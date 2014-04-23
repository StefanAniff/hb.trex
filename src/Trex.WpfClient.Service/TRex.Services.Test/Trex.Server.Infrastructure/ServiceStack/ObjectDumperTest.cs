using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using NUnit.Framework;
using Trex.Common.DataTransferObjects;
using Trex.Common.ServiceStack;
using Trex.Server.Infrastructure.ServiceStack;

namespace TRex.Services.Test.Trex.Server.Infrastructure.ServiceStack
{
    [TestFixture]
    public class ObjectDumperTest
    {
        [Test]
        public void Write_WritesValuesAsExpected()
        {
            var response = new ForecastsByUserAndMonthResponse
            {
                // Arrange
                ForecastMonth = new ForecastMonthDto
                {
                    Id = 135,
                    Month = 246,
                    Year = 2012,
                    CreatedById = 43,
                    IsLocked = false,
                    UserId = 34,
                    ForecastDtos = new Collection<ForecastDto>
                                {
                                    new ForecastDto { Date = new DateTime(), Id = 999, DedicatedForecastTypeHours = 5, ForecastType = new ForecastTypeDto { Name = "Mah type"} },
                                    new ForecastDto { Date = new DateTime(), Id = 888, DedicatedForecastTypeHours = 11, ForecastType = new ForecastTypeDto { Name = "Ooh type"} }
                                }
                },

                Holidays = new List<HolidayDto>
                        {
                            new HolidayDto { Date = new DateTime(), Description = "Weee"},
                            new HolidayDto { Date = new DateTime(), Description = "Woo"},
                            new HolidayDto { Date = new DateTime(), Description = "Waa"},
                            new HolidayDto { Date = new DateTime(), Description = "Wuuuu"}
                        }
            };

            // Act
            var result = ObjectDumper.DumpToString(response);

            // Assert
            Assert.That(result, Contains.Substring("Mah type"));
            Assert.That(result, Contains.Substring("Ooh type"));
            Assert.That(result, Contains.Substring("135"));
            Assert.That(result, Contains.Substring("246"));
            Assert.That(result, Contains.Substring("2012"));
            Assert.That(result, Contains.Substring("43"));
            Assert.That(result, Contains.Substring("34"));
            Assert.That(result, Contains.Substring("999"));
            Assert.That(result, Contains.Substring("888"));
            Assert.That(result, Contains.Substring("Weee"));
            Assert.That(result, Contains.Substring("Woo"));
            Assert.That(result, Contains.Substring("Waa"));
            Assert.That(result, Contains.Substring("Wuuuu"));
        }

        [Test]
        public void Write_ObjectGraphContainsCircularReferences_CanHandle()
        {
            var first = new TestClassSibling() { Name = "first" };
            var second = new TestClassSibling() { Name = "Second" };
            var third = new TestClassSibling() { Name = "Third" };

            first.Sibling = second;
            second.Sibling = third;
            third.Sibling = first;

            var writer = new StringWriter();
            ObjectDumper.Write(first, 10, writer);
            writer.Close();
        }

        public class TestClassSibling
        {
            public TestClassSibling Sibling { get; set; }
            public string Name { get; set; }
        }
    }
}