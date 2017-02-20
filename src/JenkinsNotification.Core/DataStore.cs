namespace JenkinsNotification.Core
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using JenkinsNotification.Core.Configurations;
    using ViewModels.Api;
    using Microsoft.Practices.Prism;

    /// <summary>
    /// データ蓄積機能クラスです。
    /// </summary>
    /// <seealso cref="JenkinsNotification.Core.IDataStore" />
    //public sealed class DataStore : IDataStore
    //{
    //    #region Const

    //    /// <summary>
    //    /// 唯一のインスタンス
    //    /// </summary>
    //    private static readonly DataStore _instance = new DataStore();

    //    /// <summary>
    //    /// アプリケーション構成情報
    //    /// </summary>
    //    private readonly ApplicationConfiguration _applicationConfiguration;

    //    #endregion

    //    #region Ctor

    //    /// <summary>
    //    /// コンストラクタ
    //    /// </summary>
    //    private DataStore()
    //    {
    //        JobResults = new ObservableCollection<IJobExecuteResult>();
    //    }

    //    #endregion

    //    #region Properties

    //    /// <summary>
    //    /// 唯一のインスタンスを取得します。
    //    /// </summary>
    //    public static DataStore Instance => _instance;

    //    /// <summary>
    //    /// Jenkins 実行結果コレクションを取得します。
    //    /// </summary>
    //    public ObservableCollection<IJobExecuteResult> JobResults { get; private set; }

    //    #endregion

    //    #region Methods

    //    /// <summary>
    //    /// Jenkins 実行結果を追加します。
    //    /// </summary>
    //    /// <param name="newData">追加データ</param>
    //    public void AddJobResult(IJobExecuteResult newData)
    //    {
    //        JobResults.Add(newData);
    //    }

    //    /// <summary>
    //    /// Jenkins 実行結果を追加します。
    //    /// </summary>
    //    /// <param name="newDataCollection">追加データ</param>
    //    public void AddJobResults(IEnumerable<IJobExecuteResult> newDataCollection)
    //    {
    //        JobResults.AddRange(newDataCollection);
    //    }

    //    /// <summary>
    //    /// 蓄積しているデータをバックアップします。
    //    /// </summary>
    //    public void Backup()
    //    {
    //        // TODO ファイルにバックアップする。
    //    }

    //    /// <summary>
    //    /// Jenkins 実行結果をすべてクリアします。
    //    /// </summary>
    //    public void ClearJobResults()
    //    {
    //        JobResults.Clear();
    //    }

    //    /// <summary>
    //    /// バックアップしたデータをリストアします。
    //    /// </summary>
    //    public void Restore()
    //    {
    //        // TODO ファイルからリストアする。
    //    }

    //    /// <summary>
    //    /// アプリケーション構成情報を取得します。
    //    /// </summary>
    //    public ApplicationConfiguration ApplicationConfiguration => _applicationConfiguration;

    //    #endregion
    //}
}