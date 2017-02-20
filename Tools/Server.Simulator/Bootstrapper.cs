namespace Server.Simulator
{
    using System;
    using System.Reflection;
    using System.Windows;
    using JenkinsNotification.Core.ComponentModels;
    using JenkinsNotification.Core.Services;
    using JenkinsNotification.CustomControls.Services;
    using Microsoft.Practices.ServiceLocation;
    using Prism.Mvvm;
    using Prism.Unity;
    using Server.Simulator.Views;

    public class Bootstrapper : UnityBootstrapper
    {
        /// <summary>
        /// アプリケーション全体で使用するダイアログサービスです。
        /// </summary>
        private readonly IDialogService _dialogService = new DialogService();

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
            // ViewModel を生成する場合、コンストラクタの引数にダイアログ サービスを設定する。
            //
            ViewModelLocationProvider.SetDefaultViewModelFactory(
                viewModelType => Activator.CreateInstance(viewModelType, _dialogService));
        }

        protected override DependencyObject CreateShell()
        {
            //return base.CreateShell();
            return ServiceLocator.Current.GetInstance<ManageView>();
        }

        protected override void InitializeShell()
        {
            //base.InitializeShell();

            var app = Application.Current;
            app.MainWindow = Shell as Window;
            app.MainWindow.Show();
        }
    }
}