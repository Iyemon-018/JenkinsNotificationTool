namespace JenkinsNotification.Debug.LogViewer
{
    using System.Windows;
    using JenkinsNotification.Core;
    using JenkinsNotification.Core.Services;
    using JenkinsNotification.CustomControls.Services;
    using JenkinsNotificationTool.Services;
    using ApplicationService = JenkinsNotification.Debug.LogViewer.Services.ApplicationService;

    /// <summary>
    /// App.xaml の相互作用ロジック
    /// </summary>
    public partial class App : Application
    {
        #region Methods

        /// <summary>
        /// <see cref="E:System.Windows.Application.Startup" /> イベントを発生させます。
        /// </summary>
        /// <param name="e">イベント データを格納している <see cref="T:System.Windows.StartupEventArgs" />。</param>
        protected override void OnStartup(StartupEventArgs e)
        {
            //
            // アプリケーションで使いまわすインジェクション サービスを設定する。
            //
            var dialogService = new DialogService();
            var viewService = new ViewService();
            var balloonTipService = new NoneNotifyBalloonTipService();
            var applicationService = new ApplicationService();
            var servicesProvider = new ServicesProvider(dialogService, viewService, balloonTipService, applicationService);
            ApplicationManager.SetDefaultViewModelLocater(servicesProvider);

            base.OnStartup(e);

            //
            // アプリケーション機能の初期化を実施する。
            //
            ApplicationManager.Initialize(null, null);
        }

        #endregion
    }
}
