namespace JenkinsNotification.Core.Configurations.Verify
{
    using System;
    using System.Runtime.Serialization;

    /// <summary>
    /// 構成情報の検証結果が異常だった場合にスロされる例外クラスです。
    /// </summary>
    /// <seealso cref="System.Exception" />
    [Serializable]
    public class ConfigurationVerifyException : Exception
    {
        #region Ctor

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="message">メッセージ</param>
        /// <param name="filePath">検証を行った構成ファイルのパス</param>
        /// <param name="result">検証結果</param>
        /// <exception cref="System.ArgumentNullException"><paramref name="result"/> がnull の場合にスローされます。</exception>
        public ConfigurationVerifyException(string message, string filePath, VerifyResult result)
            : base(message + $"Path {filePath}" + Environment.NewLine + result)
        {
            if (result == null) throw new ArgumentNullException(nameof(result));
            Result   = result;
            FilePath = filePath;
        }

        #endregion

        #region Properties

        /// <summary>
        /// 検証結果を取得します。
        /// </summary>
        public VerifyResult Result { get; private set; }

        /// <summary>
        /// 検証を行った構成ファイルのパスを取得します。
        /// </summary>
        public string FilePath { get; private set; }

        #endregion

        #region Methods

        /// <summary>
        /// 派生クラスでオーバーライドされた場合は、その例外に関する情報を使用して <see cref="T:System.Runtime.Serialization.SerializationInfo" /> を設定します。
        /// </summary>
        /// <param name="info">スローされた例外に関する、シリアル化されたオブジェクト データを保持する <see cref="T:System.Runtime.Serialization.SerializationInfo" /> です。</param>
        /// <param name="context">転送元または転送先についてのコンテキスト情報を含む <see cref="T:System.Runtime.Serialization.StreamingContext" /> です。</param>
        /// <PermissionSet>
        ///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Read="*AllFiles*" PathDiscovery="*AllFiles*" />
        ///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="SerializationFormatter" />
        /// </PermissionSet>
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);

            info.AddValue(nameof(Result), Result);
            info.AddValue(nameof(FilePath), FilePath);
        }

        #endregion
    }
}