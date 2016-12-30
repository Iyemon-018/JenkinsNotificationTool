namespace JenkinsNotificationTool.ViewModels
{
    using System.Collections.ObjectModel;
    using System.Windows.Data;
    using JenkinsNotification.Core;
    using JenkinsNotification.Core.ComponentModels;
    using JenkinsNotification.Core.Services;
    using JenkinsNotification.Core.ViewModels.Api;

    /// <summary>
    /// 受信履歴一覧View 用のViewModel 機能クラスです。
    /// </summary>
    /// <seealso cref="JenkinsNotification.Core.ComponentModels.ApplicationViewModelBase" />
    public class NotifyHistoryViewModel : ApplicationViewModelBase
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
        public NotifyHistoryViewModel() : this((IDialogService)null)
        {
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="servicesProvider">インジェクション サービス</param>
        public NotifyHistoryViewModel(IServicesProvider servicesProvider) : base(servicesProvider)
        {
            BindingOperations.EnableCollectionSynchronization(JobExecuteResults, _jobExecuteResultsLock);
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="dialogService">ダイアログサービス</param>
        public NotifyHistoryViewModel(IDialogService dialogService) : base(dialogService)
        {
            BindingOperations.EnableCollectionSynchronization(JobExecuteResults, _jobExecuteResultsLock);
        }

        #endregion

        #region Properties

        /// <summary>
        /// ジョブ結果受信データ コレクションを取得します。
        /// </summary>
        public ObservableCollection<IJobExecuteResult> JobExecuteResults => DataStore.Instance.JobResults;

        #endregion
    }
}