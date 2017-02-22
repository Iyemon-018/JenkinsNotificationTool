namespace JenkinsNotificationTool
{
    using System;
    using System.Windows;
    using System.Windows.Threading;
    using JenkinsNotification.Core;
    using JenkinsNotification.Core.Logs;
    using JenkinsNotification.Core.Utility;
    using JenkinsNotification.CustomControls;

    /// <summary>
    /// このアプリケーションのエントリポイントです。
    /// </summary>
    public partial class App : Application
    {
        #region Ctor

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public App()
        {
            //
            // 各種例外の補足イベントハンドラを登録する。
            //
            DispatcherUnhandledException                 += App_OnDispatcherUnhandledException;
            AppDomain.CurrentDomain.UnhandledException   += App_OnUnhandledException;
#if DEBUG
            AppDomain.CurrentDomain.FirstChanceException +=
                (sender, e) => LogManager.Error(JenkinsNotificationTool.Properties.Resources.FirstChanceExceptionMessage, e.Exception);
#endif
        }
        
        #endregion

        #region Methods

        /// <summary>
        /// <see cref="E:System.Windows.Application.Startup" /> イベントを発生させます。
        /// </summary>
        /// <param name="e">イベント データを格納している <see cref="T:System.Windows.StartupEventArgs" />。</param>
        protected override void OnStartup(StartupEventArgs e)
        {
            // ログ機能の初期化を行う。
            var logger = new NLogger();
            var line = new string('-', 100);

            LogManager.AddLogger(logger);
            LogManager.Info(line);
            LogManager.Info("☆☆☆  アプリケーションが起動された。 ☆☆☆");
            LogManager.Info(line);

            base.OnStartup(e);
            
            //
            // Bootstrapper による初期化処理を実行する。
            //
            var bootstrapper = new Bootstrapper();
            bootstrapper.Run();
        }

        /// <summary>
        /// 当アプリケーションのUIスレッド以外で補足できなかった例外をキャッチしたときに呼ばれるイベントハンドラです。
        /// </summary>
        /// <param name="sender">イベント送信元オブジェクト</param>
        /// <param name="e">イベント引数オブジェクト</param>
        private void App_OnDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            LogManager.Error(JenkinsNotificationTool.Properties.Resources.DispatcherUnhandledExceptionMessage, e.Exception);
            ShowExceptionMessage(e.Exception);
        }
        
        /// <summary>
        /// 当アプリケーションのUIスレッドで補足できなかった例外をキャッチしたときに呼ばれるイベントハンドラです。
        /// </summary>
        /// <param name="sender">イベント送信元オブジェクト</param>
        /// <param name="e">イベント引数オブジェクト</param>
        private void App_OnUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            var exception = e.ExceptionObject as Exception;
            LogManager.Error(JenkinsNotificationTool.Properties.Resources.UnhandledExceptionMessage, exception);
            ShowExceptionMessage(exception);
            if (e.IsTerminating)
            {
                Shutdown();
            }
        }

        /// <summary>
        /// 例外メッセージを表示します。
        /// </summary>
        /// <param name="exception">例外オブジェクト</param>
        private void ShowExceptionMessage(Exception exception)
        {
            var exceptionMessage = exception?.Message ?? string.Empty;
            MessageDialog.Show(JenkinsNotificationTool.Properties.Resources.UnhandledExceptionShowMessage
                               + Environment.NewLine
                               + exceptionMessage
                , Products.Current.Title
                , MessageBoxButton.OK
                , MessageBoxImage.Error);
        }
        
        #endregion
    }
}
