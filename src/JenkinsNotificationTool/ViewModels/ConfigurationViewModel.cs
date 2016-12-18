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
            PropertyChangedEventManager.AddHandler(this, Self_OnPropertyChanged, string.Empty);
            PropertyChangedEventManager.AddHandler(NotifyConfiguration, NotifyConfiguration_OnPropertyChanged, string.Empty);
            ApplicationManager.ApplicationConfiguration
                              .NotifyConfiguration
                              .Map(NotifyConfiguration);
        }

        private void NotifyConfiguration_OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "PopupTimeout":
                    NotifyConfiguration_OnPopupTimeoutChanged(NotifyConfiguration.PopupTimeout);
                    break;
            }
        }

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

        private void Self_OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(BalloonDisplayTimeKind):
                    OnBalloonDisplayTimeKindChanged(BalloonDisplayTimeKind);
                    break;
            }
        }

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

        #endregion
        
        public NotifyConfigurationViewModel NotifyConfiguration { get; private set; }

        /// <summary>
        /// バルーンの表示時間瀬底種別
        /// </summary>
        private BalloonDisplayTimeKind _balloonDisplayTimeKind;

        /// <summary>
        /// バルーンの表示時間瀬底種別を設定、または取得します。
        /// </summary>
        public BalloonDisplayTimeKind BalloonDisplayTimeKind
        {
            get { return _balloonDisplayTimeKind; }
            set { SetProperty(ref _balloonDisplayTimeKind, value); }
        }
    }
}