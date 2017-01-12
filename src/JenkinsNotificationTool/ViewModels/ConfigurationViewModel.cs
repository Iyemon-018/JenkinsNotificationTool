namespace JenkinsNotificationTool.ViewModels
{
    using System;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.Windows.Controls.Primitives;
    using JenkinsNotification.Core;
    using JenkinsNotification.Core.ComponentModels;
    using JenkinsNotification.Core.Configurations;
    using JenkinsNotification.Core.Extensions;
    using JenkinsNotification.Core.Logs;
    using JenkinsNotification.Core.Services;
    using JenkinsNotification.Core.ViewModels.Configurations;
    using Properties;
    using Utility;
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
        /// 成功時にも通知するかどうか
        /// </summary>
        private bool _isNotifySuccess;

        /// <summary>
        /// 受信通知履歴一覧の表示数
        /// </summary>
        private NotifyHistoryCountKind _notifyHistoryCountKind;

        /// <summary>
        /// ポップアップ アニメーション種別
        /// </summary>
        private PopupAnimation _popupAnimationType;

        /// <summary>
        /// 接続対象のURI
        /// </summary>
        private string _targetUri;

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
            ApplicationManager.ApplicationConfiguration
                              .NotifyConfiguration
                              .Map(NotifyConfiguration);

            var webSocket = ApplicationManager.WebSocketCommunicator;
            if (webSocket != null)
            {
                LogManager.Info("WebSocket のイベント購読を開始する。");
                webSocket.Connected += WebSocket_OnConnected;
                webSocket.ConnectionFailed += WebSocket_OnConnectionFailed;
            }

            // コマンドの初期化
            SaveCommand           = new DelegateCommand(ExecuteSaveCommand);
            CancelCommand         = new DelegateCommand(ExecuteCancelCommand);
            TestConnectionCommand = new DelegateCommand(ExecuteTestConnectionCommand);
        }

        #endregion

        #region Properties

        /// <summary>
        /// 通知関連の構成情報を取得します。
        /// </summary>
        private NotifyConfigurationViewModel NotifyConfiguration { get; set; }

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
        public DelegateCommand TestConnectionCommand { get; private set; }

        /// <summary>
        /// 接続対象のURIを設定、または取得します。
        /// </summary>
        [Required(ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "RequiredErrorMessage")]
        public string TargetUri
        {
            get { return _targetUri; }
            set { SetProperty(ref _targetUri, value); }
        }

        /// <summary>
        /// 成功時にも通知するかどうかを設定、または取得します。
        /// </summary>
        public bool IsNotifySuccess
        {
            get { return _isNotifySuccess; }
            set { SetProperty(ref _isNotifySuccess, value); }
        }

        /// <summary>
        /// ポップアップ アニメーション種別を設定、または取得します。
        /// </summary>
        public PopupAnimation PopupAnimationType
        {
            get { return _popupAnimationType; }
            set { SetProperty(ref _popupAnimationType, value); }
        }

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

        #endregion

        #region Methods

        /// <summary>
        /// View がロードされました。
        /// </summary>
        protected override void OnLoaded()
        {
            base.OnLoaded();

            //
            // 設定ファイルから取得したデータをバインドする。
            //
            TargetUri              = NotifyConfiguration.TargetUri;
            PopupAnimationType     = NotifyConfiguration.PopupAnimationType;
            BalloonDisplayTimeKind = BalloonDisplayTimeKindConverter.Convert(NotifyConfiguration.PopupTimeout);
            NotifyHistoryCountKind = NotifyHistoryCountKindConverter.Convert(NotifyConfiguration.DisplayHistoryCount);
            IsNotifySuccess        = NotifyConfiguration.IsNotifySuccess;
        }

        /// <summary>
        /// View が閉じられました。
        /// </summary>
        protected override void OnUnloaded()
        {
            base.OnUnloaded();

            var webSocket = ApplicationManager.WebSocketCommunicator;
            if (webSocket != null)
            {
                LogManager.Info("WebSocket のイベント購読を解除する。");
                webSocket.Connected        -= WebSocket_OnConnected;
                webSocket.ConnectionFailed -= WebSocket_OnConnectionFailed;
            }
        }

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
                case nameof(TargetUri):
                    OnTargetUriChanged(TargetUri);
                    break;
                case nameof(PopupAnimationType):
                    OnPopupAnimationTypeChanged(PopupAnimationType);
                    break;
                case nameof(IsNotifySuccess):
                    OnIsNotifySuccessChanged(IsNotifySuccess);
                    break;
            }
        }

        /// <summary>
        /// キャンセルコマンドを実行します。
        /// </summary>
        private void ExecuteCancelCommand()
        {
            // TODO 画面入力項目に変化があった場合は、メッセージを表示する。
            ViewService.Close(ScreenKey.Configuration);
        }

        /// <summary>
        /// 保存コマンドを実行します。
        /// </summary>
        private void ExecuteSaveCommand()
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
        }

        /// <summary>
        /// テスト接続コマンドを実行します。
        /// </summary>
        private void ExecuteTestConnectionCommand()
        {
            ApplicationManager.TryConnectionWebSocket();
        }
        
        /// <summary>
        /// <see cref="BalloonDisplayTimeKind"/> プロパティの値が更新されました。
        /// </summary>
        /// <param name="newValue">更新後の値</param>
        private void OnBalloonDisplayTimeKindChanged(BalloonDisplayTimeKind newValue)
        {
            NotifyConfiguration.PopupTimeout = BalloonDisplayTimeKindConverter.ConvertBack(newValue);
        }

        /// <summary>
        /// <see cref="IsNotifySuccess"/> プロパティの値が更新されました。
        /// </summary>
        /// <param name="newValue">更新後の値</param>
        private void OnIsNotifySuccessChanged(bool newValue)
        {
            NotifyConfiguration.IsNotifySuccess = newValue;
        }

        /// <summary>
        /// <see cref="NotifyHistoryCountKind"/> プロパティの値が更新されました。
        /// </summary>
        /// <param name="newValue">更新後の値</param>
        private void OnNotifyHistoryCountKindChanged(NotifyHistoryCountKind newValue)
        {
            NotifyConfiguration.DisplayHistoryCount = NotifyHistoryCountKindConverter.ConvertBack(newValue);
        }

        /// <summary>
        /// <see cref="PopupAnimationType"/> プロパティの値が更新されました。
        /// </summary>
        /// <param name="newValue">更新後の値</param>
        private void OnPopupAnimationTypeChanged(PopupAnimation newValue)
        {
            NotifyConfiguration.PopupAnimationType = newValue;
        }

        /// <summary>
        /// <see cref="TargetUri"/> プロパティの値が更新されました。
        /// </summary>
        /// <param name="newValue">更新後の値</param>
        private void OnTargetUriChanged(string newValue)
        {
            NotifyConfiguration.TargetUri = newValue;
        }

        /// <summary>
        /// WebSocket 通信で接続が確立できた場合に呼ばれるイベントハンドラです。
        /// </summary>
        /// <param name="sender">イベント送信元オブジェクト</param>
        /// <param name="e">イベント引数オブジェクト</param>
        private void WebSocket_OnConnected(object sender, EventArgs e)
        {
            DialogService.ShowInformation("接続しました。");
        }

        /// <summary>
        /// WebSocket 通信で接続に失敗した場合に呼ばれるイベントハンドラです。
        /// </summary>
        /// <param name="sender">イベント送信元オブジェクト</param>
        /// <param name="e">イベント引数オブジェクト</param>
        private void WebSocket_OnConnectionFailed(object sender, EventArgs e)
        {
            DialogService.ShowError("接続に失敗しました。");
            NotificationError(nameof(TargetUri), Resources.FailedConnectionWebSocketMessage);
        }

        #endregion
    }
}