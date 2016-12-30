﻿namespace JenkinsNotificationTool
{
    using System;
    using System.Runtime.ExceptionServices;
    using System.Windows;
    using System.Windows.Threading;
    using JenkinsNotification.Core;
    using JenkinsNotification.Core.Communicators;
    using JenkinsNotification.Core.Configurations;
    using JenkinsNotification.Core.Executers;
    using JenkinsNotification.Core.Logs;
    using JenkinsNotification.Core.Services;
    using JenkinsNotification.Core.Utility;
    using JenkinsNotification.CustomControls;
    using JenkinsNotification.CustomControls.Services;
    using JenkinsNotificationTool.Views;

    /// <summary>
    /// このアプリケーションのエントリポイントです。
    /// </summary>
    public partial class App : Application
    {
        private bool _isConnectedWebSocket = true;

        private readonly IServicesProvider _servicesProvider;

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
            AppDomain.CurrentDomain.FirstChanceException += App_OnFirstChanceException;

            _servicesProvider = new ServicesProvider(new DialogService(), new ViewService());
        }

        #endregion

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
            ViewService.RegisterView(ScreenKey.Configuration, typeof(ConfigurationView));
            ViewService.RegisterView(ScreenKey.NotificationHistory, typeof(NotifyHistoryView));
            ApplicationManager.SetDefaultViewModelLocater(_servicesProvider);

            base.OnStartup(e);

            //
            // メインウィンドウの初期化を行う。
            // タスクトレイに表示するだけでいいのでShow()は必要ない。
            // ShutdownMode="OnExplicitShutdown"
            // に設定しているのでShutdown() が唯一のアプリケーション終了方法となっている。
            //
            var view = new MainView();

            //
            // アプリケーション機能の初期化を実施する。
            //
            try
            {
                //
                // 構成ファイルを読み込む。
                //
                ApplicationConfiguration.LoadCurrent();
            }
            catch (ConfigurationLoadException)
            {
                _isConnectedWebSocket = false;
            }

            //
            // アプリケーション管理機能の初期化を行う。
            //
            var dataManager = new DataManager();
            dataManager.AddTask(new JobResultExecuter());
            ApplicationManager.Initialize(dataManager);
            ApplicationManager.InitializeBalloonTipService(new BalloonTipService(view.TaskbarIcon));

            var webSocket = new WebSocketCommunicator();
            webSocket.ConnectionFailed += WebSocket_OnConnectionFailed;
            ApplicationManager.SetWebSocketCommunicator(webSocket);
            ApplicationManager.TryConnectionWebSocket();

            if (!_isConnectedWebSocket)
            {
                _servicesProvider.ViewService.Show(ScreenKey.Configuration);
            }
        }

        private void WebSocket_OnConnectionFailed(object sender, EventArgs e)
        {
            _servicesProvider.DialogService.ShowError($"{ApplicationManager.Instance.ApplicationConfiguration.NotifyConfiguration.TargetUri} との接続に失敗しました。{Environment.NewLine}"
                                                      + "接続設定を確認してください。");
            _servicesProvider.ViewService.Show(ScreenKey.Configuration);
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
        /// 当アプリケーションで例外が発生した際に最初に呼ばれるイベントハンドラです。
        /// </summary>
        /// <param name="sender">イベント送信元オブジェクト</param>
        /// <param name="e">イベント引数オブジェクト</param>
        private void App_OnFirstChanceException(object sender, FirstChanceExceptionEventArgs e)
        {
            LogManager.Error(JenkinsNotificationTool.Properties.Resources.FirstChanceExceptionMessage, e.Exception);
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
