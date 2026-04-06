using System;
using System.Globalization;
using System.Text.RegularExpressions;
using Task3.LogStandardizer.Models;

namespace Task3.LogStandardizer.Parsing
{
    /// <summary>
    /// Парсит строку лога и преобразует её в StandardLogEntry.
    /// </summary>
    public static class LogParser
    {
        // Формат 1:
        // 10.03.2025 15:14:49.523 INFORMATION Версия программы: '3.4.0.48729'
        private static readonly Regex Format1Regex = new Regex(
            @"^(?<date>\d{2}\.\d{2}\.\d{4})\s+(?<time>\d{2}:\d{2}:\d{2}\.\d+)\s+(?<level>INFORMATION|INFO|WARNING|WARN|ERROR|DEBUG)\s+(?<message>.+)$",
            RegexOptions.Compiled);

        // Формат 2:
        // 2025-03-10 15:14:51.5882| INFO|11|MobileComputer.GetDeviceId| Код устройства: '...'
        private static readonly Regex Format2Regex = new Regex(
            @"^(?<date>\d{4}-\d{2}-\d{2})\s+(?<time>\d{2}:\d{2}:\d{2}\.\d+)\|\s*(?<level>INFORMATION|INFO|WARNING|WARN|ERROR|DEBUG)\s*\|[^|]*\|(?<method>[^|]*)\|\s*(?<message>.+)$",
            RegexOptions.Compiled);

        public static bool TryParse(string? line, out StandardLogEntry? entry)
        {
            entry = null;

            if (string.IsNullOrWhiteSpace(line))
                return false;

            var match = Format1Regex.Match(line);
            if (match.Success)
                return TryParseFormat1(match, out entry);

            match = Format2Regex.Match(line);
            if (match.Success)
                return TryParseFormat2(match, out entry);

            return false;
        }

        private static bool TryParseFormat1(Match match, out StandardLogEntry? entry)
        {
            entry = null;

            string rawDate = match.Groups["date"].Value;
            string time = match.Groups["time"].Value;
            string rawLevel = match.Groups["level"].Value;
            string message = match.Groups["message"].Value;

            if (!TryConvertDate(rawDate, "dd.MM.yyyy", out string date))
                return false;

            if (!LogLevelMapper.TryMap(rawLevel, out string level))
                return false;

            entry = new StandardLogEntry
            {
                Date = date,
                Time = time,
                LogLevel = level,
                Method = "DEFAULT",
                Message = message
            };

            return true;
        }

        private static bool TryParseFormat2(Match match, out StandardLogEntry? entry)
        {
            entry = null;

            string rawDate = match.Groups["date"].Value;
            string time = match.Groups["time"].Value;
            string rawLevel = match.Groups["level"].Value;
            string method = match.Groups["method"].Value.Trim();
            string message = match.Groups["message"].Value;

            if (!TryConvertDate(rawDate, "yyyy-MM-dd", out string date))
                return false;

            if (!LogLevelMapper.TryMap(rawLevel, out string level))
                return false;

            if (string.IsNullOrWhiteSpace(method))
                method = "DEFAULT";

            entry = new StandardLogEntry
            {
                Date = date,
                Time = time,
                LogLevel = level,
                Method = method,
                Message = message
            };

            return true;
        }

        /// <summary>
        /// Преобразует дату в формат YYYY-MM-DD.
        /// </summary>
        private static bool TryConvertDate(string input, string format, out string result)
        {
            result = string.Empty;

            if (!DateTime.TryParseExact(
                    input,
                    format,
                    CultureInfo.InvariantCulture,
                    DateTimeStyles.None,
                    out DateTime date))
            {
                return false;
            }

            result = date.ToString("yyyy-MM-dd");
            return true;
        }
    }
}