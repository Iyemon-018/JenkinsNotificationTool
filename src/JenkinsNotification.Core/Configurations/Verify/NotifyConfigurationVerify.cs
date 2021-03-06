﻿namespace JenkinsNotification.Core.Configurations.Verify
{
    using System;
    using JenkinsNotification.Core.Extensions;
    using JenkinsNotification.Core.Utility;
    using Properties;

    /// <summary>
    /// 構成情報<see cref="NotifyConfiguration"/> の検証ロジック クラスです。
    /// </summary>
    /// <seealso cref="Configurations.Verify.IConfigurationVerify{NotifyConfiguration}" />
    public class NotifyConfigurationVerify : IConfigurationVerify<NotifyConfiguration>
    {
        #region Const

        /// <summary>
        /// <see cref="NotifyConfiguration.DisplayHistoryCount"/> に設定可能な最大値
        /// </summary>
        public static readonly int DisplayHistoryMaximum = 200;

        /// <summary>
        /// <see cref="NotifyConfiguration.DisplayHistoryCount"/> に設定可能な最小値
        /// </summary>
        public static readonly int DisplayHistoryMinimum = 25;

        #endregion

        #region Methods

        /// <summary>
        /// 構成情報の検証を行います。
        /// </summary>
        /// <param name="config">構成情報オブジェクト</param>
        /// <returns>検証結果</returns>
        /// <exception cref="System.ArgumentNullException"><paramref name="config"/> がnull の場合にスローされます。</exception>
        public void Verify(NotifyConfiguration config)
        {
            if (config == null) throw new ArgumentNullException(nameof(config));

            using (TimeTracer.StartNew("通知関連の構成情報を検証する。"))
            {
                //
                // 受信履歴の最大数の検証を行う。
                //
                var displayHistoryCount = config.DisplayHistoryCount;
                if (displayHistoryCount < DisplayHistoryMinimum || DisplayHistoryMaximum < config.DisplayHistoryCount)
                {
                    throw new ConfigurationVerifyException(string.Format(Resources.DisplayHistoryCountOutOfRangeMessage,
                                                            DisplayHistoryMinimum,
                                                            DisplayHistoryMaximum));
                }

                var popupTimeoutValue = config.PopupTimeoutValue;
                // null は許容する。
                if (popupTimeoutValue.HasText() && !popupTimeoutValue.IsTimeSpanValue())
                {
                    // TODO リソースに定義する。
                    throw new ConfigurationVerifyException("PopupTimeout の値が異常です。設定値を確認してください。");
                }

                // 全ての検証が正常に終了した。
            }
        }

        #endregion
    }
}