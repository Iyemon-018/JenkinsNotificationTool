namespace JenkinsNotificationTool.Tests.Core.Communicators
{
    using System;
    using System.Collections.Generic;
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
        private MockWebSocketServer _server;

        /// <summary>
        /// WebSocket サーバーがクライアントとの通信を切断したかどうか。
        /// </summary>
        private bool _serverClosedClient;

        /// <summary>
        /// WebSocket サーバーがクライアントとの接続を確立したかどうか。
        /// </summary>
        private bool _serverConnected;

        /// <summary>
        /// WebSocket 通信の確立に失敗したかどうか。
        /// </summary>
        private bool _connectionFailed;

        /// <summary>
        /// WebSocket 通信が切断されたかどうか。
        /// </summary>
        private bool _disconnected;

        /// <summary>
        /// WebSocket 通信が確立できたかどうか。
        /// </summary>
        private bool _connected;

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

        #region Connection method test

        /// <summary>
        /// <see cref="Test_Connection_Parameters_Failed"/> で使用するテストデータを取得します。
        /// </summary>
        /// <returns>テストデータ</returns>
        public static IEnumerable<object[]> GetConnectionParametersFailedTestData()
        {
            yield return new object[] {"(異常系) 引数URI がnull の場合、ArgumentNullException をスローすること。", typeof(ArgumentNullException), null, 1};
            yield return
                new object[] {"(異常系) 引数URI がstring.Empty の場合、ArgumentNullException をスローすること。", typeof(ArgumentNullException), string.Empty, 1};
            yield return
                new object[] {"(異常系) 引数リトライ最大回数 が0 の場合、ArgumentOutOfRangeException をスローすること。", typeof(ArgumentOutOfRangeException), ClientUri, 0};
        }

        /// <summary>
        /// <see cref="WebSocketCommunicator.Connection" /> をテストします。(失敗パターン)
        /// </summary>
        /// <param name="caseName">テスト内容</param>
        /// <param name="exceptionType">スローされることを期待する例外の型</param>
        /// <param name="uri">URI</param>
        /// <param name="retryMaximum">リトライ最大回数</param>
        /// <remarks>
        /// ここでは以下の内容をテストします。
        /// ・引数例外をスローする場合のテストを実行します。
        /// </remarks>
        [Theory]
        [MemberData(nameof(GetConnectionParametersFailedTestData))]
        public void Test_Connection_Parameters_Failed(string caseName, Type exceptionType, string uri, int retryMaximum)
        {
            // arrange
            var webSocket = new WebSocketCommunicator();
            
            // act
            var ex = Assert.Throws(exceptionType, () => webSocket.Connection(uri, retryMaximum));
            webSocket.Dispose();

            // assert
            Assert.NotNull(ex);
            Assert.IsType(exceptionType, ex);
            Output.WriteLine($"{caseName}{Environment.NewLine}" +
                             $"URI:{uri}, リトライ最大回数:{retryMaximum}");
        }

        #endregion

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
        public async void Test_Connection_Success()
        {
            // arrange
            var webSocket = new WebSocketCommunicator();
            webSocket.Connected += WebSocket_OnConnected;
            webSocket.ConnectionFailed += WebSocket_OnConnectionFailed;

            // act
            var ex = Record.Exception(() => webSocket.Connection(ClientUri, 1));
            ConnectionServer();
            await Task.Run(() =>
                           {
                               while (true)
                               {
                                   if (_connected) break;
                                   if (_connectionFailed) break;
                               }
                           });

            // assert
            try
            {
                Assert.True(webSocket.IsConnected);
                Assert.Null(ex);
                Assert.True(_connected);
                Assert.False(_connectionFailed);
            }
            finally
            {
                // 最後はテスト結果が失敗でもDisposeしないとテストが終了しない。
                webSocket.Connected -= WebSocket_OnConnected;
                webSocket.ConnectionFailed -= WebSocket_OnConnectionFailed;
                webSocket.Dispose();
            }
        }

        #endregion

        #region Disconnect method test

        /// <summary>
        /// <see cref="WebSocketCommunicator.Disconnect"/> のテストメソッドです。(成功パターン)
        /// </summary>
        /// <remarks>
        /// 以下の内容をテストします。
        /// ・<see cref="WebSocketCommunicator.Connection"/> メソッドを呼び出す前に<see cref="WebSocketCommunicator.Disconnect"/> を呼び出しても例外をスローしないこと。
        /// </remarks>
        [Fact]
        public void Test_Disconnect_Success_CallBeforeConnect()
        {
            // arrange
            var webSocket = new WebSocketCommunicator();
            // don't call
            //webSocket.Connection(ClientUri, 1);

            // act
            var ex = Record.Exception(() => webSocket.Disconnect());

            // assert
            Assert.Null(ex);
            Assert.False(webSocket.IsConnected);
            webSocket.Dispose();
        }

        #endregion

        #region Connection test

        /// <summary>
        /// <see cref="WebSocketCommunicator" /> をテストします。
        /// </summary>
        /// <remarks>
        /// 以下の内容をテストします。
        /// ・サーバーとの接続を実行して、<see cref="WebSocketCommunicator.Connected"/> が呼び出されること。
        /// ・サーバー側が切断して、<see cref="WebSocketCommunicator.Disconnected"/> が呼び出されること。
        /// ・切断後は<see cref="WebSocketCommunicator.IsConnected"/> がfalse になること。
        /// </remarks>
        [Fact]
        public async void Test_Connection_to_Disconnection()
        {
            // arrange
            var webSocket = new WebSocketCommunicator();
            webSocket.Connected += WebSocket_OnConnected;
            webSocket.ConnectionFailed += WebSocket_OnConnectionFailed;
            webSocket.Disconnected += WebSocket_OnDisconnected;

            // act
            var ex = Record.Exception(() => webSocket.Connection(ClientUri, 1));
            ConnectionServer();
            await Task.Run(() =>
            {
                while (true)
                {
                    if (_disconnected) break;
                    if (_connectionFailed) break;
                    if (_connected)
                    {
                        _server.Disconnection();
                    }
                }
            });

            // assert
            try
            {
                Assert.Null(ex);
                Assert.False(webSocket.IsConnected);
                Assert.True(_connected);
                Assert.False(_connectionFailed);
                Assert.True(_disconnected);
            }
            finally
            {
                webSocket.Connected -= WebSocket_OnConnected;
                webSocket.ConnectionFailed -= WebSocket_OnConnectionFailed;
                webSocket.Disconnected -= WebSocket_OnDisconnected;
                webSocket.Dispose();
            }
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
        /// WebSocket 通信が確立できた際に呼び出されるイベントハンドラです。
        /// </summary>
        /// <param name="sender">イベント送信元オブジェクト</param>
        /// <param name="e">イベント引数オブジェクト</param>
        private void WebSocket_OnConnected(object sender, EventArgs e)
        {
            _connected = true;
        }

        /// <summary>
        /// WebSocket 通信の確立に失敗した際に呼び出されるイベントハンドラです。
        /// </summary>
        /// <param name="sender">イベント送信元オブジェクト</param>
        /// <param name="e">イベント引数オブジェクト</param>
        private void WebSocket_OnConnectionFailed(object sender, EventArgs e)
        {
            _connectionFailed = true;
        }

        /// <summary>
        /// WebSocket 通信が切断された際に呼び出されるイベントハンドラです。
        /// </summary>
        /// <param name="sender">イベント送信元オブジェクト</param>
        /// <param name="e">イベント引数オブジェクト</param>
        private void WebSocket_OnDisconnected(object sender, EventArgs e)
        {
            _disconnected = true;
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