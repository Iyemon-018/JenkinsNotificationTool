namespace JenkinsNotification.Core.Logs
{
    using System;
    using NLog;
    using LogLevel = JenkinsNotification.Core.LogLevel;

    /// <summary>
    /// NLog によるログ出力機能クラスです。
    /// </summary>
    /// <seealso cref="ILogger" />
    public class NLogger : ILogger
    {
        #region Fields

        /// <summary>
        /// NLog によるログ出力機能
        /// </summary>
        private readonly Logger _logger;

        #endregion

        #region Ctor

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public NLogger()
        {
            _logger = NLog.LogManager.GetCurrentClassLogger();
        }

        #endregion

        #region Methods

        /// <summary>
        /// ログを出力します。
        /// </summary>
        /// <param name="level">出力レベル</param>
        /// <param name="message">出力メッセージ</param>
        /// <exception cref="System.ArgumentOutOfRangeException"><paramref name="level"/> の機能が定義されていない場合にスローされます。</exception>
        public void Write(LogLevel level, string message)
        {
            switch (level)
            {
                case LogLevel.Trace:
                    _logger.Trace(message);
                    break;
                case LogLevel.Debug:
                    _logger.Debug(message);
                    break;
                case LogLevel.Information:
                    _logger.Info(message);
                    break;
                case LogLevel.Warning:
                    _logger.Warn(message);
                    break;
                case LogLevel.Error:
                    _logger.Error(message);
                    break;
                case LogLevel.Fatal:
                    _logger.Fatal(message);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(level), level, null);
            }
        }

        #endregion
    }
}