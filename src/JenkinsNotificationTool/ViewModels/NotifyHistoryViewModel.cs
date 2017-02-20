namespace JenkinsNotificationTool.ViewModels
{
    using System.Collections.ObjectModel;
    using System.Windows.Data;
    using JenkinsNotification.Core;
    using JenkinsNotification.Core.Communicators;
    using JenkinsNotification.Core.ComponentModels;
    using JenkinsNotification.Core.Services;
    using JenkinsNotification.Core.ViewModels.Api;
    using Prism.Commands;

    /// <summary>
    /// 受信履歴一覧View 用のViewModel 機能クラスです。
    /// </summary>
    /// <seealso cref="ShellViewModelBase" />
    public class NotifyHistoryViewModel : ShellViewModelBase
    {
        #region Fields

        /// <summary>
        /// <see cref="JobExecuteResults"/> の非同期ロックオブジェクトです。
        /// </summary>
        private readonly object _jobExecuteResultsLock = new object();

        #endregion

        #region Ctor

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <remarks>
        /// このコンストラクタはXamlデザイナー用に用意したコンストラクタです。
        /// 不要でも削除しないでください。
        /// </remarks>
        public NotifyHistoryViewModel() : this(null, null, null)
        {
            
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="servicesProvider">サービス提供インターフェース</param>
        /// <param name="communicatorProvider">通信インターフェース</param>
        /// <param name="dataStore">データ蓄積領域</param>
        public NotifyHistoryViewModel(IServicesProvider servicesProvider, ICommunicatorProvider communicatorProvider, IDataStore dataStore) : base(servicesProvider, communicatorProvider, dataStore)
        {
            BindingOperations.EnableCollectionSynchronization(JobExecuteResults, _jobExecuteResultsLock);

            ShowJobDetailCommand = new DelegateCommand<IJobExecuteResult>(ExecuteShowJobDetailCommand);
        }

        #endregion

        #region Properties

        /// <summary>
        /// ジョブ結果受信データ コレクションを取得します。
        /// </summary>
        public ObservableCollection<IJobExecuteResult> JobExecuteResults => DataStore.JobResults;

        /// <summary>
        /// ジョブの詳細情報表示コマンドを取得します。
        /// </summary>
        public DelegateCommand<IJobExecuteResult> ShowJobDetailCommand { get; private set; }

        #endregion

        #region Methods

        /// <summary>
        /// ジョブ詳細情報表示コマンドを実行します。
        /// </summary>
        /// <param name="parameter">コマンドパラメータ オブジェクト</param>
        private void ExecuteShowJobDetailCommand(IJobExecuteResult parameter)
        {
            // TODO ジョブ詳細情報をブラウザで表示する。
            throw new System.NotImplementedException();
        }

        #endregion
    }
}