using System;
using Task1.Compression.Core;
using Xunit;

namespace Task1.Compression.Tests
{
    public class StringCompressorTests
    {
        // Основные сценарии сжатия
        [Theory]
        [InlineData("", "")]
        [InlineData("a", "a")]
        [InlineData("abcde", "abcde")]
        [InlineData("aaabbcccdde", "a3b2c3d2e")]
        [InlineData("aaaaa", "a5")]
        [InlineData("zzzzzzzzzz", "z10")]
        public void Compress_ShouldReturnExpectedResult(string input, string expected)
        {
            var actual = StringCompressor.Compress(input);
            Assert.Equal(expected, actual);
        }

        // Чередование символов не должно сжиматься
        [Fact]
        public void Compress_ShouldNotChange_WhenCharactersAlternate()
        {
            var actual = StringCompressor.Compress("abababab");
            Assert.Equal("abababab", actual);
        }

        // Длинная группа должна корректно превращаться в многозначный count
        [Fact]
        public void Compress_ShouldHandleLongGroup()
        {
            var actual = StringCompressor.Compress(new string('a', 25));
            Assert.Equal("a25", actual);
        }

        // Невалидные данные для исходной строки
        [Theory]
        [InlineData("A")]
        [InlineData("abc1")]
        [InlineData("abc ")]
        [InlineData("a_bc")]
        [InlineData("abc!")]
        [InlineData("абв")]
        [InlineData("ёжз")]
        [InlineData("abcпр")]
        [InlineData("приabc")]
        [InlineData("aбc")]
        public void Compress_ShouldThrowArgumentException_ForInvalidInput(string input)
        {
            Assert.Throws<ArgumentException>(() => StringCompressor.Compress(input));
        }

        [Fact]
        public void Compress_ShouldThrowArgumentNullException_WhenInputIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => StringCompressor.Compress(null!));
        }

        // Основные сценарии распаковки
        [Theory]
        [InlineData("", "")]
        [InlineData("a", "a")]
        [InlineData("abcde", "abcde")]
        [InlineData("a3b2c3d2e", "aaabbcccdde")]
        [InlineData("a5", "aaaaa")]
        [InlineData("z10", "zzzzzzzzzz")]
        [InlineData("a2bc10a3", "aabccccccccccaaa")]
        public void Decompress_ShouldReturnExpectedResult(string input, string expected)
        {
            var actual = StringCompressor.Decompress(input);
            Assert.Equal(expected, actual);
        }

        // Явное указание количества, включая 1
        [Theory]
        [InlineData("a1", "a")]
        [InlineData("x1y2z3", "xyyzzz")]
        public void Decompress_ShouldHandleExplicitCounts(string input, string expected)
        {
            var actual = StringCompressor.Decompress(input);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Decompress_ShouldHandleLongGroup()
        {
            var actual = StringCompressor.Decompress("a25");
            Assert.Equal(new string('a', 25), actual);
        }

        // Невалидный формат сжатой строки
        [Theory]
        [InlineData("3a")]
        [InlineData("1")]
        [InlineData("A2")]
        [InlineData("aB")]
        [InlineData("a_2")]
        [InlineData("a-2")]
        [InlineData("a0")]
        [InlineData("a01")]
        [InlineData("абв")]
        [InlineData("я2")]
        [InlineData("a2б")]
        [InlineData("привет")]
        [InlineData("a2пр")]
        public void Decompress_ShouldThrowFormatException_ForInvalidInput(string input)
        {
            Assert.Throws<FormatException>(() => StringCompressor.Decompress(input));
        }

        [Fact]
        public void Decompress_ShouldThrowArgumentNullException_WhenInputIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => StringCompressor.Decompress(null!));
        }

        // Защита от слишком большого числа повторений
        [Fact]
        public void Decompress_ShouldThrowFormatException_ForTooLargeCount()
        {
            var input = "a999999999999999999999999999999";
            Assert.Throws<FormatException>(() => StringCompressor.Decompress(input));
        }

        // Проверяем, что операции взаимно обратимы
        [Theory]
        [InlineData("")]
        [InlineData("a")]
        [InlineData("abc")]
        [InlineData("aaabbcccdde")]
        [InlineData("zzzzzz")]
        [InlineData("aabccccccccccaaa")]
        public void Compress_Then_Decompress_ShouldReturnOriginal(string input)
        {
            var compressed = StringCompressor.Compress(input);
            var decompressed = StringCompressor.Decompress(compressed);

            Assert.Equal(input, decompressed);
        }
    }
}