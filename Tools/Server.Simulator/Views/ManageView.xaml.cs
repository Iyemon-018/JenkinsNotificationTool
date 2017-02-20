namespace Server.Simulator.Views
{
    using JenkinsNotification.Core.ComponentModels;
    using JenkinsNotification.CustomControls;
    using Server.Simulator.ViewModels;

    /// <summary>
    /// ManageView.xaml の相互作用ロジック
    /// </summary>
    [ViewModel(typeof(ManageViewModel))]
    public partial class ManageView : View
    {
        #region Ctor

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public ManageView()
        {
            InitializeComponent();
        }

        #endregion
    }
}
