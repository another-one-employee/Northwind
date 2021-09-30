using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Moq;
using Northwind.Application.Interfaces;
using Northwind.Web.Controllers;
using Northwind.Web.ViewModels.Account;
using NUnit.Framework;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

namespace Northwind.Web.UnitTests.Controllers
{
    public class AccountControllerTests
    {
        private Mocks _mocks;

        private AccountController _controller;

        [SetUp]
        public void SetUp()
        {
            _mocks = new();

            _mocks.UserManager
                .Setup(um => um.FindByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync(Mock.Of<IdentityUser>());

            _mocks.UserManager
                .Setup(um => um.CreateAsync(It.IsAny<IdentityUser>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success);

            _mocks.SignInManager
                .Setup(sm => sm.GetExternalLoginInfoAsync(It.IsAny<string>()))
                .ReturnsAsync(new Mock<ExternalLoginInfo>(Mock.Of<ClaimsPrincipal>(), null, null, null).Object);

            _mocks.SignInManager
                .Setup(sm => sm.ExternalLoginSignInAsync(It.IsAny<string>(), It.IsAny<string>(), false, false))
                .ReturnsAsync(SignInResult.Failed);

            _mocks.SignInManager
                .Setup(sm =>
                    sm.PasswordSignInAsync(
                        It.IsAny<string>(),
                        It.IsAny<string>(),
                        It.IsAny<bool>(),
                        It.Is<bool>(lof => !lof)))
                .ReturnsAsync(SignInResult.Success);

            _controller = new AccountController(_mocks.UserManager.Object, _mocks.SignInManager.Object, _mocks.EmailService.Object)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = new DefaultHttpContext()
                }
            };

            Mock<IUrlHelper> urlHelper = new();
            urlHelper
                .Setup(uh => uh.Action(It.IsAny<UrlActionContext>()))
                .Returns(string.Empty);

            urlHelper
                .Setup(uh => uh.IsLocalUrl(It.IsAny<string>()))
                .Returns(true);

            _controller.Url = urlHelper.Object;
        }

        [Test]
        public void Register_ReturnsViewResultIfHttpRequestIsGet()
        {
            // Act
            var result = _controller.Register();

            // Assert
            Assert.IsInstanceOf<ViewResult>(result);
        }

        [Test]
        public async Task Register_ReturnsRedirectToActionIfHttpRequestIsPost()
        {
            // Act
            var result = await _controller.Register(Mock.Of<RegisterViewModel>());

            // Assert
            Assert.IsInstanceOf<RedirectToActionResult>(result);
        }

        [Test]
        public async Task Register_ReturnsViewResultIfHttpRequestIsPostAndModelStateInvalid()
        {
            // Arrange
            _controller.ModelState.AddModelError(Data.ModelErrorKey, Data.ModelErrorMessage);

            // Act
            var result = await _controller.Register(Mock.Of<RegisterViewModel>());

            // Assert
            Assert.IsInstanceOf<ViewResult>(result);
        }

        [Test]
        public async Task Register_ReturnsViewResultIfHttpRequestIsPostAndIdentityResultFailed()
        {
            // Arrange
            _mocks.UserManager
                .Setup(um => um.CreateAsync(It.IsAny<IdentityUser>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Failed());

            // Act
            var result = await _controller.Register(Mock.Of<RegisterViewModel>());

            // Assert
            Assert.IsInstanceOf<ViewResult>(result);
        }

        [Test]
        public void Login_ReturnsViewResultIfHttpRequestIsGet()
        {
            // Act
            var result = _controller.Login();

            // Assert
            Assert.IsInstanceOf<ViewResult>(result);
        }

        [Test]
        public async Task Login_ReturnsRedirectToActionIfHttpRequestIsPost()
        {
            // Act
            var result = await _controller.Login(Mock.Of<LoginViewModel>());

            // Assert
            Assert.IsInstanceOf<RedirectToActionResult>(result);
        }

        [Test]
        public async Task Login_ReturnsRedirectResultIfHttpRequestIsPostAndModelReturnUrlNotEmpty()
        {
            // Act
            var result = await _controller.Login(Data.LoginViewModel);

            // Assert
            Assert.IsInstanceOf<RedirectResult>(result);
        }

        [Test]
        public async Task Logout_ReturnsRedirectToAction()
        {
            // Act
            var result = await _controller.Logout();

            // Assert
            Assert.IsInstanceOf<RedirectToActionResult>(result);
        }

        [Test]
        public void ForgotPassword_ReturnsViewResultIfHttpRequestIsGet()
        {
            // Act
            var result = _controller.ForgotPassword();

            // Assert
            Assert.IsInstanceOf<ViewResult>(result);
        }

        [Test]
        public async Task ForgotPassword_ReturnsViewResultIfHttpRequestIsPostAndModelInvalid()
        {
            // Arrange
            _controller.ModelState.AddModelError(Data.ModelErrorKey, Data.ModelErrorMessage);

            // Act
            var result = await _controller.ForgotPassword(Mock.Of<ForgotPasswordViewModel>());

            // Assert
            Assert.IsInstanceOf<ViewResult>(result);
            _mocks.UserManager.Verify(um => um.FindByEmailAsync(It.IsAny<string>()), Times.Never);
        }

        [Test]
        public async Task ForgotPassword_ReturnsViewResultIfHttpRequestIsPostAndModelValidButUserIsNull()
        {
            // Arrange
            _mocks.UserManager
                .Setup(um => um.FindByEmailAsync(It.IsAny<string>()));

            // Act
            var result = await _controller.ForgotPassword(Mock.Of<ForgotPasswordViewModel>());

            // Assert
            Assert.IsInstanceOf<ViewResult>(result);
            _mocks.UserManager.Verify(um => um.FindByEmailAsync(It.IsAny<string>()), Times.Once);
            _mocks.UserManager.Verify(um => um.GeneratePasswordResetTokenAsync(It.IsAny<IdentityUser>()), Times.Never);
        }

        [Test]
        public async Task ForgotPassword_ReturnsViewResultIfHttpRequestIsPostAndModelValid()
        {
            // Act
            var result = await _controller.ForgotPassword(Mock.Of<ForgotPasswordViewModel>());

            // Assert
            Assert.IsInstanceOf<ViewResult>(result);
            _mocks.UserManager.Verify(um => um.GeneratePasswordResetTokenAsync(It.IsAny<IdentityUser>()), Times.Once);
        }

        [Test]
        public void ResetPassword_ReturnsRedirectToActionIfHttpRequestIsGetAndCodeNull()
        {
            // Act
            var result = _controller.ResetPassword();

            // Assert
            Assert.IsInstanceOf<RedirectToActionResult>(result);
        }

        [Test]
        public void ResetPassword_ReturnsViewResultIfHttpRequestIsGet()
        {
            // Act
            var result = _controller.ResetPassword(Data.ResetPasswordCode);

            // Assert
            Assert.IsInstanceOf<ViewResult>(result);
        }

        [Test]
        public async Task ResetPassword_ReturnsViewResultIfHttpRequestIsPost()
        {
            // Arrange
            _mocks.UserManager
                .Setup(um => um.ResetPasswordAsync(It.IsAny<IdentityUser>(), It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success);

            // Act
            var result = await _controller.ResetPassword(Mock.Of<ResetPasswordViewModel>());

            // Assert
            Assert.IsInstanceOf<ViewResult>(result);
            _mocks.UserManager.Verify(um => um.ResetPasswordAsync(It.IsAny<IdentityUser>(), It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        }

        [Test]
        public async Task ResetPassword_ReturnsViewResultIfHttpRequestIsPostAndModelInvalid()
        {
            // Arrange
            _controller.ModelState.AddModelError(Data.ModelErrorKey, Data.ModelErrorMessage);

            // Act
            var result = await _controller.ResetPassword(Mock.Of<ResetPasswordViewModel>());

            // Assert
            Assert.IsInstanceOf<ViewResult>(result);
        }

        [Test]
        public async Task ResetPassword_ReturnsViewResultIfHttpRequestIsPostAndFindByEmailReturnsNull()
        {
            // Arrange
            _mocks.UserManager
                .Setup(um => um.FindByEmailAsync(It.IsAny<string>()));

            // Act
            var result = await _controller.ResetPassword(Mock.Of<ResetPasswordViewModel>());

            // Assert
            Assert.IsInstanceOf<ViewResult>(result);
            _mocks.UserManager.Verify(um => um.FindByEmailAsync(It.IsAny<string>()), Times.Once);
        }

        [Test]
        public async Task ResetPassword_ReturnsViewResultIfHttpRequestIsPostAndResetPasswordAsyncFailed()
        {
            // Arrange
            _mocks.UserManager
                .Setup(um => um.ResetPasswordAsync(It.IsAny<IdentityUser>(), It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Failed());

            // Act
            var result = await _controller.ResetPassword(Mock.Of<ResetPasswordViewModel>());

            // Assert
            Assert.IsInstanceOf<ViewResult>(result);
            _mocks.UserManager.Verify(um => um.ResetPasswordAsync(It.IsAny<IdentityUser>(), It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        }

        [Test]
        public void ExternalLogin_ReturnsChallengeResultIfHttpRequestIsGet()
        {
            // Act
            var result = _controller.ExternalLogin();

            // Assert
            Assert.IsInstanceOf<ChallengeResult>(result);
        }

        [Test]
        public async Task ExternalLoginCallback_ReturnsRedirectToActionResultIfHttpRequestIsPost()
        {
            // Arrange
            _mocks.SignInManager
                .Setup(sm => sm.ExternalLoginSignInAsync(It.IsAny<string>(), It.IsAny<string>(), false, false))
                .ReturnsAsync(SignInResult.Success);

            // Act
            var result = await _controller.ExternalLoginCallback();

            // Assert
            Assert.IsInstanceOf<RedirectToActionResult>(result);
            _mocks.SignInManager.Verify(um => um.SignInAsync(It.IsAny<IdentityUser>(), false, It.IsAny<string>()), Times.Never);
        }

        [Test]
        public async Task ExternalLoginCallback_ReturnsRedirectToActionResultIfHttpRequestIsPostAndSignInResultFailed()
        {
            // Act
            var result = await _controller.ExternalLoginCallback();

            // Assert
            Assert.IsInstanceOf<RedirectToActionResult>(result);
            var redirectToActionResult = result as RedirectToActionResult;
            Assert.IsNotNull(redirectToActionResult);
            Assert.AreEqual(nameof(AccountController.Login), redirectToActionResult.ActionName);
        }

        [Test]
        public async Task ExternalLoginCallback_ReturnsRedirectToActionResultIfHttpRequestIsPostAndSignInResultFailedWithNotEmptyIdentityName()
        {
            // Arrange
            Mock<IIdentity> iIdentity = new();
            iIdentity.SetupGet(i => i.Name).Returns(Data.IdentityName);

            Mock<ClaimsPrincipal> claimsPrincipal = new(iIdentity.Object);
            claimsPrincipal.SetupGet(cp => cp.Identity).Returns(iIdentity.Object);

            Mock<ExternalLoginInfo> externalLoginInfo = new(claimsPrincipal.Object, null, null, null);

            _mocks.SignInManager
                .Setup(sm => sm.GetExternalLoginInfoAsync(It.IsAny<string>()))
                .ReturnsAsync(externalLoginInfo.Object);

            _mocks.UserManager
                .Setup(um => um.CreateAsync(It.IsAny<IdentityUser>()))
                .ReturnsAsync(IdentityResult.Success);

            _mocks.UserManager
                .Setup(um => um.AddLoginAsync(It.IsAny<IdentityUser>(), externalLoginInfo.Object))
                .ReturnsAsync(IdentityResult.Success);

            // Act
            var result = await _controller.ExternalLoginCallback();

            // Assert
            Assert.IsInstanceOf<RedirectToActionResult>(result);
            _mocks.SignInManager.Verify(sm => sm.SignInAsync(It.IsAny<IdentityUser>(), false, It.IsAny<string>()), Times.Once);
        }

        private static class Data
        {
            public static string ModelErrorKey { get; } = "test";

            public static string ModelErrorMessage { get; } = "test model";

            public static string IdentityName { get; } = "test";

            public static string ResetPasswordCode { get; } = "43";

            public static LoginViewModel LoginViewModel { get; } = new()
            {
                Email = "test@mail.net", Password = "!Aa123", PasswordConfirm = "!Aa123", ReturnUrl = "/test"
            };
        }

        private class Mocks
        {
            private static Mock<UserManager<IdentityUser>> GetUserManagerMock() =>
                new(Mock.Of<IUserStore<IdentityUser>>(), null, null, null, null, null, null, null, null);

            public Mock<UserManager<IdentityUser>> UserManager { get; } = GetUserManagerMock();

            public Mock<SignInManager<IdentityUser>> SignInManager { get; } =
                new(GetUserManagerMock().Object, Mock.Of<IHttpContextAccessor>(), Mock.Of<IUserClaimsPrincipalFactory<IdentityUser>>(), null, null, null, null);

            public Mock<IEmailService> EmailService { get; } = new();
        }
    }
}
