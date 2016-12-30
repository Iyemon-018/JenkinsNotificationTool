namespace JenkinsNotificationTool.Views
{
    using JenkinsNotification.Core.ComponentModels;
    using JenkinsNotification.CustomControls;
    using JenkinsNotificationTool.ViewModels;

    /// <summary>
    /// 通知受信履歴一覧画面クラスです。
    /// </summary>
    [ViewModel(typeof(NotifyHistoryViewModel))]
    public partial class NotifyHistoryView : View
    {
        #region Ctor

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public NotifyHistoryView()
        {
            InitializeComponent();
        }

        #endregion
    }
}
