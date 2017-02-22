namespace JenkinsNotificationTool.Services
{
    using System;
    using System.Collections.Generic;
    using System.Windows;
    using JenkinsNotification.Core.Logs;
    using JenkinsNotification.Core.Services;

    /// <summary>
    /// このアプリケーション インスタンスの制御サービス機能クラスです。
    /// </summary>
    /// <seealso cref="IApplicationService" />
    public class ApplicationService : IApplicationService
    {
        #region Fields

        /// <summary>
        /// アプリケーション カレント インスタンス
        /// </summary>
        private readonly Application _appCurrent = Application.Current;

        /// <summary>
        /// 終了時に実行するタスク コレクション
        /// </summary>
        private readonly List<Action> _shutdownTasks;

        #endregion

        #region Ctor

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public ApplicationService()
        {
            _shutdownTasks = new List<Action>();
        }

        #endregion

        #region Methods

        /// <summary>
        /// 終了時に実行するタスクを追加します。
        /// </summary>
        /// <param name="task">実行タスク</param>
        /// <exception cref="System.ArgumentNullException"><paramref name="task"/> がnull の場合にスローされます。</exception>
        public void AddShutdownTask(Action task)
        {
            if (task == null) throw new ArgumentNullException(nameof(task));
            _shutdownTasks.Add(task);
        }

        /// <summary>
        /// シャットダウンします。
        /// </summary>
        public void Shutdown()
        {
            LogManager.Info("アプリケーションの終了タスクを実行する。");
            foreach (var task in _shutdownTasks)
            {
                task.Invoke();
            }

            var line = new string('-', 100);
            LogManager.Info(line);
            LogManager.Info("☆☆☆ アプリケーションをシャットダウンする。 ☆☆☆");
            LogManager.Info(line);

            _appCurrent.Shutdown();
        }

        #endregion
    }
}