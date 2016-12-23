namespace Server.Simulator.Communicators
{
    using System.Net;

    /// <summary>
    /// WebSocket 通信によるクライアントからのリクエストを受信した時のイベント引数クラスです。
    /// </summary>
    /// <seealso cref="Server.Simulator.Communicators.WebSocketClientEventArgs" />
    public class WebSocketReceivedRequestEventArgs : WebSocketClientEventArgs
    {
        #region Fields

        /// <summary>
        /// 受信データ
        /// </summary>
        private readonly byte[] _receivedData;

        #endregion

        #region Ctor

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="clientEndPoint">クライアントのエンドポイント</param>
        /// <param name="receivedData">受信データ</param>
        public WebSocketReceivedRequestEventArgs(IPEndPoint clientEndPoint, byte[] receivedData) : base(clientEndPoint)
        {
            _receivedData = receivedData;
        }

        #endregion

        #region Methods

        /// <summary>
        /// 受信データを取得します。
        /// </summary>
        /// <returns>受信データ</returns>
        public byte[] GetReceivedData()
        {
            return (byte[]) _receivedData.Clone();
        }

        #endregion
    }
}