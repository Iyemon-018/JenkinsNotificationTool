namespace JenkinsNotification.Core.ComponentModels
{
    using System;
    using Services;
    using Utility;

    /// <summary>
    /// Viewと対になる、このアプリケーション専用のViewModel クラスです。
    /// </summary>
    /// <seealso cref="ViewModelBase" />
    /// <remarks>
    /// このViewModelは、以下の機能を備えています。<para/>
    /// ・メッセージ ダイアログを表示する。
    /// </remarks>
    public abstract class ApplicationViewModelBase : ShareViewModelBase
    {
        #region Fields

        /// <summary>
        /// インジェクション サービス
        /// </summary>
        private readonly IServicesProvider _servicesProvider;

        #endregion

        #region Ctor

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="servicesProvider">インジェクション サービス</param>
        protected ApplicationViewModelBase(IServicesProvider servicesProvider)
            : base(servicesProvider?.DialogService)
        {
            //
            // デザイナー上でViewModelをバインドするために
            // デザインモード時は常にViewModel をインスタンス化できるようにする。
            //
            if (ViewUtility.IsDesignMode()) return;
            if (servicesProvider == null) throw new ArgumentNullException(nameof(servicesProvider));

            _servicesProvider = servicesProvider;
            ViewService = _servicesProvider.ViewService;
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="dialogService">ダイアログサービス</param>
        protected ApplicationViewModelBase(IDialogService dialogService) : base(dialogService)
        {
        }

        #endregion

        #region Properties

        /// <summary>
        /// <see cref="ApplicationManager"/> の参照を取得します。
        /// </summary>
        protected ApplicationManager ApplicationManager => ApplicationManager.Instance;

        /// <summary>
        /// バルーン表示サービスの参照を取得します。
        /// </summary>
        protected IBalloonTipService BalloonTipService => ApplicationManager.BalloonTipService;

        /// <summary>
        /// View表示サービスを設定、取得します。
        /// </summary>
        protected IViewService ViewService { get; private set; }

        #endregion

        #region Methods

        /// <summary>
        /// View が閉じられた場合に呼び出されるViewModel のアンロード機能です。
        /// </summary>
        public void Unloaded()
        {
            
        }

        /// <summary>
        /// View が閉じられました。
        /// </summary>
        protected virtual void OnUnloaded()
        {
            
        }

        /// <summary>
        /// View がロードされた際に呼び出されるViewModel のロード機能です。
        /// </summary>
        public void Loaded()
        {
            OnLoaded();
        }

        /// <summary>
        /// View がロードされました。
        /// </summary>
        protected virtual void OnLoaded()
        {

        }

        #endregion
    }
}