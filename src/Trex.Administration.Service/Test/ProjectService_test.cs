using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Test;
using Trex.Server.Infrastructure.Implemented;
using Trex.ServiceContracts;

namespace Test_ProjectService
{
    [TestFixture]
    class GetProjectById
    {       
        #region Setup/Teardown
        [SetUp]
        public void Setup()
        {
            _databaseSetup = new DatabaseSetup();
            _databaseSetup.CleanDatabase();
            _databaseSetup.CreateStandardDatabase();
            _projectService = new ProjectService(_databaseSetup.GetTrexConnection);
        }

        [TearDown]
        public void TearDown()
        {
            _databaseSetup = null;
            _projectService = null;
        }        
        private DatabaseSetup _databaseSetup;
        private ProjectService _projectService;
        #endregion

        [Test]
        public void ProjectWithId15_ProjectWithId15()
        {
            _databaseSetup.CreateProject(15, 1, "new proj", 1, "01-01-2013", false, null, 1);
            var newProj = _projectService.GetProjectById(15, false, false, false, false);

            Assert.AreEqual(15, newProj.ProjectID);
        }

        [Test]
        public void SaveNewProject()
        {
            var p = new Project();
            p.ProjectName = "New Test Project";
            p.Guid = new Guid(1, 2, 3, 4, 5, 6, 7, 8, 9, 0, 1);
            p.FixedPriceProject = false;
            p.ChangeDate = null;
            p.CustomerInvoiceGroupID = 1;
            p.CustomerID = 1;
            p.CreateDate = DateTime.Now.Date;

            _projectService.SaveProject(p);

            var pr = (from project in _databaseSetup.GetTrexConnection.TrexEntityContext.Projects
                      where project.Guid == new Guid(1, 2, 3, 4, 5, 6, 7, 8, 9, 0, 1)
                      select project).First();

            Assert.AreEqual("New Test Project", pr.ProjectName);
        }

        [Test]
        public void SaveChangesToExsitingProject()
        {

            var pr = (from project in _databaseSetup.GetTrexConnection.TrexEntityContext.Projects
                      where project.ProjectID == 1
                      select project).First();

            pr.ProjectName = "This is a change ";

            _projectService.SaveProject(pr);

            var prj = (from project1 in _databaseSetup.GetTrexConnection.TrexEntityContext.Projects
                       where project1.ProjectID == 1
                       select project1).First();

            Assert.AreEqual("This is a change ", prj.ProjectName);
        }

    }

    public class SaveProject
    {
        #region Setup/Teardown
        [SetUp]
        public void Setup()
        {
            _databaseSetup = new DatabaseSetup();
            _databaseSetup.CleanDatabase();
            _databaseSetup.CreateStandardDatabase();
            _projectService = new ProjectService(_databaseSetup.GetTrexConnection);
        }

        [TearDown]
        public void TearDown()
        {
            _databaseSetup = null;
            _projectService = null;
        }
        private DatabaseSetup _databaseSetup;
        private ProjectService _projectService;
        #endregion
        [Test]
        public void SaveNewProject()
        {
            var p = new Project();
            p.ProjectName = "New Test Project";
            p.Guid = new Guid(1, 2, 3, 4, 5, 6, 7, 8, 9, 0, 1);
            p.FixedPriceProject = false;
            p.ChangeDate = null;
            p.CustomerInvoiceGroupID = 1;
            p.CustomerID = 1;
            p.CreateDate = DateTime.Now.Date;

            _projectService.SaveProject(p);

            var pr = (from project in _databaseSetup.GetTrexConnection.TrexEntityContext.Projects
                      where project.Guid == new Guid(1, 2, 3, 4, 5, 6, 7, 8, 9, 0, 1)
                      select project).First();

            Assert.AreEqual("New Test Project", pr.ProjectName);
        }

        [Test]
        public void SaveChangesToExsitingProject()
        {

            var pr = (from project in _databaseSetup.GetTrexConnection.TrexEntityContext.Projects
                      where project.ProjectID == 1
                      select project).First();

            pr.ProjectName = "This is a change ";

            _projectService.SaveProject(pr);

            var prj = (from project1 in _databaseSetup.GetTrexConnection.TrexEntityContext.Projects
                       where project1.ProjectID == 1
                       select project1).First();

            Assert.AreEqual("This is a change ", prj.ProjectName);
        }
    }
}
