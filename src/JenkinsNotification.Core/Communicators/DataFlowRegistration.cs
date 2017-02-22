namespace JenkinsNotification.Core.Communicators
{
    using System;
    using System.Collections.Generic;
    using Executers;

    /// <summary>
    /// WebSocketデータフローの実行処理を登録するためのクラスです。
    /// </summary>
    public class DataFlowRegistration
    {
        #region Fields

        /// <summary>
        /// データ蓄積領域
        /// </summary>
        private readonly IDataStore _dataStore;

        /// <summary>
        /// WebSocketデータフロー インターフェース
        /// </summary>
        private readonly IWebSocketDataFlow _webSocketDataFlow;

        /// <summary>
        /// データ受信時に実行するタスクコレクション
        /// </summary>
        private readonly List<IExecuter> _executers;

        #endregion

        #region Ctor

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="webSocketDataFlow">データフロー インターフェース</param>
        /// <param name="dataStore">データ蓄積領域</param>
        /// <exception cref="System.ArgumentNullException">
        /// <paramref name="webSocketDataFlow"/> or <paramref name="dataStore"/> がnull の場合にスローされます。
        /// </exception>
        public DataFlowRegistration(IWebSocketDataFlow webSocketDataFlow, IDataStore dataStore)
        {
            if (webSocketDataFlow == null) throw new ArgumentNullException(nameof(webSocketDataFlow));
            if (dataStore         == null) throw new ArgumentNullException(nameof(dataStore));

            _webSocketDataFlow = webSocketDataFlow;
            _dataStore         = dataStore;
            _executers         = new List<IExecuter>();
        }

        #endregion

        #region Methods

        /// <summary>
        /// データ受信時に実行するタスクを登録します。
        /// </summary>
        /// <param name="task">タスク</param>
        /// <exception cref="System.ArgumentNullException"><paramref name="task"/> がnull の場合にスローされます。</exception>
        public void AddTask(IExecuter task)
        {
            if (task == null) throw new ArgumentNullException(nameof(task));
            _executers.Add(task);
        }

        /// <summary>
        /// データフローを初期構成を行います。
        /// </summary>
        public void Configure()
        {
            // TODO WebSocket通信で受信時に実行するタスクを登録する。
            _webSocketDataFlow.RegisterExecuteTask(new JobResultExecuter(_dataStore));

            foreach (var executer in _executers)
            {
                _webSocketDataFlow.RegisterExecuteTask(executer);
            }
        }

        #endregion
    }
}