using System.Collections.Generic;
using System.IO;
using Task3.LogStandardizer.Formatting;
using Task3.LogStandardizer.Parsing;

namespace Task3.LogStandardizer.Processing
{
    /// <summary>
    /// Отвечает за обработку всего файла.
    /// </summary>
    public class LogFileProcessor
    {
        public LogProcessingResult Process(string inputFile, string outputFile, string problemsFile)
        {
            var validLines = new List<string>();
            var invalidLines = new List<string>();

            foreach (var line in File.ReadLines(inputFile))
            {
                if (LogParser.TryParse(line, out var entry) && entry != null)
                {
                    // Валидную запись форматируем
                    validLines.Add(LogEntryFormatter.Format(entry));
                }
                else
                {
                    // Невалидную сохраняем как есть
                    invalidLines.Add(line);
                }
            }

            File.WriteAllLines(outputFile, validLines);
            File.WriteAllLines(problemsFile, invalidLines);

            return new LogProcessingResult
            {
                SuccessCount = validLines.Count,
                ProblemCount = invalidLines.Count
            };
        }
    }

    /// <summary>
    /// Результат обработки файла.
    /// </summary>
    public class LogProcessingResult
    {
        public int SuccessCount { get; set; }
        public int ProblemCount { get; set; }
    }
}