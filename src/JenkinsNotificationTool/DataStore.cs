namespace JenkinsNotificationTool
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using JenkinsNotification.Core;
    using JenkinsNotification.Core.Configurations;
    using JenkinsNotification.Core.ViewModels.Api;

    public class DataStore : IDataStore
    {
        private readonly ObservableCollection<IJobExecuteResult> _jobResults;

        private readonly ApplicationConfiguration _applicationConfiguration;

        public DataStore(ApplicationConfiguration applicationConfiguration)
        {
            _applicationConfiguration = applicationConfiguration;
            _jobResults = new ObservableCollection<IJobExecuteResult>();
        }

        /// <summary>
        /// Jenkins 実行結果コレクションを取得します。
        /// </summary>
        public ObservableCollection<IJobExecuteResult> JobResults => _jobResults;

        /// <summary>
        /// アプリケーション構成情報を取得します。
        /// </summary>
        public ApplicationConfiguration ApplicationConfiguration => _applicationConfiguration;

        /// <summary>
        /// 蓄積しているデータをバックアップします。
        /// </summary>
        /// <exception cref="System.NotImplementedException">TODO</exception>
        public void Backup()
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Jenkins 実行結果をすべてクリアします。
        /// </summary>
        /// <exception cref="System.NotImplementedException">TODO</exception>
        public void ClearJobResults()
        {
            // TODO これは消す。
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// バックアップしたデータをリストアします。
        /// </summary>
        /// <exception cref="System.NotImplementedException">TODO</exception>
        public void Restore()
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Jenkins 実行結果を追加します。
        /// </summary>
        /// <param name="newData">追加データ</param>
        public void AddJobResult(IJobExecuteResult newData)
        {
            // TODO これは消す。
        }

        /// <summary>
        /// Jenkins 実行結果を追加します。
        /// </summary>
        /// <param name="newDataCollection">追加データ</param>
        public void AddJobResults(IEnumerable<IJobExecuteResult> newDataCollection)
        {
            // TODO これは消す。
        }
    }
}