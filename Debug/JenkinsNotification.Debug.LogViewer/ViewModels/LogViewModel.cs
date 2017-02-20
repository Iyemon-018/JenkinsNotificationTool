namespace JenkinsNotification.Debug.LogViewer.ViewModels
{
    using System.Collections.ObjectModel;
    using Core;
    using Core.Communicators;
    using Core.ComponentModels;
    using Core.Services;
    using Core.Utility;
    using Microsoft.Practices.Prism.Commands;

    public class LogViewModel : ShellViewModelBase
    {
        #region Fields

        /// <summary>
        /// 全てのログを表示するかどうか
        /// </summary>
        private bool _isDisplayAllLogLevel;

        #endregion

        #region Ctor

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public LogViewModel() : this(null, null, null)
        {

        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="servicesProvider">サービス提供インターフェース</param>
        /// <param name="communicatorProvider">通信インターフェース</param>
        /// <param name="dataStore">データ蓄積領域</param>
        public LogViewModel(IServicesProvider servicesProvider, ICommunicatorProvider communicatorProvider, IDataStore dataStore) : base(servicesProvider, communicatorProvider, dataStore)
        {
            LogLevels = EnumUtility.ToObservableCollection<LogLevel>();
            IsDisplayAllLogType = true;
        }

        #endregion

        #region Properties

        public DelegateCommand LogClearCommand { get; private set; }

        public ObservableCollection<LogLevel> LogLevels { get; private set; }

        /// <summary>
        /// 全てのログを表示するかどうかを設定、または取得します。
        /// </summary>
        public bool IsDisplayAllLogType
        {
            get { return _isDisplayAllLogLevel; }
            set { SetProperty(ref _isDisplayAllLogLevel, value); }
        }

        #endregion
    }
}