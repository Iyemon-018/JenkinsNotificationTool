namespace JenkinsNotification.Core
{
    using System;
    using System.Reflection;
    using System.Windows;
    using AutoMapper;
    using ComponentModels;
    using Configurations;
    using JenkinsNotification.Core.Communicators;
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
        /// WebSocket 通信を取得します。
        /// </summary>
        public IWebSocketCommunicator WebSocketCommunicator { get; private set; }

        #endregion

        #region Methods

        /// <summary>
        /// アプリケーション機能の初期化処理を実行します。
        /// </summary>
        public static void Initialize()
        {
            using (TimeTracer.StartNew("アプリケーション初期化シークエンスを実行する。"))
            {
                // マッピングの初期化を行う。
                InitializeMapping();

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
        /// このアプリケーションを終了します。
        /// </summary>
        public void Shutdown()
        {
            LogManager.Info("☆☆ アプリケーションをシャットダウンする。");

            LogManager.Info("☆☆ WebSocket通信を切断する。");
            WebSocketCommunicator?.Disconnect();
            WebSocketCommunicator?.Dispose();

            Application.Current.Shutdown();
        }

        #endregion
    }
}