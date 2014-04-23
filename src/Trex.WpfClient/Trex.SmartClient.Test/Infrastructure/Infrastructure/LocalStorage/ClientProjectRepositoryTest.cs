using System.Collections.Generic;
using Moq;
using NUnit.Framework;
using Ploeh.AutoFixture;
using Trex.SmartClient.Core.Model;
using Trex.SmartClient.Core.Services;
using Trex.SmartClient.Data;
using Trex.SmartClient.Infrastructure.Implemented.LocalStorage;
using Trex.SmartClient.Test.TestUtil;

namespace Trex.SmartClient.Test.Infrastructure.Infrastructure.LocalStorage
{
    [TestFixture]
    public class ClientProjectRepositoryTest : AutoFixtureTestBase
    {
        [TestCase("X", 2)]
        [TestCase("x", 2)]
        [TestCase("Project", 1)]
        [TestCase("hj", 1)]
        [TestCase("cashcow", 1)]
        [TestCase("Project Z", 0)]
        public void GetBySearchStringTestCase(string searchString, int expectedCount)
        {
            // Arrange
            var fixture = InitializeFixture();

            var clientRepoMock = fixture.Create<Mock<ICompanyRepository>>();
            var datawrapperMock = CreateData(fixture, clientRepoMock);

            fixture.Inject(clientRepoMock.Object);
            fixture.Inject(datawrapperMock.Object);

            var projectRepo = fixture.Create<ClientProjectRepository>();

            // Act
            var result = projectRepo.GetBySearchString(searchString);

            // Assert
            clientRepoMock.VerifyAll();
            datawrapperMock.VerifyAll();
            Assert.That(result.Count, Is.EqualTo(expectedCount));
        }

        private Mock<DataSetWrapper> CreateData(IFixture fixture, Mock<ICompanyRepository> companyRepoMock)
        {
            var dataset = new TimeTrackerDataSet();
            var rows = new List<TimeTrackerDataSet.ProjectsRow>
                {
                    CreateProject(dataset, companyRepoMock, 1, "Project X", "Cashcow client", 10, false),
                    CreateProject(dataset, companyRepoMock, 2, "Project Z", "Cashcow", 11, true), // inactive
                    CreateProject(dataset, companyRepoMock, 3, "d60.dk X", "d60", 12, false),
                    CreateProject(dataset, companyRepoMock, 4, "asdf", "hjkl", 13, false),
                };

            var result = fixture.Create<Mock<DataSetWrapper>>();
            result
                .SetupGet(x => x.Projects)
                .Returns(rows);
            return result;
        }

        private TimeTrackerDataSet.ProjectsRow CreateProject(TimeTrackerDataSet dataset
            , Mock<ICompanyRepository> companyRepoMock
            , int projectId
            , string projectName
            , string clientName
            , int clientId
            , bool inactive)
        {
            var newProjectsRow = dataset.Projects.NewProjectsRow();
            newProjectsRow.Id = projectId;
            newProjectsRow.Name = projectName;
            newProjectsRow.CustomerId = clientId;
            newProjectsRow.Inactive = inactive;

            var newClientRow = dataset.Customers.NewCustomersRow();
            newClientRow.Id = clientId;
            newClientRow.Name = clientName;
            companyRepoMock.Setup(x => x.GetById(clientId)).Returns(Company.Create(clientName, clientId, true, false));

            return newProjectsRow;
        }
    }
}