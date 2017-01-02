namespace JenkinsNotificationTool.Tests.Core.Communicators
{
    using System;
    using System.Threading.Tasks;
    using JenkinsNotification.Core.Communicators;
    using Xunit;
    using Xunit.Abstractions;

    /// <summary>
    /// <see cref="WebSocketCommunicator" /> のテストクラスです。
    /// </summary>
    /// <seealso cref="JenkinsNotificationTool.Tests.TestBase" />
    public class WebSocketCommunicatorTests : TestBase
    {
        #region Const

        /// <summary>
        /// Server のアドレス
        /// </summary>
        private const string ServerAddress = "http://127.0.0.1:33456/";

        /// <summary>
        /// クライアントの接続用URI
        /// </summary>
        private const string ClientUri = "ws://127.0.0.1:33456/";

        #endregion

        #region Fields

        /// <summary>
        /// WebSocket サーバー
        /// </summary>
        private readonly MockWebSocketServer _server;

        /// <summary>
        /// WebSocket サーバーがクライアントとの通信を切断したかどうか。
        /// </summary>
        private bool _serverClosedClient;

        /// <summary>
        /// WebSocket サーバーがクライアントとの接続を確立したかどうか。
        /// </summary>
        private bool _serverConnected;

        #endregion

        #region Ctor

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="output">テスト出力ヘルパー</param>
        public WebSocketCommunicatorTests(ITestOutputHelper output) : base(output)
        {
            _server = new MockWebSocketServer();
            _server.ConnectionClient += Server_OnConnectionClient;
            _server.ReceivedRequest += Server_OnReceivedRequest;
            _server.ClosedClient += Server_OnClosedClient;
        }

        #endregion

        #region Methods

        #region Connected event test
        
        /// <summary>
        /// <see cref="WebSocketCommunicator.Connected"/> のテストメソッドです。(成功パターン)
        /// </summary>
        /// <remarks>
        /// 以下の内容をテストします。
        /// ・サーバーとの接続に成功した場合、<see cref="WebSocketCommunicator.Connected"/> イベントが実行されること。
        /// ・接続実行時に例外が発生しないこと。
        /// </remarks>
        [Fact]
        public async void Test_Connected_Success()
        {
            // arrange
            var connectedFlag = false;
            var webSocket = new WebSocketCommunicator();
            webSocket.Connected += (sender, e) => { connectedFlag = true; };

            // act
            var ex = Record.Exception(() => webSocket.Connection(ClientUri, 100));
            ConnectionServer();
            await Task.Delay(TimeSpan.FromSeconds(3));

            // assert
            try
            {
                Assert.Null(ex);
                Assert.True(connectedFlag);
            }
            finally
            {
                webSocket.Dispose();
            }

            Output.WriteLine("(正常系) サーバーとの接続に成功した場合、Connected イベントが実行されること。");
        }

        #endregion

        /// <summary>
        /// アンマネージドリソースを解放します。
        /// </summary>
        protected override void OnDisposingUnmanagedResources()
        {
            base.OnDisposingUnmanagedResources();

            if (_server != null)
            {
                _server.ConnectionClient -= Server_OnConnectionClient;
                _server.ReceivedRequest -= Server_OnReceivedRequest;
                _server.ClosedClient -= Server_OnClosedClient;
                _server.Disconnection();
            }
        }

        /// <summary>
        /// WebScoket サーバーとの通信を非同期で実行します。
        /// </summary>
        private async void ConnectionServer()
        {
            await _server.Connection(ServerAddress);
        }

        /// <summary>
        /// WebSocket サーバーがクライアントとの通信を切断した際に呼び出されるイベントハンドラです。
        /// </summary>
        /// <param name="sender">イベント送信元オブジェクト</param>
        /// <param name="e">イベント引数オブジェクト</param>
        private void Server_OnClosedClient(object sender, WebSocketClientEventArgs e)
        {
            _serverClosedClient = true;
        }

        /// <summary>
        /// WebSocket サーバーでクライアントとの接続が確立できた際に呼び出されるイベントハンドラです。
        /// </summary>
        /// <param name="sender">イベント送信元オブジェクト</param>
        /// <param name="e">イベント引数オブジェクト</param>
        private void Server_OnConnectionClient(object sender, WebSocketClientEventArgs e)
        {
            _serverConnected = true;
        }

        /// <summary>
        /// WebSocket サーバーでデータが受信された際に呼び出されるイベントハンドラです。
        /// </summary>
        /// <param name="sender">イベント送信元オブジェクト</param>
        /// <param name="e">イベント引数オブジェクト</param>
        private void Server_OnReceivedRequest(object sender, WebSocketReceivedRequestEventArgs e)
        {
            
        }

        #endregion
    }
}