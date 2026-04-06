namespace Task3.LogStandardizer.Models
{
    /// <summary>
    /// Унифицированная модель лог-записи после парсинга.
    /// </summary>
    public class StandardLogEntry
    {
        public string Date { get; set; } = string.Empty;   // YYYY-MM-DD
        public string Time { get; set; } = string.Empty;
        public string LogLevel { get; set; } = string.Empty;
        public string Method { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
    }
}