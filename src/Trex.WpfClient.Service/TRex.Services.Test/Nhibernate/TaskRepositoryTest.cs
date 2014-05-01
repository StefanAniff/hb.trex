using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;
using NUnit.Framework;
using Trex.Server.Core.Model;
using Trex.Server.Core.Services;
using Trex.Server.Infrastructure.Implemented;

namespace TRex.Services.Test.Nhibernate
{
    [TestFixture]
    public class TaskRepositoryTest
    {
        private DataSession _dataSession;
        private ISession _session;
        private User _user;
        private Project _project;

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
        }

        [Test]
        public void CanSaveNewTask()
        {
            var taskRepository = new TaskRepository(_session);
            var savedTask = taskRepository.SaveOrUpdate(DataGenerator.GetTask(_project, _user));

            Assert.AreNotEqual(0, savedTask.TaskID);
            Assert.AreNotEqual(Guid.Empty, savedTask.Guid);
        }

        [Test]
        public void CanUpdateExistingTask()
        {
            var _taskRepository = new TaskRepository(_session);
            var savedTask = _taskRepository.SaveOrUpdate(DataGenerator.GetTask(_project, _user));

            var retrievedTask = _taskRepository.GetById(savedTask.TaskID);
            retrievedTask.TimeLeft = 1;
            var retrievedSavedTask = _taskRepository.SaveOrUpdate(retrievedTask);


            Assert.AreEqual(retrievedTask.Guid, retrievedSavedTask.Guid);
            Assert.AreEqual(1, retrievedSavedTask.TimeLeft);
        }
    }
}
