namespace Server.Simulator.ViewModels
{
    using JenkinsNotification.Core.ComponentModels;
    using JenkinsNotification.Core.Services;
    using Server.Simulator.Communicators;
    using Server.Simulator.ViewModels.Data;

    /// <summary>
    /// WebSocket 通信のメイン機能ViewModel クラスです。
    /// </summary>
    /// <seealso cref="JenkinsNotification.Core.ComponentModels.ShareViewModelBase" />
    public class ManageViewModel : ShareViewModelBase
    {
        #region Fields

        /// <summary>
        /// WebSocket のサーバー機能
        /// </summary>
        private readonly WebSocketServer _server;

        #endregion

        #region Ctor

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <remarks>
        /// このコンストラクタはXAMLデザイナー用のデフォルトコンストラクタで、それ以外には使用しないでください。
        /// </remarks>
        public ManageViewModel()
            : this(null)
        {
            
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="dialogService">ダイアログサービス</param>
        public ManageViewModel(IDialogService dialogService) : base(dialogService)
        {
            _server = new WebSocketServer();
            NotifyData = new NotifyDataViewModel();
            ConnectionSetting = new ConnectionSettingViewModel(dialogService, _server, NotifyData);
        }

        #endregion

        #region Properties

        /// <summary>
        /// 接続設定情報をまたは取得します。
        /// </summary>
        public ConnectionSettingViewModel ConnectionSetting { get; private set; }

        /// <summary>
        /// 通知情報をまたは取得します。
        /// </summary>
        public NotifyDataViewModel NotifyData { get; private set; }

        #endregion
    }
}