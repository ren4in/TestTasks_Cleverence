using System;
using System.Threading.Tasks;
using Task2.ConcurrentServer.Core;
using Xunit;

namespace Task2.ConcurrentServer.Tests
{
    public class ServerCounterTests
    {
        public ServerCounterTests()
        {
            // Перед каждым тестом сбрасываем состояние счётчика.
            ServerCounter.Reset();
        }

        // Базовая проверка записи и чтения
        [Fact]
        public void AddToCount_ShouldIncreaseValue()
        {
            ServerCounter.AddToCount(5);

            int result = ServerCounter.GetCount();

            Assert.Equal(5, result);
        }

        // Проверяем, что отрицательные значения тоже корректно учитываются
        [Fact]
        public void AddToCount_ShouldHandleNegativeValue()
        {
            ServerCounter.AddToCount(10);
            ServerCounter.AddToCount(-3);

            int result = ServerCounter.GetCount();

            Assert.Equal(7, result);
        }

        // Несколько потоков одновременно пишут в счётчик, итог не должен теряться
        [Fact]
        public void AddToCount_ShouldBeThreadSafe()
        {
            int threadCount = 10;
            int iterations = 1000;

            Parallel.For(0, threadCount, _ =>
            {
                for (int i = 0; i < iterations; i++)
                {
                    ServerCounter.AddToCount(1);
                }
            });

            int expected = threadCount * iterations;
            int actual = ServerCounter.GetCount();

            Assert.Equal(expected, actual);
        }

        // Проверяем смешанный сценарий: один поток пишет, несколько потоков читают
        [Fact]
        public void GetCount_ShouldWorkCorrectly_WithConcurrentReadsAndWrites()
        {
            int readerCount = 5;
            int iterations = 1000;

            Task writer = Task.Run(() =>
            {
                for (int i = 0; i < iterations; i++)
                {
                    ServerCounter.AddToCount(1);
                }
            });

            Task[] readers = new Task[readerCount];

            for (int i = 0; i < readerCount; i++)
            {
                readers[i] = Task.Run(() =>
                {
                    for (int j = 0; j < iterations; j++)
                    {
                        ServerCounter.GetCount();
                    }
                });
            }

            Task.WaitAll(readers);
            writer.Wait();

            Assert.Equal(iterations, ServerCounter.GetCount());
        }

        // Даже если только читаем параллельно, состояние не должно меняться
        [Fact]
        public void GetCount_ShouldReturnSameValue_WhenReadConcurrently()
        {
            ServerCounter.AddToCount(42);

            int readerCount = 20;
            Task[] readers = new Task[readerCount];

            for (int i = 0; i < readerCount; i++)
            {
                readers[i] = Task.Run(() =>
                {
                    for (int j = 0; j < 1000; j++)
                    {
                        int value = ServerCounter.GetCount();
                        Assert.Equal(42, value);
                    }
                });
            }

            Task.WaitAll(readers);
        }

        // checked в AddToCount должен выбрасывать исключение при переполнении
        [Fact]
        public void AddToCount_ShouldThrowOverflowException_WhenValueOverflows()
        {
            ServerCounter.AddToCount(int.MaxValue);

            Assert.Throws<OverflowException>(() => ServerCounter.AddToCount(1));
        }
    }
}