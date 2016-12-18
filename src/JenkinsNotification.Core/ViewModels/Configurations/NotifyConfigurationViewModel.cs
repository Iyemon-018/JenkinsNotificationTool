namespace JenkinsNotification.Core.ViewModels.Configurations
{
    using System;
    using System.Windows.Controls.Primitives;
    using ComponentModels;
    using JenkinsNotification.Core.Configurations;

    /// <summary>
    /// <see cref="NotifyConfiguration"/> のViewModel オブジェクト クラスです。
    /// </summary>
    /// <seealso cref="ViewModelBase" />
    public class NotifyConfigurationViewModel : ViewModelBase
    {
        #region Fields

        /// <summary>
        /// 通知の受信履歴に表示する最大データ数
        /// </summary>
        private int _displayHistoryCount;

        /// <summary>
        /// ジョブ結果が成功だった場合でも通知するかどうか
        /// </summary>
        private bool _isNotifySuccess;

        /// <summary>
        /// バルーン通知のアニメーション種別
        /// </summary>
        private PopupAnimation _popupAnimationType;

        /// <summary>
        /// バルーン通知が消えるまでのタイムアウト
        /// </summary>
        private TimeSpan? _popupTimeout;

        /// <summary>
        /// 接続先のURI
        /// </summary>
        private string _targetUri;

        #endregion

        #region Properties

        /// <summary>
        /// 接続先のURIを設定、または取得します。
        /// </summary>
        public string TargetUri
        {
            get { return _targetUri; }
            set { SetProperty(ref _targetUri, value); }
        }

        /// <summary>
        /// バルーン通知のアニメーション種別を設定、または取得します。
        /// </summary>
        public PopupAnimation PopupAnimationType
        {
            get { return _popupAnimationType; }
            set { SetProperty(ref _popupAnimationType, value); }
        }

        /// <summary>
        /// バルーン通知が消えるまでのタイムアウトを設定、または取得します。
        /// </summary>
        public TimeSpan? PopupTimeout
        {
            get { return _popupTimeout; }
            set { SetProperty(ref _popupTimeout, value); }
        }

        /// <summary>
        /// 通知の受信履歴に表示する最大データ数を設定、または取得します。
        /// </summary>
        public int DisplayHistoryCount
        {
            get { return _displayHistoryCount; }
            set { SetProperty(ref _displayHistoryCount, value); }
        }

        /// <summary>
        /// ジョブ結果が成功だった場合でも通知するかどうかを設定、または取得します。
        /// </summary>
        public bool IsNotifySuccess
        {
            get { return _isNotifySuccess; }
            set { SetProperty(ref _isNotifySuccess, value); }
        }

        #endregion
    }
}