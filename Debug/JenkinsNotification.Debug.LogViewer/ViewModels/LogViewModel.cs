namespace JenkinsNotification.Debug.LogViewer.ViewModels
{
    using System.Collections.ObjectModel;
    using JenkinsNotification.Core;
    using JenkinsNotification.Core.ComponentModels;
    using JenkinsNotification.Core.Services;
    using JenkinsNotification.Core.Utility;
    using Microsoft.Practices.Prism.Commands;

    public class LogViewModel : ApplicationViewModelBase
    {
        public LogViewModel() : this(null)
        {
            
        }

        public LogViewModel(IServicesProvider servicesProvider) : base(servicesProvider)
        {
            LogLevels = EnumUtility.ToObservableCollection<LogLevel>();
            IsDisplayAllLogType = true;
        }

        public DelegateCommand LogClearCommand { get; private set; }

        public ObservableCollection<LogLevel> LogLevels { get; private set; }
        
        /// <summary>
        /// 全てのログを表示するかどうか
        /// </summary>
        private bool _isDisplayAllLogLevel;

        /// <summary>
        /// 全てのログを表示するかどうかを設定、または取得します。
        /// </summary>
        public bool IsDisplayAllLogType
        {
            get { return _isDisplayAllLogLevel; }
            set { SetProperty(ref _isDisplayAllLogLevel, value); }
        }
    }
}