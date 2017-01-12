namespace JenkinsNotification.Core
{
    using System.Windows;
    using System.Windows.Threading;

    /// <summary>
    /// 定数リソース機能クラスです。
    /// </summary>
    public static class Constants
    {
        #region Const

        /// <summary>
        /// WebSocket 通信で失敗した場合にリトライする最大回数です。
        /// </summary>
        public static readonly int WebSocketRetryMaximum = 3;

        /// <summary>
        /// 利用可能なDispatcher オブジェクト
        /// </summary>
        private static Dispatcher _rootDispatcher;

        #endregion

        #region Properties

        /// <summary>
        /// 利用可能なDispatcher オブジェクトを取得します。
        /// </summary>
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

        #endregion
    }
}