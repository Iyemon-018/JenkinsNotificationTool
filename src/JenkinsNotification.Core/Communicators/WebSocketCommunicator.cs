namespace JenkinsNotification.Core.Communicators
{
    using System;
    using System.Threading.Tasks;
    using JenkinsNotification.Core.Extensions;
    using Logs;
    using Utility;
    using SuperSocket.ClientEngine;
    using WebSocket4Net;

    /// <summary>
    /// WebSocket 通信機能クラスです。
    /// </summary>
    /// <seealso cref="JenkinsNotification.Core.Communicators.IWebSocketCommunicator" />
    public class WebSocketCommunicator : IWebSocketCommunicator
    {
        #region Fields

        /// <summary>
        /// 接続を確立したかどうか
        /// </summary>
        private bool _isConnected;

        /// <summary>
        /// 接続に失敗したかどうか
        /// </summary>
        private bool _isConnectionFailed;

        /// <summary>
        /// 現在のリトライ回数
        /// </summary>
        private int _retryCount;

        /// <summary>
        /// リトライ最大回数
        /// </summary>
        private int _retryMaximum;

        /// <summary>
        /// WebSocketクライアン
        /// </summary>
        private WebSocket _webSocket;

        #endregion

        #region Events

        /// <summary>
        /// 通信接続確率イベントです。
        /// </summary>
        public event EventHandler Connected;

        /// <summary>
        /// 通信接続の切断イベントです。
        /// </summary>
        public event EventHandler Disconnected;

        /// <summary>
        /// 接続先からのデータ受信イベントです。
        /// </summary>
        public event EventHandler<ReceivedEventArgs> Received;

        public event EventHandler SendTimeout;

        /// <summary>
        /// 接続失敗イベントです。
        /// </summary>
        public event EventHandler ConnectionFailed;

        /// <summary>
        /// 接続先からのエラー受信イベントです。
        /// </summary>
        public event EventHandler<ReceivedErrorEventArgs> ReceivedError;

        #endregion

        #region Properties

        /// <summary>
        /// 接続できたかどうかを取得します。
        /// </summary>
        public bool IsConnected => _isConnected;

        /// <summary>
        /// 接続先URIを取得します。
        /// </summary>
        public string Uri { get; private set; }

        #endregion

        #region Methods

        /// <summary>
        /// 指定したURI先へ接続を試みます。
        /// </summary>
        /// <param name="uri">URI</param>
        /// <param name="retryMaximum">リトライ最大回数</param>
        /// <exception cref="System.ArgumentNullException"><paramref name="uri"/> がnull の場合にスローされます。</exception>
        /// <exception cref="System.ArgumentOutOfRangeException"><see cref="retryMaximum"/> が0 以下の場合にスローされます。</exception>
        public void Connection(string uri, int retryMaximum)
        {
            if (uri.IsEmpty()) throw new ArgumentNullException(nameof(uri));
            if (retryMaximum <= 0) throw new ArgumentOutOfRangeException(nameof(retryMaximum));

            Uri                 = uri;
            _retryMaximum       = retryMaximum;
            _retryCount         = 0;
            _isConnectionFailed = false;
            _webSocket          = new WebSocket(Uri);

            _webSocket.Opened          += WebSocket_OnOpened;
            _webSocket.Closed          += WebSocket_OnClosed;
            _webSocket.MessageReceived += WebSocket_OnMessageReceived;
            _webSocket.DataReceived    += WebSocket_OnDataReceived;
            _webSocket.Error           += WebSocket_OnDetectedError;

            // 接続を開始します。
            Connection();
        }

        /// <summary>
        /// 通信を切断します。
        /// </summary>
        public void Disconnect()
        {
            if (_webSocket != null && _webSocket.State == WebSocketState.Open)
            {
                _webSocket.Close();
                _isConnected = false;
            }
        }

        public Task Send(byte[] sendData, TimeSpan timeout)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 接続を開始します。
        /// </summary>
        private void Connection()
        {
            _webSocket.Open();          // 接続

            _retryCount++;
            if (!_isConnectionFailed && _retryCount > _retryMaximum)
            {
                //
                // リトライ回数オーバーのため、接続失敗とする。
                //
                _isConnectionFailed = true;
                _webSocket.Dispose();
                ThreadUtility.ExecuteUiThread(OnConnectionFailed);
            }
        }

        /// <summary>
        /// <see cref="Connected"/> イベントを呼び出します。
        /// </summary>
        private void OnConnected()
        {
            Connected?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// <see cref="ConnectionFailed"/> イベントを呼び出します。
        /// </summary>
        private void OnConnectionFailed()
        {
            ConnectionFailed?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// <see cref="Disconnected"/> イベントを呼び出します。
        /// </summary>
        private void OnDisconnected()
        {
            Disconnected?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// <see cref="Received"/> イベントを呼び出します。
        /// </summary>
        /// <param name="message">受信メッセージ</param>
        private void OnReceived(string message)
        {
            Received?.Invoke(this, new ReceivedEventArgs(message));
        }

        /// <summary>
        /// <see cref="Received"/> イベントを呼び出します。
        /// </summary>
        /// <param name="data">受信データ</param>
        private void OnReceived(byte[] data)
        {
            Received?.Invoke(this, new ReceivedEventArgs(data));
        }

        /// <summary>
        /// <see cref="ReceivedError"/> イベントを呼び出します。
        /// </summary>
        /// <param name="exception">受信した例外インスタンス</param>
        private void OnReceivedError(Exception exception)
        {
            ReceivedError?.Invoke(this, new ReceivedErrorEventArgs(exception));
        }

        private void OnSendTimeout()
        {
            SendTimeout?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// WebSocket で切断される際に呼び出されるイベントハンドラです。
        /// </summary>
        /// <param name="sender">イベント送信元オブジェクト</param>
        /// <param name="e">イベント引数オブジェクト</param>
        private void WebSocket_OnClosed(object sender, EventArgs e)
        {
            LogManager.Info($"{Uri} との通信を切断した。");
            _isConnected = false;
            OnDisconnected();
        }

        /// <summary>
        /// WebSocket でバイナリデータを受信した際に呼び出されるイベントハンドラです。
        /// </summary>
        /// <param name="sender">イベント送信元オブジェクト</param>
        /// <param name="e">イベント引数オブジェクト</param>
        private void WebSocket_OnDataReceived(object sender, DataReceivedEventArgs e)
        {
            LogManager.Info($"{Uri} からバイナリ データを受信しました。");
            OnReceived(e.Data);
        }

        /// <summary>
        /// WebSocket でエラーを検出した際に呼び出されるイベントハンドラです。
        /// </summary>
        /// <param name="sender">イベント送信元オブジェクト</param>
        /// <param name="e">イベント引数オブジェクト</param>
        private void WebSocket_OnDetectedError(object sender, ErrorEventArgs e)
        {
            LogManager.Error($"{Uri} からエラーを検出しました。", e.Exception);
            OnReceivedError(e.Exception);
            Task.Delay(TimeSpan.FromSeconds(3));
            Connection();
        }

        /// <summary>
        /// WebSocket でメッセージを受信した際に呼び出されるイベントハンドラです。
        /// </summary>
        /// <param name="sender">イベント送信元オブジェクト</param>
        /// <param name="e">イベント引数オブジェクト</param>
        private void WebSocket_OnMessageReceived(object sender, MessageReceivedEventArgs e)
        {
            LogManager.Info($"{Uri} からテキスト メッセージを受信しました。({e.Message})");
            OnReceived(e.Message);
        }

        /// <summary>
        /// WebSocket で接続される際に呼び出されるイベントハンドラです。
        /// </summary>
        /// <param name="sender">イベント送信元オブジェクト</param>
        /// <param name="e">イベント引数オブジェクト</param>
        private void WebSocket_OnOpened(object sender, EventArgs e)
        {
            LogManager.Info($"{Uri} との接続を確立した。");
            _isConnected = true;
            ThreadUtility.ExecuteUiThread(OnConnected);
        }

        #endregion

        #region IDisposable Support

        /// <summary>
        /// このオブジェクトが破棄されたかどうか
        /// </summary>
        private bool _disposed;

        /// <summary>
        /// このオブジェクトを解放します。
        /// </summary>
        /// <param name="disposing">明示的な破棄かどうか。true の場合は明示的な破棄、false の場合は暗黙的な破棄です。</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    // マネージ状態を破棄します (マネージ オブジェクト)。
                }

                if (_webSocket != null)
                {
                    _webSocket.Opened          -= WebSocket_OnOpened;
                    _webSocket.Closed          -= WebSocket_OnClosed;
                    _webSocket.MessageReceived -= WebSocket_OnMessageReceived;
                    _webSocket.DataReceived    -= WebSocket_OnDataReceived;
                    _webSocket.Error           -= WebSocket_OnDetectedError;
                    _webSocket.Close();
                    _webSocket.Dispose();
                }
                LogManager.Info($"{Uri} との接続を解放した。");

                _disposed = true;
            }
        }

        /// <summary>
        /// ファイナライズ
        /// </summary>
        ~WebSocketCommunicator()
        {
            Dispose(false);
        }

        /// <summary>
        /// アンマネージ リソースの解放またはリセットに関連付けられているアプリケーション定義のタスクを実行します。
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}