namespace JenkinsNotification.Core.Communicators
{
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
        /// WebAPI通信機能を取得します。
        /// </summary>
        IWebApiCommunicator WebApiCommunicator { get; }

        /// <summary>
        /// WebSocket通信のデータフローを管理インターフェースを取得します。
        /// </summary>
        IWebSocketDataFlow WebSocketDataFlow { get; }
    }
}