﻿namespace JenkinsNotification.Core.Services
{
    /// <summary>
    /// 各種サービスを提供するインターフェースです。
    /// </summary>
    public interface IServicesProvider
    {
        /// <summary>
        /// ダイアログ表示サービスを取得します。
        /// </summary>
        IDialogService DialogService { get; }

        /// <summary>
        /// 画面表示サービスを取得します。
        /// </summary>
        IViewService ViewService { get; }

        /// <summary>
        /// バルーン通知サービスを取得します。
        /// </summary>
        IBalloonTipService BalloonTipService { get; }

        /// <summary>
        /// アプリケーション制御サービスを取得します。
        /// </summary>
        IApplicationService ApplicationService { get; }
    }
}