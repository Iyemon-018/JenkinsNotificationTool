namespace JenkinsNotificationTool.Views
{
    using JenkinsNotification.Core.ComponentModels;
    using JenkinsNotification.CustomControls;
    using ViewModels;

    /// <summary>
    /// 構成情報変更用のViewクラスです。
    /// </summary>
    [ViewModel(typeof(ConfigurationViewModel))]
    public partial class ConfigurationView : View
    {
        #region Ctor

        public ConfigurationView()
        {
            InitializeComponent();
        }

        #endregion
    }
}
