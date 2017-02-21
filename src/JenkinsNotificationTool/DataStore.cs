namespace JenkinsNotificationTool
{
    using System;
    using System.Collections.ObjectModel;
    using JenkinsNotification.Core;
    using JenkinsNotification.Core.Configurations;
    using JenkinsNotification.Core.ViewModels.Api;

    /// <summary>
    /// データ蓄積領域クラスです。
    /// </summary>
    /// <remarks>
    /// このアプリケーションで共通利用するデータは全てここで管理します。
    /// </remarks>
    /// <seealso cref="IDataStore" />
    public class DataStore : IDataStore
    {
        #region Fields

        /// <summary>
        /// アプリケーション構成情報
        /// </summary>
        private readonly ApplicationConfiguration _applicationConfiguration;

        /// <summary>
        /// ジョブ実行結果コレクション
        /// </summary>
        private readonly ObservableCollection<IJobExecuteResult> _jobResults;

        #endregion

        #region Ctor

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="applicationConfiguration">アプリケーション構成情報</param>
        /// <exception cref="System.ArgumentNullException"><paramref name="applicationConfiguration"/> がnull の場合にスローされます。</exception>
        public DataStore(ApplicationConfiguration applicationConfiguration)
        {
            if (applicationConfiguration == null) throw new ArgumentNullException(nameof(applicationConfiguration));

            _applicationConfiguration = applicationConfiguration;
            _jobResults = new ObservableCollection<IJobExecuteResult>();
        }

        #endregion

        #region Properties

        /// <summary>
        /// アプリケーション構成情報を取得します。
        /// </summary>
        public ApplicationConfiguration ApplicationConfiguration => _applicationConfiguration;

        /// <summary>
        /// Jenkins 実行結果コレクションを取得します。
        /// </summary>
        public ObservableCollection<IJobExecuteResult> JobResults => _jobResults;

        #endregion

        #region Methods

        /// <summary>
        /// 蓄積しているデータをバックアップします。
        /// </summary>
        /// <exception cref="System.NotImplementedException">TODO</exception>
        public void Backup()
        {
            // TODO ジョブ実行結果を保存する。保存数はアプリケーション構成情報にある。
            throw new NotImplementedException();
        }
        
        /// <summary>
        /// バックアップしたデータをリストアします。
        /// </summary>
        /// <exception cref="System.NotImplementedException">TODO</exception>
        public void Restore()
        {
            // TODO 所定のフォルダに有る一時データ蓄積ファイル(xml)から保存した情報を復元する。
            //      復元するのはジョブ実行結果だけでいい。
            throw new NotImplementedException();
        }

        #endregion
    }
}