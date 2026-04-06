using Task3.LogStandardizer.Parsing;
using Xunit;

namespace Task3.LogStandardizer.Tests.Parsing
{
    public class LogLevelMapperTests
    {
        // Проверяем корректное преобразование допустимых значений
        [Theory]
        [InlineData("INFORMATION", "INFO")]
        [InlineData("INFO", "INFO")]
        [InlineData("WARNING", "WARN")]
        [InlineData("WARN", "WARN")]
        [InlineData("ERROR", "ERROR")]
        [InlineData("DEBUG", "DEBUG")]
        public void TryMap_ShouldReturnMappedValue_ForValidLevel(string input, string expected)
        {
            bool success = LogLevelMapper.TryMap(input, out string result);

            Assert.True(success);
            Assert.Equal(expected, result);
        }

        // Уровень логирования должен обрабатываться независимо от регистра
        [Theory]
        [InlineData("information")]
        [InlineData("warning")]
        [InlineData("debug")]
        public void TryMap_ShouldHandleCaseInsensitiveInput(string input)
        {
            bool success = LogLevelMapper.TryMap(input, out string result);

            Assert.True(success);
            Assert.NotEmpty(result);
        }

        // Недопустимые значения должны считаться ошибкой
        [Theory]
        [InlineData("WTF")]
        [InlineData("")]
        [InlineData("TRACE")]
        public void TryMap_ShouldReturnFalse_ForInvalidLevel(string input)
        {
            bool success = LogLevelMapper.TryMap(input, out string result);

            Assert.False(success);
            Assert.Equal(string.Empty, result);
        }
    }
}