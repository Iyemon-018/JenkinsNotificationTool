namespace JenkinsNotification.Core.Jenkins.WebApi
{
    using System.Threading.Tasks;

    /// <summary>
    /// Jenkins のWebAPI 機能管理インターフェースです。
    /// </summary>
    public interface IJenkinsWebApiManager
    {
        #region Methods

        /// <summary>
        /// ジョブ一覧を取得します。<para/>
        /// 取得したジョブ一覧情報は、<see cref="IDataStore.Jobs"/> に格納します。
        /// </summary>
        /// <returns>非同期タスク</returns>
        Task GetJobList();

        /// <summary>
        /// 指定したジョブの最終ビルド情報を取得します。<para/>
        /// TODO ジョブの最終ビルドに対してどういうアクションを実行するか。
        /// </summary>
        /// <param name="jobName">ジョブ名</param>
        /// <returns>非同期タスク</returns>
        Task GetLastBuild(string jobName);

        #endregion
    }
}