namespace Task3.LogStandardizer.Parsing
{
    /// <summary>
    /// Приводит уровень логирования к допустимому значению.
    /// </summary>
    public static class LogLevelMapper
    {
        public static bool TryMap(string input, out string result)
        {
            result = input.ToUpperInvariant() switch
            {
                "INFORMATION" => "INFO",
                "INFO" => "INFO",
                "WARNING" => "WARN",
                "WARN" => "WARN",
                "ERROR" => "ERROR",
                "DEBUG" => "DEBUG",
                _ => string.Empty
            };

            // Возвращаем false, если уровень неизвестен
            return result.Length > 0;
        }
    }
} 