namespace JenkinsNotificationTool.Executers
{
    using System;
    using System.Threading;
    using JenkinsNotification.Core.Executers;
    using JenkinsNotification.Core.Extensions;
    using JenkinsNotification.Core.Jenkins.Api;
    using JenkinsNotification.Core.Logs;
    using JenkinsNotification.Core.Services;
    using JenkinsNotification.Core.Utility;
    using JenkinsNotification.Core.ViewModels.Api;

    /// <summary>
    /// ジョブ実行結果を受信してバルーン通知するための処理クラスです。
    /// </summary>
    /// <seealso cref="IExecuter" />
    public class JobReceivedNotificationExecuter : IExecuter
    {
        #region Fields

        /// <summary>
        /// バルーン通知サービス
        /// </summary>
        private readonly IBalloonTipService _balloonTipService;

        /// <summary>
        /// ジョブ結果受信時にUIスレッド上で実行するアクション
        /// </summary>
        private readonly Action _executeAction;

        /// <summary>
        /// バルーン表示のための同期ロックオブジェクト
        /// </summary>
        private readonly ReaderWriterLockSlim _lock;

        /// <summary>
        /// 受信したジョブ結果
        /// </summary>
        private JobExecuteResult _jobResult;

        #endregion

        #region Ctor

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="balloonTipService">バルーン通知サービス</param>
        public JobReceivedNotificationExecuter(IBalloonTipService balloonTipService)
        {
            // TODO ここは、DIからballoonTipService を取得したい。
            _lock = new ReaderWriterLockSlim();
            _balloonTipService = balloonTipService;
            _executeAction = ExecuteNotifyJobResult;
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
                _jobResult = message.JsonSerialize<JobExecuteResult>();
            }
            catch (Exception e)
            {
                LogManager.Error("ジョブ実行結果ではない。", e);
                return false;
            }

            LogManager.Info("ジョブ実行結果を受け取った。");
            return true;
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
            var result = _jobResult.Map<JobExecuteResultViewModel>();
            _balloonTipService.NotifyJobResult(result);
        }

        #endregion
    }
}