namespace JenkinsNotification.Debugs.ViewModels
{
    using System;
    using System.IO;
    using System.Threading.Tasks;
    using JenkinsNotification.Core.Communicators;
    using JenkinsNotification.Core.Communicators.WebApi;
    using JenkinsNotification.Core.ComponentModels;
    using JenkinsNotification.Core.Configurations;
    using JenkinsNotification.Core.Extensions;
    using JenkinsNotification.Core.Jenkins.WebApi;
    using JenkinsNotification.Core.Services;
    using JenkinsNotification.Core.Utility;
    using JenkinsNotification.Debugs.Views;
    using JenkinsNotificationTool;
    using Microsoft.Practices.Prism.Commands;

    /// <summary>
    /// <see cref="WebApiDebugView"/> 用のViewModel クラスです。
    /// </summary>
    /// <seealso cref="JenkinsNotification.Core.ComponentModels.ViewModelBase" />
    public class WebApiDebugViewModel : ViewModelBase
    {
        private readonly CommunicatorProvider _communicatorProvider;

        private readonly DataStore _dataStore;


        /// <summary>
        /// 完了メッセージ
        /// </summary>
        private string _completedMessage;

        /// <summary>
        /// 完了メッセージを設定、または取得します。
        /// </summary>
        public string CompletedMessage
        {
            get { return _completedMessage; }
            private set { SetProperty(ref _completedMessage, $"[{DateTime.Now:yyyy/MM/dd HH:mm:ss.fff}] {value}"); }
        }

        protected IJenkinsWebApiManager JenkinsWebApiManager => _communicatorProvider.JenkinsWebApiManager;

        public WebApiDebugViewModel()
        {
            if (ViewUtility.IsDesignMode())
            {
                // デザインモード時エラー対応
                return;
            }

            // 構成ファイルが無ければ予め作っておく。
            if (!File.Exists(ApplicationConfiguration.DefaultFilePath))
            {
                var config = new ApplicationConfiguration();
                config.Serialize(ApplicationConfiguration.DefaultFilePath);
            }

            ApplicationConfiguration.LoadCurrent();
            _dataStore = new DataStore(ApplicationConfiguration.Current);

            var webSocketCommunicator = new WebSocketCommunicator();
            var webSocketDataFlow = new WebSocketDataFlow(webSocketCommunicator);
            var webApiCommunicator = new WebApiCommunicator();
            webApiCommunicator.SetCredential("kishii", "gu3b03gu3b03");
            var jenkinsWebApiManager = new JenkinsWebApiManager(webApiCommunicator, DataStore);
            _communicatorProvider = new CommunicatorProvider(webSocketCommunicator, webSocketDataFlow, jenkinsWebApiManager);

            GetJobListCommand = new DelegateCommand(ExecuteGetJobListCommand);
            GetLastBuildCommand = new DelegateCommand(ExecuteGetLastBuildCommand);
        }

        private void ExecuteGetLastBuildCommand()
        {
        }

        private async void ExecuteGetJobListCommand()
        {
            await JenkinsWebApiManager.GetJobList();

            CompletedMessage = "ジョブ一覧APIの取得に成功した。";
        }

        public DelegateCommand GetJobListCommand { get; private set; }

        public DelegateCommand GetLastBuildCommand { get; private set; }

        public DataStore DataStore => _dataStore;
    }
}