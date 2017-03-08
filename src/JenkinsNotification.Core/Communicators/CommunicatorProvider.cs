namespace JenkinsNotification.Core.Communicators
{
    using System;
    using Executers;
    using JenkinsNotification.Core.Communicators.WebApi;
    using JenkinsNotification.Core.Jenkins.WebApi;

    /// <summary>
    /// 通信関連機能を提供クラスです。
    /// </summary>
    /// <seealso cref="ICommunicatorProvider" />
    public class CommunicatorProvider : ICommunicatorProvider
    {
        #region Fields
        
        /// <summary>
        /// WebSocket通信機能
        /// </summary>
        private readonly IWebSocketCommunicator _webSocketCommunicator;

        /// <summary>
        /// WebSocket通信のデータフローを管理インターフェース
        /// </summary>
        private readonly IWebSocketDataFlow _webSocketDataFlow;

        /// <summary>
        /// Jenkins のWebAPI 管理機能
        /// </summary>
        private readonly IJenkinsWebApiManager _jenkinsWebApiManager;

        #endregion

        #region Ctor

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="webSocketCommunicator">WebSocket通信機能</param>
        /// <param name="webSocketDataFlow">WebSocket通信のデータフローを管理インターフェース</param>
        /// <param name="jenkinsWebApiManager">Jenkins のWebAPI 管理機能インターフェース</param>
        /// <exception cref="System.ArgumentNullException"><paramref name="webSocketCommunicator" /> or <paramref name="webApiCommunicator" /> or <paramref name="webSocketDataFlow" /> がnull の場合にスローされます。</exception>
        public CommunicatorProvider(IWebSocketCommunicator webSocketCommunicator, IWebSocketDataFlow webSocketDataFlow, IJenkinsWebApiManager jenkinsWebApiManager)
        {
            if (webSocketCommunicator == null) throw new ArgumentNullException(nameof(webSocketCommunicator));
            if (webSocketDataFlow     == null) throw new ArgumentNullException(nameof(webSocketDataFlow));
            if (jenkinsWebApiManager  == null) throw new ArgumentNullException(nameof(jenkinsWebApiManager));

            _webSocketCommunicator = webSocketCommunicator;
            _webSocketDataFlow     = webSocketDataFlow;
            _jenkinsWebApiManager  = jenkinsWebApiManager;
        }

        #endregion

        #region Properties
        
        /// <summary>
        /// WebSocket通信機能を取得します。
        /// </summary>
        public IWebSocketCommunicator WebSocketCommunicator => _webSocketCommunicator;

        /// <summary>
        /// WebSocket通信のデータフローを管理インターフェースを取得します。
        /// </summary>
        public IWebSocketDataFlow WebSocketDataFlow => _webSocketDataFlow;

        /// <summary>
        /// Jenkins のWebAPI 管理機能インターフェースを取得します。
        /// </summary>
        public IJenkinsWebApiManager JenkinsWebApiManager => _jenkinsWebApiManager;

        #endregion
    }
}