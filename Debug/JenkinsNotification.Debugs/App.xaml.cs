using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace JenkinsNotification.Debugs
{
    using JenkinsNotification.Core;

    /// <summary>
    /// App.xaml の相互作用ロジック
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            // マッピングの初期化
            var configure = new MappingConfigure();
            configure.RegisterProfileType(typeof(Profile));
            configure.Initialize();

            base.OnStartup(e);
        }
    }
}
