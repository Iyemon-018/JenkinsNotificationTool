using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace JenkinsNotification.Debug.LogViewer.Views
{
    using JenkinsNotification.Core.ComponentModels;
    using JenkinsNotification.CustomControls;
    using JenkinsNotification.Debug.LogViewer.ViewModels;

    /// <summary>
    /// LogView.xaml の相互作用ロジック
    /// </summary>
    [ViewModel(typeof(LogViewModel))]
    public partial class LogView : View
    {
        public LogView()
        {
            InitializeComponent();
        }
    }
}
