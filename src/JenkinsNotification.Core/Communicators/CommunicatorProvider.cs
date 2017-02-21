namespace JenkinsNotification.Core.Communicators
{
    using System;
    using Executers;

    /// <summary>
    /// 通信関連機能を提供クラスです。
    /// </summary>
    /// <seealso cref="ICommunicatorProvider" />
    public class CommunicatorProvider : ICommunicatorProvider
    {
        #region Fields

        /// <summary>
        /// WebAPI通信機能
        /// </summary>
        private readonly IWebApiCommunicator _webApiCommunicator;

        /// <summary>
        /// WebSocket通信機能
        /// </summary>
        private readonly IWebSocketCommunicator _webSocketCommunicator;

        /// <summary>
        /// WebSocket通信のデータフローを管理インターフェース
        /// </summary>
        private readonly IWebSocketDataFlow _webSocketDataFlow;

        #endregion

        #region Ctor

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="webSocketCommunicator">WebSocket通信機能</param>
        /// <param name="webApiCommunicator">WebAPI通信機能</param>
        /// <param name="webSocketDataFlow">WebSocket通信のデータフローを管理インターフェース</param>
        /// <exception cref="System.ArgumentNullException"><paramref name="webSocketCommunicator"/> or <paramref name="webApiCommunicator"/> or <paramref name="webSocketDataFlow"/> がnull の場合にスローされます。</exception>
        public CommunicatorProvider(IWebSocketCommunicator webSocketCommunicator, IWebApiCommunicator webApiCommunicator, IWebSocketDataFlow webSocketDataFlow)
        {
            if (webSocketCommunicator == null) throw new ArgumentNullException(nameof(webSocketCommunicator));
            // TODO 今の時点では未実装なので、実装してから以下のコメントアウトを解除する。
            //if (webApiCommunicator    == null) throw new ArgumentNullException(nameof(webApiCommunicator));
            if (webSocketDataFlow     == null) throw new ArgumentNullException(nameof(webSocketDataFlow));

            _webSocketCommunicator = webSocketCommunicator;
            _webApiCommunicator    = webApiCommunicator;
            _webSocketDataFlow     = webSocketDataFlow;
        }

        #endregion

        #region Properties

        /// <summary>
        /// WebAPI通信機能を取得します。
        /// </summary>
        public IWebApiCommunicator WebApiCommunicator => _webApiCommunicator;

        /// <summary>
        /// WebSocket通信機能を取得します。
        /// </summary>
        public IWebSocketCommunicator WebSocketCommunicator => _webSocketCommunicator;

        /// <summary>
        /// WebSocket通信のデータフローを管理インターフェースを取得します。
        /// </summary>
        public IWebSocketDataFlow WebSocketDataFlow => _webSocketDataFlow;

        #endregion
    }
}