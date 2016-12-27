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

namespace JenkinsNotification.CustomControls
{
    using System.ComponentModel;
    using JenkinsNotification.Core.Services;
    using JenkinsNotification.Core.Utility;

    /// <summary>
    /// ProgressDialog.xaml の相互作用ロジック
    /// </summary>
    public partial class ProgressDialog : View, IProgress
    {
        public ProgressDialog()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 依存関係プロパティ <see cref="Message"/> を識別します。
        /// </summary>
        public static readonly DependencyProperty MessageProperty =
            DependencyProperty.Register("Message"
                                      , typeof(string)
                                      , typeof(ProgressDialog)
                                      , new FrameworkPropertyMetadata("Executing work task..."));

        /// <summary>
        /// 表示メッセージを設定、または取得します。
        /// </summary>
        [Category("Custom"),
         Description("表示メッセージをを設定、または取得します。")]
        internal string Message
        {
            get { return (string) GetValue(MessageProperty); }
            set { SetValue(MessageProperty, value); }
        }


        /// <summary>
        /// 依存関係プロパティ <see cref="Caption"/> を識別します。
        /// </summary>
        public static readonly DependencyProperty CaptionProperty =
            DependencyProperty.Register("Caption"
                                      , typeof(string)
                                      , typeof(ProgressDialog)
                                      , new FrameworkPropertyMetadata("Progress Dialog"));

        /// <summary>
        /// ダイアログ キャプションを設定、または取得します。
        /// </summary>
        [Category("Custom"),
         Description("ダイアログ キャプションを設定、または取得します。")]
        internal string Caption
        {
            get { return (string) GetValue(CaptionProperty); }
            set { SetValue(CaptionProperty, value); }
        }

        private void ProgressDialog_OnLoaded(object sender, RoutedEventArgs e)
        {
            //var r = Task.Run(() => _work(this))
            //          .ContinueWith(async (x) => await Task.Delay(TimeSpan.FromSeconds(2)))
            //          .Result;
            //r.Start();
            _workSource.SetResult(this);
            Close();
        }

        private readonly TaskCompletionSource<IProgress> _workSource = new TaskCompletionSource<IProgress>();

        public void SetMessage(string message)
        {
            ThreadUtility.ExecuteUiThread(() => Message = message);
        }
    }
}
