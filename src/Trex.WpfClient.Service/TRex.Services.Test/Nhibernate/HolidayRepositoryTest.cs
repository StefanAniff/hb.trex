using System;
using System.Linq;
using NUnit.Framework;
using TRex.Services.Test.Util;
using Trex.Server.Core.Model;
using Trex.Server.Infrastructure.Implemented;

namespace TRex.Services.Test.Nhibernate
{
    [TestFixture]
    public class HolidayRepositoryTest : DbTest
    {
        [Test]
        public void GetByMonth_TwoItemsInEachMonth_ReturnsTwoItemsFromRequestedMonth()
        {
            // Arrange
            var repo = new HolidayRepository(Session);
            repo.SaveOrUpdate(new Holiday(new DateTime(2013, 1, 1), "Holiday jan"));
            repo.SaveOrUpdate(new Holiday(new DateTime(2013, 1, 2), "Holiday jan 2"));
            repo.SaveOrUpdate(new Holiday(new DateTime(2013, 2, 1), "Holiday feb"));
            repo.SaveOrUpdate(new Holiday(new DateTime(2013, 2, 2), "Holiday feb 2"));
            repo.SaveOrUpdate(new Holiday(new DateTime(2013, 3, 1), "Holiday march"));
            repo.SaveOrUpdate(new Holiday(new DateTime(2013, 3, 2), "Holiday march 2"));

            // Act
            var result = repo.GetByMonth(2, 2013).ToList();

            // Assert
            Assert.That(result.Count, Is.EqualTo(2));
            AssertHoliday(result[0], new DateTime(2013, 2, 1), "Holiday feb");
            AssertHoliday(result[1], new DateTime(2013, 2, 2), "Holiday feb 2");   
        }

        private void AssertHoliday(Holiday holiday, DateTime expectedDate, string expectedDescription)
        {
            Assert.That(holiday.Date, Is.EqualTo(expectedDate));
            Assert.That(holiday.Description, Is.EqualTo(expectedDescription));
        }
    }
}