namespace JenkinsNotification.Core.ViewModels.Api
{
    /// <summary>
    /// Jenkins API のオブジェクトを生成するためのユーティリティ機能クラスです。
    /// </summary>
    public static class ApiBuilder
    {
        #region Methods

        /// <summary>
        /// ジョブ実行結果オブジェクト<see cref="IJobExecuteResult"/> を生成します。
        /// </summary>
        /// <param name="jobName">ジョブ名</param>
        /// <param name="buildNumber">ビルド番号</param>
        /// <param name="status">ジョブ状態</param>
        /// <param name="resultType">ジョブ結果</param>
        /// <returns>ジョブ実行結果</returns>
        public static IJobExecuteResult ToJobExecuteResult(string jobName, int buildNumber, JobStatus status, JobResultType resultType)
        {
            return new JobExecuteResultViewModel
                   {
                       Name = jobName,
                       BuildNumber = buildNumber,
                       Status = status,
                       Result = resultType
                   };
        }

        #endregion
    }
}