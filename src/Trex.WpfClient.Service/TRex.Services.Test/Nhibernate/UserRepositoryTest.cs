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
using Trex.Server.Infrastructure.Implemented;

namespace TRex.Services.Test.Nhibernate
{
    [TestFixture]
    public class UserRepositoryTest 
    {
        private DataSession _dataSession;
        private ISession _session;  
        private User _user;

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
            var project = DataGenerator.GetProject(_user, customer);
            project = new GenericRepository<Project>(_session).SaveOrUpdate(project);

            //add project to user
            _user.Projects.Add(project);
            _user.AddCustomerInfo(new UserCustomerInfo(_user.UserID, customer.CustomerID));
             _user = plantRepository.SaveOrUpdate(_user);

        }

        public void BuildSchema(ISession session, Configuration configuration)
        {
            var export = new SchemaExport(configuration);
            StringBuilder sb = new StringBuilder();
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
            //_session.CreateSQLQuery("DELETE FROM " + ObjectNames.TableUser).ExecuteUpdate();
        }


        [Test]
        public void CanLoadUser()
        {
            var userRepository = new GenericRepository<User>(_session);
            var users = userRepository.GetAll();

            Assert.AreEqual(1, users.Count);
            var retrievedUser = users.Single();
            Assert.AreNotEqual(0, retrievedUser.UserID);
            Assert.AreEqual(_user.UserID, retrievedUser.UserID);
        }

        [Test]
        public void CanLoadUserCustomerInfo()
        {
            var userRepository = new GenericRepository<User>(_session);
            var users = userRepository.GetAll();

            Assert.AreEqual(1, users.Count);
            var retrievedUser = users.Single();
            Assert.AreEqual(1, retrievedUser.CustomerInfo.Count);
            Assert.AreEqual(_user.CustomerInfo.Single().PricePrHour, retrievedUser.CustomerInfo.Single().PricePrHour);
        }

        [Test]
        public void CanLoadUserProjects()
        {
            var userRepository = new GenericRepository<User>(_session);
            var users = userRepository.GetAll();

            Assert.AreEqual(1, users.Count);
            var retrievedUser = users.Single();
            Assert.AreEqual(1, retrievedUser.Projects.Count);
            Assert.AreEqual(_user.Projects.Single().CreateDate, retrievedUser.Projects.Single().CreateDate);
        }

        [Test]
        public void CanLoadUserBillableTimeFormula()
        {
            var userRepository = new GenericRepository<User>(_session);
            var users = userRepository.GetAll();

            Assert.AreEqual(1, users.Count);
            var retrievedUser = users.Single();
            Assert.AreEqual(0, retrievedUser.TotalBillableTime);
        }

        [Test]
        public void CanLoadUserTotalTimeForumla()
        {
            var userRepository = new GenericRepository<User>(_session);
            var users = userRepository.GetAll();

            Assert.AreEqual(1, users.Count);
            var retrievedUser = users.Single();
            Assert.AreEqual(0, retrievedUser.TotalTime);
        }

        [Test]
        public void CanLoadUserNumOfTimeEntriesFormula()
        {
            var userRepository = new GenericRepository<User>(_session);
            var users = userRepository.GetAll();

            Assert.AreEqual(1, users.Count);
            var retrievedUser = users.Single();
            Assert.AreEqual(0, retrievedUser.NumOfTimeEntries);
        }

        [Test]
        public void CanLoadActiveUsers()
        {
            // Arrange
            var sut = new UserRepository(_session);

            // Act
            var result = sut.GetActiveUsers();

            // Assert
            Assert.That(result.Any(x => x.Inactive), Is.False);
        }
    }
}
