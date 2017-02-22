namespace JenkinsNotificationTool.ViewModels
{
    using JenkinsNotification.Core;
    using JenkinsNotification.Core.Communicators;
    using JenkinsNotification.Core.ComponentModels;
    using JenkinsNotification.Core.Services;
    using JenkinsNotificationTool.Properties;
    using Microsoft.Practices.Prism.Commands;

    /// <summary>
    /// タスクトレイに格納されるメイン画面のViewModel クラスです。
    /// </summary>
    /// <seealso cref="ShellViewModelBase" />
    public class MainViewModel : ShellViewModelBase
    {
        #region Ctor

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <remarks>
        /// このコンストラクタはXamlデザイナー用に用意したコンストラクタです。
        /// 不要でも削除しないでください。
        /// </remarks>
        public MainViewModel() : this(null, null, null)
        {

        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="servicesProvider">サービス提供インターフェース</param>
        /// <param name="communicatorProvider">通信インターフェース</param>
        /// <param name="dataStore">データ蓄積領域</param>
        public MainViewModel(IServicesProvider servicesProvider, ICommunicatorProvider communicatorProvider, IDataStore dataStore)
            : base(servicesProvider, communicatorProvider, dataStore)
        {
            //
            // 各コマンドの初期化を行う。
            //
            ExitCommand                     = new DelegateCommand(ExecuteExitCommand);
            ConfigurationCommand            = new DelegateCommand(ExecuteConfigurationCommand);
            ReceivedNotificationListCommand = new DelegateCommand(ExecuteReceivedNotificationListCommand);
            ShowBalloonCommand              = new DelegateCommand(ExecuteShowBalloonCommand);
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

        /// <summary>
        /// 通知バルーン表示コマンドを設定、または取得します。
        /// </summary>
        public DelegateCommand ShowBalloonCommand { get; private set; }

        #endregion

        #region Methods

        /// <summary>
        /// 構成情報表示コマンドを実行します。
        /// </summary>
        private void ExecuteConfigurationCommand()
        {
            // 構成情報画面を表示する。
            ViewService.Show(ScreenKey.Configuration);
        }

        /// <summary>
        /// 終了コマンドを実行します。
        /// </summary>
        private void ExecuteExitCommand()
        {
            var shutDown = DialogService.ShowQuestion(Resources.ExitConfirmMessage);
            if (shutDown)
            {
                // アプリケーションのシャットダウンを行う。
                ServiceProvider.ApplicationService.Shutdown();
            }
        }

        /// <summary>
        /// 受信履歴表示コマンドを実行します。
        /// </summary>
        private void ExecuteReceivedNotificationListCommand()
        {
            // 通知受信履歴一覧を表示する。
            ViewService.Show(ScreenKey.NotificationHistory);
        }

        /// <summary>
        /// 通知バルーン表示コマンドを実行します。
        /// </summary>
        private void ExecuteShowBalloonCommand()
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
        }

        #endregion
    }
}