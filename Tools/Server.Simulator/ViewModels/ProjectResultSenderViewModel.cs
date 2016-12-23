namespace Server.Simulator.ViewModels
{
    using System.Collections.ObjectModel;
    using System.IO;
    using System.Linq;
    using System.Runtime.Serialization.Json;
    using System.Windows.Data;
    using JenkinsNotification.Core;
    using JenkinsNotification.Core.Jenkins.Api;
    using JenkinsNotification.Core.Services;
    using JenkinsNotification.Core.ViewModels.Api;
    using JenkinsNotification.Core.ViewModels.Api.Converter;
    using Microsoft.Practices.Prism.Commands;
    using Communicators;
    using Data;

    /// <summary>
    /// Jenkins ジョブ実行結果の送信機能ViewModel クラスです。
    /// </summary>
    /// <seealso cref="Server.Simulator.ViewModels.SimulatorViewModelBase" />
    public class ProjectResultSenderViewModel : SimulatorViewModelBase
    {
        #region Fields

        /// <summary>
        /// <see cref="SendJobs"/> の非同期ロックオブジェクトです。
        /// </summary>
        private readonly object _sendJobsLock = new object();

        /// <summary>
        /// ビルド番号
        /// </summary>
        private int _buildNumber;

        /// <summary>
        /// ジョブ名
        /// </summary>
        private string _jobName;

        /// <summary>
        /// ジョブの実行結果
        /// </summary>
        private JobResultType _jobResult;

        /// <summary>
        /// ジョブの実行状態
        /// </summary>
        private JobStatus _jobStatus;

        #endregion

        #region Ctor

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="dialogService">ダイアログサービス</param>
        /// <param name="server">WebSocket サーバー機能</param>
        /// <param name="notifyData">通知情報</param>
        public ProjectResultSenderViewModel(IDialogService dialogService, WebSocketServer server, NotifyDataViewModel notifyData)
            : base(dialogService, server, notifyData)
        {
            // 表示中のジョブ情報は予めクリアしておく。
            ExecuteClearCommand();

            SendJobs = new ObservableCollection<IJobExecuteResult>();
            BindingOperations.EnableCollectionSynchronization(SendJobs, _sendJobsLock);

            // 各コマンドを初期化する。
            SendCommand = new DelegateCommand(ExecuteSendCommand);
            ClearCommand = new DelegateCommand(ExecuteClearCommand);
        }

        #endregion

        #region Properties

        /// <summary>
        /// ジョブ結果送信コマンドを取得します。
        /// </summary>
        public DelegateCommand SendCommand { get; private set; }

        /// <summary>
        /// ジョブ送信情報クリア コマンドを取得します。
        /// </summary>
        public DelegateCommand ClearCommand { get; private set; }

        /// <summary>
        /// ジョブ実行結果の送信履歴コレクションを取得します。
        /// </summary>
        public ObservableCollection<IJobExecuteResult> SendJobs { get; private set; }

        /// <summary>
        /// ジョブ名を設定、または取得します。
        /// </summary>
        public string JobName
        {
            get { return _jobName; }
            set { SetProperty(ref _jobName, value); }
        }

        /// <summary>
        /// ビルド番号を設定、または取得します。
        /// </summary>
        public int BuildNumber
        {
            get { return _buildNumber; }
            set { SetProperty(ref _buildNumber, value); }
        }

        /// <summary>
        /// ジョブの実行状態を設定、または取得します。
        /// </summary>
        public JobStatus JobStatus
        {
            get { return _jobStatus; }
            set { SetProperty(ref _jobStatus, value); }
        }

        /// <summary>
        /// ジョブの実行結果を設定、または取得します。
        /// </summary>
        public JobResultType JobResult
        {
            get { return _jobResult; }
            set { SetProperty(ref _jobResult, value); }
        }

        #endregion

        #region Methods

        /// <summary>
        /// クリアコマンドを実行します。
        /// </summary>
        private void ExecuteClearCommand()
        {
            JobName     = "Test.Job";
            BuildNumber = 1;
            JobStatus   = JobStatus.Start;
            JobResult   = JobResultType.None;
        }

        /// <summary>
        /// 送信コマンドを実行します。
        /// </summary>
        private async void ExecuteSendCommand()
        {
            var jobResult = new JobExecuteResult
                            {
                                project = JobName,
                                number  = BuildNumber,
                                result  = ApiConverter.JobResultTypeToString(JobResult),
                                status  = ApiConverter.JobStatusToString(JobStatus)
                            };

            //
            // Json にシリアライズして送信する。
            //
            var serializer = new DataContractJsonSerializer(typeof(JobExecuteResult));
            byte[] sendBuffer;
            using (var ms = new MemoryStream())
            {
                serializer.WriteObject(ms, jobResult);

                // 末尾に'\0'が付加されているので取り除いておく。
                sendBuffer = ms.GetBuffer().TakeWhile(x => x != 0).ToArray();
            }
            
            await Server.SendAsync(sendBuffer);             // 送信

            // 送信結果を追加する。
            SendJobs.Add(ApiBuilder.ToJobExecuteResult(JobName, BuildNumber, JobStatus, JobResult));
        }

        #endregion
    }
}