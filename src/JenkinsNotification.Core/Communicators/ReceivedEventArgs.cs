namespace JenkinsNotification.Core.Communicators
{
    using System;

    /// <summary>
    /// WebSocket 通信でサーバーからメッセージ、データを受信した際のイベント引数クラスです。
    /// </summary>
    /// <seealso cref="System.EventArgs" />
    public class ReceivedEventArgs : EventArgs
    {
        #region Fields

        /// <summary>
        /// 受信データ
        /// </summary>
        private readonly byte[] _data;

        #endregion

        #region Ctor

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="message">受信メッセージ</param>
        public ReceivedEventArgs(string message)
            : this(ReceivedType.Message, message)
        {
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="data">受信データ</param>
        public ReceivedEventArgs(byte[] data)
            : this(ReceivedType.Binary, data)
        {
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="receivedType">受信データ種別</param>
        /// <param name="message">受信メッセージ</param>
        protected ReceivedEventArgs(ReceivedType receivedType, string message)
        {
            ReceivedType = receivedType;
            Message = message;
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="receivedType">受信データ種別</param>
        /// <param name="data">受信データ</param>
        protected ReceivedEventArgs(ReceivedType receivedType, byte[] data)
        {
            ReceivedType = receivedType;
            _data = data;
        }

        #endregion

        #region Properties

        /// <summary>
        /// 受信データ種別を取得します。
        /// </summary>
        public ReceivedType ReceivedType { get; private set; }

        /// <summary>
        /// 受信メッセージを取得します。
        /// </summary>
        public string Message { get; private set; }

        #endregion

        #region Methods

        /// <summary>
        /// 受信データを取得します。
        /// </summary>
        /// <returns>受信データ</returns>
        public byte[] GetData()
        {
            return (byte[])_data.Clone();
        }

        #endregion
    }
}