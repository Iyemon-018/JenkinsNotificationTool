namespace JenkinsNotification.Core.Communicators
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Executers;
    using Logs;

    /// <summary>
    /// WebSocket 通信時のデータフロー制御管理機能クラスです。
    /// </summary>
    /// <remarks>
    /// WebSocket から取得したデータの制御は全てここで実行します。
    /// </remarks>
    /// <seealso cref="IWebSocketDataFlow" />
    public class WebSocketDataFlow : IWebSocketDataFlow
    {
        #region Fields

        /// <summary>
        /// 接続成功時に実行するタスク
        /// </summary>
        private readonly List<IExecuter> _connectedTasks;

        /// <summary>
        /// 接続失敗時に実行するタスク
        /// </summary>
        private readonly List<IExecuter> _connectionFailedTasks;

        /// <summary>
        /// データ受信時に実行するタスク
        /// </summary>
        private readonly List<IExecuter> _executeTasks;

        /// <summary>
        /// WebSocket 通信インターフェース
        /// </summary>
        private readonly IWebSocketCommunicator _webSocketCommunicator;

        #endregion

        #region Ctor

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="webSocketCommunicator">WebSocket通信インターフェース</param>
        /// <exception cref="System.ArgumentNullException">
        /// <paramref name="webSocketCommunicator"/> がnull の場合にスローされます。
        /// </exception>
        public WebSocketDataFlow(IWebSocketCommunicator webSocketCommunicator)
        {
            if (webSocketCommunicator == null) throw new ArgumentNullException(nameof(webSocketCommunicator));

            _executeTasks          = new List<IExecuter>();
            _connectedTasks        = new List<IExecuter>();
            _connectionFailedTasks = new List<IExecuter>();
            _webSocketCommunicator = webSocketCommunicator;
        }

        #endregion

        #region Methods

        /// <summary>
        /// データフロー管理機能の初期化構成を行います。
        /// </summary>
        public void ConfigureRegistration()
        {
            LogManager.Info("データフロー管理機能の初期化構成を行う。");
            _webSocketCommunicator.Connected        += WebSocket_OnConnected;
            _webSocketCommunicator.ConnectionFailed += WebSocket_OnConnectionFailed;
            _webSocketCommunicator.Received         += WebSocket_OnReceived;
        }

        /// <summary>
        /// WebSocket 接続成功時のタスクを登録します。
        /// </summary>
        /// <param name="task">通信成功時に実行するタスク</param>
        /// <exception cref="System.ArgumentNullException"><paramref name="task"/> がnull の場合にスローされます。</exception>
        public void RegisterConnectedTask(IExecuter task)
        {
            if (task == null) throw new ArgumentNullException(nameof(task));
            _connectedTasks.Add(task);
        }

        /// <summary>
        /// WebSocket 接続失敗時のタスクを登録します。
        /// </summary>
        /// <param name="task">通信失敗時に実行するタスク</param>
        /// <exception cref="System.ArgumentNullException"><paramref name="task"/> がnull の場合にスローされます。</exception>
        public void RegisterConnectionFailedTask(IExecuter task)
        {
            if (task == null) throw new ArgumentNullException(nameof(task));
            _connectionFailedTasks.Add(task);
        }

        /// <summary>
        /// データ受信時のタスクを登録します。
        /// </summary>
        /// <param name="task">データ受信時に実行するタスク</param>
        /// <exception cref="System.ArgumentNullException">
        /// <paramref name="task"/> がnull の場合にスローされます。
        /// </exception>
        public void RegisterExecuteTask(IExecuter task)
        {
            if (task == null) throw new ArgumentNullException(nameof(task));
            _executeTasks.Add(task);
        }

        /// <summary>
        /// WebSocket 通信でバイト配列データを受信しました。
        /// </summary>
        /// <param name="data">受信データ</param>
        private void OnReceivedData(byte[] data)
        {
            if (!_executeTasks.Any()) return;

            var task = _executeTasks.FirstOrDefault(x => x.CanExecute(data));
            if (task == null)
            {
                LogManager.Info($"実行可能なタスクはありませんでした。(受信データ:{string.Join(", ", data.Select(x => $"0x{x:X2}"))})");
                return;
            }

            task.Execute();
        }

        /// <summary>
        /// WebSocket 通信でメッセージデータを受信しました。
        /// </summary>
        /// <param name="message">受信メッセージ</param>
        private void OnReceivedMessage(string message)
        {
            if (!_executeTasks.Any()) return;

            var tasks = _executeTasks.Where(x => x.CanExecute(message)).ToArray();
            if (!tasks.Any())
            {
                LogManager.Info($"実行可能なタスクはありませんでした。(受信データ:{message})");
                return;
            }

            foreach (var executer in tasks)
            {
                executer.Execute();
            }
        }

        /// <summary>
        /// WebSocket 通信で接続が確立された際に呼び出されるイベントハンドラです。
        /// </summary>
        /// <param name="sender">イベント送信元オブジェクト</param>
        /// <param name="e">イベント引数オブジェクト</param>
        private void WebSocket_OnConnected(object sender, EventArgs e)
        {
            LogManager.Info($"{_webSocketCommunicator.Uri} との接続を確立した。");
        }

        /// <summary>
        /// WebSocket 通信で接続が失敗した際に呼び出されるイベントハンドラです。
        /// </summary>
        /// <param name="sender">イベント送信元オブジェクト</param>
        /// <param name="e">イベント引数オブジェクト</param>
        private void WebSocket_OnConnectionFailed(object sender, EventArgs e)
        {
            LogManager.Info($"{_webSocketCommunicator.Uri} との接続確立に失敗した。");
        }

        /// <summary>
        /// WebSocket 通信でデータを受信した際に呼び出されるイベントハンドラです。
        /// </summary>
        /// <param name="sender">イベント送信元オブジェクト</param>
        /// <param name="e">イベント引数オブジェクト</param>
        private void WebSocket_OnReceived(object sender, ReceivedEventArgs e)
        {
            if (e.ReceivedType == ReceivedType.Message)
            {
                LogManager.Info("WebSocket 通信でメッセージデータを受信した。");
                OnReceivedMessage(e.Message);
            }
            else if (e.ReceivedType == ReceivedType.Binary)
            {
                LogManager.Info("WebSocket 通信でバイナリデータを受信した。");
                OnReceivedData(e.GetData());
            }
        }

        #endregion
    }
}