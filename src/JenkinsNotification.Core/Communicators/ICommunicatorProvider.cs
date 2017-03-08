namespace JenkinsNotification.Core.Communicators
{
    using JenkinsNotification.Core.Communicators.WebApi;
    using JenkinsNotification.Core.Jenkins.WebApi;

    /// <summary>
    /// 通信関連機能を提供するインターフェースです。
    /// </summary>
    public interface ICommunicatorProvider
    {
        /// <summary>
        /// WebSocket通信機能を取得します。
        /// </summary>
        IWebSocketCommunicator WebSocketCommunicator { get; }
        
        /// <summary>
        /// WebSocket通信のデータフローを管理インターフェースを取得します。
        /// </summary>
        IWebSocketDataFlow WebSocketDataFlow { get; }

        /// <summary>
        /// Jenkins のWebAPI 管理機能インターフェースを取得します。
        /// </summary>
        IJenkinsWebApiManager JenkinsWebApiManager { get; }
    }
}