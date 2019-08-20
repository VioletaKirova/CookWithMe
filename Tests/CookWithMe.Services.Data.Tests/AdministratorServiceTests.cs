namespace CookWithMe.Services.Data.Tests
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using CookWithMe.Common;
    using CookWithMe.Data;
    using CookWithMe.Data.Models;
    using CookWithMe.Data.Repositories;
    using CookWithMe.Services.Data.Administrators;
    using CookWithMe.Services.Data.Tests.Common;
    using CookWithMe.Services.Models.Administrators;

    using Microsoft.AspNetCore.Identity;

    using Moq;

    using Xunit;

    public class AdministratorServiceTests
    {
        [Fact]
        public async Task RegisterAsync_WithDummyData_ShouldReturnCorrectResult()
        {
            string errorMessagePrefix = "AdministratorService RegisterAsync() method does not work properly.";

            // Arrange
            MapperInitializer.InitializeMapper();
            var context = ApplicationDbContextInMemoryFactory.InitializeContext();
            await this.SeedRoles(context);
            var userManager = this.GetUserManagerMock().Object;
            var userRepository = new EfDeletableEntityRepository<ApplicationUser>(context);
            var administratorService = new AdministratorService(userManager, userRepository);
            var administratorServiceModel = new AdministratorServiceModel();

            // Act
            var result = await administratorService.RegisterAsync(administratorServiceModel);

            // Assert
            Assert.True(result, errorMessagePrefix + " " + "Returns false.");
        }

        [Fact]
        public async Task RemoveFromRoleByIdAsync_WithExistentUserId_ShouldReturnCorrectResult()
        {
            string errorMessagePrefix = "AdministratorService RemoveFromRoleByIdAsync() method does not work properly.";

            // Arrange
            MapperInitializer.InitializeMapper();
            var context = ApplicationDbContextInMemoryFactory.InitializeContext();
            await this.SeedRoles(context);
            var userManager = this.GetUserManagerMock().Object;
            var userRepository = new EfDeletableEntityRepository<ApplicationUser>(context);
            var administratorService = new AdministratorService(userManager, userRepository);
            await userRepository.AddAsync(new ApplicationUser());
            await userRepository.SaveChangesAsync();
            var user = userRepository.All().First();

            // Act
            var result = await administratorService.RemoveFromRoleByIdAsync(user.Id);

            // Assert
            Assert.True(result, errorMessagePrefix + " " + "Returns false.");
        }

        [Fact]
        public async Task RemoveFromRoleByIdAsync_WithNonExistentUserId_ShouldThrowArgumentNullException()
        {
            // Arrange
            MapperInitializer.InitializeMapper();
            var context = ApplicationDbContextInMemoryFactory.InitializeContext();
            var userManager = this.GetUserManagerMock().Object;
            var userRepository = new EfDeletableEntityRepository<ApplicationUser>(context);
            var administratorService = new AdministratorService(userManager, userRepository);
            var nonExistentUserId = Guid.NewGuid().ToString();

            // Act

            // Assert
            await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                await administratorService.RemoveFromRoleByIdAsync(nonExistentUserId);
            });
        }

        private async Task SeedRoles(ApplicationDbContext context)
        {
            await context.Roles.AddAsync(new ApplicationRole(GlobalConstants.AdministratorRoleName));
            await context.Roles.AddAsync(new ApplicationRole(GlobalConstants.UserRoleName));

            await context.SaveChangesAsync();
        }

        private Mock<UserManager<ApplicationUser>> GetUserManagerMock()
        {
            var userStoreMock = new Mock<IUserStore<ApplicationUser>>();
            var userManagerMock = new Mock<UserManager<ApplicationUser>>(
                userStoreMock.Object, null, null, null, null, null, null, null, null);

            userManagerMock
                .Setup(x => x.CreateAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success);
            userManagerMock
                .Setup(x => x.UpdateAsync(It.IsAny<ApplicationUser>()))
                .ReturnsAsync(IdentityResult.Success);
            userManagerMock
                .Setup(x => x.AddToRoleAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success);
            userManagerMock
                .Setup(x => x.RemoveFromRoleAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success);

            return userManagerMock;
        }
    }
}
