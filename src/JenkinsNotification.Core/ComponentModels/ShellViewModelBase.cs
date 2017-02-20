namespace JenkinsNotification.Core.ComponentModels
{
    using System;
    using Communicators;
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
    public abstract class ShellViewModelBase : ShareViewModelBase
    {
        #region Fields

        /// <summary>
        /// サービス提供インターフェース
        /// </summary>
        private readonly IServicesProvider _servicesProvider;

        /// <summary>
        /// 通信インターフェース
        /// </summary>
        private readonly ICommunicatorProvider _communicatorProvider;

        /// <summary>
        /// データ蓄積領域
        /// </summary>
        private readonly IDataStore _dataStore;

        #endregion

        #region Ctor

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="servicesProvider">サービス提供インターフェース</param>
        /// <param name="communicatorProvider">通信インターフェース</param>
        /// <param name="dataStore">データ蓄積領域</param>
        /// <exception cref="System.ArgumentNullException">
        /// <paramref name="servicesProvider"/> or <paramref name="communicatorProvider"/> or <paramref name="dataStore"/> がnull の場合にスローされます。
        /// </exception>
        protected ShellViewModelBase(IServicesProvider servicesProvider, ICommunicatorProvider communicatorProvider, IDataStore dataStore)
            : base(servicesProvider?.DialogService)
        {
            //
            // デザイナー上でViewModelをバインドするために
            // デザインモード時は常にViewModel をインスタンス化できるようにする。
            //
            if (ViewUtility.IsDesignMode()) return;
            if (servicesProvider     == null) throw new ArgumentNullException(nameof(servicesProvider));
            if (communicatorProvider == null) throw new ArgumentNullException(nameof(communicatorProvider));
            if (dataStore            == null) throw new ArgumentNullException(nameof(dataStore));

            _servicesProvider     = servicesProvider;
            _communicatorProvider = communicatorProvider;
            _dataStore            = dataStore;
        }
        
        #endregion

        #region Properties

        /// <summary>
        /// バルーン表示サービスの参照を取得します。
        /// </summary>
        protected IBalloonTipService BalloonTipService => _servicesProvider.BalloonTipService;

        /// <summary>
        /// View表示サービスを取得します。
        /// </summary>
        protected IViewService ViewService => _servicesProvider.ViewService;

        /// <summary>
        /// 通信インターフェースを取得します。
        /// </summary>
        protected ICommunicatorProvider CommunicatorProvider => _communicatorProvider;

        /// <summary>
        /// データ蓄積領域を取得します。
        /// </summary>
        protected IDataStore DataStore => _dataStore;

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