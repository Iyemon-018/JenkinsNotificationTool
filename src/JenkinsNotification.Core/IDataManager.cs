namespace JenkinsNotification.Core
{
    using System.Collections.Generic;
    using Executers;

    /// <summary>
    /// データ管理機能インターフェースです。
    /// </summary>
    public interface IDataManager
    {
        /// <summary>
        /// 実行可能なタスクが存在するかどうかを取得します。
        /// </summary>
        bool HasTask { get; }

        #region Methods

        /// <summary>
        /// タスクを追加します。
        /// </summary>
        /// <param name="task">タスク</param>
        void AddTask(IExecuter task);

        /// <summary>
        /// タスクを追加します。
        /// </summary>
        /// <param name="tasks">タスク</param>
        void AddTasks(IEnumerable<IExecuter> tasks);

        /// <summary>
        /// データを受信したことを通知します。
        /// </summary>
        /// <param name="data">受信データ</param>
        void ReceivedData(byte[] data);

        /// <summary>
        /// メッセージを受信したことを通知します。
        /// </summary>
        /// <param name="message">受信メッセージ</param>
        void ReceivedMessage(string message);

        #endregion
    }
}