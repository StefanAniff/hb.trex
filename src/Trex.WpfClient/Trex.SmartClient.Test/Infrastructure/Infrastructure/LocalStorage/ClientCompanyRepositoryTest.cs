using System.Collections.Generic;
using Moq;
using NUnit.Framework;
using Ploeh.AutoFixture;
using Trex.SmartClient.Data;
using Trex.SmartClient.Infrastructure.Implemented.LocalStorage;
using Trex.SmartClient.Test.TestUtil;

namespace Trex.SmartClient.Test.Infrastructure.Infrastructure.LocalStorage
{    
    public class ClientCompanyRepositoryTest
    {
        [TestFixture]
        public class GetByNameSearchStringTest : AutoFixtureTestBase
        {
            [Test]
            public void CollectionContainsThreeCompanies_ReturnsTwoCompanies()
            {
                // Arrange
                var fixture = InitializeFixture();

                var trackerDataSet = new TimeTrackerDataSet();
                var excludedCust1 = trackerDataSet.Customers.NewCustomersRow();
                excludedCust1.Id = 1;
                excludedCust1.Name = "abcd";
                var includedCust1 = trackerDataSet.Customers.NewCustomersRow();
                includedCust1.Id = 2;
                includedCust1.Name = "defg";
                var includedCust2 = trackerDataSet.Customers.NewCustomersRow();
                includedCust2.Id = 3;
                includedCust2.Name = "dEFg";
                var excludedCust2 = trackerDataSet.Customers.NewCustomersRow(); // Fullfills searchstring but is inactive, therefore exlcuded
                excludedCust2.Id = 3;
                excludedCust2.Name = "dEFghijk";
                excludedCust2.Inactive = true;

                var dataWrapper = fixture.Create<Mock<DataSetWrapper>>();
                dataWrapper
                    .SetupGet(x => x.Customers)
                    .Returns(new List<TimeTrackerDataSet.CustomersRow>
                        {
                            excludedCust1,
                            excludedCust2,
                            includedCust1,
                            includedCust2
                        });

                var repo = new ClientCompanyRepository(dataWrapper.Object);

                // Act
                var result = repo.GetByNameSearchString("efg");

                // Assert
                Assert.That(result.Count, Is.EqualTo(2));
                Assert.That(result[0].Name, Is.EqualTo("defg"));
                Assert.That(result[1].Name, Is.EqualTo("dEFg"));
            }
        }
    }
}