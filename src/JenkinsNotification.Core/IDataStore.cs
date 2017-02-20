namespace JenkinsNotification.Core
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using JenkinsNotification.Core.Configurations;
    using ViewModels.Api;

    /// <summary>
    /// データを蓄積するためのストア機能インターフェースです。
    /// </summary>
    public interface IDataStore
    {
        #region Properties

        /// <summary>
        /// Jenkins 実行結果コレクションを取得します。
        /// </summary>
        ObservableCollection<IJobExecuteResult> JobResults { get; }

        /// <summary>
        /// アプリケーション構成情報を取得します。
        /// </summary>
        ApplicationConfiguration ApplicationConfiguration { get; }

        #endregion

        #region Methods

        /// <summary>
        /// Jenkins 実行結果を追加します。
        /// </summary>
        /// <param name="newData">追加データ</param>
        void AddJobResult(IJobExecuteResult newData);

        /// <summary>
        /// Jenkins 実行結果を追加します。
        /// </summary>
        /// <param name="newDataCollection">追加データ</param>
        void AddJobResults(IEnumerable<IJobExecuteResult> newDataCollection);

        /// <summary>
        /// 蓄積しているデータをバックアップします。
        /// </summary>
        void Backup();

        /// <summary>
        /// Jenkins 実行結果をすべてクリアします。
        /// </summary>
        void ClearJobResults();

        /// <summary>
        /// バックアップしたデータをリストアします。
        /// </summary>
        void Restore();

        #endregion
    }
}