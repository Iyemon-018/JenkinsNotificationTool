namespace JenkinsNotification.CustomControls.ViewModels
{
    using Core.ComponentModels;
    using Core.ViewModels.Api;
    using BalloonTips;

    /// <summary>
    /// <see cref="JobExecuteResultBalloonTip"/> 用のViewModel クラスです。
    /// </summary>
    /// <seealso cref="ViewModelBase" />
    public class JobExecuteResultBalloonTipViewModel : ViewModelBase
    {
        #region Fields

        /// <summary>
        /// ジョブ実行結果
        /// </summary>
        private readonly JobExecuteResultViewModel _result;

        #endregion

        #region Ctor

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="result">ジョブ実行結果</param>
        public JobExecuteResultBalloonTipViewModel(IJobExecuteResult result)
        {
            _result = result as JobExecuteResultViewModel;
        }

        #endregion

        #region Properties

        /// <summary>
        /// ジョブ実行結果を取得します。
        /// </summary>
        public JobExecuteResultViewModel Result => _result;

        #endregion
    }
}