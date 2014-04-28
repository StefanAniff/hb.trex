using NUnit.Framework;
using Trex.Common.DataTransferObjects;

namespace TRex.Services.Test.Trex.Common.DataTransferObjects
{
    [TestFixture]
    public class ForecastUserDtoTest
    {
        [Test]
        public void IsAllUsers_DtoIsAllUsersMarker_ReturnsTrue()
        {
            // Arrange
            var sut = ForecastUserDto.AllUsersDto();

            // Act
            var result = sut.IsAllUsers;

            // Assert
            Assert.That(result, Is.True);
        }

        [Test]
        public void IsAllUsers_DtoIsNotAllUsersMarker_ReturnsFalse()
        {
            // Arrange
            var sut = new ForecastUserDto
                {
                    UserId = 1,
                    UserName = "That guy"
                };

            // Act
            var result = sut.IsAllUsers;

            // Assert
            Assert.That(result, Is.False);
        }
    }
}