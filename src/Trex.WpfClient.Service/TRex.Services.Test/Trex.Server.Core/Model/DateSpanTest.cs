using System;
using System.Globalization;
using NUnit.Framework;
using TRex.Services.Test.Util;
using Trex.Server.Core.Model.Forecast;

namespace TRex.Services.Test.Trex.Server.Core.Model
{
    [TestFixture]
    public class DateSpanTest : AutoFixtureTestBase
    {
        [TestCase(2010, "01-01-2010", "31-12-2010")]
        [TestCase(2011, "01-01-2011", "31-12-2011")]
        [TestCase(2012, "01-01-2012", "31-12-2012")]
        public void YearDateSpan(int yearInput, string expectedFrom, string expectedTo)
        {
            // Arrange

            // Act
            var result = DateSpan.YearDateSpan(yearInput);

            // Assert
            Assert.That(result.From, Is.EqualTo(ParseDkDateString(expectedFrom)));
            Assert.That(result.To, Is.EqualTo(ParseDkDateString(expectedTo)));
        }


        [TestCase("15-05-2013", "01-06-2013", "31-05-2014")]
        [TestCase("31-12-2010", "01-01-2011", "31-12-2011")]
        [TestCase("29-02-2012", "01-03-2012", "28-02-2013")] // Leap year
        public void Next12MonthsDatespan(string dateInput, string expectedFrom, string expectedTo)
        {
            // Arrange

            // Act
            var result = DateSpan.Next12MonthsDatespan(ParseDkDateString(dateInput));

            // Assert
            Assert.That(result.From, Is.EqualTo(ParseDkDateString(expectedFrom)));
            Assert.That(result.To, Is.EqualTo(ParseDkDateString(expectedTo)));
        }

        [TestCase("15-05-2013", "01-05-2013", "31-05-2013")]
        [TestCase("31-12-2010", "01-12-2010", "31-12-2010")]
        [TestCase("29-02-2012", "01-02-2012", "29-02-2012")] // Leap year
        public void CurrentMonthDateSpan(string dateInput, string expectedFrom, string expectedTo)
        {
            // Arrange

            // Act
            var result = DateSpan.CurrentMonthDateSpan(ParseDkDateString(dateInput));

            // Assert
            Assert.That(result.From, Is.EqualTo(ParseDkDateString(expectedFrom)));
            Assert.That(result.To, Is.EqualTo(ParseDkDateString(expectedTo)));
        }

        [TestCase("01-02-2013", "01-05-2012", "01-02-2013")]
        [TestCase("01-05-2013", "01-05-2013", "01-05-2013")]
        public void VacationPeriodUntilDateDateSpan(string nowInput, string expectedFrom, string expectedTo)
        {
            // Arrange

            // Act
            var result = DateSpan.VacationPeriodUntilDateDateSpan(ParseDkDateString(nowInput));

            // Assert
            Assert.That(result.From, Is.EqualTo(ParseDkDateString(expectedFrom)));
            Assert.That(result.To, Is.EqualTo(ParseDkDateString(expectedTo)));
        }

        [TestCase("01-02-2013", "01-05-2012", "30-4-2013")]
        [TestCase("01-05-2013", "01-05-2013", "30-4-2014")]
        public void VacationCurrentPeriodDateSpan(string nowInput, string expectedFrom, string expectedTo)
        {
            // Arrange

            // Act
            var result = DateSpan.VacationCurrentPeriodDateSpan(ParseDkDateString(nowInput));

            // Assert
            Assert.That(result.From, Is.EqualTo(ParseDkDateString(expectedFrom)));
            Assert.That(result.To, Is.EqualTo(ParseDkDateString(expectedTo)));
        }

        [TestCase("01-02-2013", "01-05-2013", "30-4-2014")]
        [TestCase("01-05-2013", "01-05-2014", "30-4-2015")]
        public void VacationNextPeriodDateSpan(string nowInput, string expectedFrom, string expectedTo)
        {
            // Arrange

            // Act
            var result = DateSpan.VacationNextPeriodDateSpan(ParseDkDateString(nowInput));

            // Assert
            Assert.That(result.From, Is.EqualTo(ParseDkDateString(expectedFrom)));
            Assert.That(result.To, Is.EqualTo(ParseDkDateString(expectedTo)));
        }
    }
}