using Task3.LogStandardizer.Parsing;
using Xunit;

namespace Task3.LogStandardizer.Tests.Parsing
{
    public class LogParserTests
    {
        // Проверка парсинга строки первого формата
        [Fact]
        public void TryParse_ShouldParse_Format1_Correctly()
        {
            string line = "10.03.2025 15:14:49.523 INFORMATION Версия программы: '3.4.0.48729'";

            bool success = LogParser.TryParse(line, out var entry);

            Assert.True(success);
            Assert.NotNull(entry);
            Assert.Equal("2025-03-10", entry!.Date);
            Assert.Equal("15:14:49.523", entry.Time);
            Assert.Equal("INFO", entry.LogLevel);
            Assert.Equal("DEFAULT", entry.Method);
        }

        // Проверка парсинга строки второго формата
        [Fact]
        public void TryParse_ShouldParse_Format2_Correctly()
        {
            string line = "2025-03-10 15:14:51.5882| INFO|11|MobileComputer.GetDeviceId| Код устройства: '@MINDEO'";

            bool success = LogParser.TryParse(line, out var entry);

            Assert.True(success);
            Assert.NotNull(entry);
            Assert.Equal("MobileComputer.GetDeviceId", entry!.Method);
        }

        // Если метод пустой — должен подставляться DEFAULT
        [Fact]
        public void TryParse_ShouldSetDefaultMethod_WhenMethodIsEmpty()
        {
            string line = "2025-03-10 15:14:51.5882| INFO|11|| Сообщение";

            bool success = LogParser.TryParse(line, out var entry);

            Assert.True(success);
            Assert.Equal("DEFAULT", entry!.Method);
        }

        // Неизвестный уровень логирования — строка должна считаться невалидной
        [Fact]
        public void TryParse_ShouldReturnFalse_ForUnknownLogLevel()
        {
            string line = "10.03.2025 15:14:49.523 WTF Сообщение";

            bool success = LogParser.TryParse(line, out var entry);

            Assert.False(success);
            Assert.Null(entry);
        }

        // Невалидная дата должна приводить к отказу парсинга
        [Fact]
        public void TryParse_ShouldReturnFalse_ForInvalidDate()
        {
            string line = "99.99.2025 15:14:49.523 INFORMATION Сообщение";

            bool success = LogParser.TryParse(line, out var entry);

            Assert.False(success);
        }

        // Пустая строка — невалидна
        [Fact]
        public void TryParse_ShouldReturnFalse_ForEmptyLine()
        {
            bool success = LogParser.TryParse("", out var entry);

            Assert.False(success);
            Assert.Null(entry);
        }

        // null — невалидный вход
        [Fact]
        public void TryParse_ShouldReturnFalse_ForNull()
        {
            bool success = LogParser.TryParse(null, out var entry);

            Assert.False(success);
            Assert.Null(entry);
        }

        // Полностью мусорная строка
        [Fact]
        public void TryParse_ShouldReturnFalse_ForInvalidLine()
        {
            string line = "ЭТО НЕ ЛОГ";

            bool success = LogParser.TryParse(line, out var entry);

            Assert.False(success);
        }
    }
}