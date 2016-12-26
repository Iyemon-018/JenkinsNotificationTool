namespace JenkinsNotification.CustomControls
{
    using System.ComponentModel;
    using System.Windows;
    using MaterialDesignThemes.Wpf;

    /// <summary>
    /// メッセージアイコン コンポーネント クラスです。
    /// </summary>
    /// <seealso cref="MaterialDesignThemes.Wpf.PackIcon" />
    public class MessageIcon : PackIcon
    {
        #region Const

        /// <summary>
        /// <see cref="IconType"/> 依存関係プロパティを識別します。
        /// </summary>
        public static readonly DependencyProperty IconTypeProperty =
            DependencyProperty.Register("IconType"
                                      , typeof(MessageBoxImage)
                                      , typeof(MessageIcon)
                                      , new FrameworkPropertyMetadata(MessageBoxImage.Information));

        #endregion

        #region Ctor

        /// <summary>
        /// コンストラクタ
        /// </summary>
        static MessageIcon()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(MessageIcon), new FrameworkPropertyMetadata(typeof(MessageIcon)));
            UseLayoutRoundingProperty.OverrideMetadata(typeof(MessageIcon), new FrameworkPropertyMetadata(true));
        }

        #endregion

        #region Properties

        /// <summary>
        /// アイコンの種別を設定、または取得します。
        /// </summary>
        [Description("アイコンの種別を設定、または取得します。")]
        [Category("Custom")]
        [Browsable(true)]
        public MessageBoxImage IconType
        {
            get { return (MessageBoxImage) GetValue(IconTypeProperty); }
            set { SetValue(IconTypeProperty, value); }
        }

        #endregion
    }
}