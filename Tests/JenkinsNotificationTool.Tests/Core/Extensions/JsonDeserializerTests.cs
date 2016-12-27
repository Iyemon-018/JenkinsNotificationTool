namespace JenkinsNotificationTool.Tests.Core.Extensions
{
    using System;
    using System.Text;
    using JenkinsNotification.Core.Extensions;
    using Xunit;
    using Xunit.Abstractions;

    /// <summary>
    /// <see cref="JsonDeserializer" /> のテストクラスです。
    /// </summary>
    /// <seealso cref="JenkinsNotificationTool.Tests.TestBase" />
    public class JsonDeserializerTests : TestBase
    {
        #region Ctor

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="output">テスト出力ヘルパー</param>
        public JsonDeserializerTests(ITestOutputHelper output) : base(output)
        {
        }

        #endregion

        #region Methods

        /// <summary>
        /// <see cref="JsonDeserializer.JsonDeserialize{T}(T)" /> をテストします。(失敗パターン)
        /// </summary>
        /// <remarks>
        /// 以下の内容をテストします。
        /// ・オブジェクトがnull の場合、<see cref="ArgumentNullException"/> をスローすること。
        /// </remarks>
        [Fact]
        public void Test_JsonDeserialize_Failed()
        {
            // arrange
            string result = null;

            // act
            var ex = Assert.Throws<ArgumentNullException>(() => result = ((MockJson)null).JsonDeserialize(Encoding.UTF8));

            // assert
            Assert.NotNull(ex);
            Assert.Null(result);
            Output.WriteLine("(異常系) null をデシリアライズするとArgumentNullException をスローすること。");
        }

        /// <summary>
        /// <see cref="JsonDeserializer.JsonDeserialize{T}(T)" /> をテストします。(成功パターン)
        /// </summary>
        /// <remarks>
        /// 以下の内容をテストします。
        /// ・エンコードを指定してデシリアライズを実行すると例外をスローせずに完了すること。
        /// </remarks>
        [Fact]
        public void Test_JsonDeserialize_Success()
        {
            // arrange
            var expected = new MockJson
                           {
                               project = DateTime.Now.ToString("O"),
                               number = DateTime.Now.Millisecond,
                               result = "test result",
                               status = "test status"
                           };
            string result = null;

            // act
            var ex = Record.Exception(() => result = expected.JsonDeserialize(Encoding.UTF8));

            // assert
            Assert.Null(ex);
            Assert.NotNull(result);
            Output.WriteLine("(正常系) Json文字列のデシリアライズが正常に実行できること。");
        }

        /// <summary>
        /// <see cref="JsonDeserializer.JsonDeserialize{T}(T)" /> をテストします。(成功パターン)
        /// </summary>
        /// <remarks>
        /// 以下の内容をテストします。
        /// ・エンコードを指定していない場合でも、デシリアライズを実行すると例外をスローせずに完了すること。
        /// </remarks>
        [Fact]
        public void Test_JsonDeserialize_Success_ParameterNull()
        {
            // arrange
            var expected = new MockJson
            {
                project = DateTime.Now.ToString("O"),
                number = DateTime.Now.Millisecond,
                result = "test result",
                status = "test status"
            };
            string result = null;

            // act
            var ex = Record.Exception(() => result = expected.JsonDeserialize(null));

            // assert
            Assert.Null(ex);
            Assert.NotNull(result);
            Output.WriteLine("(正常系) 引数がnull の場合でも、Json文字列のデシリアライズが正常に実行できること。");
        }

        #endregion
    }
}