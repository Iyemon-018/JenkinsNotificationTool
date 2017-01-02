namespace JenkinsNotification.Core.Logs
{
    /// <summary>
    /// ログ出力機能インターフェースです。
    /// </summary>
    public interface ILogger
    {
        /// <summary>
        /// ログを出力します。
        /// </summary>
        /// <param name="level">出力レベル</param>
        /// <param name="message">出力メッセージ</param>
        void Write(LogLevel level, string message);
    }
}