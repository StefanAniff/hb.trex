using System;
using NUnit.Framework;
using Trex.SmartClient.Core.Extensions;
using Trex.SmartClient.Infrastructure.Extensions;
using System.Linq;

namespace Trex.SmartClient.Test.Infrastructure.Extensions
{    
    public class DateTimeExtensionsTest
    {
        [TestFixture]
        public class FirstDayOfPreviousMonthTest
        {
            [Test]
            public void DateIsJanuary2013_ReturnsFirstOfDecember2012()
            {
                // Arrange
                var date = new DateTime(2013, 1, 15);

                // Act
                var result = date.FirstDayOfPreviousMonth();

                // Assert
                Assert.That(result, Is.EqualTo(new DateTime(2012, 12, 1)));
            }
        }

        [TestFixture]
        public class FirstDayOfNextMonthTest
        {
            [Test]
            public void DateIsLastDayInJanuary_ReturnsFirstOfFebuary()
            {
                // Arrange
                var date = new DateTime(2013, 1, 31);

                // Act
                var result = date.FirstDayOfNextMonth();

                // Assert
                Assert.That(result, Is.EqualTo(new DateTime(2013, 2, 1)));
            }
        }

        [TestFixture]
        public class CreateatesForMonthTest
        {
            [Test]
            public void InputIsInFebuary2013_ReturnsAllDatesForFebruary()
            {
                // Arrange
                var date = new DateTime(2013, 2, 15);

                // Act
                var results = date.CreateDatesForMonth().ToList();

                // Assert
                Assert.That(results.Count, Is.EqualTo(28));
                Assert.That(results.Any(x => x.Month != 2), Is.False);
                Assert.That(results.Any(x => x.Year != 2013), Is.False);
                Assert.That(results.Select(x => x.Day).Distinct().Count(), Is.EqualTo(28)); // Test that no double dates occurs
            }
        }

        [TestFixture]
        public class IsWeekendTest
        {
            [Test]
            public void DatesAreWeekend_ReturnTrue()
            {
                // Arrange
                var sat = new DateTime(2013, 1, 5);
                var sun = new DateTime(2013, 1, 6);                

                // Act

                // Assert
                Assert.That(sat.IsWeekend(), Is.True);
                Assert.That(sun.IsWeekend(), Is.True);
            }

            [Test]
            public void DatesAreNonWeekend_ReturnsFalse()
            {
                // Arrange
                var mon = new DateTime(2013, 1, 7);
                var tue = new DateTime(2013, 1, 8);
                var wed = new DateTime(2013, 1, 9);
                var thu = new DateTime(2013, 1, 10);
                var fri = new DateTime(2013, 1, 11);

                // Act

                // Assert
                Assert.That(mon.IsWeekend(), Is.False);
                Assert.That(tue.IsWeekend(), Is.False);
                Assert.That(wed.IsWeekend(), Is.False);
                Assert.That(thu.IsWeekend(), Is.False);
                Assert.That(fri.IsWeekend(), Is.False);
            }
        }

        [TestFixture]
        public class WeeknumberTest
        {
            [TestCase(2013, 1, 1, 1)]
            [TestCase(2013, 2, 1, 5)]
            [TestCase(2013, 3, 1, 9)]
            [TestCase(2013, 4, 1, 14)]
            public void CorrectWeekNumberByGivenDateIsReturned(int year, int month, int day, int expectedResult)
            {
                // Arrange
                var date = new DateTime(year, month, day);

                // Act
                var result = date.Weeknumber("da-DK");

                // Assert
                Assert.That(result, Is.EqualTo(expectedResult));
            }
        }
    }
}