namespace JenkinsNotification.Core.Jenkins.WebApi
{
    using System;
    using System.Collections.ObjectModel;
    using System.Linq;
    using Communicators.WebApi;
    using Extensions;
    using Response;
    using Utility;
    using ViewModels.WebApi;

    /// <summary>
    /// ジョブ一覧を取得するためのWebAPIタスク クラスです。
    /// </summary>
    /// <remarks>
    /// ジョブ一覧情報を取得し、<see cref="IDataStore"/> に蓄積します。
    /// </remarks>
    /// <seealso cref="IWebApiTask" />
    public class GetJobListApiTask : IWebApiTask
    {
        #region Fields

        /// <summary>
        /// データ蓄積領域の参照
        /// </summary>
        private readonly IDataStore _dataStore;

        #endregion

        #region Ctor

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="dataStore">データ蓄積領域</param>
        public GetJobListApiTask(IDataStore dataStore)
        {
            _dataStore = dataStore;
        }

        #endregion

        #region Methods

        /// <summary>
        /// API実行エラーを検出しました。
        /// </summary>
        /// <param name="exception">キャッチした例外インスタンス</param>
        public void DetectedError(Exception exception)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// API実行タイムアウトを検出しました。
        /// </summary>
        public void DetectedTimeout()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// レスポンス取得処理を実行します。
        /// </summary>
        /// <param name="response">レスポンス データ</param>
        public void ExecuteReceivedResponse(string response)
        {
            using (TimeTracer.StartNew($"WebAPI ジョブ一覧データを登録する。"))
            {
                var jobListResponse = response.JsonSerialize<JobListResponse>();
                var jobs = jobListResponse.jobs.Select(x => x.Map<JobViewModel>());
                _dataStore.Jobs.Clear();
                _dataStore.Jobs.AddRange(jobs);
            }
        }

        /// <summary>
        /// APIを実行するためのURLを取得します。
        /// </summary>
        /// <returns>URL</returns>
        public string GetUrl()
        {
            // TODO DataStoreからサーバー名とプレフィックスを取得する。
            return $"http://mc-redbull/jenkins/api/json?tree_jons";
        }

        #endregion
    }
}