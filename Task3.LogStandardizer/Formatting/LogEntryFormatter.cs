using Task3.LogStandardizer.Models;

namespace Task3.LogStandardizer.Formatting
{
    /// <summary>
    /// Формирует строку выходного формата.
    /// </summary>
    public static class LogEntryFormatter
    {
        public static string Format(StandardLogEntry entry)
        {
            // Разделитель — табуляция
            return $"{entry.Date}\t{entry.Time}\t{entry.LogLevel}\t{entry.Method}\t{entry.Message}";
        }
    }
}