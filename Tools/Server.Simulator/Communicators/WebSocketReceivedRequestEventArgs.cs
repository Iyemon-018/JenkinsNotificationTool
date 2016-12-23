namespace Server.Simulator.Communicators
{
    using System.Net;

    /// <summary>
    /// WebSocket �ʐM�ɂ��N���C�A���g����̃��N�G�X�g����M�������̃C�x���g�����N���X�ł��B
    /// </summary>
    /// <seealso cref="Server.Simulator.Communicators.WebSocketClientEventArgs" />
    public class WebSocketReceivedRequestEventArgs : WebSocketClientEventArgs
    {
        #region Fields

        /// <summary>
        /// ��M�f�[�^
        /// </summary>
        private readonly byte[] _receivedData;

        #endregion

        #region Ctor

        /// <summary>
        /// �R���X�g���N�^
        /// </summary>
        /// <param name="clientEndPoint">�N���C�A���g�̃G���h�|�C���g</param>
        /// <param name="receivedData">��M�f�[�^</param>
        public WebSocketReceivedRequestEventArgs(IPEndPoint clientEndPoint, byte[] receivedData) : base(clientEndPoint)
        {
            _receivedData = receivedData;
        }

        #endregion

        #region Methods

        /// <summary>
        /// ��M�f�[�^���擾���܂��B
        /// </summary>
        /// <returns>��M�f�[�^</returns>
        public byte[] GetReceivedData()
        {
            return (byte[]) _receivedData.Clone();
        }

        #endregion
    }
}