namespace JenkinsNotificationTool
{
    using System;
    using System.Reflection;
    using System.Windows;
    using JenkinsNotification.Core;
    using JenkinsNotification.Core.ComponentModels;
    using JenkinsNotification.Core.Configurations;
    using JenkinsNotification.Core.Logs;
    using JenkinsNotification.Core.Services;
    using JenkinsNotification.CustomControls.Services;
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
        /// 各種サービス提供機能
        /// </summary>
        private readonly IServicesProvider _servicesProvider;

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
            //
            // ログ機能の初期化を行う。
            //
            LogManager.AddLogger(new NLogger());

            var dialogService     = new DialogService();
            var viewService       = new ViewService();
            var balloonTipService = new BalloonTipService();
            _servicesProvider     = new ServicesProvider(dialogService, viewService, balloonTipService);

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
                viewModelType => Activator.CreateInstance(viewModelType, _servicesProvider));
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
            if (!_isConnectedWebSocket)
            {
                _servicesProvider.ViewService.Show(ScreenKey.Configuration);
            }
        }

        /// <summary>
        /// アプリケーションで使用するViewを登録する。
        /// </summary>
        private void RegisterViews()
        {
            ViewService.RegisterView(ScreenKey.Configuration, typeof(ConfigurationView));
            ViewService.RegisterView(ScreenKey.NotificationHistory, typeof(NotifyHistoryView));
        }

        #endregion
    }
}