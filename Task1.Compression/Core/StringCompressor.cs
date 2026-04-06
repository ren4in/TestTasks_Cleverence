using System;
using System.Text;

namespace Task1.Compression.Core
{
    /// <summary>
    /// Реализует алгоритмы сжатия и распаковки строк.
    /// </summary>
    public static class StringCompressor
    {
        /// <summary>
        /// Сжимает строку, заменяя группы одинаковых символов форматом "символ + количество".
        /// Если символ встречается один раз, количество не добавляется.
        /// </summary>
        public static string Compress(string input)
        {
            if (input == null)
                throw new ArgumentNullException(nameof(input));

            if (input.Length == 0)
                return string.Empty;

            ValidateSourceString(input);

            var result = new StringBuilder();
            char current = input[0];
            int count = 1;

            for (int i = 1; i < input.Length; i++)
            {
                if (input[i] == current)
                {
                    count++;
                }
                else
                {
                    AppendCompressedGroup(result, current, count);
                    current = input[i];
                    count = 1;
                }
            }

            // После цикла отдельно добавляем последнюю группу.
            AppendCompressedGroup(result, current, count);

            return result.ToString();
        }

        /// <summary>
        /// Распаковывает строку, восстанавливая исходную последовательность символов.
        /// </summary>
        public static string Decompress(string input)
        {
            if (input == null)
                throw new ArgumentNullException(nameof(input));

            if (input.Length == 0)
                return string.Empty;

            var result = new StringBuilder();
            int i = 0;

            while (i < input.Length)
            {
                char currentChar = input[i];

                if (!IsLowerLatinLetter(currentChar))
                    throw new FormatException("Сжатая строка имеет неверный формат: ожидалась маленькая латинская буква.");

                i++;

                int count = 0;
                bool hasDigits = false;

                // Количество может состоять из нескольких цифр, например: a12
                while (i < input.Length && char.IsDigit(input[i]))
                {
                    hasDigits = true;

                    if (count == 0 && input[i] == '0')
                        throw new FormatException("Сжатая строка имеет неверный формат: количество не может начинаться с 0.");

                    try
                    {
                        checked
                        {
                            count = count * 10 + (input[i] - '0');
                        }
                    }
                    catch (OverflowException)
                    {
                        throw new FormatException("Сжатая строка имеет неверный формат: слишком большое количество повторений.");
                    }

                    i++;
                }

                if (!hasDigits)
                    count = 1;

                if (count <= 0)
                    throw new FormatException("Сжатая строка имеет неверный формат: количество должно быть больше 0.");

                try
                {
                    result.Append(currentChar, count);
                }
                catch (OutOfMemoryException)
                {
                    throw new FormatException("Сжатая строка имеет неверный формат: результат распаковки слишком велик.");
                }
            }

            return result.ToString();
        }

        /// <summary>
        /// Добавляет одну группу символов в сжатую строку.
        /// </summary>
        private static void AppendCompressedGroup(StringBuilder builder, char symbol, int count)
        {
            builder.Append(symbol);

            if (count > 1)
                builder.Append(count);
        }

        /// <summary>
        /// Проверяет, что исходная строка содержит только маленькие латинские буквы.
        /// </summary>
        private static void ValidateSourceString(string input)
        {
            foreach (char c in input)
            {
                if (!IsLowerLatinLetter(c))
                {
                    throw new ArgumentException(
                        "Исходная строка должна содержать только маленькие латинские буквы от a до z.",
                        nameof(input));
                }
            }
        }

        /// <summary>
        /// Проверяет, является ли символ строчной латинской буквой (a–z).
        /// </summary>
        private static bool IsLowerLatinLetter(char c)
        {
            return c >= 'a' && c <= 'z';
        }
    }
}