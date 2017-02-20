namespace Server.Simulator
{
    using System;
    using System.Reflection;
    using System.Windows;
    using System.Windows.Threading;
    using JenkinsNotification.Core.ComponentModels;
    using JenkinsNotification.Core.Services;
    using JenkinsNotification.CustomControls.Services;
    using Microsoft.Practices.Prism.Mvvm;

    /// <summary>
    /// このアプリケーションのエントリポイント クラスです。
    /// </summary>
    public partial class App : Application
    {
        #region Fields

        /// <summary>
        /// アプリケーション全体で使用するダイアログサービスです。
        /// </summary>
        private readonly IDialogService _dialogService = new DialogService();

        #endregion

        #region Ctor

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public App()
        {
            //
            // ハンドルされなかった例外をフックする。
            //
            DispatcherUnhandledException += App_OnDispatcherUnhandledException;
            AppDomain.CurrentDomain.UnhandledException += App_OnUnhandledException;
        }

        #endregion

        #region Methods

        /// <summary>
        /// <see cref="E:System.Windows.Application.Startup" /> イベントを発生させます。
        /// </summary>
        /// <param name="e">イベント データを格納している <see cref="T:System.Windows.StartupEventArgs" />。</param>
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            
            var bootstrapper = new Bootstrapper();
            bootstrapper.Run();
        }

        /// <summary>
        /// UIスレッドでキャッチされなかった例外がスローされた際に呼ばれるイベントハンドラです。
        /// </summary>
        /// <param name="sender">イベント送信元オブジェクト</param>
        /// <param name="e">イベント引数オブジェクト</param>
        private void App_OnDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            _dialogService.ShowError(e.Exception.Message);
        }

        /// <summary>
        /// UIスレッド以外のスレッドでキャッチされなかった例外がスローされた際に呼び出されるイベントハンドラです。
        /// </summary>
        /// <param name="sender">イベント送信元オブジェクト</param>
        /// <param name="e">イベント引数オブジェクト</param>
        private void App_OnUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            _dialogService.ShowError((e.ExceptionObject as Exception).Message);
        }

        #endregion
    }
}
