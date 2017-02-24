namespace JenkinsNotification.Core.Configurations
{
    using System;
    using System.IO;
    using System.Windows.Controls.Primitives;
    using System.Xml.Serialization;
    using Extensions;
    using Logs;
    using Properties;
    using Verify;
    using Utility;

    /// <summary>
    /// このアプリケーションの構成ファイル情報クラスです。
    /// </summary>
    [XmlRoot]
    [Serializable]
    public partial class ApplicationConfiguration
    {
        #region Const

        /// <summary>
        /// 現在の構成情報
        /// </summary>
        private static ApplicationConfiguration _current;

        #endregion

        #region Ctor

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public ApplicationConfiguration()
        {
            //
            // ここにはファイル読み込みできなかったときのために各構成情報のデフォルト値を設定します。
            //
            NotifyConfiguration = new NotifyConfiguration
                                  {
                                      TargetUri           = "ws://mc-tfserver:8081/jenkins",
                                      PopupAnimationType  = PopupAnimation.Slide,
                                      PopupTimeout        = TimeSpan.FromSeconds(15),
                                      DisplayHistoryCount = 100,
                                      IsNotifySuccess     = false
                                  };
            DisplayConfiguration = new DisplayConfiguration();
        }

        /// <summary>
        /// このオブジェクトの文字列へ変換します。
        /// </summary>
        /// <returns>A <see cref="System.String" /> that represents this instance.</returns>
        public override string ToString()
        {
            return $"{nameof(DisplayConfiguration)}: {DisplayConfiguration}" +
                   $", {nameof(NotifyConfiguration)}: {NotifyConfiguration}";
        }

        #endregion

        #region Properties

        /// <summary>
        /// 現在の構成情報を取得します。
        /// </summary>
        public static ApplicationConfiguration Current
        {
            get { return _current; }
            internal set { _current = value; }
        }

        /// <summary>
        /// この構成ファイルのデフォルト パスを取得します。
        /// </summary>
        public static string DefaultFilePath => Path.Combine(PathUtility.AppTempPath, "ApplicationConfiguration.xml");

        #endregion

        #region Methods

        /// <summary>
        /// デフォルト パスのファイルを現在の構成情報に読み込みます。
        /// </summary>
        /// <param name="filePath">
        /// 構成情報ファイルパス<para/>
        /// null の場合、<see cref="DefaultFilePath"/> を読み込みます。
        /// </param>
        /// <returns>読み込みに成功した場合はtrue, 失敗した場合はfalse を返します。</returns>
        public static void LoadCurrent(string filePath = null)
        {
            using (TimeTracer.StartNew("デフォルト　ファイルパスでアプリケーション構成ファイルを読み込む。"))
            {
                if (filePath.IsEmpty())
                {
                    filePath = DefaultFilePath;
                }

                try
                {
                    _current = ConfigurationUtility.Load(filePath, new ApplicationConfigurationVerify());
                }
                catch (Exception exception)
                {
                    _current = new ApplicationConfiguration();
                    throw new ConfigurationLoadException(Resources.ConfigurationLoadFailedMessage,
                                                         DefaultFilePath,
                                                         exception);
                }
            }
        }

        /// <summary>
        /// 現在の構成情報をデフォルト パスに保存します。
        /// </summary>
        /// <param name="filePath">
        /// 構成情報ファイルパス<para/>
        /// null の場合、<see cref="DefaultFilePath"/> に保存します。
        /// </param>
        /// <returns>保存に成功した場合はtrue, 失敗した場合はfalse を返します。</returns>
        public static bool SaveCurrent(string filePath = null)
        {
            if (filePath.IsEmpty())
            {
                filePath = DefaultFilePath;
            }

            try
            {
                ConfigurationUtility.Save(_current, filePath, new ApplicationConfigurationVerify());
            }
            catch(Exception exception)
            {
                LogManager.Error("構成情報ファイルの保存に失敗した。", exception);
                return false;
            }
            return true;
        }

        #endregion
    }
}