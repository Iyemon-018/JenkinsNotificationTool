namespace JenkinsNotificationTool.Tests.Core.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using JenkinsNotification.Core.Extensions;
    using Xunit;
    using Xunit.Abstractions;

    /// <summary>
    /// <see cref="JsonSerializer" /> のテストクラスです。
    /// </summary>
    /// <seealso cref="JenkinsNotificationTool.Tests.TestBase" />
    public class JsonSerializerTests : TestBase
    {
        #region Const

        /// <summary>
        /// シリアライズ可能なJson文字列
        /// </summary>
        private static readonly string MockJsonString = @"{""project"":""MC11-MC1.App-develop.ForInternal"",""number"":79,""status"":""SUCCESS"",""result"":""SUCCESS""}";

        #endregion

        #region Ctor

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="output">テスト出力ヘルパー</param>
        public JsonSerializerTests(ITestOutputHelper output) : base(output)
        {
        }

        #endregion

        #region Methods

        /// <summary>
        /// <see cref="Test_JsonSerialize_string_Theory_Failed{T}"/> で使用するテストデータ
        /// </summary>
        /// <returns>IEnumerable&lt;System.Object[]&gt;.</returns>
        public static IEnumerable<object[]> GetJsonSerializeStringTestData()
        {
            var expected = new MockJson();

            yield return
                new object[]
                {
                    "(異常系) 変換対象の文字列がstring.Empty の場合、ArgumentNullException をスローすること。",
                    expected,
                    string.Empty,
                    typeof(ArgumentNullException)
                };

            yield return
                new object[]
                {
                    "(異常系) 変換対象の文字列が変換できない場合、SerializationException をスローすること。",
                    expected,
                    @"{""project"":""MC11-MC1.App-develop.ForInternal""""number"":79,""status"":""SUCCESS"",""result"":""SUCCESS""}",
                    typeof(SerializationException)
                };
        }

        /// <summary>
        /// <see cref="JsonSerializer.string" /> をテストします。(成功パターン)
        /// </summary>
        [Fact]
        public void Test_JsonSerialize_string_Success()
        {
            // arrange
            var expected = new MockJson
                           {
                               project = "MC11-MC1.App-develop.ForInternal",
                               number = 79,
                               status = "SUCCESS",
                               result = "SUCCESS"
                           };
            MockJson result = null;

            // act
            var ex = Record.Exception(() => result = MockJsonString.JsonSerialize<MockJson>());

            // assert
            Assert.Null(ex);
            Assert.NotNull(result);
            foreach (var property in typeof(MockJson).GetProperties())
            {
                var expectedPropertyValue = property.GetValue(expected);
                var resultPropertyValue = property.GetValue(result);
                Assert.Equal(expectedPropertyValue, resultPropertyValue);
            }
            WriteResult("(正常系) シリアライズに成功し、メンバーにデータがセットされていること。", result, expected);
        }

        /// <summary>
        /// <see cref="JsonSerializer.string" /> をテストします。(失敗パターン)
        /// </summary>
        /// <typeparam name="T">シリアライズ対象の型</typeparam>
        /// <param name="caseName">テストケース名</param>
        /// <param name="expected">期待値</param>
        /// <param name="jsonText">シリアライズ対象の文字列</param>
        /// <param name="exceptionType">発生するであろう例外の型</param>
        [Theory]
        [MemberData(nameof(GetJsonSerializeStringTestData))]
        public void Test_JsonSerialize_string_Theory_Failed<T>(string caseName, T expected, string jsonText, Type exceptionType) where T : class
        {
            // arrange
            T result = null;

            // act
            var ex = Assert.Throws(exceptionType, () => result = jsonText.JsonSerialize<T>());

            // assert
            Assert.Null(result);
            Assert.NotNull(ex);
            WriteResult(caseName, result, expected);
        }

        /// <summary>
        /// <see cref="JsonSerializer.JsonSerialize{T}(byte[])" /> をテストします。(失敗パターン)
        /// </summary>
        [Fact]
        public void Test_JsonSerialize_bytes_Failed()
        {
            // arrange
            MockJson result = null;
            
            // act
            var ex = Assert.Throws<ArgumentNullException>(() => result = ((byte[])null).JsonSerialize<MockJson>());

            // assert
            Assert.NotNull(ex);
            Assert.Null(result);
            Output.WriteLine("(異常系) シリアライズ対象のbyte配列がnull の場合、ArgumentNullException をスローすること。");
        }

        #endregion
    }
}