using NUnit.Framework;
using TrexSL.Web;

namespace TRex.Services.Test.Trex.Services
{
    [TestFixture]
    public class RegisterAutoMapperMappingsTest
    {
        [Test]
        public void RegisterTest()
        {
            // Arrange
            // Act
            RegisterAutoMapperMappings.Register();

            // Assert

        }    
    }
}