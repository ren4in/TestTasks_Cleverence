using System;
using System.IO;
using Task3.LogStandardizer.Processing;
using Xunit;

namespace Task3.LogStandardizer.Tests.Processing
{
    public class LogFileProcessorTests
    {
        // Проверяем смешанный сценарий: часть строк валидна, часть нет
        [Fact]
        public void Process_ShouldSeparateValidAndInvalidLines()
        {
            string dir = CreateTempDirectory();

            try
            {
                string input = Path.Combine(dir, "input.txt");
                string output = Path.Combine(dir, "output.txt");
                string problems = Path.Combine(dir, "problems.txt");

                File.WriteAllLines(input, new[]
                {
                    "10.03.2025 15:14:49.523 INFORMATION Версия программы",
                    "НЕВАЛИДНАЯ СТРОКА"
                });

                var processor = new LogFileProcessor();
                var result = processor.Process(input, output, problems);

                string[] outputLines = File.ReadAllLines(output);
                string[] problemLines = File.ReadAllLines(problems);

                Assert.Equal(1, result.SuccessCount);
                Assert.Equal(1, result.ProblemCount);

                Assert.Single(outputLines);
                Assert.Single(problemLines);
                Assert.Equal("НЕВАЛИДНАЯ СТРОКА", problemLines[0]);
            }
            finally
            {
                Directory.Delete(dir, true);
            }
        }

        // Если все строки валидные, problems.txt должен остаться пустым
        [Fact]
        public void Process_ShouldWriteOnlyOutput_WhenAllLinesAreValid()
        {
            string dir = CreateTempDirectory();

            try
            {
                string input = Path.Combine(dir, "input.txt");
                string output = Path.Combine(dir, "output.txt");
                string problems = Path.Combine(dir, "problems.txt");

                File.WriteAllLines(input, new[]
                {
                    "10.03.2025 15:14:49.523 INFORMATION Версия программы",
                    "2025-03-10 15:14:51.5882| INFO|11|MobileComputer.GetDeviceId| Код устройства"
                });

                var processor = new LogFileProcessor();
                var result = processor.Process(input, output, problems);

                string[] outputLines = File.ReadAllLines(output);
                string[] problemLines = File.ReadAllLines(problems);

                Assert.Equal(2, result.SuccessCount);
                Assert.Equal(0, result.ProblemCount);

                Assert.Equal(2, outputLines.Length);
                Assert.Empty(problemLines);
            }
            finally
            {
                Directory.Delete(dir, true);
            }
        }

        // Если все строки невалидные, output.txt должен остаться пустым
        [Fact]
        public void Process_ShouldWriteOnlyProblems_WhenAllLinesAreInvalid()
        {
            string dir = CreateTempDirectory();

            try
            {
                string input = Path.Combine(dir, "input.txt");
                string output = Path.Combine(dir, "output.txt");
                string problems = Path.Combine(dir, "problems.txt");

                File.WriteAllLines(input, new[]
                {
                    "НЕ ЛОГ 1",
                    "НЕ ЛОГ 2"
                });

                var processor = new LogFileProcessor();
                var result = processor.Process(input, output, problems);

                string[] outputLines = File.ReadAllLines(output);
                string[] problemLines = File.ReadAllLines(problems);

                Assert.Equal(0, result.SuccessCount);
                Assert.Equal(2, result.ProblemCount);

                Assert.Empty(outputLines);
                Assert.Equal(2, problemLines.Length);
            }
            finally
            {
                Directory.Delete(dir, true);
            }
        }

        private static string CreateTempDirectory()
        {
            string dir = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
            Directory.CreateDirectory(dir);
            return dir;
        }
    }
}