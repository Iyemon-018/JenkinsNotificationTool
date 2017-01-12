namespace JenkinsNotification.CustomControls
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media;
    using System.Windows.Media.Animation;
    using System.Windows.Media.Effects;
    using JenkinsNotification.Core.Utility;
    using JenkinsNotification.CustomControls.Utility;
    using Microsoft.Practices.Prism.Mvvm;

    /// <summary>
    /// このアプリケーション専用のView コンポーネントクラスです。
    /// </summary>
    /// <seealso cref="Window" />
    /// <seealso cref="IView" />
    [TemplatePart(Name = LayoutRootKey, Type = typeof(Border))]
    [TemplatePart(Name = MinimumWindowButtonKey, Type = typeof(Button))]
    [TemplatePart(Name = MaximumWindowButtonKey, Type = typeof(Button))]
    [TemplatePart(Name = RestoreWindowButtonKey, Type = typeof(Button))]
    [TemplatePart(Name = CloseWindowButtonKey, Type = typeof(Button))]
    public class View : Window, IView
    {
        #region Const

        /// <summary>
        /// ルート要素の部品名
        /// </summary>
        public const string LayoutRootKey = "Part_LayoutRoot";

        /// <summary>
        /// 最小化ボタンの部品名
        /// </summary>
        public const string MinimumWindowButtonKey = "Part_MinimumButtonKey";

        /// <summary>
        /// 最大化ボタンの部品名
        /// </summary>
        public const string MaximumWindowButtonKey = "Part_MaximumButtonKey";

        /// <summary>
        /// 元に戻すボタンの部品名
        /// </summary>
        public const string RestoreWindowButtonKey = "Part_RestoreButtonKey";

        /// <summary>
        /// 閉じるボタンの部品名
        /// </summary>
        public const string CloseWindowButtonKey = "Part_CloseButtonKey";

        /// <summary>
        /// 依存関係プロパティ <see cref="IsVisibleCaptionButton"/> を識別します。
        /// </summary>
        public static readonly DependencyProperty IsVisibleCaptionButtonProperty =
            DependencyProperty.Register("IsVisibleCaptionButton"
                                      , typeof(bool)
                                      , typeof(View)
                                      , new FrameworkPropertyMetadata(true, IsVisibleCaptionButtonPropertyChanged));

        /// <summary>
        /// 非アクティブ時に設定する画面全体のエフェクト
        /// </summary>
        private static readonly Effect DeactivatedEffect = new BlurEffect
                                                           {
                                                               RenderingBias = RenderingBias.Performance,
                                                               Radius = 3.0d
                                                           };

        #endregion

        #region Fields

        /// <summary>
        /// 閉じるボタン
        /// </summary>
        private Button _closeButton;

        /// <summary>
        /// <see cref="Window.Closing"/> イベント時に実行するアニメーション
        /// </summary>
        private Storyboard _closingAnimation;

        /// <summary>
        /// 終了アニメーションの完了フラグ
        /// </summary>
        private bool _isCompletedClosingAnimation;

        /// <summary>
        /// この<see cref="View"/> のルート レイアウト要素
        /// </summary>
        private Border _layoutRoot;

        /// <summary>
        /// <see cref="FrameworkElement.Loaded"/> イベント時に実行するアニメーション
        /// </summary>
        private Storyboard _loadedAnimation;

        /// <summary>
        /// 最大化ボタン
        /// </summary>
        private Button _maximumButton;

        /// <summary>
        /// 最小化ボタン
        /// </summary>
        private Button _minimumButton;

        /// <summary>
        /// 元に戻すボタン
        /// </summary>
        private Button _restoreButton;

        #endregion

        #region Ctor

        /// <summary>
        /// コンストラクタ
        /// </summary>
        static View()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(View), new FrameworkPropertyMetadata(typeof(View)));
            SizeToContentProperty.OverrideMetadata(typeof(View), new FrameworkPropertyMetadata(SizeToContent.WidthAndHeight));

            HorizontalContentAlignmentProperty.OverrideMetadata(typeof(View), new FrameworkPropertyMetadata(HorizontalAlignment.Stretch));
            VerticalContentAlignmentProperty.OverrideMetadata(typeof(View), new FrameworkPropertyMetadata(VerticalAlignment.Stretch));

            // AllowsTransparent がtrue の場合は、WindowStyle = WindowStyle.None に設定する必要がある。
            WindowStyleProperty.OverrideMetadata(typeof(View), new FrameworkPropertyMetadata(WindowStyle.None));
            AllowsTransparencyProperty.OverrideMetadata(typeof(View), new FrameworkPropertyMetadata(true));

            // 表示するアイコンを取得する。
            var appIcon = Application.Current.TryFindResource("ApplicationIcon") as ImageSource;
            IconProperty.OverrideMetadata(typeof(View), new FrameworkPropertyMetadata(appIcon));
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public View()
        {
            // 以下の設定は依存関係プロパティではないので、この時点で設定する。
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            
            //
            // ViewModel インジェクション機能を有効化する。
            //
            ViewUtility.InjectionViewModelLocater(this);
        }

        #endregion

        #region Properties

        /// <summary>
        /// キャプションエリアのボタンを表示するかどうかを設定、または取得します。
        /// </summary>
        [Category("Custom"),
         Description("キャプションエリアのボタンを表示するかどうかを設定、または取得します。")]
        public bool IsVisibleCaptionButton
        {
            get { return (bool) GetValue(IsVisibleCaptionButtonProperty); }
            set { SetValue(IsVisibleCaptionButtonProperty, value); }
        }

        #endregion

        #region Methods

        /// <summary>
        /// 派生クラスでオーバーライドされると、アプリケーション コードや内部プロセスで <see cref="M:System.Windows.FrameworkElement.ApplyTemplate" /> が呼び出されるたびに呼び出されます。
        /// </summary>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            //
            // 部品の初期化を行う。
            //
            _minimumButton = GetTemplateChild(MinimumWindowButtonKey) as Button;
            if (_minimumButton != null)
            {
                _minimumButton.Click += MinimumWindowButton_OnClick;
            }

            _maximumButton = GetTemplateChild(MaximumWindowButtonKey) as Button;
            if (_maximumButton != null)
            {
                _maximumButton.Click += MaximumWindowButton_OnClick;
            }

            _restoreButton = GetTemplateChild(RestoreWindowButtonKey) as Button;
            if (_restoreButton != null)
            {
                _restoreButton.Click += RestoreWindowButton_OnClick;
            }

            _closeButton = GetTemplateChild(CloseWindowButtonKey) as Button;
            if (_closeButton != null)
            {
                _closeButton.Click += CloseWindowButton_OnClick;
            }

            _layoutRoot = GetTemplateChild(LayoutRootKey) as Border;
            if (_layoutRoot != null)
            {
                //
                // 開始、終了のアニメーションを設定する。
                //
                _loadedAnimation = StoryboardUtility.CreateFadeInStoryboard(_layoutRoot, TimeSpan.Zero, TimeSpan.FromMilliseconds(150));
                _loadedAnimation.Freeze();
                var eventTrigger = new EventTrigger(LoadedEvent);
                eventTrigger.Actions.Add(new BeginStoryboard { Storyboard = _loadedAnimation });
                Triggers.Add(eventTrigger);

                _closingAnimation = StoryboardUtility.CreateFadeOutStoryboard(_closingAnimation, TimeSpan.Zero, TimeSpan.FromMilliseconds(150));
                _closingAnimation.Completed += _closingAnimation_Completed;
                _closingAnimation.Freeze();
            }

            // 初期状態だとWindowState の変更イベントが走らないのでここで一回実行しておく。
            SetCaptionButtonsState();
        }

        /// <summary>
        /// <see cref="E:System.Windows.Window.Activated" /> イベントを発生させます。
        /// </summary>
        /// <param name="e">イベント データを格納している <see cref="T:System.EventArgs" />。</param>
        protected override void OnActivated(EventArgs e)
        {
            Effect = null;

            base.OnActivated(e);
        }

        /// <summary>
        /// <see cref="E:System.Windows.Window.Closing" /> イベントを発生させます。
        /// </summary>
        /// <param name="e">イベント データを格納している <see cref="T:System.ComponentModel.CancelEventArgs" />。</param>
        protected override void OnClosing(CancelEventArgs e)
        {
            if (!_isCompletedClosingAnimation && _closingAnimation != null)
            {
                // 先に終了アニメーションを実行する。
                // 終わったらViewをクローズする。
                e.Cancel = true;
                BeginStoryboard(_closingAnimation);
            }

            base.OnClosing(e);
        }

        /// <summary>
        /// <see cref="E:System.Windows.Window.Deactivated" /> イベントを発生させます。
        /// </summary>
        /// <param name="e">イベント データを格納している <see cref="T:System.EventArgs" />。</param>
        protected override void OnDeactivated(EventArgs e)
        {
            Effect = DeactivatedEffect;

            base.OnDeactivated(e);
        }

        /// <summary>
        /// <see cref="E:System.Windows.Window.StateChanged" /> イベントを発生させます。
        /// </summary>
        /// <param name="e">イベント データを格納している <see cref="T:System.EventArgs" />。</param>
        protected override void OnStateChanged(EventArgs e)
        {
            if (ResizeMode != ResizeMode.NoResize)
            {
                base.OnStateChanged(e);

                SetCaptionButtonsState();
            }
        }

        /// <summary>
        /// 終了アニメーションが完了したときに呼ばれるイベントハンドラです。
        /// </summary>
        /// <param name="sender">イベント送信元オブジェクト</param>
        /// <param name="e">イベント引数オブジェクト</param>
        private void _closingAnimation_Completed(object sender, EventArgs e)
        {
            // 閉じる。
            _isCompletedClosingAnimation = true;
            SystemCommands.CloseWindow(this);
        }

        /// <summary>
        /// 閉じるボタンがクリックされたときに呼ばれるイベントハンドラです。
        /// </summary>
        /// <param name="sender">イベント送信元オブジェクト</param>
        /// <param name="e">イベント引数オブジェクト</param>
        private void CloseWindowButton_OnClick(object sender, RoutedEventArgs e)
        {
            SystemCommands.CloseWindow(this);
        }

        /// <summary>
        /// <see cref="IsVisibleCaptionButton"/> 依存関係プロパティの値が変更された際に呼ばれるイベントハンドラです。
        /// </summary>
        /// <param name="d">イベント送信元オブジェクト</param>
        /// <param name="e">イベント引数オブジェクト</param>
        private static void IsVisibleCaptionButtonPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as View)?.OnIsVisibleCaptionButtonChanged((bool)e.NewValue);
        }

        /// <summary>
        /// 最大化ボタンがクリックされたときに呼ばれるイベントハンドラです。
        /// </summary>
        /// <param name="sender">イベント送信元オブジェクト</param>
        /// <param name="e">イベント引数オブジェクト</param>
        private void MaximumWindowButton_OnClick(object sender, RoutedEventArgs e)
        {
            SystemCommands.MaximizeWindow(this);
        }

        /// <summary>
        /// 最小化ボタンがクリックされたときに呼ばれるイベントハンドラです。
        /// </summary>
        /// <param name="sender">イベント送信元オブジェクト</param>
        /// <param name="e">イベント引数オブジェクト</param>
        private void MinimumWindowButton_OnClick(object sender, RoutedEventArgs e)
        {
            SystemCommands.MinimizeWindow(this);
        }

        /// <summary>
        /// <see cref="IsVisibleCaptionButton"/> プロパティの値が更新されました。
        /// </summary>
        /// <param name="newValue">更新後の値</param>
        private void OnIsVisibleCaptionButtonChanged(bool newValue)
        {
            //
            // すべてのキャプションボタンの表示を切り替える。
            //
            var visibility = newValue ? Visibility.Visible : Visibility.Collapsed;
            if (_minimumButton != null)
            {
                _minimumButton.Visibility = visibility;
            }

            if (_maximumButton != null)
            {
                _maximumButton.Visibility = visibility;
            }

            if (_restoreButton != null)
            {
                _restoreButton.Visibility = visibility;
            }

            if (_closeButton != null)
            {
                _closeButton.Visibility = visibility;
            }
        }

        /// <summary>
        /// 元に戻すボタンがクリックされたときに呼ばれるイベントハンドラです。
        /// </summary>
        /// <param name="sender">イベント送信元オブジェクト</param>
        /// <param name="e">イベント引数オブジェクト</param>
        private void RestoreWindowButton_OnClick(object sender, RoutedEventArgs e)
        {
            SystemCommands.RestoreWindow(this);
        }

        /// <summary>
        /// タイトルバーエリアのボタン状態を設定します。
        /// </summary>
        private void SetCaptionButtonsState()
        {
            if (_restoreButton != null)
            {
                _restoreButton.Visibility = WindowState == WindowState.Normal ? Visibility.Collapsed : Visibility.Visible;
            }

            if (_maximumButton != null)
            {
                _maximumButton.Visibility = WindowState == WindowState.Maximized ? Visibility.Collapsed : Visibility.Visible;
            }

            if (_minimumButton != null)
            {
                _minimumButton.Visibility = WindowState == WindowState.Minimized ? Visibility.Collapsed : Visibility.Visible;
            }

            Margin = WindowState == WindowState.Maximized ? new Thickness(9) : new Thickness(0);
        }

        #endregion
    }
}