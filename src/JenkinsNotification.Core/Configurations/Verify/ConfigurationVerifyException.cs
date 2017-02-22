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
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public ConfigurationVerifyException()
        {
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="message">エラーを説明するメッセージ。</param>
        public ConfigurationVerifyException(string message) : base(message)
        {
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="message">例外の原因を説明するエラー メッセージ。</param>
        /// <param name="innerException">現在の例外の原因である例外。内部例外が指定されていない場合は null 参照 (Visual Basic では、Nothing)。</param>
        public ConfigurationVerifyException(string message, Exception innerException) : base(message, innerException)
        {
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="info">スローされた例外に関する、シリアル化されたオブジェクト データを保持する <see cref="T:System.Runtime.Serialization.SerializationInfo" /> です。</param>
        /// <param name="context">転送元または転送先についてのコンテキスト情報を含む <see cref="T:System.Runtime.Serialization.StreamingContext" /> です。</param>
        protected ConfigurationVerifyException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}