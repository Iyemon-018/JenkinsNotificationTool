namespace JenkinsNotification.Core.Services
{
    using System;
    using Logs;

    /// <summary>
    /// 各種サービスを提供するクラスです。
    /// </summary>
    /// <seealso cref="IServicesProvider" />
    public class ServicesProvider : IServicesProvider
    {
        #region Fields

        /// <summary>
        /// ダイアログ表示サービス
        /// </summary>
        private readonly IDialogService _dialogService;

        /// <summary>
        /// 画面表示サービス
        /// </summary>
        private readonly IViewService _viewService;

        /// <summary>
        /// バルーン通知サービス
        /// </summary>
        private readonly IBalloonTipService _balloonTipService;

        /// <summary>
        /// アプリケーション制御サービス
        /// </summary>
        private readonly IApplicationService _applicationService;

        #endregion

        #region Ctor

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="dialogService">ダイアログ表示サービス</param>
        /// <param name="viewService">画面表示サービス</param>
        /// <param name="balloonTipService">バルーン通知サービス</param>
        /// <param name="applicationService">アプリケーション制御サービス</param>
        /// <exception cref="System.ArgumentNullException">
        /// <paramref name="dialogService"/> or <paramref name="viewService"/> or <paramref name="balloonTipService"/> or <paramref name="applicationService"/>がnull の場合にスローされます。
        /// </exception>
        public ServicesProvider(IDialogService dialogService, IViewService viewService, IBalloonTipService balloonTipService, IApplicationService applicationService)
        {
            if (dialogService      == null) throw new ArgumentNullException(nameof(dialogService));
            if (viewService        == null) throw new ArgumentNullException(nameof(viewService));
            if (balloonTipService  == null) throw new ArgumentNullException(nameof(balloonTipService));
            if (applicationService == null) throw new ArgumentNullException(nameof(applicationService));

            LogManager.Info($"ダイアログ表示サービスを設定します。{dialogService.GetType()}");
            LogManager.Info($"画面表示サービスを設定します。{viewService.GetType()}");
            LogManager.Info($"バルーン通知サービスを設定します。{balloonTipService.GetType()}");

            _dialogService      = dialogService;
            _viewService        = viewService;
            _balloonTipService  = balloonTipService;
            _applicationService = applicationService;
        }

        #endregion

        #region Properties

        /// <summary>
        /// ダイアログ表示サービスを取得します。
        /// </summary>
        public IDialogService DialogService => _dialogService;

        /// <summary>
        /// 画面表示サービスを取得します。
        /// </summary>
        public IViewService ViewService => _viewService;

        /// <summary>
        /// バルーン通知サービスを取得します。
        /// </summary>
        public IBalloonTipService BalloonTipService => _balloonTipService;

        /// <summary>
        /// アプリケーション制御サービスを取得します。
        /// </summary>
        public IApplicationService ApplicationService => _applicationService;

        #endregion
    }
}