using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Northwind.Web.Controllers;
using NUnit.Framework;

namespace Northwind.Web.UnitTests.Controllers
{
    public class AdministratorControllerTests
    {
        [Test]
        public void Index_ReturnsViewResult()
        {
            // Arrange
            var userStore = new Mock<IUserStore<IdentityUser>>();
            var userManager = new Mock<UserManager<IdentityUser>>(
                userStore.Object, null, null, null, null, null, null, null, null);

            var controller = new AdministratorController(userManager.Object);

            // Act
            var result = controller.Index();

            // Assert
            Assert.IsInstanceOf<ViewResult>(result);
        }
    }
}
