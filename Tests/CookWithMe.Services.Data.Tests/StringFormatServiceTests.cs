namespace CookWithMe.Services.Data.Tests
{
    using System.Collections.Generic;

    using Xunit;

    public class StringFormatServiceTests
    {
        [Fact]
        public void FormatTime_WithLessThan60Minutes_ShouldReturnCorrectResult()
        {
            var errorMessagePrefix = "StringFormatService FormatTime() method does not work properly.";

            // Arrange
            var stringFormatService = new StringFormatService();
            var minutes = 30;

            // Act
            var actualResult = stringFormatService.FormatTime(minutes);
            var expectedResult = " 30 min";

            // Assert
            Assert.True(expectedResult == actualResult, errorMessagePrefix + " " + "Formated time is not returned properly.");
        }

        [Fact]
        public void FormatTime_WithExactly60Minutes_ShouldReturnCorrectResult()
        {
            var errorMessagePrefix = "StringFormatService FormatTime() method does not work properly.";

            // Arrange
            var stringFormatService = new StringFormatService();
            var minutes = 60;

            // Act
            var actualResult = stringFormatService.FormatTime(minutes);
            var expectedResult = " 1 h";

            // Assert
            Assert.True(expectedResult == actualResult, errorMessagePrefix + " " + "Formated time is not returned properly.");
        }

        [Fact]
        public void FormatTime_WithMoreThan60Minutes_ShouldReturnCorrectResult()
        {
            var errorMessagePrefix = "StringFormatService FormatTime() method does not work properly.";

            // Arrange
            var stringFormatService = new StringFormatService();
            var minutes = 90;

            // Act
            var actualResult = stringFormatService.FormatTime(minutes);
            var expectedResult = " 1 h 30 min";

            // Assert
            Assert.True(expectedResult == actualResult, errorMessagePrefix + " " + "Formated time is not returned properly.");
        }

        [Fact]
        public void FormatTime_WithExactly120Minutes_ShouldReturnCorrectResult()
        {
            var errorMessagePrefix = "StringFormatService FormatTime() method does not work properly.";

            // Arrange
            var stringFormatService = new StringFormatService();
            var minutes = 120;

            // Act
            var actualResult = stringFormatService.FormatTime(minutes);
            var expectedResult = " 2 h";

            // Assert
            Assert.True(expectedResult == actualResult, errorMessagePrefix + " " + "Formated time is not returned properly.");
        }

        [Fact]
        public void SplitByCommaAndWhitespace_WithCorrectData_ShouldReturnCorrectResult()
        {
            var errorMessagePrefix = "StringFormatService SplitByCommaAndWhitespace() method does not work properly.";

            // Arrange
            var stringFormatService = new StringFormatService();
            var text = ", one,  ,two,,, three  ,";

            // Act
            var actualResult = stringFormatService.SplitByCommaAndWhitespace(text);
            var expectedResult = new List<string>() { "one", "two", "three" };

            // Assert
            for (int i = 0; i < actualResult.Count; i++)
            {
                Assert.True(expectedResult[i] == actualResult[i], errorMessagePrefix + " " + "Split text is not returned properly.");
            }
        }

        [Fact]
        public void SplitByCommaAndWhitespace_WithEmptyInput_ShouldReturnCorrectResult()
        {
            var errorMessagePrefix = "StringFormatService SplitByCommaAndWhitespace() method does not work properly.";

            // Arrange
            var stringFormatService = new StringFormatService();
            var text = string.Empty;

            // Act
            var actualResult = stringFormatService.SplitByCommaAndWhitespace(text).Count;
            var expectedResult = 0;

            // Assert
            Assert.True(expectedResult == actualResult, errorMessagePrefix + " " + "Collection is not empty.");
        }

        [Fact]
        public void SplitBySemicollon_WithCorrectData_ShouldReturnCorrectResult()
        {
            var errorMessagePrefix = "StringFormatService SplitBySemicollon() method does not work properly.";

            // Arrange
            var stringFormatService = new StringFormatService();
            var text = "; one two; ; ;;   three   four;  ; ;;";

            // Act
            var actualResult = stringFormatService.SplitBySemicollon(text);
            var expectedResult = new List<string>() { "one two", "three   four" };

            // Assert
            for (int i = 0; i < actualResult.Count; i++)
            {
                Assert.True(expectedResult[i] == actualResult[i], errorMessagePrefix + " " + "Split text is not returned properly.");
            }
        }

        [Fact]
        public void SplitBySemicollon_WithEmptyInput_ShouldReturnCorrectResult()
        {
            var errorMessagePrefix = "StringFormatService SplitBySemicollon() method does not work properly.";

            // Arrange
            var stringFormatService = new StringFormatService();
            var text = string.Empty;

            // Act
            var actualResult = stringFormatService.SplitBySemicollon(text).Count;
            var expectedResult = 0;

            // Assert
            Assert.True(expectedResult == actualResult, errorMessagePrefix + " " + "Collection is not empty.");
        }

        [Fact]
        public void RemoveWhitespaces_WithCorrectData_ShouldReturnCorrectResult()
        {
            var errorMessagePrefix = "StringFormatService RemoveWhitespaces() method does not work properly.";

            // Arrange
            var stringFormatService = new StringFormatService();
            var text = "  one two   three   ";

            // Act
            var actualResult = stringFormatService.RemoveWhitespaces(text);
            var expectedResult = "onetwothree";

            // Assert
            Assert.True(expectedResult == actualResult, errorMessagePrefix + " " + "Text is not returned properly.");
        }

        [Fact]
        public void RemoveWhitespaces_WithEmptyInput_ShouldReturnCorrectResult()
        {
            var errorMessagePrefix = "StringFormatService RemoveWhitespaces() method does not work properly.";

            // Arrange
            var stringFormatService = new StringFormatService();
            var text = string.Empty;

            // Act
            var actualResult = stringFormatService.RemoveWhitespaces(text);
            var expectedResult = string.Empty;

            // Assert
            Assert.True(expectedResult == actualResult, errorMessagePrefix + " " + "Text is not returned properly.");
        }
    }
}
