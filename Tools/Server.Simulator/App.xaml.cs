namespace Server.Simulator
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Reflection;
    using System.Threading.Tasks;
    using System.Windows;
    using JenkinsNotification.Core;
    using JenkinsNotification.Core.ComponentModels;
    using JenkinsNotification.CustomControls.Services;
    using Microsoft.Practices.Prism.Mvvm;

    /// <summary>
    /// App.xaml の相互作用ロジック
    /// </summary>
    public partial class App : Application
    {
        #region Methods

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var dialogService = new DialogService();

            //
            // ViewModel を生成する場合、コンストラクタの引数にダイアログ サービスを設定する。
            //
            ViewModelLocationProvider.SetDefaultViewModelFactory(
                viewModelType => Activator.CreateInstance(viewModelType, dialogService));

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

        #endregion
    }
}
