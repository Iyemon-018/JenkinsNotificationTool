namespace JenkinsNotification.Core
{
    /// <summary>
    /// 定数リソース機能クラスです。
    /// </summary>
    public static class Constants
    {
        /// <summary>
        /// WebSocket 通信で失敗した場合にリトライする最大回数です。
        /// </summary>
        public static readonly int WebSocketRetryMaximum = 3;
    }
}