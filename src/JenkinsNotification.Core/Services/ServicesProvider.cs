﻿namespace JenkinsNotification.Core.Services
{
    using System;
    using JenkinsNotification.Core.Communicators;
    using JenkinsNotification.Core.Logs;

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

        // TODO これをコンストラクタで設定できるようにする。
        private readonly IWebSocketCommunicator _webSocketCommunicator;

        #endregion

        #region Ctor

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="dialogService">ダイアログ表示サービス</param>
        /// <param name="viewService">画面表示サービス</param>
        public ServicesProvider(IDialogService dialogService, IViewService viewService)
        {
            if (dialogService == null) throw new ArgumentNullException(nameof(dialogService));
            if (viewService == null) throw new ArgumentNullException(nameof(viewService));
            
            LogManager.Info($"ダイアログ表示サービスを設定します。{dialogService.GetType()}");
            LogManager.Info($"画面表示サービスを設定します。{viewService.GetType()}");

            _dialogService = dialogService;
            _viewService   = viewService;
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

        public IWebSocketCommunicator _WebSocketCommunicator => _webSocketCommunicator;

        #endregion
    }
}