using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Northwind.Infrastructure.Services;
using NUnit.Framework;
using System;
using System.ComponentModel.Design;

namespace Northwind.Infrastructure.UnitTests.Services
{
    public class AdminInitializerTests
    {
        static Mocks _mocks;

        [SetUp]
        public void SetUp()
        {
            _mocks = new();

            Mock<IServiceScope> serviceScope = new();
            Mock<IServiceScopeFactory> serviceScopeFactory = new();

            _mocks.ServiceProvider
                .Setup(sp => sp.GetService(typeof(UserManager<IdentityUser>)))
                .Returns(_mocks.UserManager.Object);

            _mocks.ServiceProvider
                .Setup(sp => sp.GetService(typeof(RoleManager<IdentityRole>)))
                .Returns(_mocks.RoleManager.Object);

            serviceScope
                .SetupGet(ss => ss.ServiceProvider)
                .Returns(_mocks.ServiceProvider.Object);

            serviceScopeFactory
                .Setup(ssf => ssf.CreateScope())
                .Returns(serviceScope.Object);

            ServiceContainer serviceContainer = new();
            serviceContainer.AddService(typeof(IServiceScope), serviceScope.Object);
            serviceContainer.AddService(typeof(IServiceScopeFactory), serviceScopeFactory.Object);

            _mocks.AppBuilder
                .Setup(ab => ab.ApplicationServices)
                .Returns(serviceContainer);

            Mock<IConfigurationSection> adminInitializerConfigSection = new();
            Mock<IConfigurationSection> emailConfigSection = new();
            Mock<IConfigurationSection> passConfigSection = new();

            emailConfigSection.SetupProperty(cs => cs.Value, Data.EmailSection);
            passConfigSection.SetupProperty(cs => cs.Value, Data.PasswordSection);

            adminInitializerConfigSection.Setup(cs => cs.GetSection(Data.EmailSection)).Returns(emailConfigSection.Object);
            adminInitializerConfigSection.Setup(cs => cs.GetSection(Data.PasswordSection)).Returns(passConfigSection.Object);

            _mocks.Configuration.Setup(cs => cs.GetSection(nameof(AdminInitializer))).Returns(adminInitializerConfigSection.Object);
        }

        [Test]
        public void InitializeAdminAsync_DoNothingIfServicesNull()
        {
            // Arrange
            _mocks.ServiceProvider
                .Setup(sp => sp.GetService(typeof(UserManager<IdentityUser>)));

            _mocks.ServiceProvider
                .Setup(sp => sp.GetService(typeof(RoleManager<IdentityRole>)));

            // Act
            _mocks.AppBuilder.Object.InitializeAdminAsync(_mocks.Configuration.Object);

            // Assert
            _mocks.UserManager.Verify(um => um.CreateAsync(It.IsAny<IdentityUser>(), It.IsAny<string>()), Times.Never);
            _mocks.RoleManager.Verify(rm => rm.CreateAsync(It.IsAny<IdentityRole>()), Times.Never);
        }

        [Test]
        public void InitializeAdminAsync_CreateNewRoleIfFindByNameAsyncNull()
        {
            // Arrange
            _mocks.ServiceProvider
                .Setup(sp => sp.GetService(typeof(UserManager<IdentityUser>)));

            // Act
            _mocks.AppBuilder.Object.InitializeAdminAsync(_mocks.Configuration.Object);

            // Assert
            _mocks.UserManager.Verify(um => um.CreateAsync(It.IsAny<IdentityUser>(), It.IsAny<string>()), Times.Never);
            _mocks.RoleManager.Verify(rm => rm.CreateAsync(It.IsAny<IdentityRole>()), Times.Once);
        }

        [Test]
        public void InitializeAdminAsync_CreateNewUserIfFindByNameAsyncNull()
        {
            // Arrange
            _mocks.ServiceProvider
                .Setup(sp => sp.GetService(typeof(RoleManager<IdentityRole>)));

            _mocks.UserManager
                .Setup(um => um.CreateAsync(It.IsAny<IdentityUser>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success);

            // Act
            _mocks.AppBuilder.Object.InitializeAdminAsync(_mocks.Configuration.Object);

            // Assert
            _mocks.UserManager.Verify(um => um.CreateAsync(It.IsAny<IdentityUser>(), It.IsAny<string>()), Times.Once);
            _mocks.UserManager.Verify(um => um.AddToRoleAsync(It.IsAny<IdentityUser>(), It.IsAny<string>()), Times.Once);
            _mocks.RoleManager.Verify(rm => rm.CreateAsync(It.IsAny<IdentityRole>()), Times.Never);
        }

        [Test]
        public void InitializeAdminAsync_DoNothingIfRoleExist()
        {
            // Arrange
            _mocks.ServiceProvider
                .Setup(sp => sp.GetService(typeof(UserManager<IdentityUser>)));

            _mocks.RoleManager.Setup(rm => rm.FindByNameAsync(It.IsAny<string>())).ReturnsAsync(Mock.Of<IdentityRole>());

            // Act
            _mocks.AppBuilder.Object.InitializeAdminAsync(_mocks.Configuration.Object);

            // Assert
            _mocks.UserManager.Verify(um => um.CreateAsync(It.IsAny<IdentityUser>(), It.IsAny<string>()), Times.Never);
            _mocks.RoleManager.Verify(rm => rm.CreateAsync(It.IsAny<IdentityRole>()), Times.Never);
        }

        [Test]
        public void InitializeAdminAsync_DoNothingIfUserExist()
        {
            // Arrange
            _mocks.ServiceProvider
                .Setup(sp => sp.GetService(typeof(RoleManager<IdentityRole>)));

            _mocks.UserManager.Setup(um => um.FindByNameAsync(It.IsAny<string>())).ReturnsAsync(Mock.Of<IdentityUser>());

            // Act
            _mocks.AppBuilder.Object.InitializeAdminAsync(_mocks.Configuration.Object);

            // Assert
            _mocks.UserManager.Verify(um => um.CreateAsync(It.IsAny<IdentityUser>(), It.IsAny<string>()), Times.Never);
            _mocks.RoleManager.Verify(rm => rm.CreateAsync(It.IsAny<IdentityRole>()), Times.Never);
        }

        private static class Data
        {
            public static string EmailSection { get; } = "Email";

            public static string PasswordSection { get; } = "Password";
        }

        private class Mocks
        {
            public Mock<IApplicationBuilder> AppBuilder { get; } = new();

            public Mock<IConfiguration> Configuration { get; } = new();

            public Mock<UserManager<IdentityUser>> UserManager { get; } = 
                new(Mock.Of<IUserStore<IdentityUser>>(), null, null, null, null, null, null, null, null);

            public Mock<RoleManager<IdentityRole>> RoleManager { get; } =
                new(Mock.Of<IRoleStore<IdentityRole>>(), null, null, null, null);

            public Mock<IServiceProvider> ServiceProvider { get; } = new();
        }
    }
}
