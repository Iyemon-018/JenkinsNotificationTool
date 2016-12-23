namespace Server.Simulator.ViewModels.Data
{
    using JenkinsNotification.Core.ComponentModels;

    /// <summary>
    /// 通知情報を管理するViewModel クラスです。
    /// </summary>
    /// <seealso cref="JenkinsNotification.Core.ComponentModels.ViewModelBase" />
    public class NotifyDataViewModel : ViewModelBase
    {
        #region Fields

        /// <summary>
        /// 現在の状態を表すメッセージ
        /// </summary>
        private string _stateMessage;

        #endregion

        #region Properties

        /// <summary>
        /// 現在の状態を表すメッセージを設定、または取得します。
        /// </summary>
        public string StateMessage
        {
            get { return _stateMessage; }
            set { SetProperty(ref _stateMessage, value); }
        }

        #endregion
    }
}