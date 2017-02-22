namespace JenkinsNotificationTool
{
    using System;
    using System.Reflection;
    using System.Windows;
    using JenkinsNotification.Core;
    using JenkinsNotification.Core.Communicators;
    using JenkinsNotification.Core.ComponentModels;
    using JenkinsNotification.Core.Configurations;
    using JenkinsNotification.Core.Logs;
    using JenkinsNotification.Core.Services;
    using JenkinsNotification.Core.Utility;
    using JenkinsNotification.CustomControls.Services;
    using JenkinsNotificationTool.Executers;
    using JenkinsNotificationTool.Services;
    using JenkinsNotificationTool.Views;
    using Microsoft.Practices.ServiceLocation;
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
        /// 通信インターフェース
        /// </summary>
        private ICommunicatorProvider _communicatorProvider;

        /// <summary>
        /// データストア
        /// </summary>
        private IDataStore _dataStore;

        /// <summary>
        /// WebSocket通信が確立できたかどうか
        /// </summary>
        private bool _isConnectedWebSocket = true;

        /// <summary>
        /// 各種サービス提供機能
        /// </summary>
        private IServicesProvider _servicesProvider;
        
        /// <summary>
        /// アプリケーション制御サービス
        /// </summary>
        private ApplicationService _applicationService;

        #endregion

        #region Ctor

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public Bootstrapper()
        {
            // この時点のスレッドをUIスレッドと認識する。
            ThreadUtility.InitializeSynchronizationContext();

            // ログ機能の初期化を行う。
            LogManager.AddLogger(new NLogger());

            // マッピングの初期化を行う。
            InitializeMapping();

            // サービスを初期化する。
            ConfigureServices();

            // データストアを初期化する。
            ConfigureDataStore();

            // 通信インターフェースを初期化します。
            ConfigureCommunicator();
            
            // アプリケーションで使用するViewを登録する。
            RegisterViews();
        }

        #endregion

        #region Properties

        /// <summary>
        /// WebSocket通信が確立できたかどうか
        /// </summary>
        public bool IsConnectedWebSocket => _isConnectedWebSocket;

        /// <summary>
        /// 各種サービス提供機能
        /// </summary>
        public IServicesProvider ServicesProvider => _servicesProvider;

        #endregion

        #region Methods

        /// <summary>
        /// データマッピング構成情報を初期化します。
        /// </summary>
        private void InitializeMapping()
        {
            var configure = new MappingConfigure();
            configure.RegisterProfileType(typeof(Profile));
            configure.Initialize();
        }

        /// <summary>
        /// Configures the <see cref="T:Prism.Mvvm.ViewModelLocator" /> used by Prism.
        /// </summary>
        protected override void ConfigureViewModelLocator()
        {
            //base.ConfigureViewModelLocator();

            //
            // View に設定したViewModel 属性の型によってView とViewModel を紐付けます。
            //
            ViewModelLocationProvider.SetDefaultViewTypeToViewModelTypeResolver(
                viewType =>
                {
                    var vmType = viewType.GetTypeInfo().GetCustomAttribute<ViewModelAttribute>();
                    return vmType?.ViewModelType;
                });
            //
            // ViewModel を生成する場合、コンストラクタの引数にインジェクション サービスを設定する。
            //
            ViewModelLocationProvider.SetDefaultViewModelFactory(
                viewModelType => Activator.CreateInstance(viewModelType, _servicesProvider, _communicatorProvider, _dataStore));
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
            return ServiceLocator.Current.GetInstance<MainView>();
        }

        /// <summary>
        /// Shellを初期化します。
        /// </summary>
        protected override void InitializeShell()
        {
            //base.InitializeShell();
            var app = Application.Current;
            app.MainWindow = Shell as Window;

            // バルーン通知用のタスクバーアイコンを登録する。
            var mainView = Shell as MainView;
            if (mainView != null)
            {
                _servicesProvider.BalloonTipService
                                 .SetBalloonTip(mainView.TaskbarIcon);
            }

            if (!_isConnectedWebSocket)
            {
                _servicesProvider.ViewService.Show(ScreenKey.Configuration);
            }
        }

        /// <summary>
        /// 通信インターフェースを初期化します。
        /// </summary>
        private void ConfigureCommunicator()
        {
            // TODO WebAPIインターフェースを実装する。
            var webSocketCommunicator = new WebSocketCommunicator();
            var webSocketDataFlow     = new WebSocketDataFlow(webSocketCommunicator);
            _communicatorProvider     = new CommunicatorProvider(webSocketCommunicator, null, webSocketDataFlow);
            _communicatorProvider.WebSocketDataFlow.ConfigureRegistration();

            var register = new DataFlowRegistration(webSocketDataFlow, _dataStore);
            register.AddTask(new JobReceivedNotificationExecuter(_servicesProvider.BalloonTipService));
            register.Configure();
        }

        /// <summary>
        /// データストアを初期化します。
        /// </summary>
        private void ConfigureDataStore()
        {
            //
            // アプリケーション機能の初期化を実施する。
            //
            try
            {
                //
                // 構成ファイルを読み込む。
                //
                ApplicationConfiguration.LoadCurrent();
            }
            catch (ConfigurationLoadException)
            {
                _isConnectedWebSocket = false;
            }

            _dataStore = new DataStore(ApplicationConfiguration.Current);
        }

        /// <summary>
        /// サービス提供インターフェースを構成します。
        /// </summary>
        private void ConfigureServices()
        {
            var dialogService     = new DialogService();
            var viewService       = new ViewService();
            var balloonTipService = new BalloonTipService(_dataStore);
            _applicationService   = new ApplicationService();
            _servicesProvider     = new ServicesProvider(dialogService, viewService, balloonTipService, _applicationService);

            // アプリケーション終了時にタスクバーアイコンを解放する。
            var disposeTaskIcon   = new Action(() => balloonTipService.Dispose());
            _applicationService.AddShutdownTask(disposeTaskIcon);
        }

        /// <summary>
        /// アプリケーションで使用するViewを登録します。
        /// </summary>
        private void RegisterViews()
        {
            ViewService.RegisterView(ScreenKey.Configuration, typeof(ConfigurationView));
            ViewService.RegisterView(ScreenKey.NotificationHistory, typeof(NotifyHistoryView));
        }
        
        #endregion
    }
}