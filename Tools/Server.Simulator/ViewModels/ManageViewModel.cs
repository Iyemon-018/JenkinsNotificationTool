namespace Server.Simulator.ViewModels
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Text;
    using JenkinsNotification.Core.ComponentModels;
    using JenkinsNotification.Core.Services;
    using Microsoft.Practices.Prism.Commands;
    using Server.Simulator.Communicators;

    /// <summary>
    /// WebSocket 通信のメイン機能ViewModel クラスです。
    /// </summary>
    /// <seealso cref="JenkinsNotification.Core.ComponentModels.ShareViewModelBase" />
    public class ManageViewModel : ShareViewModelBase
    {
        #region Const

        /// <summary>
        /// 通信元のURIプレフィックス
        /// </summary>
        private static readonly string UriPrefix = "http://127.0.0.1:12345/";

        #endregion

        #region Fields

        /// <summary>
        /// 送信用のテキスト
        /// </summary>
        private string _remarks;

        /// <summary>
        /// WebSocket のサーバー機能
        /// </summary>
        private WebSocketServer _server;

        /// <summary>
        /// 状態メッセージ
        /// </summary>
        private string _stateMessage;

        #endregion

        #region Ctor

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <remarks>
        /// このコンストラクタはXAMLデザイナー用のデフォルトコンストラクタで、それ以外には使用しないでください。
        /// </remarks>
        public ManageViewModel()
            : this(null)
        {
            
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="dialogService">ダイアログサービス</param>
        public ManageViewModel(IDialogService dialogService) : base(dialogService)
        {
            _server = new WebSocketServer();
            Remarks = "Write send message.";

            _server.ConnectionClient += Server_OnConnectionClient;
            _server.ReceivedRequest += Server_OnReceivedRequest;
            _server.ClosedClient += Server_OnClosedClient;

            ConnectionCommand = new DelegateCommand(async () =>
                                                    {
                                                        StateMessage = "接続を開始しています...";
                                                        try
                                                        {
                                                            await _server.Connection(UriPrefix);
                                                        }
                                                        catch (Exception e)
                                                        {
                                                            dialogService.ShowError(e.Message);
                                                            StateMessage = "接続に失敗しました。";
                                                        }
                                                    });

            DisconnectionCommand = new DelegateCommand(() =>
                                                       {
                                                           StateMessage = "切断します...";
                                                           try
                                                           {
                                                               _server.Disconnection();
                                                               StateMessage = "切断しました。";
                                                           }
                                                           catch (Exception e)
                                                           {
                                                               dialogService.ShowError(e.Message);
                                                               StateMessage = e.Message;
                                                           }
                                                       });

            SendCommand = new DelegateCommand(async () =>
                                              {
                                                  StateMessage = "メッセージを送信します...";
                                                  try
                                                  {
                                                      var sendBuffer = Encoding.UTF8.GetBytes(Remarks);
                                                      await _server.SendAsync(sendBuffer);
                                                      StateMessage = "送信しました。";
                                                  }
                                                  catch (Exception exception)
                                                  {
                                                      DialogService.ShowError(exception.Message);
                                                      StateMessage = "メッセージの送信に失敗しました。";
                                                  }
                                              }
                                              , () => !HasErrors);
        }

        #endregion

        #region Properties

        /// <summary>
        /// URIプレフィックスを取得します。
        /// </summary>
        public string ConnectionUri => UriPrefix;

        /// <summary>
        /// 接続コマンドを設定、または取得します。
        /// </summary>
        public DelegateCommand ConnectionCommand { get; private set; }

        /// <summary>
        /// 切断コマンドを設定、または取得します。
        /// </summary>
        public DelegateCommand DisconnectionCommand { get; private set; }

        /// <summary>
        /// メッセージ送信コマンドを設定、または取得します。
        /// </summary>
        public DelegateCommand SendCommand { get; private set; }

        /// <summary>
        /// 状態メッセージを設定、または取得します。
        /// </summary>
        public string StateMessage
        {
            get { return _stateMessage; }
            private set { SetProperty(ref _stateMessage, value); }
        }

        /// <summary>
        /// 送信用のテキストを設定、または取得します。
        /// </summary>
        [Display(Name = "送信用テキスト")]
        [Required(ErrorMessage = "{0}に送信メッセージを入力してください。")]
        public string Remarks
        {
            get { return _remarks; }
            set { SetProperty(ref _remarks, value); }
        }

        #endregion

        #region Methods

        /// <summary>
        /// WebSocket でクライアントから切断された際に呼び出されるイベントハンドラです。
        /// </summary>
        /// <param name="sender">イベント送信元オブジェクト</param>
        /// <param name="e">イベント引数オブジェクト</param>
        private void Server_OnClosedClient(object sender, WebSocketClientEventArgs e)
        {
            DialogService.ShowInformation($"クライアントからの切断を確認しました。" +
                                          $"{Environment.NewLine}" +
                                          $"{e.ClientEndPoint.Address}:{e.ClientEndPoint.Port}");
        }

        /// <summary>
        /// WebSocket でクライアントに接続された際に呼び出されるイベントハンドラです。
        /// </summary>
        /// <param name="sender">イベント送信元オブジェクト</param>
        /// <param name="e">イベント引数オブジェクト</param>
        private void Server_OnConnectionClient(object sender, WebSocketClientEventArgs e)
        {
            DialogService.ShowInformation("クライアントとの接続を確認しました。" +
                                          $"{Environment.NewLine}" +
                                          $"{e.ClientEndPoint.Address}:{e.ClientEndPoint.Port}");
            StateMessage = "クライアントとの接続を確認しました。";
        }

        /// <summary>
        /// WebSocket でクライアントからリクエストを受信した際に呼び出されるイベントハンドラです。
        /// </summary>
        /// <param name="sender">イベント送信元オブジェクト</param>
        /// <param name="e">イベント引数オブジェクト</param>
        private void Server_OnReceivedRequest(object sender, WebSocketReceivedRequestEventArgs e)
        {
            var message = Encoding.UTF8.GetString(e.GetReceivedData());
            DialogService.ShowInformation("クライアントからメッセージを受信しました。" +
                                          $"{Environment.NewLine}" +
                                          $"{message}");
        }

        #endregion
    }
}