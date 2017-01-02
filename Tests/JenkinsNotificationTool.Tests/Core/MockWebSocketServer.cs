namespace JenkinsNotificationTool.Tests.Core
{
    using System;
    using System.Net;
    using System.Net.WebSockets;
    using System.Threading;
    using System.Threading.Tasks;

    public class MockWebSocketServer
    {
        #region Fields

        /// <summary>
        /// WebSocketクライアント
        /// </summary>
        private WebSocket _client;

        /// <summary>
        /// クライアントと接続しているかどうか
        /// </summary>
        private bool _isConnected;

        #endregion

        #region Events

        /// <summary>
        /// クライアントとの接続検出イベントです。
        /// </summary>
        public event EventHandler<WebSocketClientEventArgs> ConnectionClient;

        /// <summary>
        /// クライアントからのリクエスト受信イベントです。
        /// </summary>
        public event EventHandler<WebSocketReceivedRequestEventArgs> ReceivedRequest;

        /// <summary>
        /// クライアントとの切断検出イベントです。
        /// </summary>
        public event EventHandler<WebSocketClientEventArgs> ClosedClient;

        #endregion

        #region Properties

        /// <summary>
        /// クライアントと接続しているかどうかかどうかを取得します。
        /// </summary>
        public bool IsConnected => _isConnected;

        #endregion

        #region Methods

        /// <summary>
        /// クライアントとの接続を非同期で実行します。
        /// </summary>
        /// <param name="uriPrefix">接続URIプレフィックス</param>
        /// <returns>接続実行タスク</returns>
        public async Task Connection(string uriPrefix)
        {
            var httpListener = new HttpListener();
            httpListener.Prefixes.Add(uriPrefix);
            httpListener.Start(); // 通信開始

            //
            // クライアントからの接続を待つ。
            //
            var context = await httpListener.GetContextAsync();
            if (context.Request.IsWebSocketRequest)
            {
                // クライアントがWebSocket でのリクエストを求めているならリクエストを受信開始する。
                await ProcessRequest(context);
            }
            httpListener.Stop();
        }

        /// <summary>
        /// 通信を切断します。
        /// </summary>
        public void Disconnection()
        {
            if (_client != null && _client.State != WebSocketState.Closed)
            {
                _client.CloseAsync(WebSocketCloseStatus.NormalClosure, "", CancellationToken.None);
                _client.Dispose();
                _client = null;
            }
        }

        /// <summary>
        /// 指定したセグメントデータをクライアントに送信します。
        /// </summary>
        /// <param name="segment">送信セグメント</param>
        /// <returns>送信タスク</returns>
        /// <exception cref="System.InvalidOperationException">クライアントがオープンされていない場合にスローされます。</exception>
        public async Task SendAsync(byte[] segment)
        {
            if (_client == null || _client.State != WebSocketState.Open)
            {
                throw new InvalidOperationException("WebSocketクライアントがオープンされていません。");
            }

            var buff = new ArraySegment<byte>(segment);
            await _client.SendAsync(buff, WebSocketMessageType.Text, true, CancellationToken.None);
        }

        /// <summary>
        /// <see cref="ClosedClient"/> イベントを発行します。
        /// </summary>
        /// <param name="clientEndPoint">クライアントのエンドポイント</param>
        protected virtual void OnClosedClient(IPEndPoint clientEndPoint)
        {
            ClosedClient?.Invoke(this, new WebSocketClientEventArgs(clientEndPoint));
        }

        /// <summary>
        /// <see cref="ConnectionClient"/> イベントを発行します。
        /// </summary>
        /// <param name="clientEndPoint">クライアントのエンドポイント</param>
        protected virtual void OnConnectionClient(IPEndPoint clientEndPoint)
        {
            ConnectionClient?.Invoke(this, new WebSocketClientEventArgs(clientEndPoint));
        }

        /// <summary>
        /// <see cref="ReceivedRequest"/> イベントを発行します。
        /// </summary>
        /// <param name="clientEndPoint">クライアントのエンドポイント</param>
        /// <param name="data">受信データ</param>
        protected virtual void OnReceivedRequest(IPEndPoint clientEndPoint, byte[] data)
        {
            ReceivedRequest?.Invoke(this, new WebSocketReceivedRequestEventArgs(clientEndPoint, data));
        }

        /// <summary>
        /// クライアントからのリクエスト受信を非同期で待機します。
        /// </summary>
        /// <param name="context">HTTPリスナー コンテキスト</param>
        /// <returns>リクエスト受信まちタスク</returns>
        private async Task ProcessRequest(HttpListenerContext context)
        {
            //
            // WebSocket の接続を非同期で待ち受ける。
            //
            var contextResult = await context.AcceptWebSocketAsync(null);
            _client = contextResult.WebSocket;
            _isConnected = true;

            // 接続イベントを着火する。
            OnConnectionClient(context.Request.RemoteEndPoint);

            try
            {
                //
                // クライアントからの切断を受信するまで永久に受信を待機する。
                //
                while (_client.State == WebSocketState.Open)
                {
                    // 受信待ち
                    var buff = new ArraySegment<byte>(new byte[1024]);
                    var received = await _client.ReceiveAsync(buff, CancellationToken.None);

                    if (received.MessageType == WebSocketMessageType.Close)
                    {
                        // クライアントが切断してきた。
                        OnClosedClient(context.Request.RemoteEndPoint);
                    }
                    else if (received.MessageType == WebSocketMessageType.Text)
                    {
                        // クライアントからテキストを受信した。
                        OnReceivedRequest(context.Request.RemoteEndPoint, buff.Array);
                    }
                }
            }
            catch (Exception e)
            {
                //
                // 通信切断に失敗した場合、それ以降は通信しないものとして扱う。
                // CIサービスでテストを実行するとこのルートで例外が起きる。
                // おそらく、Dispose するよりも前のタイミングで_client.ReceiveAsync() が実行されてしまうため。
                //
                Console.WriteLine($@"<WebSocket Server> force disconnected. {e.Message}");
                OnClosedClient(context.Request.RemoteEndPoint);
            }

            // 接続終了
            _isConnected = false;
        }

        #endregion
    }
}