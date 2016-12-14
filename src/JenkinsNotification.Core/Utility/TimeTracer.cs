namespace JenkinsNotification.Core.Utility
{
    using System;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using JenkinsNotification.Core.Logs;

    /// <summary>
    /// 処理時間計測用のトレース機能クラスです。
    /// </summary>
    /// <remarks>
    /// ある処理の実行時間を計測するために使用するユーティリティ機能です。
    /// </remarks>
    /// <example>
    /// 使用する場合は以下に示すようにusingステートメントで<see cref="StartNew"/> メソッドを呼び出します。
    /// <code><![CDATA[
    /// public void ExampleMethod()
    /// {
    ///     using (TimeTracer.StartNew("Tracing heavy works."))
    ///     {
    ///         //
    ///         // 計測したい処理
    ///         //
    ///     }
    /// }
    /// ]]></code>
    /// </example>
    /// <seealso cref="System.IDisposable" />
    public sealed class TimeTracer : IDisposable
    {
        #region Fields

        /// <summary>
        /// 出力メッセージ
        /// </summary>
        private readonly string _message;

        /// <summary>
        /// 処理時間計測用のストップウォッチ
        /// </summary>
        private readonly Stopwatch _stopwatch;

        #endregion

        #region Ctor

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="message">出力するメッセージ</param>
        /// <param name="callerFilePath">呼び出し元のファイルパス</param>
        /// <param name="callerMemberName">呼び出し元のメンバー名</param>
        /// <param name="callerLineNumber">呼び出し元のファイル行数</param>
        private TimeTracer(string message,
                           string callerFilePath,
                           string callerMemberName,
                           int callerLineNumber)
        {
            _message = message;
            _stopwatch = Stopwatch.StartNew();
            LogManager.Trace($"☆☆☆ Trace Start ☆☆☆ {_message}", callerFilePath, callerMemberName, callerLineNumber);
        }

        #endregion

        #region Methods

        /// <summary>
        /// トレースを開始します。
        /// </summary>
        /// <param name="message">出力するメッセージ</param>
        /// <param name="callerFilePath">呼び出し元のファイルパス(設定不要)</param>
        /// <param name="callerMemberName">呼び出し元のメンバー名(設定不要)</param>
        /// <param name="callerLineNumber">呼び出し元のファイル行数(設定不要)</param>
        /// <returns>新規トレース インスタンス</returns>
        public static IDisposable StartNew(string message,
                                          [CallerFilePath] string callerFilePath = null,
                                          [CallerMemberName] string callerMemberName = null,
                                          [CallerLineNumber] int callerLineNumber = 0)
        {
            return new TimeTracer(message, callerFilePath, callerMemberName, callerLineNumber);
        }

        #endregion

        #region IDisposable Support

        /// <summary>
        /// 重複する呼び出しを検出するには
        /// </summary>
        private bool _disposedValue;

        /// <summary>
        /// アンマネージ リソースの解放、またはリセットに関連付けられているアプリケーション定義のタスクを実行します。
        /// </summary>
        /// <param name="disposing">true:明示的なリソースの解放, false:暗黙的なリソースの解放</param>
        private void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {
                    // マネージ状態を破棄します (マネージ オブジェクト)。
                }

                // アンマネージ リソース (アンマネージ オブジェクト) を解放し、下のファイナライザーをオーバーライドします。
                // 大きなフィールドを null に設定します。
                _stopwatch.Stop();
                LogManager.Trace($"☆☆☆ Trace End ☆☆☆ [{_stopwatch.Elapsed:c}] {_message}");

                _disposedValue = true;
            }
        }

        /// <summary>
        /// ファイナライザ
        /// </summary>
        ~TimeTracer()
        {
            // このコードを変更しないでください。クリーンアップ コードを上の Dispose(bool disposing) に記述します。
            Dispose(false);
        }

        /// <summary>
        /// アンマネージ リソースの解放またはリセットに関連付けられているアプリケーション定義のタスクを実行します。
        /// </summary>
        public void Dispose()
        {
            // このコードを変更しないでください。クリーンアップ コードを上の Dispose(bool disposing) に記述します。
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}