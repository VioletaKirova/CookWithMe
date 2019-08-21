namespace CookWithMe.Services.Data.Tests
{
    using System;

    using CookWithMe.Data.Models.Enums;

    using Moq;

    using Xunit;

    public class EnumParseServiceTests
    {
        [Fact]
        public void GetEnumDescription_WithCorrectData_ShouldReturnCorrectResult()
        {
            var errorMessagePrefix = "EnumParseService GetEnumDescription() method does not work properly.";

            // Arrange
            var stringFormatServiceMock = this.GetStringFormatServiceMock();
            var enumParseService = new EnumParseService(stringFormatServiceMock.Object);

            // Act
            var actualResult = enumParseService
                .GetEnumDescription(Period.ALaMinute.ToString(), typeof(Period));
            var expectedResult = "A La Minute";

            // Assert
            Assert.True(expectedResult == actualResult, errorMessagePrefix + " " + "Description is not returned properly.");
        }

        [Fact]
        public void GetEnumDescription_WithIncorrectData_ShouldReturnCorrectResult()
        {
            var errorMessagePrefix = "EnumParseService GetEnumDescription() method does not work properly.";

            // Arrange
            var stringFormatServiceMock = this.GetStringFormatServiceMock();
            var enumParseService = new EnumParseService(stringFormatServiceMock.Object);

            // Act
            string actualResult = enumParseService
                .GetEnumDescription("Incorrect", typeof(Period));
            string expectedResult = null;

            // Assert
            Assert.True(expectedResult == actualResult, errorMessagePrefix + " " + "Result is not null.");
        }

        [Fact]
        public void Parse_WithCorrectData_ShouldReturnCorrectResult()
        {
            var errorMessagePrefix = "EnumParseService Parse<TEnum>() method does not work properly.";

            // Arrange
            var stringFormatServiceMock = this.GetStringFormatServiceMock();
            var enumParseService = new EnumParseService(stringFormatServiceMock.Object);

            // Act
            var actualResult = enumParseService.Parse<Period>("A La Minute");
            var expectedResult = Period.ALaMinute;

            // Assert
            Assert.True(expectedResult == actualResult, errorMessagePrefix + " " + "Enum is not returned properly.");
        }

        [Fact]
        public void Parse_WithIncorrectData_ShouldThrowArgumentException()
        {
            // Arrange
            var stringFormatServiceMock = this.GetStringFormatServiceMock();
            var enumParseService = new EnumParseService(stringFormatServiceMock.Object);

            // Act

            // Assert
            Assert.Throws<ArgumentException>(() => enumParseService.Parse<Period>("Incorrect"));
        }

        private Mock<IStringFormatService> GetStringFormatServiceMock()
        {
            var stringFormatServiceMock = new Mock<IStringFormatService>();
            stringFormatServiceMock
                .Setup(x => x.RemoveWhitespaces(It.IsAny<string>()))
                .Returns((string text) => text.Replace(" ", string.Empty));

            return stringFormatServiceMock;
        }
    }
}
