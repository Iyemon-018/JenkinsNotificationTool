namespace Server.Simulator.ViewModels
{
    using System;
    using JenkinsNotification.Core.ComponentModels;
    using JenkinsNotification.Core.Services;
    using Communicators;
    using Data;

    /// <summary>
    /// このアプリケーション専用のViewModel 基底クラスです。
    /// </summary>
    /// <seealso cref="ShareViewModelBase" />
    public abstract class SimulatorViewModelBase : ShareViewModelBase
    {
        #region Fields

        /// <summary>
        /// 通知情報
        /// </summary>
        private readonly NotifyDataViewModel _notifyData;

        /// <summary>
        /// WebSocket のサーバー機能
        /// </summary>
        private readonly WebSocketServer _server;

        #endregion

        #region Ctor

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="dialogService">ダイアログサービス</param>
        /// <param name="server">WebSocket サーバー機能</param>
        /// <param name="notifyData">通知情報</param>
        protected SimulatorViewModelBase(IDialogService dialogService, WebSocketServer server, NotifyDataViewModel notifyData) : base(dialogService)
        {
            if (server == null) throw new ArgumentNullException(nameof(server));
            if (notifyData == null) throw new ArgumentNullException(nameof(notifyData));

            _notifyData = notifyData;
            _server     = server;

            Server.ConnectionClient += Server_OnConnectionClient;
            Server.ReceivedRequest  += Server_OnReceivedRequest;
            Server.ClosedClient     += Server_OnClosedClient;
        }

        #endregion

        #region Properties

        /// <summary>
        /// 通知情報を取得します。
        /// </summary>
        protected NotifyDataViewModel NotifyData => _notifyData;

        /// <summary>
        /// WebSocket のサーバー機能を取得します。
        /// </summary>
        protected WebSocketServer Server => _server;

        #endregion

        #region Methods

        /// <summary>
        /// WebSocket通信中のクライアントが切断されました。
        /// </summary>
        /// <param name="sender">イベント送信元オブジェクト</param>
        /// <param name="e">イベント引数オブジェクト</param>
        protected virtual void OnClosedClient(object sender, WebSocketClientEventArgs e)
        {

        }

        /// <summary>
        /// WebSocket通信を開始したクライアントを検出しました。
        /// </summary>
        /// <param name="sender">イベント送信元オブジェクト</param>
        /// <param name="e">イベント引数オブジェクト</param>
        protected virtual void OnConnectionClient(object sender, WebSocketClientEventArgs e)
        {

        }

        /// <summary>
        /// WebSocket通信中のクライアントからメッセージを受信しました。
        /// </summary>
        /// <param name="sender">イベント送信元オブジェクト</param>
        /// <param name="e">イベント引数オブジェクト</param>
        protected virtual void OnReceivedRequest(object sender, WebSocketReceivedRequestEventArgs e)
        {

        }

        /// <summary>
        /// クライアントが切断された際に呼び出されるイベントハンドラです。
        /// </summary>
        /// <param name="sender">イベント送信元オブジェクト</param>
        /// <param name="e">イベント引数オブジェクト</param>
        private void Server_OnClosedClient(object sender, WebSocketClientEventArgs e)
        {
            OnClosedClient(sender, e);
        }

        /// <summary>
        /// クライアントが接続した際に呼び出されるイベントハンドラです。
        /// </summary>
        /// <param name="sender">イベント送信元オブジェクト</param>
        /// <param name="e">イベント引数オブジェクト</param>
        private void Server_OnConnectionClient(object sender, WebSocketClientEventArgs e)
        {
            OnConnectionClient(sender, e);
        }

        /// <summary>
        /// クライアントからメッセージを受信した際に呼び出されるイベントハンドラです。
        /// </summary>
        /// <param name="sender">イベント送信元オブジェクト</param>
        /// <param name="e">イベント引数オブジェクト</param>
        private void Server_OnReceivedRequest(object sender, WebSocketReceivedRequestEventArgs e)
        {
            OnReceivedRequest(sender, e);
        }

        #endregion
    }
}