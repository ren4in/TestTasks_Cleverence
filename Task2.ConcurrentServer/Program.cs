using System;
using System.Threading;
using System.Threading.Tasks;
using Task2.ConcurrentServer.Core;

namespace Task2.ConcurrentServer
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Демонстрация работы ServerCounter");
            Console.WriteLine();

            Task[] tasks =
            {
                Task.Run(() => Reader("Читатель 1")),
                Task.Run(() => Reader("Читатель 2")),
                Task.Run(() => Reader("Читатель 3")),
                Task.Run(() => Writer("Писатель 1", 5)),
                Task.Run(() => Writer("Писатель 2", 10))
            };

            Task.WaitAll(tasks);

            Console.WriteLine();
            Console.WriteLine($"Итоговое значение count: {ServerCounter.GetCount()}");
            Console.WriteLine("Нажмите любую клавишу для выхода...");
            Console.ReadKey();
        }

        private static void Reader(string name)
        {
            for (int i = 0; i < 5; i++)
            {
                int value = ServerCounter.GetCount();
                Console.WriteLine($"{name} прочитал значение: {value}");

                // Небольшая задержка нужна только для наглядной демонстрации параллельной работы.
                Thread.Sleep(200);
            }
        }

        private static void Writer(string name, int valueToAdd)
        {
            for (int i = 0; i < 3; i++)
            {
                try
                {
                    ServerCounter.AddToCount(valueToAdd);
                    Console.WriteLine($"{name} добавил {valueToAdd}");
                }
                catch (OverflowException)
                {
                    Console.WriteLine($"{name}: ошибка переполнения при изменении счётчика.");
                }

                Thread.Sleep(300);
            }
        }
    }
}