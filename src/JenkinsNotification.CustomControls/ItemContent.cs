namespace JenkinsNotification.CustomControls
{
    using System.ComponentModel;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media;

    /// <summary>
    /// 設定項目とヘッダー、説明をひとまとめにしたコンポーネント クラスです。
    /// </summary>
    /// <seealso cref="System.Windows.Controls.ContentControl" />
    public class ItemContent : ContentControl
    {
        #region Const

        /// <summary>
        /// 依存関係プロパティ <see cref="DescriptionFontSize"/> を識別します。
        /// </summary>
        public static readonly DependencyProperty DescriptionFontSizeProperty =
            DependencyProperty.Register("DescriptionFontSize"
                                      , typeof(double)
                                      , typeof(ItemContent)
                                      , new FrameworkPropertyMetadata(12.0d));

        /// <summary>
        /// 依存関係プロパティ <see cref="DescriptionForeground"/> を識別します。
        /// </summary>
        public static readonly DependencyProperty DescriptionForegroundProperty =
            DependencyProperty.Register("DescriptionForeground"
                                      , typeof(Brush)
                                      , typeof(ItemContent)
                                      , new FrameworkPropertyMetadata(Brushes.Black, FrameworkPropertyMetadataOptions.AffectsRender));

        /// <summary>
        /// 依存関係プロパティ <see cref="Description"/> を識別します。
        /// </summary>
        public static readonly DependencyProperty DescriptionProperty =
            DependencyProperty.Register("Description"
                                      , typeof(string)
                                      , typeof(ItemContent)
                                      , new FrameworkPropertyMetadata("Item detail description."));

        /// <summary>
        /// 依存関係プロパティ <see cref="HeaderFontSize"/> を識別します。
        /// </summary>
        public static readonly DependencyProperty HeaderFontSizeProperty =
            DependencyProperty.Register("HeaderFontSize"
                                      , typeof(double)
                                      , typeof(ItemContent)
                                      , new FrameworkPropertyMetadata(18.0d));

        /// <summary>
        /// 依存関係プロパティ <see cref="HeaderForeground"/> を識別します。
        /// </summary>
        public static readonly DependencyProperty HeaderForegroundProperty =
            DependencyProperty.Register("HeaderForeground"
                                      , typeof(Brush)
                                      , typeof(ItemContent)
                                      , new FrameworkPropertyMetadata(new SolidColorBrush(Color.FromArgb(255, 0x7A, 0x1E, 0xA1)), FrameworkPropertyMetadataOptions.AffectsRender));

        /// <summary>
        /// 依存関係プロパティ <see cref="Header"/> を識別します。
        /// </summary>
        public static readonly DependencyProperty HeaderProperty =
            DependencyProperty.Register("Header"
                                      , typeof(string)
                                      , typeof(ItemContent)
                                      , new FrameworkPropertyMetadata("Item Header"));

        #endregion

        #region Ctor

        /// <summary>
        /// コンストラクタ
        /// </summary>
        static ItemContent()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ItemContent), new FrameworkPropertyMetadata(typeof(ItemContent)));
            PaddingProperty.OverrideMetadata(typeof(ItemContent), new FrameworkPropertyMetadata(new Thickness(4, 2, 2, 16)));
            HorizontalContentAlignmentProperty.OverrideMetadata(typeof(ItemContent), new FrameworkPropertyMetadata(HorizontalAlignment.Stretch));
            VerticalContentAlignmentProperty.OverrideMetadata(typeof(ItemContent), new FrameworkPropertyMetadata(VerticalAlignment.Stretch));
            FocusableProperty.OverrideMetadata(typeof(ItemContent), new FrameworkPropertyMetadata(false));
        }

        #endregion

        #region Properties

        /// <summary>
        /// 項目名を表すヘッダーを設定、または取得します。
        /// </summary>
        [Category("Custom"),
         Description("項目名を表すヘッダーを設定、または取得します。")]
        public string Header
        {
            get { return (string) GetValue(HeaderProperty); }
            set { SetValue(HeaderProperty, value); }
        }

        /// <summary>
        /// 項目の詳細な説明を設定、または取得します。
        /// </summary>
        [Category("Custom"),
         Description("項目の詳細な説明をを設定、または取得します。")]
        public string Description
        {
            get { return (string) GetValue(DescriptionProperty); }
            set { SetValue(DescriptionProperty, value); }
        }

        /// <summary>
        /// ヘッダーの前景色を設定、または取得します。
        /// </summary>
        [Category("Custom"),
         Description("ヘッダーの前景色を設定、または取得します。")]
        public Brush HeaderForeground
        {
            get { return (Brush) GetValue(HeaderForegroundProperty); }
            set { SetValue(HeaderForegroundProperty, value); }
        }

        /// <summary>
        /// ヘッダーのフォントサイズを設定、または取得します。
        /// </summary>
        [Category("Custom"),
         Description("ヘッダーのフォントサイズを設定、または取得します。")]
        public double HeaderFontSize
        {
            get { return (double) GetValue(HeaderFontSizeProperty); }
            set { SetValue(HeaderFontSizeProperty, value); }
        }

        /// <summary>
        /// 説明の前景色を設定、または取得します。
        /// </summary>
        [Category("Custom"),
         Description("説明の前景色を設定、または取得します。")]
        public Brush DescriptionForeground
        {
            get { return (Brush) GetValue(DescriptionForegroundProperty); }
            set { SetValue(DescriptionForegroundProperty, value); }
        }


        /// <summary>
        /// 説明のフォントサイズを設定、または取得します。
        /// </summary>
        [Category("Custom"),
         Description("説明のフォントサイズを設定、または取得します。")]
        public double DescriptionFontSize
        {
            get { return (double) GetValue(DescriptionFontSizeProperty); }
            set { SetValue(DescriptionFontSizeProperty, value); }
        }

        #endregion
    }
}