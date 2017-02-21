namespace JenkinsNotification.Core.Communicators
{
    using System;
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
        }

        #endregion

        #region Methods

        /// <summary>
        /// データフローを初期構成を行います。
        /// </summary>
        public void Configure()
        {
            // TODO WebSocket通信で受信時に実行するタスクを登録する。
            _webSocketDataFlow.RegisterExecuteTask(new JobResultExecuter(_dataStore));
        }

        #endregion
    }
}