namespace CookWithMe.Services.Data.Tests
{
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
            var administratorServiceModel = new AdministratorServiceModel
            {
                Username = "administrator",
                FullName = "Admin Admin",
                Email = "admin@admin.com",
                Password = "123456",
            };

            // Act
            var result = await administratorService.RegisterAsync(administratorServiceModel);

            // Assert
            Assert.True(result, errorMessagePrefix + " " + "Returns false.");
        }

        //[Fact]
        //public async Task GetAllAsync_WithDummyData_ShouldReturnCorrectResult()
        //{
        //    string errorMessagePrefix = "AdministratorService RegisterAsync() method does not work properly.";

        //    // Arrange
        //    MapperInitializer.InitializeMapper();
        //    var context = ApplicationDbContextInMemoryFactory.InitializeContext();
        //    await this.SeedRoles(context);
        //    var userManager = this.GetUserManagerMock().Object;
        //    var userRepository = new EfDeletableEntityRepository<ApplicationUser>(context);
        //    var administratorService = new AdministratorService(userManager, userRepository);

        //    // Creating three users
        //    for (int i = 0; i < 3; i++)
        //    {
        //        await userManager.CreateAsync(
        //        new ApplicationUser
        //        {
        //            UserName = $"administrator{i}",
        //            FullName = $"Admin {i}",
        //            Email = $"admin{i}@admin.com",
        //        },
        //        password: $"administrator{i}");
        //    }

        //    var users = await userRepository.All().ToListAsync();

        //    // Adding the first two users to Administrator Role
        //    for (int i = 0; i < users.Count - 1; i++)
        //    {
        //        await userManager.AddToRoleAsync(users[i], GlobalConstants.AdministratorRoleName);
        //    }

        //    // Act
        //    var actualResult = (await administratorService.GetAllAsync()).Count();
        //    var expectedResult = users.Count - 1;

        //    // Assert
        //    Assert.True(expectedResult == actualResult, errorMessagePrefix + " " + "Collections count mismatch.");
        //}

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

        private async Task SeedRoles(ApplicationDbContext context)
        {
            await context.Roles.AddAsync(new ApplicationRole(GlobalConstants.AdministratorRoleName));
            await context.Roles.AddAsync(new ApplicationRole(GlobalConstants.UserRoleName));

            await context.SaveChangesAsync();
        }
    }
}
