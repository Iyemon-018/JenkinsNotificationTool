namespace JenkinsNotificationTool.Tests.Core.Communicators
{
    using System;
    using JenkinsNotification.Core.Communicators;
    using Xunit;
    using Xunit.Abstractions;

    /// <summary>
    /// <see cref="WebSocketCommunicator" /> のテストクラスです。
    /// </summary>
    /// <seealso cref="TestBase" />
    public class WebSocketCommunicatorTests : TestBase
    {
        #region Const
        
        private const string ConnectionUri = "ws://127.0.0.1:33456/";

        #endregion

        #region Ctor

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="output">テスト出力ヘルパー</param>
        public WebSocketCommunicatorTests(ITestOutputHelper output) : base(output)
        {
        }

        #endregion

        #region Methods

        #region Method tests

        //
        // ここでは各メソッドの引数チェックなど、メソッド単体のテストのみ行います。
        // （引数チェックや、イベント購読チェックなど)
        // テスト対象のクラスは多機能なため、メソッド単体テストと機能テストに分割しています。
        //

        #region Connection method test

        /// <summary>
        /// <see cref="WebSocketCommunicator.Connection" /> をテストします。
        /// </summary>
        /// <param name="caseName">テスト内容</param>
        /// <param name="exceptionType">スローされるであろう例外の型</param>
        /// <param name="uri">URI</param>
        /// <param name="retryMaximum">リトライ最大回数</param>
        [Theory]
        [InlineData("(異常系) URIがnull の場合、ArgumentNullException をスローすること。", typeof(ArgumentNullException), null, 1)]
        [InlineData("(異常系) URIが空の場合、ArgumentNullException をスローすること。", typeof(ArgumentNullException), "", 1)]
        [InlineData("(異常系) リトライ最大回数が０の場合、ArgumentOutOfRangeException をスローすること。", typeof(ArgumentOutOfRangeException), ConnectionUri, 0)]
        [InlineData("(正常系) URIが設定されていて、リトライ最大回数が１以上の場合、例外をスローしないこと。", null, ConnectionUri, 1)]
        public void Test_Connection_ParameterTest_Theory(string caseName, Type exceptionType, string uri, int retryMaximum)
        {
            // arrange
            Exception ex;
            var c = new WebSocketCommunicator();

            // act
            // assert
            if (exceptionType == null)
            {
                ex = Record.Exception(() => c.Connection(uri, retryMaximum));
                Assert.Null(ex);
                Assert.Equal(uri, c.Uri);
            }
            else
            {
                ex = Assert.Throws(exceptionType, () => c.Connection(uri, retryMaximum));
                Assert.NotNull(ex);
                Assert.IsType(exceptionType, ex);
                Assert.Null(c.Uri);
            }

            Output.WriteLine($"{caseName}{Environment.NewLine}" +
                             $"URI:{uri}, リトライ最大回数:{retryMaximum}");
        }

        #endregion

        #region Send method test

        // TODO Send メソッドの引数テストを追加する。

        #endregion

        #endregion

        #endregion
    }
}