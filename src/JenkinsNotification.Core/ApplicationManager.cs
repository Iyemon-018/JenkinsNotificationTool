namespace JenkinsNotification.Core
{
    using System;
    using System.Reflection;
    using System.Windows;
    using AutoMapper;
    using Communicators;
    using ComponentModels;
    using Configurations;
    using Logs;
    using Services;
    using Utility;
    using Microsoft.Practices.Prism.Mvvm;

    /// <summary>
    /// このアプリケーションの機能管理クラスです。
    /// </summary>
    /// <remarks>
    /// <see cref="IBalloonTipService"/> はTaskbarIcon がなければその機能を使用することができません。<para/>
    /// TaskbarIcon を参照するにはView をインスタンス化する必要があります。<para/>
    /// View をインスタンス化するとViewModelLocator の機能によりViewModel が自動生成されますが、
    /// ViewModel には<see cref="IBalloonTipService"/> が必要です。<para/>
    /// 循環参照のような連鎖を回避するため、まず<see cref="SetDefaultViewModelLocater"/> メソッドで
    /// インジェクションサービスを設定しておき、<see cref="Initialize"/> メソッドで当クラスのインスタンス化を行います。<para/>
    /// よって、<see cref="IBalloonTipService"/> は当クラスだけインスタンスを持っています。<para/>
    /// <see cref="IServicesProvider"/> に<see cref="IBalloonTipService"/> が存在しないのはこのためです。<para/>
    /// </remarks>
    public sealed class ApplicationManager
    {
        #region Const

        /// <summary>
        /// 唯一のインスタンス
        /// </summary>
        private static readonly ApplicationManager _instance = new ApplicationManager();

        #endregion

        #region Fields

        /// <summary>
        /// WebSocket 通信機能
        /// </summary>
        private IWebSocketCommunicator _webSocketCommunicator;

        #endregion

        #region Ctor

        /// <summary>
        /// コンストラクタ
        /// </summary>
        private ApplicationManager()
        {
        }

        #endregion

        #region Properties

        /// <summary>
        /// 唯一のインスタンスを取得します。
        /// </summary>
        // ReSharper disable once ConvertToAutoProperty
        public static ApplicationManager Instance => _instance;

        /// <summary>
        /// アプリケーション構成情報を取得します。
        /// </summary>
        public ApplicationConfiguration ApplicationConfiguration => ApplicationConfiguration.Current;

        /// <summary>
        /// バルーン表示サービスを取得します。
        /// </summary>
        public IBalloonTipService BalloonTipService { get; private set; }

        /// <summary>
        /// データ管理機能を取得します。
        /// </summary>
        public IDataManager DataManager { get; private set; }

        /// <summary>
        /// WebSocket 通信を取得します。
        /// </summary>
        public IWebSocketCommunicator WebSocketCommunicator
        {
            get { return _webSocketCommunicator; }
            private set
            {
                _webSocketCommunicator = value;
                OnWebSocketCommunicatorChanged(value);
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// アプリケーション機能の初期化処理を実行します。
        /// </summary>
        public static void Initialize(IDataManager dataManager)
        {
            using (TimeTracer.StartNew("アプリケーション初期化シークエンスを実行する。"))
            {
                // マッピングの初期化を行う。
                InitializeMapping();

                Instance.DataManager = dataManager;
            }
        }

        /// <summary>
        /// バルーン表示サービスを初期化します。
        /// </summary>
        /// <param name="balloonTipService">バルーン表示サービス</param>
        public static void InitializeBalloonTipService(IBalloonTipService balloonTipService)
        {
            Instance.BalloonTipService = balloonTipService;
        }

        /// <summary>
        /// ViewModel の生成ルールを設定します。<para/>
        /// このメソッドは最初のView を生成する前に実行してください。
        /// </summary>
        /// <param name="servicesProvider">インジェクション サービス</param>
        public static void SetDefaultViewModelLocater(IServicesProvider servicesProvider)
        {
            //
            // ViewModel を生成する場合、コンストラクタの引数にインジェクション サービスを設定する。
            //
            ViewModelLocationProvider.SetDefaultViewModelFactory(
                viewModelType => Activator.CreateInstance(viewModelType, servicesProvider));

            //
            // View に設定したViewModel 属性の型によってView とViewModel を紐付けます。
            //
            ViewModelLocationProvider.SetDefaultViewTypeToViewModelTypeResolver(
                viewType =>
                {
                    var vmType = viewType.GetTypeInfo().GetCustomAttribute<ViewModelAttribute>();
                    return vmType?.ViewModelType;
                });
        }

        /// <summary>
        /// WebSocket 通信を設定します。
        /// </summary>
        /// <param name="communicator">WebSocket 通信機能</param>
        /// <exception cref="System.ArgumentNullException"><paramref name="communicator"/> がnull の場合にスローされます。</exception>
        public static void SetWebSocketCommunicator(IWebSocketCommunicator communicator)
        {
            if (communicator == null) throw new ArgumentNullException(nameof(communicator));
            Instance.WebSocketCommunicator = communicator;
        }

        /// <summary>
        /// WebSocket通信を試みます。
        /// </summary>
        /// <exception cref="System.InvalidOperationException"><see cref="WebSocketCommunicator"/> が初期化されていない場合にスローされます。</exception>
        public static void TryConnectionWebSocket()
        {
            if (Instance.WebSocketCommunicator == null)
            {
                // TODO リソースに追加する。
                throw new InvalidOperationException($"{nameof(WebSocketCommunicator)} が初期化されていません。{nameof(SetWebSocketCommunicator)} メソッドを呼び出してから実行してください。");
            }

            //
            // 接続を試みる。
            //
            Instance.WebSocketCommunicator
                    .Connection(Instance.ApplicationConfiguration
                                        .NotifyConfiguration
                                        .TargetUri
                              , Constants.WebSocketRetryMaximum);
        }

        /// <summary>
        /// このアプリケーションを終了します。
        /// </summary>
        public void Shutdown()
        {
            LogManager.Info("☆☆ アプリケーションをシャットダウンする。");

            LogManager.Info("☆☆ WebSocket通信を切断する。");
            CleanupWebSocket();

            Application.Current.Shutdown();
        }

        /// <summary>
        /// WebSocket 通信機能を解放する。
        /// </summary>
        private void CleanupWebSocket()
        {
            if (WebSocketCommunicator != null)
            {
                LogManager.Info("WebSocket 機能のクリーンアップを実行して、イベント購読の解除と切断を行う。");

                // イベント購読の解除
                WebSocketCommunicator.Connected        -= WebSocketCommunicator_OnConnected;
                WebSocketCommunicator.ConnectionFailed -= WebSocketCommunicator_OnConnectionFailed;
                WebSocketCommunicator.Received         -= WebSocketCommunicator_OnReceived;

                // 切断
                WebSocketCommunicator.Disconnect();
                WebSocketCommunicator.Dispose();
            }
        }

        /// <summary>
        /// オブジェクトのマッピング情報を初期化します。
        /// </summary>
        private static void InitializeMapping()
        {
            Mapper.Initialize(x => x.AddProfile<Profile>());
            Mapper.AssertConfigurationIsValid();
        }

        /// <summary>
        /// <see cref="WebSocketCommunicator"/> プロパティの値が更新されました。
        /// </summary>
        /// <param name="newValue">更新後の値</param>
        private void OnWebSocketCommunicatorChanged(IWebSocketCommunicator newValue)
        {
            if (WebSocketCommunicator != null)
            {
                LogManager.Info("☆☆ 新しいWebSocketが設定されたので通信を切断する。");
                CleanupWebSocket();
            }

            if (WebSocketCommunicator != null)
            {
                LogManager.Info("☆☆ WebSocket 機能のイベント購読を開始する。");
                WebSocketCommunicator.Connected        += WebSocketCommunicator_OnConnected;
                WebSocketCommunicator.ConnectionFailed += WebSocketCommunicator_OnConnectionFailed;
                WebSocketCommunicator.Received         += WebSocketCommunicator_OnReceived;
            }
        }

        /// <summary>
        /// WebSocket 通信で接続が確立された際に呼び出されるイベントハンドラです。
        /// </summary>
        /// <param name="sender">イベント送信元オブジェクト</param>
        /// <param name="e">イベント引数オブジェクト</param>
        private void WebSocketCommunicator_OnConnected(object sender, EventArgs e)
        {
            LogManager.Info($"{WebSocketCommunicator.Uri} との接続を確立した。");
        }

        /// <summary>
        /// WebSocket 通信で接続が失敗した際に呼び出されるイベントハンドラです。
        /// </summary>
        /// <param name="sender">イベント送信元オブジェクト</param>
        /// <param name="eventArgs">イベント引数オブジェクト</param>
        private void WebSocketCommunicator_OnConnectionFailed(object sender, EventArgs eventArgs)
        {
            LogManager.Info($"{WebSocketCommunicator.Uri} との接続確立に失敗した。");
        }

        /// <summary>
        /// WebSocket 通信でデータを受信した際に呼び出されるイベントハンドラです。
        /// </summary>
        /// <param name="sender">イベント送信元オブジェクト</param>
        /// <param name="e">イベント引数オブジェクト</param>
        private void WebSocketCommunicator_OnReceived(object sender, ReceivedEventArgs e)
        {
            if (e.ReceivedType == ReceivedType.Message)
            {
                LogManager.Info("WebSocket 通信でメッセージデータを受信した。");
                DataManager.ReceivedMessage(e.Message);
            }
            else if (e.ReceivedType == ReceivedType.Binary)
            {
                LogManager.Info("WebSocket 通信でバイナリデータを受信した。");
                DataManager.ReceivedData(e.GetData());
            }
        }

        #endregion
    }
}