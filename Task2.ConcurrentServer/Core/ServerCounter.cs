using System.Threading;

namespace Task2.ConcurrentServer.Core
{
    /// <summary>
    /// Потокобезопасный статический "сервер" для хранения и изменения счётчика.
    /// Используется ReaderWriterLockSlim: он позволяет выполнять параллельное чтение
    /// и при этом гарантирует эксклюзивную запись.
    /// </summary>
    public static class ServerCounter
    {
        private static int _count = 0;
        private static readonly ReaderWriterLockSlim _lock = new ReaderWriterLockSlim();

        /// <summary>
        /// Возвращает текущее значение счётчика.
        /// Несколько потоков могут читать одновременно.
        /// </summary>
        public static int GetCount()
        {
            _lock.EnterReadLock();
            try
            {
                return _count;
            }
            finally
            {
                _lock.ExitReadLock();
            }
        }

        /// <summary>
        /// Добавляет значение к счётчику.
        /// Запись выполняется строго одним потоком за раз.
        /// </summary>
        public static void AddToCount(int value)
        {
            _lock.EnterWriteLock();
            try
            {
                // checked позволяет явно отлавливать переполнение int.
                checked
                {
                    _count += value;
                }
            }
            finally
            {
                _lock.ExitWriteLock();
            }
        }
        /// <summary>
        /// Сбрасывает значение счётчика.
        /// Используется в тестах для изоляции состояния.
        /// </summary>
        internal static void Reset()
        {
            _lock.EnterWriteLock();
            try
            {
                _count = 0;
            }
            finally
            {
                _lock.ExitWriteLock();
            }
        }
    }
}