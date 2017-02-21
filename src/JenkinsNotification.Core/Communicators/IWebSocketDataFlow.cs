namespace JenkinsNotification.Core.Communicators
{
    using Executers;

    /// <summary>
    /// WebSocket通信のデータフロー管理インターフェースです。
    /// </summary>
    /// <remarks>
    /// <see cref="ConfigureRegistration"/> メソッドを呼び出すことでデータフロー制御を開始します。<para/>
    /// データフローにタスクを登録するには、<see cref="RegisterExecuteTask"/>、<see cref="RegisterConnectedTask"/>、<see cref="RegisterConnectionFailedTask"/> を使用します。<para/>
    /// </remarks>
    public interface IWebSocketDataFlow
    {
        /// <summary>
        /// データフロー管理機能の初期化構成を行います。
        /// </summary>
        void ConfigureRegistration();

        /// <summary>
        /// データ受信時のタスクを登録します。
        /// </summary>
        /// <param name="task">データ受信時に実行するタスク</param>
        void RegisterExecuteTask(IExecuter task);

        /// <summary>
        /// WebSocket 接続成功時のタスクを登録します。
        /// </summary>
        /// <param name="task">通信成功時に実行するタスク</param>
        void RegisterConnectedTask(IExecuter task);

        /// <summary>
        /// WebSocket 接続失敗時のタスクを登録します。
        /// </summary>
        /// <param name="task">通信失敗時に実行するタスク</param>
        void RegisterConnectionFailedTask(IExecuter task);
    }
}