using Task3.LogStandardizer.Formatting;
using Task3.LogStandardizer.Models;
using Xunit;

namespace Task3.LogStandardizer.Tests.Formatting
{
    public class LogEntryFormatterTests
    {
        // Проверяем, что строка формируется строго с табами
        [Fact]
        public void Format_ShouldReturnCorrectString()
        {
            var entry = new StandardLogEntry
            {
                Date = "2025-03-10",
                Time = "15:14:49.523",
                LogLevel = "INFO",
                Method = "DEFAULT",
                Message = "Test message"
            };

            string result = LogEntryFormatter.Format(entry);

            Assert.Equal("2025-03-10\t15:14:49.523\tINFO\tDEFAULT\tTest message", result);
        }
    }
}