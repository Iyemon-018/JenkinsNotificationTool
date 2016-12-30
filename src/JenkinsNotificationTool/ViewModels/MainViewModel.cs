﻿namespace JenkinsNotificationTool.ViewModels
{
    using JenkinsNotification.Core;
    using JenkinsNotification.Core.ComponentModels;
    using JenkinsNotification.Core.Services;
    using Properties;
    using Microsoft.Practices.Prism.Commands;

    /// <summary>
    /// タスクトレイに格納されるメイン画面のViewModel クラスです。
    /// </summary>
    /// <seealso cref="ApplicationViewModelBase" />
    public class MainViewModel : ApplicationViewModelBase
    {
        #region Ctor

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public MainViewModel()
            : this(null)
        {
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="servicesProvider">インジェクション サービス</param>
        public MainViewModel(IServicesProvider servicesProvider) : base(servicesProvider)
        {
            //
            // 各コマンドの初期化を行う。
            //

            //
            // 終了コマンド
            // コンテキストメニューからメッセージボックスを表示すると即時クローズされてしまうので
            // メッセージ無しで終了する。
            //
            ExitCommand = new DelegateCommand(() =>
                                              {
                                                  var shutDown = DialogService.ShowQuestion(Resources.ExitConfirmMessage);
                                                  if (shutDown)
                                                  {
                                                      ApplicationManager.Shutdown();
                                                  }
                                              });

            ConfigurationCommand = new DelegateCommand(() =>
                                                       {
                                                           // TODO 構成情報画面を表示する。
                                                           ViewService.Show(ScreenKey.Configuration);
                                                       });


            ReceivedNotificationListCommand = new DelegateCommand(() =>
                                                                  {
                                                                      // TODO 通知受信履歴一覧を表示する。
                                                                      ViewService.Show(ScreenKey.NotificationHistory);
                                                                  });

            ShowBalloonCommand = new DelegateCommand(() =>
                                                     {
                                                         //DialogService.ShowProgress("進捗表示中です。", async (x) =>
                                                         //                                       {
                                                         //                                           foreach (var i in Enumerable.Range(0, 3))
                                                         //                                           {
                                                         //                                               await Task.Delay(TimeSpan.FromSeconds(1));
                                                         //                                               x.SetMessage($"{i}/3 実行しました。");
                                                         //                                           }

                                                         //                                           await Task.Delay(TimeSpan.FromSeconds(1));
                                                         //                                           x.SetMessage("完了しました。しばらくお待ちください。");
                                                         //                                           await Task.Delay(TimeSpan.FromSeconds(3));
                                                         //                                       });
                                                         BalloonTipService.NotifyInformation("Test", "テスト的にバルーン出した。");
                                                     });

        }

        #endregion

        #region Properties

        /// <summary>
        /// アプリケーション終了コマンドを設定、取得します。
        /// </summary>
        public DelegateCommand ExitCommand { get; private set; }

        /// <summary>
        /// 設定画面表示コマンドを設定、取得します。
        /// </summary>
        public DelegateCommand ConfigurationCommand { get; private set; }

        /// <summary>
        /// 通知受信履歴一覧表示コマンドを設定、取得します。
        /// </summary>
        public DelegateCommand ReceivedNotificationListCommand { get; private set; }

        public DelegateCommand ShowBalloonCommand { get; private set; }

        #endregion
    }
}