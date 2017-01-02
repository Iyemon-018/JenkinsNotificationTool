namespace JenkinsNotification.Core.Configurations
{
    using System;
    using System.Runtime.Serialization;

    /// <summary>
    /// 構成ファイルの読み込みに失敗した場合にスローされる例外クラスです。
    /// </summary>
    /// <seealso cref="System.Exception" />
    [Serializable]
    public class ConfigurationLoadException : Exception
    {
        #region Ctor

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="filePath">構成ファイルパス</param>
        public ConfigurationLoadException(string filePath)
        {
            FilePath = filePath;
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="message">例外メッセージ</param>
        /// <param name="filePath">構成ファイルパス</param>
        public ConfigurationLoadException(string message, string filePath) : base(message)
        {
            FilePath = filePath;
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="message">例外メッセージ</param>
        /// <param name="filePath">構成ファイルパス</param>
        /// <param name="innerException">現在の例外の原因である例外。内部例外が指定されていない場合は null 参照</param>
        public ConfigurationLoadException(string message, string filePath, Exception innerException) : base(message, innerException)
        {
            FilePath = filePath;
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="info">スローされた例外に関する、シリアル化されたオブジェクト データを保持する <see cref="T:System.Runtime.Serialization.SerializationInfo" /> です。</param>
        /// <param name="context">転送元または転送先についてのコンテキスト情報を含む <see cref="T:System.Runtime.Serialization.StreamingContext" /> です。</param>
        protected ConfigurationLoadException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        #endregion

        #region Properties

        /// <summary>
        /// 構成ファイルパスを取得します。
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

            info.AddValue(nameof(FilePath), FilePath);
        }

        #endregion
    }
}