using System;
using Moq;
using NUnit.Framework;
using Ploeh.AutoFixture;
using TRex.Services.Test.Util;
using Trex.Server.Core.Model;
using Trex.Server.Core.Services;
using Trex.Server.Infrastructure.Implemented;
using System.Linq;

namespace TRex.Services.Test.Trex.Server.Infrastructure.Implemented
{
    [TestFixture]
    public class DomainSettingsTest : AutoFixtureTestBase
    {
        [Test]
        public void VacationForecastTypeId_ExtractsStringValueAndRetunsInt()
        {
            // Arrange
            var domSettingMock = Fixture.Create<Mock<DomainSetting>>();
            domSettingMock.SetupGet(x => x.Value).Returns("99");

            var domSettingRepoMock = FreezeMock<IDomainSettingRepository>();
            domSettingRepoMock.Setup(x => x.GetByName("VacationForecastTypeIdInt")).Returns(domSettingMock.Object);

            var sut = Fixture.Create<DomainSettings>();

            // Act

            var result = sut.VacationForecastTypeId;

            // Assert
            domSettingMock.VerifyAll();
            domSettingRepoMock.VerifyAll();
            Assert.That(result, Is.EqualTo(99));
        }

        [Test]
        public void TryExtractInt_ThrowsExceptionStringIsNotConvertableToint()
        {
            // Arrange
            
            // Act
            var exp = Assert.Throws<Exception>(() => DomainSettings.TryExtractInt("i'm words", "SomeNameId"));

            // Assert
            Assert.That(exp.Message, Is.EqualTo("SomeNameId in DomainSettings is missing or has an invalid value! String value: i'm words"));
        }

        [Test]
        public void GetSettingByName_ThrowsExceptionWhenSettingNotFound()
        {
            // Arrange
            var domSettingRepoMock = FreezeMock<IDomainSettingRepository>();
            domSettingRepoMock.Setup(x => x.GetByName(It.IsAny<string>())).Returns((DomainSetting) null);

            var sut = Fixture.Create<DomainSettings>();

            // Act
            var exp = Assert.Throws<Exception>(() => sut.GetSettingByName("someName"));

            // Assert
            domSettingRepoMock.VerifyAll();
            Assert.That(exp.Message, Is.EqualTo("The domainsetting with name someName is missing"));
        }
    }
}