namespace JenkinsNotification.Core
{
    using System.Collections.Generic;
    using System.Linq;
    using Executers;
    using Logs;

    /// <summary>
    /// データ管理機能クラスです。
    /// </summary>
    /// <seealso cref="IDataManager" />
    public class DataManager : IDataManager
    {
        #region Ctor

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public DataManager()
        {
            Tasks = new List<IExecuter>();
        }

        #endregion

        #region Properties

        /// <summary>
        /// タスク コレクションを取得します。
        /// </summary>
        private List<IExecuter> Tasks { get; }

        #endregion

        #region Methods

        /// <summary>
        /// タスクを追加します。
        /// </summary>
        /// <param name="task">タスク</param>
        public void AddTask(IExecuter task)
        {
            Tasks.Add(task);
        }

        /// <summary>
        /// タスクを追加します。
        /// </summary>
        /// <param name="tasks">タスク</param>
        public void AddTasks(IEnumerable<IExecuter> tasks)
        {
            Tasks.AddRange(tasks);
        }

        /// <summary>
        /// データを受信したことを通知します。
        /// </summary>
        /// <param name="data">受信データ</param>
        public void ReceivedData(byte[] data)
        {
            LogManager.Info("バイナリデータを受信した。");
            var task = Tasks.FirstOrDefault(x => x.CanExecute(data));
            if (task == null)
            {
                LogManager.Info("実行可能なタスクはありませんでした。");
            }
            else
            {
                task.Execute();
            }
        }

        /// <summary>
        /// メッセージを受信したことを通知します。
        /// </summary>
        /// <param name="message">受信メッセージ</param>
        public void ReceivedMessage(string message)
        {
            LogManager.Info("メッセージデータを受信した。");
            var task = Tasks.FirstOrDefault(x => x.CanExecute(message));
            if (task == null)
            {
                LogManager.Info("実行可能なタスクはありませんでした。");
            }
            else
            {
                task.Execute();
            }
        }

        #endregion
    }
}