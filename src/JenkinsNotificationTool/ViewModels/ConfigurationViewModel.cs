namespace JenkinsNotificationTool.ViewModels
{
    using System;
    using System.ComponentModel;
    using JenkinsNotification.Core.ComponentModels;
    using JenkinsNotification.Core.Extensions;
    using JenkinsNotification.Core.Services;
    using JenkinsNotification.Core.ViewModels.Configurations;

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
            
            PropertyChangedEventManager.AddHandler(NotifyConfiguration, NotifyConfiguration_OnPropertyChanged, string.Empty);

            ApplicationManager.ApplicationConfiguration
                              .NotifyConfiguration
                              .Map(NotifyConfiguration);
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