namespace Server.Simulator.Communicators
{
    using System;
    using System.Net;

    /// <summary>
    /// WebSocket による通信でクライアントとの接続、切断を検出したときのイベント引数クラスです。
    /// </summary>
    /// <seealso cref="System.EventArgs" />
    public class WebSocketClientEventArgs : EventArgs
    {
        #region Fields

        /// <summary>
        /// クライアントのエンドポイント
        /// </summary>
        private readonly IPEndPoint _clientEndPoint;

        #endregion

        #region Ctor

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="clientEndPoint">クライアントのエンドポイント</param>
        /// <exception cref="System.ArgumentNullException"><paramref name="clientEndPoint"/> がnull の場合にスローされます。</exception>
        public WebSocketClientEventArgs(IPEndPoint clientEndPoint)
        {
            if (clientEndPoint == null) throw new ArgumentNullException(nameof(clientEndPoint));
            _clientEndPoint = clientEndPoint;
        }

        #endregion

        #region Properties

        /// <summary>
        /// クライアントのエンドポイントを取得します。
        /// </summary>
        public IPEndPoint ClientEndPoint => _clientEndPoint;

        #endregion
    }
}