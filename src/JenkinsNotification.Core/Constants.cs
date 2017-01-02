namespace JenkinsNotification.Core
{
    using System.Windows;
    using System.Windows.Threading;

    /// <summary>
    /// 定数リソース機能クラスです。
    /// </summary>
    public static class Constants
    {
        /// <summary>
        /// WebSocket 通信で失敗した場合にリトライする最大回数です。
        /// </summary>
        public static readonly int WebSocketRetryMaximum = 3;

        private static Dispatcher _rootDispatcher = null;

        public static Dispatcher RootDispatcher
        {
            get
            {
                return _rootDispatcher
                       ?? (_rootDispatcher = Application.Current != null
                           ? Application.Current.Dispatcher
                           : Dispatcher.CurrentDispatcher);
            }
            internal set { _rootDispatcher = value; }
        }
    }
}