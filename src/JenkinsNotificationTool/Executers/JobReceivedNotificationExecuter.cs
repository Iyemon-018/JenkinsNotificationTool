namespace JenkinsNotificationTool.Executers
{
    using System;
    using System.Threading;
    using JenkinsNotification.Core;
    using JenkinsNotification.Core.Executers;
    using JenkinsNotification.Core.Extensions;
    using JenkinsNotification.Core.Jenkins.Api;
    using JenkinsNotification.Core.Logs;
    using JenkinsNotification.Core.Services;
    using JenkinsNotification.Core.Utility;
    using JenkinsNotification.Core.ViewModels.Api;
    using Microsoft.Practices.Unity;

    /// <summary>
    /// ジョブ実行結果を受信してバルーン通知するための処理クラスです。
    /// </summary>
    /// <seealso cref="IExecuter" />
    public sealed class JobReceivedNotificationExecuter : IExecuter, IDisposable
    {
        #region Fields
        
        /// <summary>
        /// ジョブ結果受信時にUIスレッド上で実行するアクション
        /// </summary>
        private readonly Action _executeAction;

        /// <summary>
        /// バルーン表示のための同期ロックオブジェクト
        /// </summary>
        private ReaderWriterLockSlim _lock;

        /// <summary>
        /// 受信したジョブ結果
        /// </summary>
        private JobExecuteResultViewModel _jobResult;

        /// <summary>
        /// サービス提供インターフェース
        /// </summary>
        private readonly IServicesProvider _servicesProvider;

        #endregion

        #region Ctor

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="servicesProvider">サービス提供インターフェース</param>
        /// <exception cref="System.ArgumentNullException"><paramref name="servicesProvider"/> がnull の場合にスローされます。</exception>
        public JobReceivedNotificationExecuter(IServicesProvider servicesProvider)
        {
            if (servicesProvider == null) throw new ArgumentNullException(nameof(servicesProvider));
            _lock             = new ReaderWriterLockSlim();
            _servicesProvider = servicesProvider;
            _executeAction    = ExecuteNotifyJobResult;
        }

        #endregion

        #region Methods

        /// <summary>
        /// タスクが実行できるかどうかを判定します。
        /// </summary>
        /// <param name="message">判定対象の文字列データ</param>
        /// <returns>true の場合、タスクを実行することができます。false の場合、タスクは実行することができません。</returns>
        public bool CanExecute(string message)
        {
            try
            {
                var jobResult = message.JsonSerialize<JobExecuteResult>();
                _jobResult = jobResult.Map<JobExecuteResultViewModel>();
            }
            catch (Exception e)
            {
                LogManager.Error("ジョブ実行結果ではない。", e);
                return false;
            }

            LogManager.Info("ジョブ実行結果を受け取った。");

            //
            // 実行結果None の場合、ジョブを開始を表すのでタスクは実行しない。
            // 実行結果がわかってからタスクを実行する。
            //
            return _jobResult.Result != JobResultType.None;
        }

        /// <summary>
        /// タスクが実行できるかどうかを判定します。
        /// </summary>
        /// <param name="data">判定対象のバイト配列データ</param>
        /// <returns>true の場合、タスクを実行することができます。false の場合、タスクは実行することができません。</returns>
        /// <exception cref="System.NotImplementedException">この機能は使用しません。</exception>
        public bool CanExecute(byte[] data)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 割り当てられているタスクを実行します。
        /// </summary>
        public void Execute()
        {
            LogManager.Info("ジョブ結果をバルーンに表示する。");
            try
            {
                _lock.EnterWriteLock();
                ThreadUtility.PostUi(_executeAction);
            }
            finally
            {
                _lock.ExitWriteLock();
            }
            LogManager.Info("ジョブ結果バルーン表示終了。");
        }

        /// <summary>
        /// 受信したジョブ結果をバルーン通知します。
        /// </summary>
        private void ExecuteNotifyJobResult()
        {
            _servicesProvider.BalloonTipService.NotifyJobResult(_jobResult);
        }

        #endregion

        #region IDisposable Support

        /// <summary>
        /// このオブジェクトが解放されたかどうか
        /// </summary>
        private bool _disposedValue;

        /// <summary>
        /// このオブジェクトを解放します。
        /// </summary>
        /// <param name="disposing">明示的に解放するかどうか</param>
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
                if (_lock != null)
                {
                    _lock.Dispose();
                    _lock = null;
                }
                _disposedValue = true;
            }
        }

        /// <summary>
        /// ファイナライザ
        /// </summary>
        ~JobReceivedNotificationExecuter()
        {
            Dispose(false);
        }

        /// <summary>
        /// アンマネージ リソースの解放またはリセットに関連付けられているアプリケーション定義のタスクを実行します。
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}