namespace JenkinsNotification.Core.Communicators
{
    using System;
    using System.Threading.Tasks;

    /// <summary>
    /// WebSocket 通信機能インターフェースです。
    /// </summary>
    /// <seealso cref="System.IDisposable" />
    public interface IWebSocketCommunicator : IDisposable
    {
        #region Events

        /// <summary>
        /// 通信接続確率イベントです。
        /// </summary>
        event EventHandler Connected;

        /// <summary>
        /// 通信接続の切断イベントです。
        /// </summary>
        event EventHandler Disconnected;

        /// <summary>
        /// 接続先からのデータ受信イベントです。
        /// </summary>
        event EventHandler<ReceivedEventArgs> Received;

        event EventHandler SendTimeout;

        /// <summary>
        /// 接続失敗イベントです。
        /// </summary>
        event EventHandler ConnectionFailed;

        /// <summary>
        /// 接続先からのエラー受信イベントです。
        /// </summary>
        event EventHandler<ReceivedErrorEventArgs> ReceivedError;

        #endregion

        #region Properties

        /// <summary>
        /// 接続できたかどうかを取得します。
        /// </summary>
        bool IsConnected { get; }

        /// <summary>
        /// 接続先URIを取得します。
        /// </summary>
        string Uri { get; }

        #endregion

        #region Methods

        /// <summary>
        /// 指定したURI先へ接続を試みます。
        /// </summary>
        /// <param name="uri">URI</param>
        /// <param name="retryMaximum">リトライ最大回数</param>
        void Connection(string uri, int retryMaximum);

        /// <summary>
        /// 通信を切断します。
        /// </summary>
        void Disconnect();

        Task Send(byte[] sendData, TimeSpan timeout);

        #endregion
    }
}