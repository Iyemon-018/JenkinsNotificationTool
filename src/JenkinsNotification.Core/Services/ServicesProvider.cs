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

        #endregion

        #region Ctor

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="dialogService">ダイアログ表示サービス</param>
        /// <param name="viewService">画面表示サービス</param>
        /// <param name="balloonTipService">バルーン通知サービス</param>
        /// <exception cref="System.ArgumentNullException">
        /// <paramref name="dialogService"/> or <paramref name="viewService"/> or <paramref name="balloonTipService"/> がnull の場合にスローされます。
        /// </exception>
        public ServicesProvider(IDialogService dialogService, IViewService viewService, IBalloonTipService balloonTipService)
        {
            if (dialogService     == null) throw new ArgumentNullException(nameof(dialogService));
            if (viewService       == null) throw new ArgumentNullException(nameof(viewService));
            if (balloonTipService == null) throw new ArgumentNullException(nameof(balloonTipService));

            LogManager.Info($"ダイアログ表示サービスを設定します。{dialogService.GetType()}");
            LogManager.Info($"画面表示サービスを設定します。{viewService.GetType()}");
            LogManager.Info($"バルーン通知サービスを設定します。{balloonTipService.GetType()}");

            _dialogService     = dialogService;
            _viewService       = viewService;
            _balloonTipService = balloonTipService;
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

        #endregion
    }
}