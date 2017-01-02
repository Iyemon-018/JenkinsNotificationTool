namespace JenkinsNotification.Core.Logs
{
    using System;

    /// <summary>
    /// コンソール出力するためのログ出力機能クラスです。
    /// </summary>
    /// <seealso cref="ILogger" />
    public class ConsoleLogger : ILogger
    {
        /// <summary>
        /// ログを出力します。
        /// </summary>
        /// <param name="level">出力レベル</param>
        /// <param name="message">出力メッセージ</param>
        public void Write(LogLevel level, string message)
        {
            Console.WriteLine($@"{DateTime.Now:yyyy/MM/dd HH:mm:ss}|[{level}]|{message}");
        }
    }
}