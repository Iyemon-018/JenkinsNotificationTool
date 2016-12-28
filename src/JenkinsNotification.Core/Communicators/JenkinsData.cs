namespace JenkinsNotification.Core.Communicators
{
    public class JenkinsData
    {

        /// <summary>
        /// 受信データ
        /// </summary>
        private readonly byte[] _data;

        public JenkinsData(ReceivedType receivedType, string message, byte[] data)
        {
            ReceivedType = receivedType;
            Message = message;
            _data = data;
        }

        /// <summary>
        /// 受信データ種別を取得します。
        /// </summary>
        public ReceivedType ReceivedType { get; private set; }

        /// <summary>
        /// 受信メッセージを取得します。
        /// </summary>
        public string Message { get; private set; }

        /// <summary>
        /// 受信データを取得します。
        /// </summary>
        /// <returns>受信データ</returns>
        public byte[] GetData()
        {
            return (byte[]) _data.Clone();
        }

    }
}