using System;
using Task1.Compression.Core;

namespace Task1.Compression
{
    internal class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("Выберите режим:");
                Console.WriteLine("1 - Сжатие");
                Console.WriteLine("2 - Распаковка");
                Console.WriteLine("0 - Выход");

                string? mode = Console.ReadLine();

                if (mode == "0")
                    break;

                if (mode != "1" && mode != "2")
                {
                    Console.WriteLine("Неверный пункт меню.");
                    ReturnToMenu();
                    continue;
                }

                Console.WriteLine("Введите строку:");
                string input = Console.ReadLine() ?? string.Empty;

                try
                {
                    string result = mode == "1"
                        ? StringCompressor.Compress(input)
                        : StringCompressor.Decompress(input);

                    Console.WriteLine($"Результат: {result}");
                }
                catch (ArgumentException ex)
                {
                    Console.WriteLine($"Ошибка: {ex.Message}");
                }
                catch (FormatException ex)
                {
                    Console.WriteLine($"Ошибка: {ex.Message}");
                }

                ReturnToMenu();
            }
        }

        private static void ReturnToMenu()
        {
            Console.WriteLine();
            Console.WriteLine("Нажмите любую клавишу, чтобы вернуться в меню...");
            Console.ReadKey();
        }
    }
}