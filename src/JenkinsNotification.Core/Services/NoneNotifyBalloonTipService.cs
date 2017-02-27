namespace JenkinsNotification.Core.Services
{
    using JenkinsNotification.Core.ViewModels.Api;

    /// <summary>
    /// バルーン通知を行わないバルーン通知サービス クラスです。
    /// </summary>
    /// <seealso cref="JenkinsNotification.Core.Services.IBalloonTipService" />
    public class NoneNotifyBalloonTipService : IBalloonTipService
    {
        /// <summary>
        /// このサービスで使用するバルーン通知オブジェクト インスタンスを設定します。
        /// </summary>
        /// <param name="balloonTip">バルーン通知オブジェクト</param>
        public void SetBalloonTip(object balloonTip)
        {
            // Do Nothing.
        }

        /// <summary>
        /// 情報通知バルーンを表示します。
        /// </summary>
        /// <param name="title">タイトル</param>
        /// <param name="message">メッセージ</param>
        public void NotifyInformation(string title, string message)
        {
            // Do Nothing.
        }

        /// <summary>
        /// 警告通知バルーンを表示します。
        /// </summary>
        /// <param name="title">タイトル</param>
        /// <param name="message">メッセージ</param>
        public void NotifyWarning(string title, string message)
        {
            // Do Nothing.
        }

        /// <summary>
        /// 異常通知バルーンを表示します。
        /// </summary>
        /// <param name="title">タイトル</param>
        /// <param name="message">メッセージ</param>
        public void NotifyError(string title, string message)
        {
            // Do Nothing.
        }

        /// <summary>
        /// ジョブ結果通知バルーンを表示します。
        /// </summary>
        /// <param name="executeResult">ジョブ実行結果</param>
        public void NotifyJobResult(IJobExecuteResult executeResult)
        {
            // Do Nothing.
        }

        /// <summary>
        /// 表示中のバルーンを閉じます。
        /// </summary>
        public void Close()
        {
            // Do Nothing.
        }
    }
}