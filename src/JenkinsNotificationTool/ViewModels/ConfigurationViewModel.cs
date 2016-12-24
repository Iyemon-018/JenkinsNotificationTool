namespace JenkinsNotificationTool.ViewModels
{
    using System;
    using System.ComponentModel;
    using JenkinsNotification.Core;
    using JenkinsNotification.Core.Communicators;
    using JenkinsNotification.Core.ComponentModels;
    using JenkinsNotification.Core.Configurations;
    using JenkinsNotification.Core.Extensions;
    using JenkinsNotification.Core.Logs;
    using JenkinsNotification.Core.Services;
    using JenkinsNotification.Core.Utility;
    using JenkinsNotification.Core.ViewModels.Configurations;
    using JenkinsNotificationTool.Properties;
    using Microsoft.Practices.Prism.Commands;

    /// <summary>
    /// 構成情報変更用のViewModelクラスです。
    /// </summary>
    /// <seealso cref="JenkinsNotification.Core.ComponentModels.ApplicationViewModelBase" />
    public class ConfigurationViewModel : ApplicationViewModelBase
    {
        #region Fields

        /// <summary>
        /// バルーンの表示時間瀬底種別
        /// </summary>
        private BalloonDisplayTimeKind _balloonDisplayTimeKind;

        /// <summary>
        /// 受信通知履歴一覧の表示数
        /// </summary>
        private NotifyHistoryCountKind _notifyHistoryCountKind;

        #endregion

        #region Ctor

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <remarks>
        /// このコンストラクタはXamlデザイナー用に用意したコンストラクタです。
        /// 不要でも削除しないでください。
        /// </remarks>
        public ConfigurationViewModel() : this(null)
        {
            
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="servicesProvider">インジェクション サービス</param>
        public ConfigurationViewModel(IServicesProvider servicesProvider) : base(servicesProvider)
        {
            NotifyConfiguration = new NotifyConfigurationViewModel();
            AddChild(NotifyConfiguration);
            
            PropertyChangedEventManager.AddHandler(NotifyConfiguration, NotifyConfiguration_OnPropertyChanged, string.Empty);

            ApplicationManager.ApplicationConfiguration
                              .NotifyConfiguration
                              .Map(NotifyConfiguration);

            var webSocket = ApplicationManager.WebSocketCommunicator;
            if (webSocket != null)
            {
                LogManager.Info("WebSocket のイベント購読を開始する。");
                webSocket.Connected += WebSocket_OnConnected;
                webSocket.ConnectionFailed += WebSocket_OnConnectionFailed;
                webSocket.Disconnected += WebSocket_OnDisconnected;
                webSocket.Received += WebSocket_OnReceived;
                webSocket.ReceivedError += WebSocket_OnReceivedError;
            }

            // コマンドの初期化
            SaveCommand = new DelegateCommand(() =>
                                              {
                                                  // 検証実施
                                                  Validate();
                                                  if (HasErrors)
                                                  {
                                                      // エラーあり
                                                      DialogService.ShowError(Resources.ErrorConfigurationValue);
                                                  }
                                                  else
                                                  {
                                                      // 保存するか確認
                                                      if (!DialogService.ShowQuestion(Resources.QuestionSaveConfiguration)) return;

                                                      // ファイルへの保存を実行する。
                                                      NotifyConfiguration.Map(ApplicationManager.ApplicationConfiguration.NotifyConfiguration);
                                                      var canSaved = ApplicationConfiguration.SaveCurrent();
                                                      if (canSaved)
                                                      {
                                                          // 保存成功
                                                          DialogService.ShowInformation(Resources.InformationSaveConfiguration);
                                                          ViewService.Close(ScreenKey.Configuration);
                                                      }
                                                      else
                                                      {
                                                          // 保存失敗
                                                          DialogService.ShowError(Resources.FailedSaveConfiguration);
                                                      }
                                                  }
                                              });

            CancelCommand = new DelegateCommand(() =>
                                                {
                                                    // TODO 画面入力項目に変化があった場合は、メッセージを表示する。
                                                    ViewService.Close(ScreenKey.Configuration);
                                                });

            TestConnection = new DelegateCommand(() =>
                                                 {
                                                     ApplicationManager.WebSocketCommunicator
                                                                       .Connection(NotifyConfiguration.TargetUri, 3);
                                                 });
        }

        private void WebSocket_OnReceivedError(object sender, ReceivedErrorEventArgs e)
        {
            //ThreadUtility.ExecuteUiThread(
            //    () => DialogService.ShowError("サーバーからエラーを受信しました。" +
            //                                 $"{Environment.NewLine}" +
            //                                 $"{e.Exception.Message}"));
        }

        private void WebSocket_OnConnected(object sender, EventArgs e)
        {
            DialogService.ShowInformation("接続しました。");
        }

        private void WebSocket_OnConnectionFailed(object sender, EventArgs e)
        {
            DialogService.ShowError("接続に失敗しました。");
        }

        private void WebSocket_OnDisconnected(object sender, EventArgs e)
        {
            //ThreadUtility.ExecuteUiThread(() => DialogService.ShowInformation("切断しました。"));
        }

        private void WebSocket_OnReceived(object sender, ReceivedEventArgs e)
        {
            DialogService.ShowInformation($"受信しました。{e.ReceivedType}");
        }

        protected override void OnUnloaded()
        {
            base.OnUnloaded();

            var webSocket = ApplicationManager.WebSocketCommunicator;
            if (webSocket != null)
            {
                LogManager.Info("WebSocket のイベント購読を解除する。");
                webSocket.Connected -= WebSocket_OnConnected;
                webSocket.ConnectionFailed -= WebSocket_OnConnectionFailed;
                webSocket.Disconnected -= WebSocket_OnDisconnected;
                webSocket.Received -= WebSocket_OnReceived;
                webSocket.ReceivedError -= WebSocket_OnReceivedError;
            }
        }

        #endregion

        #region Properties

        /// <summary>
        /// 通知関連の構成情報を取得します。
        /// </summary>
        public NotifyConfigurationViewModel NotifyConfiguration { get; private set; }

        /// <summary>
        /// バルーンの表示時間瀬底種別を設定、または取得します。
        /// </summary>
        public BalloonDisplayTimeKind BalloonDisplayTimeKind
        {
            get { return _balloonDisplayTimeKind; }
            set { SetProperty(ref _balloonDisplayTimeKind, value); }
        }

        /// <summary>
        /// 受信通知履歴一覧の表示数を設定、または取得します。
        /// </summary>
        public NotifyHistoryCountKind NotifyHistoryCountKind
        {
            get { return _notifyHistoryCountKind; }
            set { SetProperty(ref _notifyHistoryCountKind, value); }
        }

        /// <summary>
        /// 構成情報保存コマンドを設定、取得します。
        /// </summary>
        public DelegateCommand SaveCommand { get; private set; }

        /// <summary>
        /// 構成情報破棄コマンドを設定、取得します。
        /// </summary>
        public DelegateCommand CancelCommand { get; private set; }

        /// <summary>
        /// テスト接続コマンドを設定、取得します。
        /// </summary>
        public DelegateCommand TestConnection { get; private set; }

        #endregion

        #region Methods

        /// <summary>
        /// このオブジェクトのプロパティの値が変更された際に呼ばれるイベントハンドラです。
        /// </summary>
        /// <param name="sender">イベント送信元オブジェクト</param>
        /// <param name="e">イベント引数オブジェクト</param>
        protected override void Self_OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.Self_OnPropertyChanged(sender, e);

            switch (e.PropertyName)
            {
                case nameof(BalloonDisplayTimeKind):
                    OnBalloonDisplayTimeKindChanged(BalloonDisplayTimeKind);
                    break;
                case nameof(NotifyHistoryCountKind):
                    OnNotifyHistoryCountKindChanged(NotifyHistoryCountKind);
                    break;
            }
        }

        /// <summary>
        /// <see cref="BalloonDisplayTimeKind"/> プロパティの値が更新されました。
        /// </summary>
        /// <param name="newValue">更新後の値</param>
        private void OnBalloonDisplayTimeKindChanged(BalloonDisplayTimeKind newValue)
        {
            TimeSpan? displayTime;
            switch (newValue)
            {
                case BalloonDisplayTimeKind.Seconds5:
                    displayTime = TimeSpan.FromSeconds(5);
                    break;
                case BalloonDisplayTimeKind.Seconds15:
                    displayTime = TimeSpan.FromSeconds(15);
                    break;
                case BalloonDisplayTimeKind.Seconds30:
                    displayTime = TimeSpan.FromSeconds(30);
                    break;
                default:
                    displayTime = null;
                    break;
            }

            NotifyConfiguration.PopupTimeout = displayTime;
        }

        /// <summary>
        /// <see cref="NotifyHistoryCountKind"/> プロパティの値が更新されました。
        /// </summary>
        /// <param name="newValue">更新後の値</param>
        private void OnNotifyHistoryCountKindChanged(NotifyHistoryCountKind newValue)
        {
            int count;
            switch (newValue)
            {
                case NotifyHistoryCountKind.Count50:
                    count = 50;
                    break;
                case NotifyHistoryCountKind.Count100:
                    count = 100;
                    break;
                case NotifyHistoryCountKind.Count200:
                    count = 200;
                    break;
                default:
                    count = 100;
                    break;
            }
            NotifyConfiguration.DisplayHistoryCount = count;
        }

        /// <summary>
        /// <see cref="NotifyConfiguration"/> のプロパティ変更
        /// </summary>
        /// <param name="sender">際に呼ばれるイベントハンドラです。</param>
        /// <param name="e">イベント引数オブジェクト</param>
        private void NotifyConfiguration_OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "PopupTimeout":
                    NotifyConfiguration_OnPopupTimeoutChanged(NotifyConfiguration.PopupTimeout);
                    break;
            }
        }

        /// <summary>
        /// <see cref="NotifyConfiguration"/> の<see cref="NotifyConfigurationViewModel.PopupTimeout"/> が変更されました。
        /// </summary>
        /// <param name="newValue">更新後の値</param>
        private void NotifyConfiguration_OnPopupTimeoutChanged(TimeSpan? newValue)
        {
            if (newValue.HasValue)
            {
                if (newValue.Value.TotalSeconds == 5.0d)
                {
                    BalloonDisplayTimeKind = BalloonDisplayTimeKind.Seconds5;
                }
                else if (newValue.Value.TotalSeconds == 15.0d)
                {
                    BalloonDisplayTimeKind = BalloonDisplayTimeKind.Seconds15;
                }
                else if (newValue.Value.TotalSeconds == 30.0d)
                {
                    BalloonDisplayTimeKind = BalloonDisplayTimeKind.Seconds30;
                }
                else
                {
                    BalloonDisplayTimeKind = BalloonDisplayTimeKind.Manual;
                }
            }
            else
            {
                    BalloonDisplayTimeKind = BalloonDisplayTimeKind.Manual;
            }
        }

        #endregion
    }
}