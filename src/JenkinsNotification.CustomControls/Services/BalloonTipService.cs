namespace JenkinsNotification.CustomControls.Services
{
    using System;
    using Hardcodet.Wpf.TaskbarNotification;
    using Core;
    using Core.Services;
    using Core.ViewModels.Api;
    using BalloonTips;
    using Core.Logs;
    using ViewModels;

    /// <summary>
    /// バルーン通知サービス クラスです。
    /// </summary>
    /// <seealso cref="IBalloonTipService" />
    public sealed class BalloonTipService : IBalloonTipService, IDisposable
    {
        #region Fields

        /// <summary>
        /// データ蓄積領域
        /// </summary>
        private readonly IDataStore _dataStore;

        /// <summary>
        /// バルーン通知を表示するための<see cref="TaskbarIcon"/>
        /// </summary>
        private TaskbarIcon _taskbarIcon;

        #endregion

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="dataStore">データ蓄積領域</param>
        public BalloonTipService(IDataStore dataStore)
        {
            _dataStore = dataStore;
        }
        
        #region Methods

        /// <summary>
        /// このサービスで使用するバルーン通知オブジェクト インスタンスを設定します。
        /// </summary>
        /// <param name="balloonTip">バルーン通知オブジェクト</param>
        /// <exception cref="ArgumentNullException"><paramref name="balloonTip"/> がnull の場合にスローされます。</exception>
        /// <exception cref="InvalidOperationException">既にバルーン通知オブジェクトが設定されている場合にスローされます。</exception>
        public void SetBalloonTip(object balloonTip)
        {
            if (balloonTip == null) throw new ArgumentNullException(nameof(balloonTip));
            if (_taskbarIcon != null) throw new InvalidOperationException("Initialized BalloonTip.");

            _taskbarIcon = balloonTip as TaskbarIcon;
        }

        /// <summary>
        /// 異常通知バルーンを表示します。
        /// </summary>
        /// <param name="title">タイトル</param>
        /// <param name="message">メッセージ</param>
        public void NotifyError(string title, string message)
        {
            Notify(title, message, BalloonIcon.Error);
        }

        /// <summary>
        /// 情報通知バルーンを表示します。
        /// </summary>
        /// <param name="title">タイトル</param>
        /// <param name="message">メッセージ</param>
        public void NotifyInformation(string title, string message)
        {
            Notify(title, message, BalloonIcon.Info);
        }

        /// <summary>
        /// ジョブ結果通知バルーンを表示します。
        /// </summary>
        /// <param name="executeResult">ジョブ実行結果</param>
        public void NotifyJobResult(IJobExecuteResult executeResult)
        {
            LogManager.Info($"ジョブ実行結果通知バルーンを表示する。(Job:{executeResult.Name}" +
                            $" #{executeResult.BuildNumber}, {executeResult.Status}, {executeResult.Result}");

            var viewModel          = new JobExecuteResultBalloonTipViewModel(executeResult);
            var balloon            = new JobExecuteResultBalloonTip {DataContext = viewModel,};
            var taskbarIcon        = GetTaskbarIcon();
            var popupAnimationType = _dataStore.ApplicationConfiguration
                                               .NotifyConfiguration
                                               .PopupAnimationType;
            var popupTimeout = (int?) _dataStore.ApplicationConfiguration
                                               .NotifyConfiguration
                                               .PopupTimeout?
                                               .TotalMilliseconds;
            taskbarIcon.ShowCustomBalloon(balloon, popupAnimationType, popupTimeout);
        }

        /// <summary>
        /// 警告通知バルーンを表示します。
        /// </summary>
        /// <param name="title">タイトル</param>
        /// <param name="message">メッセージ</param>
        public void NotifyWarning(string title, string message)
        {
            Notify(title, message, BalloonIcon.Warning);
        }

        /// <summary>
        /// 標準の通知バルーンを表示します。
        /// </summary>
        /// <param name="title">タイトル</param>
        /// <param name="message">メッセージ</param>
        /// <param name="symbol">表示アイコン</param>
        private void Notify(string title, string message, BalloonIcon symbol)
        {
            LogManager.Info($"バルーンメッセージを通知する。({title}[{symbol}] : {message})");

            var taskbarIcon = GetTaskbarIcon();
            taskbarIcon.ShowBalloonTip(title, message, symbol);
        }

        /// <summary>
        /// 使用可能な<see cref="TaskbarIcon"/> オブジェクトを取得します。
        /// </summary>
        /// <returns><see cref="TaskbarIcon"/> オブジェクト</returns>
        /// <exception cref="System.InvalidOperationException">使用可能な<see cref="TaskbarIcon"/> が存在しない場合にスローされます。</exception>
        private TaskbarIcon GetTaskbarIcon()
        {
            // TODO リソース管理する。
            if (_taskbarIcon == null) throw new InvalidOperationException("Not initialized BalloonTip object.");
            return _taskbarIcon;
        }

        #region IDisposable Support

        /// <summary>
        /// このオブジェクトが既に削除されたかどうか
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
                if (_taskbarIcon != null)
                {
                    _taskbarIcon.Dispose();
                    _taskbarIcon = null;
                }

                _disposedValue = true;
            }
        }
        
        /// <summary>
        /// ファイナライザ
        /// </summary>
        ~BalloonTipService()
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

        #endregion
    }
}