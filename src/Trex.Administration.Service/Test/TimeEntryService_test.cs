using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Test;
using Trex.Server.Infrastructure.Implemented;
using Trex.ServiceContracts;

namespace Test_TimeEntryService
{
    [TestFixture]
    class GetAllTimeEntryTypes
    {
        #region Setup/Teardown
        [SetUp]
        public void Setup()
        {
            _databaseSetup = new DatabaseSetup();
            _databaseSetup.CleanDatabase();
            _databaseSetup.CreateStandardDatabase();
            _timeEntryService = new TimeEntryService(_databaseSetup.GetTrexConnection);
        }

        [TearDown]
        public void TearDown()
        {
            _databaseSetup = null;
            _timeEntryService = null;
        }
        private DatabaseSetup _databaseSetup;
        private TimeEntryService _timeEntryService;
        #endregion

        [Test]
        public void Two_TimeEntryTypes()
        {
            var types = _timeEntryService.GetAllTimeEntryTypes();

            Assert.AreEqual(2, types.Count);
        }

    }

    [TestFixture]
    class GetTimeEntriesByPeriodAndUser
    {
        #region Setup/Teardown
        [SetUp]
        public void Setup()
        {
            _databaseSetup = new DatabaseSetup();
            _databaseSetup.CleanDatabase();
            _databaseSetup.CreateStandardDatabase();
            _timeEntryService = new TimeEntryService(_databaseSetup.GetTrexConnection);
        }

        [TearDown]
        public void TearDown()
        {
            _databaseSetup = null;
            _timeEntryService = null;
        }
        private DatabaseSetup _databaseSetup;
        private TimeEntryService _timeEntryService;
        #endregion

        [Test]
        public void FourTimeEntries_ReturnsListWithTreeTimeentries()
        {
            var newUser = _databaseSetup.CreateUser(14, "MiniMe", 1337);
            _databaseSetup.CreateTimeEntry(14, null, 10, 100, true, 10, 0, "01-01-2013", "01-01-2013", 14, 1, 1);
            _databaseSetup.CreateTimeEntry(15, null, 10, 100, true, 10, 0, "01-01-2013", "02-01-2013", 14, 1, 1);
            _databaseSetup.CreateTimeEntry(16, null, 10, 100, true, 10, 0, "01-01-2013", "03-01-2013", 14, 1, 1);
            _databaseSetup.CreateTimeEntry(17, null, 10, 100, true, 10, 0, "01-01-2013", "04-01-2013", 14, 1, 1);

            var timeEntrylist = _timeEntryService.GetTimeEntriesByPeriodAndUser(newUser, new DateTime(2013, 1, 1), new DateTime(2013, 1, 3));

            Assert.AreEqual(3, timeEntrylist.Count);
        }

        [Test]
        public void FourTimeEntries_ReturnsListWithTwoTimeentries()
        {
            var newUser = _databaseSetup.CreateUser(14, "MiniMe", 1337);
            _databaseSetup.CreateTimeEntry(14, null, 10, 100, true, 10, 0, "01-01-2013", "01-01-2013", 14, 1, 1);
            _databaseSetup.CreateTimeEntry(15, null, 10, 100, true, 10, 0, "01-01-2013", "02-01-2013", 1, 1, 1);
            _databaseSetup.CreateTimeEntry(16, null, 10, 100, true, 10, 0, "01-01-2013", "03-01-2013", 14, 1, 1);
            _databaseSetup.CreateTimeEntry(17, null, 10, 100, true, 10, 0, "01-01-2013", "04-01-2013", 14, 1, 1);

            var timeEntrylist = _timeEntryService.GetTimeEntriesByPeriodAndUser(newUser, new DateTime(2013, 1, 1), new DateTime(2013, 1, 3));

            Assert.AreEqual(2, timeEntrylist.Count);
        }

    }

    [TestFixture]
    public class GetTimeEntriesByPeriod
    {
        #region Setup/Teardown
        [SetUp]
        public void Setup()
        {
            _databaseSetup = new DatabaseSetup();
            _databaseSetup.CleanDatabase();
            _databaseSetup.CreateStandardDatabase();
            _timeEntryService = new TimeEntryService(_databaseSetup.GetTrexConnection);
        }

        [TearDown]
        public void TearDown()
        {
            _databaseSetup = null;
            _timeEntryService = null;
        }
        private DatabaseSetup _databaseSetup;
        private TimeEntryService _timeEntryService;
        #endregion

        [Test]
        public void FourTimeEntries_Return3TimeEntries()
        {
            _databaseSetup.CreateTimeEntry(14, null, 10, 100, true, 10, 0, "01-01-2013", "01-01-2013", 1, 1, 1);
            _databaseSetup.CreateTimeEntry(15, null, 10, 100, true, 10, 0, "01-01-2013", "02-01-2013", 1, 1, 1);
            _databaseSetup.CreateTimeEntry(16, null, 10, 100, true, 10, 0, "01-01-2013", "03-01-2013", 1, 1, 1);
            _databaseSetup.CreateTimeEntry(17, null, 10, 100, true, 10, 0, "01-01-2013", "04-01-2013", 1, 1, 1);

            var timeentryList = _timeEntryService.GetTimeEntriesByPeriod(new DateTime(2013, 1, 1), new DateTime(2013, 1, 3));

            Assert.AreEqual(3, timeentryList.Count);
        }

    }

    [TestFixture]
    public class UpdateTimeEntryPrice
    {
        #region Setup/Teardown
        [SetUp]
        public void Setup()
        {
            _databaseSetup = new DatabaseSetup();
            _databaseSetup.CleanDatabase();
            _databaseSetup.CreateStandardDatabase();
            _timeEntryService = new TimeEntryService(_databaseSetup.GetTrexConnection);
        }

        [TearDown]
        public void TearDown()
        {
            _databaseSetup = null;
            _timeEntryService = null;
        }
        private DatabaseSetup _databaseSetup;
        private TimeEntryService _timeEntryService;
        #endregion

        [Test]
        public void TestAfUpdate()
        {
            _databaseSetup.CreateProject(15, 1, "test", 1, "01-01-2013", true, 100000, 1);
            _databaseSetup.CreateTask(15, 15, 1, "01-01-2012", "test");

            _databaseSetup.CreateTimeEntry(15, null, 100, 100, true, 100, 0, "01-01-2012", "01-01-2012", 1, 15, 1);
            _databaseSetup.CreateTimeEntry(16, null, 100, 100, true, 100, 0, "01-01-2012", "01-01-2012", 1, 15, 1);
            _databaseSetup.CreateTimeEntry(17, null, 100, 100, true, 100, 0, "01-01-2012", "01-01-2012", 1, 15, 1);
            _databaseSetup.CreateTimeEntry(18, null, 100, 100, true, 100, 0, "01-01-2012", "01-01-2012", 1, 15, 1);
            _databaseSetup.CreateTimeEntry(19, null, 100, 100, true, 100, 0, "01-01-2012", "01-01-2012", 1, 15, 1);

            var timeEntriesold = (from timeEntry in _databaseSetup.GetTrexConnection.TrexEntityContext.TimeEntries
                                  where
                                      timeEntry.TimeEntryID == 15 || timeEntry.TimeEntryID == 16 ||
                                      timeEntry.TimeEntryID == 17 || timeEntry.TimeEntryID == 18 ||
                                      timeEntry.TimeEntryID == 19
                                  select timeEntry);

            foreach (var timeEntry in timeEntriesold)
            {
                Assert.AreEqual(100, timeEntry.Price);
            }

            _timeEntryService.UpdateTimeEntryPrice(15);

            var timeEntriesNew = (from timeEntry in _databaseSetup.GetTrexConnection.TrexEntityContext.TimeEntries
                                  where
                                      timeEntry.TimeEntryID == 15 || timeEntry.TimeEntryID == 16 ||
                                      timeEntry.TimeEntryID == 17 || timeEntry.TimeEntryID == 18 ||
                                      timeEntry.TimeEntryID == 19
                                  select timeEntry);

            foreach (var timeEntry in timeEntriesNew)
            {
                Assert.AreEqual(200, timeEntry.Price);
            }
        }
    }

    [TestFixture]
    public class SaveTimeEntry
    {
        #region Setup/Teardown
        [SetUp]
        public void Setup()
        {
            _databaseSetup = new DatabaseSetup();
            _databaseSetup.CleanDatabase();
            _databaseSetup.CreateStandardDatabase();
            _timeEntryService = new TimeEntryService(_databaseSetup.GetTrexConnection);
        }

        [TearDown]
        public void TearDown()
        {
            _databaseSetup = null;
            _timeEntryService = null;
        }
        private DatabaseSetup _databaseSetup;
        private TimeEntryService _timeEntryService;
        #endregion

        [Test]
        public void SaveNewTimeEntry()
        {
            var time = new TimeEntry
                           {
                               CreateDate = DateTime.Now,
                               StartTime = DateTime.Now,
                               EndTime = DateTime.Now,
                               UserID = 1,
                               Description = "New Time Entry",
                               Price = 88888888,
                               DocumentType = 1,
                               TimeEntryTypeId = 1,
                               TaskID = 1
                           };

            _timeEntryService.SaveTimeEntry(time);

            var entry = (from timeEntry in _databaseSetup.GetTrexConnection.TrexEntityContext.TimeEntries
                         where timeEntry.Price == 88888888
                         select timeEntry).First();

            Assert.AreEqual("New Time Entry", entry.Description);
        }

        [Test]
        public void SaveChangesToExsitingTimeEntry()
        {
            var time = (from timeEntry in _databaseSetup.GetTrexConnection.TrexEntityContext.TimeEntries
                        where timeEntry.TimeEntryID == 1
                        select timeEntry).First();

            time.Description = "This is a change";

            _timeEntryService.SaveTimeEntry(time);

            var entry = (from timeEntry1 in _databaseSetup.GetTrexConnection.TrexEntityContext.TimeEntries
                         where timeEntry1.TimeEntryID == 1
                         select timeEntry1).First();

            Assert.AreEqual("This is a change", entry.Description);
        }
    }

}
