namespace JenkinsNotificationTool
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using System.Windows;
    using JenkinsNotification.Core;
    using JenkinsNotification.Core.Communicators;
    using JenkinsNotification.Core.ComponentModels;
    using JenkinsNotification.Core.Configurations;
    using JenkinsNotification.Core.Executers;
    using JenkinsNotification.Core.Logs;
    using JenkinsNotification.Core.Services;
    using JenkinsNotification.Core.Utility;
    using JenkinsNotification.CustomControls.Services;
    using JenkinsNotificationTool.Executers;
    using JenkinsNotificationTool.Services;
    using JenkinsNotificationTool.Views;
    using Microsoft.Practices.Unity;
    using Prism.Mvvm;
    using Prism.Unity;

    /// <summary>
    /// アプリケーションのBootstrapper 機能クラスです。
    /// </summary>
    /// <seealso cref="UnityBootstrapper" />
    public class Bootstrapper : UnityBootstrapper
    {
        #region Fields

        /// <summary>
        /// WebSocket通信が確立できたかどうか
        /// </summary>
        private bool _isConnectedWebSocket = true;

        #endregion

        #region Ctor

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public Bootstrapper()
        {
            // この時点のスレッドをUIスレッドと認識する。
            ThreadUtility.InitializeSynchronizationContext();

            // マッピングの初期化を行う。
            InitializeMapping();

            // アプリケーションで使用するViewを登録する。
            RegisterViews();
        }

        #endregion

        #region Methods

        /// <summary>
        /// DIコンテナにインスタンスを登録します。
        /// </summary>
        /// <typeparam name="TInstance">登録対象のインスタンスの型</typeparam>
        /// <param name="container">Unity DIコンテナ</param>
        /// <param name="instance">登録するインスタンス</param>
        /// <exception cref="System.ArgumentNullException"><paramref name="instance"/> がnull の場合にスローされます。</exception>
        private void RegisterInstanceForContainer<TInstance>(IUnityContainer container, TInstance instance)
        {
            if (instance == null) throw new ArgumentNullException(nameof(instance));

            LogManager.Info($"Unity DIコンテナに{typeof(TInstance)} インスタンスを登録する。");
            container.RegisterInstance(instance);
        }

        /// <summary>
        /// <see cref="T:Microsoft.Practices.Unity.IUnityContainer" />を構成します。<para/>
        /// 派生クラスで上書きされ、アプリケーションで必要な特定の型マッピングが追加されることがあります。
        /// </summary>
        protected override void ConfigureContainer()
        {
            LogManager.Info("Unityコンテナにインスタンスを登録します。");

            base.ConfigureContainer();

            // コンテナにインスタンスを登録する。
            // これらのインスタンスは、コンテナ上で共有する。

            // データストアを初期化する。
            var dataStore = ConfigureDataStore();
            RegisterInstanceForContainer(Container, dataStore);

            // サービスを初期化する。
            var servicesProvider = ConfigureServices();
            RegisterInstanceForContainer(Container, servicesProvider);

            // 通信インターフェースを初期化します。
            var communicatorProvider = ConfigureCommunicator();
            RegisterInstanceForContainer(Container, communicatorProvider);

            // データ処理部の初期構成を実施する。
            ConfigureDataFlow();
        }

        /// <summary>
        /// Prism の<see cref="T:Prism.Mvvm.ViewModelLocator" /> の初期化構成を実行します。
        /// </summary>
        protected override void ConfigureViewModelLocator()
        {
            LogManager.Info("ViewModelLocator の初期化構成を行う。");

            //base.ConfigureViewModelLocator();

            //
            // View に設定したViewModel 属性の型によってView とViewModel を紐付けます。
            //
            ViewModelLocationProvider.SetDefaultViewTypeToViewModelTypeResolver(viewType =>
                                                                                {
                                                                                    var vmType = viewType.GetTypeInfo().GetCustomAttribute<ViewModelAttribute>();
                                                                                    return vmType?.ViewModelType;
                                                                                });

            //
            // ViewModel を生成する場合、コンストラクタの引数にインジェクション サービスを設定する。
            //
            ViewModelLocationProvider.SetDefaultViewModelFactory(viewModelType => Container.Resolve(viewModelType));
        }

        /// <summary>
        /// アプリケーションで使用するメインView(Shell)を生成します。
        /// </summary>
        /// <returns>アプリケーションのShell</returns>
        /// <remarks>If the returned instance is a <see cref="T:System.Windows.DependencyObject" />, the
        /// <see cref="T:Microsoft.Practices.Prism.Bootstrapper" /> will attach the default <see cref="T:Microsoft.Practices.Prism.Regions.IRegionManager" /> of
        /// the application in its <see cref="F:Microsoft.Practices.Prism.Regions.RegionManager.RegionManagerProperty" /> attached property
        /// in order to be able to add regions by using the <see cref="F:Microsoft.Practices.Prism.Regions.RegionManager.RegionNameProperty" />
        /// attached property from XAML.</remarks>
        protected override DependencyObject CreateShell()
        {
            return Container.Resolve<MainView>();
        }

        /// <summary>
        /// Shellを初期化します。
        /// </summary>
        protected override void InitializeShell()
        {
            LogManager.Info("Shell の初期化処理を実行する。");

            //base.InitializeShell();

            var app = Application.Current;
            var servicesProvider = Container.Resolve<IServicesProvider>();
            app.MainWindow = Shell as Window;

            //
            // バルーン通知用のタスクバーアイコンを登録する。
            // MainView に定義しているTaskIcon はインスタンス化された時点でタスクトレイに格納される。
            // その為、MainView はShow する必要がない。
            // （というか、Show すると表示されてしまうのでしてはいけない。）
            //
            var mainView = Shell as MainView;
            if (mainView != null)
            {
                servicesProvider.BalloonTipService
                                .SetBalloonTip(mainView.TaskbarIcon);
            }

            //
            // WebSocket接続に失敗した場合、構成情報を強制的に行う。
            //
            if (!_isConnectedWebSocket)
            {
                LogManager.Info("WebSocket接続に失敗したため、構成情報設定画面を表示する。");
                servicesProvider.ViewService.Show(ScreenKey.Configuration);
            }
        }

        /// <summary>
        /// 通信機能提供インターフェースの初期化構成を行います。
        /// </summary>
        /// <returns>初期化構成が完了した通信機能提供インターフェース</returns>
        private ICommunicatorProvider ConfigureCommunicator()
        {
            LogManager.Info("通信機能提供インターフェースの初期化構成を行う。");

            // TODO WebAPIインターフェースを実装する。
            var webSocketCommunicator = new WebSocketCommunicator();
            var webSocketDataFlow     = new WebSocketDataFlow(webSocketCommunicator);
            var communicatorProvider  = new CommunicatorProvider(webSocketCommunicator, null, webSocketDataFlow);

            communicatorProvider.WebSocketDataFlow.ConfigureRegistration();

            return communicatorProvider;
        }

        /// <summary>
        /// データ処理部の初期化構成を行います。
        /// </summary>
        private void ConfigureDataFlow()
        {
            LogManager.Info("データ処理部の初期化構成を行う。");
            var communicatorProvider = Container.Resolve<ICommunicatorProvider>();
            var dataStore            = Container.Resolve<IDataStore>();
            var servicesProvider     = Container.Resolve<IServicesProvider>();

            LogManager.Info("データフローの初期化構成を行う。");

            // TODO WebSocket通信で受信時に実行するタスクを登録する。
            var tasks = new List<IExecuter>
                            {
                                new JobResultExecuter(dataStore),
                                new JobReceivedNotificationExecuter(servicesProvider)
                            };

            foreach (var task in tasks)
            {
                communicatorProvider.WebSocketDataFlow.RegisterExecuteTask(task);
            }
        }

        /// <summary>
        /// データストアの初期化構成を行います。
        /// </summary>
        /// <returns>初期化構成が完了したデータストア</returns>
        private IDataStore ConfigureDataStore()
        {
            LogManager.Info("データストアの初期化構成を行う。");

            try
            {
                //
                // 構成ファイルを読み込む。
                //
                ApplicationConfiguration.LoadCurrent();
            }
            catch (ConfigurationLoadException e)
            {
                _isConnectedWebSocket = false;
                LogManager.Error("アプリケーション構成ファイルの読み込みに失敗した。", e);
            }

            return new DataStore(ApplicationConfiguration.Current);
        }

        /// <summary>
        /// サービス機能提供インターフェースの初期化構成を行います。
        /// </summary>
        /// <returns>初期化構成が完了したサービス機能提供インターフェース</returns>
        private IServicesProvider ConfigureServices()
        {
            LogManager.Info("サービス機能提供インターフェースの初期化構成を行う。");
            var dataStore          = Container.Resolve<IDataStore>();
            var dialogService      = new DialogService();
            var viewService        = new ViewService();
            var balloonTipService  = new BalloonTipService(dataStore);
            var applicationService = new ApplicationService();

            // アプリケーション終了時にタスクバーアイコンを解放する。
            var disposeTaskIcon = new Action(() => balloonTipService.Dispose());
            applicationService.AddShutdownTask(disposeTaskIcon);

            return new ServicesProvider(dialogService, viewService, balloonTipService, applicationService);
        }

        /// <summary>
        /// データマッピング構成情報を初期化します。
        /// </summary>
        private void InitializeMapping()
        {
            LogManager.Info("データマッピングの初期化構成を行う。");
            var configure = new MappingConfigure();
            configure.RegisterProfileType(typeof(Profile));
            configure.Initialize();
        }

        /// <summary>
        /// アプリケーションで使用するViewを登録します。
        /// </summary>
        private void RegisterViews()
        {
            LogManager.Info("アプリケーションで使用するView を登録する。");
            ViewService.RegisterView(ScreenKey.Configuration, typeof(ConfigurationView));
            ViewService.RegisterView(ScreenKey.NotificationHistory, typeof(NotifyHistoryView));
        }

        #endregion
    }
}