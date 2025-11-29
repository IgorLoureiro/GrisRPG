using System.Diagnostics.CodeAnalysis;
using GrisAPI.DTOs;
using GrisAPI.Helpers.Security;
using GrisAPI.Models;
using GrisAPI.Repositories.UserRepository;
using GrisAPI.Services.AuthenticationService;
using Moq;

namespace GrisAPITests.Services
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class AuthenticationServiceTests
    {
        private Mock<IUserRepository> _userRepositoryMock = null!;
        private AuthenticationService _authenticationService = null!;

        [SetUp]
        public void Setup()
        {
            _userRepositoryMock = new Mock<IUserRepository>();
            _authenticationService = new AuthenticationService(_userRepositoryMock.Object);
        }

        [Test]
        public async Task LoginRequest_ShouldReturnInvalid_WhenUserDoesNotExist()
        {
            //Arrange
            _userRepositoryMock.Setup(repo => repo.GetUserByUsernameAsync("nonexistent"))
                .ReturnsAsync((User?)null);

            //Act
            var result = await _authenticationService.LoginRequest(new LoginRequestDto
            {
                Username = "nonexistent",
                Password = "pw"
            });

            //Assert
            Assert.That(result.NameOrPasswordInvalid, Is.True);
            Assert.That(result.IsBlocked, Is.False);
            Assert.That(result.UserClaimsPrincipal, Is.Null);
        }

        [Test]
        public async Task LoginRequest_ShouldReturnBlocked_WhenUserIsBlocked()
        {
            //Arrange
            var user = new User
            {
                Name = "test",
                IsBlocked = true
            };

            _userRepositoryMock.Setup(repo => repo.GetUserByUsernameAsync("test"))
                .ReturnsAsync(user);

            //Act
            var result = await _authenticationService.LoginRequest(new LoginRequestDto
            {
                Username = "test",
                Password = "pw"
            });

            //Assert
            Assert.That(result.IsBlocked, Is.True);
            Assert.That(result.NameOrPasswordInvalid, Is.False);
            Assert.That(result.UserClaimsPrincipal, Is.Null);
        }

        [Test]
        public async Task LoginRequest_ShouldIncrementAttempts_WhenPasswordInvalid()
        {
            //Arrange
            var user = new User
            {
                Name = "test",
                PasswordHash = PasswordHasherHelper.HashPassword("password"),
                Attempts = 2
            };

            _userRepositoryMock.Setup(repo => repo.GetUserByUsernameAsync("test"))
                .ReturnsAsync(user);

            //Act
            var result = await _authenticationService.LoginRequest(new LoginRequestDto
            {
                Username = "test",
                Password = "wrong"
            });

            //Assert
            Assert.That(result.NameOrPasswordInvalid, Is.True);
            _userRepositoryMock.Verify(repo => repo.UpdateUserAsync(It.Is<User>(u => u.Attempts == 3)));
        }

        [Test]
        public async Task LoginRequest_ShouldBlockUser_WhenAttemptsReached()
        {
            //Arrange
            var user = new User
            {
                Name = "test",
                PasswordHash = "hashed",
                Attempts = 4
            };

            _userRepositoryMock.Setup(repo => repo.GetUserByUsernameAsync("test"))
                .ReturnsAsync(user);

            //Act
            var result = await _authenticationService.LoginRequest(new LoginRequestDto
            {
                Username = "test",
                Password = "wrong"
            });

            //Assert
            Assert.That(result.NameOrPasswordInvalid, Is.True);
            _userRepositoryMock.Verify(repo => repo.UpdateUserAsync(It.Is<User>(u => u.IsBlocked)));
        }

        [Test]
        public async Task LoginRequest_ShouldResetAttempts_AndReturnClaims_WhenSuccess()
        {
            //Arrange
            var user = new User
            {
                Id = 1,
                Name = "John",
                PasswordHash = PasswordHasherHelper.HashPassword("correct"),
                Attempts = 3
            };

            _userRepositoryMock.Setup(repo => repo.GetUserByUsernameAsync("test"))
                .ReturnsAsync(user);

            //Act
            var result = await _authenticationService.LoginRequest(new LoginRequestDto
            {
                Username = "test",
                Password = "correct"
            });

            //Assert
            Assert.That(result.UserClaimsPrincipal, Is.Not.Null);
            Assert.That(user.Attempts, Is.EqualTo(0));
            Assert.That(result.UserClaimsPrincipal!.Identity!.Name, Is.EqualTo("John"));
            _userRepositoryMock.Verify(repo => repo.UpdateUserAsync(user));
        }
    }
}