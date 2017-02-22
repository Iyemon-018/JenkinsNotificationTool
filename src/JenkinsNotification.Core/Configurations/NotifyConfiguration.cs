namespace JenkinsNotification.Core.Configurations
{
    using System;
    using System.Windows.Controls.Primitives;
    using System.Xml.Serialization;
    using Extensions;

    /// <summary>
    /// 通知に関する設定情報クラスです。
    /// </summary>
    [Serializable]
    public class NotifyConfiguration
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
        /// <see cref="PopupTimeout"/> のシリアライズ用文字列を設定、または取得します。
        /// </summary>
        private string _popupTimeoutValue;

        /// <summary>
        /// WebSocketの接続先URI
        /// </summary>
        private string _targetUri;

        #endregion

        #region Properties

        /// <summary>
        /// WebSocketの接続先URIを設定、または取得します。
        /// </summary>
        public string TargetUri
        {
            get { return _targetUri; }
            set { _targetUri = value; }
        }

        /// <summary>
        /// バルーン通知のアニメーション種別を設定、または取得します。
        /// </summary>
        public PopupAnimation PopupAnimationType
        {
            get { return _popupAnimationType; }
            set { _popupAnimationType = value; }
        }

        /// <summary>
        /// バルーン通知が消えるまでのタイムアウトを設定、または取得します。
        /// </summary>
        /// <remarks>
        /// null の場合、タイムアウトはありません。
        /// </remarks>
        [XmlIgnore]
        public TimeSpan? PopupTimeout
        {
            get { return _popupTimeout; }
            set
            {
                if (_popupTimeout != value)
                {
                    _popupTimeout = value;
                    OnPopupTimeoutChanged(value);
                }
            }
        }

        /// <summary>
        /// <see cref="PopupTimeout"/> のシリアライズ用文字列を設定、または取得します。
        /// </summary>
        /// <remarks>
        /// <see cref="TimeSpan"/> は、XMLシリアライザーに対応していないので、ファイルの読み書きにはこのプロパティを使用します。
        /// </remarks>
        [XmlElement("PopupTimeout")]
        public string PopupTimeoutValue
        {
            get { return _popupTimeoutValue; }
            set
            {
                if (_popupTimeoutValue != value)
                {
                    _popupTimeoutValue = value;
                    OnPopupTimeoutValueChanged(value);
                }
            }
        }

        /// <summary>
        /// 通知の受信履歴に表示する最大データ数を設定、または取得します。
        /// </summary>
        public int DisplayHistoryCount
        {
            get { return _displayHistoryCount; }
            set { _displayHistoryCount = value; }
        }

        /// <summary>
        /// ジョブ結果が成功だった場合でも通知するかどうかを設定、または取得します。
        /// </summary>
        public bool IsNotifySuccess
        {
            get { return _isNotifySuccess; }
            set { _isNotifySuccess = value; }
        }

        #endregion

        #region Methods

        /// <summary>
        /// <see cref="PopupTimeout"/> プロパティの値が更新されました。
        /// </summary>
        /// <param name="newValue">更新後の値</param>
        private void OnPopupTimeoutChanged(TimeSpan? newValue)
        {
            var value = string.Empty;
            if (newValue.HasValue)
            {
                value = newValue.Value.ToString();
            }
            _popupTimeoutValue = value;
        }

        /// <summary>
        /// <see cref="PopupTimeoutValue"/> プロパティの値が更新されました。
        /// </summary>
        /// <param name="newValue">更新後の値</param>
        private void OnPopupTimeoutValueChanged(string newValue)
        {
            _popupTimeout = newValue.ToTimeSpan(true);
        }

        #endregion
    }
}