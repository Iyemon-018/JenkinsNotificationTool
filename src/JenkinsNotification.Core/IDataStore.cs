namespace JenkinsNotification.Core
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using JenkinsNotification.Core.Configurations;
    using JenkinsNotification.Core.ViewModels.WebApi;
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
        /// Jenkins の持つジョブ コレクションを取得します。
        /// </summary>
        ObservableCollection<JobViewModel> Jobs { get; }

        /// <summary>
        /// アプリケーション構成情報を取得します。
        /// </summary>
        ApplicationConfiguration ApplicationConfiguration { get; }

        #endregion

        #region Methods
        
        /// <summary>
        /// 蓄積しているデータをバックアップします。
        /// </summary>
        void Backup();
        
        /// <summary>
        /// バックアップしたデータをリストアします。
        /// </summary>
        void Restore();

        #endregion
    }
}