using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Test;
using Trex.Server.Infrastructure.Implemented;
using Trex.ServiceContracts;

namespace Test_TaskService
{
    [TestFixture]
    class GetTaskById
    {
        #region Setup/Teardown
        [SetUp]
        public void Setup()
        {
            _databaseSetup = new DatabaseSetup();
            _databaseSetup.CleanDatabase();
            _databaseSetup.CreateStandardDatabase();
            _taskService = new TaskService(_databaseSetup.GetTrexConnection);
        }

        [TearDown]
        public void TearDown()
        {
            _databaseSetup = null;
            _taskService = null;
        }
        private DatabaseSetup _databaseSetup;
        private TaskService _taskService;
        #endregion

        [Test]
        public void ProjectWithId15_ProjectWithId15()
        {
            _databaseSetup.CreateTask(15, 1, 1, "01-01-2013", "new");
            var newTask = _taskService.GetTaskById(15, false, false, false);

            Assert.AreEqual(15, newTask.TaskID);
        }


    }

    [TestFixture]
    public class SearchTask
    {
        #region Setup/Teardown
        [SetUp]
        public void Setup()
        {
            _databaseSetup = new DatabaseSetup();
            _databaseSetup.CleanDatabase();
            _databaseSetup.CreateStandardDatabase();
            _taskService = new TaskService(_databaseSetup.GetTrexConnection);
        }

        [TearDown]
        public void TearDown()
        {
            _databaseSetup = null;
            _taskService = null;
        }
        private DatabaseSetup _databaseSetup;
        private TaskService _taskService;
        #endregion

        [Test]
        public void SearchStringTestTask_2TasksFound()
        {
            _databaseSetup.CreateTask(15, 1, 1, "01-01-2012", "TestTask");
            _databaseSetup.CreateTask(16, 1, 1, "01-01-2012", "new");
            _databaseSetup.CreateTask(17, 1, 1, "01-01-2012", "Old");
            _databaseSetup.CreateTask(18, 1, 1, "01-01-2012", "NotTheOneYouWant");
            _databaseSetup.CreateTask(19, 1, 1, "01-01-2012", "TæskTæsk");

            var list = _taskService.SearchTasks("task");

            Assert.AreEqual(2, list.Count);
        }

        [Test]
        public void SearchStringTestTask_3Tasksfound()
        {
            _databaseSetup.CreateTask(15, 1, 1, "01-01-2012", "TestTask");
            _databaseSetup.CreateTask(16, 1, 1, "01-01-2012", "new");
            _databaseSetup.CreateTask(17, 1, 1, "01-01-2012", "Old");
            _databaseSetup.CreateTask(18, 1, 1, "01-01-2012", "NotTheOneYouWant");
            _databaseSetup.CreateTask(19, 1, 1, "01-01-2012", "ThisIsAlsoATask");

            var list = _taskService.SearchTasks("task");

            Assert.AreEqual(3, list.Count);
        }
    }

    public class SaveTask
    {
        #region Setup/Teardown
        [SetUp]
        public void Setup()
        {
            _databaseSetup = new DatabaseSetup();
            _databaseSetup.CleanDatabase();
            _databaseSetup.CreateStandardDatabase();
            _taskService = new TaskService(_databaseSetup.GetTrexConnection);
        }

        [TearDown]
        public void TearDown()
        {
            _databaseSetup = null;
            _taskService = null;
        }
        private DatabaseSetup _databaseSetup;
        private TaskService _taskService;
        #endregion

        [Test]
        public void SaveNewTask()
        {
            var task = new Task
                           {
                               TaskName = "New Test Task",
                               Guid = new Guid(1, 2, 3, 4, 5, 6, 7, 8, 9, 0, 1),
                               CreatedBy = 1,
                               ProjectID = 1,
                               ChangeDate = null,
                               CreateDate = DateTime.Now.Date,
                               ModifyDate = DateTime.Now
                           };

            _taskService.SaveTask(task);

            var first = (from Tasks in _databaseSetup.GetTrexConnection.TrexEntityContext.Tasks
                         where Tasks.Guid == new Guid(1, 2, 3, 4, 5, 6, 7, 8, 9, 0, 1)
                         select Tasks).First();

            Assert.AreEqual("New Test Task", first.TaskName);
        }

        [Test]
        public void SaveChangesToExsitingTask()
        {

            var pr = (from tasks in _databaseSetup.GetTrexConnection.TrexEntityContext.Tasks
                      where tasks.ProjectID == 1
                      select tasks).First();

            pr.TaskName = "This is a change ";

            _taskService.SaveTask(pr);

            var prj = (from tasks in _databaseSetup.GetTrexConnection.TrexEntityContext.Tasks
                       where tasks.ProjectID == 1
                       select tasks).First();

            Assert.AreEqual("This is a change ", prj.TaskName);
        }
    }

}
