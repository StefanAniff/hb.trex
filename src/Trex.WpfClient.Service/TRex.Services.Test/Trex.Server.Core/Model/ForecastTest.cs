using System;
using Moq;
using NUnit.Framework;
using Ploeh.AutoFixture;
using TRex.Services.Test.Util;
using Trex.Server.Core.Model;
using Trex.Server.Core.Model.Forecast;

namespace TRex.Services.Test.Trex.Server.Core.Model
{
    [TestFixture]
    public class ForecastTest : AutoFixtureTestBase
    {
        [Test]
        public void AddProjectRegistration_AddingCompanyHoursAlreadyExists_ThrowsException()
        {
            // Arrange
            var fixture = InitializeFixture();

            var userMock = fixture.Create<Mock<User>>();
            var projectMock = fixture.Create<Mock<Project>>();
            projectMock.SetupGet(x => x.ProjectID).Returns(33);
            projectMock.SetupGet(x => x.ProjectName).Returns("Project X");

            var forecastTypeMock = fixture.Create<Mock<ForecastType>>();
            forecastTypeMock.SetupGet(x => x.SupportsProjectHours).Returns(true);

            var monthMock = fixture.Create<Mock<ForecastMonth>>();

            var forecast = new Forecast(new DateTime(2012, 1, 1), forecastTypeMock.Object, monthMock.Object); 
            forecast.AddProjectRegistration(projectMock.Object, 3m);            

            // Act
            var msg = Assert.Throws<Exception>(() => forecast.AddProjectRegistration(projectMock.Object, 1m)).Message;

            // Assert
            Assert.That(msg, Is.EqualTo("There can only be one ForecastProjectHours project: Project X pr ProjectForecast"));
            userMock.VerifyAll();
            projectMock.VerifyAll();
            forecastTypeMock.VerifyAll();
        }

        [Test]
        public void AddProjectRegistration_ForecastTypeDoesNotSupportClientHours_ThrowsException()
        {
            // Arrange
            var fixture = InitializeFixture();

            var userMock = fixture.Freeze<Mock<User>>();            
            var projectMock = fixture.Create<Mock<Project>>();
            projectMock.SetupGet(x => x.ProjectName).Returns("Project X");

            var forecastTypeMock = fixture.Freeze<Mock<ForecastType>>();
            forecastTypeMock.SetupGet(x => x.SupportsProjectHours).Returns(false);
            forecastTypeMock.SetupGet(x => x.Name).Returns("SomeType");

            var monthMock = fixture.Create<Mock<ForecastMonth>>();

            var forecast = new Forecast(new DateTime(2012, 1, 1), forecastTypeMock.Object, monthMock.Object);

            // Act
            var msg = Assert.Throws<Exception>(() => forecast.AddProjectRegistration(projectMock.Object, 1m)).Message;

            // Assert
            Assert.That(msg, Is.EqualTo("SomeType ForecastType on date: 01-01-2012 and project: Project X does not support project hours 1"));
            userMock.VerifyAll();
            projectMock.VerifyAll();
            forecastTypeMock.VerifyAll();
        }

        [Test]
        public void Constructor_ForecastTypeDoesNotSupportDedicatedHours_ThrowsException()
        {
            // Arrange
            var fixture = InitializeFixture();
            var forecastTypeMock = fixture.Create<Mock<ForecastType>>();
            forecastTypeMock.SetupGet(x => x.Name).Returns("MyType");
            forecastTypeMock.SetupGet(x => x.SupportsDedicatedHours).Returns(false);
            var userMock = fixture.Create<Mock<User>>();
            var monthMock = fixture.Create<Mock<ForecastMonth>>();

            // Act
            var msg = Assert.Throws<Exception>(() => new Forecast(new DateTime(2013, 1, 1), forecastTypeMock.Object, monthMock.Object, 3)).Message;

            // Assert
            Assert.That(msg, Is.EqualTo("MyType ForecastType on date 01-01-2013 does not support dedicated hours 3"));
            forecastTypeMock.VerifyAll();
        }
    }
}