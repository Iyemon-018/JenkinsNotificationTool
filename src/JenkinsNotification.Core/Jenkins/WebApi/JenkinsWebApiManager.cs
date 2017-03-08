namespace JenkinsNotification.Core.Jenkins.WebApi
{
    using System.Threading.Tasks;
    using JenkinsNotification.Core.Communicators.WebApi;

    /// <summary>
    /// Jenkins のWebAPI 機能管理クラスです。
    /// </summary>
    /// <seealso cref="IJenkinsWebApiManager" />
    public class JenkinsWebApiManager : IJenkinsWebApiManager
    {
        #region Fields

        /// <summary>
        /// データ蓄積領域
        /// </summary>
        private readonly IDataStore _dataStore;

        /// <summary>
        /// WebAPI 通信機能インターフェース
        /// </summary>
        private readonly IWebApiCommunicator _webApiCommunicator;

        #endregion

        #region Ctor

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="webApiCommunicator">WebAPI 通信機能インターフェース</param>
        /// <param name="dataStore">データ蓄積領域</param>
        public JenkinsWebApiManager(IWebApiCommunicator webApiCommunicator, IDataStore dataStore)
        {
            _webApiCommunicator = webApiCommunicator;
            _dataStore          = dataStore;
        }

        #endregion

        #region Methods

        /// <summary>
        /// ジョブ一覧を取得します。<para />
        /// 取得したジョブ一覧情報は、<see cref="IDataStore.Jobs" /> に格納します。
        /// </summary>
        /// <returns>非同期タスク</returns>
        public async Task GetJobList()
        {
            var task = new GetJobListApiTask(_dataStore);
            var response = await GetResponse(task);
            task.ExecuteReceivedResponse(response);
        }

        /// <summary>
        /// 指定したジョブの最終ビルド情報を取得します。<para />
        /// TODO ジョブの最終ビルドに対してどういうアクションを実行するか。
        /// </summary>
        /// <param name="jobName">ジョブ名</param>
        /// <returns>非同期タスク</returns>
        public async Task GetLastBuild(string jobName)
        {
            var task = new GetLastBuildApiTask();
            var response = await GetResponse(task);
            task.ExecuteReceivedResponse(response);
        }

        /// <summary>
        /// 指定したタスクを使用して非同期にレスポンスデータを取得します。
        /// </summary>
        /// <param name="task">実行タスク</param>
        /// <returns>レスポンスデータ</returns>
        private async Task<string> GetResponse(IWebApiTask task)
        {
            return await _webApiCommunicator.GetRequest(task.GetUrl(), task.DetectedTimeout, task.DetectedError);
        }

        #endregion
    }
}