using System;
using System.IO;
using Task3.LogStandardizer.Processing;

namespace Task3.LogStandardizer
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("==== СТАНДАРТИЗАТОР ЛОГ-ФАЙЛОВ ====");

            Console.Write("Введите путь к входному файлу: ");
            string? inputFilePath = Console.ReadLine();

            // Базовая валидация пути
            if (string.IsNullOrWhiteSpace(inputFilePath) || !File.Exists(inputFilePath))
            {
                Console.WriteLine("Ошибка: файл не найден.");
                return;
            }

            Console.Write("Введите путь к выходному файлу (по умолчанию output.txt): ");
            string? outputFilePath = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(outputFilePath))
                outputFilePath = "output.txt";

            Console.Write("Введите путь к файлу проблемных строк (по умолчанию problems.txt): ");
            string? problemsFilePath = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(problemsFilePath))
                problemsFilePath = "problems.txt";

            try
            {
                var processor = new LogFileProcessor();

                // Основной запуск обработки
                var result = processor.Process(inputFilePath, outputFilePath, problemsFilePath);

                Console.WriteLine();
                Console.WriteLine($"Успешных записей: {result.SuccessCount}");
                Console.WriteLine($"Проблемных записей: {result.ProblemCount}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}");
            }

            Console.WriteLine("Нажмите любую клавишу для выхода...");
            Console.ReadKey();
        }
    }
}