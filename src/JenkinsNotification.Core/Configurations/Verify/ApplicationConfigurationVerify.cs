namespace JenkinsNotification.Core.Configurations.Verify
{
    using System;
    using JenkinsNotification.Core.Utility;

    /// <summary>
    /// 構成情報<see cref="ApplicationConfiguration"/> の検証ロジッククラスです。
    /// </summary>
    /// <seealso cref="Configurations.Verify.IConfigurationVerify{ApplicationConfiguration}" />
    public class ApplicationConfigurationVerify : IConfigurationVerify<ApplicationConfiguration>
    {
        #region Methods

        /// <summary>
        /// 構成情報の検証を行います。
        /// </summary>
        /// <param name="config">構成情報オブジェクト</param>
        /// <exception cref="System.ArgumentNullException"><paramref name="config"/> がnull の場合にスローされます。</exception>
        public void Verify(ApplicationConfiguration config)
        {
            if (config == null) throw new ArgumentNullException(nameof(config));

            using (TimeTracer.StartNew("アプリケーション構成情報の検証を実行する。"))
            {
                //
                // 通知関連の構成情報を検証する。
                //
                var notifyConfigVerify = new NotifyConfigurationVerify();
                notifyConfigVerify.Verify(config.NotifyConfiguration);

                // TODO 他の設定ファイルの検証も実装する。
            }
        }

        #endregion
    }
}