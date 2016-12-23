namespace Server.Simulator.ViewModels
{
    using System;
    using System.Collections.ObjectModel;
    using System.Windows.Data;
    using JenkinsNotification.Core.Services;
    using Microsoft.Practices.Prism.Commands;
    using Communicators;
    using Models;
    using Data;

    /// <summary>
    /// 接続設定情報用のViewModelクラスです。
    /// </summary>
    /// <seealso cref="SimulatorViewModelBase" />
    public class ConnectionSettingViewModel : SimulatorViewModelBase
    {
        #region Fields

        /// <summary>
        /// <see cref="Requests"/> の非同期ロックオブジェクトです。
        /// </summary>
        private readonly object _requestLock = new object();

        #endregion

        #region Ctor

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="dialogService">ダイアログサービス</param>
        /// <param name="server">WebSocket サーバー機能</param>
        /// <param name="notifyData">通知情報</param>
        public ConnectionSettingViewModel(IDialogService dialogService, WebSocketServer server, NotifyDataViewModel notifyData)
            : base(dialogService, server, notifyData)
        {
            Requests = new ObservableCollection<ClientRequestViewModel>();
            BindingOperations.EnableCollectionSynchronization(Requests, _requestLock);

            // それぞれのコマンドを初期化する。
            ConnectionCommand = new DelegateCommand(async () =>
                                                    {
                                                        notifyData.StateMessage = "接続を開始しています...";
                                                        try
                                                        {
                                                            await Server.Connection(Constants.UriPrefix);
                                                        }
                                                        catch (Exception e)
                                                        {
                                                            dialogService.ShowError(e.Message);
                                                            notifyData.StateMessage = "接続に失敗しました。";
                                                        }
                                                    });

            DisconnectionCommand = new DelegateCommand(() =>
                                                       {
                                                           notifyData.StateMessage = "切断します...";
                                                           try
                                                           {
                                                               Server.Disconnection();
                                                               notifyData.StateMessage = "切断しました。";
                                                           }
                                                           catch (Exception e)
                                                           {
                                                               dialogService.ShowError(e.Message);
                                                               notifyData.StateMessage = e.Message;
                                                           }
                                                       });
        }

        #endregion

        #region Properties

        /// <summary>
        /// 接続コマンドを設定、または取得します。
        /// </summary>
        public DelegateCommand ConnectionCommand { get; private set; }

        /// <summary>
        /// 切断コマンドを設定、または取得します。
        /// </summary>
        public DelegateCommand DisconnectionCommand { get; private set; }

        /// <summary>
        /// クライアントのリクエスト コレクションをまたは取得します。
        /// </summary>
        public ObservableCollection<ClientRequestViewModel> Requests { get; private set; }

        #endregion

        #region Methods

        /// <summary>
        /// WebSocket通信中のクライアントが切断されました。
        /// </summary>
        /// <param name="sender">イベント送信元オブジェクト</param>
        /// <param name="e">イベント引数オブジェクト</param>
        protected override void OnClosedClient(object sender, WebSocketClientEventArgs e)
        {
            Requests.Add(new ClientRequestViewModel(e.ClientEndPoint, WebSocketConnectState.Disconnect));
        }

        /// <summary>
        /// WebSocket通信を開始したクライアントを検出しました。
        /// </summary>
        /// <param name="sender">イベント送信元オブジェクト</param>
        /// <param name="e">イベント引数オブジェクト</param>
        protected override void OnConnectionClient(object sender, WebSocketClientEventArgs e)
        {
            Requests.Add(new ClientRequestViewModel(e.ClientEndPoint, WebSocketConnectState.Connect));
            NotifyData.StateMessage = "クライアントとの接続を確認しました。";
        }

        /// <summary>
        /// WebSocket通信中のクライアントからメッセージを受信しました。
        /// </summary>
        /// <param name="sender">イベント送信元オブジェクト</param>
        /// <param name="e">イベント引数オブジェクト</param>
        protected override void OnReceivedRequest(object sender, WebSocketReceivedRequestEventArgs e)
        {
            Requests.Add(new ClientRequestViewModel(e.ClientEndPoint, WebSocketConnectState.Received));
        }

        #endregion
    }
}