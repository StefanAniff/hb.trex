using NUnit.Framework;
using TRex.Services.Test.Util;
using Trex.Server.Infrastructure.Implemented;

namespace TRex.Services.Test.Nhibernate
{
    [TestFixture]
    public class ProjectRepositoryTest : DbTest
    {
        [Test]
        public void GetByCustomerId_CanExecuteSql()
        {
            // Arrange  
            var sut = new ProjectRepository(Session);

            // Act            
            // Assert
            Assert.DoesNotThrow(() => sut.GetByCustomerId(1));
        }
    }
}