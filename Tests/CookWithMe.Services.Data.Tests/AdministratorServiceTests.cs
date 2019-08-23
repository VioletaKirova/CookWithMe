namespace CookWithMe.Services.Data.Tests
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using CookWithMe.Data.Models;
    using CookWithMe.Data.Repositories;
    using CookWithMe.Services.Data.Administrators;
    using CookWithMe.Services.Data.Tests.Common;
    using CookWithMe.Services.Models.Administrators;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.Extensions.Configuration;

    using Moq;

    using Xunit;

    public class AdministratorServiceTests
    {
        private readonly IConfiguration configuration;

        public AdministratorServiceTests()
        {
            this.configuration = this.GetConfigurationBuilder().Build();
        }

        [Fact]
        public async Task RegisterAsync_WithSuccessfullActions_ShouldReturnCorrectResult()
        {
            var errorMessagePrefix = "AdministratorService RegisterAsync() method does not work properly.";

            // Arrange
            MapperInitializer.InitializeMapper();
            var context = ApplicationDbContextInMemoryFactory.InitializeContext();
            var userManager = this.GetUserManagerMock();
            userManager
                .Setup(x => x.CreateAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success);
            userManager
                .Setup(x => x.AddToRoleAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success);
            var userRepository = new EfDeletableEntityRepository<ApplicationUser>(context);
            var administratorService = new AdministratorService(this.configuration, userManager.Object, userRepository);
            var administratorServiceModel = new AdministratorServiceModel();

            // Act
            var result = await administratorService.RegisterAsync(administratorServiceModel);

            // Assert
            Assert.True(result, errorMessagePrefix + " " + "Returns false.");
        }

        [Fact]
        public async Task RegisterAsync_WithFailedCreateAsync_ShouldReturnCorrectResult()
        {
            var errorMessagePrefix = "AdministratorService RegisterAsync() method does not work properly.";

            // Arrange
            MapperInitializer.InitializeMapper();
            var context = ApplicationDbContextInMemoryFactory.InitializeContext();
            var userManager = this.GetUserManagerMock();
            userManager
                .Setup(x => x.CreateAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Failed());
            userManager
                .Setup(x => x.AddToRoleAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success);
            var userRepository = new EfDeletableEntityRepository<ApplicationUser>(context);
            var administratorService = new AdministratorService(this.configuration, userManager.Object, userRepository);
            var administratorServiceModel = new AdministratorServiceModel();

            // Act
            var result = await administratorService.RegisterAsync(administratorServiceModel);

            // Assert
            Assert.False(result, errorMessagePrefix + " " + "Returns true.");
        }

        [Fact]
        public async Task RegisterAsync_WithFailedAddToRoleAsync_ShouldReturnCorrectResult()
        {
            var errorMessagePrefix = "AdministratorService RegisterAsync() method does not work properly.";

            // Arrange
            MapperInitializer.InitializeMapper();
            var context = ApplicationDbContextInMemoryFactory.InitializeContext();
            var userManager = this.GetUserManagerMock();
            userManager
                .Setup(x => x.CreateAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success);
            userManager
                .Setup(x => x.AddToRoleAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Failed());
            var userRepository = new EfDeletableEntityRepository<ApplicationUser>(context);
            var administratorService = new AdministratorService(this.configuration, userManager.Object, userRepository);
            var administratorServiceModel = new AdministratorServiceModel();

            // Act
            var result = await administratorService.RegisterAsync(administratorServiceModel);

            // Assert
            Assert.False(result, errorMessagePrefix + " " + "Returns true.");
        }

        [Fact]
        public async Task RemoveFromRoleByIdAsync_WithExistentUserIdAndSuccessfullActions_ShouldReturnCorrectResult()
        {
            var errorMessagePrefix = "AdministratorService RemoveFromRoleByIdAsync() method does not work properly.";

            // Arrange
            MapperInitializer.InitializeMapper();
            var context = ApplicationDbContextInMemoryFactory.InitializeContext();
            var userManager = this.GetUserManagerMock();
            userManager
                .Setup(x => x.RemoveFromRoleAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success);
            userManager
                .Setup(x => x.AddToRoleAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success);
            var userRepository = new EfDeletableEntityRepository<ApplicationUser>(context);
            var administratorService = new AdministratorService(this.configuration, userManager.Object, userRepository);
            await userRepository.AddAsync(new ApplicationUser());
            await userRepository.SaveChangesAsync();
            var user = userRepository.All().First();

            // Act
            var result = await administratorService.RemoveFromRoleByIdAsync(user.Id);

            // Assert
            Assert.True(result, errorMessagePrefix + " " + "Returns false.");
        }

        [Fact]
        public async Task RemoveFromRoleByIdAsync_WithExistentUserIdAndFailedRemoveFromRoleAsync_ShouldReturnCorrectResult()
        {
            var errorMessagePrefix = "AdministratorService RemoveFromRoleByIdAsync() method does not work properly.";

            // Arrange
            MapperInitializer.InitializeMapper();
            var context = ApplicationDbContextInMemoryFactory.InitializeContext();
            var userManager = this.GetUserManagerMock();
            userManager
                .Setup(x => x.RemoveFromRoleAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Failed());
            userManager
                .Setup(x => x.AddToRoleAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success);
            var userRepository = new EfDeletableEntityRepository<ApplicationUser>(context);
            var administratorService = new AdministratorService(this.configuration, userManager.Object, userRepository);
            await userRepository.AddAsync(new ApplicationUser());
            await userRepository.SaveChangesAsync();
            var user = userRepository.All().First();

            // Act
            var result = await administratorService.RemoveFromRoleByIdAsync(user.Id);

            // Assert
            Assert.False(result, errorMessagePrefix + " " + "Returns true.");
        }

        [Fact]
        public async Task RemoveFromRoleByIdAsync_WithExistentUserIdAndFailedAddToRoleAsync_ShouldReturnCorrectResult()
        {
            var errorMessagePrefix = "AdministratorService RemoveFromRoleByIdAsync() method does not work properly.";

            // Arrange
            MapperInitializer.InitializeMapper();
            var context = ApplicationDbContextInMemoryFactory.InitializeContext();
            var userManager = this.GetUserManagerMock();
            userManager
                .Setup(x => x.RemoveFromRoleAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success);
            userManager
                .Setup(x => x.AddToRoleAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Failed());
            var userRepository = new EfDeletableEntityRepository<ApplicationUser>(context);
            var administratorService = new AdministratorService(this.configuration, userManager.Object, userRepository);
            await userRepository.AddAsync(new ApplicationUser());
            await userRepository.SaveChangesAsync();
            var user = userRepository.All().First();

            // Act
            var result = await administratorService.RemoveFromRoleByIdAsync(user.Id);

            // Assert
            Assert.False(result, errorMessagePrefix + " " + "Returns true.");
        }

        [Fact]
        public async Task RemoveFromRoleByIdAsync_WithNonExistentUserIdAndSuccessfullActions_ShouldThrowArgumentNullException()
        {
            // Arrange
            MapperInitializer.InitializeMapper();
            var context = ApplicationDbContextInMemoryFactory.InitializeContext();
            var userManager = this.GetUserManagerMock();
            userManager
                .Setup(x => x.RemoveFromRoleAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success);
            userManager
                .Setup(x => x.AddToRoleAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success);
            var userRepository = new EfDeletableEntityRepository<ApplicationUser>(context);
            var administratorService = new AdministratorService(this.configuration, userManager.Object, userRepository);
            var nonExistentUserId = Guid.NewGuid().ToString();

            // Act

            // Assert
            await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                await administratorService.RemoveFromRoleByIdAsync(nonExistentUserId);
            });
        }

        private Mock<UserManager<ApplicationUser>> GetUserManagerMock()
        {
            var userStoreMock = new Mock<IUserStore<ApplicationUser>>();
            var userManagerMock = new Mock<UserManager<ApplicationUser>>(
                userStoreMock.Object, null, null, null, null, null, null, null, null);

            return userManagerMock;
        }

        private IConfigurationBuilder GetConfigurationBuilder()
        {
            IConfigurationBuilder configurationBuilder = new ConfigurationBuilder();
            configurationBuilder.AddJsonFile("appsettings.json");
            return configurationBuilder;
        }
    }
}
