namespace JenkinsNotification.Core.Communicators
{
    using System;

    /// <summary>
    /// WebSocket 通信でエラーを受信した際のイベント引数クラスです。
    /// </summary>
    /// <seealso cref="System.EventArgs" />
    public class ReceivedErrorEventArgs : EventArgs
    {
        #region Ctor

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="exception">The exception.</param>
        public ReceivedErrorEventArgs(Exception exception)
        {
            Exception = exception;
        }

        #endregion

        #region Properties

        /// <summary>
        /// キャッチした例外インスタンスを取得します。
        /// </summary>
        public Exception Exception { get; private set; }

        #endregion
    }
}