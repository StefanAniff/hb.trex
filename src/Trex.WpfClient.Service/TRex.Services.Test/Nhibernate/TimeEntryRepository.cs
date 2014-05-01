using System;
using System.IO;
using System.Text;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;
using NUnit.Framework;
using Trex.Server.Core.Model;
using Trex.Server.Infrastructure.Implemented;
using Task = Trex.Server.Core.Model.Task;

namespace TRex.Services.Test.Nhibernate
{
    [TestFixture]
    public class TimeEntryRepositoryTest
    {
        private DataSession _dataSession;
        private ISession _session;
        private User _user;
        private Project _project;
        private Task _task;
        private TimeEntryType _timeEntryType;

        [TestFixtureSetUp]
        public void TestFixtureSetUp()
        {
            //Use In Memory database, open session and Build Schema inside In Memory database
            _dataSession = new DataSession(SQLiteConfiguration.Standard.InMemory());
            _session = _dataSession.SessionFactory.OpenSession();
            BuildSchema(_session, _dataSession.Configuration);

            //create user
            var plantRepository = new GenericRepository<User>(_session);
            _user = DataGenerator.GetUser();
            _user = plantRepository.SaveOrUpdate(_user);

            //create customer
            var customer = DataGenerator.GetCustomer(_user);
            customer = new GenericRepository<Company>(_session).SaveOrUpdate(customer);

            //Create project under customer
            _project = DataGenerator.GetProject(_user, customer);
            _project = new GenericRepository<Project>(_session).SaveOrUpdate(_project);

            //add project to user
            _user.Projects.Add(_project);
            _user = plantRepository.SaveOrUpdate(_user);

            //create task
            var taskRepository = new TaskRepository(_session);
            _task = taskRepository.SaveOrUpdate(DataGenerator.GetTask(_project, _user));

            //create timeEntryType
            var timeEntryTypeRepository = new TimeEntryTypeRepository(_session);
            _timeEntryType = timeEntryTypeRepository.SaveOrUpdate(DataGenerator.GetTimeEntryType(customer));            
        }

        public void BuildSchema(ISession session, Configuration configuration)
        {
            var export = new SchemaExport(configuration);
            var sb = new StringBuilder();
            TextWriter writer = new StringWriter(sb);
            export.Execute(true, true, false, session.Connection, writer);
            writer.Dispose(); //set breakpoint here to get schema
        }

        [TestFixtureTearDown]
        public void TestFixtureTearDown()
        {
            //Clean up after tests have run
            _session.Close();
            _dataSession.SessionFactory.Close();
        }

        [SetUp]
        public void SetUp()
        {
            _session.CreateSQLQuery("DELETE FROM " + ObjectNames.TableTask).ExecuteUpdate();

            // DocumentTypes
            _session.CreateSQLQuery("delete from DocumentType").ExecuteUpdate();
            _session.CreateSQLQuery("INSERT INTO [dbo].[DocumentType] ([Name]) VALUES('dummy')").ExecuteUpdate();
        }

        [Test]
        public void CanSaveNewTimeEntry()
        {
            var timeEntryRepository = new TimeEntryRepository(_session);
            var savedTimeEntry = timeEntryRepository.SaveOrUpdate(DataGenerator.GetTimeEntry(_task, _user, _timeEntryType));

            Assert.AreNotEqual(0, savedTimeEntry.Id);
            Assert.AreNotEqual(Guid.Empty, savedTimeEntry.Guid);
        }

        [Test]
        public void CanUpdateExistingTimeEntry()
        {
            var _timeEntryRepository = new TimeEntryRepository(_session);
            var savedTimeEntry = _timeEntryRepository.SaveOrUpdate(DataGenerator.GetTimeEntry(_task, _user, _timeEntryType));

            savedTimeEntry.TimeSpent = 1;
            var retrievedSavedTask = _timeEntryRepository.SaveOrUpdate(savedTimeEntry);


            Assert.AreEqual(savedTimeEntry.Guid, retrievedSavedTask.Guid);
            Assert.AreEqual(1, retrievedSavedTask.TimeSpent);
        }
    }
}
