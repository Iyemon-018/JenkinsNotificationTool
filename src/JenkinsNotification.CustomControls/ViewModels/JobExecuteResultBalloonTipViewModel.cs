namespace JenkinsNotification.CustomControls.ViewModels
{
    using System;
    using JenkinsNotification.Core.ComponentModels;
    using JenkinsNotification.Core.Services;
    using JenkinsNotification.Core.ViewModels.Api;
    using JenkinsNotification.CustomControls.BalloonTips;
    using Microsoft.Practices.Prism.Commands;

    /// <summary>
    /// <see cref="JobExecuteResultBalloonTip"/> 用のViewModel クラスです。
    /// </summary>
    /// <seealso cref="ViewModelBase" />
    public class JobExecuteResultBalloonTipViewModel : ViewModelBase
    {
        #region Fields

        /// <summary>
        /// バルーン通知サービスの参照
        /// </summary>
        private readonly IBalloonTipService _balloonTipService;

        /// <summary>
        /// ジョブ実行結果
        /// </summary>
        private readonly JobExecuteResultViewModel _result;

        #endregion

        #region Ctor

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="balloonTipService">バルーン通知サービス</param>
        /// <param name="result">ジョブ実行結果</param>
        public JobExecuteResultBalloonTipViewModel(IBalloonTipService balloonTipService, IJobExecuteResult result)
        {
            if (balloonTipService == null) throw new ArgumentNullException(nameof(balloonTipService));
            if (result == null) throw new ArgumentNullException(nameof(result));

            _balloonTipService = balloonTipService;
            _result            = result as JobExecuteResultViewModel;
            CloseCommand       = new DelegateCommand(ExecuteCloseCommand);
        }

        #endregion

        #region Properties

        /// <summary>
        /// ジョブ実行結果を取得します。
        /// </summary>
        public JobExecuteResultViewModel Result => _result;

        /// <summary>
        /// バルーンを閉じるコマンドを設定、または取得します。
        /// </summary>
        public DelegateCommand CloseCommand { get; private set; }

        #endregion

        #region Methods

        /// <summary>
        /// バルーン通知を閉じます。
        /// </summary>
        private void ExecuteCloseCommand()
        {
            _balloonTipService.Close();
        }

        #endregion
    }
}